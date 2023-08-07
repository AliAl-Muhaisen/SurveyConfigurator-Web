using Microsoft.AspNet.SignalR;
using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Hubs;
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
        HttpResponseCustom httpResponseCustom;
        public DbConnectionController()
        {
            try
            {
                httpResponseCustom = new HttpResponseCustom();
            }
            catch (Exception e)
            {
                Log.Error(e) ;
            }
        }

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
        public JsonResult Create(FormCollection pFormCollection)
        {
            try
            {
               
                DbManager tDbManager = FormToObj.DbManager(pFormCollection);
               
               bool tResult= tDbManager.SaveConnection();
                httpResponseCustom = HttpResponseCustom.Build(tResult);
                RefreshUI(tDbManager.IsConnect());
                return Json(httpResponseCustom);
            }
            catch (Exception e)
            {
                Log.Error(e);
                httpResponseCustom = HttpResponseCustom.BuildError();
                return Json(httpResponseCustom);

            }
        }
   
    [HttpPost]
    public JsonResult TestConnection(FormCollection pFormCollection)
        {
            httpResponseCustom = HttpResponseCustom.BuildError();
            try
            {
                DbManager tDbManager = FormToObj.DbManager(pFormCollection);
                bool isConnect = tDbManager.IsConnect();
                if (tDbManager.IsConnect())
                {
                   
                    return this.Create(pFormCollection);
                } 
                RefreshUI(isConnect);
                return Json(httpResponseCustom);

            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(httpResponseCustom);
            }
        }

        public void RefreshUI(bool pIsConnected)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<DbHub>();
                hubContext.Clients.All.UpdateConfigClient(pIsConnected);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        [Route(Routes.DB_CONNECTION_CHECK_CONNECTION)]
        [HttpGet]
        public JsonResult CheckConnection()
        {
            try
            {
                if (DbManager.IsDbConnected()!=ResultCode.SUCCESS)
                {
                httpResponseCustom = HttpResponseCustom.BuildError();
                }
                else
                {
                    httpResponseCustom = HttpResponseCustom.BuildSuccess();
                }
               
            }
            catch (Exception ex)
            {
                httpResponseCustom = HttpResponseCustom.BuildError();
                Log.Error(ex);
            }
            return Json(httpResponseCustom);
        }
    }

   
}