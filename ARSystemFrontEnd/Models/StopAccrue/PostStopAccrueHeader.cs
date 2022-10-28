using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostStopAccrueHeader : DatatableAjaxModel
    {
        public string RequestNumber { get; set; }
        public string Initiator { get; set; }
        public string ActivityOwner { get; set; }
        public string ActivityOwnerName { get; set; }
        public string InitiatorName { get; set; }
        public int ActivityID { get; set; }
        public string Activity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedDate2 { get; set; }
        public DateTime? StartEffectiveDate { get; set; }
        public DateTime? EndEffectiveDate { get; set; }
        public int  RequestTypeID { get; set; }
        public string UserRole { get; set; }
        public string DepartName { get; set; }
        public int HeaderID { get; set; }
        public int AppHeaderID { get; set; }
        public string RequestType { get; set; }
        public bool IsReHold { get; set; }

        public string SubmissionDateFrom { get; set; }
        public string SubmissionDateTo { get; set; }
        public string DirectorateCode { get; set; }
        public string AccrueType { get; set; }
        public string DetailCase { get; set; }
        public string DeptOrDetailCase { get; set; }
        public string SoNumberCount { get; set; }

    }
}