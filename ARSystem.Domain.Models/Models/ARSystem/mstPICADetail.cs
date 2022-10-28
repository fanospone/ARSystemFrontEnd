
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstPICADetail : BaseClass
	{
		public mstPICADetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstPICADetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstPICADetailID { get; set; }
		public string Description { get; set; }
		public int? mstPICATypeID { get; set; }
		public bool? IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}