using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostMasterlistAccrue : DatatableAjaxModel
    {
        public string sonumb { get; set; }
        public string companyID { get; set; }
        public string operatorID { get; set; }

        public string action { get; set; }
        //public string Action { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Remaks { get; set; }
        public string Status { get; set; }
        public string Path { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string week { get; set; }
    }
}