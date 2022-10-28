
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstRegion : BaseClass
	{
		public mstRegion()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstRegion(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int RegionID { get; set; }
		public string RegionName { get; set; }
		public string AreaID { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}