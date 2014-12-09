using System.Web;
using System.Web.Mvc;

namespace rad301CA2s00132798
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}