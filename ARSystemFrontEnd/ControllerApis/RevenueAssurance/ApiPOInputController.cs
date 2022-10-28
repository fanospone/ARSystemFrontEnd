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

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/POInput")]
    public class ApiPOInputController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetPOInputToGrid(PostPOInputView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.trxRAPurchaseOrder> POInput = new List<ARSystemService.trxRAPurchaseOrder>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    intTotalRecord = client.GetPOInputCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strGroupBy,post.strCurrency, post.isRaw);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    POInput = client.GetPOInputToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strGroupBy, post.strCurrency, strOrderBy, post.start, post.length, post.isRaw).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = POInput });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostPOInputView post)
        {
            try
            {

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    ListId = client.GetPOInputListId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strGroupBy, post.isRaw).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDetail")]
        public IHttpActionResult GetDetail(PostPOInputView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAReconcile> POInput = new List<ARSystemService.vwRAReconcile>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    POInput = client.GetReconcileDataToListByBoq(UserManager.User.UserToken, post.strBoqID, post.strCompanyId, post.strOperator, post.strRegional, post.trxRAPurchaseOrderID).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = POInput });
                
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SaveData")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile FilePO = HttpContext.Current.Request.Files["FilePO"];


                ARSystemService.trxRAPurchaseOrder data = new ARSystemService.trxRAPurchaseOrder();
                List<ARSystemService.trxReconcile> detail = new List<ARSystemService.trxReconcile>();
                ARSystemService.trxRAUploadDocument doc = new ARSystemService.trxRAUploadDocument();
                List<ARSystemService.trxRAPurchaseOrderDetail> PoDetail = new List<ARSystemService.trxRAPurchaseOrderDetail>();

                data.PONumber = nvc.Get("PONumber");

                data.PODate = DateTime.Parse(nvc.Get("PODate").ToString());
                data.POReceiveDate = DateTime.Parse(nvc.Get("POReceiveDate").ToString());
                data.StartPeriod = DateTime.Parse(nvc.Get("StartPeriod").ToString());
                data.EndPeriod = DateTime.Parse(nvc.Get("EndPeriod").ToString());
                data.CustomerID = nvc.Get("CustomerID");
                data.CompanyID = nvc.Get("CompanyID");
                data.Regional = nvc.Get("Regional");
                data.mstRABoqID = nvc.Get("mstRABoqID");
                data.Remarks = nvc.Get("Remarks");
                data.Currency = nvc.Get("Currency"); 
                data.POType = nvc.Get("POType");
                data.mstRAActivityID = Convert.ToInt32(nvc.Get("mstRAActivityID"));
                data.CreatedDate = DateTime.Now;

                var dtl = nvc.Get("PoDetail");
                if (!string.IsNullOrEmpty(dtl))
                    PoDetail = JsonConvert.DeserializeObject<List<ARSystemService.trxRAPurchaseOrderDetail>>(dtl).ToList();
                else
                    PoDetail = new List<ARSystemService.trxRAPurchaseOrderDetail>();

                var TempID = nvc.Get("ListID");
                string[] splitID = TempID.Split(',');

                foreach(var item in splitID)
                {
                    detail.Add(new ARSystemService.trxReconcile
                    {
                        ID = Convert.ToInt64(item),
                        mstRAActivityID = data.mstRAActivityID
                    });
                }

                data.TotalTenant = Convert.ToInt32(nvc.Get("TotalTenant"));
                string Amount = nvc.Get("POAmount");
                Amount = Amount.Replace(",","");
                data.POAmount = Convert.ToDecimal(Amount);

                //if (!string.IsNullOrEmpty(BAOtherCode))
                //{
                //    BAOther = HttpContext.Current.Request.Files["BAOther"];
                //}

                //string Id = nvc.Get("Id");
                //string strBOQNumber = "";
                //strBOQNumber = nvc.Get("BOQNumber");

                //string strFileName = "";
                //strFileName = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);

                //string strFilePath = "";
                var strFilePath = MapModel(FilePO, nvc.Get("PONumber"), doc);
                var Result = 0;
                //ARSystemService.mstOPMRABOQ BOQDoc;
                if(strFilePath != null)
                {
                    using (var client = new ARSystemService.POInputServiceClient())
                    {
                        Result = client.SavePO(UserManager.User.UserToken, data, doc, detail.ToArray(), PoDetail.ToArray());
                    }
                }

                //var Result = Id.Split(',');

                return Ok(Result);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        private string MapModel(HttpPostedFile postedFile, string PONumber, ARSystemService.trxRAUploadDocument doc)
        {
            
            string filePath = "\\RevenueAssurance\\PO\\";
            //using (var client = new ARSystemService.ImstDataSourceServiceClient())
            //{
            //    filePath = client.GetPath(UserManager.User.UserToken, "PO");
            //}

            if (postedFile != null)
            {
                //string dir = "\\PO\\";
                //string filename = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);
                //string fileext = postedFile.ContentType;//.FileName.Substring(postedFile.FileName.Length - 4, postedFile.FileName.Length);
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                //filePath = dir + filename + "_" + fileTimeStamp;

                doc.ContentType = postedFile.ContentType;
                doc.CreatedDate = DateTime.Now;
                doc.FileName = postedFile.FileName;
                doc.mstRADocumentTypeID = 2;
                doc.FilePath =  filePath + "PO_DOCUMENT" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + filePath);
                filePath = path + "PO_DOCUMENT" + fileTimeStamp;


                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(filePath);
            }
            return doc.FilePath;
        }


        [HttpPost, Route("GetBOQList")]
        public IHttpActionResult GetBOQList(PostPOInputView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstRABoq> BOQList = new List<ARSystemService.mstRABoq>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    BOQList = client.GetBoqList(UserManager.User.UserToken, post.strCompanyId, post.strRegional).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = BOQList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetBOQDetail")]
        public IHttpActionResult GetBOQDetail(PostPOInputView post)
        {
            try
            {
                string Result = "";
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    Result = client.GetBoqDetail(UserManager.User.UserToken, post.trxRAPurchaseOrderID);
                }

                return Ok(new { data = Result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdateNextActivity")]
        public IHttpActionResult UpdateNextActivity(PostPOInputView post)
        {
            try
            {
                int Result = 0;

                using (var client = new ARSystemService.POInputServiceClient())
                {
                    Result = client.PONextActivity(UserManager.User.UserToken, post.Activity, post.trxRAPurchaseOrderID);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdatePODone")]
        public IHttpActionResult UpdatePODone()
        {
            try
            {
                ARSystemService.trxRAPurchaseOrder data = new ARSystemService.trxRAPurchaseOrder();
                data.PONumber = HttpContext.Current.Request.Form.Get("PONumber");
                data.ID = long.Parse(HttpContext.Current.Request.Form.Get("ID"));

                ARSystemService.trxRAUploadDocument doc = new ARSystemService.trxRAUploadDocument();
                HttpPostedFile FilePO = HttpContext.Current.Request.Files["UploadDocument"];
                string strFilePath = string.Empty;
                if (FilePO != null)
                    strFilePath = MapModel(FilePO, data.PONumber, doc);

                using (var client = new ARSystemService.POInputServiceClient())
                {
                    data = client.UpdatePODone(UserManager.User.UserToken, data, doc);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { GenericError = ex.Message });
            }
        }

        #region NON TSEL
        [HttpPost, Route("gridData")]
        public IHttpActionResult GetDataReconcileToGrid(PostFilterReconcilePO post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAData> POInput = new List<ARSystemService.vwRAData>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    string strWhereClause = post.strBapsType == "INF" ? " mstRAactivityID >= 3 AND " : " mstRAActivityID = 3 AND ";
                    strWhereClause = GetParam(post, strWhereClause);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    intTotalRecord = client.GetReconcileCount(UserManager.User.UserToken, strWhereClause,post.strBapsType);

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
                string strWhereClause = post.strBapsType == "INF" ? " mstRAactivityID >= 3 AND " : " mstRAActivityID = 3 AND ";
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

        [HttpPost, Route("CheckSplitPO")]
        public IHttpActionResult CheckSplitPO(PostFilterReconcilePO post)
        {
            try
            {
                List<ARSystemService.vmStatus> ListId = new List<ARSystemService.vmStatus>();
                using (var client = new ARSystemService.POInputServiceClient())
                {
                    ListId = client.CheckSplitPO(UserManager.User.UserToken, post.param.ToArray()).ToList();
                }

                string ListSoNumber = "";
                if(ListId != null && ListId.Count > 0 )
                {
                    ListSoNumber = "SONumber : ";
                    foreach (var item in ListId)
                    {
                        ListSoNumber += item.ValueId.ToString();
                    }
                    ListSoNumber = " Already have split PO with current billing type selected!";
                }

                return Ok(ListSoNumber);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

    }
}