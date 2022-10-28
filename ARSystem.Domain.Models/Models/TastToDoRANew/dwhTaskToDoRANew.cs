
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class dwhTaskToDoRANew : BaseClass
	{
		public dwhTaskToDoRANew()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public dwhTaskToDoRANew(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public byte Id { get; set; }
		public string TaskToDoName { get; set; }
		public int? CountData { get; set; }
		public string UrlPage { get; set; }
		public byte? Sort { get; set; }
	}
}