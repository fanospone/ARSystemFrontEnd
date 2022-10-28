
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRANewBapsActivity : BaseClass
	{
		public trxRANewBapsActivity()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRANewBapsActivity(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
         public int mstRAActivityID { get; set; }
        public string SoNumber { get; set; }
        public int SIRO { get; set; }
        public int PowerType { get; set; }
        public int BapsType { get; set; }
        public bool? CheckListDoc { get; set; }
		public bool? BapsValidation { get; set; }
		public bool? BapsPrint { get; set; }
		public bool? BapsInput { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
        
    }
}