using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;
using System.Configuration;
using System.IO;
using System.Web;


namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/CheckingDoc")]
    public class ApiNewBapsCheckingDocumentController : ApiController
    {
        [HttpPost, Route("getSonumbList")]
        public async Task<IHttpActionResult> GetSonumbList(PostNewBapsCheckingDocument post)
        {
            var listData = new List<vwRANewCheckingDocumentBAUK>();
            var param = new vwRANewCheckingDocumentBAUK();
            int intTotalRecord = 0;
            try
            {
                //List<ARSystemService.vmNewBapsData> model = new List<ARSystemService.vmNewBapsData>();
                //int intTotalRecord = 0;
                //using (var client = new ARSystemService.NewBapsServiceClient())
                //{
                //    intTotalRecord = client.GetCountSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerID, post.strProductId, post.strSoNumber, post.strSiteID, "-", post.mstRAActivityID);

                //    string strOrderBy = "";
                //    if (post.order != null)
                //        if (post.columns[post.order[0].column].data != "0")
                //            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                //    model = client.GetSoNumbList(UserManager.User.UserToken, post.strCompanyId, post.strCustomerID, post.strProductId, post.strSoNumber, post.strSiteID, "-", post.mstRAActivityID, strOrderBy, post.start, post.length).ToList();

                //    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = model });
                //}
                var service = new DocumentCheckingService();
                param.CustomerID = post.strCustomerID;
                param.ActivityID = int.Parse(post.mstRAActivityID);
                param.SoNumber = post.strSoNumber;
                param.ProductID = int.Parse(post.strProductId == null ? "0": post.strProductId);
                param.CompanyID = post.strCompanyId;
                param.SiteID = post.strSiteID;
                intTotalRecord =  service.RANewCheckingDocumentBAUKCount(param);
                listData = await service.RANewCheckingDocumentBAUKList(param, post.start, post.length);
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
            catch (Exception ex)
            {
                listData.Add(new vwRANewCheckingDocumentBAUK { ErrorMessage = ex.Message, ErrorType = 1 });
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
        }

        [HttpPost, Route("getCheckDocList")]
        public IHttpActionResult GetCheckDoList(PostNewBapsCheckingDocument post)
        {
            try
            {
                List<ARSystemService.vwDataNewBapsCheckingDoc> list = new List<ARSystemService.vwDataNewBapsCheckingDoc>();
                using (var client = new ARSystemService.NewBapsCheckingDocumentServiceClient())
                {
                    int intTotalRecord = 0;
                    list = client.GetCheckDocList(UserManager.User.UserToken, post.strCustomerID, post.strSoNumber, post.strSiteID).ToList();
                    intTotalRecord = list.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitTrxCheckDoc")]
        public async Task<IHttpActionResult> SubmitTrxCheckDoc(PostNewBapsCheckingDocument post)
        {
            try
            {
                ARSystemService.trxDocProject model = new ARSystemService.trxDocProject();
                using (var client = new ARSystemService.NewBapsCheckingDocumentServiceClient())
                {
                    model = client.UpdateDocProject(UserManager.User.UserToken, post.strDocID, post.strBapsChecked, post.strSoNumber, post.mstRAActivityID, post.Remarks);

                    if (model.ErrorType == 0)
                    {
                        var service = new DocumentCheckingService();
                        var result = new trxRADocumentCheckingType();
                        var postData = new List<trxRADocumentCheckingType>();
                        vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                        for (int i = 0; i < post.DocumentCheck.Length; i++)
                        {
                            postData.Add(new trxRADocumentCheckingType
                            {
                                SoNumber = post.DocumentCheck[i].SoNumber,
                                DocumentId = post.DocumentCheck[i].DocumentId,
                                RAActivity = int.Parse(post.mstRAActivityID),
                                CheckType = post.DocumentCheck[i].CheckListType,
                                CheckTypeName = post.DocumentCheck[i].CheckListName,
                                Remark = post.DocumentCheck[i].Remark,
                                CreatedBy = userCredential.UserID,
                                CreatedDate = DateTime.Now
                            });
                        }
                        result = await service.CreateDocumentCheckingType(postData);

                        var trxDoc = new trxReceiveDocumentBAUK();
                        trxDoc.SONumber = post.strSoNumber;
                        trxDoc.SiteID = post.strSiteID;
                        trxDoc.SiteName = post.vSiteName;
                        trxDoc.CustomerName = post.strCustomerID;
                        trxDoc.Remarks = post.vRemarks;
                        trxDoc.StatusDocument = post.vAction;
                        trxDoc.PICReceive = post.vPICReceive;
                        trxDoc.ReceiveDate = post.vReceiveDate;
                        trxDoc.CreatedDate = System.DateTime.Now;

                        SubmitTrx(trxDoc);
                    }
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        public trxReceiveDocumentBAUK SubmitTrx(trxReceiveDocumentBAUK trxDoc)
        {
            var result = new trxReceiveDocumentBAUK();

            try
            {
                var docBAUKService = new ReceiveDocumentBAUKService();

                result = docBAUKService.Savetrx(UserManager.User.UserToken, trxDoc);

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        [HttpGet, Route("getDocumentSupport")]
        public async Task<IHttpActionResult> GetDocSupport(string companyId, string siteId)
        {
            var list = new List<DocumentSupport>();
            try
            {
                string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocSupportSite"].ToString() + @"\" + companyId + @"\" + siteId );
                string[] filesPath = Directory.GetFiles(path);

                if (filesPath.Length > 0)
                {
                    for (int i = 0; i < filesPath.Length; i++)
                    {
                        string fileName = Path.GetFileName(filesPath[i]);
                        list.Add(new DocumentSupport { No = (i + 1), DocumentName = fileName, UrlPath = @"\" + companyId + @"\" + siteId + @"\" + fileName });
                    }
                }
                return Ok(new { data = list, recordsTotal= filesPath.Count(), recordsFiltered = filesPath.Count() });
                //return Ok(list);
            }
            catch( Exception ex)
            {
                list.Add(new DocumentSupport { ErrorType = 1, ErrorMessage = ex.Message, DocumentName="", No=0 });
                return Ok(new { data = list, recordsTotal = 0, recordsFiltered = 0 });
            }
        }



        //[HttpPost, Route("grid")]
        //public IHttpActionResult GetCheckDocToList(PostNewBapsCheckingDocument post)
        //{
        //    try
        //    {
        //        int intTotalRecord = 0;
        //        List<ARSystemService.vwNewBapsCheckingDocument> checkDoc = new List<ARSystemService.vwNewBapsCheckingDocument>();
        //        using (var client = new ARSystemService.NewBapsCheckingDocumentServiceClient())
        //        {
        //            intTotalRecord = client.GetCheckDocCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strProductId, post.strGroupBy);
        //            checkDoc = client.GetCheckDocTodoList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strProductId, post.strGroupBy, post.start, post.length).ToList();
        //            return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = checkDoc });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        //[HttpPost, Route("getTrxCheckDoc")]
        //public IHttpActionResult GetTrxCheckDoc(PostNewBapsTrxCheckingDocument post)
        //{
        //    try
        //    {
        //        List<ARSystemService.vwDataNewBapsCheckingDoc> trxCheckDoc = new List<ARSystemService.vmTrxCheckingDocument>();
        //        using (var client = new ARSystemService.NewBapsCheckingDocumentServiceClient())
        //        {
        //            trxCheckDoc = client.GetCheckDocList(UserManager.User.UserToken, post.strSoNumber, post.strSiteId, post.strCompanyId, post.strOperatorId, "BAPS").ToList();
        //            return Ok(new { draw = post.draw, data = trxCheckDoc });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}
        //public IHttpActionResult CheckValidationSubmitTrx(PostNewBapsTrxCheckingDocument post)
        //{
        //    try
        //    {
        //        List<ARSystemService.vmTrxCheckingDocument> trxCheckDoc = new List<ARSystemService.vmTrxCheckingDocument>();
        //        using (var client = new ARSystemService.NewBapsCheckingDocumentServiceClient())
        //        {
        //            trxCheckDoc = client.GetTrxCheckDocTodoList(UserManager.User.UserToken, post.strSoNumber, post.strSiteId, post.strCompanyId, post.strOperatorId, "BAPS").ToList();
        //            int rowCount = 0;
        //            for(int i=0; i <= trxCheckDoc.Count(); i++)
        //            {
        //                rowCount += 1;
        //            }
        //            //int rowCheckProjectCount = 0;
        //            //for (int i = 0; i < trxCheckDoc.Count(); i++)
        //            //{
        //            //    trxCheckDoc.ChekedProject(i)
        //            //    if ()
        //            //}
        //            return Ok(new { draw = post.draw, data = trxCheckDoc });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

    }

    public class DocumentSupport
    {
        public int No { get; set; }
        public string DocumentName { get; set; }
        public string UrlPath { get; set; }
        public int ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}