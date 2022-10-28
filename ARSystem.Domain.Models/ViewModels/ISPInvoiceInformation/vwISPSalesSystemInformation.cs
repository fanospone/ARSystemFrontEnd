
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwISPSalesSystemInformation : BaseClass
	{
		public vwISPSalesSystemInformation()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwISPSalesSystemInformation(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		
		public string SONumber { get; set; }
        public int? STIPSiro { get; set; }
        public string STIPNumber { get; set; }
        public string STIPCode { get; set; }
        public DateTime? StartLeasePeriod { get; set; }
        public DateTime? EndLeasePeriod { get; set; }
        public decimal? OMPrice { get; set; }
        public decimal? PriceAmount { get; set; }
    }
}