using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostRevenueMovement : DatatableAjaxModel
    {
        public string fSoNumber { get; set; }
        public int? fYear { get; set; }
        public int? fMonth { get; set; }
        public string fCompanyId { get; set; }
        public string fRegionalName { get; set; }
        public string fOperatorId { get; set; }
        public string fProduct { get; set; }
        public string fGroupBy { get; set; }
        public string fDesc { get; set; }
        public string fViewBy { get; set; }
        public string fSiteID { get; set; }
        public string fSiteName { get; set; }

    }
}