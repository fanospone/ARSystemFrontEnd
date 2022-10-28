using ARSystem.Domain.Models.Models.RevenueAssurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAPSValidationBulky
    {
        public string StartLeasePeriod { get; set; }
        public string EndLeasePeriod { get; set; }
        public string Customer { get; set; }
        public string CompanyID { get; set; }
        public string StartEffectivePerio { get; set; }
        public string EndEffectivePeriod { get; set; }
        public string Remarks { get; set; }
        public string BaseLeaseCurrency { get; set; }
        public string SeriveCurrency { get; set; }
        public string BaseLeasePrice { get; set; }
        public string ServicePrice { get; set; }
        public string TotalAmount { get; set; }
        public List<vmNewBapsData> vwMstBaps { get; set; }
    }
}