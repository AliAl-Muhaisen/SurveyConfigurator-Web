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
        public ActionResult Create(QuestionFaces pQuestionFaces)
        {
           int r= questionManager.AddQuestionFaces(pQuestionFaces);
            string err = "";
            for (int i = 0; i < questionManager.ValidationErrorList.Count; i++)
            {
                err = questionManager.ValidationErrorList[i].ToString()+"\n";
            }
            Log.Info("result " + err);

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
    }
}