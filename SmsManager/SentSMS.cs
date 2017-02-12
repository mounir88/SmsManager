using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsManager
{
    public class SentSMS
    {
        [Route("/sms/sent")]
        public class SentSMSRequest : IReturn<SentSMSResponse>
        {
            [PrimaryKey]
            [AutoIncrement]
            public DateTime DateTimeFrom { get; set; }
            public DateTime DateTimeTo { get; set; }
            public int Skip { get; set; }
            public int Take { get; set; }
        }

        public class SentSMSResponse
        {
            public int TotalCount { get; set; }
            public Items[] Item { get; set; }
        }
    }

    public class Items
    {
        public DateTime DateTime { get; set; }
        public int Mcc { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public string State { get; set; }
    }
}