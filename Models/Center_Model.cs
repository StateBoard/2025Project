using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Open_Schooling.Models
{
    public class Center_Model
    {
        public Tbl_PageStatus tbl_PageStatus { get; set; }

        public string Payment_Status { get; set; }
        public string ApplicationId { get; set; }
        public string Name { get; set; }
        public string center_Name { get;set; }
        public string Contact_Center_Code { get; set; }
        public string Division { get; set; }
        public string Taluka { get; set; }
        public string Mobile_No { get; set; }
        public string Ec_Status { get; set; }
        public string Seat_No { get; set; }
        public string EC_No { get; set; }
        public string Exam_Form_Disable { get; set; }

        
    }
}