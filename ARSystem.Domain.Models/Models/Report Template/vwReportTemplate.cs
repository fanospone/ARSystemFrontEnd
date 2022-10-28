
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwReportTemplate : BaseClass
	{
		public vwReportTemplate()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReportTemplate(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string CustomerName { get; set; }
		public int ProductID { get; set; }
		public int ID { get; set; }
		public string CustomerID { get; set; }
		public string StipCategory { get; set; }
		public string ProductType { get; set; }
		public string PrintType { get; set; }
		public string HtmlString { get; set; }
		public string LogoPathLeft { get; set; }
		public string LogoPathRight { get; set; }
		public int? LogoLeftWidth { get; set; }
		public int? LogoLeftHeight { get; set; }
		public int? LogoRightWidth { get; set; }
		public int? LogoRightHeight { get; set; }
		public int? LogoRightXpos { get; set; }
		public int? LogoRightYpos { get; set; }
		public int? LogoLeftXpos { get; set; }
		public int? LogoLeftYpos { get; set; }
		public int? MarginLeft { get; set; }
		public int? MarginRight { get; set; }
		public int? MarginTop { get; set; }
		public int? MarginBottom { get; set; }
		public int? FooterHeight { get; set; }
		public int? HeaderHeight { get; set; }
		public int? QRCodeHeight { get; set; }
		public int? QRCodeWidth { get; set; }
		public int? QRCodeXpos { get; set; }
		public int? QRCodeYpos { get; set; }
		public int? PageNumberShow { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public bool? IsPotrait { get; set; }
		public bool? UseQR { get; set; }

        public string TextQrCode { get; set; }
        public string FileName { get; set; }
        public string FooterText { get; set; }
        public bool FooterTextShow { get; set; }
    }
}