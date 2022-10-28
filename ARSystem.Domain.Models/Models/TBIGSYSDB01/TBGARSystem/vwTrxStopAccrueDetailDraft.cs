
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
    public class vwTrxStopAccrueDetailDraft : BaseClass
    {
        public vwTrxStopAccrueDetailDraft()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwTrxStopAccrueDetailDraft(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public long? TrxStopAccrueHeaderID { get; set; }
        public string SONumber { get; set; }
        public int? CaseCategoryID { get; set; }
        public int? CaseDetailID { get; set; }
        public string Remarks { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? RevenueAmount { get; set; }
        public decimal? CapexAmount { get; set; }
        public bool? IsHold { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}