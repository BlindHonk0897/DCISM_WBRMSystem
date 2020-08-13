using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models
{
    public class BorrowPendingRequest
    {
        public int id { get; set; }
        public string Requestor { get; set; }
        public string DateRequested { get; set; }
        public string Purpose { get; set; }
    }
}