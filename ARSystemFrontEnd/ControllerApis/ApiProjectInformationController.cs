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
    [RoutePrefix("api/ProjectInformation")]
    public class ApiProjectInformationController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetProjectInformationToGrid(PostProjectInformationView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstProjectInformation> projectInformations = new List<ARSystemService.mstProjectInformation>();
                using (var client = new ARSystemService.ImstProjectInformationServiceClient())
                {
                    intTotalRecord = client.GetProjectInformationCount(UserManager.User.UserToken, post.SoNumber, post.SiteID, post.SiteName, post.RegionalName, null);

                    string strOrderBy = "AsatDate DESC";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    projectInformations = client.GetMstProjectInformationToList(UserManager.User.UserToken, post.SoNumber, post.SiteID, post.SiteName, post.RegionalName, null, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = projectInformations });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ListByID")]
        public IHttpActionResult GetProjectInformationListByID(PostProjectInformationView post)
        {
            try
            {
                List<ARSystemService.mstProjectInformation> projects = new List<ARSystemService.mstProjectInformation>();
                using (var client = new ARSystemService.ImstProjectInformationServiceClient())
                {
                    projects = client.GetMstProjectInformationToList(UserManager.User.UserToken, "", "", "", "", post.ListID.ToArray(), "", 0, 0).ToList();
                }
                return Ok(projects);
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}