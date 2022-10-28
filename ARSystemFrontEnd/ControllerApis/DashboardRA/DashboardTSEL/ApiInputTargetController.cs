using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ClosedXML.Excel;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;
using ARSystem.Service.RASystem.DashboardRA.DashboardTSEL;
using static ARSystem.Service.Constants;
using System.IO;
using OfficeOpenXml;
using System.Collections.Specialized;
using System.Globalization;

namespace ARSystemFrontEnd.Controllers
{

    [RoutePrefix("api/ApiInputTarget")]
    public class ApiInputTargetController : ApiController
    {
        private readonly InputTargetService _inputTargetService;
        private readonly DashboardTSELService _dashboardTSELService;
        private readonly MasterService _masterService;

        private const string CacheKeySection = "MstRASection";
        private const string CacheKeySOW = "MstRASOW";

        private const string ValidationUploadInvalidData = "<li>Invalid Data at row {0}, column {1}, message : value of {2} {3}.</li>";
        private const string ValidationUploadEmptyData = "<li>Invalid Data at row {0}, column {1}, message : Cells cannot be empty.</li>";

        public ApiInputTargetController()
        {
            _inputTargetService = new InputTargetService();
            _dashboardTSELService = new DashboardTSELService();
            _masterService = new MasterService();
        }

