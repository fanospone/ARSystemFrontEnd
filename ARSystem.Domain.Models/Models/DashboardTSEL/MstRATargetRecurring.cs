
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class MstRATargetRecurring : BaseClass
    {
        public MstRATargetRecurring()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public MstRATargetRecurring(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public long ID { get; set; }
        public int? mstBapsId { get; set; }
        public int? YearBill { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? BapsType { get; set; }
        public string PowerType { get; set; }

        public string BapsTypeName { get; set; }
        public string PowerTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        // Modification Or Added By Ibnu Setiawan 24. January 2020
        public int Qty { get; set; }
        // update 23-8-2021
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public decimal? AmountIDR { get; set; }
        public decimal? AmountUSD { get; set; }
        public string DepartmentCode { get; set; }
        public string SONumber { get; set; }

        public int? RowIndex { get; set; }
    }
}