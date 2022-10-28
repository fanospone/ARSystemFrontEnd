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
using System.Text;
using System.Configuration;


namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/StartBaps")]
    public class ApiNewBapsStartBapsController : ApiController
    {

        [HttpPost, Route("getListSoNumber")]
        public IHttpActionResult GetListSoNumber(PostNewBapsStartBaps post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.vwRABapsPrintSonumbList> sonumbList = new List<ARSystemService.vwRABapsPrintSonumbList>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    post.vwRABapsPrintSonumbList.SoNumber = post.strSoNumber;
                    post.vwRABapsPrintSonumbList.SiteID = post.strSiteID;
                    post.vwRABapsPrintSonumbList.CustomerID = post.strCustomerId;
                    post.vwRABapsPrintSonumbList.StipID = Convert.ToInt16(post.strStipID);
                    post.vwRABapsPrintSonumbList.ProductID = Convert.ToInt16(post.strProductId);
                    post.vwRABapsPrintSonumbList.CompanyID = post.strCompanyId;

                    intTotalRecord = client.GetCountSoNumber(UserManager.User.UserToken, post.vwRABapsPrintSonumbList, post.strDataType, post.strCategory);
                    sonumbList = client.GetListSoNumber(UserManager.User.UserToken, post.vwRABapsPrintSonumbList, post.strDataType, post.strCategory, post.start, post.length).ToList();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = sonumbList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getApprovalCompany")]
        public IHttpActionResult GetApprovalCompany()
        {
            try
            {
                List<ARSystemService.vwRABapsPrintApprCompany> result = new List<ARSystemService.vwRABapsPrintApprCompany>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.GetApprovalCompany().ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getApprovalCustomer")]
        public IHttpActionResult GetApprovalCustomer(PostNewBapsStartBaps post)
        {
            try
            {
                List<ARSystemService.MstApprovalBAPS> result = new List<ARSystemService.MstApprovalBAPS>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.GetApprovalCustomer(post.strCustomerId, post.strRegionId).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitBapsPrintBulky")]
        public IHttpActionResult SubmitBapsPrintBulky(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsBulkyValidation result = new ARSystemService.trxRABapsBulkyValidation();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.SubmitBapsPrintBulky(UserManager.User.UserToken, post.trxRABapsBulkyValidation);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getListBapsPrintBulky")]
        public IHttpActionResult GetListBapsPrintBulky(PostNewBapsStartBaps post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.trxRABapsBulkyValidation> sonumbList = new List<ARSystemService.trxRABapsBulkyValidation>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    post.trxRABapsBulkyValidation.CompanyID = post.strCompanyId;
                    post.trxRABapsBulkyValidation.BulkNumber = post.strBulkNumber;
                    post.trxRABapsBulkyValidation.CustomerID = post.strCustomerId;
                    intTotalRecord = client.GetCountBapsPrintBulky(UserManager.User.UserToken, post.trxRABapsBulkyValidation);
                    sonumbList = client.GetListBapsPrintBulky(UserManager.User.UserToken, post.trxRABapsBulkyValidation, post.start, post.length).ToList();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = sonumbList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitBapsPrint")]
        public IHttpActionResult SubmitBapsPrint(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsValidation result = new ARSystemService.trxRABapsValidation();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.SubmitBapsPrint(UserManager.User.UserToken, post.trxRABapsValidation);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getBapsPrint")]
        public IHttpActionResult GetBapsPrint(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsValidation result = new ARSystemService.trxRABapsValidation();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.GetBapsPrint(UserManager.User.UserToken, post.trxRABapsValidation);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getListBapsMaterials")]
        public IHttpActionResult GetListBapsMaterials(PostNewBapsStartBaps post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.trxRABapsMaterials> dataList = new List<ARSystemService.trxRABapsMaterials>();
                ARSystemService.trxRABapsMaterials postValue = new ARSystemService.trxRABapsMaterials();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    postValue.SoNumber = post.strSoNumber;
                    postValue.BulkyID = post.strBulkID;
                    if (post.strSoNumber != null || post.strBulkID != 0)
                    {
                        intTotalRecord = client.GetCountBapsMaterials(UserManager.User.UserToken, postValue);
                        dataList = client.GetListBapsMaterials(UserManager.User.UserToken, postValue, post.start, post.length).ToList();
                    }

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getBapsMaterials")]
        public IHttpActionResult GetBapsMaterials(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsMaterials data = new ARSystemService.trxRABapsMaterials();
                ARSystemService.trxRABapsMaterials postValue = new ARSystemService.trxRABapsMaterials();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    postValue.SoNumber = post.strSoNumber;
                    postValue.SIRO = post.strStipSiro;
                    data = client.GetListBapsMaterials(UserManager.User.UserToken, postValue, post.start, post.length).FirstOrDefault();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("saveBapsMaterials")]
        public IHttpActionResult SaveBapsMaterials(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsMaterials data = new ARSystemService.trxRABapsMaterials();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    data = client.SaveBapsMaterials(UserManager.User.UserToken, post.trxRABapsMaterials, post.strCustomerId, post.strCompanyId);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("deleteBapsMaterials")]
        public IHttpActionResult DeleteBapsMaterials(PostNewBapsStartBaps post)
        {
            bool result = false;
            try
            {
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.DeleteBapsMaterials(UserManager.User.UserToken, post.trxRABapsMaterials);
                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return Ok(result);
            }
        }

        [HttpPost, Route("getProductType")]
        public IHttpActionResult GetProductType(PostNewBapsStartBaps post)
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    list = client.GetProduct(UserManager.User.UserToken).ToList();

                    if (post.strCategory == "ADDITIONAL")
                    {
                        list = (from d in list
                                where new[] { "Additional Antenna RF", "Additional Antenna MW", "Additional RRU", "Additional Land" }.Contains(d.Text)
                               // where d.Text.Contains("Additional")
                                select d).ToList();
                    }
                    else
                    {
                        list = (from d in list
                                    where ! new[] { "Additional Antenna RF", "Additional Antenna MW", "Additional RRU", "Additional Land" }.Contains(d.Text)
                                //where !d.Text.ToUpper().Contains("ADDITIONAL")
                                select d).ToList();
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getListDetailXLBulky")]
        public IHttpActionResult GetListDetailXLBulky(PostNewBapsStartBaps post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.trxRABapsPrintXLBulky> dataList = new List<ARSystemService.trxRABapsPrintXLBulky>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    intTotalRecord = client.GetCountDetailXLBulky(UserManager.User.UserToken, post.strBulkID);
                    dataList = client.GetListDetailXLBulky(UserManager.User.UserToken, post.strBulkID, post.start, post.length).ToList();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getDetailXLBulky")]
        public IHttpActionResult GetDetailXLBulky(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsPrintXLBulky result = new ARSystemService.trxRABapsPrintXLBulky();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.GetDetailXLBulky(UserManager.User.UserToken, post.strBulkID);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("saveDetailXLBulky")]
        public IHttpActionResult SaveDetailXLBulky(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsPrintXLBulky data = new ARSystemService.trxRABapsPrintXLBulky();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    data = client.SaveDetailXLBulky(UserManager.User.UserToken, post.trxRABapsPrintXLBulky);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("deleteDetailXLBulky")]
        public IHttpActionResult DeleteDetailXLBulky(PostNewBapsStartBaps post)
        {
            try
            {
                bool result;
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.DeleteDetailXLBulky(UserManager.User.UserToken, post.strIDTrx);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getListDetailXlAdditional")]
        public IHttpActionResult GetListDetailXlAdditional(PostNewBapsStartBaps post)
        {
            try
            {

                List<ARSystemService.trxRABapsPrintXLAdd> dataList = new List<ARSystemService.trxRABapsPrintXLAdd>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    dataList = client.GetListDetailXlAdditional(UserManager.User.UserToken, post.strBulkID).ToList();
                    return Ok(new { draw = post.draw, data = dataList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("saveDetailXlAdditional")]
        public IHttpActionResult SaveDetailXlAdditional(PostNewBapsStartBaps post)
        {
            try
            {

                ARSystemService.trxRABapsPrintXLAdd data = new ARSystemService.trxRABapsPrintXLAdd();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    data = client.SaveDetailXlAdditional(UserManager.User.UserToken, post.trxRABapsPrintXLAdd, post.strCustomerId, post.strCompanyId);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("deleteDetailXlAdditional")]
        public IHttpActionResult DeleteDetailXlAdditional(PostNewBapsStartBaps post)
        {
            try
            {
                bool result;
                ARSystemService.trxRABapsPrintXLAdd data = new ARSystemService.trxRABapsPrintXLAdd();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.DeleteDetailXlAdditional(UserManager.User.UserToken, post.strIDTrx);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getFormValidationPrint")]
        public IHttpActionResult GetFormValidationPrint(PostNewBapsStartBaps post)
        {
            string result = "";
            if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "XL" && post.strProductId.TrimStart().TrimEnd() == "Site Access (Fiberization)")
            {
                // result = "Form.XLSiteAccessForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strStipSiro + "')";
                result = "Form.XLSiteAccessForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";
            }
            //else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "SMART8" && post.strStipCategory.TrimStart().TrimEnd().ToUpper() == "ADDITIONAL MINI CME")
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "SMART8" && post.strStipCategory.TrimStart().TrimEnd() == "STIP 2")
            {
                result = "Form.SfAddForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro+ "', '" + post.strRegionId + "')";

            }
            else if ((post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "SMART" || post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "M8") && post.strStipSiro > 0)
            {
                result = "Form.SmartSiroForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "ISAT")
            {
                result = "Form.ISATForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "XL" && post.strStipSiro > 0)
            {
                result = "Form.XLSiroForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "XL" && (post.strStipCategory.TrimStart().TrimEnd() == "STIP 6" || post.strStipCategory.TrimStart().TrimEnd() == "STIP 7"))
            {
                result = "Form.XLRelocForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "XL" && post.strStipCategory.TrimStart().TrimEnd() == "STIP 1" && post.strStipSiro == 0)
            {
                result = "Form.XLNewForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro+ "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "HCPT")
            {
                result = "Form.HCPTForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "STI")
            {
                result = "Form.STIForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "INUX" || post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "FIRST")
            {
                result = "Form.INUXForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "SMART8" && post.strStipSiro ==0 && post.strStipCategory.TrimStart().TrimEnd()=="STIP 1")
            {
                result = "Form.SFNewForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            else if (post.strCustomerId.TrimStart().TrimEnd().ToUpper() == "SMART8" && post.strStipSiro > 0)
            {
                result = "Form.SFSIROForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "', '" + post.strRegionId + "')";

            }
            return Ok(result);
        }

        [HttpGet, Route("getListStipCategory")]
        public IHttpActionResult GetListStipCategory()
        {
            try
            {
                List<ARSystemService.mstSTIP> list = new List<ARSystemService.mstSTIP>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    list = client.GetListStipCategory().ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitIsat")]
        public IHttpActionResult SubmitIsat(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsMaterials data = new ARSystemService.trxRABapsMaterials();
                ARSystemService.trxRABapsValidation data2 = new ARSystemService.trxRABapsValidation();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    post.strCustomerId = "";
                    post.strCompanyId = "";
                    post.trxRABapsMaterials.SIRO = post.trxRABapsValidation.StipSiro;
                    data = client.SaveBapsMaterials(UserManager.User.UserToken, post.trxRABapsMaterials, post.strCustomerId, post.strCompanyId);

                    if (data.ErrorMessage == "" || data.ErrorMessage == null)
                    {
                        data2 = client.SubmitBapsPrint(UserManager.User.UserToken, post.trxRABapsValidation);
                    }

                    return Ok(data2);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getListHeightSpace")]
        public IHttpActionResult GetListHeightSpace(PostNewBapsStartBaps post)
        {
            try
            {
                List<ARSystemService.trxRABapsHeightSpace> result = new List<ARSystemService.trxRABapsHeightSpace>();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    int intTotalRecord = 0;
                    intTotalRecord = client.GetCountHeightSpace(UserManager.User.UserToken, post.strSoNumber);
                    result = client.GetListHeightSpace(UserManager.User.UserToken, post.strSoNumber, post.start, post.length).ToList();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("saveHeightSpace")]
        public IHttpActionResult SaveHeightSpace(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsHeightSpace result = new ARSystemService.trxRABapsHeightSpace();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.SaveHeightSpace(UserManager.User.UserToken, post.trxRABapsHeightSpace);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("deleteHeightSpace")]
        public IHttpActionResult DeleteHeightSpace(PostNewBapsStartBaps post)
        {
            try
            {
                bool result;
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.DeleteHeightSpace(UserManager.User.UserToken, post.strIDTrx);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitApproval")]
        public IHttpActionResult SubmitApproval(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRANewBapsActivity result = new ARSystemService.trxRANewBapsActivity();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.SubmitAppproval(UserManager.User.UserToken, post.strActID, post.strStatusAppr, post.strSoNumber, post.strStipSiro);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitApprovalBulky")]
        public IHttpActionResult SubmitApprovalBulky(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRABapsBulkyValidation result = new ARSystemService.trxRABapsBulkyValidation();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.SubmitAppprovalBulky(UserManager.User.UserToken, post.strBulkID, post.strActID, post.strStatusAppr, post.strCustomerId, post.strCategory);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("skipBAPSPrint")]
        public IHttpActionResult SkipBAPSPrint(PostNewBapsStartBaps post)
        {
            try
            {
                ARSystemService.trxRANewBapsActivity result = new ARSystemService.trxRANewBapsActivity();
                using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
                {
                    result = client.SkipBAPSPrint(UserManager.User.UserToken, post.strSoNumber, post.strStipSiro);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { GenericError = ex.Message });
            }
        }

        //[HttpGet, Route("getBapsTypeList")]
        //public IHttpActionResult GetBapsTypeList()
        //{
        //    try
        //    {
        //        List<ARSystemService.mstBapsType> list = new List<ARSystemService.mstBapsType>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            list = client.GetBapsTypeList(UserManager.User.UserToken).ToList();
        //            return Ok(list);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("gridSonumbList")]
        //public IHttpActionResult GetDataSonumblist(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        int intTotalRecord = 0;
        //        List<ARSystemService.vwRABapsPrintSonumbList> sonumbList = new List<ARSystemService.vwRABapsPrintSonumbList>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            intTotalRecord = client.GetCountSoNumbListBaps(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantTypeID, post.strAction, post.strDataType, post.strBapsType, post.strCategory, post.strStipID);
        //            sonumbList = client.GetSoNumbListBaps(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantTypeID, post.strAction, post.strDataType, post.strBapsType, post.strCategory, post.strStipID, post.start, post.length).ToList();

        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = sonumbList });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("gridSonumbListDDL")]
        //public IHttpActionResult GetDataSonumblistDDL(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.vwRANewBapsSonumbList> sonumbList = new List<ARSystemService.vwRANewBapsSonumbList>();
        //        //using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        //{
        //        //    sonumbList

        //        //    //sonumbList = client.GetSoNumbListBaps(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantTypeID, post.strAction, post.strDataType, post.strBapsType, 0, 0).ToList();
        //        //    //System.Web.HttpContext.Current.Session["FileList"] = sonumbList;

        //        //    return Ok(sonumbList);
        //        //}
        //        sonumbList = System.Web.HttpContext.Current.Session["SoNumblist"] as List<ARSystemService.vwRANewBapsSonumbList>;
        //        return Ok(sonumbList);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getBapsValidation")]
        //public IHttpActionResult GetBapsValidation(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.mstBaps result = new ARSystemService.mstBaps();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetBapsValidation(UserManager.User.UserToken, post.strSoNumber, post.strCustomerId, post.strStipSiro);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("submitBapsValidation")]
        //public IHttpActionResult SubmitBapsValidation(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.mstBaps result = new ARSystemService.mstBaps();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.SubmitBapsValidation(UserManager.User.UserToken, post.bapsValidation);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}




        //[HttpPost, Route("gridValidationBulkyPrintList")]
        //public IHttpActionResult GetValidationBulkyPrintList(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.trxRABapsBulkyValidation> result = new List<ARSystemService.trxRABapsBulkyValidation>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetDataBulkValidation(UserManager.User.UserToken, post.strBulkNumber, post.strCompanyId, post.strCustomerId, 0, 0).ToList();

        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("gridValidationBulkyPrint")]
        //public IHttpActionResult GetValidationBulkyPrint(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.trxRABapsBulkyValidation> result = new List<ARSystemService.trxRABapsBulkyValidation>();
        //        int intTotalRecord = 0;
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            intTotalRecord = client.GetCountDataValidationBulky(UserManager.User.UserToken, post.strBulkNumber, post.strCompanyId, post.strCustomerId);
        //            result = client.GetDataBulkValidation(UserManager.User.UserToken, post.strBulkNumber, post.strCompanyId, post.strCustomerId, post.start, post.length).ToList();
        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("addDetailXLBulky")]
        //public IHttpActionResult AddDetailXLBulky(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsPrintXLBulky result = new ARSystemService.trxRABapsPrintXLBulky();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.AddDetailXLBulky(UserManager.User.UserToken, post.XlBulky);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("deleteDetailXLBulky")]
        //public IHttpActionResult DeleteDetailXLBulky(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        bool result;
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.DeleteDetailXLBulky(UserManager.User.UserToken, post.strBulkID);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getDetailXLBulky")]
        //public IHttpActionResult GetDetailXLBulky(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsPrintXLBulky result = new ARSystemService.trxRABapsPrintXLBulky();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetDetailXLBulky(UserManager.User.UserToken, post.XlBulky.ID);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getDetailXLBulkyList")]
        //public IHttpActionResult GetDetailXLBulkyList(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.trxRABapsPrintXLBulky> result = new List<ARSystemService.trxRABapsPrintXLBulky>();
        //        int intTotalRecord = 0;
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            intTotalRecord = client.GetCountDetailXLBulkyList(UserManager.User.UserToken, post.strBulkID);
        //            result = client.GetDetailXLBulkyList(UserManager.User.UserToken, post.strBulkID, post.start, post.length).ToList();
        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("submitXlBulky")]
        //public IHttpActionResult SubmitXLBulky(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsBulkyValidation result = new ARSystemService.trxRABapsBulkyValidation();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.UpdateValidationBulky(UserManager.User.UserToken, post.validateBulky);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getDataXLBulkyDetail")]
        //public IHttpActionResult GetDataXLBulkyDetail(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsBulkyValidation result = new ARSystemService.trxRABapsBulkyValidation();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetDataBulkyValidationDetail(UserManager.User.UserToken, post.strBulkID);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("submitValidation")]
        //public IHttpActionResult SubmitValidation(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsValidation result = new ARSystemService.trxRABapsValidation();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.SubmitValidation(UserManager.User.UserToken, post.validation);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getValidationData")]
        //public IHttpActionResult GetValidationData(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsValidation result = new ARSystemService.trxRABapsValidation();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetValidationData(UserManager.User.UserToken, post.strSoNumber, post.strSiteID, post.strCustomerId, post.strStipSiro.ToString());
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getDataEquipment")]
        //public IHttpActionResult GetDataEquipment(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.trxRABapsHeightSpace> result = new List<ARSystemService.trxRABapsHeightSpace>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            int intTotalRecord = 0;
        //            intTotalRecord = client.GetCountHeightSpace(UserManager.User.UserToken, post.strSoNumber);
        //            result = client.GetDataHeightSpace(UserManager.User.UserToken, post.strSoNumber, post.start, post.length).ToList();
        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("addEquipment")]
        //public IHttpActionResult AddEquipment(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsHeightSpace result = new ARSystemService.trxRABapsHeightSpace();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.SubmitHeightSpace(UserManager.User.UserToken, post.equipment);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("deleteEquipment")]
        //public IHttpActionResult DeleteEquipment(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        bool result;
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.DeleteHeightSpace(UserManager.User.UserToken, post.strIDTrx);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getXlAddList")]
        //public IHttpActionResult GetXLAddList(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.tempSoNumbAddtional> result = new List<ARSystemService.tempSoNumbAddtional>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            int intTotalRecord = 0;
        //            result = client.GetListSonmbAdd(UserManager.User.UserToken, post.strSoNumber, post.strSiteID).ToList();
        //            intTotalRecord = result.Count();
        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        //[HttpPost, Route("getTrxXlAddList")]
        //public IHttpActionResult GetTrxXLAddList(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.trxRABapsPrintXLAdd> result = new List<ARSystemService.trxRABapsPrintXLAdd>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetTrxXLAdd(UserManager.User.UserToken, post.strBulkID).ToList();
        //            int intTotalRecord = result.Count;
        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        //[HttpPost, Route("deleteTrxXlAddList")]
        //public IHttpActionResult DeleteTrxXLAddList(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        bool result = false;
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.XLAddDetailDelete(UserManager.User.UserToken, post.strIDTrx);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        //[HttpPost, Route("submitDetailXLAdd")]
        //public IHttpActionResult SubmitDetailXLAdd(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsPrintXLAdd result = new ARSystemService.trxRABapsPrintXLAdd();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.XlAddDetailSave(UserManager.User.UserToken, post.xlAdditional, post.strCustomerId, post.strCompanyId, post.strSoNumber);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getSiteIDCust")]
        //public IHttpActionResult GetSiteIDCust(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.vwRaNewBapsGetSiteIDCustomerList> result = new List<ARSystemService.vwRaNewBapsGetSiteIDCustomerList>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetSiteIDCustomerList(UserManager.User.UserToken, post.strCustomerId).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getSoNumbAddlist")]
        //public IHttpActionResult GetSoNumbAddlist(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.vwRaNewBapsGetSonumbAddList> result = new List<ARSystemService.vwRaNewBapsGetSonumbAddList>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetSonumbAddList(UserManager.User.UserToken, post.strCustomerId).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}


        // ===================== un use =========================//


        //[HttpPost, Route("gridPica")]
        //public IHttpActionResult GetDataPICA(PostNewBapsCheckingDocument post)
        //{
        //    try
        //    {

        //        int intTotalRecord = 0;
        //        List<ARSystemService.vwNewBapsCheckingDocument> startBaps = new List<ARSystemService.vwNewBapsCheckingDocument>();

        //        using (var client = new ARSystemService.NewBapsCheckingDocumentServiceClient())
        //        {
        //            intTotalRecord = client.GetCheckDocCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strProductId, post.strGroupBy);

        //            startBaps = client.GetCheckDocTodoList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strProductId, post.strGroupBy, post.start, post.length).ToList();

        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = startBaps });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}


        //[HttpPost, Route("formValidation")]
        //public IHttpActionResult GetFormValidation(PostGetValidationForm post)
        //{
        //    try
        //    {
        //        List<ARSystemService.mstValidationForm> formValildation = new List<ARSystemService.mstValidationForm>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            formValildation = client.GetValidationForm(UserManager.User.UserToken, post.strFormName, post.strCustomerId).ToList();
        //            return Ok(formValildation);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("fillForm")]
        //public IHttpActionResult GetFillForm(PostGetFillForm post)
        //{
        //    try
        //    {
        //        ARSystemService.vwNewBapsValidationForm result = new ARSystemService.vwNewBapsValidationForm();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetFillFormValidation(UserManager.User.UserToken, post.strCustomerId, post.strProductType, post.strSoNumber, post.strStipSiro, post.strBapsType);
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        //[HttpPost, Route("gridValidation")]
        //public IHttpActionResult GetDataValidation(PostNewBapsStartBaps post)
        //{
        //    try
        //    {


        //        int intTotalRecord = 0;
        //        //  List<ARSystemService.vwNewBapsCheckingDocument> startBaps = new List<ARSystemService.vwNewBapsCheckingDocument>();

        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            intTotalRecord = client.GetCountDataValidation(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strGroupBy, post.strBapsTypeId);

        //            //   startBaps = client.GetDataValidation(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strGroupBy, post.strBapsTypeId, post.start, post.length).ToList();

        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getValidationData")]
        //public IHttpActionResult GetValidationForm(PostGetValidationData post)
        //{
        //    try
        //    {

        //        ARSystemService.vwTrxBapsValidationData result = new ARSystemService.vwTrxBapsValidationData();

        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {

        //            result = client.GetBapsValidationData(UserManager.User.UserToken, post.strSoNumber, post.strSiteID);

        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        //[HttpPost, Route("getSoNumbXLStip3List")]
        //public IHttpActionResult GetSoNumbXLStip3List(PostNewBapsStartBaps post)
        //{
        //    try
        //    {
        //        List<ARSystemService.tempSonumbPrintBaps> result = new List<ARSystemService.tempSonumbPrintBaps>();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetSoNumbXLStip3List(UserManager.User.UserToken, post.strCustomerId).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

    }

}