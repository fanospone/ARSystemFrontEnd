
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class Notification : BaseClass
	{
		public Notification()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public Notification(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long Id { get; set; }
		public int? Type { get; set; }
		public string Details { get; set; }
		public string Title { get; set; }
		public string DetailsURL { get; set; }
		public string SentTo { get; set; }
		public DateTime? Date { get; set; }
		public bool? IsRead { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsReminder { get; set; }
		public string Code { get; set; }
		public string NotificationType { get; set; }
		public DateTime? UpdateDate { get; set; }
		public string UpdateBy { get; set; }
	}
}