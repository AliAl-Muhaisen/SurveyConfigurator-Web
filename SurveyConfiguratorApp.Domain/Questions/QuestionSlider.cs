using SurveyConfiguratorApp.Helper;
using System;

namespace SurveyConfiguratorApp.Domain.Questions
{
    public class QuestionSlider : Question
    {
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public string StartCaption { get; set; }
        public string EndCaption { get; set; }
        public QuestionSlider() : base()
        {

            try
            {
                SetTypeNumber((int)QuestionTypes.SLIDER);
                TypeName = QuestionTypes.SLIDER.ToString();

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

    }
}
