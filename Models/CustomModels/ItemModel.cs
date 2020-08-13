using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class ItemModel
    {

    }

    public class ItemUpdateModel
    {
        public string Id_Item { get; set; }
        public string Item_Name { get; set; }
        public string Serial_Number { get; set; }
        public string Category { get; set; }
        public string Brand_Model { get; set; }
        public string Custodian { get; set; }
        public string Price { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public string On_Hold_Period { get; set; }
        public string Date_Acquisition { get; set; }
        public string Remarks { get; set; }

    }
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Room_No { get; set; }
        public string Category { get; set; }
        public string Item_Name { get; set; }
        public string Brand_Model { get; set; }
        public string Date_Acquired { get; set; }
        public string Remarks { get; set; }
    }

    public class ItemToSaveModel
    {
        public string Category { get; set; }
        public string ItemName { get; set; }
        public string Model_Brand { get; set; }
        public string Date_Acquisition { get; set; }
        public string Condition { get; set; }
        public string Serial_No { get; set; }
    }

    public class MultipleItemToSaveModel
    {
        public int Id { get; set; }
        public List<ItemToSaveModel> Items { get; set; }
    }
}