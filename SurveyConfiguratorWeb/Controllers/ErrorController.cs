using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorWeb.Languages;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        ErrorModel errorModel;

        public ErrorController()
        {
            try
            {
                errorModel = new ErrorModel();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        public ActionResult Index()
        {
            try
            {
                Response.StatusCode = 500;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return View(Routes.ERROR);
        }
        public ActionResult NotFound()
        {
            try
            {
                Response.StatusCode = 404;
                errorModel.Message = Language.ERROR_NOT_FOUND_MESSAGE;
                return View(Routes.CUSTOM_ERROR, errorModel);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }

        }

      
    }
}