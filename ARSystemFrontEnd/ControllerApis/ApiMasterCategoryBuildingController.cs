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
    [RoutePrefix("api/CategoryBuilding")]
    public class ApiMasterCategoryBuildingController : ApiController
    {


        [HttpPost, Route("Save")]
        public IHttpActionResult Save(PostCategoryBuilding post)
        {
            try
            {
                ARSystemService.mstCategoryBuilding repo = new ARSystemService.mstCategoryBuilding();
                using (var client = new ARSystemService.ImstCategoryBuildingServiceClient())
                {
                    if (post.model.ID == 0)
                    {
                        post.model = client.SaveCategoryBuilding(UserManager.User.UserToken, post.model);
                    }
                    else
                    {
                        post.model = client.UpdateCategoryBuilding(UserManager.User.UserToken, post.model);
                    }
                }
                return Ok(post.model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Delete")]
        public IHttpActionResult Delete(PostCategoryBuilding post)
        {
            try
            {
                ARSystemService.mstCategoryBuilding repo = new ARSystemService.mstCategoryBuilding();
                using (var client = new ARSystemService.ImstCategoryBuildingServiceClient())
                {
                    post.model = client.DeleteCategoryBuilding(UserManager.User.UserToken, post.model.ID);
                }
                return Ok(post.model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetList")]
        public IHttpActionResult GetList(PostCategoryBuilding post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.mstCategoryBuilding> repoList = new List<ARSystemService.mstCategoryBuilding>();
                using (var client = new ARSystemService.ImstCategoryBuildingServiceClient())
                {
                    post.model.CategoryBuilding = post.strCategoryBuilding;
                    intTotalRecord = client.GetCountCategoryBuilding(UserManager.User.UserToken, post.model);
                    repoList = client.GetListCategoryBuilding(UserManager.User.UserToken, post.model, post.start, post.length).ToList();
                }
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = repoList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDetail")]
        public IHttpActionResult GetDetail(int ID)
        {
            try
            {
                ARSystemService.mstCategoryBuilding data = new ARSystemService.mstCategoryBuilding();
                using (var client = new ARSystemService.ImstCategoryBuildingServiceClient())
                {
                    data.ID = ID;
                    data = client.GetDetailCategoryBuilding(UserManager.User.UserToken, data);
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