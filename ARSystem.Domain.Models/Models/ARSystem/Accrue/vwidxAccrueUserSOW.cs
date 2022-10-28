
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwidxAccrueUserSOW : BaseClass
	{
		public vwidxAccrueUserSOW()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwidxAccrueUserSOW(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public string UserID { get; set; }
        public bool IsActive { get; set; }
        public string SOW { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}