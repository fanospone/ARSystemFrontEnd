using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Helper;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Service.ARSystem.Invoice;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/PostingInvoiceTower")]

    public class ApiPostingInvoiceTowerController : ApiController
    {
        private trxPostingInvoiceTowerService _services;
        private trxPrintInvoiceTowerService _servicesPrint;

        public ApiPostingInvoiceTowerController()
        {
            _services = new trxPostingInvoiceTowerService();
            _servicesPrint = new trxPrintInvoiceTowerService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
            _servicesPrint.Dispose();
        }

        [HttpGet, Route("")]
        public IHttpActionResult GetPostingTowerDataToList(string strCompanyId, string Operator, string strInvoiceType, int intmstInvoiceStatusId, string invNo, int invoicemanual)
        {
            try
            {
                List<ARSystemService.vwDataCreatedInvoiceTower> DataCreatedInvoiceTower = new List<ARSystemService.vwDataCreatedInvoiceTower>();
                using (var client = new ARSystemService.ItrxPostingInvoiceTowerServiceClient())
                {
                    DataCreatedInvoiceTower = client.GetTrxPostingInvoiceTowerToList(UserManager.User.UserToken, strCompanyId, Operator, strInvoiceType, intmstInvoiceStatusId, invNo, invoicemanual, "trxInvoiceHeaderID", 0, 0).ToList();
                }

                return Ok(DataCreatedInvoiceTower);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetPostingTowerDataToGrid(PostTrxPostingInvoiceTowerView post)
        {
            try
            {
                List<vwDataCreatedInvoiceTower> DataCreatedInvoiceTower = new List<vwDataCreatedInvoiceTower>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    DataCreatedInvoiceTower.Add(new vwDataCreatedInvoiceTower(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(DataCreatedInvoiceTower);
                } 
                else
                {               
                    int intTotalRecord = 0;

                    intTotalRecord = _services.GetTrxPostingInvoiceTowerCount(userCredential.UserID, post.strCompanyId, post.strOperator, post.strInvoiceType, post.intmstInvoiceStatusId, post.invNo, post.invoiceManual);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    DataCreatedInvoiceTower = _services.GetTrxPostingInvoiceTowerToList(userCredential.UserID, post.strCompanyId, post.strOperator, post.strInvoiceType, post.intmstInvoiceStatusId, post.invNo, post.invoiceManual, strOrderBy, post.start, post.length).ToList();

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = DataCreatedInvoiceTower });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                pDisposeService();
            }
        }

        [HttpPost, Route("PostingInvoice")]
        public IHttpActionResult PostingInvoice(PostTrxCreateInvoiceTowerPosting post)
        {
            try
            {
                trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    InvoiceHeader = new trxInvoiceHeader(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(InvoiceHeader);
                } 
                else
                {
                    InvoiceHeader = _services.PostingInvoiceTower(userCredential.UserID, post.strInvoiceDate, post.strSubject, post.strOperatorRegionId, post.strSignature, post.DataCreatedInvoice, post.strAdditionalNote);

                    return Ok(InvoiceHeader);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                pDisposeService();
            }
        }

        [HttpPost, Route("CancelInvoice")]
        public IHttpActionResult CancelInvoice(PostTrxCreateInvoiceTowerPosting post)
        {
            try
            {
                trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    InvoiceHeader = new trxInvoiceHeader(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(InvoiceHeader);
                }
                else
                {
                    InvoiceHeader = _services.CancelPosting(userCredential.UserID, post.DataCreatedInvoice, post.strRemarksCancel, post.mstPICATypeID, post.mstPICADetailID);

                    return Ok(InvoiceHeader);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                pDisposeService();
            }
        }

        [HttpPost, Route("ApproveCancelInvoice")]
        public IHttpActionResult ApproveCancelInvoice(PostTrxCreateInvoiceTowerPosting post)
        {
            try
            {
                trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    InvoiceHeader = new trxInvoiceHeader(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(InvoiceHeader);
                }
                else
                {
                    InvoiceHeader = _services.ApproveCancelPosting(userCredential.UserID, post.DataCreatedInvoice);

                    return Ok(InvoiceHeader);
                }
               
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                pDisposeService();
            }
        }

        [HttpGet, Route("GetSubject")]
        public IHttpActionResult GetSubjectFromMatrix(int trxInvoiceHeaderId, int mstInvoiceCategoryId)
        {
            try
            {
                ARSystemService.vmStringResult Subject = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxPostingInvoiceTowerServiceClient())
                {
                    Subject = client.GetSubjectFromMatrix(UserManager.User.UserToken, trxInvoiceHeaderId, mstInvoiceCategoryId);
                }

                return Ok(Subject);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectCancelInvoice")]
        public IHttpActionResult RejectCancelInvoice(PostTrxCreateInvoiceTowerPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                int[] HeaderID = { post.DataCreatedInvoice.trxInvoiceHeaderID.Value };
                int[] CategoryId = { post.DataCreatedInvoice.mstInvoiceCategoryId.Value };
                vm.HeaderId = HeaderID;
                vm.CategoryId = CategoryId;
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.RejectCNARDataInvoiceTower(UserManager.User.UserToken, vm, "post");
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