using System;
using System.Collections.Generic;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
   public class ISPInvoiceInformationService
    {
        //protected readonly HelperService helperService;
        //public FilingSystemService()
        //{
        //    helperService = new HelperService();
        //}
        public int GetISPInvoiceInformationCount(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwISPInvoiceInformationRepository(context);
            vwISPInvoiceInformation listRequestLibrary = new vwISPInvoiceInformation();
            try
            {
                return repo.GetCount(strWhereClause);
            }
            catch (Exception)
            {
                context.Dispose();
                return 0;
            }
        }
        public List<vwISPInvoiceInformation> GetISPInvoiceInformationList(string strWhereClause, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            List<vwISPInvoiceInformation> listRequest = new List<vwISPInvoiceInformation>();

            try
            {
                var repo = new vwISPInvoiceInformationRepository(context);
                if (pageSize < 0)
                    listRequest = repo.GetList(strWhereClause, strOrderBy);
                else
                    listRequest = repo.GetPaged(strWhereClause, strOrderBy, rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                listRequest.Add(new vwISPInvoiceInformation { ErrorMessage = ex.Message });
            }
            return listRequest;
        }

        public int GetISPSalesSytemCount(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwISPSalesSystemInformationRepository(context);
            vwISPSalesSystemInformation listRequestLibrary = new vwISPSalesSystemInformation();
            try
            {
                return repo.GetCount(strWhereClause);
            }
            catch (Exception)
            {
                context.Dispose();
                return 0;
            }
        }
        public List<vwISPSalesSystemInformation> GetISPSalesSystemList(string strWhereClause, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            List<vwISPSalesSystemInformation> listSales = new List<vwISPSalesSystemInformation>();

            try
            {
                var repo = new vwISPSalesSystemInformationRepository(context);
                if (pageSize < 0)
                    listSales = repo.GetList(strWhereClause, strOrderBy);
                else
                    listSales = repo.GetPaged(strWhereClause, strOrderBy, rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                listSales.Add(new vwISPSalesSystemInformation { ErrorMessage = ex.Message });
            }
            return listSales;
        }

        public List<vwISPSalesSystemInformation> GetSalesSystemList(string userID, string SONumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new vwISPSalesSystemInformationRepository(context);

            List<vwISPSalesSystemInformation> listSales = new List<vwISPSalesSystemInformation>();

            try
            {
                listSales = Repo.GetList("SONumber = '" + SONumber + "'");

                return listSales;
            }
            catch (Exception ex)
            {
                new vwISPSalesSystemInformation((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ISPInvoiceInformationService", "GetSalesSystemList", userID));
                return listSales;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<vwISPBAPSNewInformation> GetBAPSNewList(string userID, string SONumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new vwISPBAPSNewInformationRepository(context);

            List<vwISPBAPSNewInformation> listSales = new List<vwISPBAPSNewInformation>();

            try
            {
                listSales = Repo.GetList("SONumber = '" + SONumber + "'");

                return listSales;
            }
            catch (Exception ex)
            {
                new vwISPBAPSNewInformation((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ISPInvoiceInformationService", "GetBAPSNewList", userID));
                return listSales;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<vwISPInvoiceInformationDetail> GetInvoiceInformationDetailList(string userID, string SONumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new vwISPInvoiceInformationDetailRepository(context);

            List<vwISPInvoiceInformationDetail> listSales = new List<vwISPInvoiceInformationDetail>();

            try
            {
                listSales = Repo.GetList("SONumber = '" + SONumber + "'");

                return listSales;
            }
            catch (Exception ex)
            {
                new vwISPInvoiceInformationDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ISPInvoiceInformationService", "GetInvoiceInformationDetailList", userID));
                return listSales;
            }
            finally
            {
                context.Dispose();
            }
        }

    }
}
