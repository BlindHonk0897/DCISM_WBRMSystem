using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class GeneratedBarcodesModel
    {
        public int Id { get; set; }
        public List<string> Paths { get; set; }
        public string Message { get; set; }
    }
}