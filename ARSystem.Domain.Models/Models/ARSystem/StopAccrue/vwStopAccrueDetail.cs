
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwStopAccrueDetail : BaseClass
    {
        public vwStopAccrueDetail()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwStopAccrueDetail(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public Int64 RowIndex { get; set; }
        public long ID { get; set; }
        public long TrxStopAccrueHeaderID { get; set; }
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string Product { get; set; }
        public string CategoryCase { get; set; }
        public int? CaseCategoryID { get; set; }
        public string DetailCase { get; set; }
        public int? CaseDetailID { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public DateTime? RFIDone { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? RevenueAmount { get; set; }
        public decimal? CapexAmount { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public Int64 ViewIdx { get; set; }
        public bool IsHold { get; set; }
        public string DepartName { get; set; }
    }
}