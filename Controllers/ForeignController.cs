using Microsoft.Ajax.Utilities;
using Open_Schooling.Helper;
using Open_Schooling.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Open_Schooling.Controllers
{
    public class ForeignController : Controller
    {
        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities();
        Common common = new Common();
        SqlConnection _Con;
        SqlCommand _Command;
        // GET: Foreign
        [HttpGet]
        public ActionResult Registration()
        {
            //string applicationId = "24SIOS1099SA";
            //common.RegistrationMail("Stateboardonline@gmail.com", "Registration form saved successfully for Maharashtra State Board of Open Schooling", " <html> <body> <span>        Your registration form has been Saved Successfully      </span>  <br>    You can login through given below credentials   <br> <b>Mobile No: </b> <br> <b> Email: </b> <br/> <a href='http://msbos.mh-ssc.ac.in/Foreign/PrintForm?Application_ID=" + applicationId + "'> Click Here to Download application Form</a></body> </html>");
            bindCountry();
            try
            {

                var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Registration").FirstOrDefault();

                if (page.Status == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }

            catch (Exception e)
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult Registration(Tbl_Registration model)
        {
            try
            {
                var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Registration").FirstOrDefault();

                if (page.Status == 0)
                {
                    return Json(new { Result = false, Response = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (ModelState.IsValid)
                    {

                        if (model.First_Name == null) { return Json(new { Result = false, Response = "Please EnterFirst_Name." }, JsonRequestBehavior.AllowGet); }
                        if (model.Mother_Name == null) { return Json(new { Result = false, Response = "Please Enter Mother_Name." }, JsonRequestBehavior.AllowGet); }
                        if (model.Mobile_No == null) { return Json(new { Result = false, Response = "Please Enter Mobile_No." }, JsonRequestBehavior.AllowGet); }
                        //if (model.Center == null) { return Json(new { Result = false, Response = "Please Enter Center." }, JsonRequestBehavior.AllowGet); }
                        if (model.Standard == null) { return Json(new { Result = false, Response = "Please Enter Standard." }, JsonRequestBehavior.AllowGet); }
                        if (model.Candidate_Handicaped_YN == null) { return Json(new { Result = false, Response = "Please Enter Candidate_Handicaped_YN." }, JsonRequestBehavior.AllowGet); }
                        if (model.AgeCertificate == null) { return Json(new { Result = false, Response = "Pleaseupload Age Certification Proof." }, JsonRequestBehavior.AllowGet); }
                        if (model.Upload_Photo == null) { return Json(new { Result = false, Response = "Please Upload_Photo." }, JsonRequestBehavior.AllowGet); }
                        if (model.Upload_Sign == null) { return Json(new { Result = false, Response = "Please Upload_sign." }, JsonRequestBehavior.AllowGet); }
                        //if (model.Previous_Attend_School_YN == null) { return Json(new { Result = false, Response = "Please select Previous_Attend_School_YN." }, JsonRequestBehavior.AllowGet); }
                        if (model.Email == null) { return Json(new { Result = false, Response = "Please Enter Email." }, JsonRequestBehavior.AllowGet); }
                        if (model.Gender == null) { return Json(new { Result = false, Response = "Please select Gender" }, JsonRequestBehavior.AllowGet); }
                       
                    

                        string hostName = Dns.GetHostName();
                        model.ip = Dns.GetHostByName(hostName).AddressList[0].ToString();
                        model.DateNow = DateTime.Now;
                        Random r = new Random();
                        int genRand = r.Next(1000, 9999);
                        Tbl_BMM_Center bmm_mode = db.Tbl_BMM_Center.Where(a => a.State == model.Foreign_State && a.Country == model.Country && a.Name == model.BMM_Center).FirstOrDefault();
                        int Reg_Count = db.Tbl_Registration.Where(a => a.Country != null).Count();
                        Reg_Count = 1000 + Reg_Count;
                        model.ApplicationId = Common.Get_Year_Id() + "SIOS" + Reg_Count + bmm_mode.State_Code;
                        model.ApplicationId = model.ApplicationId.Substring(0, 6) + 0 + model.ApplicationId.Substring(6 + 1);


                        //Random ram = new Random();
                        //int num = ram.Next(100000000, 999999999);

                        //if (model.order_id == null)
                        //{
                        //    model.order_id = Common.Get_Year_Id() + num;

                        //}

                        //string Snum = ram.Next(1000000, 9999999).ToString();
                        //if (model.Ref_ID == null)
                        //{
                        //    model.Ref_ID = Snum;
                        //}

                        var applicationId = model.ApplicationId;
                        TempData["USERNAME"] = applicationId;
                        Session["id"] = applicationId;
                        model.Year_Id = Common.Get_Year();

                        db.Tbl_Registration.Add(model);

                        db.SaveChanges();

                        var data = db.Tbl_Registration.Where(x => x.ApplicationId == model.ApplicationId).FirstOrDefault();
                        if (model.Upload_Photo != null || model.Upload_Sign != null || model.AgeCertificate != null || model.Upload_Disability_Document != null)
                        {
                            if (model.Candidate_Handicaped_YN.Equals("Yes"))
                            {
                                string file4 = Path.GetExtension(model.Upload_Disability_Document.FileName);

                                string Filename4 = data.ApplicationId + file4;
                                model.Disability_Doc_Proof = "../Uploads/Disability/" + Filename4;
                                model.Upload_Disability_Document.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Disability/"), Filename4));
                            }


                            string file = Path.GetExtension(model.Upload_Photo.FileName);

                            string Filename = data.ApplicationId + file;
                            model.Photo = "../Uploads/Photo/" + Filename;
                            model.Upload_Photo.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Photo/"), Filename));

                            string file1 = Path.GetExtension(model.Upload_Sign.FileName);

                            string Filename1 = data.ApplicationId + file1;
                            model.Signature = "../Uploads/Signature/" + Filename1;
                            model.Upload_Sign.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Signature/"), Filename1));

                            string file2 = Path.GetExtension(model.AgeCertificate.FileName);

                            string Filename2 = data.ApplicationId + file2;
                            model.Age_Certified_Proof = "../Uploads/AgeCertificate/" + Filename2;
                            model.AgeCertificate.SaveAs(Path.Combine(Server.MapPath("~/Uploads/AgeCertificate/"), Filename2));

                            if (model.Candidate_Handicaped_YN == "No")
                            {
                                model.Handicap = "N.A.";
                            }

                          //  FormsAuthentication.SetAuthCookie(model.ApplicationId, false);

                            /*var center_info = db.Tbl_BMM_Center.Where(x => x.Name == model.Center).FirstOrDefault();
                            model.Center_Code = center_info.State_Code;
                            var DivCode = db.Tbl_CenterList.Where(x => x.center_code == center_info).FirstOrDefault().div_code;
                            int id = model.Id;
                            string Enrollment_No = Common.Get_Year_Id() + "0" + DivCode.ToString() + (100000 + id).ToString();
                            model.Enrollment_No = Enrollment_No;*/

                            db.Tbl_Registration.Attach(model);
                            //db.Entry(model).Property(x => x.Center_Code).IsModified = true;
                            //db.Entry(model).Property(x => x.Enrollment_No).IsModified = true;
                            //db.Entry(model).Property(x => x.ApplicationId).IsModified = true;
                            db.Entry(model).Property(x => x.Handicap).IsModified = true;
                            db.Entry(model).Property(x => x.Photo).IsModified = true;
                            db.Entry(model).Property(x => x.Signature).IsModified = true;
                            db.Entry(model).Property(x => x.Disability_Doc_Proof).IsModified = true;
                            db.Entry(model).Property(x => x.Age_Certified_Proof).IsModified = true;
                            db.SaveChanges();
                            //TempData["Mobile_No"] = model.ApplicationId;
                            //Session["App_id"] = model.ApplicationId;
                            //var centerCode = db.Center_Login_Information.Where(x => x.Center_Name == model.Center).ToList();
                            //foreach (var item in centerCode)
                            //{
                            //    TempData["UDISE_No"] = item.UDISE_No;
                            //    TempData["Contact_Center_Code"] = item.Contact_Center_Code;
                            //}
                            common.RegistrationMail(model.Email, "Registration form saved successfully for Maharashtra State Board of Open Schooling", " <html> <body><span><b>Name: </b>" + model.First_Name+" "+model.Middle_Name+" "+model.Last_Name +" <br> </span><b>Center: </b></span>" + model.BMM_Center+"<br><span> <b>State: </b>"+ model.Foreign_State +" <br> <b> Country: </b>"+ model.Country+ " </span> <br>  <span> <b>Application Id: </b>" + applicationId+" </span><br><brs> You can login through given below credentials <br>><b> Mobile No: </b> "+ model.Mobile_No +" <br> <b> Email: </b> "+ model.Email+" <a href='http://msbos.mh-ssc.ac.in/Foreign/PrintForm?Application_ID=" + applicationId+ "'> <br> Click Here to Download application Form</a> </body> </html>");
                            // http://msbos.mh-ssc.ac.in/Foreign/PrintForm?Application_ID=" + applicationId + "
                            //return Json(new { Result = "Submitted", Message = "../dataFrom.htm?order=" + model.order_id + "&ref=" + model.Ref_ID + "&name=" + model.First_Name + "&adress=" + model.Address + "&city=" + model.Village + "&state=" + model.State + "&pin=" + model.Pin_Code + "&email=" + model.Email + "&mobile=" + model.Mobile_No + "&application=" + model.ApplicationId  }, JsonRequestBehavior.AllowGet);
                            /*+"&amount=" + model.Amount
*/
                            return Json(new { Result = true, Response = "Record Save Successfully",Message= "../Foreign/PrintForm?Application_ID="+ applicationId }, JsonRequestBehavior.AllowGet);
                        }
                        //TempData["Msg"] = "save successfully...!";
                    }
                    //return Json(new { Result = true, Response = "Record Sucessfully" }, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("PrintForm", "Foreign", new { Application_ID = model.ApplicationId });
                    return Json(new { Result = false, Response = "Failed" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Response = "Failed" + ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public void bindCountry()
        {
            try
            {
                var centerlist = db.Tbl_BMM_Center
                        .DistinctBy(c => c.Country)
                        .OrderBy(c => c.Country)
                        .ToList();
                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem { Text = "-Select Country-", Value = "0" });
                foreach (var m in centerlist)
                {
                    li.Add(new SelectListItem { Text = m.Country, Value = m.Country.ToString() });
                    ViewBag.country = li;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult getState(string id)
        {
            try
            {
                var stateList = db.Tbl_BMM_Center
                    .Where(s => s.Country.Equals(id))
                    .DistinctBy(s => s.State)
                    .OrderBy(s => s.State)
                    .Select(s => s.State)
                    .ToList();
                List<SelectListItem> licent = new List<SelectListItem>();
                licent.Add(new SelectListItem { Text = "-Select State-", Value = "0" });
                if (stateList != null)
                {
                    foreach (var x in stateList)
                    {
                        licent.Add(new SelectListItem { Text = x, Value = x.ToString() });
                    }
                }
                return Json(new SelectList(licent, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult getCenter(string id)
        {
            try
            {
                var centerList = db.Tbl_BMM_Center
                    .Where(x => x.State == id)
                    .OrderBy(c => c.Name)
                    .ToList();
                List<SelectListItem> licent = new List<SelectListItem>();
                licent.Add(new SelectListItem { Text = "-Select Center-", Value = "0" });
                if (centerList != null)
                {
                    foreach (var x in centerList)
                    {
                        licent.Add(new SelectListItem { Text = x.Name, Value = x.Name.ToString() });
                    }
                }
                return Json(new SelectList(licent, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult getCenterCode(string center)
        {
            try
            {
                var centerCode = db.Tbl_CenterList
                    .Where(x => x.center_name == center)
                    .FirstOrDefault();
                return Json(centerCode.center_code, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult getSubject(string id, string handicaped, Tbl_Registration subject)
        {
            try
            {
                subject.SubjectListA = db.Tbl_Subject.Where(x => x.Subject_Code == "AA_01" && x.Subject_Name == "Marathi" && x.Handicaped == handicaped).ToList<Tbl_Subject>();
                /* if (handicaped == "No" && id == "8th")
                 {
                     subject.SubjectListD = db.Tbl_Subject.Where(x => x.Class == id && x.Handicaped == handicaped && x.Subject_Group == "D").ToList<Tbl_Subject>();
                     var allList = new { SubjectListA = subject.SubjectListA, SubjectListB = subject.SubjectListB, SubjectListC = subject.SubjectListC, SubjectListD = subject.SubjectListD };
                     return Json(allList, JsonRequestBehavior.AllowGet);
                 }
                 else
                 {
                     var allList = new { SubjectListA = subject.SubjectListA, SubjectListB = subject.SubjectListB, SubjectListC = subject.SubjectListC };
                     return Json(allList, JsonRequestBehavior.AllowGet);
                 }*/
                return Json(new { SubjectListA = subject.SubjectListA }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult getNSQFSubject(string standard, Tbl_Registration subject)
        {
            try
            {
                subject.SubjectListD_NSQF = db.Tbl_Subject.Where(x => x.Class == standard && x.Subject_Group == "D-NSQF").ToList<Tbl_Subject>();
                return Json(subject.SubjectListD_NSQF, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PrintForm(String Application_ID)
        {
            //Tbl_Registration model = db.Tbl_Registration.Where(a => a.ApplicationId == "MSBOSPR08247066").FirstOrDefault();
            Tbl_Registration _model = db.Tbl_Registration.Where(a => a.ApplicationId == Application_ID).FirstOrDefault();
            return View(_model);
        }
    }/*
       public HttpPostedFileBase Upload_Disability_Document { get; set; }
        public HttpPostedFileBase Upload_Photo { get; set; }
        public HttpPostedFileBase Upload_Sign { get; set; }
        public HttpPostedFileBase AgeCertificate { get; set; }
        public List<string> SUBNSQF { get; set; }
        public List<string> SUB { get; set; }
        public List<Tbl_Subject> SubjectListA { get; set; }
        public List<Tbl_Subject> SubjectListB { get; set; }
        public List<Tbl_Subject> SubjectListC { get; set; }
        public List<Tbl_Subject> SubjectListD { get; set; }
        public List<Tbl_Subject> SubjectListD_NSQF { get; set; }
      */
}