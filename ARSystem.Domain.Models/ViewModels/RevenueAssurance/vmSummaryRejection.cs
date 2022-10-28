using ARSystem.Domain.Models.ViewModels.Datatable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmSummaryRejection : BaseClass
    {
        public vmSummaryRejection()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmSummaryRejection(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        
        //Header
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int CountRejectFin { get; set; }
        public decimal? AmountFin { get; set; }
        public int RepatitiveFin { get; set; }
        public int CountRejectOpr { get; set; }
        public decimal? AmountOpr { get; set; }
        public int RepatitiveOpr { get; set; }

        //Detail
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CustomerInvoice { get; set; }
        public string CompanyInvoice { get; set; }
        public string RegionName { get; set; }
        public string BapsNo { get; set; }
        public string BapsType { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime StartDateInvoice { get; set; }
        public DateTime EndDateInvoice { get; set; }
        public decimal? AmountInvoice { get; set; }
        public string Currency { get; set; }
        public int RejectYear { get; set; }
        public int RejectMonth { get; set; }
        public DateTime? RejectDate { get; set; }
        public DateTime? RTIDate { get; set; }
        public DateTime? BapsConfirmDate { get; set; }
        public string PicaType { get; set; }
        public string PicaMajor { get; set; }
        public string PicaDetail { get; set; }
        public string Remarks { get; set; }
    }
}
