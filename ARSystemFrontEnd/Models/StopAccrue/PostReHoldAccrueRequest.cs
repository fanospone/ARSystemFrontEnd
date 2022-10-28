using ARSystem.Domain.Models;
using System.Collections.Generic;
using System;

namespace ARSystemFrontEnd.Models
{
    public class PostReHoldAccrueRequest
    {
        public List<vwStopAccrueDetail> stopAccrueDetail = new List<vwStopAccrueDetail>();
        public List<ReHoldAccrue> reHoldAccrue = new List<ReHoldAccrue>();

        public DateTime StartEffectiveDate { get; set; }
        public DateTime EndEffectiveDate { get; set; }
        public string Remarks { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestType { get; set; }
        public int PrevAppHeaderID { get; set; }
        public string RequestNumber { get; set; }
    }
}