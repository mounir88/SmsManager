using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SmsManager.SentSMS;
using static SmsManager.Statistics;
using System.Data;

namespace SmsManager
{
    public class StatisticsService: Service
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public object GET(StatisticsRequest request)
        {
            return new StatisticsResponse { StatisticRecord = GetStatisticRecords(request) };
        }

        private StatisticRecord[] GetStatisticRecords(StatisticsRequest request)
        {

            List<Items> ls = new List<Items>();

            if (String.IsNullOrEmpty(request.MccList))
            //get all messages
            {
                return getAllRecords(request);
            }
            else
            {
                if (!request.MccList.Contains(','))
                {
                    return getThisMccRecord(request);
                }
                else
                {
                    List<int> lsMcc = request.MccList.Split(',').Select(int.Parse).ToList();

                    return getTheseMccRecords(request, lsMcc);
                }
            }          
        }

        private StatisticRecord[] getTheseMccRecords(StatisticsRequest request, List<int> lsMcc)
        {
            List<int> countryIDs = getCountryIDs(lsMcc);

            List<StatisticRecord> records = new List<StatisticRecord>();

            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                List<SMS> messages = db.Select<SMS>().Where(x => x.EntryTime >= request.DateFrom
                && x.EntryTime <= request.DateTo &&  countryIDs.Contains(x.CountryId)).ToList();

                foreach (SMS m in messages)
                {
                    records.Add(getRecord(m, db, messages));
                }
            }

            return (records.ToArray());
        }

        private StatisticRecord[] getThisMccRecord(StatisticsRequest request)
        {
            List<StatisticRecord> records = new List<StatisticRecord>();

            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                Country c = db.Select<Country>().Where(x => x.MCC == Convert.ToInt16(request.MccList)).First();
                List<SMS> messages = db.Select<SMS>().Where(x => x.EntryTime >= request.DateFrom
                && x.EntryTime <= request.DateTo && x.CountryId == c.Id).ToList();

                foreach (SMS m in messages)
                {
                    records.Add(getRecord(m, db, messages));
                }
            }

            return (records.ToArray());
        }

        private StatisticRecord[] getAllRecords(StatisticsRequest request)
        {
            List<StatisticRecord> records = new List<StatisticRecord>();

            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                List<SMS> messages = db.Select<SMS>().Where(x => x.EntryTime >= request.DateFrom
                && x.EntryTime <= request.DateTo).ToList();

                foreach (SMS m in messages)
                {                 
                    records.Add(getRecord(m, db, messages));
                }
            }

            return (records.ToArray());
        }

        private StatisticRecord getRecord(SMS m, IDbConnection db, List<SMS> messages)
        {
            Country c = db.Select<Country>().Where(x => x.Id == m.CountryId).First();
            StatisticRecord record = new StatisticRecord();
            record.Day = m.EntryTime;
            record.Mcc = c.MCC;
            record.PricePerSms = c.PricePerSms;
            record.Count = messages.Where(x => x.CountryId == c.Id).Count();
            record.TotalPrice = record.Count * record.PricePerSms;

            return (record);
        }

        private List<int> getCountryIDs(List<int> lsMcc)
        {
            List<int> IDs = new List<int>();
            foreach (int mcc in lsMcc)
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    IDs.Add(db.Select<Country>().Where(x => x.MCC == mcc).First().Id);
                }
            }

            return (IDs);
        }

        private int GetTotalSmsCount(SentSMSRequest request)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                return (db.Select<SMS>(e => e.EntryTime >= request.DateTimeFrom && e.EntryTime <= request.DateTimeTo)
                    .Take(request.Take).Skip(request.Skip).Count());
            }
        }
    }
}