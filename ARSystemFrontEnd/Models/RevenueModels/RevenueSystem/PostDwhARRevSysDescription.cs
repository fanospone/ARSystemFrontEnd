using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostDwhARRevSysDescription    : DatatableAjaxModel
    {
        public DateTime? RfiStartDate { get; set; }
        public DateTime? RfiEndDate { get; set; }
        public DateTime? StartSLDDate { get; set; }
        public DateTime? EndSLDDate { get; set; }
        public string DepartmentName { get; set; }
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
        public string RegionName { get; set; }
        public short? RegionId { get; set; }
        public string SoNumber { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
    }
}