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
        #region Attributes

        public readonly QuestionManager questionManager;
        ErrorModel errorModel;
        #endregion

        #region Constructor
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
        #endregion

        #region Actions & Methods

        /// <summary>
        /// Render the detail page of the Stars Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_STARS_DETAIL)]
        public ActionResult Detail(int id)
        {
            try
            {
                 QuestionStars tQuestionStars = new QuestionStars();
                tQuestionStars.SetId(id);
                // Call the 'GetQuestionStars' method of the 'questionManager' to retrieve the QuestionStars data
                int tResult =questionManager.GetQuestionStars(ref tQuestionStars);

                // Check the result of the retrieval operation
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


        /// <summary>
        /// Render the Edit page of the question stars 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_STARS_EDIT)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                 QuestionStars tQuestionStars = new QuestionStars();
                tQuestionStars.SetId(id);

                int tResult = questionManager.GetQuestionStars(ref tQuestionStars);
                // Check if the 'GetQuestionStars' method was not successful (result is an error)
                if (tResult != ResultCode.SUCCESS)
                {
                    // If there was an error retrieving the data, return a custom error view
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


        /// <summary>
        ///  Handle the [Edit] question, when the user submit the form
        /// </summary>
        /// <param name="tQuestionStars"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_STARS_EDIT)]
        [HttpPost]
        public ActionResult Edit(QuestionStars tQuestionStars)
        {
            try
            {
                 int tResult = questionManager.UpdateQuestionStars(tQuestionStars);
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
                        ValidationMessages.StarsValidation(ref tQuestionStars, ModelState, questionManager.ValidationErrorList);
                        return View(tQuestionStars);
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