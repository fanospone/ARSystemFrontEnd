using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Linq;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using ARSystem.Domain.Repositories.Repositories.RevenueAssurance;
using ARSystem.Domain.Models.Models;

namespace ARSystem.Service
{
    public class BapsValidationService
    {
        public async Task<List<vwRATaskTodoCreateBAPS>> RATaskTodoCreateBAPSList()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRATaskTodoCreateBAPSRepository(context);
            var result = new List<vwRATaskTodoCreateBAPS>();
            try
            {
                result= repo.GetList("");

                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vwRATaskTodoCreateBAPS
                {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "BapsValidationService", "vwRATaskTodoCreateBAPSList", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<vwRANewBapsValidationDefaultPrice> DefaultPrice(vwRANewBapsValidationDefaultPrice param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRANewBapsValidationDefaultPriceRepository(context);
            var result = new vwRANewBapsValidationDefaultPrice();
            try
            {
                string whereClause = "SONumber = '" + param.SONumber + "'";
                var listData = new List<vwRANewBapsValidationDefaultPrice>();
                listData = repo.GetList(whereClause, "");

                if (listData.Count >= 0)
                {
                    result = listData[0];
                }

                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = Helper.logError(ex.Message.ToString(), "BapsValidationService", "DefaultPrice", "");
                result.ErrorType = (int)Helper.ErrorType.Error;
               
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public vmBAPSValidationUpdate UpdateBAPSValidationInfo(string strToken, string strSONumber, string strFieldName, string strFieldValue)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var uow = context.CreateUnitOfWork();
            var ssrDropFORepo = new trxSSRDropFORepository(context);

            try
            {
                DateTime dtNow = Helper.GetDateTimeNow();
                vmBAPSValidationUpdate bapsValidationUpdate = new vmBAPSValidationUpdate();

                if (strFieldName == "DropFODistance")
                {
                    trxSSRDropFO ssrDropFO = ssrDropFORepo.GetList("SONumber = '" + strSONumber + "'").FirstOrDefault();
                    if (ssrDropFO != null)
                    {
                        ssrDropFO.Distance = int.Parse(strFieldValue);
                        ssrDropFORepo.Update(ssrDropFO);
                    }
                    else
                    {
                        bapsValidationUpdate.ErrorType = (int)Helper.ErrorType.Info;
                        bapsValidationUpdate.ErrorMessage = "SSR Drop FO untuk SO Number " + strSONumber + " tidak ditemukan";
                    }
                }

                uow.SaveChanges();

                return bapsValidationUpdate;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new vmBAPSValidationUpdate((int)Helper.ErrorType.Error, ex.Message.ToString());
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmNewBapsData> GetSoNumbList(string strToken, string strCompanyID, string strCustomerID, string strProductID, string strSoNumber, 
                string strSiteID, string strTenantType, string mstRAActivityID, string strStipID, string strSiroID, DateTime? strStartBaukDoneDate, DateTime? strEndBaukDoneDate,
                string strOrderBy = "", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var dataSonumb = new vmMstNewBapsRepository(context);
            List<vmNewBapsData> listData = new List<vmNewBapsData>();

            try
            {
                string strWhereClause = " ActivityID = " + mstRAActivityID + " AND ";

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

                if (!string.IsNullOrEmpty(strTenantType))
                {
                    if (strTenantType.ToLower() == "baps")
                        strWhereClause += "ID IS NOT NULL AND ";
                    else if (strTenantType.ToLower() == "validation")
                        strWhereClause += "ID IS NULL AND ";
                    else
                        strWhereClause += "DoneDate IS NULL AND ";
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
        public List<vmNewBapsData> GetBapsDataValidationList(string strToken, string strCompanyID, string strCustomerID, string strProductID, string strSoNumber, string strSiteID, string strTenantType, string mstRAActivityID, int BaukDoneYear,
            string strStipID, string strSiroID, DateTime? strStartBaukDoneDate, DateTime? strEndBaukDoneDate, string strBapsTypeID, string strSiteName, List<string> strSoNumberMultiple, string strOrderBy = "", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var dataSonumb = new vmMstNewBapsRepository(context);
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
                if (!string.IsNullOrWhiteSpace(strStipID))
                {
                    strWhereClause += "StipID = '" + strStipID.TrimStart().TrimEnd() + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(strBapsTypeID))
                {
                    //except 5 
                    strWhereClause += "PoType != '" + strBapsTypeID.TrimStart().TrimEnd() + "' AND ";

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
                if (!string.IsNullOrEmpty(strTenantType))
                {
                    if (strTenantType.ToLower() == "baps")
                        strWhereClause += "ID IS NOT NULL AND ";
                    else if (strTenantType.ToLower() == "validation" || strTenantType.ToLower() == "validation-bulky")
                        strWhereClause += "ID IS NULL AND ";
                    else
                        strWhereClause += "DoneDate IS NULL AND ";
                }

                if (BaukDoneYear != 0)
                {
                    strWhereClause += "YEAR(BaukDate) = " + BaukDoneYear + " AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + strSiteName + "%' AND ";

                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listData = dataSonumb.pGetPagedValidation(strWhereClause, strOrderBy, intRowSkip, intPageSize);

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

        public int GetCountSoNumbList(string strToken, string strCompanyID, string strCustomerID, string strProductID, string strSoNumber, string strSiteID, string strTenantType, string mstRAActivityID,
            string strStipID, string strSiroID, DateTime? strStartBaukDoneDate, DateTime? strEndBaukDoneDate)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var dataSonumb = new vmMstNewBapsRepository(context);
            List<vmNewBapsData> listData = new List<vmNewBapsData>();
       
            try
            {
                string strWhereClause = " ActivityID = " + mstRAActivityID + " AND ";
               
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

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return dataSonumb.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                listData.Add(new vmNewBapsData((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        public int GetCountBapsDataValidationList(string strToken, string strCompanyID, string strCustomerID, string strProductID, string strSoNumber, string strSiteID, string strTenantType, string mstRAActivityID, int BaukDoneYear,
            string strStipID, string strSiroID, DateTime? strStartBaukDoneDate, DateTime? strEndBaukDoneDate, string strBapsTypeID, string strSiteName, List<string> strSoNumberMultiple)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var dataSonumb = new vmMstNewBapsRepository(context);
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
                    strWhereClause += "SoNumber like '%" + strSoNumber.TrimStart().TrimEnd() + "%' AND ";
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
                if (!string.IsNullOrWhiteSpace(strBapsTypeID))
                {
                    strWhereClause += "PoType != '" + strBapsTypeID.TrimStart().TrimEnd() + "' AND ";

                }
                if (strStartBaukDoneDate != null)
                {
                    strWhereClause += "BaukDate >= '" + strStartBaukDoneDate + "' AND ";
                }
                if (strEndBaukDoneDate != null)
                {
                    strWhereClause += "BaukDate <= '" + strEndBaukDoneDate + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + strSiteName + "%' AND ";
                }
                if (!string.IsNullOrEmpty(strTenantType))
                {
                    if (strTenantType.ToLower() == "baps")
                        strWhereClause += "ID IS NOT NULL AND ";
                    else if (strTenantType.ToLower() == "validation" || strTenantType.ToLower() == "validation-bulky")
                        strWhereClause += "ID IS NULL AND ";
                    else
                        strWhereClause += "DoneDate IS NULL AND ";
                }

                if (BaukDoneYear != 0)
                {
                    strWhereClause += "YEAR(BaukDate) = " + BaukDoneYear + " AND ";
                }


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return dataSonumb.pGetCountValidation(strWhereClause);
            }
            catch (Exception ex)
            {
                listData.Add(new vmNewBapsData((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }



        #region migration from backend for submit validation

        public vmMstBapsBulky BAPSValidationBulkySubmit(string userID, vmMstBapsBulky post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var data = new vmMstBapsRepository(context);
            vmMstBapsBulky model = new vmMstBapsBulky();
            var logs = new trxRALogActivityRepository(context);
            var logact = new List<trxRALogActivity>();

            try
            {
                string strAction = "BAPSVALIDATION";

                List<mstRACustomerInvoice> CustInvoiceList = new List<mstRACustomerInvoice>();
                foreach (var bapsValidation in post.ListMstBaps)
                {
                    bapsValidation.StartBapsDate = post.StartBapsDate;
                    bapsValidation.EndBapsDate = post.EndBapsDate;
                    bapsValidation.StartEffectiveDate = post.StartEffectiveDate;
                    bapsValidation.EndEffectiveDate = post.EndEffectiveDate;
                    //bapsValidation.mstCustomerInvoiceID =  null;
                    bapsValidation.CompanyInvoiceId = post.CompanyInvoiceId;
                    bapsValidation.Remark = post.Remark;
                    bapsValidation.BaseLeaseCurrency = post.BaseLeaseCurrency;
                    bapsValidation.BaseLeasePrice = post.BaseLeasePrice;
                    bapsValidation.ServiceCurrency = post.ServiceCurrency;
                    bapsValidation.ServicePrice = post.ServicePrice;
                    bapsValidation.mstBapsTypeID = 5; //fixed TOWER

                }
                data.UploadBAPSValidation(userID, post.ListMstBaps);
                //var invoiceTypeID = new mstRACustomerInvoiceRepository(context);
                //mstRACustomerInvoice CustInvoice = new mstRACustomerInvoice();

                //foreach (var bapsValidation in post.ListMstBaps)
                //{
                //    var CheckBaps = data.CheckBaps(bapsValidation.SoNumber, post.StipSiro, post.mstBapsTypeID, Convert.ToInt32(post.PowerTypeID));

                //    bapsValidation.StartBapsDate = post.StartBapsDate;
                //    bapsValidation.EndBapsDate = post.EndBapsDate;
                //    bapsValidation.StartEffectiveDate = post.StartEffectiveDate;
                //    bapsValidation.EndEffectiveDate = post.EndEffectiveDate;
                   // CustInvoice = invoiceTypeID.GetInvoiceID(post.CustomerId, post.CompanyInvoiceId);
                //    if(CustInvoice != null)
                //    {
                //        bapsValidation.mstCustomerInvoiceID = CustInvoice.ID;
                //    }
                //    bapsValidation.CompanyInvoiceId = post.CompanyInvoiceId;
                //    bapsValidation.Remark = post.Remark;
                //    bapsValidation.BaseLeaseCurrency = post.BaseLeaseCurrency;
                //    bapsValidation.BaseLeasePrice = post.BaseLeasePrice;
                //    bapsValidation.ServiceCurrency = post.ServiceCurrency;
                //    bapsValidation.ServicePrice = post.ServicePrice;
                //    bapsValidation.mstBapsTypeID = 5; //fixed TOWER
                //    //bapsValidation.pr = post.ServicePrice;

                //    if (bapsValidation.ID == 0 && CheckBaps == null)
                //    {
                //        post.CreatedBy = userID;
                //        var res = data.Create(bapsValidation, strAction);
                //        trxRANewBapsActivity mdlAct = new trxRANewBapsActivity();
                //        mdlAct.BapsInput = false;
                //        mdlAct.BapsPrint = false;
                //        mdlAct.BapsValidation = true;
                //        mdlAct.CreatedBy = userID;
                //        mdlAct.UpdatedBy = userID;
                //        mdlAct.SoNumber = bapsValidation.SoNumber;
                //        mdlAct.CheckListDoc = true;
                //        mdlAct.SIRO = bapsValidation.StipSiro;
                //        mdlAct.mstRAActivityID = 19;//auto approved by system jadi BAPS SUBMIT
                //        mdlAct.BapsType = 5; //tower
                //        mdlAct = ApproveBAPSValidation(userID, mdlAct);
                        
                //        logact.Add(new trxRALogActivity
                //        {
                //            Label = "BAPS Created",
                //            LogDate = DateTime.Now,
                //            LogState = true,
                //            mstRAActivityID = 19, //auto approved by system jadi BAPS SUBMIT
                //            Remarks = "BAPS Created : " + bapsValidation.SoNumber,
                //            TransactionID = res.ID,
                //            UserID = userID
                //        });

                //        logs.CreateBulky(logact);
                //    }
                //    else
                //    {
                //        if (bapsValidation.ID == 0)
                //        {
                //            bapsValidation.ID = CheckBaps.ID;
                //        }

                //        post.UpdatedBy = userID;
                //        var res = data.Update(bapsValidation, strAction);

                //        logact.Add(new trxRALogActivity
                //        {
                //            Label = "BAPS Update",
                //            LogDate = DateTime.Now,
                //            LogState = true,
                //            mstRAActivityID = 19,
                //            Remarks = "BAPS Update : " + bapsValidation.SoNumber,
                //            TransactionID = bapsValidation.ID,
                //            UserID = userID
                //        });

                //        logs.CreateBulky(logact);
                //    }
                //}

            }
            catch (Exception ex)
            {
                model = (new vmMstBapsBulky((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NewBapsValidationBulky", "SubmitBapsValidationBulky", userID)));
                return model;
            }
            finally
            {
                context.Dispose();
            }
            return model;
        }
        public trxRANewBapsActivity ApproveBAPSValidation(string userID, trxRANewBapsActivity post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new trxRANewBapsActivityRepository(context);
            trxRANewBapsActivity model = new trxRANewBapsActivity();
            var logs = new trxRALogActivityRepository(context);
            var logact = new List<trxRALogActivity>();

            try
            {
                trxRANewBapsActivity result = new trxRANewBapsActivity();
                result = repo.GetByPK(0, post.SoNumber, post.SIRO, post.BapsType, post.PowerType);
                post.BapsInput = false;
                post.BapsPrint = false;
                post.BapsValidation = true;
                post.CheckListDoc = true;

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

                    logact.Add(new trxRALogActivity
                    {
                        Label = "BAPS Validation",
                        LogDate = DateTime.Now,
                        LogState = true,
                        mstRAActivityID = post.mstRAActivityID,
                        Remarks = "BAPS Validation : " + post.SoNumber,
                        TransactionID = post.ID,
                        UserID = userID
                    });

                    logs.CreateBulky(logact);
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


        #endregion



        public List<mstBapsType> GetBapsTypeToList()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var BapsTypeRepo = new mstBapsTypeRepository(context);
            List<mstBapsType> listBapsType = new List<mstBapsType>();

            try
            {
                string strWhereClause = String.Empty;
                string strOrderBy = String.Empty;
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listBapsType = BapsTypeRepo.GetList(strWhereClause, strOrderBy);


                return listBapsType;
            }
            catch (Exception ex)
            {
                listBapsType.Add(new mstBapsType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstDataSourceService", "GetBapsTypeToListTSEL", String.Empty)));
                return listBapsType;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
