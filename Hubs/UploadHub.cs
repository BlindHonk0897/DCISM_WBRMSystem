using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DCISM_WBRMSystem.Hubs
{
    [HubName("uploadHub")]
    public class UploadHub : Hub
    {
        public void ShowUploadProgess(string progress)
        {
            Clients.All.showDeleteProgress(progress);
        }
    }
}