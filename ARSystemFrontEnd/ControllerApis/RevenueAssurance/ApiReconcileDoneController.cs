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
    [RoutePrefix("api/ReconcileDone")]
    public class ApiReconcileDoneController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetReconcileDoneToGrid(PostReconcileDoneView post)
        { 
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReconcileDone> reconciledone = new List<ARSystemService.vwReconcileDone>();
                using (var client = new ARSystemService.ReconcileDoneServiceClient())
                {
                    intTotalRecord = client.GetReconcileDoneCount(UserManager.User.UserToken, post.strOperator, post.strYear, post.strResidence, post.strRegional, post.strProvince, post.isRaw);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    reconciledone = client.GetReconcileDoneToList(UserManager.User.UserToken, post.strOperator, post.strYear, post.strResidence, post.strRegional, post.strProvince, strOrderBy, post.start, post.length, post.isRaw).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = reconciledone });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostReconcileDoneView post)
        {
            try
            {

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.ReconcileDoneServiceClient())
                {
                    ListId = client.GetReconcileDoneListId(UserManager.User.UserToken, post.strOperator, post.strYear, post.strResidence, post.strRegional, post.strProvince, post.isRaw).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("BackToProcess")]
        public IHttpActionResult BackToProcess(PostReconcileDoneView post)
        {

            try
            {
                int ListId = 0;
                using (var client = new ARSystemService.ReconcileDoneServiceClient())
                {
                    foreach (int sn in post.soNumb)
                    {
                        ListId = 1;//client.doInput(UserManager.User.UserToken, sn);
                    }
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }
}