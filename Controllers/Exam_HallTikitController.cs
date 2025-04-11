using Open_Schooling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace New_Open_Schooling.Controllers
{
    public class Exam_HallTikitController : Controller
    {

        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities(); 
        int x;
        string y;
        // GET: Exam_HallTikit
        public ActionResult Halltikit()
        {
            return View();
        }

        public string NumberToWords(int number, string stand)
        {
            try
            {
                if (number == 0)
                    return "zero";

                if (number < 0)
                    return "minus " + NumberToWords(Math.Abs(number), stand);

                string words = "";

                if ((number / 1000000) > 0)
                {
                    words += NumberToWords(number / 1000000, stand) + " million ";
                    number %= 1000000;
                }

                if ((number / 1000) > 0)
                {
                    words += NumberToWords(number / 1000, stand) + " thousand ";
                    number %= 1000;
                }

                if ((number / 100) > 0)
                {
                    words += NumberToWords(number / 100, stand) + " hundred ";
                    number %= 100;
                }

                if (number > 0)
                {
                    if (words != "")
                        words += "and ";

                    var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                    if (number < 20)
                        words += unitsMap[number];
                    else
                    {
                        words += tensMap[number / 10];
                        if ((number % 10) > 0)

                            words += " " + unitsMap[number % 10];
                    }
                }


                if (stand == "5")
                {
                    ViewData["Class"] = y + " ZERO FIVE" + "-" + words.ToUpper();
                }
                else if (stand == "8")
                {
                    ViewData["Class"] = y + " ZERO EIGHT" + "- " + words.ToUpper();
                }
                return words;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult HalltikitPrint(Tbl_Registration os_Form_Data)
        {

            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "HalltikitPrint").FirstOrDefault();

            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {

                    Tbl_Application_Form tbl_Application_Form = db.Tbl_Application_Form.Where(x => x.Form_No == os_Form_Data.ApplicationId || x.Mobile_no == os_Form_Data.Mobile_No).FirstOrDefault();

                    x = Int32.Parse(tbl_Application_Form.Seat_No.Substring(4, 5));

                    y = tbl_Application_Form.Seat_No.Substring(0, 2);
                    tbl_Application_Form.Stand = tbl_Application_Form.Seat_No.Substring(3, 1);
                    os_Form_Data = db.Tbl_Registration.Where(x => x.ApplicationId == os_Form_Data.ApplicationId || x.Mobile_No == os_Form_Data.Mobile_No).FirstOrDefault();
                    NumberToWords(x, tbl_Application_Form.Stand);
                    if (os_Form_Data.Country != null && os_Form_Data.State != null)
                    {



                        string Name = tbl_Application_Form.Name.ToUpper() + " " + tbl_Application_Form.Middle_name.ToUpper() + " " + tbl_Application_Form.Last_name.ToUpper();
                        TempData["Name"] = Name;


                        var sub1 = db.Tbl_Subject.Where(x => x.Subject_Code == os_Form_Data.Subject1).FirstOrDefault();
                        var sub2 = db.Tbl_Subject.Where(x => x.Subject_Code == os_Form_Data.Subject2).FirstOrDefault();
                        var sub3 = db.Tbl_Subject.Where(x => x.Subject_Code == os_Form_Data.Subject3).FirstOrDefault();
                        var sub4 = db.Tbl_Subject.Where(x => x.Subject_Code == os_Form_Data.Subject4).FirstOrDefault();
                        var sub5 = db.Tbl_Subject.Where(x => x.Subject_Code == os_Form_Data.Subject5).FirstOrDefault();

                        List<Student_Model> Student_Model = new List<Student_Model>();

                        Student_Model = (from A in db.Tbl_Application_Form

                                         join
                                               B in db.Tbl_Registration
                                         on
                                         A.Form_No equals B.ApplicationId
                                         join
                                         C in db.Center_Login_Information
                                         on
                                         A.Contact_center equals C.Contact_Center_Code
                                         where A.Form_No.Trim() == os_Form_Data.ApplicationId.ToString()
                                         select new Student_Model
                                         {
                                             Subject1 = A.Subject1,
                                             Subject2 = A.Subject2,
                                             Subject3 = A.Subject3,
                                             Subject4 = A.Subject4,
                                             Subject5 = A.Subject5,
                                             Subject1_value = sub1.Subject_Name,
                                             Subject2_value = sub2.Subject_Name,
                                             Subject3_value = sub3.Subject_Name,
                                             Subject4_value = sub4.Subject_Name,
                                             Subject5_value = sub5.Subject_Name,
                                             Division = C.Division,
                                             Date_of_Birth = A.Date_of_birth,
                                             Seat_No = A.Seat_No,
                                             Type_Of_User = B.Type_Of_User,
                                             Center_Code = B.Center_Code,
                                             Handicap = B.Handicap,
                                             DOB_Village = B.DOB_Village,
                                             Gender = B.Gender,
                                             Center = B.Center,
                                             Mother_Name = B.Mother_Name,
                                             Medium = B.Medium,
                                             Standard = B.Standard,
                                             Photo = A.Photo,
                                             Signature = A.Signature,

                                         }).ToList();

                        return View(Student_Model);
                    }
                    else
                    {
                        string Name = tbl_Application_Form.Name.ToUpper() + " " + tbl_Application_Form.Middle_name.ToUpper() + " " + tbl_Application_Form.Last_name.ToUpper();
                        TempData["Name"] = Name;


                        var sub1 = db.Tbl_Subject.Where(x => x.Subject_Code == os_Form_Data.Subject1).FirstOrDefault();

                        List<Student_Model> Student_Model = new List<Student_Model>();

                        Student_Model = (from A in db.Tbl_Application_Form

                                         join
                                               B in db.Tbl_Registration
                                         on
                                         A.Form_No equals B.ApplicationId
                                         join
                                         C in db.Center_Login_Information
                                         on
                                         A.Contact_center equals C.Contact_Center_Code
                                         where A.Form_No.Trim() == os_Form_Data.ApplicationId.ToString()
                                         select new Student_Model
                                         {
                                             Subject1 = "AA_01",
                                             Subject1_value = A.Subject1,
                                             Division = "BMM",
                                             Date_of_Birth = B.DOB_Place_Address,
                                             Seat_No = A.Seat_No,
                                             Type_Of_User = B.Type_Of_User,
                                             Center_Code = B.Center_Code,
                                             Handicap = B.Handicap,
                                             DOB_Village = B.DOB_Village,
                                             Gender = B.Gender,
                                             Center = B.BMM_Center,
                                             Mother_Name = B.Mother_Name,
                                             Medium = "Marathi",
                                             Standard = B.Standard,
                                             Photo = A.Photo,
                                             Signature = A.Signature,

                                         }).ToList();

                        return View(Student_Model);
                    }

                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}