
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstPICAType : BaseClass
	{
		public mstPICAType()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstPICAType(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstPICATypeID { get; set; }
		public string Description { get; set; }
		public bool? IsActive { get; set; }
		public int? mstUserGroupId { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string Recipient { get; set; }
		public string CC { get; set; }
	}
}