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
        public QuestionSliderController()
        {
            try
            {
                questionManager=new QuestionManager();
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

        [Route("Question/Detail/Slider")]
        public ActionResult Detail(int id)
        {
            QuestionSlider tQuestionSlider = new QuestionSlider();
            tQuestionSlider.SetId(id);
            questionManager.GetQuestionSlider(ref tQuestionSlider);


            return View(tQuestionSlider);
        }

        [HttpPost]
        [Route("Question/Create/Slider")]
        public ActionResult Create(QuestionSlider pQuestionSlider)
        {
            questionManager.AddQuestionSlider(pQuestionSlider);
            ValidationMessages.Validate(pQuestionSlider, ModelState);
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create", "Question");
            }
            return View("Create", "Question", pQuestionSlider);
        }

        [Route("Question/Edit/Slider")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            QuestionSlider tQuestionSlider = new QuestionSlider();
            tQuestionSlider.SetId(id);
            questionManager.GetQuestionSlider(ref tQuestionSlider);
            return View(tQuestionSlider);
        }

        [HttpPost]
        [Route("Question/Edit/Slider")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionSlider pQuestionSlider)
        {
 
          
            int result = questionManager.UpdateQuestionSlider(pQuestionSlider);
            if (result != ResultCode.SUCCESS)
            { 
                ValidationMessages.SliderValidation(ref pQuestionSlider, ModelState, questionManager.ValidationErrorList);
                return View(pQuestionSlider);
                
            }
                return RedirectToAction("Index", "Home");

           
        }
    }
}