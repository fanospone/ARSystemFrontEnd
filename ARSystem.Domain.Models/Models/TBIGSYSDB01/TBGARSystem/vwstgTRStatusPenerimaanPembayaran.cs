
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwstgTRStatusPenerimaanPembayaran : BaseClass
	{
		public vwstgTRStatusPenerimaanPembayaran()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwstgTRStatusPenerimaanPembayaran(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string Documentpayment { get; set; }
        public string RekeningKoranid { get; set; }
        public string Companycode { get; set; }
		public string Tanggaluangmasuk { get; set; }
		public decimal? Totalpayment { get; set; }
		public string Currency { get; set; }
		public string Namabank { get; set; }
		public string Nomorrekening { get; set; }
		public string Keterangan { get; set; }
        public int? Status { get; set; }
	}
}