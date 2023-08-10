using Microsoft.AspNet.SignalR;
using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Hubs;
using SurveyConfiguratorWeb.Languages;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class QuestionController : Controller
    {
        #region Attributes
        public readonly QuestionManager questionManager;
        List<Question> questionList;
        ErrorModel errorModel;
        QuestionModel questionModel;

        HttpResponseCustom httpResponse;
        #endregion

        #region Constructor
        public QuestionController()
        {
            try
            {
                questionModel = new QuestionModel();
                questionManager = new QuestionManager();
                questionList = new List<Question>();
                int tResult = questionManager.GetQuestions(ref questionList);

                questionModel.QuestionList = questionList;

                QuestionManager.DataChangedUIWeb += RefreshUI;
                questionManager.AutoRefreshWeb(questionModel.QuestionList);

                errorModel = new ErrorModel();

                questionModel.IsDbConnected = DbManager.IsDbConnected();


            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        #endregion


        #region Actions & Methods
        // GET: Question
        /// <summary>
        /// Return the main page 
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            try
            {
                return View(questionModel);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
        }

        /// <summary>
        /// Delete specific question through AJAX 
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public JsonResult Delete(int pID)
        {
            try
            {
                int result = questionManager.Delete(pID);
                if (result == ResultCode.SUCCESS)
                {
                    httpResponse = HttpResponseCustom.BuildSuccess();
                    return Json(httpResponse);
                }
                else
                {
                    httpResponse = HttpResponseCustom.BuildError();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                httpResponse = HttpResponseCustom.BuildError();
            }
            return Json(httpResponse);

        }

        /// <summary>
        /// Render the create page 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
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

        /// <summary>
        /// handle the create question, when the user submit the form
        /// </summary>
        /// <param name="pFormCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FormCollection pFormCollection)
        {
            try
            {
                
                //get the question type
                string tTypeName = pFormCollection[FormToObj.TYPE_NAME];

                //convert string to QuestionTypes (enum)
                Question.QuestionTypes tQuestionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), tTypeName));
                int tResult = 0;


                QuestionFaces tQuestionFaces;
                QuestionStars tQuestionStars;
                QuestionSlider tQuestionSlider;
                switch (tQuestionType)
                {
                    case Question.QuestionTypes.FACES:

                        tQuestionFaces = FormToObj.QuestionFaces(pFormCollection);

                        if (tQuestionFaces == null)
                        {
                            goto ErrorLabel;
                        }
                        tResult = questionManager.AddQuestionFaces(tQuestionFaces);
                        //Check for validation error,if it is not VALIDATION_ERROR it will be Success or Error
                        if (tResult == ResultCode.VALIDATION_ERROR)
                        {
                            //To display the error messages 
                            ValidationMessages.FacesValidation(ref tQuestionFaces, ModelState, questionManager.ValidationErrorList);
                            //return the create page with the previous data
                            return View(tQuestionFaces);
                        }

                        break;
                    case Question.QuestionTypes.SLIDER:
                        tQuestionSlider = FormToObj.QuestionSlider(pFormCollection);
                        if (tQuestionSlider == null)
                        {
                            goto ErrorLabel;
                        }


                        tResult = questionManager.AddQuestionSlider(tQuestionSlider);
                        if (tResult == ResultCode.VALIDATION_ERROR)
                        {
                            ValidationMessages.SliderValidation(ref tQuestionSlider, ModelState, questionManager.ValidationErrorList);
                            return View(tQuestionSlider);
                        }

                        break;
                    case Question.QuestionTypes.STARS:
                        tQuestionStars = FormToObj.QuestionStars(pFormCollection);
                        if (tQuestionStars == null)
                        {
                            goto ErrorLabel;
                        }

                        tResult = questionManager.AddQuestionStars(tQuestionStars);
                        if (tResult == ResultCode.VALIDATION_ERROR)
                        {
                            ValidationMessages.StarsValidation(ref tQuestionStars, ModelState, questionManager.ValidationErrorList);
                            return View(tQuestionStars);
                        }
                        break;
                    default:
                        break;
                }

                if (tResult == ResultCode.SUCCESS)
                    return RedirectToAction(Routes.INDEX);

            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        ErrorLabel:
            {
                return View(Routes.ERROR);
            }



        }

        /// <summary>
        /// To display the details of the question with the provided id and type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Detail(int id, string type)
        {
            try
            {
                //convert string to QuestionTypes (enum)
                Question.QuestionTypes tQuestionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));
                int tResult = questionManager.IsQuestionExists(id);

                errorModel.Message = Language.DB_RECORD_NOT_EXISTS;
                if (tResult != ResultCode.SUCCESS)
                {
                    return View(Routes.CUSTOM_ERROR, errorModel);

                }
                switch (tQuestionType)
                {
                    case Question.QuestionTypes.FACES:
                        return RedirectToAction(Routes.DETAIL, Routes.QUESTION_FACES, new { id = id });


                    case Question.QuestionTypes.SLIDER:
                        return RedirectToAction(Routes.DETAIL, Routes.QUESTION_SLIDER, new { id = id });

                    case Question.QuestionTypes.STARS:
                        return RedirectToAction(Routes.DETAIL, Routes.QUESTION_STARS, new { id = id });

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


        /// <summary>
        /// Render the Edit page 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id, string type)
        {
            try
            {   //convert string to QuestionTypes (enum)
                Question.QuestionTypes tQuestionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));

                //check if the question still exists
                int result = questionManager.IsQuestionExists(id);

                switch (result)
                {
                    case ResultCode.ERROR:
                        return View(Routes.ERROR);
                    case ResultCode.DB_RECORD_NOT_EXISTS:
                        return View(Routes.CUSTOM_ERROR, errorModel);

                }

                //Redirect 
                switch (tQuestionType)
                {
                    case Question.QuestionTypes.FACES:
                        return RedirectToAction(Routes.EDIT, Routes.QUESTION_FACES, new { id = id });


                    case Question.QuestionTypes.SLIDER:
                        return RedirectToAction(Routes.EDIT, Routes.QUESTION_SLIDER, new { id = id });

                    case Question.QuestionTypes.STARS:
                        return RedirectToAction(Routes.EDIT, Routes.QUESTION_STARS, new { id = id });

                    default:
                        break;
                }


            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return View(Routes.ERROR);
        }


        //The Initialize method is a method of the Controller class in ASP.NET MVC. It is called automatically before any action method is executed for a controller
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            try
            {

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            base.Initialize(requestContext);


        }
        /// <summary>
        /// help to handle the Auto Refresh
        /// </summary>
        /// <param name="pQuestions"></param>
        public void RefreshUI(List<Question> pQuestions)
        {
            try
            {
                // Get the hub context for the QuestionHub
                var tHubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();
                // Call the 'RefreshQuestionsClient' method on all connected clients in the 'QuestionHub'
                // The 'RefreshQuestionsClient' method is a client-side method that needs to be defined in JS code to handle the received data.

                tHubContext.Clients.All.RefreshQuestionsClient(pQuestions);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        #endregion

    }
}