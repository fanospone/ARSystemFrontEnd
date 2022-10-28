using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class trxStopAccrueDetailDraft : BaseClass
    {
        public trxStopAccrueDetailDraft()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public trxStopAccrueDetailDraft(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public long ID { get; set; }
        public long? TrxStopAccrueHeaderID { get; set; }
        public string SONumber { get; set; }
        public int? CaseCategoryID { get; set; }
        public int? CaseDetailID { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? CapexAmount { get; set; }
        public decimal? RevenueAmount { get; set; }
        public bool IsHold { get; set; }
    }
}