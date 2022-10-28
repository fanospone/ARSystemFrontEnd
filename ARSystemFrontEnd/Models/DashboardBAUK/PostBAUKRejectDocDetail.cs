using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAUKRejectDocDetail : DatatableAjaxModel
    {
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public int? STIPID { get; set; }
        public int Id { get; set; }
        public string SoNumber { get; set; }
        public string DocumentName { get; set; }
        public byte CheckType { get; set; }
        public string RejectReason { get; set; }
        public List<int> Months { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}