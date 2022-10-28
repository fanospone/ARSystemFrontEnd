using System;
using System.Collections.Generic;
using System.Web;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostStopAccrue : DatatableAjaxModel
    {
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public int? ProductID { get; set; }
        public int RegionID { get; set; }
        public int ActivityID { get; set; }
        public int PrevActivityID { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestType { get; set; }
        public int HeaderID { get; set; }
        public int AppHeaderID { get; set; }
        public string ActivityOwner { get; set; }
        public string InitiatorID { get; set; }
        public string Remarks { get; set; }
        public string ActivityLabel { get; set; }
        public string PrevActivityLabel { get; set; }
        public string NextFlag { get; set; }
        public DateTime? StartEffectiveDate { get; set; }
        public DateTime? EnndEffectiveDate { get; set; }
        public string RequestNumber { get; set; }

        public List<PostStopAccrueRequest> StopAccrueRequest = new List<PostStopAccrueRequest>();
        public PostStopAccrueRequest StopAccrueRequestDetail = new PostStopAccrueRequest();
        public vwStopAccrueHeader stopAccrueHeader = new vwStopAccrueHeader();
    }
}