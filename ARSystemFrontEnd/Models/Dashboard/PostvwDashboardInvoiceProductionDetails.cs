using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;
namespace ARSystemFrontEnd.Models
{
    public class PostvwDashboardInvoiceProductionDetails : DatatableAjaxModel
    {
		public string Sonumb { get; set; }
		public string SiteName { get; set; }
		public string SiteIDOpr { get; set; }
		public string SiteNameOpr { get; set; }
		public string BAPSType { get; set; }
		public DateTime? StartPeriodeInvoice { get; set; }
		public DateTime? EndPeriodeInvoice { get; set; }
		public decimal AmountInvoice { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public DateTime? ConfirmDate { get; set; }
		public string NoInvoice { get; set; }
		public DateTime? CreateInvDate { get; set; }
		public DateTime? PostingDate { get; set; }
		public DateTime? PrintDate { get; set; }
		public DateTime? SubmitChecklistDate { get; set; }
		public DateTime? ApproveChecklistdate { get; set; }
	}
}