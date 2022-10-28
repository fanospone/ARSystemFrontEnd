using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Collections.Specialized;
using ARSystemFrontEnd.Helper;
using System.IO;
using System.Web;
using Newtonsoft.Json;

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
                if (post.strSONumber == null)
                {
                    post.strSONumber = new List<string>();
                }

                int intTotalRecord = 0;

                List<ARSystemService.vwRAReconcile> reconciledata = new List<ARSystemService.vwRAReconcile>();


                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth, post.Batch,post.strSONumber.ToArray(),post.TenantType, post.isRaw);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    reconciledata = client.GetReconcileDataToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth,post.Batch, post.strSONumber.ToArray(), post.TenantType, strOrderBy, post.start, post.length, post.isRaw).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = reconciledata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridupload")]
        public IHttpActionResult GetReconcileDataToGridUpload(PostReconcileDataView post)
        {
            try
            {

                int intTotalRecord = 0;

                List<ARSystemService.vwRAReconcile> reconciledata = new List<ARSystemService.vwRAReconcile>();


                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    intTotalRecord = client.GetCountSiteRegional(UserManager.User.UserToken, post.strOperator, post.strRegional, "", post.Batch);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();
                    
                    //reconciledata = client.GetReconcileRegionalToList(UserManager.User.UserToken, post.strOperator, post.strRegional, post.Batch, strOrderBy).ToList();
                    reconciledata = client.GetReconcileSiteRegionalToList(UserManager.User.UserToken,"", post.strOperator, post.strRegional, "", post.Batch, strOrderBy, post.start, post.length).ToList();
                    //intTotalRecord = reconciledata.Count;
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = reconciledata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SitelistGrid")]
        public IHttpActionResult SitelistGrid(PostProcessNextActivity ListID)
        {
            try
            {
                List<ARSystemService.vwRAReconcile> reconciledata = new List<ARSystemService.vwRAReconcile>();

                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    reconciledata = client.GetReconcileSiteRegionalToList(UserManager.User.UserToken, ListID.Id, "", "", "", "", "", 0, 100000).ToList();
                }

                return Ok(reconciledata);
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
                if(post.strSONumber == null)
                {
                    post.strSONumber = new List<string>();
                }

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = client.GetReconcileDataListId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, 
                        post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, 
                        post.strDueDatePerMonth, post.Batch, post.strSONumber.ToArray(), post.TenantType, post.isRaw).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListIdUpload")]
        public IHttpActionResult GetListIdUpload(PostReconcileDataView post)
        {
            try
            {
                if (post.strSONumber == null)
                {
                    post.strSONumber = new List<string>();
                }

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = client.GetListReconcileSiteRegional(UserManager.User.UserToken, post.strOperator, post.strRegional, "", post.Batch).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PICA")]
        public IHttpActionResult doReject(PostProcessNextActivity post)
        {

            try
            {
                long ListId = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    var resuts = client.InsertPICAReconcileBulky(UserManager.User.UserToken, post.mstRAScheduleId.ToArray(), post.Department, post.PICA, post.PIC, post.Remarks, post.ExpiredDate);
                    if (resuts != null)
                        ListId = 1;
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PICAUpdate")]
        public IHttpActionResult PICAUpdate(PostProcessNextActivity post)
        {

            try
            {
                long ListId = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    var resuts = client.UpdatePICAReconcileBulky(UserManager.User.UserToken, post.mstRAScheduleId.ToArray(), post.IsActive,post.ExpiredDate);
                    if (resuts != null)
                        ListId = 1;
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Upload")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile FilePO = HttpContext.Current.Request.Files["File"];


                ARSystemService.vwRAReconcile data = new ARSystemService.vwRAReconcile();
                List<ARSystemService.trxReconcile> detail = new List<ARSystemService.trxReconcile>();
                ARSystemService.trxRAUploadDocument doc = new ARSystemService.trxRAUploadDocument();


                data.ReconYear = Int32.Parse(nvc.Get("ReconYear"));
                data.CustomerID = nvc.Get("CustomerID");
                var ID = nvc.Get("Id").ToString();
                data.RegionID = Int32.Parse(nvc.Get("RegionID"));
                data.BatchID = nvc.Get("Batch");

                data.TotalTenant = Int32.Parse(nvc.Get("TotalTenant"));
                data.TotalAmount = decimal.Parse(nvc.Get("TotalAmount"));

                var strFilePath = MapModel(FilePO, nvc.Get("RegionID"), doc);
                var Result = 0;
                data.FileName = doc.FileName;
                data.FilePath = doc.FilePath;
                data.ContenType = doc.ContentType;

                if (strFilePath != null)
                {
                    using (var client = new ARSystemService.ReconcileDataServiceClient())
                    {
                        Result = client.UploadDocument(UserManager.User.UserToken, data, doc, detail.ToArray(), ID);
                    }
                }

                return Ok(Result);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        private string MapModel(HttpPostedFile postedFile, string PONumber, ARSystemService.trxRAUploadDocument doc)
        {

            string filePath = "\\RevenueAssurance\\Regional\\";

            if (postedFile != null)
            {

                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);

                doc.ContentType = postedFile.ContentType;
                doc.CreatedDate = DateTime.Now;
                doc.FileName = postedFile.FileName;
                doc.mstRADocumentTypeID = 5;
                doc.FilePath = filePath + "Recon_Region_DOCUMENT" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + filePath);
                filePath = path + "Recon_Region_DOCUMENT" + fileTimeStamp;


                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(filePath);
            }
            return doc.FilePath;
        }

        [HttpPost, Route("ProcessNextActivity")]
        public IHttpActionResult ProcessNextActivity(PostProcessNextActivity post)
        {
            int ListId = 0;
            //string id = post.Id.ToString();

            try
            {
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = client.ProcessNextActivity(UserManager.User.UserToken, post.NextActivity, post.mstRAScheduleId.ToArray(),post.ListID);
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("UpdateNextActivity")]
        public IHttpActionResult UpdateNextActivity(PostProcessNextActivity post)
        {
            int ListId = 0;
            string id = post.Id.ToString();

            try
            {
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = client.UpdateNextActivity(UserManager.User.UserToken, post.Id, post.NextActivity);
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("UpdateReconcile")]
        public IHttpActionResult UpdateReconcile(ARSystemService.trxReconcile post)
        {
            try
            {
                ARSystemService.trxReconcile reconciledata = new ARSystemService.trxReconcile();
                
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    reconciledata = client.UpdateReconcileData(UserManager.User.UserToken, post);
                }
                
                return Ok(new { recordsTotal = reconciledata.ID, data = reconciledata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetProRateAmount")]
        public IHttpActionResult GetProRateAmount(PostReconcileDataUpdate post)
        {
            try
            {
                decimal result = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    result = client.GetProRateAmount(UserManager.User.UserToken, post.CustomerID, post.StartInvoiceDate, post.EndInvoiceDate, post.InvoiceAmount, post.ServiceAmount);
                }

                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetProRateAmountSplit")]
        public IHttpActionResult GetProRateAmountSplit(PostReconcileDataUpdate post)
        {
            try
            {
                decimal result = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    result = client.GetProRateAmountSplit(UserManager.User.UserToken, post.CustomerID, post.StartInvoiceDate, post.EndInvoiceDate, post.StartSplitDate, post.EndSplitDate, post.InvoiceAmount, post.ServiceAmount);
                }

                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CheckActiveDate")]
        public IHttpActionResult CheckActiveDate(PostProcessNextActivity post)
        {

            try
            {
                long ListId = 0;
                
                if(post.ExpiredDate >= post.InvoiceDate)
                {
                    ListId = 1;
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdateBulkyAmount")]
        public IHttpActionResult UpdateBulkyAmount()
        {
            try
            {
                int result = 0;
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                PostReconcileUpdateBulkyAmount post = new PostReconcileUpdateBulkyAmount();
                List<ARSystemService.trxReconcile> data = new List<ARSystemService.trxReconcile>();

                var dtl = nvc.Get("data");
                post = JsonConvert.DeserializeObject<PostReconcileUpdateBulkyAmount>(dtl);
                
                foreach(var item in post.ID)
                {
                    data.Add(new ARSystemService.trxReconcile
                    {
                        ID = item,
                        BaseLeasePrice = post.BaseLeasePrice,
                        ServicePrice = post.ServicePrice,
                        InflationAmount = post.InflationAmount,
                        AdditionalAmount = post.AdditionalAmount
                    });
                }

                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    result = client.UpdateBulkyAmount(UserManager.User.UserToken, data.ToArray());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #region old method
        //[HttpPost, Route("grid")]
        //public IHttpActionResult GetReconcileDataToGrid(PostReconcileDataView post)
        //{
        //    try
        //    {
        //        int intTotalRecord = 0;

        //        List<ARSystemService.vwOPMRASow> reconciledata = new List<ARSystemService.vwOPMRASow>();
        //        using (var client = new ARSystemService.ReconcileDataServiceClient())
        //        {
        //            intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth, post.isRaw);

        //            string strOrderBy = "";
        //            if (post.order != null)
        //                if (post.columns[post.order[0].column].data != "0")
        //                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

        //            reconciledata = client.GetReconcileDataToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strRenewalYear, post.strRenewalYearSeq, post.strReconcileType, post.strCurrency, post.strRegional, post.strProvince, post.strDueDatePerMonth, strOrderBy, post.start, post.length, post.isRaw).ToList();
        //        }

        //        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = reconciledata });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        /*
        
        [HttpPost, Route("updateOMPrice")]
        public IHttpActionResult updateOMPrice(PostReconcileDataUpdate post)//(int Id, int Price)
        {
            int ListId = 0;
            try
            {
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    ListId = 0;// client.updateOMPrice(UserManager.User.UserToken, post.Id, post.Price);
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("UploadFile")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile BADocument = HttpContext.Current.Request.Files["BADocument"];

                HttpPostedFile BAOther = null;

                string BADocumentCode = nvc.Get("BADocumentCode");

                string BAOtherCode = nvc.Get("BAOtherCode");

                if (!string.IsNullOrEmpty(BAOtherCode))
                {
                    BAOther = HttpContext.Current.Request.Files["BAOther"];
                }

                string Id = nvc.Get("Id");
                //string strBOQNumber = "";
                //strBOQNumber = nvc.Get("BOQNumber");

                //string strFileName = "";
                //strFileName = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);

                //string strFilePath = "";
                //strFilePath = MapModel(postedFile);

                //ARSystemService.mstOPMRABOQ BOQDoc;
                //using (var client = new ARSystemService.BOQDataServiceClient())
                //{
                //    BOQDoc = client.UploadFileBOQDoc(UserManager.User.UserToken, strBOQNumber, strFileName, strFilePath);
                //}

                var Result = Id.Split(',');

                return Ok(Result);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("doDone")]
        public IHttpActionResult doDone(PostReconcileDataView post)
        {

            try
            {
                int ListId = 1;
                //using (var client = new ARSystemService.ReconcileDataServiceClient())
                //{
                //    foreach (int sn in post.soNumb)
                //    {
                //        ListId = client.doProcess(UserManager.User.UserToken, sn);
                //    }
                //}

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        private string MapModel(HttpPostedFile postedFile)
        {

            string filePath = "";
            if (postedFile != null)
            {
                string dir = "\\RA\\DOC_RECONCILE\\";
                string filename = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                filePath = dir + filename + "_" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + filename + "_" + fileTimeStamp);
            }
            return filePath;
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

            try
            {
                int ListId = 0;
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    foreach (int sn in post.soNumb)
                    {
                        ListId = 0;// client.doInput(UserManager.User.UserToken, sn);
                    }
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
         */
        #endregion
    }
}