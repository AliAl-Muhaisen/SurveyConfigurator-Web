using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    public class Routes
    {
        #region Question Routes

        public const string QUESTION_CRAETE = "Question/Create";
        public const string QUESTION_DETAIL = "Question/Detail";
        public const string QUESTION_EDIT = "Question/Edit";

        public const string QUESTION_FACES_CRAETE= QUESTION_CRAETE+"/Faces";
       public  const string QUESTION_FACES_DETAIL = QUESTION_DETAIL +"/Faces";
        public const string QUESTION_FACES_EDIT= QUESTION_EDIT +"/Faces";


        public const string QUESTION_STARS_CRAETE = QUESTION_CRAETE + "/Stars";
        public const string QUESTION_STARS_DETAIL = QUESTION_DETAIL + "/Stars";
        public const string QUESTION_STARS_EDIT = QUESTION_EDIT + "/Stars";


        public const string QUESTION_SLIDER_CRAETE = QUESTION_CRAETE + "/Slider";
        public const string QUESTION_SLIDER_DETAIL = QUESTION_DETAIL + "/Slider";
        public const string QUESTION_SLIDER_EDIT = QUESTION_EDIT + "/Slider";
        #endregion
    }
}