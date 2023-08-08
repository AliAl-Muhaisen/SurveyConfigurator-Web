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
            try
            {
                 AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
          
            
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
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageController.LANGAUGE_CULTURE_NAME_ENGLISH);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageController.LANGAUGE_CULTURE_NAME_ENGLISH);
                }

              
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                //get the last error was thrown
                Exception tException = Server.GetLastError();

                if (tException is HttpException)
                {
                    HttpException tHttpException = (HttpException)tException;
                    int tHttpCode = tHttpException.GetHttpCode();

                    switch (tHttpCode)
                    {
                        case 404:
                            Response.Redirect(Routes.ERROR_NOTFOUND);
                            break;
                        default:
                            Response.Redirect(Routes.ERROR);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Response.Redirect(Routes.ERROR);
            }
           
        }
    }
}
