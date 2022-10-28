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

namespace ARSystemFrontEnd.ControllerApis.DashboardRA
{
    [RoutePrefix("api/DashboardXL")]
    public class ApiDashboardXLController : ApiController
    {
        [HttpPost, Route("GetList")]
        public IHttpActionResult GetList(PostDashboardXL post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDashboardIntegrasiXL> data = new List<ARSystemService.vwDashboardIntegrasiXL>();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    data = client.GetDashboardIntegrasiXLToList(UserManager.User.UserToken, MappingParam(post)).ToList();
                }
                intTotalRecord = data.Count;

                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string MappingParam(PostDashboardXL post)
        {
            string strWhereClause = " AND ";

            if (!string.IsNullOrEmpty(post.CustomerID))
            {
                strWhereClause += " PIVOTED.CustomerID = '" + post.CustomerID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.CompanyID))
            {
                strWhereClause += " PIVOTED.CompanyID = '" + post.CompanyID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.Year))
            {
                strWhereClause += " PIVOTED.Year = " + post.Year + " AND ";
            }

            if (!string.IsNullOrEmpty(post.StipCategory))
            {
                strWhereClause += " PIVOTED.StipID = " + post.StipCategory + " AND ";
            }

            if (!string.IsNullOrEmpty(post.TenantType))
            {
                strWhereClause += " PIVOTED.ProductID = " + post.TenantType + " AND ";
            }

            strWhereClause += " 1=1 ";

            return strWhereClause;
        }

        [HttpPost, Route("GetListDetail")]
        public IHttpActionResult GetListDetail(PostDashboardXL post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAIntegrasiXL> data = new List<ARSystemService.vwRAIntegrasiXL>();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetvwRAIntegrasiXLToList(UserManager.User.UserToken, MappingParamDetail(post), post.start, post.length).ToList();

                    intTotalRecord = data.Count;
                }



                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string MappingParamDetail(PostDashboardXL post)
        {
            string strWhereClause = " AND ";

            if (!string.IsNullOrEmpty(post.CustomerID))
            {
                strWhereClause += " CustomerID = '" + post.CustomerID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.CompanyID))
            {
                strWhereClause += " CompanyID = '" + post.CompanyID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.Year))
            {
                strWhereClause += " Year = " + post.Year + " AND ";
            }

            if (!string.IsNullOrEmpty(post.StipCategory))
            {
                strWhereClause += " StipID = " + post.StipCategory + " AND ";
            }

            if (!string.IsNullOrEmpty(post.TenantType))
            {
                strWhereClause += " ProductID = " + post.TenantType + " AND ";
            }

            if (!string.IsNullOrEmpty(post.LeadTime))
            {
                strWhereClause += " LeadTime = '" + post.LeadTime + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.ID) || !string.IsNullOrEmpty(post.StatusID))
            {
                if (post.ID == "0")
                {
                    strWhereClause += " ID = '" + post.StatusID + "' AND ";
                }
                else
                {
                    strWhereClause += " (ParentID = '" + post.ID + "' OR ID = '" + post.StatusID + "' ) AND ";
                }
            }


            strWhereClause += " 1=1 ";

            return strWhereClause;
        }

        [HttpPost, Route("GetListDetailParent")]
        public IHttpActionResult GetListDetailParent(PostDashboardXL post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAIntegrasiXL> data = new List<ARSystemService.vwRAIntegrasiXL>();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetvwRAIntegrasiXLToList(UserManager.User.UserToken, MappingParamDetailParent(post), post.start, post.length).ToList();

                    intTotalRecord = data.Count;
                }



                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string MappingParamDetailParent(PostDashboardXL post)
        {
            string strWhereClause = " AND ";

            if (!string.IsNullOrEmpty(post.CustomerID))
            {
                strWhereClause += " CustomerID = '" + post.CustomerID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.CompanyID))
            {
                strWhereClause += " CompanyID = '" + post.CompanyID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.Year))
            {
                strWhereClause += " Year = " + post.Year + " AND ";
            }

            if (!string.IsNullOrEmpty(post.StipCategory))
            {
                strWhereClause += " StipID = " + post.StipCategory + " AND ";
            }

            if (!string.IsNullOrEmpty(post.TenantType))
            {
                strWhereClause += " ProductID = " + post.TenantType + " AND ";
            }

            if (!string.IsNullOrEmpty(post.LeadTime))
            {
                strWhereClause += " LeadTime = '" + post.LeadTime + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.ID) || !string.IsNullOrEmpty(post.StatusID))
            {
                strWhereClause += " (ParentID = '" + post.ID + "' OR ID = '" + post.StatusID + "' ) AND ";
            }

            strWhereClause += " 1=1 ";

            return strWhereClause;
        }

        #region UpdatePICA
        [HttpGet, Route("GetTypeProcess")]
        public IHttpActionResult GetTypeProcess()
        {
            try
            {
                List<ARSystemService.vwRATypeProcessPICA> data = new List<ARSystemService.vwRATypeProcessPICA>();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    data = client.GetvwRATypeProcessPICAToList(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetTypePICA")]
        public IHttpActionResult GetTypePICA(PostNewPICA post)
        {
            try
            {
                List<ARSystemService.vwRATypePICA> data = new List<ARSystemService.vwRATypePICA>();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    data = client.GetvwRATypePICAToList(UserManager.User.UserToken, MappingTypePICA(post)).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetCategoryPICA")]
        public IHttpActionResult GetCategoryPICA(PostNewPICA post)
        {
            try
            {
                List<ARSystemService.vwRACategoryPICA> data = new List<ARSystemService.vwRACategoryPICA>();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    data = client.GetvwRACategoryPICAToList(UserManager.User.UserToken, MappingCategoryPICA(post)).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string MappingTypePICA(PostNewPICA post)
        {
            string strWhereClause = " ";

            if (!string.IsNullOrEmpty(post.Process))
            {
                strWhereClause += " AND Process = '" + post.Process + "' ";
            }

            return strWhereClause;
        }

        private string MappingCategoryPICA(PostNewPICA post)
        {
            string strWhereClause = " ";

            if (!string.IsNullOrEmpty(post.Process))
            {
                strWhereClause += " AND Process = '" + post.Process + "' ";
            }

            if (!string.IsNullOrEmpty(post.TypePICA))
            {
                strWhereClause += " AND TypePICA = '" + post.TypePICA + "'  ";
            }

            //strWhereClause += " 1=1 ";

            return strWhereClause;
        }
        #endregion

        [HttpPost, Route("InsertDataPICA")]
        public IHttpActionResult InsertDataPICA(ARSystemService.TrxRANewPICA post)
        {
            try
            {
                post.CreatedDate = DateTime.Now;
                ARSystemService.TrxRANewPICA result = new ARSystemService.TrxRANewPICA();
                using (var client = new ARSystemService.RADashboardServiceClient())
                {
                    result = client.CreateTrxRANewPICAToList(UserManager.User.UserToken, post);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}