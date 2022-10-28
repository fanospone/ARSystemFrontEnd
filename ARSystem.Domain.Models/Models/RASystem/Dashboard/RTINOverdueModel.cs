using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class RTINOverdueModel
    {

        // public List<Dictionary<string, string>> Data { get; set; }
        public decimal RTI { get; set; }
        public decimal OverDue { get; set; }
        public decimal NearlyOverDue { get; set; }
        public string Month { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerID { get; set; }
        public string Status { get; set; }

    }

    public class vwRTINOverdueModel : BaseClass
    {
        public vwRTINOverdueModel()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwRTINOverdueModel(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public List<RTINOverdueModel> dataChart = new List<RTINOverdueModel>();

        public List<SummaryData> vwSumData = new List<SummaryData>();
        //public List<SummaryData> vwSumData = new List<SummaryData>();

        //public List<Operator> OperatorList = new List<Operator>();
        public string CustomerID { get; set; }
        public decimal RTI { get; set; }
        public decimal RTINearly { get; set; }
        public decimal OverDue { get; set; }
        public decimal NearlyOverDue { get; set; }


    }

    public class SummaryData
    {
        public string CustomerID { get; set; }
        public decimal RTI { get; set; }
        public decimal RTINearly { get; set; }
        public decimal OverDue { get; set; }
        public decimal NearlyOverDue { get; set; }

    }

    public class SummaryData2
    {
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Operator> Operators { get; set; }

    }
    public class Operator
    {
        public string Status { get; set; }
        public string CustomerID { get; set; }
        public decimal Amount { get; set; }
    }
}
