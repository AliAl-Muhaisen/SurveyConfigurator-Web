using SurveyConfiguratorApp.Data.Questions;
using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.Logic
{
    public class QuestionManager
    {
        private readonly DbQuestion dbQuestion;
        private readonly DbQuestionFaces dbQuestionFaces;
        private readonly DbQuestionSlider dbQuestionSlider;
        private readonly DbQuestionStars dbQuestionStars;

        readonly QuestionValidation questionValidation;

        private static List<Question> questionsList = new List<Question>();
        private static bool firstCall = true;

        public event EventHandler DataChangedUI;

        public delegate void  DataChange(List<Question> pQuestions);
        public static event DataChange DataChangedUIWeb;

        private Thread thread;

        public List<int> ValidationErrorList;

        const int REFRESH_DURATION = 4000;
        public QuestionManager()
        {
            try
            {
                questionsList = new List<Question>();
                dbQuestion = new DbQuestion();
                dbQuestionFaces = new DbQuestionFaces();
                dbQuestionSlider = new DbQuestionSlider();
                dbQuestionStars = new DbQuestionStars();
                dbQuestion.GetQuestions(ref questionsList);
                questionValidation = new QuestionValidation();
                ValidationErrorList = new List<int>();


            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }

        protected void OnDataChanged()
        {
            try
            {
                DataChangedUI?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }


        }

        private void DbQuestion_DataChanged(object sender, EventArgs e)
        {
            try
            {
                dbQuestion.GetQuestions(ref questionsList);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }


        public static int IsOrderAlreadyExists(int pOrder, int pQuestionId = -1)
        {
            try
            {
                for (int questionIndex = 0; questionIndex < questionsList.Count; questionIndex++)
                {
                    if (questionsList[questionIndex].Order == pOrder && questionsList[questionIndex].GetId() != pQuestionId)
                    {
                        return ResultCode.VALIDATION_ERROR_ORDER_EXIST;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
            return ResultCode.SUCCESS;
        }
        public int IsQuestionExists(int pQuestionId)
        {
            try
            {
                return dbQuestion.IsQuestionExists(pQuestionId);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return ResultCode.ERROR;

            }
        }
        public void AutoRefresh()
        {
            try
            {
                //I used ThreadStart because the method does not take any parameters
                List<Question> list = new List<Question>();

                thread = new Thread(new ThreadStart(delegate
               {
                   while (true)
                   {
                       
                       int tResult = GetQuestions(ref list);

                       bool tIsNotEqual = !(list.SequenceEqual<Question>(questionsList));
                       if (tIsNotEqual || firstCall)
                       {
                           questionsList.Clear();
                           GetQuestions(ref questionsList);
                           firstCall = false;
                           OnDataChanged();
                           DataChangedUIWeb?.Invoke(list);

                       }
                       list.Clear();
                       Thread.Sleep(REFRESH_DURATION);
                   }

               }));


                thread.IsBackground = true;
                thread.Start();
                // Wait for the background thread to complete
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void AutoRefreshWeb(List<Question> pQuestions)
        {
            try
            {
                questionsList = pQuestions;
                AutoRefresh();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }


        public int GetQuestions(ref List<Question> pList)
        {
            try
            {
                return dbQuestion.GetQuestions(ref pList);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        public Question GetQuestion(int pID)
        {
            try
            {
                return dbQuestion.Get(pID);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return null;
        }
       
        public int Delete(int pId)
        {
            try
            {
                return dbQuestion.Delete(pId);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        #region QuestionFaces
        public int AddQuestionFaces(QuestionFaces pQuestionFaces)
        {
            ValidationErrorList.Clear();
            try
            {

                int tResult = questionValidation.IsValidFacesQuestion(pQuestionFaces);
                if (tResult != ResultCode.SUCCESS)
                {
                    ValidationErrorList = questionValidation.ErorrValidationList;
                    return ResultCode.VALIDATION_ERROR;
                }

                return dbQuestionFaces.Add(pQuestionFaces);

            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        public int UpdateQuestionFaces(QuestionFaces pQuestionFaces)
        {
            ValidationErrorList.Clear();

            try
            {
                int tResult = questionValidation.IsValidFacesQuestion(pQuestionFaces);
                if (tResult != ResultCode.SUCCESS)
                {
                    ValidationErrorList = questionValidation.ErorrValidationList;
                    return ResultCode.VALIDATION_ERROR;
                }
                return dbQuestionFaces.Update(pQuestionFaces);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        public int GetQuestionFaces(ref QuestionFaces tQuestionFaces)
        {
            try
            {
                return dbQuestionFaces.Get(ref tQuestionFaces); ;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }

        }
        #endregion

        #region QuestionStars

        //Stars
        public int AddQuestionStars(QuestionStars tQuestionStars)
        {
            try
            {
                ValidationErrorList.Clear();
                int tResult = questionValidation.IsValidStarsQuestion(tQuestionStars);

                if (tResult != ResultCode.SUCCESS)
                {
                    ValidationErrorList = questionValidation.ErorrValidationList;
                    return ResultCode.VALIDATION_ERROR;
                }

                return dbQuestionStars.Add(tQuestionStars);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        public int UpdateQuestionStars(QuestionStars pQuestionStars)
        {
            try
            {
                ValidationErrorList.Clear();

                int tResult = questionValidation.IsValidStarsQuestion(pQuestionStars);
                if (tResult != ResultCode.SUCCESS)
                {
                    ValidationErrorList = questionValidation.ErorrValidationList;
                    return ResultCode.VALIDATION_ERROR;
                }


                return dbQuestionStars.Update(pQuestionStars);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        public int GetQuestionStars(ref QuestionStars pQuestionStars)
        {
            try
            {
                return dbQuestionStars.Get(ref pQuestionStars);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        #endregion

        #region QuestionSlider
        //Slider
        public int AddQuestionSlider(QuestionSlider pQuestionSlider)
        {
            try
            {
                ValidationErrorList.Clear();

                int tResult = questionValidation.IsValidSliderQuestion(pQuestionSlider);
                if (tResult != ResultCode.SUCCESS)
                {
                    ValidationErrorList = questionValidation.ErorrValidationList;
                    return ResultCode.VALIDATION_ERROR;
                }

                return dbQuestionSlider.Add(pQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }

        }
        public int UpdateQuestionSlider(QuestionSlider pQuestionSlider)
        {
            try
            {
                ValidationErrorList.Clear();

                int tResult = questionValidation.IsValidSliderQuestion(pQuestionSlider);
                if (tResult != ResultCode.SUCCESS)
                {
                    ValidationErrorList = questionValidation.ErorrValidationList;
                    return ResultCode.VALIDATION_ERROR;
                }

                return dbQuestionSlider.Update(pQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }

        }

        public int GetQuestionSlider(ref QuestionSlider pQuestionSlider)
        {
            try
            {
                return dbQuestionSlider.Get(ref pQuestionSlider);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        #endregion

    }

}
