
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwRevSysSonumbInflasiList : BaseClass
	{
		public vwRevSysSonumbInflasiList()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRevSysSonumbInflasiList(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string Sonumb { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string OperatorID { get; set; }
		public string SiteNameOpr { get; set; }
		public string CustomerInvoice { get; set; }
		public string CompanyInvoice { get; set; }
		public string RegionalName { get; set; }
		public decimal? AmountRental { get; set; }
		public decimal? AmountService { get; set; }
		public decimal? AmountInflation { get; set; }
        public decimal? InflationRate { get; set; }
        public string Status { get; set; }
	}
}