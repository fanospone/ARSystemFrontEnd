using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.RevenueAssurance
{
    public class trxRALogActivity : BaseClass
    {
        public trxRALogActivity()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public trxRALogActivity(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public long ID { get; set; }
        public string UserID { get; set; }
        public int? mstRAActivityID { get; set; }
        public long? TransactionID { get; set; }
        public string Remarks { get; set; }
        public DateTime? LogDate { get; set; }
        public bool? LogState { get; set; }
        public string Label { get; set; }
        public string SONumber { get; set; }

    }
}
