using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DCISM_WBRMSystem.Hubs
{
    [HubName("usernotificationHub")]
    public class UserNotificationHub : Hub
    {
        public void Notification(string id,string notification)
        {
            Clients.All.showNotification(id,notification);
        }
    }
}