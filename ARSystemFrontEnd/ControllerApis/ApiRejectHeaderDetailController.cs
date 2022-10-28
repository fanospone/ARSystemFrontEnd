using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/RejectHeaderDetail")]
    public class ApiRejectHeaderDetailController : ApiController
    {


        [HttpPost, Route("Hdr")]
        public IHttpActionResult CreateRejectHdr(ARSystemService.mstPICAType mstPICAType)
        {
            try
            {
                using (var client = new ARSystemService.ImstRejectServiceClient())
                {
                    mstPICAType = client.CreateRejectHdr(UserManager.User.UserToken, mstPICAType);
                }

                return Ok(mstPICAType);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("Dtl")]
        public IHttpActionResult CreateRejectDtl(ARSystemService.mstPICADetail mstPICADetail)
        {
            try
            {
                using (var client = new ARSystemService.ImstRejectServiceClient())
                {
                    mstPICADetail = client.CreateRejectDtl(UserManager.User.UserToken, mstPICADetail);
                }

                return Ok(mstPICADetail);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPut, Route("Hdr/{id}")]
        public IHttpActionResult UpdateRejectHdr(int id, ARSystemService.mstPICAType mstPICAType)
        {
            try
            {
                using (var client = new ARSystemService.ImstRejectServiceClient())
                {
                    mstPICAType = client.UpdateRejectHdr(UserManager.User.UserToken, id, mstPICAType);
                }

                return Ok(mstPICAType);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPut, Route("Dtl/{id}")]
        public IHttpActionResult UpdateRejectDtl(int id, ARSystemService.mstPICADetail mstPICADetail)
        {
            try
            {
                using (var client = new ARSystemService.ImstRejectServiceClient())
                {
                    mstPICADetail = client.UpdateRejectDtl(UserManager.User.UserToken, id, mstPICADetail);
                }

                return Ok(mstPICADetail);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("grid")]
        public IHttpActionResult GetRejectHdrDtlToGrid(PostMstRejectHdrDtlView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmMstRejectHdrDtl> RejectHdrDtl = new List<ARSystemService.vmMstRejectHdrDtl>();
                using (var client = new ARSystemService.ImstRejectServiceClient())
                {
                    intTotalRecord = client.GetRejectHdrCount(UserManager.User.UserToken, post.strHdr, post.intIsActive,post.mstUserGroupId);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    RejectHdrDtl = client.GetMstHdrDtlToList(UserManager.User.UserToken, post.strHdr, post.intIsActive, post.mstUserGroupId, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = RejectHdrDtl });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}