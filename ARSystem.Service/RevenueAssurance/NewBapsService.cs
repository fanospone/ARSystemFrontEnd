using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Repositories.Repositories.RevenueAssurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service.RevenueAssurance
{
    public class NewBapsService
    {
        public int GetCountSoNumbList(string strToken, string strCompanyID, string strCustomerID, string strProductID, string strSoNumber, string strSiteID, string strTenantType, string mstRAActivityID,
             string strStipID, string strSiroID, DateTime? strStartBaukDoneDate, DateTime? strEndBaukDoneDate, string strSiteName, List<string> strSoNumberMultiple)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var dataSonumb = new mstNewBapsRepository(context);
            List<mstBaps> listData = new List<mstBaps>();
            
            try
            {
                string strWhereClause = " ActivityID = " + mstRAActivityID + " AND ";
                if (strSoNumberMultiple != null && strSoNumberMultiple.Any())
                {
                    strWhereClause += "(";
                    for (int i = 0; i < strSoNumberMultiple.Count(); i++)
                    {

                        strWhereClause += "SoNumber like '%" + strSoNumberMultiple[i] + "%' ";
                        strWhereClause += (i == (strSoNumberMultiple.Count() - 1)) ? "" : "OR ";
                    }
                    strWhereClause += ") AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSoNumber))
                {
                    strWhereClause += "SoNumber = '" + strSoNumber.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCustomerID))
                {
                    strWhereClause += "CustomerID = '" + strCustomerID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strProductID))
                {
                    strWhereClause += "ProductID = '" + strProductID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCompanyID))
                {
                    strWhereClause += "CompanyID = '" + strCompanyID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteID))
                {
                    strWhereClause += "SiteID = '" + strSiteID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrEmpty(strTenantType))
                {
                    if (strTenantType.ToLower() == "baps")
                        strWhereClause += "ID IS NOT NULL AND ";
                    else if (strTenantType.ToLower() == "validation")
                        strWhereClause += "ID IS NULL AND ";
                    else
                        strWhereClause += "DoneDate IS NULL AND ";
                }
                if(mstRAActivityID == "19") //BAPS INPUT
                {
                    strWhereClause += "ID IS NOT NULL AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStipID))
                {
                    strWhereClause += "StipID = '" + strStipID.TrimStart().TrimEnd() + "' AND ";
                }


                if (!string.IsNullOrWhiteSpace(strSiroID))
                {
                    strWhereClause += "SIRO = '" + strSiroID.TrimStart().TrimEnd() + "' AND ";
                }

                if (strStartBaukDoneDate != null)
                {
                    strWhereClause += "BaukDoneDate >= '" + strStartBaukDoneDate + "' AND ";
                }

                if (strEndBaukDoneDate != null)
                {
                    strWhereClause += "BaukDoneDate <= '" + strEndBaukDoneDate + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + strSiteName + "%' AND ";

                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return dataSonumb.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                listData.Add(new mstBaps((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmNewBapsData> GetSoNumbList(string strToken, string strCompanyID, string strCustomerID, string strProductID, string strSoNumber, string strSiteID, string strTenantType, string mstRAActivityID,
             string strStipID, string strSiroID, DateTime? strStartBaukDoneDate, DateTime? strEndBaukDoneDate, string strSiteName, List<string> strSoNumberMultiple, string strOrderBy = "", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var dataSonumb = new mstNewBapsRepository(context);
            List<vmNewBapsData> listData = new List<vmNewBapsData>();

            
            try
            {
                string strWhereClause = " ActivityID = " + mstRAActivityID + " AND ";
                if (strSoNumberMultiple != null && strSoNumberMultiple.Any())
                {
                    strWhereClause += "(";
                    for (int i = 0; i < strSoNumberMultiple.Count(); i++)
                    {

                        strWhereClause += "SoNumber like '%" + strSoNumberMultiple[i] + "%' ";
                        strWhereClause += (i == (strSoNumberMultiple.Count() - 1)) ? "" : "OR ";
                    }
                    strWhereClause += ") AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSoNumber))
                {
                    strWhereClause += "SoNumber LIKE '%" + strSoNumber.TrimStart().TrimEnd() + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCustomerID))
                {
                    strWhereClause += "CustomerID = '" + strCustomerID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strProductID))
                {
                    strWhereClause += "ProductID = '" + strProductID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCompanyID))
                {
                    strWhereClause += "CompanyID = '" + strCompanyID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteID))
                {
                    strWhereClause += "SiteID = '" + strSiteID.TrimStart().TrimEnd() + "' AND ";
                }
                if (!string.IsNullOrEmpty(strTenantType))
                {
                    if (strTenantType.ToLower() == "baps")
                        strWhereClause += "ID IS NOT NULL AND ";
                    else if (strTenantType.ToLower() == "validation")
                        strWhereClause += "ID IS NULL AND ";
                    else
                        strWhereClause += "DoneDate IS NULL AND ";
                }
                if (mstRAActivityID == "19") //BAPS INPUT
                {
                    strWhereClause += "ID IS NOT NULL AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStipID))
                {
                    strWhereClause += "StipID = '" + strStipID.TrimStart().TrimEnd() + "' AND ";
                }


                if (!string.IsNullOrWhiteSpace(strSiroID))
                {
                    strWhereClause += "SIRO = '" + strSiroID.TrimStart().TrimEnd() + "' AND ";
                }

                if (strStartBaukDoneDate != null)
                {
                    strWhereClause += "BaukDoneDate >= '" + strStartBaukDoneDate + "' AND ";
                }

                if (strEndBaukDoneDate != null)
                {
                    strWhereClause += "BaukDoneDate <= '" + strEndBaukDoneDate + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + strSiteName + "%' AND ";

                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listData = dataSonumb.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);

                return listData;
            }
            catch (Exception ex)
            {
                listData.Add(new vmNewBapsData((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return listData;
            }
            finally
            {
                context.Dispose();
            }
        }

        //public trxRANewBapsActivity UpdateNewBapsActivity(string strToken, trxRANewBapsActivity post)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new trxRANewBapsActivityRepository(context);
        //    try
        //    {
        //        trxRANewBapsActivity result = new trxRANewBapsActivity();
        //        result = repo.GetByPK(0, post.SoNumber, post.SIRO, post.BapsType, post.PowerType);

        //        if (result == null)
        //        {

        //            post.CreatedBy = userCredential.UserID;
        //            post.CreatedDate = DateTime.Now;
        //            model = repo.Create(post);
        //        }
        //        else
        //        {

        //            post.UpdatedBy = userCredential.UserID;
        //            post.UpdatedDate = DateTime.Now;
        //            model = repo.Update(post);
        //        }

        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new trxRANewBapsActivity((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NewBapsCheckingDocumentService", "GetCheckDocList", userCredential.UserID));
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}


        public mstBaps ApproveNewBAPSInput(string userID, mstBaps post, trxRANewBapsActivity act, trxRAUploadDocument upload, int SplitBill)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var data = new vmMstBapsRepository(context);
            var trxRAUploadDocumentRepo = new trxRAUploadDocumentRepository(context);
            var RARepo = new RevenueAssuranceRepository(context);

            var logs = new trxRALogActivityRepository(context);
            var logact = new List<trxRALogActivity>();
            mstBaps model = new mstBaps();

            var actrepo = new trxRANewBapsActivityRepository(context);
            var actdata = actrepo.GetByPK(0, act.SoNumber, act.SIRO, act.BapsType, act.PowerType);
            var trxBapsDataExists = RARepo.CheckBapsData(actdata.ID);

            if (trxBapsDataExists != null && trxBapsDataExists.trxBapsDataId > 0)
            {
                model.ErrorType = 1;
                model.ErrorMessage = "Data Already Exists";
                return model;
            }

            try
            {
                post.UpdatedBy = userID;
                upload.CreatedBy = userID;
                upload.CreatedDate = DateTime.Now;
                var result = data.BapsDone(post, SplitBill);

                if (string.IsNullOrEmpty(result.ErrorMessage))
                {
                    act.CheckListDoc = true;
                    act.BapsValidation = true;
                    act.BapsPrint = true;
                    act.BapsInput = true;
                    var BAPSAct = UpdateNewBapsActivity(userID, act);
                    upload.TransactionID = post.ID;
                    if (upload.FilePath != null && upload.FilePath != "")
                    {
                        var uplresult = trxRAUploadDocumentRepo.Create(upload).ID;
                    }

                    logact.Add(new trxRALogActivity
                    {
                        Label = "BAPS Submit",
                        LogDate = DateTime.Now,
                        LogState = true,
                        mstRAActivityID = act.mstRAActivityID,
                        Remarks = "BAPS Submit : " + post.SoNumber,
                        TransactionID = post.ID,
                        UserID = userID
                    });

                    logs.CreateBulky(logact);

                    if (string.IsNullOrEmpty(BAPSAct.ErrorMessage))
                        model = result;
                }
            }
            catch (Exception ex)
            {
                model = (new mstBaps((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NewBapsInputService", "SubmitBapsInput", userID)));
                return model;
            }
            finally
            {
                context.Dispose();
            }
            return model;
        }

        public trxRASplitNewBaps SplitBillNewBaps(string userID, trxRASplitNewBaps post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var data = new trxRASplitNewBapsRepository(context);

            trxRASplitNewBaps model = new trxRASplitNewBaps();
            try
            {
                post.CreatedBy = userID;
                post.CreatedDate = DateTime.Now;
                return data.Create(post);
            }
            catch (Exception ex)
            {
                model = (new trxRASplitNewBaps((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NewBapsInputService", "SplitBillNewBaps", userID)));
                return model;
            }
            finally
            {
                context.Dispose();
            }
        }


        public trxRANewBapsActivity UpdateNewBapsActivity(string userID, trxRANewBapsActivity post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new trxRANewBapsActivityRepository(context);
            trxRANewBapsActivity model = new trxRANewBapsActivity();
     
            try
            {
                trxRANewBapsActivity result = new trxRANewBapsActivity();
                result = repo.GetByPK(0, post.SoNumber, post.SIRO, post.BapsType, post.PowerType);

                if (result == null)
                {

                    post.CreatedBy = userID;
                    post.CreatedDate = DateTime.Now;
                    model = repo.Create(post);
                }
                else
                {

                    post.UpdatedBy = userID;
                    post.UpdatedDate = DateTime.Now;
                    model = repo.Update(post);
                }

                return model;
            }
            catch (Exception ex)
            {
                return new trxRANewBapsActivity((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NewBapsCheckingDocumentService", "GetCheckDocList", userID));
            }
            finally
            {
                context.Dispose();
            }
        }


    }
}
