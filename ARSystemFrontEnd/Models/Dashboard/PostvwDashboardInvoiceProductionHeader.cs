using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostvwDashboardInvoiceProductionHeader : DatatableAjaxModel
    {
		public string NoInvoice { get; set; }
		public int? StatusID { get; set; }
		public string Status { get; set; }
		public string SubjectInvoice { get; set; }
		public decimal? AmountInvoice { get; set; }
		public string TypeInvoice { get; set; }
		public DateTime? Createinvoice { get; set; }
		public DateTime? PostingInvoice { get; set; }
		public DateTime? SubmitDocInvoice { get; set; }
		public DateTime? ApproveDocInvoice { get; set; }
		public DateTime? ReceiptInvoiceDate { get; set; }
		public string TypePayment { get; set; }
		public DateTime? PaymentDate { get; set; }
		public string AgingCategory { get; set; }
		public string InvoiceCategory { get; set; }
		public string OutStanding { get; set; }
		public int IsCreateInvToFullPaidOutStanding { get; set; }
		public int IsSubmitDocToFullPaidOutStanding { get; set; }
		public int IsSubmitOperatorToFullPaidOutStanding { get; set; }
		public int IsCreateInvOutStandingProd { get; set; }
		public int IsPostingOutStandingProd { get; set; }
		public int IsSubmitDocInvOutStandingProd { get; set; }
		public int IsReceiptARROutStandingProd { get; set; }
		public int IsSubmitOprInvOutStandingProd { get; set; }
		public int IsCreateInvToFullPaidOutStandingProd { get; set; }
		public int IsSubmitDocToFullPaidOutStandingProd { get; set; }
		public int IsSubmitOperatorToFullPaidOutStandingProd { get; set; }
	}
}