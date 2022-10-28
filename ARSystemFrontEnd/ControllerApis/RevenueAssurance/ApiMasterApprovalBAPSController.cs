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
    [RoutePrefix("api/ApprovalBAPS")]
    public class ApiMasterApprovalBAPSController : ApiController
    {
        // GET: ApiMasterApprovalBAPS

        [HttpPost, Route("getList")]
        public IHttpActionResult GetList(PostMstApprovalBAPS post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.MstApprovalBAPS> list = new List<ARSystemService.MstApprovalBAPS>();
                using (var client = new ARSystemService.ImstApprovalBAPSServiceClient())
                {
                    post.ApprBaps.ApprovalName = post.strApprovalName;
                    post.ApprBaps.Position = post.strPosition;
                    post.ApprBaps.RegionID = post.strRegionID;
                    post.ApprBaps.OperatorID = post.strCustomerID;
                    intTotalRecord = client.GetCountApprovalBAPS(UserManager.User.UserToken, post.ApprBaps);
                    list = client.GetListApprovalBAPS(UserManager.User.UserToken, post.ApprBaps, post.start, post.length).ToList();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("submit")]
        public IHttpActionResult Submit(PostMstApprovalBAPS post)
        {
            try
            {
                ARSystemService.MstApprovalBAPS result = new ARSystemService.MstApprovalBAPS();
                using (var client = new ARSystemService.ImstApprovalBAPSServiceClient())
                {
                    result = client.SaveApprovalBAPS(UserManager.User.UserToken,post.ApprBaps);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}