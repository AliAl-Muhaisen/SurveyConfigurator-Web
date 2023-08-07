using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Controllers.Settings;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SurveyConfiguratorWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        protected void Application_BeginRequest(object obj,EventArgs e)
        {
            try
            {
                HttpCookie tHttpCookie = HttpContext.Current.Request.Cookies[LanguageController.LANGAUGE_NAME_COOKIE];
                if (tHttpCookie !=null && tHttpCookie.Value!=null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(tHttpCookie.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(tHttpCookie.Value);
                  
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }

              
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
