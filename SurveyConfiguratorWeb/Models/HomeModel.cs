using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{

    public class HomeModel
    {
       public List<Question> questionList;
       public QuestionManager questionManager;

        public HomeModel()
        {
            try
            {
                questionList = new List<Question>();
                questionManager = new QuestionManager();
                questionManager.GetQuestions(ref questionList);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public int GetQuestions(ref List<Question> pQuestionList)
        {
            try
            {
                pQuestionList= questionList;
                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }

        }
    }
}