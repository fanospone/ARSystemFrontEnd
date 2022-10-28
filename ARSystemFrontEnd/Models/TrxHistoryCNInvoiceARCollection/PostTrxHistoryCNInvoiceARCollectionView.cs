using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxHistoryCNInvoiceARCollectionView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public string InvNo { get; set; }
        public string CNStatus { get; set; }
        public string InvoiceTypeId { get; set; }

    }
}