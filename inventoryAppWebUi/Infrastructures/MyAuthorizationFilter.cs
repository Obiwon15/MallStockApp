using Hangfire.Dashboard;
using Microsoft.Owin;

namespace inventoryAppWebUi.Infrastructures
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            return owinContext.Authentication.User.Identity.IsAuthenticated;
        }
    }
}