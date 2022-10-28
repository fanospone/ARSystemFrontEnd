using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    public class DocumentCheckingService
    {
        public async Task<trxRADocumentCheckingType> CreateDocumentCheckingType(List<trxRADocumentCheckingType> postData)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new trxRADocumentCheckingTypeRepository(context);
            var result = new trxRADocumentCheckingType();
            try
            {
                repo.CreateBulky(postData);

                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = Helper.logError(ex.Message.ToString(), "DocumentCheckingService", "CreateDocumentCheckingType", postData[0].CreatedBy);
                result.ErrorType = (int)Helper.ErrorType.Error;
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<List<DocumentCheckingSummary>> GetSummaryDocumentCheck(string customerId, string siteId, string soNumber, string companyId, int productId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new DocumentCheckingSummaryRepository(context);
            var result = new List<DocumentCheckingSummary>();
            try
            {
                result = repo.GetList(customerId, siteId, soNumber, companyId, productId);

                return result;
            }
            catch (Exception ex)
            {

                result.Add(new DocumentCheckingSummary
                {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "DocumentCheckingService", "GetSummaryDocumentCheck", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<List<DocumentCheckingSummaryPerDoc>> GetSummaryDocumentCheckPerDoc(string customerId, string siteId, string soNumber, string companyId, int productId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new DocumentCheckingSummaryPerDocRepository(context);
            var result = new List<DocumentCheckingSummaryPerDoc>();
            try
            {
                result = repo.GetList(customerId, siteId, soNumber, companyId, productId);

                return result;
            }
            catch (Exception ex)
            {

                result.Add(new DocumentCheckingSummaryPerDoc
                {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "DocumentCheckingService", "GetSummaryDocumentCheckPerDoc", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<List<vwRANewCheckingDocumentBAUK>> RANewCheckingDocumentBAUKList(vwRANewCheckingDocumentBAUK postData, int RowSkip = 0, int PageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRANewCheckingDocumentBAUKRepository(context);
            var result = new List<vwRANewCheckingDocumentBAUK>();
            try
            {
                string whereClause = RANewCheckingDocumentBAUKWhereClause(postData);
                if (PageSize != 0)
                {
                    result = repo.GetPaged(whereClause, "", RowSkip, PageSize);
                }
                else
                {
                    result = repo.GetList();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vwRANewCheckingDocumentBAUK {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "DocumentCheckingService", "RANewCheckingDocumentBAUKList", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });
                
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int RANewCheckingDocumentBAUKCount(vwRANewCheckingDocumentBAUK postData)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRANewCheckingDocumentBAUKRepository(context);
            try
            {
                string whereClause = RANewCheckingDocumentBAUKWhereClause(postData);
                return repo.GetCount(whereClause);
            }
            catch 
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        private string RANewCheckingDocumentBAUKWhereClause(vwRANewCheckingDocumentBAUK postData)
        {
            string strWhereClause = " ActivityID = " + postData.ActivityID + " AND ";
            strWhereClause += "DoneDate IS NULL AND ";

            if (!string.IsNullOrWhiteSpace(postData.SoNumber))
            {
                strWhereClause += "SoNumber = '" + postData.SoNumber.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CustomerID))
            {
                strWhereClause += "CustomerID = '" + postData.CustomerID.TrimStart().TrimEnd() + "' AND ";
            }
            if (postData.ProductID!=0)
            {
                strWhereClause += "ProductID = '" + postData.ProductID + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CompanyID))
            {
                strWhereClause += "CompanyID = '" + postData.CompanyID.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.SiteID))
            {
                strWhereClause += "SiteID = '" + postData.SiteID.TrimStart().TrimEnd() + "' AND ";
            }
            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
            return strWhereClause;
        }

    }

}
