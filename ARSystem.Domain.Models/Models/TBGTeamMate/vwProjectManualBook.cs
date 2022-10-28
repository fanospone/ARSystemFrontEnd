
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwProjectManualBook : BaseClass
	{
		public vwProjectManualBook()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwProjectManualBook(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ProjectID { get; set; }
		public string ProjectName { get; set; }
		public int? ApplicationID { get; set; }
		public string ApplicationName { get; set; }
		public string FileName { get; set; }
		public string DocumentName { get; set; }
		public string ContentType { get; set; }
		public string FilePath { get; set; }
		public string ProjectManagerID { get; set; }
		public string ProjectManager { get; set; }
		public string ProjectDescription { get; set; }
	}
}