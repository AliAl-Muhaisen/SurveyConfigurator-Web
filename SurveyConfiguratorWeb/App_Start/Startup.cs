using Microsoft.Owin;
using Owin;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//[assembly: OwinStartup(typeof(SurveyConfiguratorWeb.Startup))]
namespace SurveyConfiguratorWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                    // Enable SignalR hubs
                     app.MapSignalR();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
           

        }
    }
}