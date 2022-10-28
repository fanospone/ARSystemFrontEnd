
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstSTIP : BaseClass
	{
		public mstSTIP()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstSTIP(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int STIPID { get; set; }
		public string STIPCode { get; set; }
		public string STIPDescription { get; set; }
		public bool? IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}