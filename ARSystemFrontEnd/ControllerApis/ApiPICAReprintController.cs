using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/MstPICAReprint")]
    public class ApiPICAReprintController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetPICAReprintToList(string picaReprint = "", string productCode = "", int isActive = -1)
        {
            try
            {
                List<ARSystemService.mstPICAReprint> picaReprints = new List<ARSystemService.mstPICAReprint>();
                using (var client = new ARSystemService.ImstPICAReprintServiceClient())
                {
                    picaReprints = client.GetMstPICAReprintToList(UserManager.User.UserToken, picaReprint, isActive, "PICAReprintId", 0, 0).ToList();
                }

                return Ok(picaReprints);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreatePICAReprint(ARSystemService.mstPICAReprint PICAReprint)
        {
            try
            {
                using (var client = new ARSystemService.ImstPICAReprintServiceClient())
                {
                    PICAReprint = client.CreatePICAReprint(UserManager.User.UserToken, PICAReprint);
                }

                return Ok(PICAReprint);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdatePICAReprint(int id, ARSystemService.mstPICAReprint PICAReprint)
        {
            try
            {
                using (var client = new ARSystemService.ImstPICAReprintServiceClient())
                {
                    PICAReprint = client.UpdatePICAReprint(UserManager.User.UserToken, id, PICAReprint);
                }

                return Ok(PICAReprint);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetPICAReprintToGrid(PostPICAReprintView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstPICAReprint> picaReprints = new List<ARSystemService.mstPICAReprint>();
                using (var client = new ARSystemService.ImstPICAReprintServiceClient())
                {
                    intTotalRecord = client.GetPICAReprintCount(UserManager.User.UserToken, post.picaReprint, post.isActive);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    picaReprints = client.GetMstPICAReprintToList(UserManager.User.UserToken, post.picaReprint, post.isActive, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = picaReprints });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}