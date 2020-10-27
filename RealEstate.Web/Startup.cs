using Hangfire;
using Microsoft.Owin;
using Owin;
using RealEstate.Web.Areas.Admin.Controllers;
using RealEstate.Web.Controllers;
using RealEstate.Web.Models;
using System.Net.Http;

[assembly: OwinStartupAttribute(typeof(RealEstate.Web.Startup))]
namespace RealEstate.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("EFDBContext");

            ////app.UseHangfireDashboard("/JobScheduler");

            app.UseHangfireDashboard("/JobScheduler", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseHangfireDashboard();
            TaskSchedularController Obj = new TaskSchedularController();
            RecurringJob.AddOrUpdate(()=>Obj.SendEmail(),Cron.HourInterval(6));
            RecurringJob.AddOrUpdate(() => Pinger.Ping("https://www.justbere.com"), Cron.MinuteInterval(4));
            app.UseHangfireServer();
        }
        public static class Pinger
        {            
            static HttpClient client = new HttpClient();
            public static void Ping(string url)
            {
                client.GetAsync(url);
            }
        }


    }
}
