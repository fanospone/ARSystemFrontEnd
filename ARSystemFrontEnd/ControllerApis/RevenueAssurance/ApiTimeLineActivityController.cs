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

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/TimeLineActivity")]
    public class ApiTimeLineActivityController : ApiController
    {
        [HttpPost, Route("GetList")]
        public IHttpActionResult GetList(PostTimeLineActivity post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRALogActivity> vwRALogActivity = new List<ARSystemService.vwRALogActivity>();
                using (var client = new ARSystemService.ItrxRALogActivityClient())
                {
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    if(post.strSONumber != null)
                    {
                        if (post.strSONumber.Split(',').Length > 1)
                        {
                            string SONumber = "";
                            foreach (var item in post.strSONumber.Split(','))
                            {
                                SONumber += "'" + item + "',";
                            }

                            SONumber = SONumber + "'0'";
                            post.strSONumber = SONumber;
                        }
                        else
                        {
                            post.strSONumber = "'" + post.strSONumber + "'";
                        }
                    }

                    vwRALogActivity = client.GetLogList(UserManager.User.UserToken, post.strTransactionID, post.strActivity, post.strUserID,
                        post.strDate, post.strSONumber, post.strCompanyID, post.strCustomerID, strOrderBy).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = vwRALogActivity });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDetailList")]
        public IHttpActionResult GetDetailList(PostTimeLineActivity post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.trxRALogActivity> vwRALogActivity = new List<ARSystemService.trxRALogActivity>();
                using (var client = new ARSystemService.ItrxRALogActivityClient())
                {
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();
                    vwRALogActivity = client.GetLogListDetail(UserManager.User.UserToken, post.strTransactionID, post.strActivity, post.strUserID, post.strDate, strOrderBy).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = vwRALogActivity });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
