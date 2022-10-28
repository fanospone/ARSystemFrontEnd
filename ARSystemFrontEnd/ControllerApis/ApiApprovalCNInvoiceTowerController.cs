using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

using ARSystemFrontEnd.Helper;
using ARSystem.Service.ARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ApprovalCNInvoiceTower")]
    public class ApiApprovalCNInvoiceTowerController : ApiController
    {
        private trxApprovalCNInvoiceTowerService _services;

        public ApiApprovalCNInvoiceTowerController()
        {
            _services = new trxApprovalCNInvoiceTowerService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetChecklistInvoiceTowerToGrid(PostTrxCNInvoiceTowerView post)
        {
            try
            {
               
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwCNInvoiceTower data = new vwCNInvoiceTower(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    List<vwCNInvoiceTower> data = new List<vwCNInvoiceTower>();

                    int intTotalRecord = 0;

                    intTotalRecord = _services.GetApprovalCNInvoiceTowerCount(userCredential.UserID, post.invoiceTypeId, post.companyId, post.operatorId, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = _services.GetApprovalCNInvoiceTowerToList(userCredential.UserID, post.invoiceTypeId, post.companyId, post.operatorId, post.invNo, strOrderBy, post.start, post.length).ToList();

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
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

        [HttpPost, Route("ApprovalCNDeptHead")]
        public IHttpActionResult ApprovalCNDeptHeadInvoiceTower(PostApproveCNInvoiceTower post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxPICAAR data = new trxPICAAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    var invoice = new vwCNInvoiceTower();

                    invoice = _services.ApprovalCNDeptHeadInvoiceTower(userCredential.UserID, post.trxInvoiceHeaderID, post.mstInvoiceCategoryId, post.vSource);

                    return Ok(invoice);
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

        [HttpPost, Route("ApprovalCNSPV")]
        public IHttpActionResult ApprovalCNSPVInvoiceTower(PostApproveCNInvoiceTower post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxPICAAR data = new trxPICAAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    var invoice = new vwCNInvoiceTower();

                    invoice = _services.ApprovalCNSPVInvoiceTower(userCredential.UserID, post.trxInvoiceHeaderID, post.mstInvoiceCategoryId, post.mstPICATypeIDSection, post.mstPICADetailIDSection);

                    return Ok(invoice);
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

        [HttpPost, Route("RejectCN")]
        public IHttpActionResult RejectCNInvoiceTower(PostApproveCNInvoiceTower post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxPICAAR data = new trxPICAAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    var invoice = new vwCNInvoiceTower();

                    invoice = _services.RejectCNInvoiceTower(userCredential.UserID, post.trxInvoiceHeaderID, post.mstInvoiceCategoryId, post.RejectRole);

                    return Ok(invoice);
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