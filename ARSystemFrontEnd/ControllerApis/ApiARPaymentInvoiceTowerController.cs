using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service.ARSystem.Invoice;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.ViewModels.Datatable;
using System.Globalization;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ARPaymentInvoiceTower")]
    public class ApiARPaymentInvoiceTowerController : ApiController
    {
        private trxARPaymentInvoiceTowerService _services;

        public ApiARPaymentInvoiceTowerController()
        {
            _services = new trxARPaymentInvoiceTowerService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetARPaymentDataToGrid(PostARPaymentInvoiceTowerView post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmARPaymentInvoiceTower data = new vmARPaymentInvoiceTower(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    int intTotalRecord = 0;
                    List<vmARPaymentInvoiceTower> list = new List<vmARPaymentInvoiceTower>();

                    intTotalRecord = _services.GetARPaymentInvoiceTowerCount(UserManager.User.UserToken, post.invoiceTypeId, post.Operator, post.invCompanyId, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = _services.GetARPaymentInvoiceTowerToList(userCredential.UserID, post.invoiceTypeId, post.Operator, post.invCompanyId, post.invNo, strOrderBy, post.start, post.length).ToList();

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
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

        [HttpGet, Route("GetValidateIncludedAmount")]
        public IHttpActionResult GetValidateIncludedAmount(int HeaderId,int CategoryId)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmARPaymentInvoiceTower data = new vmARPaymentInvoiceTower(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    vmStringResult result = new vmStringResult();

                    result = _services.ValidateIncludedAmount(UserManager.User.UserToken, HeaderId, CategoryId);

                    return Ok(result);
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

        [HttpPost, Route("SavePayment")]
        public IHttpActionResult SavePayment(PostARPaymentInvoiceTowerProcess post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmARPaymentInvoiceTower data = new vmARPaymentInvoiceTower(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {

                    vmARPaymentInvoiceTower vm = new vmARPaymentInvoiceTower();
                    vm.trxInvoiceHeaderID = post.trxInvoiceHeaderId;
                    vm.mstInvoiceCategoryId = post.mstInvoiceCategoryId;
                    vm.InvPaidDate = Convert.ToDateTime(post.InvPaidDate);
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
                    vm.isPPHFinal = post.IsPPHFInal;
                    vm.InvoiceMatchingAR = post.InvoiceMatchingAR;

                    trxInvoiceHeader result = new trxInvoiceHeader();

                    result = _services.SavePayment(userCredential.UserID, vm);

                    return Ok(result);
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

        [HttpPost, Route("CNInvoiceToARProcess")]
        public IHttpActionResult CNInvoiceToARProcess(PostARPaymentInvoiceTowerProcess post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxInvoiceHeader data = new trxInvoiceHeader(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();

                    InvoiceHeader = _services.BackToARProcess(userCredential.UserID, post.trxInvoiceHeaderId, post.mstInvoiceCategoryId, post.strRemarks);

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

        [HttpPost, Route("docPaymentSAP")]
        public IHttpActionResult GetDocumentPaymentSAP(PostARPaymentInvoiceTowerProcess post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwstgTRStatusPenerimaanPembayaran data = new vwstgTRStatusPenerimaanPembayaran(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    List<vwstgTRStatusPenerimaanPembayaran> result = _services.GetDocumentPaymentSAP(userCredential.UserID, post.vDocumentPayment, post.vCompanyCode, post.vTglUangMasuk, post.InvPaidDate).ToList();

                    return Ok(new { data = result });
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
    }
}