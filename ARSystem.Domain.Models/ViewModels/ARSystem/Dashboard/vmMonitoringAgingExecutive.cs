using ARSystem.Domain.Models.ViewModels.Datatable;

using System.Collections.Generic;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmMonitoringAgingExecutive : DatatableAjaxModel
    {
        public string vCompanyID { get; set; }
        public string vOperatorID { get; set; }
        public string vAmountType { get; set; }
        public string vPeriod { get; set; }
        public string vInvoiceType { get; set; }
        public int vCategory { get; set; }
        public string vPKP { get; set; }
    }
}
