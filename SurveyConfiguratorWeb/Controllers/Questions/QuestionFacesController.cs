using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
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
        public QuestionFacesController()
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
        // GET: QuestionFaces
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Route("Question/Faces/Create")]
        public ActionResult Create(QuestionFaces pQuestionFaces,FormCollection formCollection)
        {

           int r= questionManager.AddQuestionFaces(pQuestionFaces);
     

            return RedirectToAction("Create", "Question");
        }

        [HttpGet]
        [Route("Question/Detail/Faces")]
        public ActionResult Detail(int id)
        {
            QuestionFaces tQuestionFaces = new QuestionFaces();
            tQuestionFaces.SetId(id);
            questionManager.GetQuestionFaces(ref tQuestionFaces);


            return View(tQuestionFaces);
        }

        [Route("Question/Edit/Faces")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            QuestionFaces tQuestionFaces = new QuestionFaces();
            tQuestionFaces.SetId(id);
            questionManager.GetQuestionFaces(ref tQuestionFaces);
            return View(tQuestionFaces);
        }
        [Route("Question/Edit/Faces")]
        [HttpPost]
        public ActionResult Edit(QuestionFaces pQuestionFaces)
        {
            int result = questionManager.UpdateQuestionFaces(pQuestionFaces);
            if (result != ResultCode.SUCCESS)
            {
                ValidationMessages.FacesValidation(ref pQuestionFaces, ModelState, questionManager.ValidationErrorList);
                return View(pQuestionFaces);

            }
            return RedirectToAction("Index", "Home");
        }
    }
}