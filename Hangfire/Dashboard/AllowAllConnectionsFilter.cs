namespace Hangfire.Dashboard
{
    public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize (DashboardContext context)
        {
            return true;
        }
    }
}