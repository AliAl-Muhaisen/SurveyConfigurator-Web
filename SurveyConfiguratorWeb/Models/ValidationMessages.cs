using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Models
{
    public class ValidationMessages
    {
        static void QuestionMessages( Question pQuestion, ref ModelStateDictionary pModelState,int pResultCode)
        {
            try
            {
                switch (pResultCode)
                {
                    case ResultCode.VALIDATION_ERROR_ORDER_EXIST:
                        pModelState.AddModelError(nameof(pQuestion.Order), "Order should be unique");
                        break;
                    case ResultCode.VALIDATION_ERROR_ORDER_MIN:
                        pModelState.AddModelError(nameof(pQuestion.Order), "Order should be greater than 0");
                        break;
                    case ResultCode.VALIDATION_ERROR_ORDER_MAX:
                        pModelState.AddModelError(nameof(pQuestion.Order), "Order should be less than or equal 100");
                        break;
                    case ResultCode.VALIDATION_ERROR_QUESTION_TEXT:
                        pModelState.AddModelError(nameof(pQuestion.Text), "The Text is Required!");
                        break;
                    case ResultCode.VALIDATION_ERROR_LONG_TEXT:
                        pModelState.AddModelError(nameof(pQuestion.Text), "Too long");
                        break;
                    case ResultCode.VALIDATION_ERROR_SHORT_TEXT:
                        pModelState.AddModelError(nameof(pQuestion.Text), "Too short");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
      static  public void Validate(Question pQuestionSlider, ModelStateDictionary modelState)
        {
            // Perform validation and add errors to the ModelState
            if (string.IsNullOrEmpty(pQuestionSlider.Text))
            {
                modelState.AddModelError(nameof(pQuestionSlider.Text), "The Text is Required!");
            }

            // Add more validation rules as needed...
        }

        static public void SliderValidation(ref QuestionSlider pQuestionSlider, ModelStateDictionary pModelState,List<int>pErrorCode)
        {
            try
            {
                for (int index = 0; index < pErrorCode.Count; index++)
                {

                    QuestionMessages( pQuestionSlider,ref pModelState, pErrorCode[index]);
                    switch (pErrorCode[index])
                    {
                        case ResultCode.VALIDATION_ERROR_SLIDER_END_VALUE:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndValue), "End value should be greater than the Min value");
                            break;
                        case ResultCode.VALIDATION_ERROR_SLIDER_START_VALUE:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartValue), " Min value should be less than the End value");
                            break;
                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_EMPTY:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartCaption), "Required");
                            break;


                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_EMPTY:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndCaption), "Required");
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_SHORT:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartCaption), "Too Short");
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_LONG:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartCaption), "Too Long");
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_SHORT:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndCaption), "Too Short");
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_LONG:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndCaption), "Too Long");
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        static public void FacesValidation(ref QuestionFaces pQuestionFaces, ModelStateDictionary pModelState, List<int> pErrorCode)
        {
            try
            {
                for (int index = 0; index < pErrorCode.Count; index++)
                {
                    QuestionMessages(pQuestionFaces, ref pModelState, pErrorCode[index]);
                    switch (pErrorCode[index])
                    {
                        case ResultCode.VALIDATION_ERROR_FACES_NUMBER:
                            pModelState.AddModelError(nameof(pQuestionFaces.FacesNumber), "Faces number should be between 1 to 5");
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        static public void StarsValidation(ref QuestionStars pQuestionStars, ModelStateDictionary pModelState, List<int> pErrorCode)
        {
            try
            {
                for (int index = 0; index < pErrorCode.Count; index++)
                {
                    QuestionMessages(pQuestionStars, ref pModelState, pErrorCode[index]);
                    switch (pErrorCode[index])
                    {
                        case ResultCode.VALIDATION_ERROR_STARS_NUMBER:
                            pModelState.AddModelError(nameof(pQuestionStars.StarsNumber), "Stars number should be between 1 to 10");
                            break;

                        default:
                            break;
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