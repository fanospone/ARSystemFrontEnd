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
using ARSystem.Service;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ReOpenPaymentDate")]
    public class ApiReOpenPaymentDateController : ApiController
    {
        private readonly trxARReOPenPaymentDateService ARReOpenPaymentService;

        public ApiReOpenPaymentDateController()
        {
            this.ARReOpenPaymentService = new trxARReOPenPaymentDateService();
        }

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

                List<TrxARReOpenPaymentDate> listData = new List<TrxARReOpenPaymentDate>();
                TrxARReOpenPaymentDate request = new TrxARReOpenPaymentDate();
                request.RequestNumber = post.RequestNumber;
                request.PaymentDateRevision = post.PaymentDate;
                request.PaymentDateRevision2 = post.PaymentDate2;
                request.CreateDate = post.CreatedDate;
                request.CreateDate2 = post.CreatedDate2;
                intTotalRecord = UserManager.User.CompanyCode==Constants.CompanyCode.PKP 
                    ? ARReOpenPaymentService.GetCountPaymentDateRequestByCompany(request, Constants.CompanyCode.PKP) 
                    : ARReOpenPaymentService.GetCountPaymentDateRequest(request);
                listData = UserManager.User.CompanyCode==Constants.CompanyCode.PKP
                    ? ARReOpenPaymentService.GetListPaymentDateRequestByCompany(request, post.start, post.length, Constants.CompanyCode.PKP)
                    : ARReOpenPaymentService.GetListPaymentDateRequest(request, post.start, post.length).ToList();
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
                TrxARReOpenPaymentDateDetail data = new TrxARReOpenPaymentDateDetail();
                List<TrxARReOpenPaymentDateDetail> listData = new List<TrxARReOpenPaymentDateDetail>();
                data.TrxARReOpenPaymentDateID = post.HeaderID;
                //data.PaymentDate = post.PaymentDate;

                intTotalRecord = UserManager.User.CompanyCode == Constants.CompanyCode.PKP
                    ? ARReOpenPaymentService.GetCountPaymentDateDtlByCompany(data,Constants.CompanyCode.PKP)
                    : ARReOpenPaymentService.GetCountPaymentDateDtl(data);
                listData = UserManager.User.CompanyCode == Constants.CompanyCode.PKP
                    ? ARReOpenPaymentService.GetListPaymentDateDtlByCompany(data, post.start, post.length, Constants.CompanyCode.PKP)
                    : ARReOpenPaymentService.GetListPaymentDateDtl(data, post.start, post.length).ToList();

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