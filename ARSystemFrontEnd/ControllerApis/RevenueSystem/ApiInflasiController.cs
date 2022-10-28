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
using ARSystem.Service.RevenueSystem;
using ARSystem.Domain.Models.ViewModels.RevenueSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.ControllerApis.RevenueSystem
{
    [RoutePrefix("api/Inflasi")]
    public class ApiInflasiController : ApiController
    {
        private readonly InflasiService _IS;

        public ApiInflasiController()
        {
            _IS = new InflasiService();
        }
        private void pDisposedService()
        {
            _IS.Dispose();
        }

        [HttpPost, Route("InflationRateGridList")]
        public IHttpActionResult InflationRateGridList(GridInflasiParam post)
        {
            try
            {
                var result = _IS.GetInflationGridList(UserManager.User.UserToken, post).ToList();

                if (post.Year != null)
                {
                    result = result.Where(fl => fl.Year == post.Year).ToList();
                }

                int intTotalRecord = result.Count;

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }
        [HttpPost, Route("SonumbInflasiGridList")]
        public IHttpActionResult SonumbInflasiGridList(GridInflasiParam post)
        {
            try
            {
                var result = _IS.GetSonumbInflasiGridList(UserManager.User.UserToken, post).ToList();

                if (post.Sonumb != null)
                {
                    result = result.Where(fl => fl.Sonumb.ToLower().Contains(post.Sonumb.ToLower())).ToList();
                }
                if (post.SiteID != null)
                {
                    result = result.Where(fl => fl.SiteID.ToLower().Contains(post.SiteID.ToLower())).ToList();
                }
                if (post.SiteName != null)
                {
                    result = result.Where(fl => fl.SiteName.ToLower().Contains(post.SiteName.ToLower())).ToList();
                }
                if (post.SiteIDOpr != null)
                {
                    result = result.Where(fl => fl.OperatorID.ToLower().Contains(post.SiteIDOpr.ToLower())).ToList();
                }
                if (post.SiteNameOpr != null)
                {
                    result = result.Where(fl => fl.SiteNameOpr.ToLower().Contains(post.SiteNameOpr.ToLower())).ToList();
                }
                if (post.Customer != null)
                {
                    result = result.Where(fl => fl.CustomerInvoice.ToLower().Contains(post.Customer.ToLower())).ToList();
                }
                if (post.Company != null)
                {
                    result = result.Where(fl => fl.CompanyInvoice.ToLower().Contains(post.Company.ToLower())).ToList();
                }
                if (post.Regional != null)
                {
                    result = result.Where(fl => fl.RegionalName.ToLower().Contains(post.Regional.ToLower())).ToList();
                }

                int intTotalRecord = result.Count;

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }
        [HttpPost, Route("SonumbGridList")]
        public IHttpActionResult SonumbGridList(GridInflasiParam post)
        {
            try
            {
                var result = _IS.GetSonumbGridList(UserManager.User.UserToken, post).ToList();

                if (post.Sonumb != null)
                {
                    result = result.Where(fl => fl.SoNumber.ToLower().Contains(post.Sonumb.ToLower())).ToList();
                }
                if (post.SiteID != null)
                {
                    result = result.Where(fl => fl.SiteID.ToLower().Contains(post.SiteID.ToLower())).ToList();
                }
                if (post.SiteName != null)
                {
                    result = result.Where(fl => fl.SiteName.ToLower().Contains(post.SiteName.ToLower())).ToList();
                }
                if (post.Customer != null)
                {
                    result = result.Where(fl => fl.OperatorID.ToLower().Contains(post.Customer.ToLower())).ToList();
                }
                if (post.Company != null)
                {
                    result = result.Where(fl => fl.CompanyID.ToLower().Contains(post.Company.ToLower())).ToList();
                }
                if (post.Regional != null)
                {
                    result = result.Where(fl => fl.RegionalName.ToLower().Contains(post.Regional.ToLower())).ToList();
                }

                int intTotalRecord = result.Count;

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }
        [HttpPost, Route("SubmitIR")]
        public IHttpActionResult SubmitIR()
        {
            try
            {
                NameValueCollection va = HttpContext.Current.Request.Form;
                var userCredential = new vmUserCredential();
                var attachment = HttpContext.Current.Request.Files.Get("attachment");
                var post = Newtonsoft.Json.JsonConvert.DeserializeObject<mstRevSysInflationRate>(va.Get("datainput"));
                var result = _IS.SubmitIR(UserManager.User.UserToken, post, userCredential, attachment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }
        [HttpPost, Route("SubmitSI")]
        public IHttpActionResult SubmitSI()
        {
            string errorMessage = "";
            var result = new mstRevSysSonumbInflasi();
            mstRevSysSonumbInflasi dataExl = new mstRevSysSonumbInflasi();
            try
            {
                NameValueCollection va = HttpContext.Current.Request.Form;
                var userCredential = new vmUserCredential();
                var attachment = HttpContext.Current.Request.Files.Get("attachment");
                var post = Newtonsoft.Json.JsonConvert.DeserializeObject<mstRevSysSonumbInflasi>(va.Get("datainput"));
                List<mstRevSysSonumbInflasi> resultExcel = new List<mstRevSysSonumbInflasi>();
                var dataMst = post.SonumbListMst;
                if (attachment != null)
                {
                    var dataList = new List<ArCorrectiveRevenueTemp>();

                    ISheet sheet; //Create the ISheet object to read the sheet cell values  
                    string filename = Path.GetFileName(HttpContext.Current.Server.MapPath(attachment.FileName)); //get the uploaded file name  
                    var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
                    if (fileExt == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(attachment.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(attachment.InputStream); //XSSFWorkBook will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                    }
                    string sheetName = string.Empty;
                    string soNumber = string.Empty;
                    int startIndex = 1;

                    IRow row;
                    ICell cell;

                    DataFormatter formatter = new DataFormatter();
                    string UserId = userCredential.UserID;

                    for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                    {
                        if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                        {
                            row = sheet.GetRow(i);
                            if (row != null && row.GetCell(1) != null)
                            {
                                cell = row.GetCell(1);
                                if (cell != null)
                                {
                                    dataMst = dataMst.Where(x => x.Sonumb.Contains(formatter.FormatCellValue(sheet.GetRow(i).GetCell(0)))).ToList();
                                    if (dataMst.Count > 0)
                                    {
                                        errorMessage += "Sonumb Of Number " + i + " already exist;";
                                        break;
                                    }
                                    else
                                    {
                                        dataExl.Sonumb = formatter.FormatCellValue(sheet.GetRow(i).GetCell(0));
                                        dataExl.AmountRental = Convert.ToDecimal(formatter.FormatCellValue(sheet.GetRow(i).GetCell(1)));
                                        dataExl.AmountService = Convert.ToDecimal(formatter.FormatCellValue(sheet.GetRow(i).GetCell(2)));
                                        dataExl.AmountInflation = Convert.ToDecimal(formatter.FormatCellValue(sheet.GetRow(i).GetCell(3)));
                                        dataExl.InflationRate = Convert.ToDecimal(formatter.FormatCellValue(sheet.GetRow(i).GetCell(4)));
                                        dataExl.CreatedDate = DateTime.Now;
                                        dataExl.CreatedBy = UserId;

                                        resultExcel.Add(new mstRevSysSonumbInflasi()
                                        {
                                            Sonumb = dataExl.Sonumb,
                                            AmountRental = dataExl.AmountRental,
                                            AmountService = dataExl.AmountService,
                                            AmountInflation = dataExl.AmountInflation,
                                            InflationRate = dataExl.InflationRate,
                                            CreatedDate = dataExl.CreatedDate,
                                            CreatedBy = dataExl.CreatedBy
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                if (errorMessage == "")
                {
                    result = _IS.SubmitSI(UserManager.User.UserToken, post, attachment, resultExcel, userCredential);
                }
                else
                {
                    result.ErrorMessage = errorMessage;
                    result.ErrorType = 1;
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (errorMessage != "")
                {
                    dataExl.ErrorMessage = ex.Message;
                    dataExl.ErrorType = 1;
                    return Ok(result);
                }
                else
                {
                    return Ok(ex);
                }
            }
            finally
            {
                this.Dispose();
            }
        }
        [HttpPost, Route("KursGridList")]
        public IHttpActionResult KursGridList(GridInflasiParam post)
        {
            try
            {
                var result = _IS.GetKursGridList(UserManager.User.UserToken, post).ToList();

                if (post.StartDate != null)
                {
                    result = result.Where(fl => fl.StartDate == post.StartDate).ToList();
                }
                if (post.EndDate != null)
                {
                    result = result.Where(fl => fl.EndDate == post.EndDate).ToList();
                }

                int intTotalRecord = result.Count;

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }
        [HttpPost, Route("SubmitKurs")]
        public IHttpActionResult SubmitKurs()
        {
            try
            {
                NameValueCollection va = HttpContext.Current.Request.Form;
                var userCredential = new vmUserCredential();
                var attachment = HttpContext.Current.Request.Files.Get("attachment");
                var post = Newtonsoft.Json.JsonConvert.DeserializeObject<mstRevSysKurs>(va.Get("datainput"));
                var result = _IS.SubmitKurs(UserManager.User.UserToken, post, userCredential, attachment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }

        
        [HttpPost, Route("IsDateRangeOverlaped")]
        public IHttpActionResult IsDateRangeOverlaped(mstRevSysKurs post)
        {
            try
            {
                var isValid = true;
                var list = _IS.GetKursGridList(UserManager.User.UserToken, new GridInflasiParam()
                {
                    StartDate = post.StartDate, EndDate = post.EndDate
                }).ToList();
                if(post.UpdatedStartDate != null && post.UpdatedEndDate != null)
                {
                    isValid = list.Where(m => m.StartDate != post.UpdatedStartDate && m.EndDate != post.UpdatedEndDate).Any(m => 
                    m.EndDate >= post.StartDate && m.StartDate <= post.EndDate);
                }else
                {
                    isValid = list.Any(m => m.EndDate >= post.StartDate && m.StartDate <= post.EndDate);
                }

                return Ok(new { isValid = !isValid });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                this.Dispose();
            }
        }

    }
}