using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Web;
using System.Collections.Specialized;

namespace ARSystemFrontEnd.ControllerApis.RevenueAssurance
{
    [RoutePrefix("api/RelocBAPS")]
    public class ApiRelocBAPSController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetSonumbList(PostRelocBAPS post)
        {
            try
            {
                List<ARSystemService.vwRARelocBAPS> model = new List<ARSystemService.vwRARelocBAPS>();
                int intTotalRecord = 0;
                using (var client = new ARSystemService.NewBapsServiceClient())
                {
                    //intTotalRecord = client.relo(UserManager.User.UserToken, post.strCompanyId, post.strCustomerID, post.strProductId, post.strSoNumber, post.strSiteID, "-", post.mstRAActivityID);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    model = client.GetRelocBAPSList(UserManager.User.UserToken, MapParameters(post), strOrderBy, post.start, post.length).ToList();

                    intTotalRecord = model.Count;

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = model });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submit")]
        public IHttpActionResult SubmitData(ARSystemService.trxRARelocBAPS post)
        {
            try
            {
                using (var client = new ARSystemService.NewBapsServiceClient())
                {
                    post = client.ProcessRelocBAPS(UserManager.User.UserToken, post);

                    return Ok(post);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("datevalidate")]
        public IHttpActionResult datevalidate(ARSystemService.trxRARelocBAPS post)
        {
            try
            {
                using (var client = new ARSystemService.NewBapsServiceClient())
                {
                    var date = client.GetValidateRelocBAPS(UserManager.User.UserToken, post.SONumber);

                    if(post.EndBapsDate == DateTime.Parse("1900-01-01"))
                    {
                        if(post.StartBapsDate < date)
                        {
                            return Ok("Last Invoice " + date.ToShortDateString());
                        }
                    }
                    else
                    {
                        if(post.EndBapsDate <= post.StartBapsDate)
                        {
                            return Ok("End Must Bigger Than Start");
                        }
                    }

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string MapParameters(PostRelocBAPS post)
        {
            string WhereClause = "";

            if (!string.IsNullOrEmpty(post.Company))
            {
                WhereClause += " AND Company = '" + post.Company + "'";
            }

            if (!string.IsNullOrEmpty(post.Customer))
            {
                WhereClause += " AND Customer = '" + post.Customer + "'";
            }

            if (!string.IsNullOrEmpty(post.Stip))
            {
                WhereClause += " AND Stip = '" + post.Stip + "'";
            }

            if (!string.IsNullOrEmpty(post.Tenant))
            {
                WhereClause += " AND TenantType = '" + post.Tenant + "'";
            }

            if (!string.IsNullOrEmpty(post.SONumber))
            {
                WhereClause += " AND (SONumber = '" + post.SONumber + "' or SONumberLive = '" + post.SONumber + "')";
            }

            return WhereClause;
        }

    }
}