using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    /// <summary>
    /// Contain app routes
    /// </summary>
    public class Routes
    {
        public const string SLASH = "/";


        #region Question Routes

        public const string QUESTION_CRAETE = "Question/Create";
        public const string QUESTION_DETAIL = "Question/Detail";
        public const string QUESTION_EDIT = "Question/Edit";
        public const string QUESTION_DELETE= "Question/Delete";

        public const string QUESTION_FACES_CRAETE = QUESTION_CRAETE + "/Faces";
        public const string QUESTION_FACES_DETAIL = QUESTION_DETAIL + "/Faces";
        public const string QUESTION_FACES_EDIT = QUESTION_EDIT + "/Faces";


        public const string QUESTION_STARS_CRAETE = QUESTION_CRAETE + "/Stars";
        public const string QUESTION_STARS_DETAIL = QUESTION_DETAIL + "/Stars";
        public const string QUESTION_STARS_EDIT = QUESTION_EDIT + "/Stars";


        public const string QUESTION_SLIDER_CRAETE = QUESTION_CRAETE + "/Slider";
        public const string QUESTION_SLIDER_DETAIL = QUESTION_DETAIL + "/Slider";
        public const string QUESTION_SLIDER_EDIT = QUESTION_EDIT + "/Slider";
        #endregion


        #region Error
        public const string ERROR = "Error";
        public const string ERROR_NOTFOUND = "~/Error/NotFound";
        public const string ERROR_SERVER = "~/Error/ServerError";
        public const string CUSTOM_ERROR = "CustomError";
        #endregion

        #region Controllers & Actions
        public const string QUESTION = "Question";
        public const string INDEX= "Index";
        public const string CREATE= "Create";
        public const string DETAIL = "Detail";
        public const string QUESTION_FACES= "QuestionFaces";
        public const string QUESTION_SLIDER= "QuestionSlider";
        public const string QUESTION_STARS= "QuestionStars";
        public const string EDIT = "Edit";


        public const string LANGUAGE = "Language";


        #endregion

        #region Sttings

        #region DB_CONNECTION
        public const string DB_CONNECTION = "DbConnection";
        public const string DB_CONNECTION_CREATE = "DbConnection/Create";
        public const string DB_CONNECTION_CHECK_CONNECTION = DB_CONNECTION+"/Check";
        public const string DB_CONNECTION_TEST = "DbConnection/TestConnection";
        #endregion


        #endregion

        #region Paths
        public const string PATH_QUESTION_CRAETE_FILE = "Questions/_Create";
        public const string PATH_QUESTION_FACES_CRAETE_FILE = "Questions/_CreateQuestionFaces";
        public const string PATH_QUESTION_STARS_CRAETE_FILE = "Questions/_CreateQuestionStars";
        public const string PATH_QUESTION_SLIDER_CRAETE_FILE = "Questions/_CreateQuestionSlider";
        public const string PATH_BACK_TO_LIST_FILE = "Questions/_BackToList";
        public const string PATH_DB_CONNECTION_FAILED = "_DbConnectionFailed";
        #endregion
    }
}