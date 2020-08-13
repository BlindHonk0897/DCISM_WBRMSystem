using DCISM_WBRMSystem.Infrastructure;
using DCISM_WBRMSystem.Models;
using DCISM_WBRMSystem.Models.CustomModels;
using IronBarCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    [CustomAuthenticationFilter]
    public class ItemsController : Controller
    {
        // GET: Items
        [CustomAuthorize("Administrator")]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize("Administrator")]
        public ActionResult Items()
        {
            return View();
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public JsonResult AjaxSaveItem(ItemToSaveModel model)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            using(WBRMSystemEntities database = new WBRMSystemEntities())
            {
                
                Item it = new Item()
                {
                    Item_Name = model.ItemName,
                    Category = model.Category,
                    Date_Added = DateTime.Now,
                    Condition = model.Condition,
                    Date_Acquisition = Convert.ToDateTime(model.Date_Acquisition),
                    Serial_Number = model.Serial_No,
                    Brand_Model = model.Model_Brand,
                    Status = model.Condition =="Functional"?"Available":"",
                    Is_Assign = false,
                    Is_Verify = false
                };
                var exist = database.Items.FirstOrDefault(i => i.Item_Name == model.ItemName
                    && i.Brand_Model == model.Model_Brand && i.Category == model.Category &&
                    i.Category == model.Category && i.Serial_Number == model.Serial_No);

                if (exist == null)
                {


                    database.Items.Add(it);
                    database.SaveChanges();
                    /// CODE TO UPDATE Item Barcode
                    database.update_Barcode_sp();
                    Generate_Barcodes_For_All_NotGenerated_Items();
                    ajm.Message = "success";
                }
                else
                {
                    ajm.Message = "Item have the same record in the database.Please check it first.";
                }
            }
            return Json(ajm);
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public JsonResult AjaxSaveMultipleItem(MultipleItemToSaveModel model)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                foreach(var item in model.Items)
                {
                    Item it = new Item()
                    {
                        Item_Name = item.ItemName,
                        Category = item.Category,
                        Date_Added = DateTime.Now,
                        Condition = item.Condition,
                        Date_Acquisition = Convert.ToDateTime(item.Date_Acquisition),
                        Serial_Number = item.Serial_No,
                        Brand_Model = item.Model_Brand,
                        Status = item.Condition == "Functional" ? "Available" : "",
                        Is_Assign = false,
                        Is_Verify = false
                    };
                    database.Items.Add(it);
                    database.SaveChanges();
                    /// CODE TO UPDATE Item Barcode
                    database.update_Barcode_sp();
                    Generate_Barcodes_For_All_NotGenerated_Items();
                    ajm.Message = "success";
                }
               
            }
            return Json(ajm);
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public JsonResult AjaxAssignOrReAssignItem(string id ,string room,string action)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            var idItem = Convert.ToInt32(id);
            var user = Session["Username"] != null ? Session["Username"].ToString() : "";
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                database.Ass_sp(idItem, room, user, action);
                ajm.Message = "success";
            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult DeleteItem(DeleteItemModel model)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var DateNow = DateTime.Now;
                var ID = Convert.ToInt32(model.Id);
                var item = database.Items.FirstOrDefault(i => i.Id_Item == ID);
                var user = Session["Username"] != null ? Session["Username"].ToString() : "";
                database.Delete_Item_sp(ID, user, model.Description, model.Reason);
                
                if (item != null) {

                    Item_History his = new Item_History();
                    his.Id_Item = item.Id_Item;
                    his.Str_Code = item.Str_Code;
                    his.Action = "Delete";
                    his.Action_Date = DateNow;
                    his.Action_History_Desc = "Item is deleted";
                    his.Done_By = user;
                    database.Item_History.Add(his);
                    database.SaveChanges();

                    // Delete Barcode image 
                    //string path = Server.MapPath("~/Barcodes/" + item.Str_Code + ".jpg");
                    //Debug.WriteLine(Directory.Exists(Server.MapPath("~/Barcodes/")));
                    //if (Directory.Exists(path))
                    //{
                    //    Directory.Delete(path,true);
                    //}

                    string filePath = Server.MapPath("~/Barcodes/" + item.Str_Code + ".jpg");
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                }
                ajm.Message = "success";
                // DELETE Barcode Here - image 
            }

            return Json(ajm);
        }

        [CustomAuthorize("Administrator")]
        [HttpPost]
        public JsonResult UpdateItem(ItemUpdateModel model)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            using(WBRMSystemEntities database = new WBRMSystemEntities())
            {
                try
                {
                    var user = Session["Username"] != null ? Session["Username"].ToString() : "";
                    int Id = Convert.ToInt32(model.Id_Item);
                    Decimal Price =model.Price!=null? Convert.ToDecimal(model.Price):0;
                    int On_Hold_Period = model.On_Hold_Period!=null? Convert.ToInt32(model.On_Hold_Period):0;
                    DateTime Date_Acquisition = model.Date_Acquisition!=null? Convert.ToDateTime(model.Date_Acquisition):new DateTime();
                    string Custodian =model.Custodian!=null?model.Custodian:"";
                    string Remarks = model.Remarks!=null?model.Remarks:"";
                    database.Update_Item_sp(Id, model.Item_Name, model.Serial_Number, model.Category, model.Brand_Model, Custodian, Price, model.Condition, model.Status,
                    On_Hold_Period, Date_Acquisition, Remarks, user);
                    ajm.Message = "success";
                }catch(Exception ex)
                {
                    ajm.Message = ex.Message;
                    Debug.WriteLine(ex);
                }
            }
            return Json(ajm);
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public JsonResult CheckSerialExist(string serial)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "no"
            };
            using(WBRMSystemEntities database = new WBRMSystemEntities())
            {
                ajm.Message = database.Items.FirstOrDefault(i => i.Serial_Number == serial) != null ? "yes" : "no";
            }
            return Json(ajm);
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedFile)
        {
            if (Request.Files.Count > 0)
            {
                postedFile = Request.Files[0];
            }

            DateTime now = DateTime.Now;
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                    default:
                        return Content("File not Matched.");
                }

                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();

                            Debug.WriteLine("Row: " + dt.Rows.Count);
                            foreach (DataRow row in dt.Rows)
                            {
                                using (WBRMSystemEntities database = new WBRMSystemEntities())
                                {
                                    Item item = new Item()
                                    {
                                        Item_Name = row["Item Name"].ToString().Trim(),
                                        Brand_Model = row["Brand/Model"].ToString().Trim(),
                                        Serial_Number = row["Serial Number"].ToString().Trim(),
                                        Year_Acquired = (row["Date Acquired"].ToString().Trim()),
                                        Custodian = row["Custodian"].ToString().Trim(),
                                        Remarks = row["Remarks"].ToString().Trim(),
                                        Room_No = row["Room Number"].ToString().Trim(),
                                        Category = row["Category"].ToString().Trim(),
                                        Id_Barcode = 0,
                                        Id_Category = 0,
                                        Id_Condition = 0,
                                        Id_Status = 0,
                                        Id_SubCategory = 0,
                                        Date_Added = DateTime.Now
                                    };
                                    database.Items.Add(item);
                                    database.SaveChanges();
                                }
                                Debug.WriteLine("[ ");

                                Debug.Write("Item Name: " + row["Item Name"].ToString() + "\n");
                                Debug.Write("Brand/Model: " + row["Brand/Model"].ToString() + "\n");
                                Debug.Write("Serial Number: " + row["Serial Number"].ToString() + "\n");
                                Debug.Write("Date Acquired: " + row["Date Acquired"].ToString() + "\n");
                                Debug.Write("Custodian: " + row["Custodian"].ToString() + "\n");
                                Debug.Write("Remarks: " + row["Remarks"].ToString() + "\n");
                                Debug.Write("Room No.: " + row["Room Number"].ToString() + "\n");
                                Debug.Write("Category: " + row["Category"].ToString() + "\n");
                                Debug.Write("Qty: " + row["Qty"].ToString() + "\n");
                                Debug.WriteLine("[ ");
                            }
                        }
                    }
                }
            }
            else
            {
                return Content("File not Matched.");
            }
            return Content("Success");
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public ActionResult UploadFiles()
        {
            //List<ExcelDetails> customers = new List<ExcelDetails>();
            int TotalSuccessLoaded = 0;
            int TotalFailed = 0;
            int TotalData = 0;

            UploadStatus upStatus = new UploadStatus()
            {
                Message = "failed",
                Faileds = new List<FailedUpload>(),
                Success = 0,
                TotalData = 0
            };

            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    // =============================== FOR UPLOADING FILES ======================= //
                    //for (int i = 0; i < files.Count; i++)
                    //{
                    //    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    //    HttpPostedFileBase file = files[i];
                    //    string fname;

                    //    // Checking for Internet Explorer  
                    //    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    //    {
                    //        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    //        fname = testfiles[testfiles.Length - 1];
                    //    }
                    //    else
                    //    {
                    //        fname = file.FileName;
                    //    }

                    //    // Get the complete folder path and store the file inside it.  
                    //   fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                    //   file.SaveAs(fname);
                    //}
                    // =============================== END FOR UPLOADING FILES ======================= //

                    HttpPostedFileBase postedFile = files[0];
                    DateTime now = DateTime.Now;
                    string filePath = string.Empty;
                    if (postedFile != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(postedFile.FileName);
                        string extension = Path.GetExtension(postedFile.FileName);
                        postedFile.SaveAs(filePath);

                        string conString = string.Empty;
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                                break;
                            default:
                                upStatus.Message= "File not Matched";
                                return Json(upStatus);
                        }

                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    DataTable dt = new DataTable();
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();

                                    Debug.WriteLine("Row: " + dt.Rows.Count);
                                    upStatus.TotalData = dt.Rows.Count;
                                    if (dt.Columns.Count >= 7)
                                    {
                                        if(dt.Columns["Item Name"]!=null && dt.Columns["Brand/Model"] != null && dt.Columns["Serial Number"] != null
                                            &&dt.Columns["Date Acquisation"] != null && dt.Columns["Custodian"] != null && dt.Columns["Condition"] != null && dt.Columns["Remarks"] != null
                                            && dt.Columns["Room Number"] != null && dt.Columns["Category"] != null)
                                        {
                                            var ro = 1;
                                            foreach (DataRow row in dt.Rows)
                                            {
                                                using (WBRMSystemEntities database = new WBRMSystemEntities())
                                                {
                                                    try
                                                    {
                                                        Item item = new Item()
                                                        {
                                                            Item_Name = row["Item Name"].ToString().Trim(),
                                                            Brand_Model = row["Brand/Model"].ToString().Trim(),
                                                            Serial_Number = row["Serial Number"] != null && row["Serial Number"].ToString() != "" ? row["Serial Number"].ToString().Trim().ToUpper() : "NONE",
                                                            Year_Acquired = row["Date Acquisation"].ToString()!="" ? (row["Date Acquisation"].ToString().Trim().Length > 4? Convert.ToDateTime(row["Date Acquisation"].ToString()).Year.ToString(): row["Date Acquisation"].ToString()) :DateTime.Now.Year.ToString(),
                                                            Date_Acquisition = row["Date Acquisation"].ToString()!=""?Convert.ToDateTime(row["Date Acquisation"].ToString()):DateTime.Now,
                                                            Custodian = row["Custodian"].ToString().Trim(),
                                                            Condition = row["Condition"].ToString().Trim(),
                                                            Remarks = row["Remarks"].ToString().Trim(),
                                                            Room_No = row["Room Number"].ToString().Trim(),
                                                            Category = row["Category"].ToString()!=""?row["Category"].ToString().Trim().ToUpper():"",
                                                            Id_Barcode = 0,
                                                            Id_Category = 0,
                                                            Id_Condition = 0,
                                                            Id_Status = 0,
                                                            Status = row["Condition"].ToString().Trim() == "Functional" ? "Available" : "",
                                                            Id_SubCategory = 0,
                                                            Date_Added = DateTime.Now,
                                                            Is_Verify = false
                                                        };

                                                        

                                                        var room_no = row["Room Number"].ToString().Trim();
                                                        // If naa nay data sa room uncomment this
                                                        //var room_exist = database.Rooms.FirstOrDefault(r => r.Room_No == room_no);
                                                        if (room_no != null && room_no != "")
                                                        {
                                                            item.Is_Assign = true;
                                                        }
                                                        else
                                                        {
                                                            item.Is_Assign = false;
                                                        }

                                                        if (database.Items.FirstOrDefault(i => i.Item_Name == item.Item_Name && i.Brand_Model == item.Brand_Model && i.Category == item.Category &&
                                                           i.Category == item.Category && i.Room_No == item.Room_No && i.Serial_Number == item.Serial_Number) == null)
                                                        {
                                                            database.Items.Add(item);
                                                            database.SaveChanges();
                                                            upStatus.Success++;
                                                            database.update_Barcode_sp();
                                                        }
                                                        else
                                                        {
                                                            upStatus.Faileds.Add(new FailedUpload()
                                                            {
                                                                Row = ro,
                                                                ErrorMessage = "item already exist"
                                                            });
                                                        }

                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        upStatus.Faileds.Add(new FailedUpload()
                                                        {
                                                            Row = ro,
                                                            ErrorMessage = ex.Message
                                                        });
                                                        continue;
                                                    }
                                                    ro++;
                                                }
                                                Debug.WriteLine("[ ");
                                                Debug.Write("Item Name: " + row["Item Name"].ToString() + "\n");
                                                Debug.Write("Brand/Model: " + row["Brand/Model"].ToString() + "\n");
                                                Debug.Write("Serial Number: " + row["Serial Number"].ToString() + "\n");
                                                Debug.Write("Date Acquired: " + row["Date Acquisation"].ToString() + "\n");
                                                Debug.Write("Custodian: " + row["Custodian"].ToString() + "\n");
                                                Debug.Write("Condition: " + row["Condition"].ToString() + "\n");
                                                Debug.Write("Remarks: " + row["Remarks"].ToString() + "\n");
                                                Debug.Write("Room No.: " + row["Room Number"].ToString() + "\n");
                                                Debug.Write("Category: " + row["Category"].ToString() + "\n");
                                               
                                                Debug.WriteLine("[ ");
                                            }
                                            upStatus.Message = "Success";
                                            Generate_Barcodes_For_All_NotGenerated_Items();
                                            return Json(upStatus);
                                        }
                                        else
                                        {
                                            upStatus.Message = "File not in proper format!";
                                            return Json(upStatus);
                                        }

                                    }
                                    else
                                    {
                                        upStatus.Message = "File not in proper format!";
                                        return Json(upStatus);
                                    }
                                    
                                }
                            }
                        }
                    }
                    else
                    {
                        upStatus.Message = "File not Matched.!";
                        return Json(upStatus);                       
                    }
                    // Returns message that successfully uploaded  
                   // return Json("SUCCESS");
                }
                catch (Exception ex)
                {
                    upStatus.Message = "Error occurred. Error details: " + ex.Message;
                    return Json(upStatus);
                }
            }
            else
            {
                upStatus.Message = "No files selected.";
                return Json(upStatus);
            }
        }

        [CustomAuthorize("Administrator")]
        public FileResult DownloadFormat()
        {
            string path = "/File/ItemsToLoadFormat.xlsx";
            return File(path, "application/vnd.ms-excel", "Format.xlsx");
        }


        [CustomAuthorize("Administrator")]
        public JsonResult UpdateIsVerifiedField(string strcode)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };

            using(WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = Session["Username"] != null ? Session["Username"].ToString() : "";
                Item item = database.Items.FirstOrDefault(it => it.Str_Code == strcode);
                if (item != null)
                {
                    database.Update_IsVerifiedField_sp(strcode, user);
                    ajm.Message = "success";
                }
            }
    
            return Json(ajm);
        }

        public void Generate_Barcodes_For_All_NotGenerated_Items()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var items = database.Items.Where(it => it.Is_Generated_Barcode == false || it.Is_Generated_Barcode == null).OrderByDescending(it => it.Id_Item).ToList();
                foreach (var it in items)
                {
                    // check here if already generated barcoden else generate
                    BarcodeWriter.CreateBarcode(it.Str_Code, BarcodeWriterEncoding.Code128, 250, 50).SetMargins(1,1,1,1).SaveAsJpeg(Server.MapPath("~/Barcodes/" + it.Str_Code + ".jpg"));
                    //BarcodeWriter.CreateBarcode(it.Str_Code, BarcodeWriterEncoding.Code128).ResizeTo(250, 30).SetMargins(1).AddAnnotationTextBelowBarcode(it.Str_Code).SaveAsJpeg(Server.MapPath("~/Barcodes/" + it.Str_Code + ".jpg"));

                }
                database.Update_IsGenerated_sp();
            }
        }


        [CustomAuthorize("Administrator")]
        [HttpPost]
        public JsonResult GenerateBarcode(string roomno, string category)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            
            GeneratedBarcodesModel model = new GeneratedBarcodesModel();
            using(WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var items = database.Items.Where(r => r.Room_No == roomno && r.Category == category).ToList();
                model.Paths = new List<string>();
                foreach(var data in items)
                {
                    var barcode = (data.Id_Item+"-"+data.Category.Substring(0,3) +"-"+data.Room_No);
                    model.Paths.Add(roomno + "/" + category + "/" + barcode + ".png");
                    BarcodeWriter.CreateBarcode(barcode, BarcodeWriterEncoding.Code128,259,179).SaveAsJpeg(Server.MapPath("~/Barcodes/"+roomno+"/"+category+"/"+barcode+".png"));
                }
                model.Message = "success";
            }
            

            return Json(model);
        }

        [CustomAuthorize("Administrator")]
        public ActionResult Barcodes()
        {
            System.Console.WriteLine("Success");
            /******     WRITE     *******/
            //HttpContext.Current.Server.MapPath("~/images/barcode.jpg")
            // Create A Barcode in 1 Line of Code
            BarcodeWriter.CreateBarcode("37437MAKA", BarcodeWriterEncoding.Code128).SaveAsJpeg(Server.MapPath("~/Uploads/barcode1.jpg"));
            /******    READ    *******/
            // Read A Barcode in 1 Line of Code.  Gets Text, Numeric Codes, Binary Data and an Image of the barcode
            BarcodeResult Result = BarcodeReader.QuicklyReadOneBarcode(Server.MapPath("~/Uploads/barcode.jpg"));

            // Assert that IronBarCode Works :-)
            if (Result != null && Result.Text == "37437MAKA")
            {
                Debug.WriteLine("Success");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult DynamicBarcodePdfGenerator()
        {
            using(WBRMSystemEntities database = new WBRMSystemEntities())
            {
                
                BarcodeToPDFModel model = new BarcodeToPDFModel();
                var items = database.Items.OrderByDescending(i => i.Id_Item).ToList();
                //BarcodeCategory bc = new BarcodeCategory();
                //bc.Category = "MOUSE";
                model.barcodeDetails = (from pd in items where pd.Category == "MOUSE"
                         select new BarcodeDetails()
                         {
                             Path = "~/Barcodes/"+pd.Str_Code+".jpg",
                             Category = pd.Category,
                             Strcode = pd.Str_Code,
                             FileName = pd.Str_Code+ ".jpg",
                             Extension = "jpg"
                         }).Take(45).ToList();
               
                return new Rotativa.PartialViewAsPdf("_DynamicPdfBarcodeImage", model)
                {
                    FileName = "MOUSE-Devices.pdf",
                    PageWidth = 90

                };
            }

        }


        [CustomAuthorize("Administrator")]
        [HttpGet]
        public ActionResult DownloadPDF(int batch , string optiondownload)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                BarcodeToPDFModel model = new BarcodeToPDFModel();
               
                var indexStart = (batch - 1)*45;
                var indexEnd = (batch)*45;
                
                if (optiondownload == "Not Verified Items") {
                    var items = database.Items.ToList();
                    items = items.Where(i => i.Is_Verify == false || i.Is_Verify == null).ToList();
                    var exceed = Convert.ToInt32(items.Count() / 45) + 1;
                    if (items.Count >= 45)
                    {
                        if (batch == exceed)
                        {
                            BarcodeDetails bd;
                            for (; indexStart < items.Count(); indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = items.ElementAt(indexStart).Category,
                                    Strcode = items.ElementAt(indexStart).Str_Code,
                                    FileName = items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                        else
                        {
                            BarcodeDetails bd;
                            for (; indexStart < indexEnd; indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = items.ElementAt(indexStart).Category,
                                    Strcode = items.ElementAt(indexStart).Str_Code,
                                    FileName = items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                    }
                    else
                    {
                        BarcodeDetails bd;
                        for (; indexStart < items.Count(); indexStart++)
                        {
                            bd = new BarcodeDetails()
                            {
                                Path = "~/Barcodes/" + items.ElementAt(indexStart).Str_Code + ".jpg",
                                Category = items.ElementAt(indexStart).Category,
                                Strcode = items.ElementAt(indexStart).Str_Code,
                                FileName = items.ElementAt(indexStart).Str_Code + ".jpg",
                                Extension = "jpg"
                            };
                            model.barcodeDetails.Add(bd);
                        }
                    }
                    
                    return new Rotativa.PartialViewAsPdf("_DynamicPdfBarcodeImage", model)
                    {
                        FileName = ordinal_suffix_of(batch)+" ("+optiondownload+").pdf",
                        PageWidth = 90

                    };
                }
                else if(optiondownload == "Added Today")
                {
                   var addedToday = database.added_today_vw.ToList();
                   var exceed = Convert.ToInt32(addedToday.Count() / 45) + 1;
                    if (addedToday.Count >= 45)
                    {
                        if (batch == exceed)
                        {
                            BarcodeDetails bd;
                            for (; indexStart < addedToday.Count(); indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = addedToday.ElementAt(indexStart).Category,
                                    Strcode = addedToday.ElementAt(indexStart).Str_Code,
                                    FileName = addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                        else
                        {
                            BarcodeDetails bd;
                            for (; indexStart < indexEnd; indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = addedToday.ElementAt(indexStart).Category,
                                    Strcode = addedToday.ElementAt(indexStart).Str_Code,
                                    FileName = addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                    }
                    else
                    {
                        BarcodeDetails bd;
                        for (; indexStart < addedToday.Count(); indexStart++)
                        {
                            bd = new BarcodeDetails()
                            {
                                Path = "~/Barcodes/" + addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                Category = addedToday.ElementAt(indexStart).Category,
                                Strcode = addedToday.ElementAt(indexStart).Str_Code,
                                FileName = addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                Extension = "jpg"
                            };
                            model.barcodeDetails.Add(bd);
                        }
                    }


                    return new Rotativa.PartialViewAsPdf("_DynamicPdfBarcodeImage", model)
                    {
                        FileName = ordinal_suffix_of(batch) + " (" + optiondownload + ").pdf",
                        PageWidth = 90

                    };
                }
                else
                {
                    return null;
                }
            }

        }

        public ActionResult AjaxGetBarcodesToPrint(int batch ,string optiondownload)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                BarcodeToPDFModel model = new BarcodeToPDFModel();

                var indexStart = (batch - 1) * 45;
                var indexEnd = (batch) * 45;

                if (optiondownload == "Not Verified Items")
                {
                    var items = database.Items.ToList();
                    items = items.Where(i => i.Is_Verify == false || i.Is_Verify == null).ToList();
                    var exceed = Convert.ToInt32(items.Count() / 45) + 1;
                    if (items.Count >= 45)
                    {
                        if (batch == exceed)
                        {
                            BarcodeDetails bd;
                            for (; indexStart < items.Count(); indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = items.ElementAt(indexStart).Category,
                                    Strcode = items.ElementAt(indexStart).Str_Code,
                                    FileName = items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                        else
                        {
                            BarcodeDetails bd;
                            for (; indexStart < indexEnd; indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = items.ElementAt(indexStart).Category,
                                    Strcode = items.ElementAt(indexStart).Str_Code,
                                    FileName = items.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                    }
                    else
                    {
                        BarcodeDetails bd;
                        for (; indexStart < items.Count(); indexStart++)
                        {
                            bd = new BarcodeDetails()
                            {
                                Path = "~/Barcodes/" + items.ElementAt(indexStart).Str_Code + ".jpg",
                                Category = items.ElementAt(indexStart).Category,
                                Strcode = items.ElementAt(indexStart).Str_Code,
                                FileName = items.ElementAt(indexStart).Str_Code + ".jpg",
                                Extension = "jpg"
                            };
                            model.barcodeDetails.Add(bd);
                        }
                    }

                    

                    return Json(model);
                }
                else if (optiondownload == "Added Today")
                {
                    var addedToday = database.added_today_vw.ToList();
                    var exceed = Convert.ToInt32(addedToday.Count() / 45) + 1;
                    if (addedToday.Count >= 45)
                    {
                        if (batch == exceed)
                        {
                            BarcodeDetails bd;
                            for (; indexStart < addedToday.Count(); indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = addedToday.ElementAt(indexStart).Category,
                                    Strcode = addedToday.ElementAt(indexStart).Str_Code,
                                    FileName = addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                        else
                        {
                            BarcodeDetails bd;
                            for (; indexStart < indexEnd; indexStart++)
                            {
                                bd = new BarcodeDetails()
                                {
                                    Path = "~/Barcodes/" + addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Category = addedToday.ElementAt(indexStart).Category,
                                    Strcode = addedToday.ElementAt(indexStart).Str_Code,
                                    FileName = addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                    Extension = "jpg"
                                };
                                model.barcodeDetails.Add(bd);
                            }
                        }
                    }
                    else
                    {
                        BarcodeDetails bd;
                        for (; indexStart < addedToday.Count(); indexStart++)
                        {
                            bd = new BarcodeDetails()
                            {
                                Path = "~/Barcodes/" + addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                Category = addedToday.ElementAt(indexStart).Category,
                                Strcode = addedToday.ElementAt(indexStart).Str_Code,
                                FileName = addedToday.ElementAt(indexStart).Str_Code + ".jpg",
                                Extension = "jpg"
                            };
                            model.barcodeDetails.Add(bd);
                        }
                    }


                    return Json(model);
                }
                else
                {
                    return null;
                }
            }
        }

        public string ordinal_suffix_of(int i)
        {
            var j = i % 10;
            var k = i % 100;
            if (j == 1 && k != 11)
            {
                return i + "st";
            }
            if (j == 2 && k != 12)
            {
                return i + "nd";
            }
            if (j == 3 && k != 13)
            {
                return i + "rd";
            }
            return i + "th";
        }
    }
}