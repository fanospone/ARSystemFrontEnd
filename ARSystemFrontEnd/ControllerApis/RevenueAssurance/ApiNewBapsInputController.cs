using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Web;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;
using ARSystem.Service;
using ARSystem.Domain.Models;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using ARSystem.Domain.Repositories;
using ARSystem.Service.RevenueAssurance;
using System.Globalization;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/NewBapsInput")]
    public class ApiNewBapsInputController : ApiController
    {
        private readonly NewBapsService newBapsService;
        public ApiNewBapsInputController()
        {
            newBapsService = new NewBapsService();
        }

        [HttpPost, Route("getSonumbList")]
        public IHttpActionResult GetSonumbList(PostNewBapsCheckingDocument post)
        {
            try
            { 
                //migrated from backend 1/9/2021
                List<vmNewBapsData> model = new List<vmNewBapsData>();
                int intTotalRecord = 0;
                intTotalRecord = newBapsService.GetCountSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerID, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantType, post.mstRAActivityID,
                    post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, post.strSiteName, post.strSONumberMultiple);
                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                model = newBapsService.GetSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerID, post.strProductId, post.strSoNumber, post.strSiteID, 
                    post.strTenantType, post.mstRAActivityID, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, post.strSiteName, post.strSONumberMultiple, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = model });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("gridSonumbListIds")]//kepake
        public IHttpActionResult GetDataSoNumbListIds(PostNewBapsCheckingDocument post)
        {
            try
            {
                //migrated from backend 1/9/2021
                List<vmNewBapsData> model = new List<vmNewBapsData>();
                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                model = newBapsService.GetSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerID, post.strProductId, post.strSoNumber, post.strSiteID, 
                    post.strTenantType, post.mstRAActivityID, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, post.strSiteName, post.strSONumberMultiple, strOrderBy, post.start, post.length).ToList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("getBapsDone")]
        public async Task<IHttpActionResult> GetBapsDoneList(PostNewBapsCheckingDocument post)
        {

            var dataList = new List<vwRABapsDone>();
            int intTotalRecord = 0;
            try
            {
                var service = new BapsDoneService();
                var param = new vwRABapsDone();
                param.CustomerID = post.strCustomerID;
                param.ActivityID = int.Parse(post.mstRAActivityID);
                param.SoNumber = post.strSoNumber;
                param.ProductID = int.Parse(post.strProductId == null ? "0" : post.strProductId);
                param.CompanyID = post.strCompanyId;
                param.SiteID = post.strSiteID;
          
                dataList = await service.BapsDoneList(param, post.start, post.length);
                intTotalRecord = service.BapsDoneCount(param);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception ex)
            {
                dataList.Add(new vwRABapsDone { ErrorType = 1, ErrorMessage = ex.Message });
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
        }

        [HttpPost, Route("Upload")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                mstBaps data = new mstBaps();
                ARSystemService.vmNewBapsData detail = new ARSystemService.vmNewBapsData();
                trxRAUploadDocument doc = new trxRAUploadDocument();
                trxRANewBapsActivity act = new trxRANewBapsActivity();
                List<trxRASplitNewBaps> split = new List<trxRASplitNewBaps>();

                detail.trxBapsRejectId = Int32.Parse(nvc.Get("trxBapsRejectId"));
                HttpPostedFile FilePO = HttpContext.Current.Request.Files["File"];

                var pwt = string.IsNullOrEmpty(nvc.Get("PowerType")) ? "0" : nvc.Get("PowerType");
                data.BapsDoneDate = DateTime.Now;
                data.ID = Int32.Parse(nvc.Get("mstBapsID"));
                data.BAPSNumber = nvc.Get("BAPSNumber");
                data.BapsDate = DateTime.Parse(nvc.Get("BAPSDate"));
                act.mstRAActivityID = Int32.Parse(nvc.Get("mstRAActivityID"));
                act.SoNumber = nvc.Get("SoNumber");
                act.SIRO = Int32.Parse(nvc.Get("SIRO"));
                act.BapsType = Int32.Parse(nvc.Get("BapsType"));
                act.PowerType = Int32.Parse(pwt);
                var SplitStatus = nvc.Get("SplitStatus");
                var SplitData = nvc.Get("SplitData").Replace("}],", "}]");
                int SplitBills = 0;

                if (!string.IsNullOrEmpty(SplitStatus))
                {
                    split = JsonConvert.DeserializeObject<List<trxRASplitNewBaps>>(SplitData).ToList();
                    SplitBills = 1;
                }
                    

                data.UpdatedDate = DateTime.Now;

                var strFilePath = "";
                var Result = 0;

                if (FilePO != null)
                {
                    strFilePath = MapModel(FilePO, doc);
                }
                

                if (strFilePath != null)
                {
                    foreach (var item in split)
                    {
                        Result = newBapsService.SplitBillNewBaps(UserManager.User.UserID, item).ID;
                    }
                    Result = newBapsService.ApproveNewBAPSInput(UserManager.User.UserID, data, act, doc, SplitBills).ID;
                }

                return Ok(Result);

            }
            catch (Exception ex)
            {
                return Ok(0);
            }
        }

        
        [HttpPost, Route("UploadBapsBulky")]
        public IHttpActionResult UploadBapsBulky()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                List<int> res = new List<int>();
                var bapsNumber = nvc.Get("BapsNumberBulky");
                var bapsDoneDate = DateTime.Now;
                var bapsDate = DateTime.Parse(nvc.Get("BapsDateBulky"));

                var bapsValidationService = new BapsValidationService();
                var bapsTypeList = bapsValidationService.GetBapsTypeToList();

                var listBapsInput = nvc.Get("vmBapsData").Replace("}],", "}]");
                var listBapsData = JsonConvert.DeserializeObject<List<vmNewBapsData>>(listBapsInput).ToList();
                foreach (vmNewBapsData bapsData in listBapsData)
                {
                    mstBaps data = new mstBaps();
                    trxRAUploadDocument doc = new trxRAUploadDocument();
                    trxRANewBapsActivity act = new trxRANewBapsActivity();

                    data.ID = bapsData.ID != null ? (int)bapsData.ID : 0;
                    data.BAPSNumber = bapsNumber;
                    data.BapsDate = bapsDate;
                    data.BapsDoneDate = bapsDoneDate;
                    act.mstRAActivityID = 20; //bapsDone
                    act.SoNumber = bapsData.SoNumber;
                    act.SIRO = bapsData.SIRO != null ? (int)(bapsData.SIRO) : 0;
                    var bapsTypeName = bapsData.TowerTypeID;
                    if(bapsTypeName != null)
                    {
                        var bapsTypeId = bapsTypeList.FirstOrDefault(m => m.BapsType == bapsTypeName);
                        act.BapsType = (bapsTypeId != null) ? bapsTypeId.mstBapsTypeId : 0;
                    }
                    act.PowerType = bapsData.PowerTypeID != null ? (int)(bapsData.PowerTypeID) : 0;

                    int SplitBills = 0;

                    data.UpdatedDate = DateTime.Now;

                    var strFilePath = "";
                    var Result = 0;


                    HttpPostedFile FilePO = HttpContext.Current.Request.Files["File"];
                    if (FilePO != null)
                    {
                        strFilePath = MapModel(FilePO, doc);
                    }

                    if (strFilePath != null)
                    {
                        Result = newBapsService.ApproveNewBAPSInput(UserManager.User.UserID, data, act, doc, SplitBills).ID;
                        res.Add(Result);
                    }

                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return Ok(0);
            }
        }



        [HttpPost, Route("Reject")]
        public IHttpActionResult Reject(ARSystemService.mstBaps data)
        {
            try
            {
                var Result = 0;

                using (var client = new ARSystemService.NewBapsInputServiceClient())
                {
                    Result = client.RejectNewBAPSInput(UserManager.User.UserToken, data).ID;
                }

                return Ok(Result);

            }
            catch (Exception ex)
            {
                return Ok(0);
            }
        }
        private string MapModel(HttpPostedFile postedFile, ARSystemService.trxRAUploadDocument doc)
        {

            string filePath = "\\RevenueAssurance\\NewBAPS\\";

            if (postedFile != null)
            {

                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);

                doc.ContentType = postedFile.ContentType;
                doc.CreatedDate = DateTime.Now;
                doc.FileName = postedFile.FileName;
                doc.mstRADocumentTypeID = 6;
                doc.FilePath = filePath + "NewBAPS_DOCUMENT" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + filePath);
                filePath = path + "NewBAPS_DOCUMENT" + fileTimeStamp;


                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(filePath);
            }
            return doc.FilePath;
        }
        private string MapModel(HttpPostedFile postedFile, trxRAUploadDocument doc)
        {

            string filePath = "\\RevenueAssurance\\NewBAPS\\";

            if (postedFile != null)
            {

                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);

                doc.ContentType = postedFile.ContentType;
                doc.CreatedDate = DateTime.Now;
                doc.FileName = postedFile.FileName;
                doc.mstRADocumentTypeID = 6;
                doc.FilePath = filePath + "NewBAPS_DOCUMENT" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + filePath);
                filePath = path + "NewBAPS_DOCUMENT" + fileTimeStamp;


                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(filePath);
            }
            return doc.FilePath;
        }
    }


}