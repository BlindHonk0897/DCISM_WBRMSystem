using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class DeleteItemModel
    {
        public int Id { get; set; }
        public string Deleted_by { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
       
    }
}