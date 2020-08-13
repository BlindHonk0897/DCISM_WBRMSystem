using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class BorrowRequest
    {
        public DateTime Date_Want_To_Receive { get; set; }
        public DateTime Date_Expected_Return { get; set; }
        public string Purpose { get; set; }
        public  List<ItemsRequest> Items_Request { get; set; }
    }
    public class ItemsRequest
    {
        public string Item_Name { get; set; }
        public int Qty { get; set; }

    }
}