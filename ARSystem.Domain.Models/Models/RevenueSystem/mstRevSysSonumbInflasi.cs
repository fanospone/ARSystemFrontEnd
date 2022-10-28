
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstRevSysSonumbInflasi : BaseClass
	{
		public mstRevSysSonumbInflasi()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstRevSysSonumbInflasi(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string Sonumb { get; set; }
		public decimal? AmountRental { get; set; }
		public decimal? AmountService { get; set; }
		public decimal? AmountInflation { get; set; }
        public decimal? InflationRate { get; set; }
        public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
        public List<string> SonumbList { get; set; }
        public List<mstRevSysSonumbInflasi> SonumbListMst { get; set; }
    }
}