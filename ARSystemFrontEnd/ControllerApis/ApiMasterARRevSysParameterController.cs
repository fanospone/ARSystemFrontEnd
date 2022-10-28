using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystem.Domain.Models;
using ARSystem.Service;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/MasterARRevSysParameter")]
    public class ApiMasterARRevSysParameterController : ApiController
    {

        private readonly mstARRevSysParameterServices Service;
        
        public ApiMasterARRevSysParameterController()
        {
            Service = new mstARRevSysParameterServices();
        }

        [HttpPost, Route("save")]
        public IHttpActionResult Save(mstARRevSysParameter post)
        {
            try
            {
                
                post = Service.mstARRevSysParameterCreate(post.CreatedBy, post);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("update")]
        public IHttpActionResult Update(PostRequest p)
        {
            try
            {
                mstARRevSysParameter t = new mstARRevSysParameter();
                t.ID = p.ID;
                t.ParamName = p.ParamName;
                t.ParamValue = p.ParamValue;
                t.CreatedBy = p.CreatedBy;
                t.CreatedDate = p.CreatedDate;
                
                t = Service.mstARRevSysParameterUpdate(p.UpdatedBy, t);                
                return Ok(p);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
 
        [HttpPost, Route("GetList")]
        public IHttpActionResult GetList(PostRequest post)
        {
            try
            {
                int intTotalRecord = 0;
                List<mstARRevSysParameter> DataList = new List<mstARRevSysParameter>();
                intTotalRecord = Service.CountmstARRevSysParameterList(UserManager.User.UserToken);
                DataList = Service.GetmstARRevSysParameterList(UserManager.User.UserToken, post.strOrderBy, post.start, post.length).ToList();
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = DataList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("startjob")]
        public IHttpActionResult StartJob(PostRequest post)
        {
            try
            {
                mstARRevSysParameter t = new mstARRevSysParameter();
                Service.mstARRevSysParameterStartJob(post.Flag, t);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }

    public class PostRequest : DatatableAjaxModel
    {
        public int ID { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string strOrderBy { get; set; }
        public string Flag { get; set; }


    }
}