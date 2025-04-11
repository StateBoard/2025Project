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
    public class HomeController : Controller
    {
        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities(); 
         Common common = new Common();
        SqlConnection _Con;
        SqlCommand _Command;

        // Home Page
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Get_Msg_Info()
        {

            try
            {
                var msg_info = db.Tbl_Admin.Where(x => x.MsgStatus == 1).ToList();
                return Json(new { Result = true, Response = msg_info }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }

        }


        //Student Login GET
        public ActionResult Student_Login()
        {

            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Student_Login").FirstOrDefault();

            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public ActionResult Student_Login(Tbl_Registration tbl_Registration)
        {
            //Student Login Active/ inactive code

            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Student_Login").FirstOrDefault();


            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Check credentials

                Tbl_payment tbl_Payment = new Tbl_payment();
                var tbl_reg = db.Tbl_Registration.Where(x => x.Mobile_No == tbl_Registration.Mobile_No && x.Email == tbl_Registration.Email && x.Payment_Status == "1"/*&&x.Ec_Status=="Completed"*/).FirstOrDefault();
                tbl_Payment = db.Tbl_payment.Where(x => x.billing_name == tbl_reg.First_Name && x.billing_email == tbl_reg.Email && x.order_status == "Success").FirstOrDefault();

                var center_info = db.Center_Login_Information.Where(x => x.Center_Name == tbl_reg.Center).FirstOrDefault();

                var Full_Name = tbl_reg.First_Name + " " + tbl_reg.Middle_Name + " " + tbl_reg.Last_Name;
                TempData["Full_Name"] = Full_Name;

                if (tbl_Payment != null)
                {
                    return RedirectToAction("Student_DashBoard", "Home", tbl_reg);
                }
                else
                {
                    return View();
                }
            }
        }

        //Student Dashboard GET
        public ActionResult Student_DashBoard(Student_Model model)
        {
            try
            {
                var page2 = db.Tbl_PageStatus.Where(x => x.Pagename == "EnrollmentCertificate").FirstOrDefault();
                ViewBag.s2 = page2.Status;

                List<Student_Model> StudentViewModel = new List<Student_Model>();


                StudentViewModel = (from A in db.Tbl_Registration

                                    join
                                          B in db.Tbl_payment
                                    on
                                    A.ApplicationId equals B.merchant_param1
                                    join
                                    C in db.Center_Login_Information
                                    on
                                    A.Center_Code equals C.Contact_Center_Code
                                    where A.ApplicationId.Trim() == model.ApplicationId.ToString() && A.Payment_Status == "1"
                                    select new Student_Model
                                    {
                                        Payment_Status = A.Payment_Status,
                                        ApplicationId = A.ApplicationId,
                                        Name = A.First_Name,
                                        Mobile_No = A.Mobile_No,
                                        Division = C.Division,
                                        Taluka = C.Taluka,
                                        Ec_Status = A.Ec_Status,
                                        EC_No = A.Enrollment_No,
                                    }).ToList();

                return View(StudentViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Application Print
        public ActionResult RePrintForm(Student_Model model)
        {

            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "RePrintForm").FirstOrDefault();

            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Tbl_Registration _Model = db.Tbl_Registration.Where(x => x.ApplicationId == model.ApplicationId).FirstOrDefault();

                string[] str = _Model.Date_of_Birth.Split('-');
                string da = (str[2] + '/' + str[1] + '/' + str[0]);

                TempData["BirthDate"] = da;
                string extension = Path.GetExtension(_Model.Photo.Trim());
                if (extension == ".jpg" || extension == ".JPG" || extension == ".jpeg")
                {
                    ViewData["Photo"] = "/Uploads/Photo/" + _Model.ApplicationId + extension;
                    ViewData["Sign"] = "/Uploads/Signature/" + _Model.ApplicationId + extension;
                }
                else
                {
                    ViewData["Photo"] = _Model.Photo;
                    ViewData["Sign"] = _Model.Signature;

                }

                List<Student_Model> StudentViewModel = new List<Student_Model>();

                StudentViewModel = (from A in db.Tbl_Registration

                                    join
                                          B in db.Tbl_payment
                                    on
                                    A.ApplicationId equals B.merchant_param1
                                    join
                                    C in db.Center_Login_Information
                                    on
                                    A.Center_Code equals C.Contact_Center_Code
                                    where A.ApplicationId.Trim() == model.ApplicationId.ToString() && A.Payment_Status == "1"
                                    select new Student_Model
                                    {
                                        Payment_Status = A.Payment_Status,
                                        ApplicationId = A.ApplicationId,
                                        Name = A.First_Name,
                                        Mobile_No = A.Mobile_No,
                                        Division = C.Division,
                                        Adhar_no = A.Adhar_no,
                                        Address = A.Address,
                                        Standard = A.Standard,
                                        UDISE_No = C.UDISE_No,
                                        Date_of_Birth = A.Date_of_Birth,
                                        Center_Code = A.Center_Code,
                                        Last_Name = A.Last_Name,
                                        Middle_Name = A.Middle_Name,
                                        First_Name = A.First_Name,
                                        Mother_Name = A.Mother_Name,
                                        Village = A.Village,
                                        Pin_Code = A.Pin_Code,
                                        Email = A.Email,
                                        DOB_Village = A.DOB_Village,
                                        Gender = A.Gender,
                                        Minority_Religion = A.Minority_Religion,
                                        Handicap = A.Handicap,
                                        Subject_List = A.Subject_List,
                                        Center = A.Center,
                                        amount_money = B.amount_money,
                                        card_name = B.card_name,
                                        trans_date = B.trans_date,
                                        tracking_id = B.tracking_id,
                                        bank_ref_no = B.bank_ref_no,
                                        Photo = A.Photo,
                                        Signature = A.Signature,

                                    }).ToList();

                return View(StudentViewModel);

            }
        }

        public ActionResult ForgotStudentPassword()
        {
            return View();
        }
        public JsonResult getStudentPassword(string mobileNo)
        {
            try
            {
                var password = db.Tbl_Registration.Where(x => x.Mobile_No == mobileNo).FirstOrDefault();
                if (password == null)
                {
                    string Error = "Invalid Mobile Number.";
                    return Json(Error, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(password.Email, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult Registration()

        {
            bindDistrict();

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
        [Obsolete]
        public ActionResult Registration(Tbl_Registration model)
        {
            try
            {
                var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Registration").FirstOrDefault();

                if (page.Status == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (ModelState.IsValid)
                    {

                        if (model.First_Name == null) { return Json(new { Result = false, Response = "Please EnterFirst_Name." }, JsonRequestBehavior.AllowGet); }
                        if (model.Mother_Name == null) { return Json(new { Result = false, Response = "Please Enter Mother_Name." }, JsonRequestBehavior.AllowGet); }
                        if (model.Mobile_No == null) { return Json(new { Result = false, Response = "Please Enter Mobile_No." }, JsonRequestBehavior.AllowGet); }
                        if (model.Center == null) { return Json(new { Result = false, Response = "Please Enter Center." }, JsonRequestBehavior.AllowGet); }
                        if (model.Standard == null) { return Json(new { Result = false, Response = "Please Enter Standard." }, JsonRequestBehavior.AllowGet); }
                        if (model.Candidate_Handicaped_YN == null) { return Json(new { Result = false, Response = "Please Enter Candidate_Handicaped_YN." }, JsonRequestBehavior.AllowGet); }
                        if (model.AgeCertificate == null) { return Json(new { Result = false, Response = "Pleaseupload Age Certification Proof." }, JsonRequestBehavior.AllowGet); }
                        if (model.Upload_Photo == null) { return Json(new { Result = false, Response = "Please Upload_Photo." }, JsonRequestBehavior.AllowGet); }
                        if (model.Upload_Sign == null) { return Json(new { Result = false, Response = "Please Upload_sign." }, JsonRequestBehavior.AllowGet); }
                        if (model.Previous_Attend_School_YN == null) { return Json(new { Result = false, Response = "Please select Previous_Attend_School_YN." }, JsonRequestBehavior.AllowGet); }
                        if (model.Email == null) { return Json(new { Result = false, Response = "Please Enter Email." }, JsonRequestBehavior.AllowGet); }
                        if (model.Gender == null) { return Json(new { Result = false, Response = "Please select Gender" }, JsonRequestBehavior.AllowGet); }

                        if (model.SUBNSQF == null)
                        {
                            var comman1 = model.SUB.ToList();
                            int commanCount1 = comman1.Count;
                            Tbl_Subject obj = new Tbl_Subject();
                            if (commanCount1 == 5)
                            {
                                model.Subject1 = comman1[0].ToString();
                                model.Subject2 = comman1[1].ToString();
                                model.Subject3 = comman1[2].ToString();
                                model.Subject4 = comman1[3].ToString();
                                model.Subject5 = comman1[4].ToString();
                                model.Subject_List = model.Subject1 + ", " + model.Subject2 + ", " + model.Subject3 + ", " + model.Subject4 + ", " + model.Subject5;
                                var sub1 = "";
                                var sub2 = "";
                                var sub3 = "";
                                var sub4 = "";
                                var sub5 = "";

                                Tbl_Subject[] tbl_Subjects = common.Get_Nsqf_Sub(model);
                                sub1 = tbl_Subjects[0].Subject_Name + "(" + tbl_Subjects[0].Subject_Code + ")";
                                sub2 = tbl_Subjects[1].Subject_Name + "(" + tbl_Subjects[1].Subject_Code + ")";
                                sub3 = tbl_Subjects[2].Subject_Name + "(" + tbl_Subjects[2].Subject_Code + ")";
                                sub4 = tbl_Subjects[3].Subject_Name + "(" + tbl_Subjects[3].Subject_Code + ")";
                                sub5 = tbl_Subjects[4].Subject_Name + "(" + tbl_Subjects[4].Subject_Code + ")";

                                TempData["Subject_List"] = sub1 + " , " + sub2 + " , " + sub3 + " , " + sub4 + " , " + sub5;

                                model.Subject_List = TempData["Subject_List"].ToString();
                            }
                            else
                            {

                                return Json(new { success = false, message = "You have to select only 5 subjects from available groups" }, JsonRequestBehavior.AllowGet);

                            }
                        }
                        else
                        {
                            var comman = model.SUB.ToList();
                            var nsqfSub = model.SUBNSQF.ToList();
                            int nsqfsubCount = nsqfSub.Count;
                            int commanCount = comman.Count;
                            var totalSubject = commanCount + nsqfsubCount;

                            var sub1 = "";
                            var sub2 = "";
                            var sub3 = "";
                            var sub4 = "";
                            var sub5 = "";

                            Tbl_Subject[] tbl_Subjects = common.Get_Nsqf_Sub(model);
                            sub1 = tbl_Subjects[0].Subject_Name + "(" + tbl_Subjects[0].Subject_Code + ")";
                            sub2 = tbl_Subjects[1].Subject_Name + "(" + tbl_Subjects[1].Subject_Code + ")";
                            sub3 = tbl_Subjects[2].Subject_Name + "(" + tbl_Subjects[2].Subject_Code + ")";
                            sub4 = tbl_Subjects[3].Subject_Name + "(" + tbl_Subjects[3].Subject_Code + ")";
                            sub5 = tbl_Subjects[4].Subject_Name + "(" + tbl_Subjects[4].Subject_Code + ")";
                            TempData["Subject_List"] = sub1 + " , " + sub2 + " , " + sub3 + " , " + sub4 + " , " + sub5;

                            model.Subject_List = TempData["Subject_List"].ToString();
                            if (totalSubject == 5)
                            {
                                if (nsqfsubCount <= 2)
                                {
                                    if (nsqfSub.Count == 1)
                                    {
                                        model.Nsqf_Subject = nsqfSub[0].ToString();

                                        model.Subject1 = comman[0].ToString();
                                        model.Subject2 = comman[1].ToString();
                                        model.Subject3 = comman[2].ToString();
                                        model.Subject4 = comman[3].ToString();
                                        //model.Subject_List = model.Subject1 + ", " + model.Subject2 + ", " + model.Subject3 + ", " + model.Subject4;
                                        model.Subject_List = TempData["Subject_List"].ToString();
                                    }
                                    else
                                    {
                                        var NSQF_Subject1 = nsqfSub[0].ToString();
                                        var NSQF_Subject2 = nsqfSub[1].ToString();
                                        model.Nsqf_Subject = NSQF_Subject1 + " " + NSQF_Subject2;

                                        model.Subject1 = comman[0].ToString();
                                        model.Subject2 = comman[1].ToString();
                                        model.Subject3 = comman[2].ToString();
                                        //model.Subject_List = model.Subject1 + ", " + model.Subject2 + ", " + model.Subject3;
                                        model.Subject_List = TempData["Subject_List"].ToString();
                                    }
                                }
                                else
                                {
                                    return Json(new { success = false, message = "Select Only 1 or 2 Subject from NSQF Subject" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { success = false, message = "Select only 5 Subject in Group A,B,C,D." }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        string hostName = Dns.GetHostName();
                        model.ip = Dns.GetHostByName(hostName).AddressList[0].ToString();
                        model.DateNow = DateTime.Now;
                        Random r = new Random();
                        int genRand = r.Next(1000, 9999);
                        if (model.Standard == "5th")
                        {

                            model.ApplicationId = "MSBOSPR05" + Common.Get_Year_Id() + genRand;
                        }
                        else
                        {
                            model.ApplicationId = "MSBOSPR08" + Common.Get_Year_Id() + genRand;
                        }

                        Random ram = new Random();
                        int num = ram.Next(100000000, 999999999);
                        if (model.order_id == null)
                        {
                            model.order_id = Common.Get_Year_Id() + num;

                        }

                        string Snum = ram.Next(1000000, 9999999).ToString();
                        if (model.Ref_ID == null)
                        {
                            model.Ref_ID = Snum;
                        }
                        var applicationId = model.ApplicationId;
                        TempData["USERNAME"] = applicationId;
                        Session["id"] = applicationId;
                        model.Year_Id = Common.Get_Year();

                        db.Tbl_Registration.Add(model);

                        db.SaveChanges();

                        var data = db.Tbl_Registration.Where(x => x.ApplicationId == model.ApplicationId).FirstOrDefault();
                        if (model.Upload_Photo != null || model.Upload_Sign != null || model.AgeCertificate != null)
                        {
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

                            FormsAuthentication.SetAuthCookie(model.ApplicationId, false);

                            var center_info = db.Center_Login_Information.Where(x => x.Center_Name == model.Center).FirstOrDefault().Contact_Center_Code;
                            model.Center_Code = center_info;
                            var DivCode = db.Tbl_CenterList.Where(x => x.center_code == center_info).FirstOrDefault().div_code;
                            int id = model.Id;
                            string Enrollment_No = Common.Get_Year_Id() + "0" + DivCode.ToString() + (100000 + id).ToString();
                            model.Enrollment_No = Enrollment_No;

                            db.Tbl_Registration.Attach(model);
                            db.Entry(model).Property(x => x.Center_Code).IsModified = true;
                            db.Entry(model).Property(x => x.Enrollment_No).IsModified = true;
                            db.Entry(model).Property(x => x.ApplicationId).IsModified = true;
                            db.Entry(model).Property(x => x.Handicap).IsModified = true;
                            db.Entry(model).Property(x => x.Photo).IsModified = true;
                            db.Entry(model).Property(x => x.Signature).IsModified = true;
                            db.Entry(model).Property(x => x.Age_Certified_Proof).IsModified = true;
                            db.SaveChanges();
                            //TempData["Mobile_No"] = model.ApplicationId;
                            Session["App_id"] = model.ApplicationId;
                            var centerCode = db.Center_Login_Information.Where(x => x.Center_Name == model.Center).ToList();
                            foreach (var item in centerCode)
                            {
                                TempData["UDISE_No"] = item.UDISE_No;
                                TempData["Contact_Center_Code"] = item.Contact_Center_Code;
                            }

                            return Json(new { Result = "Submitted", Message = "../dataFrom.htm?order=" + model.order_id + "&ref=" + model.Ref_ID + "&name=" + model.First_Name + "&adress=" + model.Address + "&city=" + model.Village + "&state=" + model.State + "&pin=" + model.Pin_Code + "&email=" + model.Email + "&mobile=" + model.Mobile_No + "&application=" + model.ApplicationId + " " }, JsonRequestBehavior.AllowGet);
                        }
                        //TempData["Msg"] = "save successfully...!";
                    }
                    return Json(new { Result = true, Response = "Record Sucessfully" }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Response = "Failed" + ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintForm(CenterViewModel model)

        {

            
            List<CenterViewModel> CenterViewModel = new List<CenterViewModel>();
            Tbl_payment tbl_Payment = new Tbl_payment();
            Tbl_Registration _Model = db.Tbl_Registration.Where(x => x.ApplicationId == model.ApplicationId).FirstOrDefault();
                string[] str = _Model.Date_of_Birth.Split('-');
                string da = (str[2] + '/' + str[1] + '/' + str[0]);

                TempData["BirthDate"] = da;
                tbl_Payment = db.Tbl_payment.Where(x => x.billing_name == _Model.First_Name && x.billing_email == _Model.Email).FirstOrDefault();

                tbl_Payment.merchant_param1 = model.ApplicationId;
                db.Tbl_payment.Attach(tbl_Payment);
                db.Entry(tbl_Payment).Property(x => x.merchant_param1).IsModified = true;
            /*if (_Model.Country == null && _Model.State == null)
            {*/
                if (_Model.Id != 0)
                {
                    _Model.Payment_Status = "1";
                }
                else
                {
                    _Model.Payment_Status = "0";
                }

                CenterViewModel = (from A in db.Tbl_Registration

                                   join
                                         B in db.Tbl_payment
                                   on
                                   A.ApplicationId equals B.merchant_param1
                                   join
                                   C in db.Center_Login_Information
                                   on
                                   A.Center_Code equals C.Contact_Center_Code
                                   where A.ApplicationId.Trim() == model.ApplicationId.ToString() && A.Payment_Status == "1"
                                   select new CenterViewModel
                                   {
                                       Payment_Status = B.order_status,
                                       ApplicationId = A.ApplicationId,
                                       Name = A.First_Name,
                                       Mobile_No = A.Mobile_No,
                                       Division = C.Division,
                                       Adhar_no = A.Adhar_no,
                                       Address = A.Address,
                                       Standard = A.Standard,
                                       UDISE_No1 = C.UDISE_No,
                                       Date_of_Birth = A.Date_of_Birth,
                                       Center_Code = A.Center_Code,
                                       Last_Name = A.Last_Name,
                                       Middle_Name = A.Middle_Name,
                                       First_Name = A.First_Name,
                                       Mother_Name = A.Mother_Name,
                                       Village = A.Village,
                                       Pin_Code = A.Pin_Code,
                                       Email = A.Email,
                                       DOB_Village = A.DOB_Village,
                                       Gender = A.Gender,
                                       Minority_Religion = A.Minority_Religion,
                                       Handicap = A.Handicap,
                                       Subject_List = A.Subject_List,
                                       Center = A.Center,
                                       amount_money = B.amount_money,
                                       card_name = B.card_name,
                                       trans_date = B.trans_date,
                                       tracking_id = B.tracking_id,
                                       bank_ref_no = B.bank_ref_no,
                                       Photo = A.Photo,
                                       Signature = A.Signature,

                                   }).ToList();

                return View(CenterViewModel);

            //}
  /*          else {
                CenterViewModel = (from A in db.Tbl_Registration

                                   join
                                         B in db.Tbl_payment
                                   on
                                   A.ApplicationId equals B.merchant_param1
                                   join
                                   C in db.Center_Login_Information
                                   on
                                   A.Center_Code equals C.Contact_Center_Code
                                   where A.ApplicationId.Trim() == model.ApplicationId.ToString() && A.Payment_Status == "1"
                                   select new CenterViewModel
                                   {
                                       ApplicationId = A.ApplicationId,
                                       Name = A.First_Name,
                                       Mobile_No = A.Mobile_No,
                                       Adhar_no = A.Adhar_no,
                                       Address = A.Address,
                                       Standard = A.Standard,
                                       UDISE_No1 = C.UDISE_No,
                                       Date_of_Birth = A.Date_of_Birth,
                                       Center_Code = A.Center_Code,
                                       Last_Name = A.Last_Name,
                                       Middle_Name = A.Middle_Name,
                                       First_Name = A.First_Name,
                                       Mother_Name = A.Mother_Name,
                                       Village = A.Village,
                                       Pin_Code = A.Pin_Code,
                                       Email = A.Email,
                                       DOB_Village = A.DOB_Village,
                                       Gender = A.Gender,
                                       Minority_Religion = A.Minority_Religion,
                                       Handicap = A.Handicap,
                                       Center = A.BMM_Center,
                                       Photo = A.Photo,
                                       Signature = A.Signature,

                                   }).ToList();

                return View(CenterViewModel);

            }*/
        }

        [HttpGet]
        public ActionResult Application_Form(Tbl_Registration ExamFormModel)
        {
            var List = db.Tbl_PageStatus.ToList();

            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Application_Form").FirstOrDefault();

            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                Tbl_Registration tbl_Registration = new Tbl_Registration();
                var UserName = ExamFormModel.ApplicationId;
                tbl_Registration = db.Tbl_Registration.Where(os => os.ApplicationId == UserName).FirstOrDefault();
                Tbl_Subject tbl_Subject1 = new Tbl_Subject();
                Tbl_Subject tbl_Subject2 = new Tbl_Subject();
                Tbl_Subject tbl_Subject3 = new Tbl_Subject();
                Tbl_Subject tbl_Subject4 = new Tbl_Subject();
                Tbl_Subject tbl_Subject5 = new Tbl_Subject();

                Tbl_Subject[] tbl_Subjects = common.Get_Nsqf_Sub(tbl_Registration);

                var examForm = new CenterViewModel
                {
                    tbl_Registration = db.Tbl_Registration.Where(os => os.ApplicationId == UserName).FirstOrDefault(),
                    Center_Login = db.Center_Login_Information.Where(c => c.Contact_Center_Code == tbl_Registration.Center_Code).FirstOrDefault(),
                    Subject1 = tbl_Subjects[0].Subject_Name + "(" + tbl_Subjects[0].Subject_Code + ")",
                    Subject2 = tbl_Subjects[1].Subject_Name + "(" + tbl_Subjects[1].Subject_Code + ")",
                    Subject3 = tbl_Subjects[2].Subject_Name + "(" + tbl_Subjects[2].Subject_Code + ")",
                    Subject4 = tbl_Subjects[3].Subject_Name + "(" + tbl_Subjects[3].Subject_Code + ")",
                    Subject5 = tbl_Subjects[4].Subject_Name + "(" + tbl_Subjects[4].Subject_Code + ")",
                };
                string extension = Path.GetExtension(tbl_Registration.Photo.Trim());

                return View(examForm);
            }
        }
        public JsonResult Save_Application_Form(CenterViewModel model)
        {
            CenterViewModel Model = new CenterViewModel();

            try
            {
                if (ModelState.IsValid)
                {
                    Tbl_Application_Form obj = new Tbl_Application_Form();
                    Tbl_Registration tbl_Registration = db.Tbl_Registration.Where(x => x.ApplicationId == model.tbl_Registration.ApplicationId).FirstOrDefault();
                    obj.Form_No = tbl_Registration.ApplicationId;
                    obj.UDISE_No = model.Center_Login.UDISE_No.ToString();
                    obj.Contact_center = tbl_Registration.Center_Code;
                    obj.Last_name = tbl_Registration.Last_Name;
                    obj.Name = tbl_Registration.First_Name;
                    obj.Middle_name = tbl_Registration.Middle_Name;
                    obj.Mother_name = tbl_Registration.Mother_Name;
                    obj.Address = tbl_Registration.Address;
                    obj.Pincode = tbl_Registration.Pin_Code;
                    obj.Mobile_no = tbl_Registration.Mobile_No;
                    obj.Place_of_birth = tbl_Registration.DOB_Village;
                    obj.Adhar_no = tbl_Registration.Adhar_no;
                    obj.Gender = tbl_Registration.Gender;
                    obj.Minority_Religion = tbl_Registration.Minority_Religion;
                    obj.Divyang = tbl_Registration.Handicap;
                    obj.Medium = tbl_Registration.Medium;
                    obj.Type_Of_User = model.Type_Of_User;
                    obj.Subject1 = model.Subject1;
                    obj.Subject2 = model.Subject2;
                    obj.Subject3 = model.Subject3;
                    obj.Subject4 = model.Subject4;
                    obj.Subject5 = model.Subject5;
                    obj.Subjects = model.Subject1 + "," + model.Subject2 + "," + model.Subject3 + "," + model.Subject4 + "," + model.Subject5;
                    obj.EC_Number = tbl_Registration.Enrollment_No;
                    obj.EC_Year = model.EC_Year;
                    obj.LastExamSeatNo = model.LastExamSeatNo;
                    obj.LastExamYear = model.LastExamYear;
                    obj.Date_of_birth = tbl_Registration.Date_of_Birth;
                    obj.DateNow = DateTime.Now.ToString();

                    Tbl_Registration data = db.Tbl_Registration.Where(x => x.ApplicationId == obj.Form_No).FirstOrDefault();

                    string file = Path.GetExtension(model.Upload_Photo.FileName);

                    string Filename = data.ApplicationId + file;
                    obj.Photo = "../App_Uploads/Photo/" + Filename;
                    model.Upload_Photo.SaveAs(Path.Combine(Server.MapPath("~/App_Uploads/Photo/"), Filename));

                    string file1 = Path.GetExtension(model.Upload_Sign.FileName);

                    string Filename1 = data.ApplicationId + file1;
                    obj.Signature = "../App_Uploads/Signature/" + Filename1;
                    model.Upload_Sign.SaveAs(Path.Combine(Server.MapPath("~/App_Uploads/Signature/"), Filename1));

                    db.Tbl_Application_Form.Add(obj);
                    db.SaveChanges();

                    _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
                    _Con.Open();
                    _Command = new SqlCommand("Update Tbl_Registration set Exam_Form_Disable='Downloaded' where ApplicationId='" + obj.Form_No + "'", _Con);
                    _Command.ExecuteNonQuery();
                    _Con.Close();

                    Model.ApplicationId = obj.Form_No;
                }

                return Json(new { Result = true, Response = "Record Saved Successfully", url = Url.Action("PrintExamApplicationForm", "Home", Model) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { Result = false, Response = "Failed" + ex }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult PrintExamApplicationForm(CenterViewModel centerViewModel)
        {
            var List = db.Tbl_PageStatus.ToList();

            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "PrintExamApplicationForm").FirstOrDefault();

            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Tbl_Registration oS_Form_Data = new Tbl_Registration();
                var UserName = centerViewModel.ApplicationId;
                if (UserName == null)
                {
                    return RedirectToAction("CenterInformation", "CenterLogin");
                }
                oS_Form_Data = db.Tbl_Registration.Where(os => os.ApplicationId == UserName).FirstOrDefault();
                string Name = oS_Form_Data.First_Name.ToUpper() + " " + oS_Form_Data.Middle_Name.ToUpper() + " " + oS_Form_Data.Last_Name.ToUpper();
                Tbl_Subject tbl_Subject1 = new Tbl_Subject();
                Tbl_Subject tbl_Subject2 = new Tbl_Subject();
                Tbl_Subject tbl_Subject3 = new Tbl_Subject();
                Tbl_Subject tbl_Subject4 = new Tbl_Subject();
                Tbl_Subject tbl_Subject5 = new Tbl_Subject();

                Tbl_Subject[] tbl_Subjects = common.Get_Nsqf_Sub(oS_Form_Data);

                var examForm = new CenterViewModel
                {
                    tbl_Registration = db.Tbl_Registration.Where(os => os.ApplicationId == UserName).FirstOrDefault(),
                    Center_Login = db.Center_Login_Information.Where(c => c.Contact_Center_Code == oS_Form_Data.Center_Code).FirstOrDefault(),
                    Subject1 = tbl_Subjects[0].Subject_Name + "(" + tbl_Subjects[0].Subject_Code + ")",
                    Subject2 = tbl_Subjects[1].Subject_Name + "(" + tbl_Subjects[1].Subject_Code + ")",
                    Subject3 = tbl_Subjects[2].Subject_Name + "(" + tbl_Subjects[2].Subject_Code + ")",
                    Subject4 = tbl_Subjects[3].Subject_Name + "(" + tbl_Subjects[3].Subject_Code + ")",
                    Subject5 = tbl_Subjects[4].Subject_Name + "(" + tbl_Subjects[4].Subject_Code + ")",

                };

                var centerCode = db.Center_Login_Information.Where(x => x.Center_Name == oS_Form_Data.Center && x.Contact_Center_Code == oS_Form_Data.Center_Code).FirstOrDefault();

                CenterViewModel Model = new CenterViewModel();
                Model.UDISE_No = centerCode.UDISE_No.ToString();
                Model.tbl_Registration = oS_Form_Data;
                examForm.Name = Name;
                Tbl_Application_Form model_1 = db.Tbl_Application_Form.Where(x => x.Form_No == oS_Form_Data.ApplicationId).FirstOrDefault();

                string extension = Path.GetExtension(model_1.Photo.Trim());
                string extension1 = Path.GetExtension(model_1.Signature.Trim());
                if (extension == ".jpg" || extension == ".JPG" || extension == ".jpeg" || extension == ".png")
                {
                    ViewData["Photo"] = "/App_Uploads/Photo/" + model_1.Form_No + extension;

                }
                if (extension1 == ".jpg" || extension1 == ".JPG" || extension1 == ".jpeg" || extension1 == ".png")
                {
                    ViewData["Sign"] = "/App_Uploads/Signature/" + model_1.Form_No + extension1;
                }
                else
                {
                    ViewData["Photo"] = model_1.Photo;
                    ViewData["Sign"] = model_1.Signature;

                }
                return View(examForm);
            }
        }

        public void bindDistrict()
        {
            try
            {
                var districtList = db.Tbl_District.ToList();
                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem { Text = "-Select District-", Value = "0" });

                foreach (var m in districtList)
                {
                    li.Add(new SelectListItem { Text = m.District, Value = m.Id.ToString() });
                    ViewBag.districtList = li;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult getTaluka(string id)
        {
            try
            {
                var talukaList = db.All_Taluka.Where(s => s.District == id).GroupBy(x => x.Taluka).Select(grp => grp.FirstOrDefault()).ToList();
                List<SelectListItem> licent = new List<SelectListItem>();
                licent.Add(new SelectListItem { Text = "-Select Taluka-", Value = "0" });
                if (talukaList != null)
                {
                    foreach (var x in talukaList)
                    {
                        licent.Add(new SelectListItem { Text = x.Taluka, Value = x.Taluka.ToString() });
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
                var centerList = db.Tbl_CenterList.Where(x => x.taluka == id).ToList();
                List<SelectListItem> licent = new List<SelectListItem>();
                licent.Add(new SelectListItem { Text = "-Select Center-", Value = "0" });
                if (centerList != null)
                {
                    foreach (var x in centerList)
                    {
                        licent.Add(new SelectListItem { Text = x.center_name, Value = x.center_name.ToString() });
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
                var centerCode = db.Tbl_CenterList.Where(x => x.center_name == center).FirstOrDefault();
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
                subject.SubjectListA = db.Tbl_Subject.Where(x => x.Class == id && x.Handicaped == handicaped && x.Subject_Group == "A").ToList<Tbl_Subject>();
                subject.SubjectListB = db.Tbl_Subject.Where(x => x.Class == id && x.Handicaped == handicaped && x.Subject_Group == "B").ToList<Tbl_Subject>();
                subject.SubjectListC = db.Tbl_Subject.Where(x => x.Class == id && x.Handicaped == handicaped && x.Subject_Group == "C").ToList<Tbl_Subject>();
                if (handicaped == "No" && id == "8th")
                {
                    subject.SubjectListD = db.Tbl_Subject.Where(x => x.Class == id && x.Handicaped == handicaped && x.Subject_Group == "D").ToList<Tbl_Subject>();
                    var allList = new { SubjectListA = subject.SubjectListA, SubjectListB = subject.SubjectListB, SubjectListC = subject.SubjectListC, SubjectListD = subject.SubjectListD };
                    return Json(allList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var allList = new { SubjectListA = subject.SubjectListA, SubjectListB = subject.SubjectListB, SubjectListC = subject.SubjectListC };
                    return Json(allList, JsonRequestBehavior.AllowGet);
                }


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



    }
}