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

        #region Attributes
        HttpResponseCustom httpResponseCustom;
        #endregion
        #region Constructor
        public DbConnectionController()
        {
            try
            {
                httpResponseCustom = new HttpResponseCustom();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        #endregion

        #region Actions & Methods

        /// <summary>
        /// Render the create page 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// To save the new config data
        /// </summary>
        /// <param name="pFormCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(FormCollection pFormCollection)
        {
            try
            { 
                httpResponseCustom = HttpResponseCustom.BuildError();
                DbManager tDbManager = FormToObj.DbManager(pFormCollection);
                if (tDbManager == null)
                {
                    return Json(httpResponseCustom);
                }
                bool tResult = tDbManager.SaveConnection();
                httpResponseCustom = HttpResponseCustom.Build(tResult);
                return Json(httpResponseCustom);
            }
            catch (Exception e)
            {
                Log.Error(e);
               
                return Json(httpResponseCustom);

            }
        }



        /// <summary>
        /// To Test the connection before save it
        /// </summary>
        /// <param name="pFormCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TestConnection(FormCollection pFormCollection)
        {
            httpResponseCustom = HttpResponseCustom.BuildError();
            try
            {
                //to convert the pFormCollection to object
                DbManager tDbManager = FormToObj.DbManager(pFormCollection);
                if (tDbManager == null)
                {
                    return Json(httpResponseCustom);
                }
                bool isConnect = tDbManager.IsConnect();
                // if the data was provided connected successfully
                if (tDbManager.IsConnect())
                {
                    //save the data
                    return this.Create(pFormCollection);
                }
                //to notify the ui
                return Json(httpResponseCustom);

            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(httpResponseCustom);
            }
        }



        #endregion
    }


}