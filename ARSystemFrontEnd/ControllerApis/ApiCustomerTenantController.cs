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
    [RoutePrefix("api/CustomerTenant")]
    public class ApiCustomerTenantController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetCustomerTenantToList(string customerName = "", int customerTypeId = 0, int isActive = -1, string companyType = "", int customerId = 0)
        {
            try
            {
                List<ARSystemService.vmCustomerTenant> list = new List<ARSystemService.vmCustomerTenant>();
                using (var client = new ARSystemService.ImstCustomerTenantServiceClient())
                {
                    list = client.GetMstCustomerTenantToList(UserManager.User.UserToken, customerName, customerTypeId, isActive, companyType, customerId, "CustomerID", 0, 0).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateCustomerTenant(ARSystemService.vmCustomerTenant customerTenant)
        {
            try
            {
                using (var client = new ARSystemService.ImstCustomerTenantServiceClient())
                {
                    customerTenant = client.CreateCustomerTenant(UserManager.User.UserToken, customerTenant);
                }

                return Ok(customerTenant);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateCustomerTenant(int id, ARSystemService.vmCustomerTenant customerTenant)
        {
            try
            {
                using (var client = new ARSystemService.ImstCustomerTenantServiceClient())
                {
                    customerTenant = client.UpdateCustomerTenant(UserManager.User.UserToken, id, customerTenant);
                }

                return Ok(customerTenant);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetCustomerTenantToGrid(PostCustomerTenantView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmCustomerTenant> list = new List<ARSystemService.vmCustomerTenant>();
                using (var client = new ARSystemService.ImstCustomerTenantServiceClient())
                {
                    intTotalRecord = client.GetCustomerTenantCount(UserManager.User.UserToken, post.customerName, post.customerTypeId, post.intIsActive, post.companyType, post.customerId);
                    
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetMstCustomerTenantToList(UserManager.User.UserToken, post.customerName, post.customerTypeId, post.intIsActive, post.companyType, post.customerId, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Delete/{id}")]
        public IHttpActionResult DeleteTenant(int id)
        {
            try
            {
                ARSystemService.vmCustomerTenant customerTenant;
                using (var client = new ARSystemService.ImstCustomerTenantServiceClient())
                {
                    customerTenant = client.DeleteTenantByCustomerId(UserManager.User.UserToken, id);
                }

                return Ok(customerTenant);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}