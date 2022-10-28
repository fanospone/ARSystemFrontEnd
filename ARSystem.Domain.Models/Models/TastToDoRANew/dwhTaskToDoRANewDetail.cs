
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class dwhTaskToDoRANewDetail : BaseClass
	{
		public dwhTaskToDoRANewDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public dwhTaskToDoRANewDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public short Id { get; set; }
		public string CustomerId { get; set; }
		public string TaskToDoName { get; set; }
		public int? Stip1 { get; set; }
		public int? Stip267 { get; set; }
        public int? Others { get; set; }
    }
}