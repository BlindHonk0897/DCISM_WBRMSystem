using IronBarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class CodeGenerator
    {
        public int Generate_Four_Digit_Random()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

    }

}