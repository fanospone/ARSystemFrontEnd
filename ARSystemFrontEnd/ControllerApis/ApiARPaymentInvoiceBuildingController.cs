using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ARPaymentInvoiceBuilding")]
    public class ApiARPaymentInvoiceBuildingController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetARPaymentInvoiceBuildingToGrid(PostARPaymentInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmARPaymentInvoiceBuilding> list = new List<ARSystemService.vmARPaymentInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxARPaymentInvoiceBuildingServiceClient())
                {
                    intTotalRecord = client.GetARPaymentInvoiceBuildingCount(UserManager.User.UserToken, post.invoiceTypeId, post.invCompanyId, post.customerName, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetARPaymentInvoiceBuildingToList(UserManager.User.UserToken, post.invoiceTypeId, post.invCompanyId, post.customerName, post.invNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetValidateIncludedAmount")]
        public IHttpActionResult GetValidateIncludedAmount(int trxInvoiceHeaderID)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxARPaymentInvoiceBuildingServiceClient())
                {
                    Result = client.ValidateIncludedAmountInvoiceBuilding(UserManager.User.UserToken, trxInvoiceHeaderID);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SavePayment")]
        public IHttpActionResult SavePayment(PostARPaymentInvoiceBuildingProcess post)
        {
            try
            {
                ARSystemService.vmARPaymentInvoiceBuilding vm = new ARSystemService.vmARPaymentInvoiceBuilding();
                vm.trxInvoiceHeaderID = post.trxInvoiceHeaderId;
                vm.InvPaidDate = DateTime.Parse(post.InvPaidDate);
                vm.mstPaymentId = post.mstPaymentId;
                vm.InvPaidStatus = post.InvPaidStatus;
                vm.PAM = post.PAM;
                vm.RND = post.RND;
                vm.DBT = post.DBT;
                vm.RTGS = post.RTGS;
                vm.PNT = post.PNT;
                vm.PPE = post.PPE;
                vm.PPH = post.PPH;
                vm.PAT = post.PAT;
                vm.ARTotalPaid = post.ARTotalPaid;
                vm.PPF = post.PPF;
                vm.isPPHFinal = post.IsPPHFinal;
                ARSystemService.trxInvoiceHeader Result = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxARPaymentInvoiceBuildingServiceClient())
                {
                    Result = client.SavePaymentInvoiceBuilding(UserManager.User.UserToken, vm);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNInvoiceToARProcess")]
        public IHttpActionResult CNInvoiceToARProcess(PostARPaymentInvoiceBuildingProcess post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                //Helper.Helper.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["DocPath"].ToString(), post.filePath);
                using (var client = new ARSystemService.ItrxARPaymentInvoiceBuildingServiceClient())
                {
                    InvoiceHeader = client.BackToARProcessBuilding(UserManager.User.UserToken, post.trxInvoiceHeaderId, post.strRemarks);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}