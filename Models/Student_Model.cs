using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Open_Schooling.Models
{
    public class Student_Model
    {
        public Tbl_Registration tbl_Registration { get; set; }
        public Tbl_Application_Form tbl_Application_Form { get; set; }
        public Tbl_payment Tbl_Payment { get; set; }
        public Center_Login_Information Center_Login { get; set; }
        public string ApplicationId { get; set; }
        public string Payment_Status { get; set; }
        public string Name { get; set; }
        public string Medium { get; set; }
        public string Mobile_No { get; set; }
        public string amount_money { get; set; }
        
        public string order_id { get; set; }
        public string tracking_id { get; set; }
        public string bank_ref_no { get; set; }
        public string order_status { get; set; }
      
        public string card_name { get; set; }
       
    
      
      
        public string merchant_param1 { get; set; }
       
        public string trans_date { get; set; }

        public Nullable<double> UDISE_No { get; set; }      
        public string Division { get; set; }
        public string Div_Code { get; set; }
        public string District { get; set; }
        public string Taluka { get; set; }
        public string Contact_Center_Code { get; set; }
        public string Center_Name { get; set; }

        public string Subject_List { get; set; }
    
        public string Date_of_Birth { get; set; }
        public string Center_Code { get; set; }
        
        public string Adhar_no { get; set; }
        public string Standard { get; set; }
        public string Minority_Religion { get; set; }
        public string Gender { get; set; }
        public string Handicap { get; set; }
        public string Pin_Code { get; set; }
        public string billing_city { get; set; }
        public string DOB_Village { get; set; }
        public string Village { get; set; }
        public string Registration_Form { get; set; }
        public string Form_No { get; set; }
        public string Email { get; set; }
        public string EC_No { get; set; }
        public string Ec_Status { get; set; }
        public string Subject1 { get; set; }
        public string Subject2 { get; set; }
        public string Subject3 { get; set; }
        public string Subject4 { get; set; }
        public string Subject5 { get; set; }
        public string Subject1_value { get; set; }
        public string Subject2_value { get; set; }
        public string Subject3_value { get; set; }
        public string Subject4_value { get; set; }
        public string Subject5_value { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Mother_Name { get; set; }
        public string Exam_Form_Disable { get; set; }
        public string Address { get; set; }
        public string Seat_No { get; set; }
        public string Type_Of_User { get; set; }
        public string Photo { get; set; }
        public string Signature { get; set; }
        public string Center { get; set; }
        public HttpPostedFileBase Upload_Photo { get; set; }
        public HttpPostedFileBase Upload_Sign { get; set; }


        //--------------------------------------------
        public string EC_Year { get; set; }
        public string LastExamYear { get; set; }
        public string LastExamSeatNo { get; set; }
    }
}