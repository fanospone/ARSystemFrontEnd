using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.ControllerApis.RevenueAssurance
{
    [RoutePrefix("api/UpdateInflation")]
    public class ApiUpdateInflationController : ApiController
    {
        private string GetParam(PostFilterReconcilePO post, string strWhereClause = "")
        {
            if (!string.IsNullOrWhiteSpace(post.strFilterID))
            {
                strWhereClause += "id in (" + post.strFilterID + ") AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCompanyId))
            {
                strWhereClause += "CompanyInvoice = '" + post.strCompanyId + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCustomerId))
            {
                strWhereClause += "CustomerInvoice = '" + post.strCustomerId + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPowerType))
            {
                strWhereClause += "PowerTypeID = " + post.strPowerType + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strProduct))
            {
                strWhereClause += "ProductID = " + post.strProduct + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strQuarterly))
            {
                strWhereClause += "Quartal = " + post.strQuarterly + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCurrency))
            {
                strWhereClause += "BaseLeaseCurrency = '" + post.strCurrency + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteID))
            {
                strWhereClause += "SiteID LIKE '%" + post.strSiteID + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteName))
            {
                strWhereClause += "SiteName LIKE '%" + post.strSiteName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSONumber))
            {
                strWhereClause += "SONumber LIKE '%" + post.strSONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBapsType))
            {
                strWhereClause += "BapsType LIKE '%" + post.strBapsType + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strStipID))
            {
                strWhereClause += "STIPID = " + post.strStipID + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strYear))
            {
                strWhereClause += "YEAR = " + post.strYear + " AND ";
            }

            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }

        [HttpPost, Route("gridData")]
        public IHttpActionResult GetDataReconcileToGrid(PostFilterReconcilePO post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAData> POInput = new List<ARSystemService.vwRAData>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    string strWhereClause = post.strBapsType == "INF" ? " mstRAactivityID <= 12 AND " : " mstRAActivityID = 3 AND ";
                    strWhereClause = GetParam(post, strWhereClause);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    intTotalRecord = client.GetReconcileCount(UserManager.User.UserToken, strWhereClause, post.strBapsType);

                    POInput = client.GetReconcileToList(UserManager.User.UserToken, strWhereClause, post.strBapsType, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = POInput });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDataListId")]
        public IHttpActionResult GetDataListId(PostFilterReconcilePO post)
        {
            try
            {
                string strWhereClause = post.strBapsType == "INF" ? " mstRAactivityID <= 12 AND " : " mstRAActivityID = 3 AND ";
                strWhereClause = GetParam(post, strWhereClause);
                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    ListId = client.GetListIdNonTSEL(UserManager.User.UserToken, strWhereClause, post.strBapsType).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdateData")]
        public IHttpActionResult UpdateData(UpdateInflationModel post)
        {
            try
            {
                ARSystemService.trxReconcile rst = new ARSystemService.trxReconcile();

                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    rst = client.UpdateReconcileInflation(UserManager.User.UserToken, post.ListID, post.Percentage, post.Year);
                }

                return Ok(rst);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        public static IList<List<string>> Split<T>(List<string> source)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 100)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        [HttpPost, Route("InsertData")]
        public IHttpActionResult InsertData(UpdateInflationModel post)
        {
            try
            {
                ARSystemService.vwRAMstBaps rst = new ARSystemService.vwRAMstBaps();

                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    var result = client.CreateBapsInflation(UserManager.User.UserToken, post.ListID);

                    if (result != 0)
                    {
                        rst.ID = result;
                    }
                    else
                    {
                        rst.ErrorType = 1;
                        rst.ErrorMessage = "Error on system !";
                    }
                }

                return Ok(rst);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridBaps")]
        public IHttpActionResult gridBaps(PostFilterReconcilePO post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAMstBaps> POInput = new List<ARSystemService.vwRAMstBaps>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    string strWhereClause = "  ";
                    strWhereClause = GetParam(post, strWhereClause);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    intTotalRecord = client.GetCountListMstBaps(UserManager.User.UserToken, strWhereClause);

                    POInput = client.GetListMstBaps(UserManager.User.UserToken, strWhereClause, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = POInput });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}