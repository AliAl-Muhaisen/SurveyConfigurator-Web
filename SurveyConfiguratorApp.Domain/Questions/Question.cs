using SurveyConfiguratorApp.Helper;
using System;
namespace SurveyConfiguratorApp.Domain.Questions
{

    public class Question
    {


        public enum QuestionTypes
        {
            FACES = 1,
            SLIDER = 2,
            STARS = 3,

        }

        public int Id { get { return id; } set { id = value; } }
        private int id;
        public string Text { get; set; }

        private int typeNumber;
        public string TypeName { get; set; }
        public int Order { get; set; }

        public Question()
        {
            SetId(-1);

        }
        public Question(int id, string text, int type, int order)
        {
            try
            {
                Text = text;
                SetTypeNumber(type);
                SetId(id);
                Order = order;
                TypeName = ((QuestionTypes)typeNumber).ToString();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }


        }

        public void SetId(int id)
        {
            try
            {
                this.id = id;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
        public int GetId()
        {
            try
            {
                return this.id;

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return -1;

        }

        protected void SetTypeNumber(int type)
        {
            try
            {
                typeNumber = type;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
        public int GetTypeNumber()
        {
            try
            {
                return typeNumber;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return 0;

        }
        public override bool Equals(object obj)
        {
            try
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                Question otherQuestion = (Question)obj;

                // Compare all relevant properties for equality
                return (this.GetId() == otherQuestion.GetId() &&
                    Text == otherQuestion.Text &&
                       Order == otherQuestion.Order &&
                       TypeName == otherQuestion.TypeName);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }

        }

        ///  <summary>
        /// The GetHashCode method should be overridden consistently with the Equals method.
        /// The GetHashCode method should be overridden consistently with the Equals method. 
        /// That means if two objects are considered equal based on their property values in the Equals method, 
        /// they should have the same hash code in the GetHashCode method
        /// </summary>

        public override int GetHashCode()
        {
            return GetId()*Order*typeNumber;
        }
    }
}
