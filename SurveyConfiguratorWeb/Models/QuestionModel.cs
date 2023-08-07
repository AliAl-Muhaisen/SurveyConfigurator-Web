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
       public List<Question> QuestionList { get; set; }
        public int IsDbConnected { get; set; }
    }
}