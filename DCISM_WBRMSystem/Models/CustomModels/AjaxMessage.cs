using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class AjaxMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }

    public class UploadStatus
    {
        public int Success { get; set; }
        public List<FailedUpload> Faileds { get; set; }
        public string Message { get; set; }
        public int TotalData { get; set; }
    }

    public class FailedUpload
    {
        public int Row { get; set; }
        public string ErrorMessage  { get; set; }
    }
}