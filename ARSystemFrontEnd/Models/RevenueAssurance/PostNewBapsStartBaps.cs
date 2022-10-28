using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostNewBapsStartBaps : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strCustomerId { get; set; }
        public string strGroupBy { get; set; }
        public string strProductId { get; set; }
        public string strBapsTypeId { get; set; }
        public string strBulkNumber { get; set; }
        public string strRegionId { get; set; }
        public string strSoNumber { get; set; }
        public string strStipCategory { get; set; }
        public int strBulkID { get; set; }
        public string strSiteID { get; set; }
        public string strDataType { get; set; }
        public int strIDTrx { get; set; }
        public string strCategory { get; set; }
        public int strStipSiro { get; set; }
        public string strStipID { get; set; }
        public int strActID { get; set; }
        public string strStatusAppr { get; set; }

        // =================== validation =============================//
        //public DateTime? strIssuingDate { get; set; }
        //public string strPoNumber { get; set; }
        //public DateTime? strPoDate { get; set; }
        //public string strMLAnumber { get; set; }
        //public DateTime? strMLAdate { get; set; }
        //public string strBaukNumber { get; set; }
        //public DateTime? strBaukdate { get; set; }
        //public DateTime? strStartLeasePriod { get; set; }
        //public DateTime? strEndLeasePriod { get; set; }
        //public string strInvoiceTypeID { get; set; }
        //public DateTime? strStartEffectivePeriod { get; set; }
        //public DateTime? strEndEffectivePeriod { get; set; }
        //public decimal strBaseLeasePrice { get; set; }
        //public decimal strServicePrice { get; set; }
        //public decimal strTotalLeasePrice { get; set; }
        //public decimal strInitialPoAmount { get; set; }
        //public decimal strPoAmount { get; set; }
        //public string strCorresRegionOpr { get; set; }
        //public string strApprOprID { get; set; }
        //public string strAction { get; set; }
        //public string strTenantTypeID { get; set; }
        //public string strBapsType { get; set; }
        //public string strSiteIDCustomer { get; set; }
        // =================== end validation =============================//
        // == XL bulky == //             

        //public ARSystemService.trxRABapsPrintXLBulky XlBulky = new ARSystemService.trxRABapsPrintXLBulky();

        //public ARSystemService.trxRABapsBulkyValidation validateBulky = new ARSystemService.trxRABapsBulkyValidation();

        //public ARSystemService.trxRABapsValidation validation = new ARSystemService.trxRABapsValidation();

        //public ARSystemService.mstRAGeneratorPDF generatePdf = new ARSystemService.mstRAGeneratorPDF();

        //public List<ARSystemService.trxRABapsPrintXLBulky> XlBulkyList = new List<ARSystemService.trxRABapsPrintXLBulky>();

        //public ARSystemService.mstBaps bapsValidation = new ARSystemService.mstBaps();

        //public ARSystemService.trxRABapsPrintXLAdd xlAdditional = new ARSystemService.trxRABapsPrintXLAdd();




        public List<ARSystemService.trxRABapsMaterials> trxRABapsMaterialsList = new List<ARSystemService.trxRABapsMaterials>();

        public ARSystemService.trxRABapsMaterials trxRABapsMaterials = new ARSystemService.trxRABapsMaterials();

        public ARSystemService.trxRABapsPrintXLBulky trxRABapsPrintXLBulky = new ARSystemService.trxRABapsPrintXLBulky();

        public List<ARSystemService.trxRABapsPrintXLBulky> trxRABapsPrintXLBulkyList = new List<ARSystemService.trxRABapsPrintXLBulky>();

        public ARSystemService.trxRABapsBulkyValidation trxRABapsBulkyValidation = new ARSystemService.trxRABapsBulkyValidation();

        public List<ARSystemService.trxRABapsBulkyValidation> trxRABapsBulkyValidationList = new List<ARSystemService.trxRABapsBulkyValidation>();

        public ARSystemService.trxRABapsPrintXLAdd trxRABapsPrintXLAdd = new ARSystemService.trxRABapsPrintXLAdd();

        public List<ARSystemService.trxRABapsPrintXLAdd> trxRABapsPrintXLAddList = new List<ARSystemService.trxRABapsPrintXLAdd>();

        public ARSystemService.mstRAGeneratorPDF mstRAGeneratorPDF = new ARSystemService.mstRAGeneratorPDF();

        public ARSystemService.trxRABapsValidation trxRABapsValidation = new ARSystemService.trxRABapsValidation();

        public List<ARSystemService.trxRABapsValidation> trxRABapsValidationList = new List<ARSystemService.trxRABapsValidation>();

        public ARSystemService.vwRABapsPrintSonumbList vwRABapsPrintSonumbList = new ARSystemService.vwRABapsPrintSonumbList();

        public ARSystemService.trxRABapsHeightSpace trxRABapsHeightSpace = new ARSystemService.trxRABapsHeightSpace();


    }

}