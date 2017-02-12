using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsManager
{
    public class GetCountriesService : Service
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public object Any(Country request)
        {
            return new CountryResponse { ArrayCountries = GetCountries() };
        }

        public Country[] GetCountries()
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {              
                List<Country> lc = db.Select<Country>();

                return (lc.ToArray());
            }

        }
    }
}