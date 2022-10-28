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
    [RoutePrefix("api/ReconcileData")]
    public class ApiReconcileDataController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetReconcileDataToGrid(PostReconcileDataView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwOPMRASow> reconciledata = new List<ARSystemService.vwOPMRASow>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth, post.isRaw);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    reconciledata = client.GetReconcileDataToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth, strOrderBy, post.start, post.length, post.isRaw).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = reconciledata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostReconcileDataView post)
        {
            try
            {

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = client.GetReconcileDataListId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth, post.isRaw).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("doProcess")]
        public IHttpActionResult doProcess(PostReconcileDataView post)
        {

            try
            {
               int ListId = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    foreach (int sn in post.soNumb)
                    {
                        ListId = client.doProcess(UserManager.User.UserToken, sn);
                    }
                }

                 return Ok(ListId);
             }
             catch (Exception ex)
             {
                 return Ok(ex);
            }
                   
        }


        [HttpPost, Route("doInput")]
        public IHttpActionResult doInput(PostReconcileDataView post)
        {
            int ListId = 0;
            /*try
            {

                int ListId = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = client.doInput(UserManager.User.UserToken, post.soNumb);
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }*/

            return Ok(ListId);
            
        }
    }
}