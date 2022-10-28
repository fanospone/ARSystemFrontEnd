using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/CompanyInformation")]
    public class ApiCompanyInformationController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetCompanyInformationToList(string strCompany = "", string strTerm="", int intIsActive = -1, string strCompanyType = "", int companyInformationId = 0)
        {
            try
            {
                List<ARSystemService.mstCompanyInformation> CompanyInformation = new List<ARSystemService.mstCompanyInformation>();
                using (var client = new ARSystemService.ImstCompanyInformationServiceClient())
                {
                    CompanyInformation = client.GetMstCompanyToList(UserManager.User.UserToken, strCompany, strTerm, intIsActive, strCompanyType, companyInformationId, "mstCompanyInformationId",0,0).ToList();
                }

                return Ok(CompanyInformation);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateCompanyInformation(ARSystemService.mstCompanyInformation CompanyInformation)
        {
            try
            {
                using (var client = new ARSystemService.ImstCompanyInformationServiceClient())
                {
                    CompanyInformation = client.CreateCompany(UserManager.User.UserToken, CompanyInformation);
                }

                return Ok(CompanyInformation);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateCompanyInformation(int id, ARSystemService.mstCompanyInformation CompanyInformation)
        {
            try
            {
                using (var client = new ARSystemService.ImstCompanyInformationServiceClient())
                {
                    CompanyInformation = client.UpdateCompany(UserManager.User.UserToken, id, CompanyInformation);
                }

                return Ok(CompanyInformation);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("grid")]
        public IHttpActionResult GetCompanyInformationToGrid(PostCompanyInformationView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstCompanyInformation> companyinformation = new List<ARSystemService.mstCompanyInformation>();
                using (var client = new ARSystemService.ImstCompanyInformationServiceClient())
                {
                    intTotalRecord = client.GetCompanyInformationCount(UserManager.User.UserToken, post.strCompany,post.strTerm, post.intIsActive, post.strCompanyType, 0);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    companyinformation = client.GetMstCompanyToList(UserManager.User.UserToken, post.strCompany, post.strTerm, post.intIsActive, post.strCompanyType, 0, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = companyinformation });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}