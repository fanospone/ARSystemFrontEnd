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
using ARSystem.Domain.Models.ViewModels;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/AllocatePayment")]
    public class ApiAllocatePaymentController : ApiController
    {
        private AllocatePaymentService _services;

        public ApiAllocatePaymentController()
        {
            _services = new AllocatePaymentService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        #region GetData
        [HttpPost, Route("grid")]
        public IHttpActionResult GetSummaryData(vmAllocatePayment param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwtrxAllocatePayment data = new vwtrxAllocatePayment(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    Datatable<vwtrxAllocatePayment> result = _services.GetDataAllocatePayment(userCredential.UserID, param);

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

        [HttpPost, Route("gridBankOut")]
        public IHttpActionResult GetDataBankOut(vmAllocatePayment param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxAllocatePaymentBankOut data = new trxAllocatePaymentBankOut(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    Datatable<trxAllocatePaymentBankOut> result = _services.GetDataBankOut(userCredential.UserID, param);

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

        [HttpPost, Route("validateVAR")]
        public IHttpActionResult ValidateVAR(vmAllocatePayment param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxAllocatePaymentBankOut data = new trxAllocatePaymentBankOut(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    string result = _services.ValidateAmount(userCredential.UserID, param);
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

        [HttpGet, Route("getVariance")]
        public IHttpActionResult GetVariance(int vtrxAllocatePaymentBankInID)
        {
            try
            {
                decimal result = 0;

                result = _services.getVariance(UserManager.User.UserToken, vtrxAllocatePaymentBankInID);

                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #region Processing Data
        [HttpPost, Route("createBankIn")]
        public IHttpActionResult CreateBankIn(trxAllocatePaymentBankIn param)
        {
            var trx = new trxAllocatePaymentBankIn();

            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trx = new trxAllocatePaymentBankIn(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(trx);
                }
                else
                {
                    trx = _services.CreateBankIn(userCredential.UserID, param);

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

        [HttpPost, Route("editBankIn")]
        public IHttpActionResult EditBankIn(trxAllocatePaymentBankIn param)
        {
            var trx = new trxAllocatePaymentBankIn();

            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trx = new trxAllocatePaymentBankIn(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(trx);
                }
                else
                {
                    trx = _services.EditBankIn(userCredential.UserID, param);

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

        [HttpGet, Route("deleteBankIn")]
        public IHttpActionResult deteleBankIn(int vtrxAllocatePaymentBankInID)
        {
            bool result = false;

            try
            {
                result = _services.deleteBankIn(UserManager.User.UserToken, vtrxAllocatePaymentBankInID);
                return Ok(result);
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

        [HttpPost, Route("addBankOut")]
        public IHttpActionResult AddBankOut(trxAllocatePaymentBankOut param)
        {
            var trx = new trxAllocatePaymentBankOut();

            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trx = new trxAllocatePaymentBankOut(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(trx);
                }
                else
                {
                    trx = _services.AddBankOut(userCredential.UserID, param);

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

        [HttpPost, Route("editBankOut")]
        public IHttpActionResult EditBankOut(trxAllocatePaymentBankOut param)
        {
            var trx = new trxAllocatePaymentBankOut();

            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trx = new trxAllocatePaymentBankOut(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(trx);
                }
                else
                {
                    trx = _services.EditBankOut(userCredential.UserID, param);

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

        [HttpGet, Route("deleteBankOut")]
        public IHttpActionResult deteleBankOut(int vtrxAllocatePaymentBankOutID)
        {
            bool result = false;

            try
            {
                result = _services.deleteBankOut(UserManager.User.UserToken, vtrxAllocatePaymentBankOutID);
                return Ok(result);
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