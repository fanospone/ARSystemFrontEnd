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
    [RoutePrefix("api/Tenant")]
    public class ApiTenantController : ApiController
    {
        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateTenant(int id, ARSystemService.mstTenant tenant)
        {
            try
            {
                using (var client = new ARSystemService.ImstTenantServiceClient())
                {
                    tenant = client.UpdateTenant(UserManager.User.UserToken, id, tenant);
                }

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateTenant(ARSystemService.mstTenant tenant)
        {
            try
            {
                using (var client = new ARSystemService.ImstTenantServiceClient())
                {
                    tenant = client.CreateTenant(UserManager.User.UserToken, tenant);
                }

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpDelete, Route("{id}")]
        public IHttpActionResult DeleteTenant(int id)
        {
            try
            {
                ARSystemService.mstTenant tenant;
                using (var client = new ARSystemService.ImstTenantServiceClient())
                {
                    tenant = client.DeleteTenant(UserManager.User.UserToken, id);
                }

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}