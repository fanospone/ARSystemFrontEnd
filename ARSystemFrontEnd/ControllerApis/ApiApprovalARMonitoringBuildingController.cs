using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/ApprovalARMonitoringBuilding")]
    public class ApiApprovalARMonitoringBuildingController : ApiController
    {
        // GET: ApiApprovalARMonitoringBuilding
        [HttpPost, Route("grid")]
        public IHttpActionResult GetApprovalARMonitoringInvoiceBuildingDataToGrid(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwApprovalARMonitoringInvoiceBuilding> ApprovalInvoiceData = new List<ARSystemService.vwApprovalARMonitoringInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    intTotalRecord = client.GetTrxApprovalARMonitoringInvoiceBuildingCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ApprovalInvoiceData = client.GetTrxApprovalARMonitoringInvoiceBuildingToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod,post.strCompanyId, post.invNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ApprovalInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GenerateToCollection")]
        public IHttpActionResult GenerateToCollection(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.ListHeaderId.ToArray();
                vm.CategoryId = post.ListCategoryId.ToArray();
                vm.BatchNumber = post.ListBatchNumber.ToArray();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    InvoiceHeader = client.GenerateToCollectionBuilding(UserManager.User.UserToken, vm);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ReOpenInvoice")]
        public IHttpActionResult ReOpenInvoice(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                //Helper.Helper.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["DocPath"].ToString(), post.filePath);
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    InvoiceHeader = client.ReOpenInvoiceBuilding(UserManager.User.UserToken,post.HeaderId,post.CategoryId,post.strRemarks);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridByCollection")]
        public IHttpActionResult GetApprovalARMonitoringCollectionBuildingDataToGrid(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwApprovalARMonitoringCollectionBuilding> ApprovalInvoiceData = new List<ARSystemService.vwApprovalARMonitoringCollectionBuilding>();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    intTotalRecord = client.GetTrxApprovalARMonitoringCollectionBuildingCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo, post.strStatusGenerate);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ApprovalInvoiceData = client.GetTrxApprovalARMonitoringCollectionBuildingToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo, post.strStatusGenerate, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ApprovalInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GenerateToAX")]
        public IHttpActionResult GenerateToAX(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.ListHeaderId.ToArray();
                vm.CategoryId = post.ListCategoryId.ToArray();
                vm.BatchNumber = post.ListBatchNumber.ToArray();
                vm.mstPaymentCodeId = post.ListPaymentCodeId.ToArray();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    InvoiceHeader = client.GenerateToAXBuilding(UserManager.User.UserToken, vm);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                List<ARSystemService.vmCheckAllApprovalARMonitoring> Result = new List<ARSystemService.vmCheckAllApprovalARMonitoring>();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    if(post.isCollection)
                        Result = client.GetTrxApprovalARMonitoringCollectionBuildingListId(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo,post.strStatusGenerate).ToList();
                    else
                        Result = client.GetTrxApprovalARMonitoringBuildingListId(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetPaidAmount")]
        public IHttpActionResult GetPaidAmount(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                ARSystemService.vmGetInvoicePostedList checkedItems = new ARSystemService.vmGetInvoicePostedList();
                checkedItems.BatchNumber = post.ListBatchNumber.ToArray();
                checkedItems.CategoryId = post.ListCategoryId.ToArray();
                checkedItems.HeaderId = post.ListHeaderId.ToArray();
                decimal paidAmount = 0.00m;
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    paidAmount = client.GetApprovalARMonitoringBuildingAmountPaid(UserManager.User.UserToken, checkedItems);
                }

                return Ok(paidAmount);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ReOpenInvoiceToPayment")]
        public IHttpActionResult ReOpenInvoiceToPayment(PostTrxApprovalARMonitoringBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                //Helper.Helper.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["DocPath"].ToString(), post.filePath);
                using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
                {
                    InvoiceHeader = client.ReOpenInvoiceBuildingToPayment(UserManager.User.UserToken, post.HeaderId, post.CategoryId, post.strRemarks);
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