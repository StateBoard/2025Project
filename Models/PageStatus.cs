using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Open_Schooling.Models
{
    public class PageStatus
    {
        public int Id { get; set; }
        public string Pagename { get; set; }
        public Nullable<int> Status { get; set; }
    }
}