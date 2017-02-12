using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsManager
{
    public class Statistics
    {
        [Route("/statistics")]
        public class StatisticsRequest : IReturn<StatisticsResponse>
        {
            [PrimaryKey]
            [AutoIncrement]
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String MccList { get; set; }
        }

        public class StatisticsResponse
        {
            public StatisticRecord[] StatisticRecord { get; set; }
        }
    }

    public class StatisticRecord
    {
        public DateTime Day { get; set; }
        public int Mcc { get; set; }
        public Double PricePerSms { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
    }
}