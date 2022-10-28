using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReconcileDataView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strRenewalYear { get; set; }
        public string strRenewalYearSeq { get; set; }
        public string strRegional { get; set; }
        public string strCurrency { get; set; }
        public string strProvince { get; set; }
        public string strDueDatePerMonth { get; set; }
        public string strReconcileType { get; set; }
        public int isRaw { get; set; }
        public List<long> soNumb { get; set; }
        public string Batch { get; set; }
        public List<string> strSONumber { get; set; }
        public string TenantType { get; set; }
        //public List<int> soNumb { get; set; }
    }

    public class PostReconcileDataUpdate
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        //public decimal? inflationRupiah { get; set; }
        //public decimal? inflationDolar { get; set; }
        //public decimal? additionalRupiah { get; set; }
        //public decimal? additionalDolar { get; set; }
        //public decimal? deductionPowerRupiah { get; set; }
        //public decimal? deductionPowerDolar { get; set; }
        //public decimal? penaltySlaRupiah { get; set; }
        //public decimal? penaltySlaDolar { get; set; }
        //public decimal? totalPaymentRupiah { get; set; }
        //public decimal? totalPaymentDolar { get; set; }

        public string CustomerID { get; set; }
        public string StartInvoiceDate { get; set; }
        public string EndInvoiceDate { get; set; }
        public string StartSplitDate { get; set; }
        public string EndSplitDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal ServiceAmount { get; set; }
        public decimal DropFODistance { get; set; }
        public int ProductID { get; set; }
    }

    public class PostProcessNextActivity
    {
        public string Id { get; set; }
        public string NextActivity { get; set; }
        public string ListID { get; set; }
        public List<long> mstRAScheduleId { get; set; }
        public string Department { get; set; }
        public string PICA { get; set; }
        public string PIC { get; set; }
        public string Remarks { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int IsActive { get; set; }
        public DateTime InvoiceDate { get; set; }
    }

    public class PostReconcileUpdateBulkyAmount
    {
        public List<long> ID { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? InflationAmount { get; set; }
        public decimal? AdditionalAmount { get; set; }
    }
}