using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;

namespace SurveyConfiguratorWeb.Hubs
{    
    public class QuestionHub : Hub
    {
        public async Task NotifyClients()
        {
            await Clients.All.updateUI();
        }

        public void RefreshQuestions(List<Question> pQuestionsList)
        {
            try
            {
                Clients.All.RefreshQuestionsClient(pQuestionsList);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}