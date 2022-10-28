using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Http;
using ARSystem.Domain.Models;
using ARSystem.Service;
using System.Linq;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Models;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/CorrectiveRevenue")]
    public class ApiUploadCorrectiveDataController : ApiController
    {
        // GET: ApiUploadCorrectiveData
        [HttpGet, Route("GetPeriod")]
        public IHttpActionResult GetPeriod()
        {
            var result = new ArCorrectiveRevenuePeriod();

            try
            {

                var service = new CorrectiveRevenueService();
                result = service.GetPeriod();

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorType = 1;

            }

            return Ok(result);
        }

        [HttpPost, Route("UploadData")]
        public IHttpActionResult UploadData()
        {
            var dataList = new List<ArCorrectiveRevenueTemp>();
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["File"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                byte Month = 0;
                short Year = 0;

                Month = Convert.ToByte(nvc.Get("Month").ToString());
                Year = Convert.ToInt16(nvc.Get("Year").ToString());

                dataList = ExtractData(postedFile, Year, Month);


                var duplicateCount = dataList.GroupBy(x => new { x.SoNumber, x.TypeAdjusment })
                   .Select(g => new { g.Key.SoNumber, g.Key.TypeAdjusment, dataCount = g.Count() }).ToList();

                if (duplicateCount.Where(x => x.dataCount > 1).Count() > 1)
                {
                    duplicateCount = duplicateCount.Where(x => x.dataCount > 1).ToList();
                    var warningLog = duplicateCount.GroupBy(x => new { x.SoNumber, x.TypeAdjusment }).Select(g => new { g.Key.SoNumber, g.Key.TypeAdjusment }).ToList();
                    dataList = new List<ArCorrectiveRevenueTemp>();
                    dataList = (from c in warningLog
                                select new ArCorrectiveRevenueTemp
                                {
                                    ErrorMessage = "Duplicate data on So Number = " + c.SoNumber+" and Type Adjusment = "+ c.TypeAdjusment,
                                    ErrorType = 1
                                }).ToList();
                }
                else
                {
                    if (dataList.Where(x => x.ErrorType == 1).Count() == 0)
                    {
                        var service = new CorrectiveRevenueService();
                        var result = new ArCorrectiveRevenueTemp();

                        result = service.GenerateData(dataList);
                        dataList = new List<ArCorrectiveRevenueTemp>();
                        dataList.Add(result);
                    }
                    else
                    {
                        dataList = dataList.Where(x => x.ErrorType == 1).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                dataList.Add(new ArCorrectiveRevenueTemp { ErrorMessage = ex.Message, ErrorType = 1 });
            }

            return Ok(dataList);
        }

        private List<ArCorrectiveRevenueTemp> ExtractData(HttpPostedFile postedFile, short Year, byte Month)
        {
            var dataList = new List<ArCorrectiveRevenueTemp>();

            ISheet sheet; //Create the ISheet object to read the sheet cell values  
            string filename = Path.GetFileName(HttpContext.Current.Server.MapPath(postedFile.FileName)); //get the uploaded file name  
            var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
            if (fileExt == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(postedFile.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(postedFile.InputStream); //XSSFWorkBook will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
            }
            string sheetName = string.Empty;
            string soNumber = string.Empty;
            int startIndex = 1;

            IRow row;
            ICell cell;

            DateTime currentDate = new DateTime();
            currentDate = DateTime.Now;
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            string UserId = userCredential.UserID;

            for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
            {
                var data = new ArCorrectiveRevenueTemp();
                if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                {
                    row = sheet.GetRow(i);
                    if (row != null && row.GetCell(1) != null)
                    {
                        data.RowIndex = i;
                        data.MonthPeriod = Month;
                        data.YearPeriod = Year;
                        data.CeatedDate = currentDate;
                        data.CreatedBy = UserId;

                        try
                        {

                            string errorMessage = "";
                            cell = row.GetCell(1);
                            if (cell != null)
                            {
                                var sonumb = sheet.GetRow(i).GetCell(1).StringCellValue;
                                data.SoNumber = sonumb == null ? "" : sonumb.TrimStart().TrimEnd();
                                if (data.SoNumber.TrimStart().TrimEnd() == "" || data.SoNumber == null)
                                {
                                    errorMessage = "SO Number Of Number" + i + " is empty;";
                                }
                            }
                            else
                            {
                                errorMessage = "SO Number Of Number" + i + " is empty;";
                            }
                            cell = row.GetCell(2);
                            if (cell != null)
                            {
                                var remarks = sheet.GetRow(i).GetCell(2).StringCellValue;
                                data.TypeAdjusment = remarks == null ? "" : remarks;//== null ?"" : sheet.GetRow(i).GetCell(2).StringCellValue.TrimStart().TrimEnd();
                                if (data.TypeAdjusment.TrimStart().TrimEnd() == "")
                                {
                                    errorMessage += "Remark Adjusment Of  Number " + i + " is empty;";
                                }
                            }
                            else
                            {
                                errorMessage = "Remark Adjusment Of  Number " + i + " is empty;";
                            }

                            cell = row.GetCell(3);
                            if (cell != null)
                            {
                                data.TotalAdjusment = Convert.ToDecimal(sheet.GetRow(i).GetCell(3).NumericCellValue);
                            }

                            data.ErrorMessage = errorMessage;
                            data.ErrorType = (errorMessage != "" ? 1 : 0);

                        }
                        catch (Exception ex)
                        {
                            data.ErrorMessage = ex.Message;
                            data.ErrorType = 1;
                        }
                        dataList.Add(data);
                    }
                }
            }
            return dataList;
        }

        [HttpGet, Route("ProcessData")]
        public IHttpActionResult ProcessData()
        {
            var result = new ArCorrectiveRevenueTemp();
            try
            {
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var service = new CorrectiveRevenueService();
                result = service.ProcessData(userCredential.UserID);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorType = 1;
            }

            return Ok(result);
        }

        [HttpGet, Route("GetDataGenerated")]
        public IHttpActionResult DataGenerated()
        {
            var result = new List<ArCorrectiveRevenueFinalTemp>();

            try
            {
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var service = new CorrectiveRevenueService();
                result = service.GetDataGenerated(userCredential.UserID);

            }
            catch (Exception ex)
            {
                result.Add(new ArCorrectiveRevenueFinalTemp { ErrorMessage = ex.Message, ErrorType = 1 });

            }

            return Ok(new { data = result });
        }


        //[HttpGet, Route("GetListId")]
        //public IHttpActionResult GetListId()
        //{
        //    var result = new List<vwArCorrectiveRevenue>();

        //    try
        //    {
        //        vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
        //        var service = new CorrectiveRevenueService();
        //        result = service.GetDataGenerated(userCredential.UserID, 0, 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Add(new vwArCorrectiveRevenue { ErrorMessage = ex.Message, ErrorType = 1 });

        //    }

        //    return Ok(result);
        //}

        [HttpPost, Route("Delete")]
        public IHttpActionResult Delete(PostData param)
        {
            var result = new ArCorrectiveRevenueTemp();
            try
            {
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var service = new CorrectiveRevenueService();

                var paramList = new List<ArCorrectiveRevenueFinalTemp>();
                if (param.AllDelete)
                {
                    result = service.Delete(paramList, param.AllDelete, userCredential.UserID);
                }
                else
                {
                    if (param.RowID.Count > 0)
                    {
                        for (int i = 0; i < param.RowID.Count; i++)
                        {
                            paramList.Add(new ArCorrectiveRevenueFinalTemp { Id = param.RowID[i].Id });
                        }
                        result = service.Delete(paramList, param.AllDelete, userCredential.UserID);
                    }
                }

            }
            catch (Exception ex)
            {

                result.ErrorMessage = ex.Message;
                result.ErrorType = 1;
            }

            return Ok(result);
        }
    }


    public class RowId
    {
        public int Id { get; set; }
    }

    public class PostData
    {
        public bool AllDelete { get; set; }
        public List<RowId> RowID;
    }


}