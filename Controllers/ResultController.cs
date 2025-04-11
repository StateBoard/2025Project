
using Open_Schooling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace New_Open_Schooling.Controllers
{
    public class ResultController : Controller
    {


        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities();
        // GET: Result
        [HttpGet]
        public ActionResult ResultCredentials()
        {
            var page = db.Tbl_PageStatus.Where(x => x.Pagename == "Result").FirstOrDefault();

            if (page.Status == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ResultCredentials(string seatno, string mobile)
        {
            return View();
        }

        public JsonResult VerifyStudent(string seatno, string mobile)
        {
           

            if (seatno.Trim() == "" || mobile.Trim() == "")
            {
                return Json(new { Result = false, Message = "Please Enter Details." }, JsonRequestBehavior.AllowGet);
            }
            var found = db.Tbl_OpenSch_Result.Where(x => x.seatnumber.ToUpper() == seatno.ToUpper() && x.Mobile_Number.ToUpper() == mobile.Trim().ToUpper()).FirstOrDefault();
            if (found != null)
            {
                //Session["Seat_No"] = found.seatnumber;
                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
            else if (found == null)
            {
                return Json(new { Result = false, Message = "Invalid Seat No / Mobile Number" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResultPrint(string Seat_No)
        {
            string ss = Seat_No;
            var tbl = db.Tbl_OpenSch_Result.Where(a => a.seatnumber == ss).FirstOrDefault();
            return View(tbl);
        }
    }
}





