using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/PICARASystem")]
    public class ApiPICARASystemController : ApiController
    {
        PICARASystemService service = new PICARASystemService();

        [HttpGet, Route("GetDdlCustomer")]
        public IHttpActionResult GetDdlCustomer()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlCustomer();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlCompany")]
        public IHttpActionResult GetDdlCompany()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlCompany();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlProduct")]
        public IHttpActionResult GetDdlProduct()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlProduct();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlBapsType")]
        public IHttpActionResult GetDdlBapsType()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlBapsType();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlStipCategory")]
        public IHttpActionResult GetDdlStipCategory()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlStipCategory();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlActivityStatus")]
        public IHttpActionResult GetDdlActivityStatus()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlActivityStatus();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlCategoryPICA")]
        public IHttpActionResult GetDdlCategoryPICA()
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlCategoryPICA();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetDdlPICA")]
        public IHttpActionResult GetDdlPICA(string PICA)
        {
            try
            {
                var data = new List<GetDropDownList>();
                data = service.GetDdlPICA(PICA);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListHeader")]
        public IHttpActionResult GetListHeader(PostPICARASystem post)
        {
            var intTotalRecord = 0;
            List<vwRADashboardPICA> result = new List<vwRADashboardPICA>();
            try
            {
                intTotalRecord = service.GetCountListHeader(post.DashboardPICARASystem);
                result = service.GetListHeader(post.DashboardPICARASystem, post.start, post.length).ToList();
                //return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        }

        [HttpPost, Route("GetListDetail")]
        public IHttpActionResult GetListDetail(PostPICARASystem post)
        {
            var intTotalRecord = 0;
            List<vwRADashboardPICA> result = new List<vwRADashboardPICA>();
            try
            {
                intTotalRecord = service.GetCountListDetail(post.DashboardPICARASystem);
                result = service.GetListDetail(post.DashboardPICARASystem, post.start, post.length).ToList();
                
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        }

        [HttpPost, Route("GetHistoryPICA")]
        public IHttpActionResult GetHistoryPICA(PostPICARASystem post)
        {
            var intTotalRecord = 0;
            List<vwRADashboardPICA> result = new List<vwRADashboardPICA>();
            try
            {
                intTotalRecord = service.GetCountHistoryPICA(post.DashboardPICARASystem);
                result = service.GetHistoryPICA(post.DashboardPICARASystem, post.start, post.length).ToList();

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        }

        [HttpPost, Route("InsertPICA")]
        public IHttpActionResult InsertPICA(PostPICARASystem post)
        {
            vwRADashboardPICA result = new vwRADashboardPICA();
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            if (!string.IsNullOrWhiteSpace(userCredential.ErrorMessage))
                return Ok(new { data = "" });

            try
            {
                result = service.InsertPICA(userCredential.UserID, post.DashboardPICARASystem);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

            return Ok(new { data = result });
        }
    }
}