using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ApprovalCNInvoiceBuilding")]
    public class ApiApprovalCNInvoiceBuildingController : ApiController
    {
        [HttpPost, Route("grid")]
        // GET: ApiChecklistInvoiceBuilding
        public IHttpActionResult GetChecklistInvoiceBuildingToGrid(PostTrxCNInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwCNInvoiceBuilding> data = new List<ARSystemService.vwCNInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxApprovalCNInvoiceBuildingServiceClient())
                {
                    intTotalRecord = client.GetApprovalCNInvoiceBuildingCount(UserManager.User.UserToken, post.invoiceTypeId, post.companyName, post.invCompanyId, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetApprovalCNInvoiceBuildingToList(UserManager.User.UserToken, post.invoiceTypeId, post.companyName, post.invCompanyId, post.invNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalCNSPV")]
        public IHttpActionResult ApprovalCNSPV(PostApproveCNInvoiceBuilding post)
        {
            try
            {
                ARSystemService.vwCNInvoiceBuilding invoice;
                using (var client = new ARSystemService.ItrxApprovalCNInvoiceBuildingServiceClient())
                {
                    invoice = client.ApprovalCNSPVInvoiceBuilding(UserManager.User.UserToken, post.trxInvoiceHeaderID);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalCNDeptHead")]
        public IHttpActionResult ApprovalCNDeptHead(PostApproveCNInvoiceBuilding post)
        {
            try
            {
                ARSystemService.vwCNInvoiceBuilding invoice;
                using (var client = new ARSystemService.ItrxApprovalCNInvoiceBuildingServiceClient())
                {
                    invoice = client.ApprovalCNDeptHeadInvoiceBuilding(UserManager.User.UserToken, post.trxInvoiceHeaderID);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ValidateUser")]
        public IHttpActionResult ValidateUser(PostValidateUserCNInvoice post)
        {
            try
            {
                ARSystemService.vmStringResult user;
                ARSystemService.vmLogin login = new ARSystemService.vmLogin();
                login.UserID = post.UserID.Trim();
                login.Password = post.Password;
                login.Application = Helper.Helper.GetApplicationName();
                login.IP = Helper.Helper.GetIPAddress();

                using (var client = new ARSystemService.UserServiceClient())
                {
                    user = client.ValidateUser(UserManager.User.UserToken, login, post.Role);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("RejectCN")]
        public IHttpActionResult RejectCNInvoiceTower(PostApproveCNInvoiceBuilding post)
        {
            try
            {
                ARSystemService.vwCNInvoiceBuilding invoice;
                using (var client = new ARSystemService.ItrxApprovalCNInvoiceBuildingServiceClient())
                {
                    invoice = client.RejectCNInvoiceBuilding(UserManager.User.UserToken, post.trxInvoiceHeaderID,post.RejectRole);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}