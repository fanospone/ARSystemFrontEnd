using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystem.Service.ARSystem.Invoice;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels.ARSystem;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/CreateInvoiceNonRevenue")]
    public class ApiCreateInvoiceNonRevenueController : ApiController
    {
        private NonRevenueService _services;
        private readonly InvoiceNonSonumbService _InvNonSonumbService;

        public ApiCreateInvoiceNonRevenueController()
        {
            _services = new NonRevenueService();
            _InvNonSonumbService = new InvoiceNonSonumbService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
            _InvNonSonumbService.Dispose();
        }

        #region GetData
        [HttpPost, Route("grid")]
        public IHttpActionResult GetSummaryData(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataInvoiceNonRevenue dataNonRevenue = new vwDataInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    Datatable<vwDataInvoiceNonRevenue> result = _services.GetDataNonRevenue(userCredential.UserID, param);

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

        [HttpPost, Route("gridHistory")]
        public IHttpActionResult GetSummaryHistory(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataInvoiceNonRevenue site = new vwDataInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(site);
                }
                else
                {
                    Datatable<vwDataInvoiceNonRevenue> result = _services.GetDataHistory(userCredential.UserID, param);

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

        [HttpPost, Route("gridSite")]
        public IHttpActionResult GetCreateInvoiceNonRevenue(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    if (param.strSONumber == null)
                    {
                        param.strSONumber = new List<string>();
                    }

                    Datatable<vwDataCreateInvoiceNonRevenue> result = _services.GetDataSiteNonRevenue(userCredential.UserID, param, param.strSONumber.ToList());

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

        [HttpPost, Route("gridSummarySite")]
        public IHttpActionResult GetSummarySite(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxInvoiceNonRevenueSite site = new trxInvoiceNonRevenueSite(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(site);
                }
                else
                {
                    var list = new List<trxInvoiceNonRevenueSite>();
                    list = _services.GetSiteNonRevenueByID(userCredential.UserID, param);

                    return Ok(list);
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

        [HttpPost, Route("getListID")]
        public IHttpActionResult GetListID(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    List<int> data = new List<int>();

                    data = _services.GetListID(userCredential.UserID, param);

                    return Ok(data);
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

        [HttpPost, Route("gridSiteCheckList")]
        public IHttpActionResult GetSiteCheckList(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    List<vwDataCreateInvoiceNonRevenue> data = new List<vwDataCreateInvoiceNonRevenue>();

                    data = _services.GetSiteCheckList(userCredential.UserID, param.ListId).ToList();

                    return Ok(data);
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

        #region Processing Data
        [HttpPost, Route("createInvoice")]
        public IHttpActionResult CreateInvoice(PostCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    var trx = new trxInvoiceNonRevenue();
                    trx.CompanyID = param.vCompany;
                    trx.OperatorID = param.vOperator;
                    trx.Amount = param.vAmount;
                    trx.Discount = param.vDiscount;
                    trx.DPP = param.vDPP;
                    trx.TotalPPN = param.vTotalPPN;
                    trx.TotalPPH = param.vTotalPPH;
                    trx.Penalty = param.vPenalty;
                    trx.InvoiceTotal = param.vInvoiceTotal;
                    trx.IsPPN = param.IsPPN;
                    trx.IsPPH = param.IsPPH;
                    trx.IsPPHFinal = param.IsPPHFinal;
                    trx.mstCategoryInvoiceID = param.mstCategoryInvoiceID;

                    trx = _services.CreateInvoiceNonRevenue(userCredential.UserID, trx, param.ListInvoiceNonRevenueSite);

                    return Ok(trx);
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

        [HttpPost, Route("updateInvoice")]
        public IHttpActionResult UpdateInvoice(PostCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    var trx = new trxInvoiceNonRevenue();
                    trx.CompanyID = param.vCompany;
                    trx.OperatorID = param.vOperator;
                    trx.Amount = param.vAmount;
                    trx.Discount = param.vDiscount;
                    trx.DPP = param.vDPP;
                    trx.TotalPPN = param.vTotalPPN;
                    trx.TotalPPH = param.vTotalPPH;
                    trx.Penalty = param.vPenalty;
                    trx.InvoiceTotal = param.vInvoiceTotal;
                    trx.IsPPN = param.IsPPN;
                    trx.IsPPH = param.IsPPH;
                    trx.InvSubject = param.vDescription;
                    trx.mstCategoryInvoiceID = param.mstCategoryInvoiceID;

                    trx = _services.UpdateInvoiceNonRevenue(userCredential.UserID, trx, param.ListInvoiceNonRevenueSite, param.vtrxInvoiceNonRevenueID);

                    return Ok(trx);
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


        #region InvoiceNonSonumb

        [HttpGet, Route("CategoryInvoiceList")]
        public IHttpActionResult CategoryInvoiceList()
        {
            try
            {
                var categoryList = _InvNonSonumbService.GetCategoryInvoiceList(UserManager.User.UserID);
                return Ok(categoryList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridSiteInvoiceNonSonumb")]
        public IHttpActionResult gridSiteInvoiceNonSonumb(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    if (param.strSONumber == null)
                    {
                        param.strSONumber = new List<string>();
                    }

                    Datatable<vwDataCreateInvoiceNonRevenue> result = _InvNonSonumbService.GetDataSiteNonRevenue(userCredential.UserID, param, param.strSONumber.ToList());

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

        [HttpPost, Route("gridSiteCheckInvoiceNonSonumbList")]
        public IHttpActionResult GetSiteCheckInvoiceNonSonumbList(vmCreateInvoiceNonRevenue param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwDataCreateInvoiceNonRevenue dataNonRevenue = new vwDataCreateInvoiceNonRevenue(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(dataNonRevenue);
                }
                else
                {
                    List<vwDataCreateInvoiceNonRevenue> data = new List<vwDataCreateInvoiceNonRevenue>();

                    data = _InvNonSonumbService.GetSiteCheckList(userCredential.UserID, param.ListId).ToList();

                    return Ok(data);
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