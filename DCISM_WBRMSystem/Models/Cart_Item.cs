//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCISM_WBRMSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cart_Item
    {
        public int Id { get; set; }
        public Nullable<int> Id_Item { get; set; }
        public string Str_Code { get; set; }
        public int Id_Cart { get; set; }
        public string Item_Name { get; set; }
        public string Codition_Before_Released { get; set; }
        public string Remarks_for_Released { get; set; }
        public Nullable<System.DateTime> Date_Released { get; set; }
        public Nullable<bool> Is_Returned { get; set; }
        public Nullable<System.DateTime> Date_Returned { get; set; }
        public string Codition_After_Returned { get; set; }
        public string Remarks_for_Retrun { get; set; }
        public string Status { get; set; }
        public string Remarks_for_Declined { get; set; }
        public Nullable<System.DateTime> Date_Declined { get; set; }
        public Nullable<System.DateTime> Date_Approved { get; set; }
    }
}
