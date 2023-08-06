using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class HomeController : Controller
    {
        readonly HomeModel homeModel;
       public HomeController()
        {
            try
            {
                homeModel = new HomeModel();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        public ActionResult Index()
        {
            return View(homeModel);
        }

        public ActionResult SetCulture(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }

            return RedirectToAction(Routes.INDEX,Routes.QUESTION);
        }
    }
}