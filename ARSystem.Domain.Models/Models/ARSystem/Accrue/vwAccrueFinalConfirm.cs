
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwAccrueFinalConfirm : BaseClass
	{
		public vwAccrueFinalConfirm()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwAccrueFinalConfirm(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public string SOW { get; set; }
        public int? Week { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? NEWCURRNoOfSite { get; set; }
        public decimal? NEWCURRTotalAmount { get; set; }
        public int? NEWODNoOfSite { get; set; }
        public decimal? NEWODTotalAmount { get; set; }
        public int? RECCURRNoOfSite { get; set; }
        public decimal? RECCURRTotalAmount { get; set; }
        public int? RECODNoOfSite { get; set; }
        public decimal? RECODTotalAmount { get; set; }
        public int? TotalNoOfSite { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal Percen { get; set; }
	}
}