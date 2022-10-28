using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxApprovalARMonitoringBuildingView : DatatableAjaxModel
    {
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public string strCompanyId { get; set; }
        public string strStatusGenerate { get; set; }
        public string invNo { get; set; }
        public int HeaderId { get; set; }
        public int CategoryId { get; set; }
        public string strRemarks { get; set; }
        public string filePath { get; set; }
        public List<int> ListHeaderId { get; set; }
        public List<int> ListCategoryId { get; set; }
        public List<int> ListBatchNumber { get; set; }
        public bool isCollection { get; set; }
        public List<int> ListPaymentCodeId { get; set; }
    }
}