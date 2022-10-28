using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueSummaryParameters
    {
        public string vAccount { get; set; }
        public int? vYear { get; set; }
        public string vCompanyId { get; set; }
        public string vRegionalName { get; set; }
        public string vOperatorId { get; set; }
        public string vProduct { get; set; }
        public string vGroupBy { get; set; }
        public string vViewBy { get; set; }
        public int? vMonth { get; set; }
        public string vGroupValue { get; set; }
        public int? vStipSiro { get; set; }
        public string vDesc { get; set; }
        public string vSoNumber { get; set; }
        public string vSiteID { get; set; }
        public string vSiteName { get; set; }
    }
}
