using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Helper;
using ARSystem.Service.ARSystem;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/ApprovalMonitoringAR")]
    public class ApiApprovalMonitoringARController : ApiController
    {
        private ApprovalMonitoringARService _services;

        public ApiApprovalMonitoringARController()
        {
            _services = new ApprovalMonitoringARService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        #region GetData
        [HttpPost, Route("invoiceGrid")]
        public IHttpActionResult GetSummaryInvoice(vmInvoiceMatchingAR param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwReportInvoiceMatchingAR data = new vwReportInvoiceMatchingAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    Datatable<vwReportInvoiceMatchingAR> result = _services.GetDataInvoiceMatchingAR(userCredential.UserID, param);

                    return Ok(new { draw = param.draw, recordsTotal = result.Count, recordsFiltered = result.Count, data = result.List });
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

        [HttpPost, Route("collectionGrid")]
        public IHttpActionResult GetSummaryCollection(vmInvoiceMatchingAR param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwtrxInvoiceMatchingAR data = new vwtrxInvoiceMatchingAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    Datatable<vwtrxInvoiceMatchingAR> result = _services.GetDataCollectionMatchingAR(userCredential.UserID, param);

                    return Ok(new { data = result.List });
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

        [HttpPost, Route("docPaymentLog")]
        public IHttpActionResult GetDocumentPaymentLog()
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwMatchingARLogDocumentPayment data = new vwMatchingARLogDocumentPayment(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    List<vwMatchingARLogDocumentPayment> result = _services.GetDocumentPaymentLog(userCredential.UserID).ToList();

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

        [HttpGet, Route("DocumentPayment")]
        public IHttpActionResult GetDocumentPayment()
        {
            try
            {
                List<stgTRStatusPenerimaanPembayaran> list = new List<stgTRStatusPenerimaanPembayaran>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    stgTRStatusPenerimaanPembayaran data = new stgTRStatusPenerimaanPembayaran(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    list = _services.GetDocumentPaymentList(userCredential.UserID);
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Status")]
        public IHttpActionResult GetStatus()
        {
            try
            {
                List<stgTRStatusPenerimaanPembayaran> list = new List<stgTRStatusPenerimaanPembayaran>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    stgTRStatusPenerimaanPembayaran data = new stgTRStatusPenerimaanPembayaran(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    list = _services.GetDocumentPaymentList(userCredential.UserID);
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #region Processing Data
        [HttpPost, Route("insertToStg")]
        public IHttpActionResult CreateInvoice(vmInvoiceMatchingAR param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    stgTRStatusMatchingAR dataNonRevenue = new stgTRStatusMatchingAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    stgTRStatusMatchingAR result = _services.GenerateToSAP(userCredential.UserID, param.ListMatchingARCollection);

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

        [HttpPost, Route("updateDocPaymentOther")]
        public IHttpActionResult UpdateDocumentPaymentOther(vmInvoiceMatchingAR param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxInvoiceMatchingAR data = new trxInvoiceMatchingAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    trxInvoiceMatchingAR result = _services.UpdateDocumentPaymentOther(userCredential.UserID, param.trxInvoiceMatchingARID, param.DocumentPaymentLog);

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
        #endregion
    }
}