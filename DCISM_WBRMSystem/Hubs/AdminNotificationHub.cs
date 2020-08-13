using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DCISM_WBRMSystem.Hubs
{
    [HubName("adminnotificationHub")]
    public class AdminNotificationHub : Hub
    {
        public void Notification(string notification)
        {
            Clients.All.showNotification(notification);
        }
    }
}