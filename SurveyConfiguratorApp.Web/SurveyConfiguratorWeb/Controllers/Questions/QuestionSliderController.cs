using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Languages;
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
        #region Attributes
        public readonly QuestionManager questionManager;
        ErrorModel errorModel;
        #endregion

        #region Constructor
        public QuestionSliderController()
        {
            try
            {
                questionManager = new QuestionManager();
                errorModel = new ErrorModel();

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        #endregion

        #region Actions & Methods


        /// <summary>
        /// Render the detail page of the Slider Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_SLIDER_DETAIL)]
        public ActionResult Detail(int id)
        {
            try
            {
                QuestionSlider tQuestionSlider = new QuestionSlider();
                tQuestionSlider.SetId(id);

                // Call the 'GetQuestionSlider' method of the 'questionManager' to retrieve the QuestionSlider data
                int tResult = questionManager.GetQuestionSlider(ref tQuestionSlider);

                // Check the result of the retrieval operation
                if (tResult != ResultCode.SUCCESS)
                {
                    return View(Routes.CUSTOM_ERROR, errorModel);
                }
                return View(tQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }

        }


        /// <summary>
        /// Render the Edit page of the Question SLIDER 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_SLIDER_EDIT)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                QuestionSlider tQuestionSlider = new QuestionSlider();
                tQuestionSlider.SetId(id);
                // Call the 'GetQuestionSlider' method of the 'questionManager' to retrieve the QuestionSlider data
                // The 'GetQuestionSlider' method should populate the 'tQuestionSlider' object with the data from the database.

                int tResult = questionManager.GetQuestionSlider(ref tQuestionSlider);
                if (tResult != ResultCode.SUCCESS)
                {
                    // If there was an error retrieving the data, return a custom error view
                    return View(Routes.CUSTOM_ERROR, errorModel);
                }

                // If the 'GetQuestionSlider' method was successful, return the 'tQuestionSlider' object to the view for rendering
                return View(tQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }


        }


        /// <summary>
        /// Handle the [Edit] question, when the user submit the form
        /// </summary>
        /// <param name="pQuestionSlider"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(Routes.QUESTION_SLIDER_EDIT)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionSlider pQuestionSlider)
        {
            try
            {
                int tResult = questionManager.UpdateQuestionSlider(pQuestionSlider);
                // Check the result of the update operation
                switch (tResult)
                {
                    case ResultCode.SUCCESS:
                        // If the update was successful, redirect to the index page of the Question controller
                        return RedirectToAction(Routes.INDEX, Routes.QUESTION);
                    case ResultCode.DB_RECORD_NOT_EXISTS:
                        // If the record to update does not exist, return a custom error view
                        return View(Routes.CUSTOM_ERROR, errorModel);

                    case ResultCode.VALIDATION_ERROR:
                        // If there are validation errors in the QuestionFaces object, call the 'FacesValidation' method to populate ModelState with validation errors
                        ValidationMessages.SliderValidation(ref pQuestionSlider, ModelState, questionManager.ValidationErrorList);
                        return View(pQuestionSlider);
                    default:
                        // For any other error, return the default error view
                        return View(Routes.ERROR);
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }

        }
        #endregion
    }
}