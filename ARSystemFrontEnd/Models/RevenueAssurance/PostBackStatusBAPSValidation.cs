using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBackStatusBAPSValidationView : DatatableAjaxModel
    {
        public string CustomerSiteName { get; set; }
        public string BapsType { get; set; }
        public string SoNumber { get; set; }
        public int StipSiro { get; set; }
    }

    public class PostBackStatusBAPSValidationSubmit
    {
        public string remark { get; set; }
        public List<BackStatusBAPSValidationData> dataList { get; set; }
    }
}