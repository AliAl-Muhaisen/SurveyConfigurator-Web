﻿using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using SurveyConfiguratorApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Models
{

    /// <summary>
    /// Convert FormCollection To Object
    /// </summary>
    public class FormToObj
    {



        #region Questions
        const string TEXT = "Text";
        const string ORDER = "Order";
        public const string TYPE_NAME = "TypeName";
        const string FACES_NUMBER = "FacesNumber";
        const string STARS_NUMBER = "StarsNumber";
        const string START_VALUE = "StartValue";
        const string END_VALUE = "EndValue";
        const string START_CAPTION = "StartCaption";
        const string END_CAPTION = "EndCaption";
        static public QuestionFaces QuestionFaces(FormCollection pFormCollection)
        {
            try
            {
                QuestionFaces tQuestionFaces = new QuestionFaces();
                tQuestionFaces.Text = pFormCollection[TEXT];
                tQuestionFaces.Order = int.Parse(pFormCollection[ORDER]);
                tQuestionFaces.FacesNumber = int.Parse(pFormCollection[FACES_NUMBER]);
                return tQuestionFaces;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return new QuestionFaces();
        }
        static public QuestionStars QuestionStars(FormCollection pFormCollection)
        {
            try
            {
                QuestionStars tQuestionStars = new QuestionStars();
                tQuestionStars.Text = pFormCollection[TEXT];
                tQuestionStars.Order = int.Parse(pFormCollection[ORDER]);
                tQuestionStars.StarsNumber = int.Parse(pFormCollection[STARS_NUMBER]);
                return tQuestionStars;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return new QuestionStars();
        }
        static public QuestionSlider QuestionSlider(FormCollection pFormCollection)
        {
            try
            {
                QuestionSlider tQuestionSlider = new QuestionSlider();
                tQuestionSlider.Text = pFormCollection[TEXT];
                tQuestionSlider.StartCaption = pFormCollection[START_CAPTION];
                tQuestionSlider.EndCaption = pFormCollection[END_CAPTION];
                tQuestionSlider.Order = int.Parse(pFormCollection[ORDER]);
                tQuestionSlider.EndValue = int.Parse(pFormCollection[END_VALUE]);
                tQuestionSlider.StartValue = int.Parse(pFormCollection[START_VALUE]);
                return tQuestionSlider;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return new QuestionSlider();
        }
        #endregion


        #region DB Connection

        const string SERVER= "Server";
        const string DB= "Database";
        const string USER_NAME= "Username";
        const string PASSWORD= "Password";
        static public DbManager DbManager(FormCollection pFormCollection)
        {
            try
            {
                string tServer = pFormCollection[SERVER];
                string tDb = pFormCollection[DB];
                string tUserName = pFormCollection[USER_NAME];
                string tPassword = pFormCollection[PASSWORD];
                DbManager tDbManager = new DbManager(tServer, tDb, tUserName, tPassword);
                return tDbManager;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new DbManager();
            }
        }
        #endregion
    }
}