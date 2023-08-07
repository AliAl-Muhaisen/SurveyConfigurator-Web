using Microsoft.AspNet.SignalR;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Hubs
{
    public class DbHub:Hub
    {
        public void UpdateConfig(bool isConnected)
        {
            try
            {
                Clients.All.UpdateConfigClient(isConnected);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}