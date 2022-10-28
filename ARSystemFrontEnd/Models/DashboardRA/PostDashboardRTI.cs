using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardRTI : DatatableAjaxModel
    {
        public int? Year { get; set; }
        public string Month { get; set; }
        public string Category { get; set; } //Target or Achivement
        public List<string> CustomerID { get; set; }
        public List<string> DepartmentCode { get; set; }
        public string GroupBy { get; set; }

    }
}