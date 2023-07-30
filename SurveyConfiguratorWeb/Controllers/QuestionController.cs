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
    public class QuestionController : Controller
    {
        readonly HomeModel homeModel;
        public QuestionController()
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
        // GET: Question
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Delete(int pID)
        {
            try
            {
                int result = homeModel.questionManager.Delete(pID);
                if (result == ResultCode.SUCCESS)
                {
                    return Json(new { success = true, Status = 200 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return Json(new { success = false, Status = 404 }, JsonRequestBehavior.AllowGet);

        }
    }
}