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
using ARSystem.Service;
using ARSystem.Domain.Models;
using System.Threading.Tasks;
using ARSystem.Domain.Models.Models.RevenueAssurance;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/BAPSValidation")]
    public class ApiBAPSValidationController : ApiController
    {
        private readonly BapsValidationService bapsValidationService;
        public ApiBAPSValidationController()
        {
            bapsValidationService = new BapsValidationService();
        }

        [HttpGet, Route("getBapsTypeList")]//kepake
        public IHttpActionResult GetBapsTypeList()
        {
            try
            {
                List<ARSystemService.mstBapsType> list = new List<ARSystemService.mstBapsType>();
                using (var client = new ARSystemService.NewBapsServiceClient())
                {
                    list = client.GetBapsTypeList(UserManager.User.UserToken).ToList();
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridSonumbList")]//kepake
        public IHttpActionResult GetDataSonumblist(PostBAPSValidation post)
        {
            try
            {
                int intTotalRecord = 0;
                List<vmNewBapsData> sonumbList = new List<vmNewBapsData>();


                if (!string.IsNullOrEmpty(post.strTenantTypeID) && post.strTenantTypeID == "baps")
                {
                    intTotalRecord = bapsValidationService.GetCountSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantTypeID,
                        post.strAction, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    sonumbList = bapsValidationService.GetSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID,
                        post.strTenantTypeID, post.strAction, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, strOrderBy, post.start, post.length).ToList();

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = sonumbList });
                }
                else
                {
                    if (post.strTenantTypeID == "validation-bulky")
                    {
                        //BAPS TYPE - 5 -> TOWER
                        //BAPS bulky filter validated baps with tower type = 5
                        post.strBapsTypeId = "5";
                    }
                    intTotalRecord = bapsValidationService.GetCountBapsDataValidationList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID,
                        post.strTenantTypeID, post.strAction, post.strBaukDoneYear, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, post.strBapsTypeId, post.strSiteName, post.strSONumberMultiple);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    sonumbList = bapsValidationService.GetBapsDataValidationList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID,
                        post.strTenantTypeID, post.strAction, post.strBaukDoneYear, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, post.strBapsTypeId, post.strSiteName, post.strSONumberMultiple, strOrderBy, post.start, post.length).ToList();

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = sonumbList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridSonumbListIds")]//kepake
        public IHttpActionResult GetDataSoNumbListIds(PostBAPSValidation post)
        {
            try
            {
                int intTotalRecord = 0;
                List<vmNewBapsData> sonumbList = new List<vmNewBapsData>();

                if (!string.IsNullOrEmpty(post.strTenantTypeID) && post.strTenantTypeID == "baps")
                {

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    sonumbList = bapsValidationService.GetSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID,
                        post.strTenantTypeID, post.strAction, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, strOrderBy, 0, 0).ToList();

                    return Ok(sonumbList);
                }
                else
                {
                    if(post.strTenantTypeID == "validation-bulky")
                    {
                        //BAPS TYPE - 5 -> TOWER
                        //BAPS bulky filter validated baps with tower type = 5
                        post.strBapsTypeId = "5";
                    }
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    sonumbList = bapsValidationService.GetBapsDataValidationList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, 
                        post.strTenantTypeID, post.strAction, post.strBaukDoneYear, post.strStipID, post.strSiroID, post.strStartBaukDoneDate, post.strEndBaukDoneDate, post.strBapsTypeId, post.strSiteName, post.strSONumberMultiple, strOrderBy, 0, 0).ToList();

                    return Ok(sonumbList);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("getBapsValidation")]//kepake
        public IHttpActionResult GetBapsValidation(PostBAPSValidation post)
        {
            try
            {
                ARSystemService.mstBaps result = new ARSystemService.mstBaps();
                using (var client = new ARSystemService.BAPSValidationClient())
                {
                    result = client.GetDataBAPSValidation(UserManager.User.UserToken, post.strSoNumber, post.strCustomerId, post.strStipSiro);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitBapsValidation")]//kepake
        public IHttpActionResult SubmitBapsValidation(PostBAPSValidation post)
        {
            try
            {
                post.bapsValidation.BapsDate = null;
                ARSystemService.mstBaps result = new ARSystemService.mstBaps();
                using (var client = new ARSystemService.BAPSValidationClient())
                {
                    result = client.BAPSValidationSubmit(UserManager.User.UserToken, post.strProductId == null ? 0 : int.Parse(post.strProductId), post.bapsValidation);

                    if (string.IsNullOrEmpty(result.ErrorMessage) && post.FreeRent.FreeRent != 0)
                    {
                        client.SaveFreeRent(UserManager.User.UserToken, post.FreeRent);
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getFormValidationPrint")] //Kepake
        public IHttpActionResult GetFormValidationPrint(PostBAPSValidation post)
        {
            string result = "";
            if (post.strCustomerId.Trim().ToUpper() == "XL" && post.strStipCategory.Trim().ToUpper() == "STIP 2")
            {
                result = "Form.XLAddForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strStipSiro + "')";
            }
            else if
                  (post.strCustomerId.Trim().ToUpper() == "SMART8" && post.strStipCategory.Trim().ToUpper() == "STIP 2")
            {
                result = "Form.SfAddForm('" + post.strSoNumber + "', '" + post.strSiteID + "', '" + post.strCustomerId + "', '" + post.strProductId + "', '" + post.strStipCategory + "', '" + post.strStipSiro + "')";

            }
            return Ok(result);
        }

        [HttpPost, Route("ApproveBAPSValidation")]//kepake
        public IHttpActionResult ApproveBapsValidation(ARSystemService.trxRANewBapsActivity post)
        {
            try
            {
                ARSystemService.trxRANewBapsActivity result = new ARSystemService.trxRANewBapsActivity();
                using (var client = new ARSystemService.BAPSValidationClient())
                {
                    result = client.ApproveBAPSValidation(UserManager.User.UserToken, post);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdateBAPSValidation")]//kepake
        public IHttpActionResult UpdateBAPSValidation(PostBAPSValidationUpdate post)
        {
            try
            {
                vmBAPSValidationUpdate bapsValidationUpdate = new vmBAPSValidationUpdate();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                string UserID = userCredential.UserID;
                bapsValidationUpdate = bapsValidationService.UpdateBAPSValidationInfo(UserID, post.strSONumber, post.strFieldName, post.strFieldValue);

                return Ok(bapsValidationUpdate);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetEndDate")]
        public IHttpActionResult GetEndDate(DateTime date, int invoice)
        {
            try
            {
                if (invoice == 1)
                    date = date.AddMonths(1);
                else if (invoice == 2)
                    date = date.AddMonths(3);
                else
                    date = date.AddYears(1);


                date = date.AddDays(-1);

                return Ok(date);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region WaitingPo
        [HttpPost, Route("gridWaitingInputPoList")]//kepake
        public IHttpActionResult GetDataWaitingInputPoList(PostBAPSValidation post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.vmNewBapsData> sonumbList = new List<ARSystemService.vmNewBapsData>();

                using (var client = new ARSystemService.BAPSValidationClient())
                {
                    intTotalRecord = client.GetCountWaitingPoList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantTypeID, post.strAction);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    sonumbList = client.GetBapsWaitingPoList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerId, post.strProductId, post.strSoNumber, post.strSiteID, post.strTenantTypeID, post.strAction, strOrderBy, post.start, post.length).ToList();

                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = sonumbList });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        [HttpGet, Route("GetDefaultPrice")]
        public async Task<IHttpActionResult> GetDefaultPrice(string SoNumber)
        {
            var data = new vwRANewBapsValidationDefaultPrice();

            try
            {
                var service = new BapsValidationService();
                data = await service.DefaultPrice(new vwRANewBapsValidationDefaultPrice { SONumber = SoNumber });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //added ftm 30-8-2021
        [HttpPost, Route("submitBulkyBapsValidation")]//kepake
        public IHttpActionResult SubmitBulkyBapsValidation(vmMstBapsBulky post)
        {
            try
            {
                var param = new vmMstBapsBulky();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                string UserID = userCredential.UserID;
                var result = bapsValidationService.BAPSValidationBulkySubmit(UserID, post);


                    return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}