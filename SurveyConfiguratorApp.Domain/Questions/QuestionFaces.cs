using SurveyConfiguratorApp.Helper;
using System;

namespace SurveyConfiguratorApp.Domain.Questions
{
    public class QuestionFaces : Question
    {

        public QuestionFaces() : base()
        {
            try
            {
                SetTypeNumber((int)QuestionTypes.FACES);
                TypeName = QuestionTypes.FACES.ToString();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }


        }
        public int FacesNumber { get; set; }


    }
}
