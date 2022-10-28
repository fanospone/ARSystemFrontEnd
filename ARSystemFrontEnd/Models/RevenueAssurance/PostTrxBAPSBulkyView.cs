using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBAPSBulkyView : DatatableAjaxModel
    {
        public string ID { get; set; }
        public string BAPSNumber { get; set; }
        public string BAPSSignDate { get; set; }
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public string Remark { get; set; }
        public string Attachment { get; set; }
        public string AmountBAPS { get; set; }
        public List<string> PONumber { get; set; }
        public List<int> ListId { get; set; }
        public List<string> ListBAPSNumber { get; set; }

    }
} 