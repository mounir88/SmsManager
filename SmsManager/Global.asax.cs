using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Funq;
using static SmsManager.SendSmsService;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace SmsManager
{
    public class Global : System.Web.HttpApplication
    {
        public class SmsManagerAppHost : AppHostBase
        {
            public SmsManagerAppHost() : base("Sms Manager", typeof(SendSmsService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                var dbConnectionFactory = new OrmLiteConnectionFactory(HttpContext.Current.Server.MapPath("~/App_Data/data.txt"), 
                    SqliteDialect.Provider);
                container.Register<IDbConnectionFactory>(dbConnectionFactory);
                container.RegisterAutoWired<TrackedRepository>();
            }
        } 
        protected void Application_Start(object sender, EventArgs e)
        {
            new SmsManagerAppHost().Init();
        }

        
    }
}