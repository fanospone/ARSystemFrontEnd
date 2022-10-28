using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;


namespace ARSystemFrontEnd.Models
{
    public class PostDashboardPotential : DatatableAjaxModel
    {
        public string Type { get; set; }
        public string STIPCategory { get; set; }

        public string RFIDateYear { get; set; }
        public string RFIDateMonth { get; set; }
        public string RFIDateWeek { get; set; }

        public string Step { get; set; }

        public string CustomerID { get; set; }


        public string SoNumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }

    }
}
 
//{ data: "POStep"},
//{ data: "RFIStep"},
//{ data: "BAUKStep"},
//{ data: "BAPSStep"},
//{ data: "SiteID"},
//{ data: "SiteName"},
//{ data: "SoNumber"},
//{ data: "CustomerSiteID"},
//{ data: "CustomerSiteName"},
//{ data: "CustomerID"},
//{ data: "RegionName"},
//{ data: "ProvinceName"},
//{ data: "ResidenceName"},
//{ data: "po_number"},
//{ data: "MLANumber"},
//{ data: "StartLeaseDate"},
//{ data: "EndLeaseDate"},
//{ data: "BapsType"},
//{ data: "CustomerInvoice"},
//{ data: "CompanyInvoice"},
//{ data: "CompanyInvoiceName"},
//{ data: "CompanyID"},
//{ data: "Currency"},
//{ data: "InvoiceStartDate"},
//{ data: "InvoiceEndDate"},
//{ data: "BaseLeasePrice"},
//{ data: "ServicePrice"},
//{ data: "DeductionAmount"},
//{ data: "AmountTotal"},
//{ data: "STIPCategory"},
//{ data: "RFIDate"}