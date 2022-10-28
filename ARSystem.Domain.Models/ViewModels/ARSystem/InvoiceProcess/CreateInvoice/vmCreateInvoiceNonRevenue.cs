using ARSystem.Domain.Models.ViewModels.Datatable;

using System.Collections.Generic;

namespace ARSystem.Domain.Models.ViewModels.ARSystem
{
    public class vmCreateInvoiceNonRevenue : DatatableAjaxModel
    {
        public string vCompany { get; set; }
        public string vOperator { get; set; }
        public List<int> ListId { get; set; }
        public int trxInvoiceNonRevenueID { get; set; }
        public List<string> strSONumber { get; set; }
        public string vInvNo { get; set; }
        public int CategoryInvoiceID { get; set; } = 0;
    }
}
