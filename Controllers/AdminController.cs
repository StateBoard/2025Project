using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Open_Schooling.Models;
using Open_Schooling.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Syncfusion.XlsIO;
using Newtonsoft.Json;
using System.Web.Security;
using OfficeOpenXml;
using System.Data;

namespace Open_Schooling.Controllers
{
    public class AdminController : Controller
    {
        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities();
        Common common = new Common();


        // GET: Admin
        public ActionResult Admin_Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Admin_Login(string Username, string Password)
        {
            try
            {
                if (Username == "1" && Password == "1")
                {
                    return RedirectToAction("Admin_Details", "Admin");
                }
                else
                {
                    TempData["ErrorMsg"] = "Invalid Username or Password.";
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Admin_Login", "Admin");
            }

            return View();
        }

        public ActionResult Admin_Details()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Get_PageInfo()
        {

            try
            {
                var page_info = db.Tbl_PageStatus.ToList();
                return Json(new { Result = true, Response = page_info }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult Get_MsgInfo()
        {

            try
            {
                var msg_info = db.Tbl_Admin.ToList();
                return Json(new { Result = true, Response = msg_info }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult MSG_Delete(int id)
        {
            var record = db.Tbl_Admin.Where(x => x.Id == id).FirstOrDefault();
            db.Tbl_Admin.Remove(record);
            db.SaveChanges();
            return Json(new { Result = true, Response = "Massage Record Deleted." }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EditStatus(int id)
        {
            try
            {
                var item = db.Tbl_PageStatus.Find(id);
                if (item.Status == 1)
                {
                    item.Status = 0;
                }
                else
                {
                    item.Status = 1;
                }
              
                db.SaveChanges();

                return Json(new { Result = true, Response = "Status Updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = " Unable to Change Status" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult EditMsgStatus(int id)
        {
            try
            {
                var item = db.Tbl_Admin.Find(id);
                if (item.MsgStatus == 1)
                {
                    item.MsgStatus = 0;
                }
                else
                {
                    item.MsgStatus = 1;
                }
                
                db.SaveChanges();

                return Json(new { Result = true, Response = "Status Updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = " Unable to Change Status" }, JsonRequestBehavior.AllowGet);
            }
        }
        

        

        [HttpPost]
         public ActionResult Save_Message_To_DB(string msg)
        {
            try
            {
                var a = db.Tbl_Admin.ToList();
                Tbl_Admin m = new Tbl_Admin();
                m.Message = msg;
                m.MsgStatus = 0;
                db.Tbl_Admin.Add(m);
                db.SaveChanges();

                return Json(new { Result = true, Response = " record seved successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = " Unable to save information" }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public JsonResult Download_Registration_Data()
        {
            string fileName = "", excel = "";
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                //List<Tbl_Registration> model = db.Tbl_Registration.ToList();
                string reg= "select ApplicationId,(First_Name+' '+Middle_Name+' '+Last_Name)as Name,Standard,Center_Name,B.Division as Division,Center_Code,Subject1,Subject2,Subject3,Subject4,Subject5,Nsqf_Subject from Tbl_Registration,Tbl_payment,Center_Login_Information AS B where Tbl_Registration.ApplicationId = Tbl_payment.merchant_param1 and Tbl_payment.order_status = 'Success' AND Tbl_Registration.Center_Code = B.Contact_Center_Code order by Standard";
                
                List<excel_reg_model> model = db.Database.SqlQuery<excel_reg_model>(reg).ToList();


                excel = "registration";

                DataTable dt = common.ToDataTable(model);
                dt.TableName = "Registration_Table";
                fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "Registration_Data";
                common.CreateExcelFile(dt, fileName, excel);

                return Json(new { Result = true, response= model, FileName = fileName });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Download_Application_Data()
        {
            string fileName = "", excel = "";
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                List<Tbl_Application_Form> model = db.Tbl_Application_Form.ToList();

                excel = "application";
               
                DataTable dt = common.ToDataTable(model);
                dt.TableName = "Application_Table";
                fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "Application_Data";
                common.CreateExcelFile(dt, fileName, excel);

                return Json(new { Result = true, response = model, FileName = fileName });
            }
            catch (Exception e)
            {
                throw;
               
            }
        }

        [HttpPost]
        public JsonResult Get_Payment_failed_record()
               {

            try
            {
                
                string count = "";
                count = " select * from Tbl_payment b   join Tbl_Registration a on a.Payment_Status is null and b.order_status != 'Success' and a.ApplicationId = b.merchant_param1 ";
                List<CenterViewModel> model = db.Database.SqlQuery<CenterViewModel>(count).ToList();

                return Json(new { Result = true, Response = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }

        }

       


    }
}