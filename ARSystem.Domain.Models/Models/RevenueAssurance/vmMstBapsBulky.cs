using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.RevenueAssurance
{
    public class vmMstBapsBulky : BaseClass
    {
        public vmMstBapsBulky()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmMstBapsBulky(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int StipSiro { get; set; }
        public int mstBapsTypeID { get; set; }
        public int? PowerTypeID { get; set; }
        public int mstCustomerInvoiceID { get; set; }
        public string CompanyInvoiceId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public DateTime? StartBapsDate { get; set; }
        public DateTime? EndBapsDate { get; set; }

        public DateTime? StartEffectiveDate { get; set; }
        public DateTime? EndEffectiveDate { get; set; }

        public DateTime? BapsDoneDate { get; set; }

        public DateTime? BapsPeriodDate { get; set; }

        public DateTime? EffectiveBapsDate { get; set; }

        public string Remark { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public string BaseLeaseCurrency { get; set; }
        public string ServiceCurrency { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? BapsDate { get; set; }
        public List<mstBaps> ListMstBaps { get; set; }

    }
}
