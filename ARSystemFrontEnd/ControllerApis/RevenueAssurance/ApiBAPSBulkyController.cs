using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Web;
using System.Collections.Specialized;
using ARSystemFrontEnd.Helper;
using System.IO;
using System.Configuration;
using ARSystem.Service.RevenueAssurance;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/BAPSBulky")]
    public class ApiBAPSBulkyController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetBAPSToGrid(PostTrxBAPSBulkyView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBAPSBulky> BAPSData = new List<ARSystemService.vwBAPSBulky>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    intTotalRecord = client.GetBAPSBulkyCount(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.PONumber.ToArray());

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    BAPSData = client.GetBAPSBulkyToList(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.PONumber.ToArray(), strOrderBy, post.start, post.length).ToList();
                }
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = BAPSData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SitelistGrid")]
        public IHttpActionResult GetSiteListData(PostTrxBAPSBulkyView post)
        {
            try
            {
                List<ARSystemService.vwBAPSBulky> BapsData = new List<ARSystemService.vwBAPSBulky>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    BapsData = client.GetListDataToList(UserManager.User.UserToken, post.ListId.ToArray()).ToList();
                }

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridProcess")]
        public IHttpActionResult GetBAPSProcessToGrid(PostTrxBAPSBulkyView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBAPSBulkyHeader> BAPSData = new List<ARSystemService.vwBAPSBulkyHeader>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    intTotalRecord = client.GetBAPSBulkyProcessCount(UserManager.User.UserToken);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    BAPSData = client.GetBAPSBulkyProcessToList(UserManager.User.UserToken, strOrderBy, post.start, post.length).ToList();
                }
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = BAPSData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridProcessDetail")]
        public IHttpActionResult GetBOQDataProcessDetailToList(PostTrxBAPSBulkyView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBAPSBulkyDetail> boqdatadetail = new List<ARSystemService.vwBAPSBulkyDetail>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    intTotalRecord = client.GetBAPSBulkyProcessDetailCount(UserManager.User.UserToken, post.ID);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    boqdatadetail = client.GetBAPSBulkyProcessDetailToList(UserManager.User.UserToken, post.ID, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = boqdatadetail });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridDone")]
        public IHttpActionResult GetBAPSDoneToGrid(PostTrxBAPSBulkyView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBAPSBulkyDoneHeader> BAPSData = new List<ARSystemService.vwBAPSBulkyDoneHeader>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    intTotalRecord = client.GetBAPSBulkyDoneCount(UserManager.User.UserToken, post.CompanyID, post.ListBAPSNumber.ToArray());

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    BAPSData = client.GetBAPSBulkyDoneToList(UserManager.User.UserToken, post.CompanyID, post.ListBAPSNumber.ToArray(), strOrderBy, post.start, post.length).ToList();
                }
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = BAPSData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridDoneDetail")]
        public IHttpActionResult GetBOQDataDoneDetailToList(PostTrxBAPSBulkyView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBAPSBulkyDoneDetail> baps = new List<ARSystemService.vwBAPSBulkyDoneDetail>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    intTotalRecord = client.GetBAPSBulkyDoneDetailCount(UserManager.User.UserToken, post.ID);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    baps = client.GetBAPSBulkyDoneDetailToList(UserManager.User.UserToken, post.ID, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = baps });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        //[HttpPost, Route("DoclistGrid")]
        //public IHttpActionResult GetDocListData(PostTrxBAPSBulkyView post)
        //{
        //    try
        //    {
        //        List<ARSystemService.mstBAPSDocument> BapsData = new List<ARSystemService.mstBAPSDocument>();
        //        using (var client = new ARSystemService.BAPSBulkyServiceClient())
        //        {
        //            BapsData = client.GetDocDataToList(UserManager.User.UserToken, post.ListId.ToArray()).ToList();
        //        }

        //        return Ok(BapsData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        [HttpGet, Route("PORef")]
        public IHttpActionResult GetPORefToList(string strCompany, string strOperator)
        {
            try
            {
                List<ARSystemService.vwPORefReconcile> PORef = new List<ARSystemService.vwPORefReconcile>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    PORef = client.GetPOReferenceToList(UserManager.User.UserToken, strCompany, strOperator).ToList();
                }

                return Ok(PORef);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Operator")]
        public IHttpActionResult GetOperatorToList()
        {
            try
            {
                var service = new BAPSBulkyService();

                List<mstOperator> Operator = new List<mstOperator>();
                //using (var client = new ARSystemService.BAPSBulkyServiceClient())
                //{
                //    Operator = client.GetOperatorList(UserManager.User.UserToken, "").ToList();
                //}

                Operator = service.GetOperatorList(UserManager.User.UserToken, "", UserManager.User.UserID);

                return Ok(Operator);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Status")]
        public IHttpActionResult getStatus(string CurrentActivity)
        {
            try
            {
                List<ARSystemService.vmStatus> list = new List<ARSystemService.vmStatus>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    list = client.GetStatus(UserManager.User.UserToken, CurrentActivity).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //[HttpPost, Route("BAPSDocument")]
        //public IHttpActionResult GetBAPSDocumentToList(PostTrxBAPSDocumentView post)
        //{
        //    try
        //    {

        //        int intTotalRecord = 0;

        //        List<ARSystemService.mstBAPSDocument> Document = new List<ARSystemService.mstBAPSDocument>();
        //        using (var client = new ARSystemService.BAPSBulkyServiceClient())
        //        {
        //            intTotalRecord = client.GetBAPSDocumentCount(UserManager.User.UserToken, post.CustomerID);

        //            string strOrderBy = "";
        //            if (post.order != null)
        //                if (post.columns[post.order[0].column].data != "0")
        //                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

        //            Document = client.GetBAPSDocumentToList(UserManager.User.UserToken, post.CustomerID, strOrderBy).ToList();
        //        }
        //        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Document });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        [HttpPost, Route("SaveBAPSBulky")]
        public IHttpActionResult SaveBAPSBulky(PostTrxBAPSBulkyProcess post)
        {
            try
            {

                var BAPS = new mstBAPSRecurring();
                var service = new BAPSBulkyService();
                //using (var client = new ARSystemService.BAPSBulkyServiceClient())
                //{
                //    ARSystemService.vmSaveBAPSBulky VM = new ARSystemService.vmSaveBAPSBulky();
                //    VM.BAPSNumber = post.BAPSNumber;
                //    VM.CompanyID = post.CompanyID;
                //    VM.CustomerID = post.CustomerID;
                //    VM.TotalTenant = post.TotalTenant;
                //    VM.TotalAmount = post.TotalAmount;
                //    VM.BAPSSignDate = post.BAPSSignDate;
                //    VM.Remarks = post.Remarks;
                //    VM.ListTrxBAPS = post.ListTrxBAPS.ToArray();

                //    BAPS = client.SaveBAPSBulky(UserManager.User.UserToken, VM);
                //}

                var VM = new vmSaveBAPSBulky();
                VM.BAPSNumber = post.BAPSNumber;
                VM.CompanyID = post.CompanyID;
                VM.CustomerID = post.CustomerID;
                VM.TotalTenant = post.TotalTenant;
                VM.TotalAmount = post.TotalAmount;
                VM.BAPSSignDate = post.BAPSSignDate;
                VM.Remarks = post.Remarks;
                VM.ListTrxBAPS = post.ListTrxBAPS;

                BAPS = service.SaveBAPSBulky(VM, UserManager.User.UserID);


                return Ok(BAPS);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("BacktoBAPSInput")]
        public IHttpActionResult BacktoBOQInput(PostTrxBAPSBulkyProcess post)
        {

            try
            {
                var service = new BAPSBulkyService();
                var BAPS = new mstBAPSRecurring();
                //using (var client = new ARSystemService.BAPSBulkyServiceClient())
                //{
                //    ARSystemService.vmSaveBAPSBulky vm = new ARSystemService.vmSaveBAPSBulky();
                //    vm.detailIDs = post.detailIDs.ToArray();
                //    BAPS = client.BacktoBAPSInput(UserManager.User.UserToken, vm);

                //    return Ok(BAPS);
                //}

                var vm = new vmSaveBAPSBulky();
                vm.detailIDs = post.detailIDs.ToList();
                BAPS = service.BacktoBAPSInput(UserManager.User.UserToken, vm, UserManager.User.UserID);

                return Ok(BAPS);

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
                //NameValueCollection nvc = HttpContext.Current.Request.Form;

                //HttpPostedFile postedFile = HttpContext.Current.Request.Files["BAPSDoc"];

                //var ID = nvc.Get("BAPSID");

                //var strFilePath = MapModel(postedFile, Convert.ToInt32(ID));

                //return Json(new { strFilePath });

                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile postedFile = HttpContext.Current.Request.Files["BAPSDoc"];

                //string strBOQNumber = "";
                //strBOQNumber = nvc.Get("BOQNumber");
                string strID = "";
                strID = nvc.Get("ID");

                string strFileName = "";
                //strFileName = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);
                strFileName = postedFile.FileName;

                string strFilePath = "";
                strFilePath = MapModel(postedFile);

                string strContentType = "";
                strContentType = postedFile.ContentType;

                ARSystemService.trxRAUploadDocument BAPSDoc;
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    BAPSDoc = client.UploadFileBAPSDoc(UserManager.User.UserToken, strID, strFileName, strFilePath, strContentType);
                }

                return Ok(BAPSDoc);


            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //private ARSystemService.trxRAUploadDocument MapModel(HttpPostedFile postedFile, int postedID)
        //{

        //    string filePath = "";
        //    using (var client = new ARSystemService.ImstDataSourceServiceClient())
        //    {
        //        filePath = client.GetPath(UserManager.User.UserToken, "BAPSRecurring");
        //    }

        //    ARSystemService.trxRAUploadDocument uploadDoc = new ARSystemService.trxRAUploadDocument();
        //    using (var client = new ARSystemService.BAPSBulkyServiceClient())
        //    { 
        //        if (postedFile != null)
        //        {

        //            string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);

        //            uploadDoc.mstRADocumentTypeID = 3;
        //            uploadDoc.TransactionID = postedID;
        //            uploadDoc.ContentType = postedFile.ContentType;
        //            uploadDoc.FilePath = filePath + "BAPS_DOCUMENT" + fileTimeStamp;
        //            uploadDoc.FileName = postedFile.FileName;

        //            uploadDoc = client.UploadFile(UserManager.User.UserToken, uploadDoc);

        //            string path = System.Web.HttpContext.Current.Server.MapPath(filePath);
        //            DirectoryInfo di = new DirectoryInfo(path);
        //            if (!di.Exists)
        //                di.Create();

        //            postedFile.SaveAs(path + "BAPS_DOCUMENT" + fileTimeStamp);
        //        }
        //    }
        //    return uploadDoc;
        //}

        private string MapModel(HttpPostedFile postedFile)
        {

            string filePath = "";
            if (postedFile != null)
            {
                string dir = "\\RevenueAssurance\\BAPS\\";
                string filename = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                filePath = dir + "BAPS_RECURRING_" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + "BAPS_RECURRING_" + fileTimeStamp);
            }
            return filePath;
        }

        [HttpPost, Route("ApproveRejectBAPSData")]
        public IHttpActionResult ApproveRejectBAPSData(PostTrxBAPSBulkyProcess post)
        {
            try
            {
                ARSystemService.mstBAPSRecurring BAPS = new ARSystemService.mstBAPSRecurring();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    ARSystemService.vmSaveBAPSBulky VM = new ARSystemService.vmSaveBAPSBulky();
                    VM.ID = post.ID;
                    VM.mstRAActivityID = post.mstRAActivityID;
                    VM.RemarksApproval = post.RemarksApproval;
                    BAPS = client.ApproveRejectBAPSData(UserManager.User.UserToken, VM);
                }

                return Ok(BAPS);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("UpdateDetailBAPS")]
        public IHttpActionResult UpdateDetailBAPS(PostTrxBAPSBulkyProcess post)
        {
            try
            {
                ARSystemService.mstBAPSRecurring mst = new ARSystemService.mstBAPSRecurring();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    ARSystemService.vmSaveBAPSBulky VM = new ARSystemService.vmSaveBAPSBulky();
                    VM.ID = post.ID;
                    VM.trxReconcileID = post.trxReconcileID;
                    VM.SONumber = post.SoNumber;
                    VM.CustomerSiteID = post.CustomerSiteID;
                    VM.CustomerSiteName = post.CustomerSiteName;
                    VM.CustomerMLANumber = post.CustomerMLANumber;
                    VM.BaseLeasePrice = post.BaseLeasePrice;
                    VM.ServicePrice = post.ServicePrice;
                    VM.DeductionAmount = post.DeductionAmount;
                    VM.AmountIDR = post.AmountIDR;
                    VM.RFIOprDate = post.RFIOprDate == "0" ? (DateTime?)null : DateTime.Parse(post.RFIOprDate);
                    mst = client.UpdateDetailBAPS(UserManager.User.UserToken, VM);
                }

                return Ok(mst);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("DeleteDetailBAPS")]
        public IHttpActionResult DeleteDetailBAPS(PostTrxBAPSBulkyProcess post)
        {
            try
            {
                ARSystemService.mstBAPSRecurring mst = new ARSystemService.mstBAPSRecurring();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    ARSystemService.vmSaveBAPSBulky VM = new ARSystemService.vmSaveBAPSBulky();
                    VM.ID = post.ID;
                    VM.trxReconcileID = post.trxReconcileID;
                    mst = client.DeleteDetailBAPS(UserManager.User.UserToken, VM);
                }

                return Ok(mst);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("ApproveBAPSData")]
        public IHttpActionResult ApproveBAPSData(PostTrxBAPSBulkyProcess post)
        {
            try
            {
                ARSystemService.mstBAPSRecurring BAPS = new ARSystemService.mstBAPSRecurring();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    BAPS = client.ApproveBAPSData(UserManager.User.UserToken, post.ID);
                }

                return Ok(BAPS);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxBAPSBulkyView post)
        {
            try
            {

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    var strPONumber = new List<string>();
                    strPONumber.Add("0");

                    ListId = client.GetListBAPSBulkyId(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.PONumber == null ? strPONumber.ToArray() : post.PONumber.ToArray()).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListProcessId")]
        public IHttpActionResult GetListProcessId(PostTrxBAPSBulkyView post)
        {
            try
            {
                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    ListId = client.GetListBAPSProcessBulkyId(UserManager.User.UserToken).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListDoneId")]
        public IHttpActionResult GetListDoneId(PostTrxBAPSBulkyView post)
        {
            try
            {
                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.BAPSBulkyServiceClient())
                {
                    var strBAPSNumber = new List<string>();
                    strBAPSNumber.Add("0");

                    ListId = client.GetListBAPSDoneBulkyId(UserManager.User.UserToken, post.CompanyID, post.ListBAPSNumber == null ? strBAPSNumber.ToArray() : post.ListBAPSNumber.ToArray()).ToList();
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