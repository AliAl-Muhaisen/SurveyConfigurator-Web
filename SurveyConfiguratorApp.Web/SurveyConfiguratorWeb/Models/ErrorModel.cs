using SurveyConfiguratorWeb.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    /// <summary>
    /// This class represents the Error Model used to pass error details to the Custom Error page.
    /// </summary>
    public class ErrorModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public ErrorModel(){
            // Set the default error title and message
            Title = Language.ERROR;
            Message = Language.DB_RECORD_NOT_EXISTS;
        }
    }
}