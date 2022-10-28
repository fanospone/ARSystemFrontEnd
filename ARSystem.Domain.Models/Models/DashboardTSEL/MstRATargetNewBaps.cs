
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class MstRATargetNewBaps : BaseClass
	{
		public MstRATargetNewBaps()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public MstRATargetNewBaps(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
        public int? RowIndex { get; set; }

        public string SoNumber { get; set; }
        public int? YearBill { get; set; }
		public int? Year { get; set; }
		public int? Month { get; set; }
		public int? BapsType { get; set; }
		public string PowerType { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }

        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public decimal? AmountIDR { get; set; }
        public decimal? AmountUSD { get; set; }
        public string DepartmentCode { get; set; }
    }
}