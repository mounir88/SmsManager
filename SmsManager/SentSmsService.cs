using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SmsManager.SentSMS;

namespace SmsManager
{
    public class SentSmsService: Service
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public object GET(SentSMSRequest request)
        {
            return new SentSMSResponse { TotalCount = GetTotalSmsCount(request), Item = GetSmsItems(request) };
        }

        private Items[] GetSmsItems(SentSMSRequest request)
        {
            List<Items> ls = new List<Items>();

            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                var q = db.From<SMS>().Join<Country>((x, y) => x.CountryId == y.Id)
                .Where<SMS>(e => e.EntryTime >= request.DateTimeFrom && e.EntryTime <= request.DateTimeTo)
                    .Take(request.Take).Skip(request.Skip);

                List<SMS> results = db.Select<SMS>(q);

                results.ForEach(e => {
                    Items item = new Items();
                    Country c = db.Select<Country>(x => x.Id == e.Id).First();
                    item.DateTime = e.EntryTime;
                    item.Mcc = c.MCC;
                    item.From = e.From;
                    item.To = e.To;
                    item.Price = c.PricePerSms;
                    //dummy implementation - assuming state is always success
                    item.State = "success";

                    ls.Add(item);
                });
            }

            return (ls.ToArray());
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