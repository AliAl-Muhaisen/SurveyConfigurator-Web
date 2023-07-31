using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Models.Questions
{
    public class QuestionFacesController : Controller
    {
        readonly QuestionModel questionModel;
        public QuestionFacesController()
        {
            try
            {
                questionModel = new QuestionModel();
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
        [Route("Question/Faces")]
        public ActionResult Detail(int id)
        {
            QuestionFaces tQuestionFaces = new QuestionFaces();
            tQuestionFaces.SetId(id);
            questionModel.questionManager.GetQuestionFaces(ref tQuestionFaces);


            return View(tQuestionFaces);
        }
    }
}