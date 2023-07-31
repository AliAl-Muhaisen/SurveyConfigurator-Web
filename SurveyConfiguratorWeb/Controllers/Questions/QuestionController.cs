﻿using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorWeb.Controllers.Questions;
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
        readonly QuestionModel questionModel;
        readonly HomeModel homeModel;
        public QuestionController()
        {
            try
            {
                homeModel = new HomeModel();
                questionModel = new QuestionModel();
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(QuestionFaces pQuestionModel)
        {

           
            Log.Info("FacesNumber "+pQuestionModel.FacesNumber.ToString());
            questionModel.questionManager.AddQuestionFaces(pQuestionModel);
            return View();
        }
        public ActionResult Detail(int id,string type)
        {

            Log.Info(" " + id + "    t " + type);
            Question.QuestionTypes questionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));

            switch (questionType)
            {
                case Question.QuestionTypes.FACES:
                    return RedirectToAction("Detail", "QuestionFaces", new { id = id });
                  

                case Question.QuestionTypes.SLIDER:
                    // Handle SLIDER type
                    break;

                case Question.QuestionTypes.STARS:
                    return RedirectToAction("Detail", "QuestionStars", new { id = id });
                   


                default:
                    // Handle the default case if the enum value doesn't match any of the cases above
                    break;
            }

            return View("Error");
        }
    }
}