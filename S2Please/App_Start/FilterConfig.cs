using System.Web;
using System.Web.Mvc;
using S2Please.Filters;
namespace S2Please
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionExecuting());
        }
    }
}
