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
using ARSystem.Service.RevenueAssurance;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/RTI")]
    public class ApiRTIController : ApiController
    {
        private readonly vwRTIDataDoneService _vwRTIDataDoneService;
        private readonly vwRTIDataService _vwRTIDataService;
        public ApiRTIController()
        {
            _vwRTIDataService = new vwRTIDataService();
            _vwRTIDataDoneService = new vwRTIDataDoneService();
        }

        //[HttpPost, Route("grid")]
        //public IHttpActionResult grid(trxRARTIPost post)
        //{
        //    try
        //    {
        //        int intTotalRecord = 0;
        //        ARSystemService.trxRARTI model = new ARSystemService.trxRARTI();
        //        model.BAPSNumber = post.BAPS;
        //        model.PONumber = post.PO;
        //        model.CustomerID = post.CustomerID;
        //        model.CompanyID = post.CompanyID;
        //        model.SONumber = post.SONumber;
        //        model.isRaw = post.isRaw;

        //        List<ARSystemService.vwRAReconcile> Result = new List<ARSystemService.vwRAReconcile>();
        //        using (var client = new ARSystemService.RTIServiceClient())
        //        {
        //            intTotalRecord = client.CountData(UserManager.User.UserToken, model);

        //            string strOrderBy = "";
        //            if (post.order != null)
        //                if (post.columns[post.order[0].column].data != "0")
        //                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

        //            Result = client.GetData(UserManager.User.UserToken, model, strOrderBy, post.start, post.length).ToList();
        //        }

        //        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        [HttpPost, Route("grid")]
        public IHttpActionResult grid(trxRARTIPost post)
        {
            try
            {
                int intTotalRecord = 0;
         
                List<vwRTIData> Result = new List<vwRTIData>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    Result.Add(new vwRTIData(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(Result);
                }
                intTotalRecord = _vwRTIDataService.GetCount(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                Result = _vwRTIDataService.GetData(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType, strOrderBy, post.start, post.length).ToList();

                //List<ARSystemService.vwRTIData> Result = new List<ARSystemService.vwRTIData>();
                //using (var client = new ARSystemService.RTIServiceClient())
                //{
                //    intTotalRecord = client.GetCount(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToArray(), post.strPONumber.ToArray(), post.strSONumber.ToArray(), post.Year, post.Quartal, post.BapsType, post.PowerType);

                //    string strOrderBy = "";
                //    if (post.order != null)
                //        if (post.columns[post.order[0].column].data != "0")
                //            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                //    Result = client.GetData(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToArray(), post.strPONumber.ToArray(), post.strSONumber.ToArray(), post.Year, post.Quartal, post.BapsType, post.PowerType, strOrderBy, post.start, post.length).ToList();
                //}

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("gridDone")]
        public IHttpActionResult gridDone(trxRARTIPost post)
        {
            try
            {
                int intTotalRecord = 0;

                List<vwRTIDataDone> Result = new List<vwRTIDataDone>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    Result.Add(new vwRTIDataDone(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(Result);
                }
                intTotalRecord = _vwRTIDataDoneService.GetCountDone(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    Result = _vwRTIDataDoneService.GetDataDone(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }


        //[HttpPost, Route("GetList")]
        //public IHttpActionResult GetList(ARSystemService.trxRARTI post)
        //{
        //    try
        //    {
        //        int intTotalRecord = 0;

        //        List<ARSystemService.trxRARTI> Result = new List<ARSystemService.trxRARTI>();
        //        using (var client = new ARSystemService.RTIServiceClient())
        //        {
        //            Result = client.GetList(UserManager.User.UserToken, post).ToList();
        //        }

        //        var Process = Result.Where(w => string.IsNullOrEmpty(w.FilePath)).ToList();
        //        var Done = Result.Where(w => !string.IsNullOrEmpty(w.FilePath)).ToList();

        //        return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Process, done = Done });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        /*
        [HttpPost, Route("GetData")]
        public IHttpActionResult GetData(ARSystemService.trxRARTI post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRAReconcile> Result = new List<ARSystemService.vwRAReconcile>();
                using (var client = new ARSystemService.RTIServiceClient())
                {
                    Result = client.GetData(UserManager.User.UserToken, post).ToList();
                }

                return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Result });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }*/

        [HttpPost, Route("ValidationSplitAmount")]
        public IHttpActionResult GetValidationSplitAmount(trxRARTIPost post)
        {
            bool result = false;

            try
            {
                using (var client = new ARSystemService.RTIServiceClient())
                {
                    result = client.ValidationUpload(UserManager.User.UserToken, post.trxReconcileID);
                }

                return Ok(result);
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


                trxRARTI data = new trxRARTI();
                List<trxReconcile> detail = new List<trxReconcile>();
                trxRAUploadDocument doc = new trxRAUploadDocument();


                data.Year = nvc.Get("Year");
                data.CustomerID = nvc.Get("CustomerID");
                data.CompanyID = nvc.Get("CompanyID");
                data.Regional = nvc.Get("Regional");
                data.Area = nvc.Get("Area");
                data.Type = nvc.Get("Type");
                data.Currency = nvc.Get("Currency");
                data.PONumber = nvc.Get("PONumber");

                data.TotalSite = Int32.Parse(nvc.Get("TotalSite"));
                data.TotalBoq = Int32.Parse(nvc.Get("TotalBoq"));
                data.TotalPO = Int32.Parse(nvc.Get("TotalPO"));

                data.CreatedDate = DateTime.Now;

                var TempID = nvc.Get("ListID");
                string[] splitID = TempID.Split(',');

                foreach (var item in splitID)
                {
                    detail.Add(new trxReconcile
                    {
                        ID = Convert.ToInt64(item),
                        mstRAActivityID = 10
                    });
                }

                var strFilePath = MapModel(FilePO, nvc.Get("Regional"), doc);
                var Result = 0;
                data.FileName = doc.FileName;
                data.FilePath = doc.FilePath;
                data.ContenType = doc.ContentType;

                if (strFilePath != null || data.Type != "TOWER")
                {

                    Result = _vwRTIDataService.UploadRTI(UserManager.User.UserToken, data, doc, detail.ToList(), data.Type, data.PONumber);
                    //using (var client = new ARSystemService.RTIServiceClient())
                    //{
                    //    Result = client.UploadRTI(UserManager.User.UserToken, data, doc, detail.ToArray(), data.Type, data.PONumber);
                    //}
                }

                return Ok(Result);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        private string MapModel(HttpPostedFile postedFile, string PONumber, trxRAUploadDocument doc)
        {

            string filePath = "\\RevenueAssurance\\RTI\\";

            if (postedFile != null)
            {

                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);

                doc.ContentType = postedFile.ContentType;
                doc.CreatedDate = DateTime.Now;
                doc.FileName = postedFile.FileName;
                doc.mstRADocumentTypeID = 4;
                doc.FilePath = filePath + "RTI_DOCUMENT" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + filePath);
                filePath = path + "RTI_DOCUMENT" + fileTimeStamp;


                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(filePath);
            }
            return doc.FilePath;
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(trxRARTIPost post)
        {
            try
            {
                //ARSystemService.trxRARTI model = new ARSystemService.trxRARTI();
                //model.BAPSNumber = post.BAPS;
                //model.PONumber = post.PO;
                //model.CustomerID = post.CustomerID;
                //model.CompanyID = post.CompanyID;
                //model.SONumber = post.SONumber;
                //model.isRaw = post.isRaw;

                var strBAPSNumber = new List<string>();
                strBAPSNumber.Add("0");

                var strSONumber = new List<string>();
                strSONumber.Add("0");

                var strPONumber = new List<string>();
                strPONumber.Add("0");

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.RTIServiceClient())
                {
                    ListId = client.GetListId(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber == null ? strBAPSNumber.ToArray() : post.strBAPSNumber.ToArray(), post.strPONumber == null ? strPONumber.ToArray() : post.strPONumber.ToArray(), post.strSONumber == null ? strSONumber.ToArray() : post.strSONumber.ToArray(), post.Year, post.Quartal, post.BapsType, post.PowerType).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListReconcilePartition")]
        public IHttpActionResult GetListReconcilePartition(trxRARTIPartitionPost post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwRTIPartition> Result = new List<ARSystemService.vwRTIPartition>();
                using (var client = new ARSystemService.RTIServiceClient())
                {
                    intTotalRecord = client.GetListReconcilePartitionCount(UserManager.User.UserToken, post.trxReconcileID);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    Result = client.GetListReconcilePartition(UserManager.User.UserToken, post.trxReconcileID).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Result });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //[HttpPost, Route("CheckingPeriod")]
        //public IHttpActionResult GetReconcilePartitionValidatePeriod(trxRARTIPartitionPost post)
        //{
        //    bool result = false;

        //    try
        //    {
        //        using (var client = new ARSystemService.RTIServiceClient())
        //        {
        //            result = client.GetReconcilePartitionValidatePeriod(UserManager.User.UserToken, post.trxReconcileID, post.StartInvoiceDate, post.Term);
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }

        //}

        [HttpPost, Route("SaveRTIPartition")]
        public IHttpActionResult InsertRTIPartition(trxRARTIPartitionPost post)
        {
            try
            {
                int result = 0;

                using (var client = new ARSystemService.RTIServiceClient())
                {
                    ARSystemService.vmSaveRTIPartition VM = new ARSystemService.vmSaveRTIPartition();
                    VM.State = post.State;
                    VM.trxReconcileID = post.trxReconcileID;
                    VM.Term = post.Term;
                    VM.EndInvoiceDate = post.EndInvoiceDate;
                    VM.StartPeriodInvoiceDate = post.StartPeriodInvoiceDate;
                    VM.EndPeriodInvoiceDate = post.EndPeriodInvoiceDate;
                    VM.CustomerID = post.CustomerID;
                    VM.BaseLeasePrice = post.BaseLeasePrice;
                    VM.ServicePrice = post.ServicePrice;
                    VM.DropFODistance = post.DropFODistance;
                    VM.ProductID = post.ProductID;
                    result = client.SaveRTIPartition(UserManager.User.UserToken, VM);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("DeleteRTIPartition")]
        public IHttpActionResult DeleteDetailBAPS(trxRARTIPartitionPost post)
        {
            bool result = false;

            try
            {
                using (var client = new ARSystemService.RTIServiceClient())
                {
                    result = client.DeleteRTIPartition(UserManager.User.UserToken, post.trxReconcileID, post.Term);
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
