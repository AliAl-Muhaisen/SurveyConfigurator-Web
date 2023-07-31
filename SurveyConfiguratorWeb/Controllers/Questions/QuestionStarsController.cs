using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
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
        readonly QuestionModel questionModel;

        public QuestionStarsController()
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
        // GET: QuestionStars
        public ActionResult Index()
        {
            return View();
        }

        [Route("Question/Stars")]
        public ActionResult Detail(int id)
        {
            QuestionStars tQuestionStars = new QuestionStars();
            tQuestionStars.SetId(id);
            questionModel.questionManager.GetQuestionStars(ref tQuestionStars);


            return View(tQuestionStars);
        }
    }
}