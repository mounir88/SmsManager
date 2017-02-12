using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsManager
{
    [Route ("/sms/send")]
    public class SMS: IReturn<SMSResponse>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set;  }
        public DateTime EntryTime { get; set; }
        [References(typeof(Country))]
        public int CountryId { get; set; }
    }

    public class SMSResponse
    {
        public string state { get; set; }
    }

   
}