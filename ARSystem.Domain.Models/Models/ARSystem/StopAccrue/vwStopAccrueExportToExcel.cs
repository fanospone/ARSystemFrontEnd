using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwStopAccrueExportToExcel : BaseClass
    {
        public vwStopAccrueExportToExcel()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwStopAccrueExportToExcel(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string ActualStatus { get; set; }
        public string InputReqBy { get; set; }
        public string RequestNumber { get; set; }
        public string DepartmentName { get; set; }
        public string DirectorateCode { get; set; }
        public string DirectorateName { get; set; }
        public int? AppHeaderID { get; set; }
        public string Submit { get; set; }
        public string VerificationDivHead { get; set; }
        public string ActivtyDivHead { get; set; }
        public string VerifiedByChief { get; set; }
        public string ActivtyChief { get; set; }
        public string RecomendedByChiefOfMarketing { get; set; }
        public string ActivtyChiefMKT { get; set; }
        public string FeedbackDepHeadREALatest { get; set; }
        public string ActivtyDeptREALatest { get; set; }
        public string FeedbackDepHeadREA2ndLatest { get; set; }
        public string ActivtyDeptREA2ndlatest { get; set; }
        public string FeedbackFromDeptAcc { get; set; }
        public string ActivtyDeptACC { get; set; }
        public string FeedbackVerificationDivControllershipLatest { get; set; }
        public string ActivtyDivCONTLatest { get; set; }
        public string FeedbackVerificationDivControllership2ndlatest { get; set; }
        public string ActivtyDivCONT2ndLatest { get; set; }
        public string FeedbackFromDivAcc { get; set; }
        public string ActivtyDivACC { get; set; }
        public string SubmitDocument { get; set; }
        public string ActivtySubmitDoc { get; set; }
        public string DocumentReceiveByAccounting { get; set; }
        public string ActivtyDocACC { get; set; }
        public string DocumentReceiveByAset { get; set; }
        public string ActivtyDocAST { get; set; }
        public string Finish { get; set; }
        public string ActivtyFinish { get; set; }
        public string Rejected { get; set; }
        public string ActivtyReject { get; set; }

    }
}
