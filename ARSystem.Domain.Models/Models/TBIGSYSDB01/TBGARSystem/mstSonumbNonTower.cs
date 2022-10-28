
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstSonumbNonTower : BaseClass
	{
		public mstSonumbNonTower()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstSonumbNonTower(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SoNumb { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string OperatorID { get; set; }
		public string CompanyCode { get; set; }
		public int mstCategoryInvoiceID { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}