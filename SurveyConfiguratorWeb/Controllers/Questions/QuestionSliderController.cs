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
    public class QuestionSliderController : Controller
    {
        private QuestionModel questionModel;
        public QuestionSliderController()
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

        // GET: QuestionSlider
        public ActionResult Index()
        {
            return View();
        }

        [Route("Question/Slider")]
        public ActionResult Detail(int id)
        {
            QuestionSlider tQuestionSlider = new QuestionSlider();
            tQuestionSlider.SetId(id);
            questionModel.questionManager.GetQuestionSlider(ref tQuestionSlider);


            return View(tQuestionSlider);
        }
    }
}