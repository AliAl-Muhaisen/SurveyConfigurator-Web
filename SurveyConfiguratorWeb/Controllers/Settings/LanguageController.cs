using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers.Settings
{
    public class LanguageController : Controller
    {
        // GET: Language
        public const string  LANGAUGE_NAME_COOKIE= "lang";
        public ActionResult Index(string lang)
        {
            try
            {
                if (!string.IsNullOrEmpty(lang))
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                    HttpCookie tHttpCookie = new HttpCookie(LANGAUGE_NAME_COOKIE);
                    tHttpCookie.Value = lang;
                    Response.Cookies.Add(tHttpCookie);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
           return Redirect(Request.UrlReferrer.ToString());
        }
    }
}