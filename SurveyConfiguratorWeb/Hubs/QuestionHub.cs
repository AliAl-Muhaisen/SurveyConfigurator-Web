using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SurveyConfiguratorWeb.Hubs
{    
    public class QuestionHub : Hub
    {
        public async Task NotifyClients()
        {
            await Clients.All.updateUI();
        }
    }
}