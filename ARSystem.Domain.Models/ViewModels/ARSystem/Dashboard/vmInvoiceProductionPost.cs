using ARSystem.Domain.Models.ViewModels.Datatable;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmInvoiceProductionPost : DatatableAjaxModel
    {
        public string vOperator { get; set; }
        public string vCompany { get; set; }
        public string vBAPSReceiveStart { get; set; }
        public string vBAPSReceiveEnd { get; set; }
        public string vAgingCategory { get; set; }
        public string vInvoiceCategory { get; set; }
        public string vInvoiceDateStart { get; set; }
        public string vInvoiceDateEnd { get; set; }
        public string vType { get; set; }
        public int vGroup { get; set; }
        public string vPKP { get; set; }
    }
}
