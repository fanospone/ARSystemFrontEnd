using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBapsBigDataReject : PostTrxBapsBigDataView
    {
        public string Remarks { get; set; }
        public string Department { get; set; }
        public int MstRejectDtlId { get; set; } 
    }
}