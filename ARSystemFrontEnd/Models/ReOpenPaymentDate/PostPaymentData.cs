using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostPaymentData : DatatableAjaxModel
    {
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDate2 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedDate2 { get; set; }
        public string InvCompanyId { get; set; }
        public string InvOperatorID { get; set; }
        public string InvNo { get; set; }
        public string RequestNumber { get; set; }
        //public ARSystemService.vwARReOpenPaymentDate vwData = new ARSystemService.vwARReOpenPaymentDate();
    }

    public class PostTrxPaymentData : DatatableAjaxModel
    {
        public ARSystemService.TrxARReOpenPaymentDate request = new ARSystemService.TrxARReOpenPaymentDate();
        public List<ARSystemService.TrxARReOpenPaymentDateDetail> requestDetailList = new List<ARSystemService.TrxARReOpenPaymentDateDetail>();

    }

    public class PostTrxPaymentDataDetail : DatatableAjaxModel
    {
        public int HeaderID { get; set; }
        public DateTime? PaymentDate { get; set; }

    }
}