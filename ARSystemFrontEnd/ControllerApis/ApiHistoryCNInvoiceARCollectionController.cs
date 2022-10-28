using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/HistoryCNInvoiceARCollection")]
    public class ApiHistoryCNInvoiceARCollectionController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetHistoryCNInvoiceARCollectionDataToGrid(PostTrxHistoryCNInvoiceARCollectionView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwHistoryCNInvoiceARCollection> listData = new List<ARSystemService.vwHistoryCNInvoiceARCollection>();
                using (var client = new ARSystemService.ItrxHistoryCNInvoiceARCollectionServiceClient())
                {
                    intTotalRecord = client.GetHistoryCNInvoiceARCollectionCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.strOperator, post.InvNo,post.CNStatus,post.InvoiceTypeId);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    listData = client.GetHistoryCNInvoiceARCollectionToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.strOperator,  post.InvNo, post.CNStatus, post.InvoiceTypeId, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}