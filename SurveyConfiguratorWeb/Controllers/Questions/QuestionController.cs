using Microsoft.AspNet.SignalR;
using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Controllers.Questions;
using SurveyConfiguratorWeb.Hubs;
using SurveyConfiguratorWeb.Models;
using SurveyConfiguratorWeb.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class QuestionController : Controller
    {
        public readonly QuestionManager questionManager;

        readonly HomeModel homeModel;
        public QuestionController()
        {
            try
            {
                homeModel = new HomeModel();
                questionManager = new QuestionManager();
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

        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }

        public ActionResult Detail(int id,string type)
        {

            Question.QuestionTypes questionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));

            switch (questionType)
            {
                case Question.QuestionTypes.FACES:
                    return RedirectToAction("Detail", "QuestionFaces", new { id = id });
                  

                case Question.QuestionTypes.SLIDER:
                    return RedirectToAction("Detail", "QuestionSlider", new { id = id });

                case Question.QuestionTypes.STARS:
                    return RedirectToAction("Detail", "QuestionStars", new { id = id });

                default:
                    break;
            }

            return View("Error");
        }

        [HttpGet]
        public ActionResult Edit(int id,string type)
        {
            try
            {
                Question.QuestionTypes questionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));
                switch (questionType)
                {
                    case Question.QuestionTypes.FACES:
                        return RedirectToAction("Edit", "QuestionFaces", new { id = id });


                    case Question.QuestionTypes.SLIDER:
                        return RedirectToAction("Edit", "QuestionSlider", new { id = id });

                    case Question.QuestionTypes.STARS:
                        return RedirectToAction("Edit", "QuestionStars", new { id = id });

                    default:
                        break;
                }


            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return View("Error");
        }
        public ActionResult CreateAdvance()
        {
            return View();
        }


        private void QuestionManager_DataChangedUI(object sender, EventArgs e)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            hubContext.Clients.All.NotifyClients();
        }


        //The Initialize method is a method of the Controller class in ASP.NET MVC. It is called automatically before any action method is executed for a controller
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var questionManager = new QuestionManager(); 
            questionManager.DataChangedUI += QuestionManager_DataChangedUI;
            questionManager.FollowDbChanges();
        }
    }
}