
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstAccrueSettingAutoConfirm : BaseClass
	{
		public mstAccrueSettingAutoConfirm()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstAccrueSettingAutoConfirm(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int Week { get; set; }
		public DateTime Period { get; set; }
		public int AccrueStatusID { get; set; }
		public DateTime AutoConfirmDate { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}