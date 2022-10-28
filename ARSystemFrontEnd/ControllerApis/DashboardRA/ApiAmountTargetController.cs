using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/AmountTarget")]
    public class ApiAmountTargetController : ApiController
    {
        TrxRAAmountTargetService service = new TrxRAAmountTargetService();

        [HttpPost, Route("Submit")]
        public IHttpActionResult CreateRequest(PostTarget post)
        {
            try
            {

                //List<TrxRAAmountTargetDetail> listTarget = new List<TrxRAAmountTargetDetail>();
                var trxRAAmountTarget = new TrxRAAmountTarget();
                
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    
                    return Ok(new TrxRAAmountTarget(userCredential.ErrorType, userCredential.ErrorMessage));
                }

                post.post.CreatedBy = userCredential.UserID;
                post.post.CreatedDate = DateTime.Now;
                if (post.post.ID == 0)
                {
                    trxRAAmountTarget = service.CreateRequest(post.post, post.postDetail);
                }else
                {
                    trxRAAmountTarget = service.UpdateRequest(post.post, post.postDetail);
                }
                
                
               
                return Ok(trxRAAmountTarget);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetListHeader")]
        public IHttpActionResult GetListHeader(PostRTIAchievement post)
        {
            try
            {

                
                var trxRAAmountTargetList = new List<TrxRAAmountTarget>();
                int intTotalRecord = 0;
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {

                    return Ok(new TrxRAAmountTarget(userCredential.ErrorType, userCredential.ErrorMessage));
                }

                var param = new TrxRAAmountTarget();

                param.CustomerID = post.CustomerID;
                param.Year = post.Year;
                intTotalRecord = service.GetCountRequest(param);
                trxRAAmountTargetList = service.GetPagedRequest(param,post.start, post.length);

                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = trxRAAmountTargetList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListDetail")]
        public IHttpActionResult GetListDetail(PostIntID post)
        {
            try
            {
                var trxRAAmountTargetDetail = new List<TrxRAAmountTargetDetail>();
                int intTotalRecord = 0;
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {

                    return Ok(new TrxRAAmountTarget(userCredential.ErrorType, userCredential.ErrorMessage));
                }

                var param = new TrxRAAmountTarget();
                
                intTotalRecord = service.GetCountRequestDetail(post.ID);
                trxRAAmountTargetDetail = service.GetPagedRequestDetail(post.ID);

                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = trxRAAmountTargetDetail });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetAmountDetailReady")]
        public IHttpActionResult GetAmountDetailReady(PostRTIAchievement post)
        {
            MstRAAmountTargetService serviceReady = new MstRAAmountTargetService();
            try
            {
                var mstRAAmountTarget = new List<MstRAAmountTarget>();
                int intTotalRecord = 0;
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {

                    return Ok(new TrxRAAmountTarget(userCredential.ErrorType, userCredential.ErrorMessage));
                }


                var param = new MstRAAmountTarget();

                param.CustomerID = post.CustomerID;
                param.Year = post.Year;

                mstRAAmountTarget = serviceReady.GetList(param, post.start, post.length);
                intTotalRecord = serviceReady.GetCount(param);
                
                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = mstRAAmountTarget });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }



        [HttpGet, Route("GetListApprovalStatus")]
        public IHttpActionResult GetListApprovalStatus()
        {
            try
            {
                
                var approStatusList = new List<mstApprovalStatus>();
                approStatusList = MasterDataListService.GetApprovalStatus();

                return Ok(approStatusList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }

}