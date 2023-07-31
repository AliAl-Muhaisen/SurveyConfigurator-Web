using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    public class QuestionModel2
    {
       public readonly Question question;
       public QuestionFaces questionFaces { get; set; }
       public readonly QuestionStars questionStars;
        public readonly QuestionManager questionManager;
        public QuestionModel2()
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