using Microsoft.Extensions.DependencyInjection;
using SurveyConfiguratorApp.Data.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using SurveyConfiguratorApp.Logic.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.Domain.Questions
{
    /// <summary>
    /// [Singleton Class] 
    /// 
    /// To Read more about Singleton https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/
    /// </summary>
    public class QuestionValidation
    {
        public int SliderMaxValue { get; private set; }
        public int SliderMinValue { get; private set; }
        public int StarsMaxValue { get; private set; }
        public int StarsMinValue { get; private set; }

        public int FacesMaxValue { get; private set; }
        public int FacesMinValue { get; private set; }

        const int QUESTION_TEXT_LENGTH = 1500;
        const int SLIDER_CAPTION_TEXT_LENGTH_MAX = 500;
        const int SLIDER_CAPTION_TEXT_LENGTH_MIN = 3;
        const int ORDER_MIN_VALUE = 1;
        const int ORDER_MAX_VALUE = 100;
        public List<int> ErorrValidationList;

        public QuestionValidation()
        {
            try
            {
                SliderMaxValue = 100;
                SliderMinValue = 0;


                StarsMaxValue = 10;
                StarsMinValue = 1;

                FacesMaxValue = 5;
                FacesMinValue = 2;
                ErorrValidationList = new List<int>();
            }
            catch (Exception e)
            {
                Log.Error(e);

            }

        }



        //General Functions
        public bool IsNotEmpty(string pText)
        {
            return (pText != null && pText.Trim().Length > 0);
        }
        public bool IsEmpty(string pText)
        {
            return !IsNotEmpty(pText);
        }

        private bool IsMinNum(int pSourceNum, int pCompareNum)
        {
            return pCompareNum >= pSourceNum;
        }
     
        private int IsValidQuestionText(string pText)
        {
            try
            {
                if (pText==null)
                {
                    return ResultCode.VALIDATION_ERROR_QUESTION_TEXT;
                }
                int tTextLength=pText.Trim().Length;
                if (tTextLength<=0)
                {
                    return ResultCode.VALIDATION_ERROR_QUESTION_TEXT;
                }
               else if (tTextLength<=10)
                {
                    return ResultCode.VALIDATION_ERROR_SHORT_TEXT;
                }
                else if(tTextLength>QUESTION_TEXT_LENGTH)
                {
                    return ResultCode.VALIDATION_ERROR_LONG_TEXT;
                }
                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
     
        //End General Functions
        public int IsValidOrderRange(int pOrder)
        {
            try
            {
                if (pOrder<ORDER_MIN_VALUE)
                {
                    return ResultCode.VALIDATION_ERROR_ORDER_MIN;
                }
                else if(pOrder > ORDER_MAX_VALUE)
                {
                    return ResultCode.VALIDATION_ERROR_ORDER_MAX;
                }
                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        public int IsOrderAlreadyExists(int pOrder, int pQuestionId)
        {
            try
            {
                return QuestionManager.IsOrderAlreadyExists(pOrder, pQuestionId);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        #region Faces
        private int IsValidFacesNumber(int pFacesNumber)
        {
            try
            {
                return (pFacesNumber >= FacesMinValue && pFacesNumber <= FacesMaxValue) ? ResultCode.SUCCESS : ResultCode.VALIDATION_ERROR_FACES_NUMBER;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        public int IsValidFacesQuestion(QuestionFaces pQuestionFaces)
        {

            try
            {
                ErorrValidationList.Clear();

                int tIsOrderExists;
                if (pQuestionFaces == null)
                {
                    return ResultCode.DB_RECORD_NOT_EXISTS; ;
                }


                tIsOrderExists = IsOrderAlreadyExists(pQuestionFaces.Order, pQuestionFaces.GetId());
                ErorrValidationList.Add(IsValidOrderRange(pQuestionFaces.Order));
                ErorrValidationList.Add(tIsOrderExists);
                ErorrValidationList.Add(IsValidQuestionText(pQuestionFaces.Text));
                ErorrValidationList.Add(IsValidFacesNumber(pQuestionFaces.FacesNumber));
                ClearSuccessState(ref ErorrValidationList);
                if (ErorrValidationList.Count != 0) return ResultCode.VALIDATION_ERROR;

                return ResultCode.SUCCESS;

            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        #endregion
        
        
        #region Stars
        private int IsValidStarsNumber(int pStarsNumber)
        {
            try
            {
                return (pStarsNumber >= StarsMinValue && pStarsNumber <= StarsMaxValue) ? ResultCode.SUCCESS : ResultCode.VALIDATION_ERROR_STARS_NUMBER;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        public int IsValidStarsQuestion(QuestionStars pQuestionStars)
        {
            try
            {
                ErorrValidationList.Clear();
                int pIsOrderExists;
                if (pQuestionStars == null)
                {
                    return ResultCode.DB_RECORD_NOT_EXISTS; ;
                }

                pIsOrderExists = IsOrderAlreadyExists(pQuestionStars.Order, pQuestionStars.GetId());
                ErorrValidationList.Add(pIsOrderExists);
                ErorrValidationList.Add(IsValidOrderRange(pQuestionStars.Order));

                ErorrValidationList.Add(IsValidQuestionText(pQuestionStars.Text));
                ErorrValidationList.Add(IsValidStarsNumber(pQuestionStars.StarsNumber));
                ClearSuccessState(ref ErorrValidationList);
                if (ErorrValidationList.Count != 0)
                    return ResultCode.VALIDATION_ERROR;

                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        #endregion

        #region Slider
        private int IsValidSliderStartValue(int pValue)
        {
            try
            {
                return (pValue >= SliderMinValue && pValue < SliderMaxValue) ? ResultCode.SUCCESS : ResultCode.VALIDATION_ERROR_SLIDER_START_VALUE;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        private int IsValidSliderEndtValue(int pValue)
        {
            try
            {
                return (pValue > SliderMinValue && (pValue <= SliderMaxValue)) ? ResultCode.SUCCESS : ResultCode.VALIDATION_ERROR_SLIDER_END_VALUE;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        private int IsValidSilderValue(int pMin, int pMax)
        {
            try
            {
                int tStartValueCode = IsValidSliderStartValue(pMin);
                int tEndValueCode = IsValidSliderEndtValue(pMax);
                if (tStartValueCode == ResultCode.SUCCESS && tEndValueCode == ResultCode.SUCCESS)
                {
                    return (pMin < pMax) ? ResultCode.SUCCESS : ResultCode.VALIDATION_ERROR_SLIDER_VALUE;
                }
                return ResultCode.VALIDATION_ERROR_SLIDER_VALUE;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        private int IsValidCaption(string pCaption)
        {
            try
            {
                return (pCaption.Trim().Length >= SLIDER_CAPTION_TEXT_LENGTH_MIN && pCaption.Trim().Length <= SLIDER_CAPTION_TEXT_LENGTH_MAX) ? ResultCode.SUCCESS : ResultCode.VALIDATION_ERROR_SLIDER_CAPTION;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }
        private int IsValidStartCaption(string pCaption)
        {
            try
            {
                
                if (pCaption ==null || pCaption.Trim().Length==0)
                {
                    return ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_EMPTY;
                }

                int tCaptionLength = pCaption.Trim().Length;
                if (tCaptionLength <= SLIDER_CAPTION_TEXT_LENGTH_MIN)
                {
                    return ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_SHORT;
                }
                else if(tCaptionLength > SLIDER_CAPTION_TEXT_LENGTH_MAX)
                {
                    return ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_LONG;
                }
                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        private int IsValidEndCaption(string pCaption)
        {
            try
            {

                if (pCaption == null || pCaption.Trim().Length == 0)
                {
                    return ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_EMPTY;
                }

                int tCaptionLength = pCaption.Trim().Length;
                if (tCaptionLength <= SLIDER_CAPTION_TEXT_LENGTH_MIN)
                {
                    return ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_SHORT;
                }
                else if (tCaptionLength > SLIDER_CAPTION_TEXT_LENGTH_MAX)
                {
                    return ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_LONG;
                }
                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        public int IsValidSliderQuestion(QuestionSlider pQuestionSlider)
        {
            try
            {
                ErorrValidationList.Clear();
                int tIsOrderExists;
                if (pQuestionSlider == null)
                {
                    return ResultCode.DB_RECORD_NOT_EXISTS;
                }

                tIsOrderExists = IsOrderAlreadyExists(pQuestionSlider.Order, pQuestionSlider.GetId());
                ErorrValidationList.Add(tIsOrderExists);
                ErorrValidationList.Add(IsValidOrderRange(pQuestionSlider.Order));
                ErorrValidationList.Add(IsValidQuestionText(pQuestionSlider.Text));
                ErorrValidationList.Add(IsValidSliderStartValue(pQuestionSlider.StartValue));
                ErorrValidationList.Add(IsValidSliderEndtValue(pQuestionSlider.EndValue));
                ErorrValidationList.Add(IsValidSilderValue(pQuestionSlider.StartValue, pQuestionSlider.EndValue));
                ErorrValidationList.Add(IsValidStartCaption(pQuestionSlider.StartCaption));
                ErorrValidationList.Add(IsValidEndCaption(pQuestionSlider.EndCaption));
                ClearSuccessState(ref ErorrValidationList);
                if (ErorrValidationList.Count != 0)
                    return ResultCode.VALIDATION_ERROR;

                return ResultCode.SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        #endregion

        void ClearSuccessState(ref List<int> pList)
        {
            try
            {
                pList = pList.Distinct<int>().ToList();

                for (int tListIndex = 0; tListIndex < pList.Count; tListIndex++)
                {
                    if (pList[tListIndex] == ResultCode.SUCCESS)
                    {
                        pList.RemoveAt(tListIndex);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

    }
}
