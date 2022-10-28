using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/RESlip")]
    public class ApiMasterRESlipController : ApiController
    {

        [HttpPost, Route("Save")]
        public IHttpActionResult Save(PostRESlip post)
        {
            try
            {
                ARSystemService.mstRESlip repo = new ARSystemService.mstRESlip();
                using (var client2 = new ARSystemService.ImstRESlipServiceClient())
                {
                    repo = client2.CreateRESlip(UserManager.User.UserToken, post.model);
                }
                return Ok(post.model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Delete")]
        public IHttpActionResult Delete(PostRESlip post)
        {
            try
            {
                ARSystemService.mstRESlip repo = new ARSystemService.mstRESlip();
                using (var client2 = new ARSystemService.ImstRESlipServiceClient())
                {
                    post.model = client2.DeleteRESlip(UserManager.User.UserToken, post.model.ID);
                }
                return Ok(post.model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetList")]
        public IHttpActionResult GetList(PostRESlip post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.mstRESlip> repoList = new List<ARSystemService.mstRESlip>();
                using (var client2 = new ARSystemService.ImstRESlipServiceClient())
                {

                    intTotalRecord = client2.GetRESlipCount(UserManager.User.UserToken, post.SONumber);
                    repoList = client2.GetMstRESlipToList(UserManager.User.UserToken, post.SONumber, post.start, post.length).ToList();
                }
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = repoList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


    }
}