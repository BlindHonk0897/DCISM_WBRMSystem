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
    
    public partial class Inventory
    {
        public int Id { get; set; }
        public int Id_Transaction { get; set; }
        public int Id_Category { get; set; }
        public Nullable<int> Id_SubCategory { get; set; }
        public Nullable<int> No_Of_Stock { get; set; }
        public Nullable<int> No_Of_Damaged { get; set; }
        public Nullable<int> No_Of_Borrowed { get; set; }
    }
}