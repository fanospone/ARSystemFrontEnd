using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/email")]
    public class ApiEmailController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetEmailToGrid(PostEmailView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmEmailVariable> emailvariables = new List<ARSystemService.vmEmailVariable>();
                using (var client = new ARSystemService.EmailServiceClient())
                {
                    intTotalRecord = client.GetEmailCount(UserManager.User.UserToken, post.strEmailName, post.intIsActive);

                    string strOrderBy = "";
                    if (post.order != null)
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    emailvariables = client.GetEmailToList(UserManager.User.UserToken, post.strEmailName, post.intIsActive, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = emailvariables });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateEmail(int id, ARSystemService.mstEmail email)
        {
            try
            {
                using (var client = new ARSystemService.EmailServiceClient())
                {
                    email = client.UpdateEmail(UserManager.User.UserToken, id, email);
                }

                return Ok(email);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
