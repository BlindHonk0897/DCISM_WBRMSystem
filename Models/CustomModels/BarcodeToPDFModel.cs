using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class BarcodeToPDFModel
    {
        public List<BarcodeDetails> barcodeDetails { get; set; }
        public BarcodeToPDFModel()
        {
            barcodeDetails = new List<BarcodeDetails>();
        }
    }

    //public class BarcodeCategory
    //{
    //    public string Category { get; set; }
    //    public List<BarcodeDetails> barcodeDetails { get; set; }
    //}

    public class BarcodeDetails
    {
        public string Path { get; set; }
        public string Category { get; set; }
        public string Strcode { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}