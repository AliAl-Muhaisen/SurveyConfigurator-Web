using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{

    /// <summary>
    /// HTTP Response
    /// </summary>
    public class HttpResponseCustom
    {
        public bool Success { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Default constructor for the HttpResponseCustom class.
        /// Initializes the Success, Error, and Message properties to default values.
        /// </summary>
        public HttpResponseCustom()
        {
            Success = false;
            Message = "";
            Error = false;
        }

        // <summary>
        /// Creates a new instance of HttpResponseCustom with Success set to true.
        /// </summary>
        /// <param name="pMessage">Optional success message.</param>
        /// <returns>An instance of HttpResponseCustom with Success set to true and the specified message.</returns>
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

        /// <summary>
        /// Creates a new instance of HttpResponseCustom with Error set to true.
        /// </summary>
        /// <param name="pMessage">Optional error message</param>
        /// <returns>An instance of HttpResponseCustom with Error set to true and the specified message.</returns>
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

        /// <summary>
        /// Creates a new instance of HttpResponseCustom with specified success and error messages based on the success flag.
        /// </summary>
        /// <param name="pIsSuccess">A flag indicating whether the response is a success.</param>
        /// <param name="pMessageSuccess">Optional success message.</param>
        /// <param name="pMessageError">Optional error message.</param>
        /// <returns>An instance of HttpResponseCustom with Success and Error properties set based on the success flag, and the specified message.</returns>
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