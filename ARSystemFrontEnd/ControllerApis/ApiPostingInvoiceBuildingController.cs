﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/PostingInvoiceBuilding")]
    public class ApiPostingInvoiceBuildingController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetInvoiceBuildingDetailToList(string companyName, string invoiceTypeId, string invNo, string InvoiceCategory)
        {
            try
            {
                List<ARSystemService.vwInvoiceBuildingDetail> data = new List<ARSystemService.vwInvoiceBuildingDetail>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    data = client.GetTrxInvoiceBuildingDetailToList(UserManager.User.UserToken, companyName, invoiceTypeId, -1, invNo, InvoiceCategory, "trxInvoiceBuildingDetailID", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceBuildingDetailDataToGrid(PostTrxInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwInvoiceBuildingDetail> data = new List<ARSystemService.vwInvoiceBuildingDetail>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetInvoiceBuildingDetailCount(UserManager.User.UserToken, post.companyName, post.invoiceTypeId, post.invoiceStatusId, post.invNo, post.InvoiceCategory);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetTrxInvoiceBuildingDetailToList(UserManager.User.UserToken, post.companyName, post.invoiceTypeId, post.invoiceStatusId, post.invNo, post.InvoiceCategory, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult PostingTrxInvoiceBuilding(PostTrxInvoiceBuildingPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.PostingInvoiceBuildingDetail(UserManager.User.UserToken, post.trxInvoiceHeaderID, post.invoiceDate, post.subject, post.signature);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Cancel")]
        public IHttpActionResult CancelTrxInvoiceBuilding(PostTrxInvoiceBuildingPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.CancelInvoiceBuildingDetail(UserManager.User.UserToken, post.trxInvoiceHeaderID, post.invoiceDate, post.subject, post.signature, post.remark);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApproveCancel")]
        public IHttpActionResult ApproveCancelTrxInvoiceBuilding(PostTrxInvoiceBuildingPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.ApproveCancelInvoiceBuildingDetail(UserManager.User.UserToken, post.trxInvoiceHeaderID);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectCancel")]
        public IHttpActionResult RejectCancelTrxInvoiceBuilding(PostTrxInvoiceBuildingPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.RejectCancelInvoiceBuildingDetail(UserManager.User.UserToken, post.trxInvoiceHeaderID);
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