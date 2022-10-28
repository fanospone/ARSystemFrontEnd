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


namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/BOQData")]
    public class ApiBOQDataController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetBOQDataToGrid(PostTrxBOQDataView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBOQData> boqdata = new List<ARSystemService.vwBOQData>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    intTotalRecord = client.GetBOQDataCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strYear, post.strArea, post.strRegional, post.strSONumber.ToArray());     

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    boqdata = client.GetBOQDataToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strYear, post.strArea, post.strRegional, post.strSONumber.ToArray(), post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = boqdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SitelistGrid")]
        public IHttpActionResult GetSiteListData(PostTrxBOQDataView post)
        {
            try
            {
                List<ARSystemService.vwBOQData> BOQData = new List<ARSystemService.vwBOQData>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    BOQData = client.GetListDataBOQToList(UserManager.User.UserToken, post.ListId.ToArray()).ToList();
                }

                return Ok(BOQData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridProcess")]
        public IHttpActionResult GetBOQDataProcessToList(PostTrxBOQDataProcessView post)

        {
            try
            {
                int intTotalRecord = 0; 

                List<ARSystemService.vwBOQDataHeader> boqdata = new List<ARSystemService.vwBOQDataHeader>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    intTotalRecord = client.GetBOQDataProcessCount(UserManager.User.UserToken, post.strCompanyId, post.listBOQNumber.ToArray());

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    boqdata = client.GetBOQDataProcessToList(UserManager.User.UserToken, post.strCompanyId, post.listBOQNumber.ToArray(), strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = boqdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridProcessDetail")]
        public IHttpActionResult GetBOQDataProcessDetailToList(PostTrxBOQDataProcessView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBOQDataDetail> boqdatadetail = new List<ARSystemService.vwBOQDataDetail>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    intTotalRecord = client.GetBOQDataProcessDetailCount(UserManager.User.UserToken, post.ID);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    boqdatadetail = client.GetBOQDataProcessDetailToList(UserManager.User.UserToken, post.ID).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = boqdatadetail });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridDone")]
        public IHttpActionResult GetBOQDataDoneToList(PostTrxBOQDataProcessView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBOQDataDoneHeader> boqdata = new List<ARSystemService.vwBOQDataDoneHeader>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    intTotalRecord = client.GetBOQDataDoneCount(UserManager.User.UserToken, post.strCompanyId, post.listBOQNumber.ToArray());

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    boqdata = client.GetBOQDataDoneToList(UserManager.User.UserToken, post.strCompanyId, post.listBOQNumber.ToArray(), strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = boqdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("gridDoneDetail")]
        public IHttpActionResult GetBOQDataDoneDetailToList(PostTrxBOQDataProcessView post)

        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwBOQDataDoneDetail> boqdata = new List<ARSystemService.vwBOQDataDoneDetail>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    intTotalRecord = client.GetBOQDataDoneDetailCount(UserManager.User.UserToken, post.ID);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    boqdata = client.GetBOQDataDoneDetailToList(UserManager.User.UserToken, post.ID, strOrderBy).ToList(); 
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = boqdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetSignatory")]
        public IHttpActionResult GetSignatory(string ID)
        {
            try
            {
                List<ARSystemService.trxRABOQSignatory> list = new List<ARSystemService.trxRABOQSignatory>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    list = client.GetSignatory(UserManager.User.UserToken, ID).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApplySignatory")]
        public IHttpActionResult ApplySignatory(PostTrxBOQDataDone post)
        {
            try

            {
                ARSystemService.trxRABOQSignatory Signatory = new ARSystemService.trxRABOQSignatory();

                using (var client = new ARSystemService.BOQDataServiceClient())

                {
                    ARSystemService.vmPostSignatory vm = new ARSystemService.vmPostSignatory();
                    vm.mstRABoqID = post.mstRABoqID;
                    vm.PrintID = post.PrintID;
                    vm.BatchID = post.BatchID;
                    vm.ListSignatory = post.ListSignatory.ToArray();

                    Signatory = client.ApplySignatory(UserManager.User.UserToken, vm);
                }

                return Ok(Signatory);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("ApplyAllSignatory")]
        public IHttpActionResult ApplyAllSignatory(PostTrxBOQDataDone post)
        {
            try

            {
                ARSystemService.trxRABOQSignatory Signatory = new ARSystemService.trxRABOQSignatory();

                using (var client = new ARSystemService.BOQDataServiceClient())

                {
                    ARSystemService.vmPostSignatory vm = new ARSystemService.vmPostSignatory();
                    vm.PrintID = post.PrintID;
                    vm.BatchID = post.BatchID;
                    vm.ListSignatory = post.ListSignatory.ToArray();

                    Signatory = client.ApplyAllSignatory(UserManager.User.UserToken, vm);
                }

                return Ok(Signatory);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("EditSignatory")]
        public IHttpActionResult EditSignatory(PostTrxBOQDataDone post)
        {
            try

            {
                ARSystemService.trxRABOQSignatory Signatory = new ARSystemService.trxRABOQSignatory();

                using (var client = new ARSystemService.BOQDataServiceClient())

                {
                    ARSystemService.vmPostSignatory vm = new ARSystemService.vmPostSignatory();
                    vm.mstRABoqID = post.mstRABoqID;
                    vm.PrintID = post.PrintID;
                    vm.BatchID = post.BatchID;
                    vm.ListSignatory = post.ListSignatory.ToArray();

                    Signatory = client.EditSignatory(UserManager.User.UserToken, vm);
                }

                return Ok(Signatory);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("AreaOM")]
        public IHttpActionResult GetRAAreaOM()
        {
            try
            {
                List<ARSystemService.vwAreaOM> list = new List<ARSystemService.vwAreaOM>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    list = client.GetmstAreaOM(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Region")]
        public IHttpActionResult getRARegion(string strArea)
        {
            try
            {
                List<ARSystemService.vwRegion> list = new List<ARSystemService.vwRegion>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    list = client.GetmstRegional(UserManager.User.UserToken, strArea).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //[HttpGet, Route("Years")]
        //public IHttpActionResult getYears()
        //{
        //    try
        //    {
        //        List<ARSystemService.vmYears> list = new List<ARSystemService.vmYears>();
        //        using (var client = new ARSystemService.BOQDataServiceClient())
        //        {
        //            list = client.GetYears(UserManager.User.UserToken).ToList();
        //        }

        //        return Ok(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

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

        [HttpGet, Route("OperatorSign")]
        public IHttpActionResult getOperatorSign()
        {
            try
            {
                List<ARSystemService.mstRASignature> list = new List<ARSystemService.mstRASignature>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    list = client.getSignature(UserManager.User.UserToken, "BOQ", "Operator").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CompanySign")]
        public IHttpActionResult getCompanySign()
        {
            try
            {
                List<ARSystemService.mstRASignature> list = new List<ARSystemService.mstRASignature>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    list = client.getSignature(UserManager.User.UserToken, "BOQ", "Vendor").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //[HttpGet, Route("GetDataBOQAmount")]
        //public IHttpActionResult GetDataBOQAmount(string ID)
        //{
        //    try
        //    {
        //        List<ARSystemService.vmDataBOQAmount> list = new List<ARSystemService.vmDataBOQAmount>();
        //        using (var client = new ARSystemService.BOQDataServiceClient())
        //        {
        //            list = client.GetDataBOQAmount(UserManager.User.UserToken, ID).ToList();
        //        }

        //        return Ok(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPut, Route("UpdateStatusBOQDetail/{id}")]
        //public IHttpActionResult UpdateStatusBOQDetail(int id)
        //{
        //    try
        //    {
        //        ARSystemService.mstOPMRABOQDetail BOQDetail;
        //        using (var client = new ARSystemService.BOQDataServiceClient())
        //        {
        //            BOQDetail = client.UpdateStatusBOQDetail(UserManager.User.UserToken, id);
        //        }

        //        return Ok(BOQDetail);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpGet, Route("UndoStatusBOQDetail")]
        //public IHttpActionResult UndoStatusBOQDetail(string ID)
        //{
        //    try
        //    {
        //        List<ARSystemService.mstOPMRABOQDetail> BOQDetail = new List<ARSystemService.mstOPMRABOQDetail>();
        //        using (var client = new ARSystemService.BOQDataServiceClient())
        //        {
        //            BOQDetail = client.UndoStatusBOQDetail(UserManager.User.UserToken, ID).ToList();
        //        }

        //        return Ok(BOQDetail);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        [HttpPost, Route("InsertBOQData")]
        public IHttpActionResult InsertBOQData(PostTrxBOQDataProcess post)
        {
            try
            {
                ARSystemService.mstRABoq BOQHeader = new ARSystemService.mstRABoq();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    ARSystemService.vmInsertIntoMstRABoq vm = new ARSystemService.vmInsertIntoMstRABoq();
                    vm.ListTrxReconcile = post.ListTrxReconcile.ToArray();
                    vm.AreaId = post.AreaId;
                    vm.CompanyId = post.CompanyId;
                    vm.TotalTenant = post.TotalTenant;
                    vm.TotalAmount = post.TotalAmount;

                    BOQHeader = client.InsertBOQData(UserManager.User.UserToken, vm);

                    return Ok(BOQHeader);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("BacktoBOQInput")]
        public IHttpActionResult BacktoBOQInput(PostTrxBOQDataProcess post)
        {

            try
            {
                ARSystemService.mstRABoq BOQHeader = new ARSystemService.mstRABoq();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    ARSystemService.vmUpdateMstRABoq vm = new ARSystemService.vmUpdateMstRABoq();
                    vm.detailIDs = post.detailIDs.ToArray();
                    BOQHeader = client.BacktoBOQInput(UserManager.User.UserToken, vm);

                    return Ok(BOQHeader);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("ApproveRejectBOQData")]
        public IHttpActionResult ApproveRejectBOQData(ARSystemService.vmUpdateMstRABoq post)
        {
            try
            {
                ARSystemService.mstRABoq BOQHeader = new ARSystemService.mstRABoq();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    ARSystemService.vmUpdateMstRABoq vm = new ARSystemService.vmUpdateMstRABoq();
                    vm.ID = post.ID;
                    vm.mstRAActivityID = post.mstRAActivityID;
                    vm.Remarks = post.Remarks;
                    BOQHeader = client.ApproveRejectBOQData(UserManager.User.UserToken, vm);
                }

                return Ok(BOQHeader);
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

                HttpPostedFile postedFile = HttpContext.Current.Request.Files["BOQDoc"];

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

                ARSystemService.trxRAUploadDocument BOQDoc;
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    BOQDoc = client.UploadFileBOQDoc(UserManager.User.UserToken, strID, strFileName, strFilePath, strContentType);
                }

                return Ok(BOQDoc);

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
                string dir = "\\RevenueAssurance\\BOQ\\";
                string filename = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                filePath = dir + "BOQ_DOCUMENT_" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + "BOQ_DOCUMENT_" + fileTimeStamp);
            }
            return filePath;
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxBOQDataView post)
        {
            try
            {

                List<int> ListId = new List<int>();

                using (var client = new ARSystemService.BOQDataServiceClient())
                {

                    var strSONumber = new List<string>();
                    strSONumber.Add("0");

                    //ARSystemService.vmBOQView vm = new ARSystemService.vmBOQView();
                    //vm.strCompanyId = post.strCompanyId;
                    //vm.strOperator = post.strOperator;
                    //vm.strYear = post.strYear;
                    //vm.strArea = post.strArea;
                    //vm.strRegional = post.strRegional;
                    //vm.ListSONumber = post.strSONumber == null ? strSONumber.ToArray() : post.strSONumber.ToArray();
                    //ListId = client.GetListDataBOQId(UserManager.User.UserToken, vm).ToList();

                    ListId = client.GetListDataBOQId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strYear, post.strArea, post.strRegional, post.strSONumber == null ? strSONumber.ToArray() : post.strSONumber.ToArray()).ToList();

                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListProcessId")]
        public IHttpActionResult GetListProcessId(PostTrxBOQDataView post)
        {
            try
            {
                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.BOQDataServiceClient())
                {
                    var strBOQNumber = new List<string>();
                    strBOQNumber.Add("0");

                    ListId = client.GetListDataProcessBOQId(UserManager.User.UserToken, post.strBOQNumber == null ? strBOQNumber.ToArray() : post.strBOQNumber.ToArray()).ToList();
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