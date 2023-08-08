using SurveyConfiguratorApp.Helper;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            try
            {
                filters.Add(new HandleErrorAttribute());
            }
            catch (System.Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
