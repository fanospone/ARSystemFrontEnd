using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostChecklistInvoiceBuildingView : DatatableAjaxModel
    {
        public string companyName { get; set; }
        public string InvoiceTypeId { get; set; }
        public string Status { get; set; }
        public DateTime? PostingDateFrom { get; set; }
        public DateTime? PostingDateTo { get; set; }
        public string InvNo { get; set; }
        public ARSystemService.vmChecklistInvoiceBuilding dataChecklist { get; set; }

        public string strRemarksCancel { get; set; }
        public int mstPICATypeID { get; set; }
        public int mstPICADetailID { get; set; }
    }
}