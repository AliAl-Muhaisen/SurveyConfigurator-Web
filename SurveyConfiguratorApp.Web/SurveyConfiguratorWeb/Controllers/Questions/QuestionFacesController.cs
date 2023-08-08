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
        #region Attributes
        public readonly QuestionManager questionManager;
        ErrorModel errorModel;
        #endregion

        #region Constructor
        public QuestionFacesController()
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
        /// Render the detail page of the Faces Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(Routes.QUESTION_FACES_DETAIL)]
        public ActionResult Detail(int id)
        {
            try
            {
                QuestionFaces tQuestionFaces = new QuestionFaces();
                tQuestionFaces.SetId(id);

                // Call the 'GetQuestionFaces' method of the 'questionManager' to retrieve the QuestionFaces data
                int tResult = questionManager.GetQuestionFaces(ref tQuestionFaces);

                // Check the result of the retrieval operation
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

        /// <summary>
        /// Render the Edit page of the question faces 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_FACES_EDIT)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                QuestionFaces tQuestionFaces = new QuestionFaces();
                tQuestionFaces.SetId(id);

                // Call the 'GetQuestionFaces' method of the 'questionManager' to retrieve the QuestionFaces data
                // The 'GetQuestionFaces' method should populate the 'tQuestionFaces' object with the data from the database.
                int tResult = questionManager.GetQuestionFaces(ref tQuestionFaces);

                // Check if the 'GetQuestionFaces' method was not successful (result is an error)
                if (tResult != ResultCode.SUCCESS)
                {
                    // If there was an error retrieving the data, return a custom error view
                    return View(Routes.CUSTOM_ERROR, errorModel);
                }

                // If the 'GetQuestionFaces' method was successful, return the 'tQuestionFaces' object to the view for rendering
                return View(tQuestionFaces);
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
        /// <param name="pQuestionFaces"></param>
        /// <returns></returns>
        [Route(Routes.QUESTION_FACES_EDIT)]
        [HttpPost]
        public ActionResult Edit(QuestionFaces pQuestionFaces)
        {
            try
            {
                int tResult = questionManager.UpdateQuestionFaces(pQuestionFaces);
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
                        ValidationMessages.FacesValidation(ref pQuestionFaces, ModelState, questionManager.ValidationErrorList);
                        return View(pQuestionFaces);
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