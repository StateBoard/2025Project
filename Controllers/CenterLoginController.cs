
using Newtonsoft.Json;
using Open_Schooling.Helper;
using Open_Schooling.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Open_Schooling.Controllers
{
    public class CenterLoginController : Controller
    {
        Common common = new Common();
        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities();
        string x;
        public ActionResult CenterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CenterLogin(Center_Login_Information center_Login)
        {
            try
            {
                Login_Model login_Model = new Login_Model();
                var IsValid = db.Center_Login_Information.Where(c => c.UDISE_No == center_Login.UDISE_No && c.Center_Password == center_Login.Center_Password).FirstOrDefault();
                if (IsValid != null)
                {
                    FormsAuthentication.SetAuthCookie(IsValid.Contact_Center_Code, false);

                    login_Model.center_Name = IsValid.Center_Name;
                    login_Model.Contact_Center_Code = IsValid.Contact_Center_Code;
                    login_Model.Division = IsValid.Division;
                    login_Model.Taluka = IsValid.Taluka;
                    login_Model.Country = IsValid.Country;
                    login_Model.State = IsValid.State;
                    /*login_Model.State = IsValid.State;
                    login_Model.Country = IsValid.Country;*/
                    String json = JsonConvert.SerializeObject(login_Model);
                    FormsAuthentication.SetAuthCookie(json, false);
                    return RedirectToAction("CenterInformation", "CenterLogin");
                }
                else
                {
                    TempData["ErrorMsg"] = "Invalid Username or Password.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult getPassword(double udiseNo)
        {
            try
            {
                var center = db.Center_Login_Information.FirstOrDefault(x => x.UDISE_No == udiseNo);
                if (center == null)
                {
                    return Json("Invalid UDISE Number.", JsonRequestBehavior.AllowGet);
                }

                string email = center.Email;
                if (string.IsNullOrEmpty(email))
                {
                    return Json("Email ID not found for this center.", JsonRequestBehavior.AllowGet);
                }

                string subject = "Center Login Password Recovery";
                string body = $"Dear {center.Center_Name},<br/><br/>Your login password is: <b>{center.Center_Password}</b><br/><br/>Thank you.";

                string result = common.SendMail(email, subject, body); // Assuming your SendMail method returns a success/failure string

                if (result == "success")
                {
                    return Json("Password has been sent to your registered email.", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed to send email. Please try again later.", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CenterInformation()
        {

            try
            {
                Login_Model login_Model = common.Get_Login_Details();

                var page1 = db.Tbl_PageStatus.Where(x => x.Pagename == "HalltikitPrint").FirstOrDefault();
                var page2 = db.Tbl_PageStatus.Where(x => x.Pagename == "EnrollmentCertificate").FirstOrDefault();
                var page3 = db.Tbl_PageStatus.Where(x => x.Pagename == "Application_Form").FirstOrDefault();

                ViewBag.s1 = page1.Status;
                ViewBag.s2 = page2.Status;
                ViewBag.s3 = page3.Status;


                Center_Information_Model center_Information_Model = new Center_Information_Model();
                center_Information_Model.Login_Model = login_Model;
                if (center_Information_Model.Login_Model.Country == null)
                {
                    center_Information_Model.centerViewModel = (from A in db.Tbl_Registration
                                                                join
                                                                B in db.Tbl_payment
                                                                on
                                                                A.ApplicationId equals B.merchant_param1
                                                                join
                                                                 C in db.Center_Login_Information
                                                                 on
                                                                 login_Model.Contact_Center_Code equals C.Contact_Center_Code.Trim()

                                                                where A.Center_Code.Trim() == login_Model.Contact_Center_Code.ToString() && A.Payment_Status == "1"
                                                                select new Center_Model
                                                                {
                                                                    Payment_Status = A.Payment_Status,
                                                                    ApplicationId = A.ApplicationId,
                                                                    Name = A.First_Name,
                                                                    Mobile_No = A.Mobile_No,
                                                                    Ec_Status = A.Ec_Status,
                                                                    Seat_No = db.Tbl_Application_Form.Where(a => a.Form_No == A.ApplicationId).FirstOrDefault().Seat_No,
                                                                    EC_No = A.Enrollment_No,
                                                                    Exam_Form_Disable = A.Exam_Form_Disable,
                                                                    center_Name = C.Center_Name,

                                                                }).ToList();



                    return View(center_Information_Model);
                }
                else
                {
                    center_Information_Model.centerViewModel = (from A in db.Tbl_Registration
                                                                join
                                                                 C in db.Center_Login_Information
                                                                 on
                                                                 login_Model.Contact_Center_Code equals C.Contact_Center_Code.Trim()

                                                                where A.Center_Code.Trim() == login_Model.Contact_Center_Code.ToString()
                                                                select new Center_Model
                                                                {
                                                                    ApplicationId = A.ApplicationId,
                                                                    Name = A.First_Name,
                                                                    Mobile_No = A.Mobile_No,
                                                                    Ec_Status = A.Ec_Status,
                                                                    Seat_No = db.Tbl_Application_Form.Where(a => a.Form_No == A.ApplicationId).FirstOrDefault().Seat_No,
                                                                    EC_No = A.Enrollment_No,
                                                                    Exam_Form_Disable = A.Exam_Form_Disable,
                                                                    center_Name = C.Center_Name,

                                                                }).ToList();



                    return View(center_Information_Model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        public JsonResult getPassword(double udiseNo)
        {
            try
            {
                var password = db.Center_Login_Information.Where(x => x.UDISE_No == udiseNo).FirstOrDefault();
                if (password == null)
                {
                    string Error = "Invalid UDISE Number.";
                    return Json(Error, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(password.Center_Password, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult EC_Form()


        {
            try
            {
                var List = db.Tbl_PageStatus.ToList();

                var page = db.Tbl_PageStatus.Where(x => x.Pagename == "EnrollmentCertificate").FirstOrDefault();

                if (page.Status == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [HttpPost]
        public ActionResult EC_Form(Tbl_Registration tbl_Registration)
        {
            try
            {

                var IsValid = db.Tbl_Registration.Where(c => c.ApplicationId == tbl_Registration.ApplicationId && c.Mobile_No == tbl_Registration.Mobile_No).FirstOrDefault();
                if (IsValid != null)
                {
                    FormsAuthentication.SetAuthCookie(IsValid.ApplicationId, false);
                    //Session["FormNo"] = IsValid.ApplicationId;
                    //    Session["CenterCode"] = IsValid.Center_Code;
                    return RedirectToAction("EnrollmentCertificate");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Details.");
                    return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult EnrollmentCertificate(string ApplicationId)
        {
            try
            {

                var List = db.Tbl_PageStatus.ToList();

                var page = db.Tbl_PageStatus.Where(x => x.Pagename == "EnrollmentCertificate").FirstOrDefault();

                if (page.Status == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    Tbl_Registration registration_Model = new Tbl_Registration();
                    registration_Model = db.Tbl_Registration.Where(x => x.ApplicationId == ApplicationId).FirstOrDefault();
                    if (registration_Model != null)
                    {

                        var formNo = ApplicationId;
                        var CenterCode = registration_Model.Center_Code;
                        ViewData["stand"] = ApplicationId.Substring(8, 1);

                        registration_Model = db.Tbl_Registration.Where(os => os.ApplicationId == formNo.ToString()).FirstOrDefault();
                        var ecCertificate = new CenterViewModel
                        {
                            tbl_Registration = db.Tbl_Registration.Where(os => os.ApplicationId == formNo.ToString()).FirstOrDefault(),

                            Center_Login = db.Center_Login_Information.Where(c => c.Contact_Center_Code == CenterCode.ToString()).FirstOrDefault()
                        };
                        string extension = Path.GetExtension(registration_Model.Photo.Trim());
                        if (extension == ".jpg" || extension == ".JPG" || extension == ".jpeg")
                        {

                            Session["Photo"] = registration_Model.Photo;
                        }
                        else
                        {
                            ViewData["Photo"] = registration_Model.Photo;
                        }
                        return View(ecCertificate);
                    }
                    else
                    {
                        registration_Model = db.Tbl_Registration.Where(x => x.ApplicationId == ApplicationId && x.Ec_Status != null).FirstOrDefault();
                        if (registration_Model != null)
                        {
                            var contactCeneterCode = registration_Model.Center_Code;
                            ViewData["stand"] = ApplicationId.Substring(8, 1);

                            var centerEc = new CenterViewModel
                            {
                                tbl_Registration = db.Tbl_Registration.Where(os => os.ApplicationId.Trim() == ApplicationId.Trim()).FirstOrDefault(),

                                Center_Login = db.Center_Login_Information.Where(c => c.Contact_Center_Code == contactCeneterCode).FirstOrDefault()
                            };

                            string extension = Path.GetExtension(registration_Model.Photo);
                            if (extension == ".jpg" || extension == ".JPG" || extension == ".jpeg")
                            {
                                Session["Photo"] = registration_Model.Photo;
                            }
                            else
                            {
                                ViewData["Photo"] = "/AppFiles/Images/" + registration_Model.ApplicationId.TrimEnd() + "/Profile" + ".jpg";
                            }
                            return View(centerEc);
                        }
                        else
                        {
                            return RedirectToAction("CenterInformation");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("CenterInformation");
            }
        }

        public ActionResult Download_Page()
        {
            try
            {
                List<CenterViewModel> centerViewModel = new List<CenterViewModel>();
                var CenterCode = Session["Center_Code"];
                centerViewModel = (from A in db.Tbl_Registration

                                   where A.Center_Code.Trim() == CenterCode.ToString() && A.Payment_Status == "1"
                                   select new CenterViewModel
                                   {
                                       Payment_Status = A.Payment_Status,
                                       ApplicationId = A.ApplicationId,
                                       Name = A.First_Name,
                                       Mobile_No = A.Mobile_No,
                                       Ec_Status = A.Ec_Status,
                                       EC_No = A.Enrollment_No,
                                       Exam_Form_Disable = A.Exam_Form_Disable,

                                   }).ToList();

                return View(centerViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}