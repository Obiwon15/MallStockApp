using Hangfire;
using inventoryAppWebUi.Infrastructures;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(inventoryAppWebUi.Startup))]
namespace inventoryAppWebUi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireDashboard();
            ConfigureAuth(app);
        }
    }
}
