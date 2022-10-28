using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAUKActivity : DatatableAjaxModel
    {
        public string GroupSum { get; set; }
        public string GroupSumID { get; set; }
        public int RFIDone { get; set; }
        public float AmountRFIDone { get; set; }
        public int BAUKSubmitted { get; set; }
        public float AmountBAUKSubmitted { get; set; }
        public int BAUKApproved { get; set; }
        public float AmountBAUKApproved { get; set; }
        public int BAUKRejected { get; set; }
        public float AmountBAUKRejected { get; set; }
        public int Total { get; set; }
        public float AmountTotal { get; set; }
        public float PercentTotal { get; set; }
    }
}