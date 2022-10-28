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
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI;
using ARSystemFrontEnd.Helper;
//using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("Api/RevenueSystem")]
    public class ApiRevenueSystemController : ApiController
    {
        // GET: ApiRevenueSystem
        #region Revenue System Parameters
        [HttpPost, Route("GetRevSysCompany")]
        public IHttpActionResult GetCompany()
        {
            try
            {
                List<ARSystemService.vmRevSysCompanyAsset> data = new List<ARSystemService.vmRevSysCompanyAsset>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysAttributeHeader(UserManager.User.UserToken, "LoadFilterCompany").ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetCompanyInv")]
        public IHttpActionResult GetCompanyInv()
        {
            try
            {
                List<ARSystemService.vmRevSysCompanyAsset> data = new List<ARSystemService.vmRevSysCompanyAsset>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysAttributeHeader(UserManager.User.UserToken, "LoadFilterCompany").ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetOperatorRevSys")]
        public IHttpActionResult GetOperatorRevSys()
        {
            try
            {
                List<ARSystemService.vmRevSysCompanyAsset> data = new List<ARSystemService.vmRevSysCompanyAsset>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysAttributeHeader(UserManager.User.UserToken, "LoadFilterOperator").ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }



        [HttpPost, Route("GetRegionRevSys")]
        public IHttpActionResult GetRegionRevSys()
        {
            try
            {
                List<ARSystemService.vmRevSysCompanyAsset> data = new List<ARSystemService.vmRevSysCompanyAsset>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysAttributeHeader(UserManager.User.UserToken, "LoadFilterRegion").ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetKategoryRevenueRevSys")]
        public IHttpActionResult GetKategoryRevenueRevSys()
        {
            try
            {
                List<ARSystemService.vmRevSysCompanyAsset> data = new List<ARSystemService.vmRevSysCompanyAsset>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysAttributeHeader(UserManager.User.UserToken, "LoadFilterKategoryRevenue").ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);


            }
        }

        [HttpPost, Route("getMaxAsatDate")]
        public IHttpActionResult getMaxAsatDate()
        {
            try
            {
                List<ARSystemService.vwRevSysMaxAsatDate> data = new List<ARSystemService.vwRevSysMaxAsatDate>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysMaxAssatDate(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);


            }
        }



        [HttpGet, Route("GetRevSysAmountHeader")]
        public IHttpActionResult GetRevSysAmountHeader(string comAsset, string comInv, string operatorid, string region, string KategoryRevenue, string MonthYear)
        {
            try
            {
                List<ARSystemService.vmRevSysAmountHeader> data = new List<ARSystemService.vmRevSysAmountHeader>();
                ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
                param.CompanyAssetID = comAsset;
                param.CompanyInvoiceID = comInv;
                param.OperatorID = operatorid;
                param.RegionID = region;
                param.KategoryRevenue = KategoryRevenue;
                param.Month = MonthYear.ToString().Substring(5).Trim();
                param.Year = MonthYear.ToString().Substring(0, 4).Trim();
                param.SpParam = "LoadHeader";

                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysAmountHeader(UserManager.User.UserToken, param).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);


            }
        }


        [HttpPost, Route("GetRevSysDetail")]
        public IHttpActionResult GetRevSysDetail(PostRevenueSystem post)
        {
            try
            {
                List<ARSystemService.vmRevSysDataDetail> RevSysdataDetail = new List<ARSystemService.vmRevSysDataDetail>();
                ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
                param.Sonumb = post.Sonumb;
                param.KategoryRevenue = post.KategoryRevenue;
                param.SpParam = "LoadDetailRevSys";
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    RevSysdataDetail = client.GetRevSysDataDetail(UserManager.User.UserToken, param).ToList();
                }
                return Ok(new { data = RevSysdataDetail });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetRevSysSearchDataHeader")]
        public IHttpActionResult GetRevSysSearchDataHeader(PostRevenueSystem post)
        {
            int intTotalRecord = 0;
            List<ARSystemService.vmRevSysDataHeader> RevSysdataSearch = new List<ARSystemService.vmRevSysDataHeader>();
            ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();


            try
            {

                string whereclouse = "";
                for (int i = 1; i < post.columns.Count; i++)
                {

                    var columns = post.columns[i].data.ToString().Trim();

                    int r = i - 1;
                    {
                        var datasearchColumn = post.columnValue[r];
                        if (datasearchColumn != null && datasearchColumn != "")
                        {
                            if (i == 1)
                            {
                                whereclouse = columns.ToString().Trim() + " like  '%" + datasearchColumn.ToString().Trim() + "%' ";
                            }
                            else
                            {
                                if (whereclouse.ToString().Trim().Length == 0)
                                    whereclouse = columns.ToString().Trim() + " like  '%" + datasearchColumn.ToString().Trim() + "%' ";
                                else
                                    whereclouse += " and " + columns.ToString().Trim() + " like '%" + datasearchColumn.ToString().Trim() + "%' ";

                            }
                        }
                    }
                }

                param.KategoryRevenue = post.KategoryRevenue;
                param.Month = post.MonthYear.ToString().Substring(5).Trim();
                param.Year = post.MonthYear.ToString().Substring(0, 4).Trim();
                param.Startidx = post.start;
                param.Endidx = post.start + post.length;

                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    param.SpParam = "LoadSearchGridAccrueRevenueCount";
                    intTotalRecord = client.GetRevSysSearchDataHeaderCount(UserManager.User.UserToken, param, whereclouse);

                    param.SpParam = "LoadSearchGridAccrueRevenue";
                    RevSysdataSearch = client.GetRevSysSearchDataHeader(UserManager.User.UserToken, whereclouse, param).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = RevSysdataSearch });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("GetRevSysDetailSonumb")]
        public IHttpActionResult GetRevSysDetailSonumb(PostRevenueSystem post)
        {
            try
            {
                List<ARSystemService.vmRevSysDataHeader> data = new List<ARSystemService.vmRevSysDataHeader>();
                ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
                param.SpParam = "LoadDataHeaderSonumb";
                param.Sonumb = post.Sonumb;
                param.Month = post.Month;
                param.Year = post.Year;
                param.KategoryRevenue = post.KategoryRevenue;
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysDataHeader(UserManager.User.UserToken, param).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetRevSysDataHeader")]
        public IHttpActionResult GetRevSysDataHeader(PostRevenueSystem post)
        {
            try
            {

                int intTotalRecord = 0;
                List<ARSystemService.vmRevSysDataHeader> RevSysdata = new List<ARSystemService.vmRevSysDataHeader>();
                ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
                param.CompanyAssetID = post.comAsset;
                param.CompanyInvoiceID = post.comInv;
                param.OperatorID = post.OperatorID;
                param.RegionID = post.region;
                param.KategoryRevenue = post.KategoryRevenue;
                param.Month = post.MonthYear.ToString().Substring(5).Trim();
                param.Year = post.MonthYear.ToString().Substring(0, 4).Trim();


                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    param.SpParam = "LoadGridCountAccrueRevenue";
                    intTotalRecord = client.GetRevSysDataHeaderCount(UserManager.User.UserToken, param);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


                    param.Startidx = post.start;
                    param.Endidx = post.start + post.length;
                    param.SpParam = "LoadGridAccrueRevenue";
                    RevSysdata = client.GetRevSysDataHeader(UserManager.User.UserToken, param).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = RevSysdata });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetRevSysDetailDescInv")]
        public IHttpActionResult GetRevSysDetailDescInv(PostRevenueSystem post)
        {
            try
            {
                ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
                param.SpParam = "LoadDetailDescInv";
                param.Sonumb = post.Sonumb;
                param.StartPeriod = post.StartPeriod;

                List<ARSystemService.vmRevSysDataDetail> data = new List<ARSystemService.vmRevSysDataDetail>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    data = client.GetRevSysDetailDescInv(UserManager.User.UserToken, param).ToList();
                }

                return Ok(data);

                //return Ok(new { invNo = dt[0].InvoiceNo.ToString().Trim(), invTemp = dt[0].InvoiceTemp.ToString().Trim(), invDate = dt[0].InvoiceDate.ToString().Trim(), startPeriod = dt[0].StartPeriod.ToString().Trim(), endPeriode = dt[0].EndPeriod.ToString().Trim(), AmountInv = dt[0].AmountTotalInvoice.ToString().Trim() });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
        #endregion
        #region Hold/Stop Accrue
        [HttpPost, Route("GetRevSysDataHoldStopAccrue")]
        public IHttpActionResult GetRevSysDataHoldStopAccrue(PostMasterlistAccrue post)
        {
            try
            {
                List<ARSystemService.vmRevSysDataHoldStopAccrue> RevSysdataHoldStopAccrue = new List<ARSystemService.vmRevSysDataHoldStopAccrue>();
                ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();

                if (post.sonumb == null)
                    param.sonumb = "";
                else
                    param.sonumb = post.sonumb;

                param.company = post.companyID;
                param.OperatorId = post.operatorID;
                param.Startidx = post.start;
                param.Endidx = post.start + post.length;
                int intTotalRecord = 0;
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {

                    param.flag = "CountData";
                    intTotalRecord = client.GetRevSysCountHoldStopAccrue(UserManager.User.UserToken, param);

                    param.flag = "Show";
                    RevSysdataHoldStopAccrue = client.GetRevSysDataHoldStopAccrue(UserManager.User.UserToken, param).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = RevSysdataHoldStopAccrue });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }



        [HttpPost, Route("SaveRevSysHoldStopAccrue")]
        public IHttpActionResult SaveRevSysHoldStopAccrue()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;


                ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["Path"];
                param.flag = "Simpan";
                param.sonumb = nvc.Get("sonumb").ToString();
                param.date_end = nvc.Get("DateEnd").ToString();
                param.date_start = nvc.Get("DateStart").ToString();
                param.Action = nvc.Get("action").ToString();
                param.remaks = nvc.Get("Remaks").ToString();

                //if (post.Path == null)
                //    param.doc_path = "";
                //else
                //    param.doc_path = post.Path;

                //param.doc_path = post.Path;
                bool st = true;
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    param.doc_path = MapModel(postedFile);
                    st = client.SaveRevSysHoldStopAccrue(UserManager.User.UserToken, param);
                }

                return Ok(new { success = st });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        private string MapModel(HttpPostedFile postedFile)
        {
            //string nullString = "null";
            string filePath = "";
            if (postedFile != null)
            {
                string dir = "\\RevenueSystem\\HoldStopAccrue\\";
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                filePath = dir + fileTimeStamp;
                //invoice.InvReceiptFile = postedFile.FileName;
                //filepath = filePath;
                //invoice.ContentType = postedFile.ContentType;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + fileTimeStamp);
            }
            return filePath;
            //return invoice;
        }
        #endregion
        #region Accrue Per Dept

        [HttpPost, Route("GetRevSysAccruePerPIC")]
        public IHttpActionResult GetRevSysAccruePerPIC(PostMasterlistAccrue post)
        {
            try
            {
                List<ARSystemService.vmRevSysAccruePerPIC> RevSysdataAccruePerPIC = new List<ARSystemService.vmRevSysAccruePerPIC>();
                ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();

                //param.Startidx = post.start;
                //param.Endidx = post.start + post.length;
                param.year = post.year;
                param.month = post.month;
                param.week = post.week;

                int intTotalRecord = 0;
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    param.flag = "AccruePerPIC";
                    RevSysdataAccruePerPIC = client.ShowAccruePerDept(UserManager.User.UserToken, param).ToList();
                }

                return Ok(new { draw = 0, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = RevSysdataAccruePerPIC });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetRevSysDataAccruePerDept")]
        public IHttpActionResult GetRevSysDataAccruePerDept(PostAccruePerDept post)
        {
            try
            {
                List<ARSystemService.vmRevSysAccruePerDept> RevSysdataAccruePerDept = new List<ARSystemService.vmRevSysAccruePerDept>();
                ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();

                param.Category = post.Category;
                param.Startidx = post.start;
                param.Endidx = post.start + post.length;
                int intTotalRecord = 0;
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    if (post.Category == "Accrue")
                    {
                        param.flag = "CountDataAccruePerDept";
                        intTotalRecord = client.GetRevSysCountAccruePerDept(UserManager.User.UserToken, param);

                        param.year = post.year;
                        param.month = post.month;
                        param.week = post.week;
                        param.flag = "ShowAndValidasiPaging";
                        RevSysdataAccruePerDept = client.GetRevSysDataLoadAccruePerDept(UserManager.User.UserToken, param).ToList();
                    }
                    else
                    {
                        param.flag = "CountTempOverAll";
                        //param.Category = post.Category;
                        intTotalRecord = client.GetRevSysCountTempOver(UserManager.User.UserToken, param);

                        param.flag = "ShowUploadTempOverALLPaging";
                        RevSysdataAccruePerDept = client.ShowUploadTempOver(UserManager.User.UserToken, param).ToList();

                    }
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = RevSysdataAccruePerDept });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("SaveRevSysUploadPerDept")]
        public IHttpActionResult SaveRevSysUploadPerDept(PostAccruePerDept post)
        {
            try
            {
                bool st;
                ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    param.Category = post.Category;
                    if (post.Category == "Accrue")
                    {
                        param.flag = "InsertUploadPerDeptData";
                        param.year = post.year;
                        param.month = post.month;
                    }
                    else
                    {
                        param.flag = "InsertUploadOverAll";
                    }
                    st = client.SaveRevSysUpload(UserManager.User.UserToken, param);
                }

                return Ok(new { success = st });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion
        #region Invoice
        [HttpPost, Route("GetRevSysDetailInvoice")]
        public IHttpActionResult GetRevSysDetailInvoice(PostRevenueSystem post)
        {
            try
            {
                ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
                param.SpParam = "LoadDetailDescInv";
                param.Sonumb = post.Sonumb;
                param.InvoiceDate = post.InvoiceDate + "01";

                List<ARSystemService.vmRevSysDataDetail> ls = new List<ARSystemService.vmRevSysDataDetail>();
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    ls = client.GetRevSysDetailInvoice(UserManager.User.UserToken, param).ToList();
                }

                return Ok(new { data = ls });

                //return Ok(new { invNo = dt[0].InvoiceNo.ToString().Trim(), invTemp = dt[0].InvoiceTemp.ToString().Trim(), invDate = dt[0].InvoiceDate.ToString().Trim(), startPeriod = dt[0].StartPeriod.ToString().Trim(), endPeriode = dt[0].EndPeriod.ToString().Trim(), AmountInv = dt[0].AmountTotalInvoice.ToString().Trim() });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
        #endregion


    }
}