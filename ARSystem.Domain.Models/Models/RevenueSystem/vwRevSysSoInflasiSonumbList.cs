
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwRevSysSoInflasiSonumbList : BaseClass
	{
		public vwRevSysSoInflasiSonumbList()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRevSysSoInflasiSonumbList(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SoNumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string OperatorID { get; set; }
		public string CompanyID { get; set; }
		public string RegionalName { get; set; }
	}
}