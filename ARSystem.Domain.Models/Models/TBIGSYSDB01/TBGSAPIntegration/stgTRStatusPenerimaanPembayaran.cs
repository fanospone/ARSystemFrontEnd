
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration
{
	public class stgTRStatusPenerimaanPembayaran : BaseClass
	{
		public stgTRStatusPenerimaanPembayaran()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public stgTRStatusPenerimaanPembayaran(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public string Rekeningkoranid { get; set; }
		public string Documentpayment { get; set; }
		public string Companycode { get; set; }
		public string Tanggaluangmasuk { get; set; }
		public decimal? Totalpayment { get; set; }
		public string Currency { get; set; }
		public string Namabank { get; set; }
		public string Nomorrekening { get; set; }
		public string Keterangan { get; set; }
		public string Entrydate { get; set; }
		public string Entrytime { get; set; }
		public string MessageType { get; set; }
		public string Message { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}