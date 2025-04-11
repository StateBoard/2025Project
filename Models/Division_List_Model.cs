using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Open_Schooling.Models
{
    public class Division_List_Model
    {
        public string Div_Code { get; set; }


        public string ApplicationId { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Mother_Name { get; set; }
        public string Adhar_no { get; set; }
        public string Mobile_No { get; set; }
        public string Address { get; set; }
        public string Village { get; set; }
        public string District { get; set; }
        public string Taluka { get; set; }
        public string State { get; set; }
        public string Pin_Code { get; set; }
        public string Date_of_Birth { get; set; }
        public string DOB_Words { get; set; }
        public string DOB_Village { get; set; }
        public string DOB_Taluka { get; set; }
        public string DOB_District { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Standard { get; set; }
        public string Medium { get; set; }
        public string Age_Certified_Proof { get; set; }
        public string District_Center { get; set; }
        public string Taluka_Center { get; set; }
        public string Center { get; set; }
        public string Previous_Attend_School_YN { get; set; }
        public string Self_Declaration_Not_Gone_School { get; set; }
        public string Previous_Attend_School { get; set; }
        public string Date_Of_Leaving_Last_Attended_School { get; set; }
        public string Handicap { get; set; }
        public string Candidate_Handicaped_YN { get; set; }
        public string Subject_List { get; set; }
        public string Subject1 { get; set; }
        public string Subject2 { get; set; }
        public string Subject3 { get; set; }
        public string Subject4 { get; set; }
        public string Subject5 { get; set; }
        public string Minority_Religion { get; set; }
        public string Nsqf_Subject { get; set; }

        public string Password { get; set; }

        public string ip { get; set; }
        public Nullable<System.DateTime> DateNow { get; set; }
        public string Year_Id { get; set; }
        public string Center_Code { get; set; }
        public string Enrollment_No { get; set; }
        public string Payment_Status { get; set; }
        public string Exam_Form_Disable { get; set; }
        public int countData { get; set; }
    }
}