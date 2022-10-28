
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstAccrueMappingSOW : BaseClass
	{
		public mstAccrueMappingSOW()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstAccrueMappingSOW(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string SOW { get; set; }
		public bool? Commercial { get; set; }
		public bool? Contract { get; set; }
		public bool? PO { get; set; }
		public bool? BAUK { get; set; }
		public bool? BAPS { get; set; }
		public bool? Invoice { get; set; }
		public string Type { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}