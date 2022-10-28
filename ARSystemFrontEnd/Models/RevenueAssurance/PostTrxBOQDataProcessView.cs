using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBOQDataProcessView : DatatableAjaxModel
    {
        public string ID { get; set; }
        public string strYear { get; set; }
        public string strOperator { get; set; }
        public string strCompanyId { get; set; }
        public string strArea { get; set; }
        public string strRegional { get; set; }
        public string strBOQNumber { get; set; }
        public string TotalRenewalTenant { get; set; }
        public string TotalAmount { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string StatusBOQ { get; set; }
        public int mstRAActivityID { get; set; }
        public List<string> listBOQNumber { get; set; }


    }
} 