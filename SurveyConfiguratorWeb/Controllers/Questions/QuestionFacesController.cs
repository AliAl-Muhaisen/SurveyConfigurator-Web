using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Models.Questions
{
    public class QuestionFacesController : Controller
    {
        public readonly QuestionManager questionManager;
        ErrorModel errorModel;
        public QuestionFacesController()
        {
            try
            {
                questionManager = new QuestionManager();
                //errorModel.Title = Resource.ERROR;
                //errorModel.Message = Resource.QUESTION_NOT_FOUND;
                errorModel = new ErrorModel();
               
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        // GET: QuestionFaces
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


        

        [HttpGet]
        [Route(Routes.QUESTION_FACES_DETAIL)]
        public ActionResult Detail(int id)
        {
            try
            {
                QuestionFaces tQuestionFaces = new QuestionFaces();
                tQuestionFaces.SetId(id);
               int tResult= questionManager.GetQuestionFaces(ref tQuestionFaces);
                if (tResult != ResultCode.SUCCESS)
                {
                    return View(Routes.CUSTOM_ERROR, errorModel);

                }
                return View(tQuestionFaces);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
           
        }

        [Route(Routes.QUESTION_FACES_EDIT)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                QuestionFaces tQuestionFaces = new QuestionFaces();
                tQuestionFaces.SetId(id);
                questionManager.GetQuestionFaces(ref tQuestionFaces);
                return View(tQuestionFaces);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
           
            
        }
        [Route(Routes.QUESTION_FACES_EDIT)]
        [HttpPost]
        public ActionResult Edit(QuestionFaces pQuestionFaces)
        {
            try
            {
             int tResult = questionManager.UpdateQuestionFaces(pQuestionFaces);
            switch (tResult)
            {
                case ResultCode.SUCCESS:
                    return RedirectToAction(Routes.INDEX, Routes.QUESTION);
                case ResultCode.DB_RECORD_NOT_EXISTS:
                    return View(Routes.CUSTOM_ERROR, errorModel);

                case ResultCode.VALIDATION_ERROR:
                    ValidationMessages.FacesValidation(ref pQuestionFaces, ModelState, questionManager.ValidationErrorList);
                    return View(pQuestionFaces);
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