using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers.Db
{
    public class DbConnectionController : Controller
    {
        // GET: DbConnection
      
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                DbManager dbManager = new DbManager();
              
                return View(dbManager);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
        }

        [HttpPost]
        public ActionResult Create(DbManager pDbManager)
        {
            try
            {
                Log.Info("server " + pDbManager.Server);
                pDbManager.SaveConnection();
                return View(pDbManager);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
        }
    }
}