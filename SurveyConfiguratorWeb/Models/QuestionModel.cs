using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    public class QuestionModel
    {
       public readonly Question question;
       public QuestionFaces questionFaces;
       public readonly QuestionStars questionStars;
        public readonly QuestionManager questionManager;
        public QuestionModel()
        {
            try
            {
                question = new Question();
                questionFaces = new QuestionFaces
                {
                    Text = "Default Text",
                    Order = 1,
                    FacesNumber = 5 // or any other default value you want
                };
                questionStars = new QuestionStars();
                questionManager = new QuestionManager();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}