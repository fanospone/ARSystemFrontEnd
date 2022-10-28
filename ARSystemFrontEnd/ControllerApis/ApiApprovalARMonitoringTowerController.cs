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
    [RoutePrefix("api/ApprovalARMonitoringTower")]
    public class ApiApprovalARMonitoringTowerController : ApiController
    {
        // GET: ApiApprovalARMonitoringTower
        [HttpPost, Route("grid")]
        public IHttpActionResult GetApprovalARMonitoringInvoiceTowerDataToGrid(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwApprovalARMonitoringInvoiceTower> ApprovalInvoiceData = new List<ARSystemService.vwApprovalARMonitoringInvoiceTower>();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxApprovalARMonitoringInvoiceTowerCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ApprovalInvoiceData = client.GetTrxApprovalARMonitoringInvoiceTowerToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ApprovalInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GenerateToCollection")]
        public IHttpActionResult GenerateToCollection(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.ListHeaderId.ToArray();
                vm.CategoryId = post.ListCategoryId.ToArray();
                vm.BatchNumber = post.ListBatchNumber.ToArray();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    InvoiceHeader = client.GenerateToCollectionTower(UserManager.User.UserToken, vm);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ReOpenInvoice")]
        public IHttpActionResult ReOpenInvoice(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                //Helper.Helper.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["DocPath"].ToString(), post.filePath);
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    InvoiceHeader = client.ReOpenInvoiceTower(UserManager.User.UserToken,post.HeaderId,post.CategoryId,post.strRemarks);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridByCollection")]
        public IHttpActionResult GetApprovalARMonitoringCollectionTowerDataToGrid(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwApprovalARMonitoringCollectionTower> ApprovalInvoiceData = new List<ARSystemService.vwApprovalARMonitoringCollectionTower>();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxApprovalARMonitoringCollectionTowerCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo,post.strStatusGenerate);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ApprovalInvoiceData = client.GetTrxApprovalARMonitoringCollectionTowerToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo, post.strStatusGenerate, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ApprovalInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GenerateToAX")]
        public IHttpActionResult GenerateToAX(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.ListHeaderId.ToArray();
                vm.CategoryId = post.ListCategoryId.ToArray();
                vm.BatchNumber = post.ListBatchNumber.ToArray();
                vm.BatchNumber = post.ListBatchNumber.ToArray();
                vm.mstPaymentCodeId = post.ListPaymentCodeId.ToArray();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    InvoiceHeader = client.GenerateToAXTower(UserManager.User.UserToken, vm);
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
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    if (post.isCollection)
                        Result = client.GetTrxApprovalARMonitoringCollectionTowerListId(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo, post.strStatusGenerate).ToList();
                    else
                        Result = client.GetTrxApprovalARMonitoringTowerListId(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.invNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetTotalAmount")]
        public IHttpActionResult GetApprovalARMonitoringTowerAmountPaid(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                decimal TotalAmount = 0.00m;
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.ListHeaderId.ToArray();
                vm.CategoryId = post.ListCategoryId.ToArray();
                vm.BatchNumber = post.ListBatchNumber.ToArray();
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    TotalAmount = client.GetTotalApprovalARMonitoringTowerAmountPaid(UserManager.User.UserToken, vm);
                }

                return Ok(TotalAmount);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ReOpenInvoiceToPayment")]
        public IHttpActionResult ReOpenInvoiceToPayment(PostTrxApprovalARMonitoringTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                //Helper.Helper.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["DocPath"].ToString(), post.filePath);
                using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
                {
                    InvoiceHeader = client.ReOpenInvoiceTowerToPayment(UserManager.User.UserToken, post.HeaderId, post.CategoryId, post.strRemarks);
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