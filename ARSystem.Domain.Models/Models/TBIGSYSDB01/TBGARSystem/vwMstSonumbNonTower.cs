
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwMstSonumbNonTower : BaseClass
	{
		public vwMstSonumbNonTower()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwMstSonumbNonTower(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long? RowNumber { get; set; }
		public string SoNumb { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string OperatorID { get; set; }
		public string CompanyCode { get; set; }
		public int mstCategoryInvoiceID { get; set; }
		public string CategoryName { get; set; }
		public bool IsActive { get; set; }
	}
}