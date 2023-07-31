using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
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

    }
}