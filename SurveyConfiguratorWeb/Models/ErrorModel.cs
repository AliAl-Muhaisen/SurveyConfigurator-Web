using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    public class ErrorModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public ErrorModel(){
            Title = "Error";
            Message = "This Question does not exists";
        }
    }
}