using Microsoft.AspNet.SignalR;
using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorWeb.Hubs;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class QuestionController : Controller
    {
        public readonly QuestionManager questionManager;
        List<Question> questionList;
        ErrorModel errorModel;
        public QuestionController()
        {
            try
            {
                questionManager = new QuestionManager();
                questionList = new List<Question>();
                questionManager.GetQuestions(ref questionList);

                QuestionManager.DataChangedUIWeb += RefreshUI;
                  questionManager.FollowDbChangesWeb(questionList);
            //   questionManager.DataChangedUI+=
              //  questionManager.FollowDbChanges();
                
                errorModel = new ErrorModel();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        // GET: Question
        public ActionResult Index()
        {
            try
            {
                return View(questionList);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
        }
        public JsonResult Delete(int pID)
        {
            try
            {
                int result = questionManager.Delete(pID);
                if (result == ResultCode.SUCCESS)
                {
                    return Json(new { success = true, Status = 200 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return Json(new { success = false, Status = 404 }, JsonRequestBehavior.AllowGet);

        }

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


        [HttpPost]
        public ActionResult Create(FormCollection pFormCollection)
        {
            try
            {
                string tTypeName = pFormCollection[FormToObj.TYPE_NAME];
                Question.QuestionTypes tQuestionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), tTypeName));
                int result = 0;


                QuestionFaces tQuestionFaces;
                QuestionStars tQuestionStars;
                QuestionSlider tQuestionSlider;
                switch (tQuestionType)
                {
                    case Question.QuestionTypes.FACES:

                        tQuestionFaces = FormToObj.QuestionFaces(pFormCollection);
                        result = questionManager.AddQuestionFaces(tQuestionFaces);
                        if (result != ResultCode.SUCCESS)
                        {
                            ValidationMessages.FacesValidation(ref tQuestionFaces, ModelState, questionManager.ValidationErrorList);
                            return View(tQuestionFaces);
                        }

                        break;
                    case Question.QuestionTypes.SLIDER:
                        tQuestionSlider = FormToObj.QuestionSlider(pFormCollection);
                        result = questionManager.AddQuestionSlider(tQuestionSlider);
                        if (result != ResultCode.SUCCESS)
                        {
                            ValidationMessages.SliderValidation(ref tQuestionSlider, ModelState, questionManager.ValidationErrorList);
                            return View(tQuestionSlider);
                        }

                        break;
                    case Question.QuestionTypes.STARS:
                        tQuestionStars = FormToObj.QuestionStars(pFormCollection);
                        result = questionManager.AddQuestionStars(tQuestionStars);
                        if (result != ResultCode.SUCCESS)
                        {
                            ValidationMessages.StarsValidation(ref tQuestionStars, ModelState, questionManager.ValidationErrorList);
                            return View(tQuestionStars);
                        }
                        break;
                    default:
                        break;
                }
                if (result == ResultCode.SUCCESS)
                    return RedirectToAction(Routes.INDEX);
                return View();
            }
            catch (Exception e)
            {
                Log.Error(e);
                return View(Routes.ERROR);
            }
          
        }

        public ActionResult Detail(int id,string type)
        {
            try
            {
         Question.QuestionTypes questionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));
            int tResult = questionManager.IsQuestionExists(id);
            errorModel.Message = "This Question does not exists or the connection failed";
            if (tResult!=ResultCode.SUCCESS)
            {
                return View(Routes.CUSTOM_ERROR, errorModel);

            }
            switch (questionType)
            {
                case Question.QuestionTypes.FACES:
                    return RedirectToAction(Routes.DETAIL, Routes.QUESTION_FACES, new { id = id });
                  

                case Question.QuestionTypes.SLIDER:
                    return RedirectToAction(Routes.DETAIL,Routes.QUESTION_SLIDER , new { id = id });

                case Question.QuestionTypes.STARS:
                    return RedirectToAction(Routes.DETAIL, Routes.QUESTION_STARS , new { id = id });

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

        [HttpGet]
        public ActionResult Edit(int id,string type)
        {
            try
            {
                Question.QuestionTypes tQuestionType = ((Question.QuestionTypes)Enum.Parse(typeof(Question.QuestionTypes), type));


                int result = questionManager.IsQuestionExists(id);

                switch (result)
                {
                    case ResultCode.ERROR:
                        return View(Routes.ERROR);
                    case ResultCode.DB_RECORD_NOT_EXISTS:
                        return View(Routes.CUSTOM_ERROR, errorModel);
                    
                }


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
       

        private void QuestionManager_DataChangedUI(object sender, EventArgs e)
        {
            try
            {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            hubContext.Clients.All.NotifyClients();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            
        }


        //The Initialize method is a method of the Controller class in ASP.NET MVC. It is called automatically before any action method is executed for a controller
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            try
            {
            var questionManager = new QuestionManager(); 
            questionManager.DataChangedUI += QuestionManager_DataChangedUI;
            questionManager.FollowDbChanges();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            base.Initialize(requestContext);

           
        }

        public void RefreshUI(List<Question> pQuestions)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

                hubContext.Clients.All.RefreshQuestions(pQuestions);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        public void RefreshUI2(object pSender,EventArgs e)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

                hubContext.Clients.All.NotifyClients();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}