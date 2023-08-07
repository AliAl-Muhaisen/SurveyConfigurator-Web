using SurveyConfiguratorApp.Domain;
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
        ErrorModel errorModel;

        public QuestionStarsController()
        {
            try
            {
                errorModel = new ErrorModel();
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
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
            
        }

        [Route(Routes.QUESTION_STARS_DETAIL)]
        public ActionResult Detail(int id)
        {
            try
            {
                 QuestionStars tQuestionStars = new QuestionStars();
                tQuestionStars.SetId(id);
                int tResult=questionManager.GetQuestionStars(ref tQuestionStars);
                if (tResult != ResultCode.SUCCESS)
                {
                    return View(Routes.CUSTOM_ERROR, errorModel);
                }

                return View(tQuestionStars);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
           
        }

        [HttpPost]
        [Route(Routes.QUESTION_STARS_CRAETE)]
        public ActionResult Create(QuestionStars pQuestionStars)
        {
            try
            {
            questionManager.AddQuestionStars(pQuestionStars);
           
            return RedirectToAction(Routes.CREATE,Routes.QUESTION);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
           
        }
        [Route(Routes.QUESTION_STARS_EDIT)]

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                 QuestionStars tQuestionStars = new QuestionStars();
                tQuestionStars.SetId(id);
                questionManager.GetQuestionStars(ref tQuestionStars);
                return View(tQuestionStars);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
         
        }

        [Route(Routes.QUESTION_STARS_EDIT)]
        [HttpPost]
        public ActionResult Edit(QuestionStars tQuestionStars)
        {
            try
            {
                 int tResult = questionManager.UpdateQuestionStars(tQuestionStars);
                switch (tResult)
                {
                    case ResultCode.SUCCESS:
                        return RedirectToAction(Routes.INDEX, Routes.QUESTION);
                    case ResultCode.DB_RECORD_NOT_EXISTS:
                        return View(Routes.CUSTOM_ERROR, errorModel);

                    case ResultCode.VALIDATION_ERROR:
                        ValidationMessages.StarsValidation(ref tQuestionStars, ModelState, questionManager.ValidationErrorList);
                        return View(tQuestionStars);
                    default:
                        return View(Routes.ERROR);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }

           

        }
    }
}