        [HttpPost, Route("NonTselGrid")]
        // GET: ApiInputTarget
        public IHttpActionResult GetDashboardNonTSELDataToGrid(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                //non TSEL filter managed in UI Layer (javascript)
                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;

                param.DepartmentType = RADepartmentTypeEnum.NonTSEL;

                int intTotalRecord = 0;

                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();

                intTotalRecord = _inputTargetService.GetTrxTargetNonTSELDataCount(param);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                if (intTotalRecord == 0)
                {
                    dashboardtseldata = _inputTargetService.GetTrxDashbordNonTSELDataToList(param, post.start, 0).ToList();
                }
                else
                    dashboardtseldata = _inputTargetService.GetTrxDashbordNonTSELDataToList(param, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dashboardtseldata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListNonTselData")]
        public IHttpActionResult GetListNonTselData(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;

                param.DepartmentType = RADepartmentTypeEnum.NonTSEL;


                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                dashboardtseldata = _inputTargetService.GetTrxDashbordNonTSELDataToList(param, post.start, 0).ToList();
                return Ok(dashboardtseldata);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListNonTselId")]
        public IHttpActionResult GetListNonTselId(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                //non TSEL filter managed in UI Layer (javascript)
                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;

                param.DepartmentType = RADepartmentTypeEnum.NonTSEL;
                var ListId = _inputTargetService.GetListNonTSELId(post.vwRABapsSite).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("NewBapsGrid")]
        // GET: ApiInputTarget
        public IHttpActionResult GetDashboardNewBapsDataToGrid(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;
                //non TSEL filter managed in UI Layer (javascript)
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;
                param.StipSiro = post.schStipSiro;
                int intTotalRecord = 0;

                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();

                intTotalRecord = _inputTargetService.GetTrxTargetNewBapsDataCount(param);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                if (intTotalRecord == 0)
                {
                    dashboardtseldata = _inputTargetService.GetTrxTargetNewBapsToList(param, post.start, 0).ToList();
                }
                else
                    dashboardtseldata = _inputTargetService.GetTrxTargetNewBapsToList(param, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dashboardtseldata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListNewBapsId")]
        public IHttpActionResult GetListNewBapsId(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;
                //non TSEL filter managed in UI Layer (javascript)
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;
                var ListId = _inputTargetService.GetListNewBapsId(post.vwRABapsSite).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListNewBapsData")]
        public IHttpActionResult GetListNewBapsData(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;



                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                dashboardtseldata = _inputTargetService.GetTrxTargetNewBapsToList(param, post.start, 0).ToList();
                return Ok(dashboardtseldata);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }


        [HttpPost, Route("NewProductGrid")]
        // GET: ApiInputTarget
        public IHttpActionResult GetDashboardNewProductDataToGrid(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;

                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;

                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;
                int intTotalRecord = 0;

                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();

                intTotalRecord = _inputTargetService.GetTrxTargetNewProductDataCount(param);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                if (intTotalRecord == 0)
                {
                    dashboardtseldata = _inputTargetService.GetTrxDashbordNewProductDataToList(param, post.start, 0).ToList();
                }
                else
                    dashboardtseldata = _inputTargetService.GetTrxDashbordNewProductDataToList(param, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dashboardtseldata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListNewProductId")]
        public IHttpActionResult GetListNewProductId(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;

                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;

                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;
                var ListId = _inputTargetService.GetListNewProductId(post.vwRABapsSite).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListNewProductData")]
        public IHttpActionResult GetListNewProductData(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;

                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.MonthBill = post.strBillingMonth;

                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;

                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();


                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                dashboardtseldata = _inputTargetService.GetTrxDashbordNewProductDataToList(param, post.start, 0).ToList();
                return Ok(dashboardtseldata);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("UploadExcelTargetRecurring")]
        public IHttpActionResult UploadExcelTargetRecurring()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile postedFile = HttpContext.Current.Request.Files["FileUpload"];
                string departmentCode = nvc.Get("DepartmentCode");
                //string strFilePath = MapModel(postedFile);
                string strContentType = postedFile.ContentType;
                var bapsTargets = ReadExcelTargetReccuring(postedFile.InputStream, postedFile.FileName, departmentCode);

                var uploadResult = _inputTargetService.UploadTargetRecurring(bapsTargets, departmentCode);
                if (uploadResult.Count() > 0)
                {
                    string excelException = string.Empty;
                    foreach (var res in uploadResult)
                    {
                        if(res.ErrorMessage == "Error on System. Please Call IT Help Desk!")
                        {
                            throw new Exception(res.ErrorMessage);
                        }
                
                        excelException += String.Format(ValidationUploadInvalidData, res.RowIndex, "SONumber", "SONumber " + res.SONumber, "is not listed in our database");
                    }
                    if (excelException != string.Empty)
                    {
                        throw new Exception(excelException);
                    }
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("UploadExcelTargetNewBaps")]
        public IHttpActionResult UploadExcelTargetNewBaps()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile postedFile = HttpContext.Current.Request.Files["FileUpload"];

                //string departmentCode = nvc.Get("DepartmentCode");
                //string strFilePath = MapModel(postedFile);
                string strContentType = postedFile.ContentType;
                var bapsTargets = ReadExcelTargetNewBaps(postedFile.InputStream, postedFile.FileName);

                var uploadResult = _inputTargetService.UploadTargetNewBaps(bapsTargets);
                if (uploadResult.Count() > 0)
                {
                    string excelException = string.Empty;
                    foreach (var res in uploadResult)
                    {
                        if (res.ErrorMessage == "Error on System. Please Call IT Help Desk!")
                        {
                            throw new Exception(res.ErrorMessage);
                        }
                        excelException += String.Format(ValidationUploadInvalidData, res.RowIndex, "SONumber", "SONumber " + res.SoNumber, "is not listed in our database.");
                    }
                    if (excelException != string.Empty)
                    {
                        throw new Exception(excelException);
                    }
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [NonAction]
        private List<MstRATargetRecurring> ReadExcelTargetReccuring(Stream fileInfo, string fileName, string departmentCode)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            using (var package = new ExcelPackage(fileInfo))
            {
                string excelException = string.Empty;
                var bapsSites = new List<MstRATargetRecurring>();
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(); // package.Workbook.Worksheets[worksheetName];
                var rowCount = worksheet.Dimension?.Rows;


                var mstBapsType = new List<ARSystemService.mstBapsType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    mstBapsType = client.GetBapsTypeToList(UserManager.User.UserToken, "").ToList();
                }
                var mstPowerType = new List<ARSystemService.mstBapsPowerType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    mstPowerType = client.GetBapsPowerTypeToList(UserManager.User.UserToken, "PowerType ASC").ToList();
                }
                for (int row = 2; row <= rowCount.Value; row++)
                {
                    var bapsSite = new MstRATargetRecurring();
                    bapsSite.RowIndex = row;
                    if (worksheet.Cells[row, 1].Value == null &&
                        worksheet.Cells[row, 2].Value == null &&
                        worksheet.Cells[row, 3].Value == null &&
                        worksheet.Cells[row, 4].Value == null &&
                        worksheet.Cells[row, 5].Value == null)
                    {
                        break;
                    }
                    else
                    {
                        bapsSite.SONumber = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : string.Empty;
                    }
                    DateTime? _startInvDate;
                    excelException += CheckDateFormatExcel(worksheet.Cells[row, 2].Value, row, "Start Invoice Date", out _startInvDate);
                    if (_startInvDate != null)
                    {
                        bapsSite.StartInvoiceDate = _startInvDate;
                    }
                    DateTime? _endInvDate;
                    excelException += CheckDateFormatExcel(worksheet.Cells[row, 3].Value, row, "End Invoice Date", out _endInvDate);
                    if (_endInvDate != null)
                    {
                        bapsSite.EndInvoiceDate = _endInvDate;
                    }


                    Decimal? _amountIDR;
                    excelException += CheckDecimalFormatExcel(worksheet.Cells[row, 4].Value, row, "AmountIDR", "must be numeric", out _amountIDR);
                    if (_amountIDR != null)
                    {
                        bapsSite.AmountIDR = _amountIDR;
                    }
                    Decimal? _amountUSD;
                    excelException += CheckDecimalFormatExcel(worksheet.Cells[row, 5].Value, row, "AmountUSD", "must be numeric", out _amountUSD);
                    if (_amountUSD != null)
                    {
                        bapsSite.AmountUSD = _amountUSD;
                    }

                    Int32? _month;
                    excelException += CheckIntFormatExcel(worksheet.Cells[row, 6].Value, row, "Month Target", "must be number of month (1-12)", out _month);
                    if (_month != null)
                    {
                        bapsSite.Month = _month;
                    }

                    Int32? _year;
                    excelException += CheckIntFormatExcel(worksheet.Cells[row, 7].Value, row, "Year Target", "must be number of year", out _year);
                    if (_year != null)
                    {
                        bapsSite.Year = _year;
                    }

                    var bapsName = worksheet.Cells[row, 8].Value != null ? (worksheet.Cells[row, 8].Value.ToString()) : string.Empty;
                    if (bapsName == string.Empty)
                    {
                        excelException += String.Format(ValidationUploadEmptyData, row, "BAPS Type");
                    }
                    else
                    {
                        var _baps = mstBapsType.FirstOrDefault(m => m.BapsType == bapsName);
                        if (_baps != null)
                        {
                            bapsSite.BapsType = _baps.mstBapsTypeId;
                        }
                        else
                        {
                            excelException += String.Format(ValidationUploadInvalidData, row, "BAPS Type", bapsName, "not found in master BAPS Type");
                        }
                    }
                    var powerTypeName = worksheet.Cells[row, 9].Value != null ? (worksheet.Cells[row, 9].Value.ToString()) : string.Empty;
                    //bapsSite.PowerType = "0000"; //default set 0000, based on history di database existing, value power diisi 0000 if null.
                    if (bapsName == string.Empty)
                    {
                        excelException += String.Format(ValidationUploadEmptyData, row, "Power Type");
                    }
                    else
                    {
                        if (bapsSite.BapsType == 3)
                        {
                            var _power = mstPowerType.FirstOrDefault(m => m.PowerType == powerTypeName);
                            if (_power != null)
                            {
                                bapsSite.PowerType = _power.KodeType;
                            }
                            else
                            {
                                excelException += String.Format(ValidationUploadInvalidData, row, "Power Type", powerTypeName, "not found in master Power Type");
                            }
                        }
                    }

                    bapsSite.CreatedBy = userCredential.UserID;
                    bapsSite.DepartmentCode = departmentCode;
                    bapsSites.Add(bapsSite);
                }
                if (excelException != String.Empty)
                {
                    throw new Exception(excelException);
                }
                return bapsSites;
            }

        }

        [NonAction]
        private List<MstRATargetNewBaps> ReadExcelTargetNewBaps(Stream fileInfo, string fileName)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            using (var package = new ExcelPackage(fileInfo))
            {
                string excelException = string.Empty;
                var bapsSites = new List<MstRATargetNewBaps>();
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(); // package.Workbook.Worksheets[worksheetName];
                var rowCount = worksheet.Dimension?.Rows;


                var mstBapsType = new List<ARSystemService.mstBapsType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    mstBapsType = client.GetBapsTypeToList(UserManager.User.UserToken, "").ToList();
                }
                var mstPowerType = new List<ARSystemService.mstBapsPowerType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    mstPowerType = client.GetBapsPowerTypeToList(UserManager.User.UserToken, "PowerType ASC").ToList();
                }
                for (int row = 2; row <= rowCount.Value; row++)
                {
                    var bapsSite = new MstRATargetNewBaps();
                    bapsSite.RowIndex = row;
                    bapsSite.DepartmentCode = RADepartmentCodeTabEnum.NewBaps;
                    if (worksheet.Cells[row, 1].Value == null &&
                        worksheet.Cells[row, 2].Value == null &&
                        worksheet.Cells[row, 3].Value == null &&
                        worksheet.Cells[row, 4].Value == null &&
                        worksheet.Cells[row, 5].Value == null)
                    {
                        break;
                    }
                    else
                    {
                        bapsSite.SoNumber = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : string.Empty;
                    }
                    DateTime? _startInvDate;
                    excelException += CheckDateFormatExcel(worksheet.Cells[row, 2].Value, row, "Start Invoice Date", out _startInvDate);
                    if (_startInvDate != null)
                    {
                        bapsSite.StartInvoiceDate = _startInvDate;
                    }
                    DateTime? _endInvDate;
                    excelException += CheckDateFormatExcel(worksheet.Cells[row, 3].Value, row, "End Invoice Date", out _endInvDate);
                    if (_startInvDate != null)
                    {
                        bapsSite.EndInvoiceDate = _endInvDate;
                    }


                    Decimal? _amountIDR;
                    excelException += CheckDecimalFormatExcel(worksheet.Cells[row, 4].Value, row, "AmountIDR", "must be numeric", out _amountIDR);
                    if (_amountIDR != null)
                    {
                        bapsSite.AmountIDR = _amountIDR;
                    }
                    Decimal? _amountUSD;
                    excelException += CheckDecimalFormatExcel(worksheet.Cells[row, 5].Value, row, "AmountUSD", "must be numeric", out _amountUSD);
                    if (_amountUSD != null)
                    {
                        bapsSite.AmountUSD = _amountUSD;
                    }

                    Int32? _month;
                    excelException += CheckIntFormatExcel(worksheet.Cells[row, 6].Value, row, "Month Target", "must be number of month (1-12)", out _month);
                    if (_month != null)
                    {
                        bapsSite.Month = _month;
                    }

                    Int32? _year;
                    excelException += CheckIntFormatExcel(worksheet.Cells[row, 7].Value, row, "Year Target", "must be number of year", out _year);
                    if (_year != null)
                    {
                        bapsSite.Year = _year;
                    }

                    var bapsName = worksheet.Cells[row, 8].Value != null ? (worksheet.Cells[row, 8].Value.ToString()) : string.Empty;
                    if (bapsName == string.Empty)
                    {
                        excelException += String.Format(ValidationUploadEmptyData, row, "BAPS Type");
                    }
                    else
                    {
                        var _baps = mstBapsType.FirstOrDefault(m => m.BapsType == bapsName);
                        if (_baps != null)
                        {
                            bapsSite.BapsType = _baps.mstBapsTypeId;
                        }
                        else
                        {
                            excelException += String.Format(ValidationUploadInvalidData, row, "BAPS Type", bapsName, "not found in master BAPS Type");
                        }
                    }
                    var powerTypeName = worksheet.Cells[row, 9].Value != null ? (worksheet.Cells[row, 9].Value.ToString()) : string.Empty;
                    bapsSite.PowerType = "0000"; //default set 0000, based on history di database existing, value power diisi 0000 if null.
                    if (bapsName == string.Empty)
                    {
                        excelException += String.Format(ValidationUploadEmptyData, row, "Power Type");
                    }
                    else
                    {
                        if (bapsSite.BapsType == 3)
                        {
                            var _power = mstPowerType.FirstOrDefault(m => m.PowerType == powerTypeName);
                            if (_power != null)
                            {
                                bapsSite.PowerType = _power.KodeType;
                            }
                            else
                            {
                                excelException += String.Format(ValidationUploadInvalidData, row, "Power Type", powerTypeName, "not found in master Power Type");
                            }
                        }
                    }

                    bapsSite.CreatedBy = userCredential.UserID;
                    bapsSites.Add(bapsSite);
                }
                if (excelException != String.Empty)
                {
                    throw new Exception(excelException);
                }
                return bapsSites;
            }
        }

        [NonAction]
        private string MapModel(HttpPostedFile postedFile)
        {

            string filePath = "";
            if (postedFile != null)
            {
                string dir = "\\RevenueAssurance\\InputTarget\\";
                string filename = postedFile.FileName.Substring(0, postedFile.FileName.Length - 4);
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                filePath = dir + "BAPS_TARGET_RECURRING_" + fileTimeStamp;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + "BAPS_TARGET_RECURRING_" + fileTimeStamp);
            }
            return filePath;
        }


        [NonAction]
        private string CheckDateFormatExcel(object excelData, int row, string column, out DateTime? outDate)
        {
            outDate = null;
            DateTime _date;
            if (excelData == null)
            {
                var ex = String.Format(ValidationUploadEmptyData, row, column);
                return ex;
            }
            else
            {
                double dateInDouble;
                var isDouble = double.TryParse(excelData.ToString(), out dateInDouble);
                if (isDouble)
                {
                    outDate = DateTime.FromOADate(dateInDouble);
                    return String.Empty;
                }
                var _isStartInvDate = DateTime.TryParse(excelData.ToString(), out _date);
                if (!_isStartInvDate)
                {
                    var ex = String.Format(ValidationUploadInvalidData, row, column, excelData, "must be formated M/D/YYYY");
                    return ex;
                }
                else
                {
                    outDate = _date;
                    return String.Empty;
                }
            }
        }
        [NonAction]
        private string CheckIntFormatExcel(object excelData, int row, string column, string message, out int? outNumber)
        {
            outNumber = null;
            int _number;
            if (excelData == null)
            {
                var ex = String.Format(ValidationUploadEmptyData, row, column);
                return ex;
            }
            else
            {
                var _isNumber = Int32.TryParse(excelData.ToString(), out _number);
                if (!_isNumber)
                {
                    var ex = String.Format(ValidationUploadInvalidData, row, column, excelData, message);
                    return ex;
                }
                else
                {
                    outNumber = _number;
                    return String.Empty;
                }
            }
        }
        [NonAction]
        private string CheckDecimalFormatExcel(object excelData, int row, string column, string message, out Decimal? outNumber)
        {
            outNumber = null;
            Decimal _number;
            if (excelData == null)
            {
                if ((column == "AmountUSD"))
                {
                    return null;
                }
                var ex = String.Format(ValidationUploadEmptyData, row, column);
                return ex;
            }
            else
            {
                //replace . to , if . appear more than once, it means it currency format
                //eq: 1.000.000 are invalid => the correct format is : 1,000,000
                if ((excelData.ToString().Split('.').Length - 1) >= 2)
                {
                    excelData = excelData.ToString().Replace('.', ',');
                }
                var _isNumber = Decimal.TryParse(excelData.ToString(), out _number);
                if (!_isNumber)
                {
                    var ex = String.Format(ValidationUploadInvalidData, row, column, excelData, message);
                    return ex;
                }
                else
                {
                    outNumber = _number;
                    return String.Empty;
                }
            }
        }





        #region UPDATE DELETE


        [HttpPost, Route("UpdateTargetRecurring")]
        public IHttpActionResult UpdateTargetRecurring(vwRABapsSite vwRaBapsSite)
        {
            try
            {
                var res = _inputTargetService.UpdateHistoryRecurringInputTarget(vwRaBapsSite);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetTargetRecurringByID")]
        public IHttpActionResult GetTargetRecurringByID(vwRABapsSite vwRaBapsSite)
        {
            try
            {
                var res = _inputTargetService.GetHistoryRecurringInputTargetByID(vwRaBapsSite.TargetID.GetValueOrDefault());
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("DeleteTargetRecurring")]
        public IHttpActionResult DeleteTargetRecurring(vwRABapsSite vwRaBapsSite)
        {
            try
            {
                var res = _inputTargetService.DeleteHistoryRecurringInputTarget(vwRaBapsSite.TargetID.GetValueOrDefault());
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("DeleteHistoryInputTarget")]
        public IHttpActionResult DeleteHistoryInputTarget(PostRecurring model)
        {
            try
            {
                foreach (var id in model.ListStrDeleteHistory)
                {
                    long _id = 0;
                    var ids = id.Split('_');
                    if(ids[0] != null)
                    {
                        var res = long.TryParse(ids[0], out _id);
                    } 
                    if(ids[1].ToUpper().Contains("NEW BAPS"))
                    {
                        var res = _inputTargetService.DeleteHistoryNewBapsInputTarget(_id);
                    }
                    else
                    {
                        var res = _inputTargetService.DeleteHistoryRecurringInputTarget(_id);
                    }
                }
                return Ok(model.ListStrDeleteHistory);
            }
            catch (Exception ex)
            {
                return Ok(new { ErrorMessage = ex.Message});
            }
        }
        
        //NEW BAPS


        [HttpPost, Route("UpdateTargetNewBaps")]
        public IHttpActionResult UpdateTargetNewBaps(vwRABapsSite vwRaBapsSite)
        {
            try
            {
                var res = _inputTargetService.UpdateHistoryNewBapsInputTarget(vwRaBapsSite);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetTargetNewBapsByID")]
        public IHttpActionResult GetTargetNewBapsByID(vwRABapsSite vwRaBapsSite)
        {
            try
            {
                var res = _inputTargetService.GetHistoryNewBapsInputTargetByID(vwRaBapsSite.TargetID.GetValueOrDefault());
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("DeleteTargetNewBaps")]
        public IHttpActionResult DeleteTargetNewBaps(vwRABapsSite vwRaBapsSite)
        {
            try
            {
                var res = _inputTargetService.DeleteHistoryNewBapsInputTarget(vwRaBapsSite.TargetID.GetValueOrDefault());
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }



        #endregion

    }
}