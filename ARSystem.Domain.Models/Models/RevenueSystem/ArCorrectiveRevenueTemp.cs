
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class ArCorrectiveRevenueTemp : BaseClass
	{
		public ArCorrectiveRevenueTemp()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public ArCorrectiveRevenueTemp(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public int RowIndex { get; set; }
        public string SoNumber { get; set; }
		public string TypeAdjusment { get; set; }
		public decimal TotalAdjusment { get; set; }
		public byte MonthPeriod { get; set; }
		public short YearPeriod { get; set; }
		public DateTime CeatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}