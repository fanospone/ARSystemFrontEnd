using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReconcileDoneView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strRenewalYear { get; set; }
        public string strRenewalYearSeq { get; set; }
        public string strRegional { get; set; }
        public string strCurrency { get; set; }
        public string strProvince { get; set; }
        public string strRegency { get; set; }
        public string strDueDatePerMonth { get; set; }
        public string strReconcileType { get; set; }
        public int isRaw { get; set; }
        public List<int> soNumb { get; set; }
        public string strYear { get; set; }
        public string strResidence { get; set; }
        
    }
}