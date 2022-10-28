using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;
using System.Collections.Specialized;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/StopAccrue")]
    public class ApiStopAccrueController : ApiController
    {
        StopAccrueService service = new StopAccrueService();

        #region Transaction


        [HttpPost, Route("SonumbList")]
        public IHttpActionResult GetSonumbList(PostStopAccrueSonumbList post)
        {
            try
            {
                var data = new List<vwStopAccrueSonumbList>();
                int intTotalRecord = 0;
                var param = new vwStopAccrueSonumbList();
                param.RegionID = post.RegionID;
                param.CustomerID = post.CustomerID;
                param.CompanyID = post.CompanyID;
                param.ProductID = post.ProductID;
                param.RFIDone = post.RFIDone;
                param.SiteID = post.SiteID;
                param.SiteName = post.SiteName;
                param.SONumber = post.SONumber;

                intTotalRecord = service.GetCountSonumb(param);
                data = service.GetSonumbList(param, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("logActivity")]
        public IHttpActionResult LogActivity(PostStopAccrue post)
        {
            try
            {
                var data = new List<vwWfHeaderActivityLogs>();
                data = service.GetListActivityLogs(post.HeaderID);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("RequestHeader")]
        public IHttpActionResult GetRequestHeader(PostStopAccrueHeader post)
        {
            try
            {
                var data = new List<vwStopAccrueHeader>();
                int intTotalRecord = 0;
                var param = new vwStopAccrueHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.ActivityID = post.ActivityID;
                param.ActivityOwner = post.ActivityOwner;
                param.ActivityOwnerName = post.ActivityOwnerName;
                param.InitiatorName = post.InitiatorName;
                param.Initiator = post.Initiator;
                param.StartEffectiveDate = post.StartEffectiveDate;//DateTime.Parse(String.Format("{0:dd/MM/yyy}",post.EffectiveDate));
                param.EndEffectiveDate = post.EndEffectiveDate;
                param.CreatedDate = post.CreatedDate;
                param.RequestNumber = post.RequestNumber;
                param.UserRole = post.UserRole;

                intTotalRecord = service.GetCountHeaderRequest(param);
                data = service.GetHeaderRequestList(param, post.start, post.length);
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("RequestHeaderUpdateAmount")]
        public IHttpActionResult GetRequestHeaderUpdateAmount(PostStopAccrueHeader post)
        {
            try
            {
                var data = new List<vwStopAccrueHeader>();
                int intTotalRecord = 0;
                var param = new vwStopAccrueHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.ActivityOwnerName = post.ActivityOwnerName;
                param.InitiatorName = post.InitiatorName;
                param.StartEffectiveDate = post.StartEffectiveDate;
                param.CreatedDate = post.CreatedDate;
                param.RequestNumber = post.RequestNumber;
                param.Initiator = post.Initiator;
                param.UserRole = post.UserRole;

                intTotalRecord = service.GetCountRequestUpdateAmountList(param);
                data = service.GetRequestUpdateAmountList(param, post.start, post.length);
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("RequestDetail")]
        public IHttpActionResult GetRequestDetail(PostStopAccrue post)
        {
            try
            {

                var logAct = new List<vwWfHeaderActivityLogs>();
                logAct = service.GetListActivityLogs(post.AppHeaderID);
                var stopAccrueDetail = new List<vwStopAccrueDetail>();
                stopAccrueDetail = service.GetDetailRequestList(post.HeaderID);

                string htmlElements = "";
                // bool isFeedBack = false;
                for (int i = 0; i < logAct.Count; i++)
                {
                    string feedback = "";

                    //try
                    //{
                    //    if (logAct[i + 1].FollowUp.ToUpper().Contains("FEEDBACK"))
                    //    {
                    //        int no = 1;
                    //        foreach (var item2 in stopAccrueDetail)
                    //        {
                    //            string[] files = item2.FileName.Split(',');
                    //            if (files.Count() > 1)
                    //            {
                    //                feedback += " <a href='/StopAccrue/downloadFile?fileName=" + files[files.Count() - 1] + "'> "+ no+"). "+ item2.SONumber+" (" + files[files.Count() - 1] + ")</a><br />";
                    //                no++;
                    //            }
                    //        }

                    //    }
                    //}
                    //catch (Exception)
                    //{


                    //}
                    //finally
                    //{
                    htmlElements += "<div class='timeline-item'>" +
                                      "<div class='timeline-badge'>" +
                                                      "<i class='timeline-badge-userpic  font-green-dark'><label><b>" + (logAct[i].FollowUp.Replace("HOLD", "").Replace("STOP", "")) + "</b></label></i>" +
                                                  "</div>" +
                                                  "<div class='timeline-body'>" +
                                                      "<div class='timeline-body-arrow'> </div>" +
                                                      "<div class='timeline-body-head'>" +
                                                          "<div class='timeline-body-head-caption'>" +
                                                              "<label class='timeline-body-title font-blue-madison'>" + logAct[i].Activity + "</label>" + "<span class='timeline-body-time font-blue'> (" + logAct[i].name + ")</span>" +
                                                              "<span class='timeline-body-time font-green'>" + String.Format("{0:d/M/yyyy HH:mm:ss}", logAct[i].LogTime) + "</span>" +
                                                          "</div>" +
                                                      "</div>" +
                                                      "<div class='timeline-body-content'>" +
                                                          "<span class='font-grey-dark'>" +
                                                              logAct[i].LogText + (feedback != "" ? "<br/> <b>Files : </b> <br/>" + feedback : "") +
                                                          "</span>" +
                                                      "</div>" +
                                                  "</div>" +
                                              "</div>";
                    //   }


                    //if (logAct[i-1].FollowUp.ToUpper().Contains("FEEDBACK"))
                    //    isFeedBack = true;
                    //else
                    //    isFeedBack = false;

                }




                var data = new StopAccrue();
                data.vwStopAccrueDetail = stopAccrueDetail;
                data.HtmlElements = htmlElements;
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("submitRequest")]
        public IHttpActionResult SubmitRequest(PostStopAccrue post)
        {
            try
            {
                List<trxStopAccrueDetail> StopAccrueDetail = new List<trxStopAccrueDetail>();
                List<trxStopAccrueDetailDraft> StopAccrueDetailDraft = new List<trxStopAccrueDetailDraft>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                foreach (var item in post.StopAccrueRequest)
                {
                    StopAccrueDetail.Add(new trxStopAccrueDetail
                    {
                        ID = item.DetailID,
                        CaseCategoryID = item.CategoryCaseID,
                        CaseDetailID = item.DetailCaseID,
                        SONumber = item.SONumber,
                        TrxStopAccrueHeaderID = post.HeaderID,
                        Remarks = item.Remarks,
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = post.RequestType.ToUpper() == "HOLD" ? post.StartEffectiveDate : item.EffectiveDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = true

                    });
                }
                foreach (var item in post.StopAccrueRequest)
                {
                    StopAccrueDetailDraft.Add(new trxStopAccrueDetailDraft
                    {
                        ID = item.DetailID,
                        CaseCategoryID = item.CategoryCaseID,
                        CaseDetailID = item.DetailCaseID,
                        SONumber = item.SONumber,
                        TrxStopAccrueHeaderID = post.HeaderID,
                        Remarks = item.Remarks,
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = post.RequestType.ToUpper() == "HOLD" ? post.StartEffectiveDate : item.EffectiveDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = true

                    });

                }

                var AccrueHeader = new trxStopAccrueHeader();
                AccrueHeader.ID = post.HeaderID;
                AccrueHeader.RequestTypeID = post.RequestTypeID;
                AccrueHeader.StartEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.StartEffectiveDate;
                AccrueHeader.EndEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.EnndEffectiveDate;
                AccrueHeader.Initiator = userCredential.UserID;
                AccrueHeader.CreatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedBy = userCredential.UserID;
                AccrueHeader.Remarks = post.Remarks;
                AccrueHeader.RequestType = post.RequestType;
                AccrueHeader.IsReHold = false;


                trxStopAccrueHeader result = service.SubmitRequest(StopAccrueDetail, AccrueHeader, "NEW", StopAccrueDetailDraft);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("clearDraft")]
        public IHttpActionResult ClearDraft(PostStopAccrue post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
             
                var AccrueHeader = new trxStopAccrueHeader();
                AccrueHeader.ID = post.HeaderID;
                AccrueHeader.RequestTypeID = post.RequestTypeID;
                AccrueHeader.StartEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.StartEffectiveDate;
                AccrueHeader.EndEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.EnndEffectiveDate;
                AccrueHeader.Initiator = userCredential.UserID;
                AccrueHeader.CreatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedBy = userCredential.UserID;
                AccrueHeader.Remarks = post.Remarks;
                AccrueHeader.RequestType = post.RequestType;
                AccrueHeader.IsReHold = false;
                trxStopAccrueHeader result = service.ClearDraft( AccrueHeader);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            
        }

        [HttpPost, Route("submitEdit")]
        public IHttpActionResult SubmitEdit(PostStopAccrue post)
        {
            try
            {
                List<trxStopAccrueDetail> StopAccrueDetail = new List<trxStopAccrueDetail>();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                
                foreach (var item in post.StopAccrueRequest)
                {
                    StopAccrueDetail.Add(new trxStopAccrueDetail
                    {
                        ID = item.DetailID,
                        CaseCategoryID = item.CategoryCaseID,
                        CaseDetailID = item.DetailCaseID,
                        SONumber = item.SONumber,
                        TrxStopAccrueHeaderID = post.HeaderID,
                        Remarks = item.Remarks,
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = post.RequestType.ToUpper() == "HOLD" ? post.StartEffectiveDate : item.EffectiveDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = true,
                        RequestNumber = item.RequestNumber
                    });
                }
                var AccrueHeader = new trxStopAccrueHeader();
                AccrueHeader.ID = post.HeaderID;
                AccrueHeader.RequestTypeID = post.RequestTypeID;
                AccrueHeader.StartEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.StartEffectiveDate;
                AccrueHeader.EndEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.EnndEffectiveDate;
                AccrueHeader.Initiator = userCredential.UserID;
                AccrueHeader.CreatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedBy = userCredential.UserID;
                AccrueHeader.Remarks = post.Remarks;
                AccrueHeader.RequestType = post.RequestType;
                AccrueHeader.RequestNumber = post.RequestNumber;
                AccrueHeader.IsReHold = false;


                trxStopAccrueHeader result = service.SubmitEdit(StopAccrueDetail, AccrueHeader, "NEW");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("submitReHoldRequest")]
        public IHttpActionResult SubmitReHoldRequest(PostReHoldAccrueRequest post)
        {
            try
            {
                List<trxStopAccrueDetail> StopAccrueDetail = new List<trxStopAccrueDetail>();
                List<trxStopAccrueDetailDraft> StopAccrueDetailDraft = new List<trxStopAccrueDetailDraft>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);


                foreach (var item in post.stopAccrueDetail)
                {
                    var data = post.reHoldAccrue.Where(x => x.SONumber == item.SONumber).FirstOrDefault();

                    string remarks = item.Remarks;
                    DateTime? effecDate = item.EffectiveDate;
                    if (data != null)
                    {
                        item.IsHold = false;
                        remarks = data.Remarks;
                        effecDate = post.StartEffectiveDate;
                    }

                    StopAccrueDetail.Add(new trxStopAccrueDetail
                    {
                        CaseCategoryID = item.CaseCategoryID,
                        CaseDetailID = item.CaseDetailID,
                        SONumber = item.SONumber,
                        Remarks = remarks,
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = effecDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = item.IsHold
                    });
                    StopAccrueDetailDraft.Add(new trxStopAccrueDetailDraft
                    {
                        CaseCategoryID = item.CaseCategoryID,
                        CaseDetailID = item.CaseDetailID,
                        SONumber = item.SONumber,
                        Remarks = remarks,
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = effecDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = item.IsHold
                    });

                }

                string[] requestNumber = post.RequestNumber.Split('/');
                int number = 1;
                //if (Int32.TryParse(requestNumber[1], out number))
                //{
                //    number = Int32.Parse(requestNumber[1]) + 1;
                //}
                string code = "";
                if (requestNumber[1].Contains("-"))
                {
                    string[] num = requestNumber[1].Split('-');
                    number = int.Parse(num[1]) + 1;
                    code = num[0];
                }
                else
                {
                    code = requestNumber[1];
                }

                string reqNumber = "";
                for (int i = 0; i < requestNumber.Count(); i++)
                {

                    if (i == 1)
                        reqNumber += code + "-" + (number) + "/";
                    else if (i == requestNumber.Count() - 1)
                        reqNumber += requestNumber[i];
                    else
                        reqNumber += requestNumber[i] + "/";
                }
                var AccrueHeader = new trxStopAccrueHeader();
                AccrueHeader.ID = 0;
                AccrueHeader.RequestTypeID = post.RequestTypeID;
                AccrueHeader.StartEffectiveDate = post.StartEffectiveDate;
                AccrueHeader.EndEffectiveDate = post.EndEffectiveDate;
                AccrueHeader.Initiator = userCredential.UserID;
                AccrueHeader.CreatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedBy = userCredential.UserID;
                AccrueHeader.Remarks = post.Remarks;
                AccrueHeader.RequestType = post.RequestType;
                AccrueHeader.PrevAppHeaderID = post.PrevAppHeaderID;
                AccrueHeader.RequestNumber = reqNumber;
                AccrueHeader.IsReHold = true;

                //trxStopAccrueHeader result = new trxStopAccrueHeader();
                trxStopAccrueHeader result = service.SubmitRequest(StopAccrueDetail, AccrueHeader, "REHOLD", StopAccrueDetailDraft);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("updateRequest")]
        public IHttpActionResult UpdateRequest(PostStopAccrue post)
        {
            try
            {

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var AccrueHeader = new trxStopAccrueHeader();
                AccrueHeader.AppHeaderID = post.AppHeaderID;
                AccrueHeader.ID = post.HeaderID;
                AccrueHeader.UpdatedDate = System.DateTime.Now;
                AccrueHeader.Remarks = post.Remarks;
                AccrueHeader.NextFlag = post.NextFlag;
                AccrueHeader.UpdatedBy = userCredential.UserID;
                AccrueHeader.ActivityOwner = userCredential.UserID;
                AccrueHeader.RequestType = post.RequestType;
                trxStopAccrueHeader result = service.UpdateRequest(AccrueHeader);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("receiveDoc")]
        public IHttpActionResult ReceiveDoc(List<PostStopAccrue> post)
        {
            try
            {
                string result = "";
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                foreach (var item in post)
                {
                    var AccrueHeader = new trxStopAccrueHeader();
                    AccrueHeader.AppHeaderID = item.AppHeaderID;
                    AccrueHeader.ID = item.HeaderID;
                    AccrueHeader.UpdatedDate = System.DateTime.Now;
                    AccrueHeader.Remarks = "Document receive";
                    AccrueHeader.NextFlag = item.NextFlag;
                    AccrueHeader.UpdatedBy = userCredential.UserID;
                    AccrueHeader.ActivityOwner = userCredential.UserID;

                    trxStopAccrueHeader data = service.UpdateRequest(AccrueHeader);
                }
                result = "";
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("reviseDetail")]
        public IHttpActionResult RevisiDetail(PostStopAccrue post)
        {
            try
            {
                var data = new trxStopAccrueDetailFile();
                data.ID = 0;
                data.FileName = post.StopAccrueRequestDetail.FileName;
                data.trxStopAccrueDetailID = post.StopAccrueRequestDetail.DetailID;
                data.Remarks = post.StopAccrueRequestDetail.Remarks;
                data.CreatedDate = System.DateTime.Now;
                data.FilePath = "StopAccrue";
                data = service.DetailFileRequestSave(data);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }


        }

        [HttpPost, Route("updateAmountCapexRevenue")]
        public IHttpActionResult UpdateAmountCapexRevenue()
        {

            var ErrorMsg = "";
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                // HttpPostedFileBase files = Request.Files[0]; //Read the first Posted Excel File  
                HttpPostedFile files = HttpContext.Current.Request.Files["File"];
                ISheet sheet;
                //Create the ISheet object to read the sheet cell values  
                //string filename = Path.GetFileName(Server.MapPath(files.FileName)); //get the uploaded file name  
                var fileExt = Path.GetExtension(files.FileName); //get the extension of uploaded excel file  

                //var fileExt = files.FileName;

                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(files.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(files.InputStream); //XSSFWorkBook will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                }
                List<trxStopAccrueDetail> data = new List<trxStopAccrueDetail>();
                trxStopAccrueDetail temp;
                string sheetName = string.Empty;
                string soNumber = string.Empty;
                int startIndex = 1;
                IRow row;
                ICell cell;

                string strID = "";
                strID = nvc.Get("HeaderID");

                for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                {
                    if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                    {
                        temp = new trxStopAccrueDetail();
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            cell = row.GetCell(0);
                            if (cell != null)
                            {
                                temp.SONumber = sheet.GetRow(i).GetCell(0).StringCellValue;
                                if (temp.SONumber.TrimStart().TrimEnd() == "" || temp.SONumber == null)
                                {
                                    ErrorMsg = "Number " + sheet.GetRow(i).GetCell(0).StringCellValue + " SO Number is empty!";
                                    return Json(ErrorMsg);
                                }
                            }
                            else
                            {
                                ErrorMsg = "Number " + sheet.GetRow(i).GetCell(0).StringCellValue + " SO Number is empty!";
                                return Json(ErrorMsg);
                            }

                            cell = row.GetCell(1);
                            if (cell != null)
                            {
                                var capexAmount = sheet.GetRow(i).GetCell(1).NumericCellValue;
                                if (capexAmount.ToString() == "" || capexAmount == null)
                                {
                                    ErrorMsg = "Number " + sheet.GetRow(i).GetCell(1).StringCellValue + " CapexAmount is empty!";
                                    return Json(ErrorMsg);
                                }
                                else if (!checkNumeric(capexAmount.ToString()))
                                {
                                    ErrorMsg = "Number " + sheet.GetRow(i).GetCell(1).StringCellValue + " CapexAmount is not numeric!";
                                    return Json(ErrorMsg);
                                }
                                temp.CapexAmount = Convert.ToDecimal(capexAmount);
                            }
                            else
                            {
                                ErrorMsg = "Number " + sheet.GetRow(i).GetCell(1).StringCellValue + " CapexAmount is empty!";
                                return Json(ErrorMsg);
                            }

                            cell = row.GetCell(2);
                            if (cell != null)
                            {
                                var revenueAmount = sheet.GetRow(i).GetCell(2).NumericCellValue;
                                if (revenueAmount.ToString() == "" || revenueAmount == null)
                                {
                                    ErrorMsg = "Number " + sheet.GetRow(i).GetCell(2).StringCellValue + " RevenueAmount is empty!";
                                    return Json(ErrorMsg);
                                }
                                else if (!checkNumeric(revenueAmount.ToString()))
                                {
                                    ErrorMsg = "Number " + sheet.GetRow(i).GetCell(2).StringCellValue + " RevenueAmount is not numeric!";
                                    return Json(ErrorMsg);
                                }
                                temp.RevenueAmount = Convert.ToDecimal(revenueAmount);
                            }
                            else
                            {
                                ErrorMsg = "Number " + sheet.GetRow(i).GetCell(2).StringCellValue + " RevenueAmount is empty!";
                                return Json(ErrorMsg);
                            }
                            temp.TrxStopAccrueHeaderID = int.Parse(strID);
                            if (!string.IsNullOrEmpty(temp.SONumber) && temp.CapexAmount != null && temp.RevenueAmount != null)
                                data.Add(temp);
                        }
                        else
                        {
                            ErrorMsg = "Number " + sheet.GetRow(i).GetCell(1).StringCellValue + " CapexAmount is empty!";
                            return Json(ErrorMsg);
                        }


                    }
                    else
                    {
                        ErrorMsg = "Data is empty!";
                        return Json(ErrorMsg);
                    }
                }
                var service = new StopAccrueService();
                temp = service.UpdateAmountCapexRevenue(data);
                ErrorMsg = temp.ErrorMessage == null ? "" : temp.ErrorMessage;
                return Json(ErrorMsg);
            }
            catch (Exception ex)
            {
                ErrorMsg = "Upload is failed!";
                return Json(ErrorMsg);
            }
        }

        [Authorize]
        [HttpPost, Route("UploadDocRequest")]
        public IHttpActionResult UploadDocRequest()
        {
            string actionTokenView = "B5C847DA-234E-415C-BC52-0523AEE0CC73";

            var ErrorMsg = "";
            try
            {


                NameValueCollection nvc = HttpContext.Current.Request.Form;

                // HttpPostedFileBase files = Request.Files[0]; //Read the first Posted Excel File  
                HttpPostedFile files = HttpContext.Current.Request.Files["File"];

                FileInfo fi = new FileInfo(files.FileName);
                if (fi.Extension != ".pdf" && fi.Extension != ".zip" && fi.Extension != ".rar")
                {
                    ErrorMsg = "Please upload an PDF or ZIP or RAR File.!";
                }
                else if ((files.ContentLength / 1048576.0) > 2)
                {
                    ErrorMsg = "Upload File Can`t bigger then 2048 bytes (2mb)..! ";
                }
                else
                {

                    string _FileName = Path.GetFileName(files.FileName);
                    string _path = System.Web.Hosting.HostingEnvironment.MapPath(Helper.Helper.GetDocPath() + "StopAccrue/RequestHeader");//@"D:\Project\GIT\ARSystem_StopAccrue\ARSystemFrontEnd" + Helper.Helper.GetDocPath() + "StopAccrue";
                    if (!Directory.Exists(_path))
                    {
                        Directory.CreateDirectory(_path);
                    }
                    files.SaveAs(Path.Combine(_path, _FileName));


                    ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    var AccrueHeader = new trxStopAccrueHeader();

                    AccrueHeader.AppHeaderID = int.Parse(nvc.Get("AppHeaderID"));
                    AccrueHeader.ID = int.Parse(nvc.Get("HeaderID"));
                    AccrueHeader.UpdatedDate = System.DateTime.Now;
                    AccrueHeader.Remarks = "Submit document";
                    AccrueHeader.NextFlag = "Submit";
                    AccrueHeader.UpdatedBy = userCredential.UserID;
                    AccrueHeader.ActivityOwner = userCredential.UserID;
                    AccrueHeader.FileName = files.FileName;
                    trxStopAccrueHeader result = service.UpdateRequest(AccrueHeader);
                    ErrorMsg = "";
                }

            }
            catch (Exception ex)
            {

                ErrorMsg = ex.Message;
            }
            return Json(ErrorMsg);
        }

        private bool checkNumeric(string value)
        {
            bool result;
            decimal n;
            result = decimal.TryParse(value, out n);
            return result;
        }

        [HttpPost, Route("submitRequestDraft")]
        public IHttpActionResult SubmitRequestDraft(PostStopAccrue post)
        {
            try
            {
                List<trxStopAccrueDetail> StopAccrueDetail = new List<trxStopAccrueDetail>();
                List<trxStopAccrueDetailDraft> StopAccrueDetailDraft = new List<trxStopAccrueDetailDraft>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                foreach (var item in post.StopAccrueRequest)
                {

                    StopAccrueDetail.Add(new trxStopAccrueDetail
                    {
                        ID = item.DetailID,
                        CaseCategoryID = item.CategoryCaseID,
                        CaseDetailID = item.DetailCaseID,
                        SONumber = item.SONumber,
                        TrxStopAccrueHeaderID = post.HeaderID,
                        Remarks = "DRAFT",
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = post.RequestType.ToUpper() == "HOLD" ? post.StartEffectiveDate : item.EffectiveDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = true
                    });

                    StopAccrueDetailDraft.Add(new trxStopAccrueDetailDraft
                    {
                        ID = item.DetailID,
                        CaseCategoryID = item.CategoryCaseID,
                        CaseDetailID = item.DetailCaseID,
                        SONumber = item.SONumber,
                        TrxStopAccrueHeaderID = post.HeaderID,
                        Remarks = "DRAFT",
                        FileName = item.FileName,
                        FilePath = "StopAccrue",
                        EffectiveDate = post.RequestType.ToUpper() == "HOLD" ? post.StartEffectiveDate : item.EffectiveDate,
                        CreatedDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now,
                        CapexAmount = null,
                        RevenueAmount = null,
                        IsHold = true
                    });
                }
                var AccrueHeader = new trxStopAccrueHeader();
                AccrueHeader.ID = post.HeaderID;
                AccrueHeader.RequestTypeID = post.RequestTypeID;
                AccrueHeader.StartEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.StartEffectiveDate;
                AccrueHeader.EndEffectiveDate = post.RequestType.ToUpper() == "STOP" ? null : post.EnndEffectiveDate;
                AccrueHeader.Initiator = userCredential.UserID;
                AccrueHeader.CreatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedDate = System.DateTime.Now;
                AccrueHeader.UpdatedBy = userCredential.UserID;
                AccrueHeader.Remarks = "DRAFT";
                AccrueHeader.RequestType = post.RequestType;
                AccrueHeader.IsReHold = false;


                trxStopAccrueHeader result = service.SubmitRequest(StopAccrueDetail, AccrueHeader, "NEW",StopAccrueDetailDraft);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
        #endregion

        #region Headers
        [HttpPost, Route("dashboardHeader")]
        public IHttpActionResult DashboardHeader(PostStopAccrueHeader post)
        {
            try
            {
                int totalRecords = 0;
                var data = new List<vwStopAccrueDashboardHeader>();
                var param = new vwStopAccrueDashboardHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.RequestNumber = post.RequestNumber;
                param.CraetedDate = post.CreatedDate;
                param.CraetedDate2 = post.CreatedDate2;
                param.DepartName = post.DepartName;
                param.Activity = post.Activity;

                totalRecords = service.GetCountDashboardHeader(param);
                data = service.GetDashboardHeaderList(param, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("dashboardDetail")]
        public IHttpActionResult DashboardDetail(PostStopAccrueHeader post)
        {
            try
            {
                int totalRecords = 0;
                var data = new List<vwStopAccrueDashboardDetail>();
                var param = new vwStopAccrueDashboardDetail();
                param.TrxStopAccrueHeaderID = post.HeaderID;

                totalRecords = service.GetCountDashboardDetail(param);
                data = service.GetDashboardDetailList(param, post.start, post.length);
                return Ok(data);
                //return Ok(new { draw = post.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
        #endregion

        #region Master Data

        [HttpGet, Route("detailCase")]
        public IHttpActionResult StopAccrueDetailCase(int ID)
        {
            try
            {
                var data = new List<mstStopAccrueDetailCase>();
                data = service.StopAccrueDetailCase(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpGet, Route("categoryCase")]
        public IHttpActionResult StopAccueCategoryCase()
        {
            try
            {
                var data = new List<mstStopAccrueCategoryCase>();
                data = service.StopAccrueCategoryCase();
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        //[HttpGet, Route("nextFlag")]
        //public IHttpActionResult NextFlag(int ID)
        //{
        //    try
        //    {
        //        var data = new List<WfPrDef_NextFlag>();
        //        data = service.NextFlag(ID);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(ex);
        //    }
        //}

        [HttpGet, Route("accrueType")]
        public IHttpActionResult StopAccrueType()
        {
            try
            {
                var data = new List<mstStopAccrueType>();
                data = service.StopAccrueType();
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpGet, Route("accrueActivity")]
        public IHttpActionResult StopAccrueActivity()
        {
            try
            {
                var data = new List<vwWfPrDefActivity>();
                var param = new vwWfPrDefActivity();
                param.Code = "STOP_ACCRUE";
                var wfService = new WorkFlowService();
                data = wfService.GetActivity(param);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpGet, Route("nextFlag")]
        public IHttpActionResult StopAccrueActivity(string RoleLabel, string ActivityLabel)
        {
            try
            {
                var data = new List<mstStopAccrueApprovalStatus>();
                var param = new mstStopAccrueApprovalStatus();
                param.RoleLabel = RoleLabel;
                param.ActivityLabel = ActivityLabel;
                data = service.ApprovalStatus(param);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
        [HttpGet, Route("CheckDraft")]
        public IHttpActionResult CheckDraft(string initiator)
        {
            try
            {
                var data = service.GetDraftList(initiator);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("EditHeader")]
        public IHttpActionResult EditHeader(string RequestNumber)
        {
            try
            {
                var data = service.GetSoNumberList(RequestNumber);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion


    }
}