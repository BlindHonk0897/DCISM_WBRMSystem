using ClosedXML.Excel;
using DCISM_WBRMSystem.Infrastructure;
using DCISM_WBRMSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using Syncfusion.XlsIO;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    [CustomAuthenticationFilter]
    public class ReportController : Controller
    {
        // GET: Report
        [CustomAuthorize("Administrator")]
        public ActionResult Reports()
        {
            return View();
        }
        [CustomAuthorize("Administrator")]
        public FileResult Download()
        {
            string path = "/File/Report.xlsx";
            return File(path, "application/vnd.ms-excel", "Report.xlsx");
        }

        [CustomAuthorize("Administrator")]
        public FileResult DamagedItems_Excel()
        {
            DateTime date = DateTime.Now;
            var today = (date.ToString("MMMM dd, yyyy") + ".");
            var todayNumeric = date.ToShortDateString();

            DataTable dt = new DataTable("Damaged Items");

            dt.Columns.Add(new DataColumn("Item Name"));
            dt.Columns.Add(new DataColumn("Brand/Model"));
            dt.Columns.Add(new DataColumn("Serial Number"));
            dt.Columns.Add(new DataColumn("Barcode"));
            dt.Columns.Add(new DataColumn("Date Aquisition"));
            dt.Columns.Add(new DataColumn("Custodian"));
            dt.Columns.Add(new DataColumn("Room Number"));
            dt.Columns.Add(new DataColumn("Category"));
            dt.Columns.Add(new DataColumn("Condition"));
            dt.Columns.Add(new DataColumn("Remarks"));

            using (WBRMSystemEntities db = new WBRMSystemEntities())
            {
                var damaged_Items = db.damaged_items_vw.ToList();
                foreach (var i in damaged_Items)
                {
                    dt.Rows.Add(i.Item_Name, i.Brand_Model, i.Serial_Number, i.Str_Code, i.Date_Acquisition, i.Custodian, i.Room_No, i.Category, i.Condition, i.Remarks);
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                //wb.Worksheets.Add(progress);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("dlc", cookieValue));

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report-" + today + "( Damaged Items ).xlsx");
                }
            }

        }

        [CustomAuthorize("Administrator")]
        public ActionResult DamagedItems_PDF()
        {
            DateTime date = DateTime.Now;
            var today = (date.ToString("MMMM dd, yyyy") + ".");
            var todayNumeric = date.ToShortDateString();
            using (WBRMSystemEntities db = new WBRMSystemEntities())
            {
                var damaged_items = db.damaged_items_vw.ToList();
                return new Rotativa.PartialViewAsPdf("_DamagedItemsPDF", damaged_items)
                {
                    FileName = "Report-" + todayNumeric + " (Damaged Items).pdf",
                    PageWidth = 90,
                    CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
            }

        }

        [CustomAuthorize("Administrator")]
        public FileResult BorrowedItems_Excel()
        {
            DateTime date = DateTime.Now;
            var today = (date.ToString("MMMM dd, yyyy") + ".");
            var todayNumeric = date.ToShortDateString();

            DataTable dt = new DataTable("Borrowed Items");

            dt.Columns.Add(new DataColumn("Item Name"));
            dt.Columns.Add(new DataColumn("Brand/Model"));
            dt.Columns.Add(new DataColumn("Serial Number"));
            dt.Columns.Add(new DataColumn("Barcode"));
            dt.Columns.Add(new DataColumn("Date Aquisition"));
            dt.Columns.Add(new DataColumn("Custodian"));
            dt.Columns.Add(new DataColumn("Room Number"));
            dt.Columns.Add(new DataColumn("Category"));
            dt.Columns.Add(new DataColumn("Condition"));
            dt.Columns.Add(new DataColumn("Remarks"));
            dt.Columns.Add(new DataColumn("Status"));

            using (WBRMSystemEntities db = new WBRMSystemEntities())
            {
                var borrowed_Items = db.borrowed_items_vw.ToList();
                foreach (var i in borrowed_Items)
                {
                    dt.Rows.Add(i.Item_Name, i.Brand_Model, i.Serial_Number, i.Str_Code, i.Date_Acquisition, i.Custodian, i.Room_No, i.Category, i.Condition, i.Remarks, i.Status);
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                //wb.Worksheets.Add(progress);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("dlc", cookieValue));

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report-" + today + "( Borrowed Items ).xlsx");
                }
            }
        }

        [CustomAuthorize("Administrator")]
        public ActionResult BorrowedItems_PDF()
        {
            DateTime date = DateTime.Now;
            var today = (date.ToString("MMMM dd, yyyy") + ".");
            var todayNumeric = date.ToShortDateString();
            using (WBRMSystemEntities db = new WBRMSystemEntities())
            {
                var borrowed_items = db.borrowed_items_vw.ToList();
                return new Rotativa.PartialViewAsPdf("_BorrowedItemsPDF", borrowed_items)
                {
                    FileName = "Report-" + todayNumeric + " (Borrowed Items).pdf",
                    PageWidth = 90,
                    CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
            }
        }

        public FileResult InventoryOfItems()
        {
            ReadyInventory();
            string[] selectedfiles = Directory.GetFiles(
                            Server.MapPath("~/file/temp_inventory"));
            if (System.IO.File.Exists(Server.MapPath
                                         ("~/zipfiles/Inventory.zip")))
            {
                System.IO.File.Delete(Server.MapPath
                              ("~/zipfiles/Inventory.zip"));
            }
            ZipArchive zip = ZipFile.Open(Server.MapPath
                     ("~/zipfiles/Inventory.zip"), ZipArchiveMode.Create);
            foreach (string file in selectedfiles)
            {
                zip.CreateEntryFromFile(Server.MapPath
                     ("~/file/temp_inventory/" + Path.GetFileName(file)), Path.GetFileName(file));
            }
            zip.Dispose();
            return File(Server.MapPath("~/zipfiles/Inventory.zip"),
                      "application/zip", "Inventory.zip");

        }

        public void ReadyInventory()
        {
            string path = Server.MapPath("~/file/Inventory_Format.xls");
            string pathSave = Server.MapPath("~/file/temp_inventory");

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                List<string> list_Rooms = new List<string>();
                int count_Category = 0;
                List<item_vw> list_items = new List<item_vw>();
                using(WBRMSystemEntities  database = new WBRMSystemEntities())
                {
                    list_Rooms = database.Items.ToList().Select(i => i.Room_No).Distinct().ToList();
                    count_Category = database.ItemCategories.ToList().Distinct().Count();
                    list_items = database.item_vw.ToList();
                }
                foreach (var el in list_Rooms)
                {
                    var items = list_items.Where(i => i.Room_No == el).ToList();
                    //Initialize application
                    IApplication application = excelEngine.Excel;

                    //Open existing workbook with data entered
                    //Assembly assembly = typeof(Object).GetTypeInfo().Assembly;
                    //Stream fileStream = assembly.GetManifestResourceStream("FormatCard.xlsx");


                    // Path.GetFileName(path.FileName);
                    IWorkbook workbook = application.Workbooks.Open(path, ExcelOpenType.Automatic);
                    IWorksheet worksheet0 = workbook.Worksheets[0];

                    worksheet0.Name = "MOUSE";
                    worksheet0.Range["E11"].Text = el;
                    worksheet0.Range["K10"].Text = DateTime.Now.ToShortDateString();
                    for (int y = 0; y < count_Category - 1; y++)
                    {
                        workbook.Worksheets.AddCopy(worksheet0);
                    }

                    var mouses = items.Where(i => i.Category == "MOUSE").ToList();
                    var keyboard = items.Where(i => i.Category == "KEYBOARD").ToList();
                    var cpus = items.Where(i => i.Category == "CPU").ToList();
                    var monitors = items.Where(i => i.Category == "MONITOR").ToList();
                    var extras = items.Where(i => i.Category == "EXTRAS").ToList();

                    // for mouses
                    if (mouses.Count > 25)
                    {
                        worksheet0.InsertRow(39, mouses.Count - 25);
                    }
                    for(int i = 0; i < mouses.Count-25; i++)
                    {
                        worksheet0.Range["C16:S16"].CopyTo(worksheet0.Range["C"+(39+i)+":S"+ (39 + i)]);
                    }
                    for (int i = 0;i< mouses.Count();i++)
                    {
                        worksheet0.Range["D" + (15 + i)].Text= mouses.ElementAt(i).Item_Name;
                        worksheet0.Range["F" + (15 + i)].Text= mouses.ElementAt(i).Brand_Model;
                        worksheet0.Range["I" + (15 + i)].Text= mouses.ElementAt(i).Str_Code;
                        worksheet0.Range["J" + (15 + i)].Text= mouses.ElementAt(i).Serial_Number;
                        worksheet0.Range["K" + (15 + i)].Text= mouses.ElementAt(i).Date_Acquisition;
                        worksheet0.Range["L" + (15 + i)].Text= mouses.ElementAt(i).Custodian;
                        worksheet0.Range["Q" + (15 + i)].Text= mouses.ElementAt(i).Condition;
                    }

                    IWorksheet worksheet1 = workbook.Worksheets[1];
                    IWorksheet worksheet2 = workbook.Worksheets[2];
                    IWorksheet worksheet3 = workbook.Worksheets[3];
                    IWorksheet worksheet4 = workbook.Worksheets[4];

                    worksheet1.Name = "KEYBOARD";
                    worksheet2.Name = "CPU";
                    worksheet3.Name = "MONITOR";
                    worksheet4.Name = "EXTRAS";

                    // for Keyboards
                    if (keyboard.Count > 25)
                    {
                        worksheet1.InsertRow(39, keyboard.Count - 25);
                    }
                    for (int i = 0; i < keyboard.Count - 25; i++)
                    {
                        worksheet1.Range["C16:S16"].CopyTo(worksheet1.Range["C" + (39 + i) + ":S" + (39 + i)]);
                    }
                    for (int i = 0; i < keyboard.Count(); i++)
                    {
                        worksheet1.Range["D" + (15 + i)].Text = keyboard.ElementAt(i).Item_Name;
                        worksheet1.Range["F" + (15 + i)].Text = keyboard.ElementAt(i).Brand_Model;
                        worksheet1.Range["I" + (15 + i)].Text = keyboard.ElementAt(i).Str_Code;
                        worksheet1.Range["J" + (15 + i)].Text = keyboard.ElementAt(i).Serial_Number;
                        worksheet1.Range["K" + (15 + i)].Text = keyboard.ElementAt(i).Date_Acquisition;
                        worksheet1.Range["L" + (15 + i)].Text = keyboard.ElementAt(i).Custodian;
                        worksheet1.Range["Q" + (15 + i)].Text = keyboard.ElementAt(i).Condition;
                    }

                    // for cpus
                    if (cpus.Count > 25)
                    {
                        worksheet2.InsertRow(39, cpus.Count - 25);
                    }
                    for (int i = 0; i < cpus.Count - 25; i++)
                    {
                        worksheet2.Range["C16:S16"].CopyTo(worksheet2.Range["C" + (39 + i) + ":S" + (39 + i)]);
                    }
                    for (int i = 0; i < cpus.Count(); i++)
                    {
                        worksheet2.Range["D" + (15 + i)].Text = cpus.ElementAt(i).Item_Name;
                        worksheet2.Range["F" + (15 + i)].Text = cpus.ElementAt(i).Brand_Model;
                        worksheet2.Range["I" + (15 + i)].Text = cpus.ElementAt(i).Str_Code;
                        worksheet2.Range["J" + (15 + i)].Text = cpus.ElementAt(i).Serial_Number;
                        worksheet2.Range["K" + (15 + i)].Text = cpus.ElementAt(i).Date_Acquisition;
                        worksheet2.Range["L" + (15 + i)].Text = cpus.ElementAt(i).Custodian;
                        worksheet2.Range["Q" + (15 + i)].Text = cpus.ElementAt(i).Condition;
                    }


                    // for monitors
                    if (monitors.Count > 25)
                    {
                        worksheet3.InsertRow(39, monitors.Count - 25);
                    }
                    for (int i = 0; i < monitors.Count - 25; i++)
                    {
                        worksheet3.Range["C16:S16"].CopyTo(worksheet3.Range["C" + (39 + i) + ":S" + (39 + i)]);
                    }
                    for (int i = 0; i < monitors.Count(); i++)
                    {
                        worksheet3.Range["D" + (15 + i)].Text = monitors.ElementAt(i).Item_Name;
                        worksheet3.Range["F" + (15 + i)].Text = monitors.ElementAt(i).Brand_Model;
                        worksheet3.Range["I" + (15 + i)].Text = monitors.ElementAt(i).Str_Code;
                        worksheet3.Range["J" + (15 + i)].Text = monitors.ElementAt(i).Serial_Number;
                        worksheet3.Range["K" + (15 + i)].Text = monitors.ElementAt(i).Date_Acquisition;
                        worksheet3.Range["L" + (15 + i)].Text = monitors.ElementAt(i).Custodian;
                        worksheet3.Range["Q" + (15 + i)].Text = monitors.ElementAt(i).Condition;
                    }

                    // for extras
                    if (extras.Count > 25)
                    {
                        worksheet4.InsertRow(39, extras.Count - 25);
                    }
                    for (int i = 0; i < extras.Count - 25; i++)
                    {
                        worksheet4.Range["C16:S16"].CopyTo(worksheet4.Range["C" + (39 + i) + ":S" + (39 + i)]);
                    }
                    for (int i = 0; i < extras.Count(); i++)
                    {
                        worksheet4.Range["D" + (15 + i)].Text = extras.ElementAt(i).Item_Name;
                        worksheet4.Range["F" + (15 + i)].Text = extras.ElementAt(i).Brand_Model;
                        worksheet4.Range["I" + (15 + i)].Text = extras.ElementAt(i).Str_Code;
                        worksheet4.Range["J" + (15 + i)].Text = extras.ElementAt(i).Serial_Number;
                        worksheet4.Range["K" + (15 + i)].Text = extras.ElementAt(i).Date_Acquisition;
                        worksheet4.Range["L" + (15 + i)].Text = extras.ElementAt(i).Custodian;
                        worksheet4.Range["Q" + (15 + i)].Text = extras.ElementAt(i).Condition;
                    }
                    workbook.SaveAs(Path.Combine(pathSave, el+" (" + items.Count() + " items).xlsx"));
                }
                

            }

        }
    }
}