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
    public class QuestionSliderController : Controller
    {
        public readonly QuestionManager questionManager;
        ErrorModel errorModel;
        public QuestionSliderController()
        {
            try
            {
                questionManager=new QuestionManager();
                errorModel = new ErrorModel();
                errorModel.Title = "Error";
                errorModel.Message = "This Question does not exists";
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        // GET: QuestionSlider
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

        [Route(Routes.QUESTION_SLIDER_DETAIL)]
        public ActionResult Detail(int id)
        {
            try
            {
                QuestionSlider tQuestionSlider = new QuestionSlider();
                tQuestionSlider.SetId(id);
                questionManager.GetQuestionSlider(ref tQuestionSlider);
                return View(tQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
           
        }

     

        [Route(Routes.QUESTION_SLIDER_EDIT)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                     QuestionSlider tQuestionSlider = new QuestionSlider();
                    tQuestionSlider.SetId(id);
                    questionManager.GetQuestionSlider(ref tQuestionSlider);
                    return View(tQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
          
           
        }

        [HttpPost]
        [Route(Routes.QUESTION_SLIDER_EDIT)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionSlider pQuestionSlider)
        {
            try
            {
                int tResult = questionManager.UpdateQuestionSlider(pQuestionSlider);
                switch (tResult)
                {
                    case ResultCode.SUCCESS:
                        return RedirectToAction(Routes.INDEX, Routes.QUESTION);
                    case ResultCode.DB_RECORD_NOT_EXISTS:
                        return View(Routes.CUSTOM_ERROR, errorModel);

                    case ResultCode.VALIDATION_ERROR:
                        ValidationMessages.SliderValidation(ref pQuestionSlider, ModelState, questionManager.ValidationErrorList);
                        return View(pQuestionSlider);
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