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
using System.Text;
using System.Configuration;


namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ReconcileReject")]
    public class ApiReconcileRejectController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetDataList(PostReconcileRejectNonTSEL post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAReconcileRejectNonTSEL> modelList = new List<ARSystemService.vwRAReconcileRejectNonTSEL>();
                using (var client = new ARSystemService.ItrxReconcileRejectNonTSELServiceClient())
                {
                    post.vwReconcile.ReconcileID = post.strReconcileID;
                    post.vwReconcile.mstBapsTypeID = post.strBAPSTypeID;
                    post.vwReconcile.ProductID = post.strProductID;
                    post.vwReconcile.Year = post.strYear;
                    post.vwReconcile.STIPID = post.strSTIPID;
                    post.vwReconcile.SoNumber = post.strSONumber;
                    post.vwReconcile.SiteID = post.strSiteID;
                    post.vwReconcile.InvoiceTypeID = post.strInvoiceTypeID;
                    post.vwReconcile.PowerTypeID = post.strPowerTypeID;
                    post.vwReconcile.CompanyInvoiceId = post.strCompanyID;
                    post.vwReconcile.CustomerInvID = post.strCustomerID;

                    intTotalRecord = client.GetCountReconcileRejectNonTSEL(UserManager.User.UserToken, post.vwReconcile);
                    modelList = client.GetListReconcileRejectNonTSEL(UserManager.User.UserToken, post.vwReconcile, post.start, post.length).ToList();
                }
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = modelList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("update")]
        public IHttpActionResult Update(PostReconcileRejectNonTSEL post)
        {
            try
            {
                ARSystemService.trxReconcile model = new ARSystemService.trxReconcile();
                using (var client = new ARSystemService.ItrxReconcileRejectNonTSELServiceClient())
                {
                    model = client.UpdateTrxReconcile(UserManager.User.UserToken, post.trxReconcile, post.strCustomerID, post.strCompanyID, post.strPOType, post.strPODtlID);
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("prorate")]
        public IHttpActionResult ProrateAmount(PostReconcileRejectNonTSEL post)
        {
            try
            {
                decimal amount;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    amount = 0;
                    string service = post.strServicePrice;
                    string baselease = post.strBaseLease;


                    if (post.strPOType.ToUpper() == "BASELEASE")
                    {
                        if (post.strBaseLeaseCurr == "USD")
                        {
                            baselease = post.strBaseLease;
                            if (post.strServiceCurr != "USD")
                            {
                                service = "0";
                            }
                            else
                            {
                                service = post.strServicePrice;
                            }
                        }
                        else
                        {
                            baselease = post.strBaseLease;
                            if (post.strServiceCurr != "IDR")
                            {
                                service = "0";
                            }
                            else
                            {
                                service = post.strServicePrice;
                            }
                        }
                    }
                    else if (post.strPOType.ToUpper() == "SERVICE")
                    {
                        if (post.strServiceCurr == "USD")
                        {
                            service = post.strServicePrice;
                            if (post.strBaseLeaseCurr != "USD")
                            {
                                baselease = "0";
                            }
                            else
                            {
                                
                                baselease = post.strBaseLease;
                            }
                        }
                        else
                        {
                            service = post.strServicePrice;
                            if (post.strBaseLeaseCurr != "IDR")
                            {
                                baselease = "0";
                            }
                            else
                            {
                                
                                baselease = post.strBaseLease;
                            }
                        }
                    }

                    if (post.strBaseLeaseCurr == post.strServiceCurr)
                    {
                        baselease = post.strBaseLease;
                        service = post.strServicePrice;
                    }

                    post.strBaseLease = baselease.Replace(".00", "");
                    post.strBaseLease = baselease.Replace(",", "");
                    post.strServicePrice = service.Replace(".00", "");
                    post.strServicePrice = service.Replace(",", "");
                    amount = client.GetProRateAmount(UserManager.User.UserToken, post.strCustomerID, post.strStartDate, post.strEndDate, decimal.Parse(post.strBaseLease), decimal.Parse(post.strServicePrice), 0, 0);
                }
                return Ok(amount);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }

}