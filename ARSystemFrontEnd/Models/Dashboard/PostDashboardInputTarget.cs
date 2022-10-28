using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardInputTarget : DatatableAjaxModel
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string DepartmentCode { get; set; }

        public string CustomerID { get; set; }
        public string CompanyInvoiceID { get; set; }
    }
}