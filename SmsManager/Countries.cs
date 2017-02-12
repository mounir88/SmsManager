using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsManager
{
    [Route("/countries", "GET")]
    public class Country : IReturn<CountryResponse>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MCC { get; set; }
        public int CC { get; set; }
        public double PricePerSms { get; set; }
    }

    public class CountryResponse
    {
        public Country[] ArrayCountries { get; set; }
    }
}