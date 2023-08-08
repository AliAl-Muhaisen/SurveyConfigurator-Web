using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorWeb.Languages;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Models
{
    /// <summary>
    /// Handle the validation messages that should appear on the UI
    /// </summary>
    public class ValidationMessages
    {
        static void QuestionMessages(Question pQuestion, ref ModelStateDictionary pModelState,int pResultCode)
        {
            try
            {
                switch (pResultCode)
                {
                    case ResultCode.VALIDATION_ERROR_ORDER_EXIST:
                        pModelState.AddModelError(nameof(pQuestion.Order), Language.VALIDATION_ERROR_ORDER_EXIST);
                        break;
                    case ResultCode.VALIDATION_ERROR_ORDER_MIN:
                        pModelState.AddModelError(nameof(pQuestion.Order), Language.VALIDATION_ERROR_ORDER_MIN);
                        break;
                    case ResultCode.VALIDATION_ERROR_ORDER_MAX:
                        pModelState.AddModelError(nameof(pQuestion.Order), Language.VALIDATION_ERROR_ORDER_MAX);
                        break;
                    case ResultCode.VALIDATION_ERROR_QUESTION_TEXT:
                        pModelState.AddModelError(nameof(pQuestion.Text), Language.VALIDATION_ERROR_QUESTION_TEXT);
                        break;
                    case ResultCode.VALIDATION_ERROR_LONG_TEXT:
                        pModelState.AddModelError(nameof(pQuestion.Text), Language.VALIDATION_ERROR_LONG_TEXT);
                        break;
                    case ResultCode.VALIDATION_ERROR_SHORT_TEXT:
                        pModelState.AddModelError(nameof(pQuestion.Text), Language.VALIDATION_ERROR_SHORT_TEXT);
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
                            pModelState.AddModelError(nameof(pQuestionSlider.EndValue), Language.VALIDATION_ERROR_SLIDER_END_VALUE);
                            break;
                        case ResultCode.VALIDATION_ERROR_SLIDER_START_VALUE:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartValue), Language.VALIDATION_ERROR_SLIDER_START_VALUE);
                            break;
                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_EMPTY:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartCaption), Language.VALIDATION_ERROR_SLIDER_CAPTION);
                            break;


                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_EMPTY:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndCaption), Language.VALIDATION_ERROR_SLIDER_CAPTION);
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_SHORT:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartCaption), Language.STRING_LENGTH_SHORT);
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_START_LONG:
                            pModelState.AddModelError(nameof(pQuestionSlider.StartCaption), Language.STRING_LENGTH_LONG);
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_SHORT:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndCaption), Language.STRING_LENGTH_SHORT);
                            break;

                        case ResultCode.VALIDATION_ERROR_SLIDER_CAPTION_END_LONG:
                            pModelState.AddModelError(nameof(pQuestionSlider.EndCaption), Language.STRING_LENGTH_LONG);
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
                            pModelState.AddModelError(nameof(pQuestionFaces.FacesNumber), Language.VALIDATION_ERROR_QUESTION_FACES);
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
                            pModelState.AddModelError(nameof(pQuestionStars.StarsNumber), Language.VALIDATION_ERROR_QUESTION_STARS);
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