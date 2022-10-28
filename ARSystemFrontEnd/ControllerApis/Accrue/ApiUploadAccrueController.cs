using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.IO;

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


namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/UploadAccrue")]
    public class ApiUploadAccrueController : ApiController
    {

        //NameValueCollection nvc;
        //ISheet sheet;
        UploadAccrueService service = new UploadAccrueService();
        public List<List<trxUploadDataAccrue>> list = new List<List<trxUploadDataAccrue>>();
        UploadAccrueService client = new UploadAccrueService();

        [HttpPost, Route("list")]
        public IHttpActionResult GetUploadAccrueList(PostUploadAccrueView post)
        {
            try
            {
                var data = new List<vwtrxUploadDataAccrue>();
                int intTotalRecord = 0;
                var param = new vwtrxUploadDataAccrue();
                param.SONumber = post.SONumber;
                param.SiteID = post.SiteID;
                param.Remarks = post.Remarks;

                intTotalRecord = client.GetValidateAccrueListCount(param);
                data = client.GetValidateAccrueList(param, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("checkremarks")]
        public IHttpActionResult CheckRemarks(PostUploadAccrueView post)
        {
            try
            {
                var data = new List<vwtrxUploadDataAccrue>();
                var param = new vwtrxUploadDataAccrue();
                data = client.GetValidateAccrueList(param, 0, 0);

                bool flagValid = false;
                foreach (var item in data)
                {
                    if (item.IsValid == true)
                    {
                        flagValid = true;
                        break;
                    }
                }

                return Ok(flagValid);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("listhistory")]
        public IHttpActionResult GetHistoryUploadAccrueList(PostUploadAccrueView post)
        {
            try
            {
                var data = new List<vwtrxUploadDataAccrueStaging>();
                int intTotalRecord = 0;
                var param = new vwtrxUploadDataAccrueStaging();
                param.MonthYear = post.MonthYear;
                param.Week = post.Week;
                param.AccrueStatusID = post.AccrueStatusID;
                param.SONumber = post.SONumber;
                param.SiteID = post.SiteID;

                intTotalRecord = client.GetDataAccrueStagingListCount(param);
                data = client.GetHistoryUploadList(param, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ValidateDataAccrue")]
        public async Task<IHttpActionResult> ValidateDataAccrue()
        {
            var ErrorMsg = "";
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                // nvc = HttpContext.Current.Request.Form;

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
                List<trxUploadDataAccrue> List = new List<trxUploadDataAccrue>();

                trxUploadDataAccrue temp;
                string sheetName = string.Empty;
                int startIndex = 1;
                IRow row;
                ICell cell;

                int DataperBatch = 500;

                int countRow = 0;
                int countNullRow = 0;
                for (int i = 1; i < sheet.LastRowNum + 1; i++)
                {
                    if (sheet.GetRow(i).GetCell(0).ToString() != "")
                    {
                        countNullRow = 0;
                        countRow = countRow + 1;
                    }
                    else
                    {
                        countNullRow = countNullRow + 1;
                    }

                    if (countNullRow == 10)
                        goto nextStep;


                }

                nextStep:
                int DataRowNumber = countRow;
                int counterBacth = 1;


                if (DataRowNumber > DataperBatch)
                {
                    counterBacth = DataRowNumber / DataperBatch;

                    if ((counterBacth * DataperBatch) < DataRowNumber)
                        counterBacth = counterBacth + 1;
                }
                else
                {
                    DataperBatch = DataRowNumber;
                }


                int start = 1;
                int end = DataperBatch;

                for (int i = 1; i <= counterBacth; i++)
                {
                    if (i == counterBacth && DataRowNumber < end)
                        end = DataRowNumber;

                    List.AddRange(await SplitDataExcel(start, end, sheet, nvc));

                    start = start + DataperBatch;
                    end = end + DataperBatch;

                }
                return Json(ErrorMsg);
            }
            catch (Exception ex)
            {
                ErrorMsg = "Upload is failed!";
                return Json(ErrorMsg);
            }
        }

        [HttpGet, Route("CountDataValid")]
        public IHttpActionResult CountDataValid()
        {
            try
            {
                int DataValid = 0;

                DataValid = client.CountDataValid();

                return Ok(DataValid);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CountDataNotValid")]
        public IHttpActionResult CountDataNotValid()
        {
            try
            {
                int DataValid = 0;

                DataValid = client.CountDataNotValid();

                return Ok(DataValid);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CancelUpload")]
        public IHttpActionResult DeleteUpload(PostUploadAccrueView post)
        {
            try
            {
                trxUploadDataAccrue data = new trxUploadDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxUploadDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = client.CancelUpload(userCredential.UserID);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Delete")]
        public IHttpActionResult DeleteDataAccrue(PostUploadAccrueView post)
        {
            try
            {
                trxUploadDataAccrue data = new trxUploadDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxUploadDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = client.DeleteAccrue(userCredential.UserID, Convert.ToInt32(post.ID));
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("DeleteHistory")]
        public IHttpActionResult DeleteDataAccrueHistory(PostUploadAccrueView post)
        {
            try
            {
                trxUploadDataAccrueStaging data = new trxUploadDataAccrueStaging();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxUploadDataAccrueStaging(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = client.DeleteAccrueHistory(userCredential.UserID, post.ListID.ToList());
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("DeleteHistoryByParams")]
        public IHttpActionResult DeleteDataAccrueHistoryByParams(PostUploadAccrueView post)
        {
            try
            {
                var param = new vwtrxUploadDataAccrueStaging();
                param.MonthYear = post.MonthYear;
                param.Week = post.Week;
                param.AccrueStatusID = post.AccrueStatusID;
                param.SONumber = post.SONumber;
                param.SiteID = post.SiteID;

                trxUploadDataAccrueStaging data = new trxUploadDataAccrueStaging();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxUploadDataAccrueStaging(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = client.DeleteAccrueHistoryByParams(userCredential.UserID, param);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SubmitUploadDataAccrue")]
        public IHttpActionResult SubmitUploadDataAccrue(PostUploadAccrueView post)
        {
            try
            {
                trxUploadDataAccrueStaging data = new trxUploadDataAccrueStaging();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxUploadDataAccrueStaging(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.SubmitUploadDataAccrue(userCredential.UserID);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("statusAccrue/list")]
        public IHttpActionResult GetStatusAccrueToList()
        {
            try
            {
                List<mstAccrueStatus> accrueStatusList = new List<mstAccrueStatus>();
                accrueStatusList = client.GetStatusAccrueToList("").ToList();
                return Ok(accrueStatusList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostUploadAccrueView post)
        {
            try
            {
                List<string> ListId = new List<string>();

                var param = new vwtrxUploadDataAccrueStaging();
                param.MonthYear = post.MonthYear;
                param.Week = post.Week;
                param.AccrueStatusID = post.AccrueStatusID;
                param.SONumber = post.SONumber;
                param.SiteID = post.SiteID;

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                ListId = client.GetHistoryUploadListId(userCredential.UserID, param).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private async Task<List<trxUploadDataAccrue>> SplitDataExcel(int startIndex, int startEnd, ISheet sheet, NameValueCollection nvc)
        {
            try
            {
                List<trxUploadDataAccrue> List = new List<trxUploadDataAccrue>();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                string UserID = userCredential.UserID;

                for (int i = startIndex; i <= startEnd; i++)
                {
                    var aa = sheet.GetRow(i).GetCell(0).ToString();

                    if (sheet.GetRow(i) != null && aa != "") //null is when the row only contains empty cells   
                    {
                        #region Get Input Variables
                        var InputSoNumber = sheet.GetRow(i).GetCell(0);
                        var InputSiteID = sheet.GetRow(i).GetCell(1);
                        var InputSiteIDOpr = sheet.GetRow(i).GetCell(2).StringCellValue;
                        var InputSiteName = sheet.GetRow(i).GetCell(3).StringCellValue;
                        var InputSiteNameOpr = sheet.GetRow(i).GetCell(4).StringCellValue;
                        var InputCompanyID = sheet.GetRow(i).GetCell(5).StringCellValue;
                        var InputCompanyInvID = sheet.GetRow(i).GetCell(6).StringCellValue;
                        var InputCustomerID = sheet.GetRow(i).GetCell(7).StringCellValue;

                        var InputRFIDate = sheet.GetRow(i).GetCell(8).ToString();
                        var InputSldDate = sheet.GetRow(i).GetCell(9).ToString();

                        var InputRentalStart = sheet.GetRow(i).GetCell(10).ToString();
                        var InputRentalEndNew = sheet.GetRow(i).GetCell(11).ToString();
                        var InputTenantType = sheet.GetRow(i).GetCell(12).StringCellValue;
                        var InputType = sheet.GetRow(i).GetCell(13).StringCellValue;
                        var InputBaseLeasePrice = sheet.GetRow(i).GetCell(14).NumericCellValue;
                        var InputServicePrice = sheet.GetRow(i).GetCell(15).NumericCellValue;

                        var InputBaseOnMasterListData = sheet.GetRow(i).GetCell(16).ToString();
                        if (InputBaseOnMasterListData == "-" || InputBaseOnMasterListData == "")
                            InputBaseOnMasterListData = "0";
                        else
                            InputBaseOnMasterListData = sheet.GetRow(i).GetCell(16).NumericCellValue.ToString();

                        var InputBaseOnRevenueListingNew = sheet.GetRow(i).GetCell(17).NumericCellValue;

                        var InputStartDateAccrue = sheet.GetRow(i).GetCell(18).ToString();
                        var InputEndDateAccrue = sheet.GetRow(i).GetCell(19).ToString();

                        var InputTotalAccrue = sheet.GetRow(i).GetCell(20).NumericCellValue;
                        #endregion

                        trxUploadDataAccrue trxUploadDataAccrue = new trxUploadDataAccrue();
                        trxUploadDataAccrue.IsValid = true;
                        trxUploadDataAccrue.SONumber = InputSoNumber.ToString();
                        trxUploadDataAccrue.SiteID = InputSiteID.ToString();
                        trxUploadDataAccrue.SiteName = InputSiteName;
                        trxUploadDataAccrue.SiteIDOpr = InputSiteIDOpr;
                        trxUploadDataAccrue.SiteNameOpr = InputSiteNameOpr;
                        trxUploadDataAccrue.CompanyID = InputCompanyID;
                        trxUploadDataAccrue.CompanyInvID = InputCompanyInvID;
                        trxUploadDataAccrue.CustomerID = InputCustomerID;
                        if (InputRFIDate.ToString() == "")
                        {
                            trxUploadDataAccrue.RFIDate = null;

                        }
                        else
                            trxUploadDataAccrue.RFIDate = Convert.ToDateTime(InputRFIDate);

                        if (InputSldDate.ToString() == "")
                            trxUploadDataAccrue.SldDate = null;
                        else
                            trxUploadDataAccrue.SldDate = Convert.ToDateTime(InputSldDate);

                        if (InputRentalStart.ToString() == "")
                            trxUploadDataAccrue.RentalStart = null;
                        else
                            trxUploadDataAccrue.RentalStart = Convert.ToDateTime(InputRentalStart);

                        if (InputRentalEndNew.ToString() == "")
                            trxUploadDataAccrue.RentalEndNew = null;
                        else
                            trxUploadDataAccrue.RentalEndNew = Convert.ToDateTime(InputRentalEndNew);
                        trxUploadDataAccrue.TenantType = InputTenantType;
                        trxUploadDataAccrue.Type = InputType;
                        trxUploadDataAccrue.BaseLeasePrice = Convert.ToDecimal(InputBaseLeasePrice);
                        trxUploadDataAccrue.ServicePrice = Convert.ToDecimal(InputServicePrice);
                        trxUploadDataAccrue.BaseOnMasterListData = Convert.ToDecimal(InputBaseOnMasterListData);

                        trxUploadDataAccrue.BaseOnRevenueListingNew = Convert.ToDecimal(InputBaseOnRevenueListingNew);

                        if (InputStartDateAccrue.ToString() == "")
                        {
                            trxUploadDataAccrue.IsValid = false;
                            trxUploadDataAccrue.StartDateAccrue = null;
                            if (trxUploadDataAccrue.Remarks == null)
                                trxUploadDataAccrue.Remarks += "StartDateAccrue Is Null";
                            else
                                trxUploadDataAccrue.Remarks += ", StartDateAccrue Is Null";
                        }
                        else
                            trxUploadDataAccrue.StartDateAccrue = Convert.ToDateTime(InputStartDateAccrue);

                        if (InputEndDateAccrue.ToString() == "")
                        {
                            trxUploadDataAccrue.IsValid = false;
                            trxUploadDataAccrue.EndDateAccrue = null;
                            if (trxUploadDataAccrue.Remarks == null)
                                trxUploadDataAccrue.Remarks += "EndDateAccrue Is Null";
                            else
                                trxUploadDataAccrue.Remarks += ", EndDateAccrue Is Null";
                        }
                        else
                            trxUploadDataAccrue.EndDateAccrue = Convert.ToDateTime(InputEndDateAccrue);

                        trxUploadDataAccrue.TotalAccrue = Convert.ToDecimal(InputTotalAccrue);
                        trxUploadDataAccrue.MonthYear = DateTime.Parse(nvc.Get("Month"));
                        trxUploadDataAccrue.Year = System.DateTime.Now.Year.ToString();
                        trxUploadDataAccrue.Week = int.Parse(nvc.Get("Week"));
                        trxUploadDataAccrue.CreatedBy = UserID;
                        trxUploadDataAccrue.CreatedDate = System.DateTime.Now;
                        trxUploadDataAccrue.ModifiedBy = UserID;
                        trxUploadDataAccrue.ModifiedDate = System.DateTime.Now;
                        trxUploadDataAccrue.IsActive = true;

                        List.Add(trxUploadDataAccrue);
                    }

                }
                if (List.Count > 0)
                    service.ValidateDataAccrue(List);
                return List;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}