using ARSystem.Domain.Models.ViewModels.Datatable;

using System.Collections.Generic;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmAllocatePayment : DatatableAjaxModel
    {
        public int vtrxAllocatePaymentBankInID { get; set; }
        public string vStatus { get; set; }
        public string vCompany { get; set; }
        public string vOperator { get; set; }
        public string vStartPaid { get; set; }
        public string vEndPaid { get; set; }
        public decimal vAmount { get; set; }
        public decimal vAmountBankOutExs { get; set; }
    }
}
