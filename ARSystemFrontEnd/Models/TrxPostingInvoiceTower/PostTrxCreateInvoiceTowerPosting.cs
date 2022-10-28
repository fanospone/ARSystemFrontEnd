using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxCreateInvoiceTowerPosting : DatatableAjaxModel
    {
        public string strInvoiceDate { get; set; }
        public string strSubject { get; set; }
        public string strOperatorRegionId { get; set; }
        public string strSignature { get; set; }
        public vwDataCreatedInvoiceTower DataCreatedInvoice { get; set; }
        public List<ARSystemService.vwDataPostedInvoiceTower> ListInvoicePosted { get; set; }
        public string strRemarksCancel { get; set; }
        public string SumADPP { get; set; }
        public string SumAPPN { get; set; }
        public string SumAPenalty { get; set; }
        public string SumADiscount { get; set; }
        public int mstPICATypeID { get; set; }
        public int mstPICADetailID { get; set; }

        public string strAdditionalNote { get; set; }
    }
}