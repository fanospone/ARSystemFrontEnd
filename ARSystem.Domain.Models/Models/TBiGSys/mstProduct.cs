
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstProduct : BaseClass
	{
		public mstProduct()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstProduct(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ProductID { get; set; }
		public string Product { get; set; }
		public int? ProductSiteID { get; set; }
		public int ProductTypeID { get; set; }
		public string LegacyCode { get; set; }
		public int? ProductMaintenanceID { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}