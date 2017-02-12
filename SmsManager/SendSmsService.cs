using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SmsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsManager
{
    public class SendSmsService: Service
    {
        public TrackedRepository TrackedRepository { get; set; }
        public object ANY(SMS request)
        {
            //later do check here if sms was correctly sent
            var id = TrackedRepository.AddEntry(request);
            if (id == 0)
                return "failed";
            else
                return "success";
        }
    }

    public class TrackedRepository
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int AddEntry(SMS entry)
        {
            entry.EntryTime = DateTime.Now;

            //UrlDecode somehow adds spaces to the numbers instead of the +
            entry.From = HttpContext.Current.Server.UrlDecode(entry.From).Replace(" ", string.Empty);
            entry.To = HttpContext.Current.Server.UrlDecode(entry.To).Replace(" ", string.Empty);

            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                //get country id from receiver's country code 
                entry.CountryId = (db.Select<Country>(e => e.CC == Convert.ToInt16(entry.To.Substring(0, 2)))
                    .First().Id);

                //creates SMS table if it doesn't exist
                db.CreateTable<SMS>();
                db.Insert(entry);
                return (int)db.LastInsertId();
            }
        }
    }
}