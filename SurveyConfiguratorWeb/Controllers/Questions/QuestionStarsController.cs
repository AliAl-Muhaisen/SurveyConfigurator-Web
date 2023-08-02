﻿using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers.Questions
{
    public class QuestionStarsController : Controller
    {
        public readonly QuestionManager questionManager;

        public QuestionStarsController()
        {
            try
            {
                questionManager = new QuestionManager();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        // GET: QuestionStars
        public ActionResult Index()
        {
            return View();
        }

        [Route(Routes.QUESTION_STARS_DETAIL)]
        public ActionResult Detail(int id)
        {
            QuestionStars tQuestionStars = new QuestionStars();
            tQuestionStars.SetId(id);
            questionManager.GetQuestionStars(ref tQuestionStars);


            return View(tQuestionStars);
        }

        [HttpPost]
        [Route(Routes.QUESTION_STARS_CRAETE)]
        public ActionResult Create(QuestionStars pQuestionStars)
        {
           questionManager.AddQuestionStars(pQuestionStars);
           
            return RedirectToAction("Create", "Question");
        }
        [Route(Routes.QUESTION_STARS_EDIT)]

        [HttpGet]
        public ActionResult Edit(int id)
        {
            QuestionStars tQuestionStars = new QuestionStars();
            tQuestionStars.SetId(id);
            questionManager.GetQuestionStars(ref tQuestionStars);
            return View(tQuestionStars);
        }

        [Route(Routes.QUESTION_STARS_EDIT)]
        [HttpPost]
        public ActionResult Edit(QuestionStars tQuestionStars)
        {

            int result = questionManager.UpdateQuestionStars(tQuestionStars);
            if (result != ResultCode.SUCCESS)
            {
                ValidationMessages.StarsValidation(ref tQuestionStars, ModelState, questionManager.ValidationErrorList);
                return View(tQuestionStars);

            }
            return RedirectToAction("Index", "Home");
        }
    }
}