using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    public class HttpResponseCustom
    {
        public bool Success { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
        public HttpResponseCustom()
        {
            Success = false;
            Message = "";
            Error = false;
        }
        public static HttpResponseCustom BuildSuccess(string pMessage="")
        {
            try
            {
                HttpResponseCustom tHttpResponseCustom = new HttpResponseCustom
                {
                    Success = true,
                    Message = pMessage,
                    Error = false
                };
                return tHttpResponseCustom;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new HttpResponseCustom();
            }
        }

        public static HttpResponseCustom BuildError(string pMessage = "")
        {
            try
            {
                HttpResponseCustom tHttpResponseCustom = new HttpResponseCustom
                {
                    Success = false,
                    Message = pMessage,
                    Error = true
                };
                return tHttpResponseCustom;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new HttpResponseCustom();
            }
        }
        public static HttpResponseCustom Build(bool pIsSuccess=false,string pMessageSuccess = "",string pMessageError = "")
        {
            try
            {
                HttpResponseCustom tHttpResponseCustom = new HttpResponseCustom
                {
                    Success = pIsSuccess,
                    Message = pIsSuccess? pMessageSuccess: pMessageError,
                    Error = !pIsSuccess
                };
                return tHttpResponseCustom;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new HttpResponseCustom();
            }
        }


    }
}