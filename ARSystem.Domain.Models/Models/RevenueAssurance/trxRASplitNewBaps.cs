using System;

namespace ARSystem.Domain.Models.Models.RevenueAssurance
{
    public class trxRASplitNewBaps : BaseClass
    {
        public trxRASplitNewBaps()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public trxRASplitNewBaps(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public string SONumber { get; set; }
        public int StipSiro { get; set; }
        public int? SplitRow { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}
