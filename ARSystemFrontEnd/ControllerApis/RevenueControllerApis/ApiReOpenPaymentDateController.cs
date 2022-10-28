using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Web;
using System.Collections.Specialized;
using ARSystemFrontEnd.Helper;
using System.IO;
using System.Configuration;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ReOpenPaymentDate")]
    public class ApiReOpenPaymentDateController : ApiController
    {
        [HttpPost, Route("Request")]
        public IHttpActionResult Reuquet(PostPaymentData post)

        {
            try
            {
                int intTotalRecord = 0;
                ARSystemService.vwARReOpenPaymentDate vwData = new ARSystemService.vwARReOpenPaymentDate();
                vwData.PaymentDate = post.PaymentDate;
                vwData.PaymentDate2 = post.PaymentDate2;
                vwData.InvCompanyId = post.InvCompanyId;
                vwData.InvOperatorID = post.InvOperatorID;
                vwData.InvNo = post.InvNo;
                List<ARSystemService.vwARReOpenPaymentDate> listData = new List<ARSystemService.vwARReOpenPaymentDate>();
                using (var client = new ARSystemService.ItrxARReOPenPaymentDateClient())
                {
                    intTotalRecord = client.GetCountPaymentDate(vwData);
                    listData = client.GetListPaymentDate(vwData, post.start, post.length).ToList();
                }
                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("SummaryData")]
        public IHttpActionResult SummaryData(PostPaymentData post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.TrxARReOpenPaymentDate> listData = new List<ARSystemService.TrxARReOpenPaymentDate>();
                using (var client = new ARSystemService.ItrxARReOPenPaymentDateClient())
                {
                    ARSystemService.TrxARReOpenPaymentDate request = new ARSystemService.TrxARReOpenPaymentDate();
                    request.RequestNumber = post.RequestNumber;
                    request.PaymentDateRevision = post.PaymentDate;
                    request.PaymentDateRevision2 = post.PaymentDate2;
                    request.CreateDate = post.CreatedDate;
                    request.CreateDate2 = post.CreatedDate2;
                    intTotalRecord = client.GetCountPaymentDateRequest(request);
                    listData = client.GetListPaymentDateRequest(request, post.start, post.length).ToList();
                }
                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("SummaryDataDetail")]
        public IHttpActionResult SummaryDataDetail(PostTrxPaymentDataDetail post)

        {
            try
            {
                int intTotalRecord = 0;
                ARSystemService.TrxARReOpenPaymentDateDetail data = new ARSystemService.TrxARReOpenPaymentDateDetail();
                List<ARSystemService.TrxARReOpenPaymentDateDetail> listData = new List<ARSystemService.TrxARReOpenPaymentDateDetail>();
                using (var client = new ARSystemService.ItrxARReOPenPaymentDateClient())
                {
                    data.TrxARReOpenPaymentDateID = post.HeaderID;
                    //data.PaymentDate = post.PaymentDate;

                    intTotalRecord = client.GetCountPaymentDateDtl(data);
                    listData = client.GetListPaymentDateDtl(data, post.start, post.length).ToList();
                }
                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CraeteRequest")]
        public IHttpActionResult CraeteRequest(PostTrxPaymentData post)

        {
            try
            {

                ARSystemService.TrxARReOpenPaymentDate data = new ARSystemService.TrxARReOpenPaymentDate();
                using (var client = new ARSystemService.ItrxARReOPenPaymentDateClient())
                {
                    data = client.CreatePaymentDate(UserManager.User.UserToken, post.request, post.requestDetailList.ToArray());
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalStatus")]
        public IHttpActionResult ApprovalStatus()

        {
            try
            {

                List<ARSystemService.mstARApprovalStatus> data = new List<ARSystemService.mstARApprovalStatus>();
                using (var client = new ARSystemService.ItrxARReOPenPaymentDateClient())
                {
                    data = client.GetARApprovalStatus().ToList();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdateRequest")]
        public IHttpActionResult UpdateRequest(PostTrxPaymentData post)

        {
            try
            {

                ARSystemService.TrxARReOpenPaymentDate data = new ARSystemService.TrxARReOpenPaymentDate();
                using (var client = new ARSystemService.ItrxARReOPenPaymentDateClient())
                {
                    data = client.UpdatePaymentDate(UserManager.User.UserToken, post.request);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}