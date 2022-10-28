using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    public class BapsDataService
    {
        public int GetVwChangePPHFinalCount(string UserID, vwChangePPHFinal vwChangePPHFinal)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var vwChangePPHFinalRepo = new vwChangePPHFinalRepository(context);

            try
            {
                string strWhereClause = "1=1";

                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.InvoiceNumber))
                {
                    strWhereClause += "AND InvoiceNumber LIKE '%" + vwChangePPHFinal.InvoiceNumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.SONumber))
                {
                    strWhereClause += "AND SONumber LIKE '%" + vwChangePPHFinal.SONumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.BAPSNumber))
                {
                    strWhereClause += "AND BAPSNumber LIKE '%" + vwChangePPHFinal.BAPSNumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.PONumber))
                {
                    strWhereClause += "AND PONumber LIKE '%" + vwChangePPHFinal.PONumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.OperatorID))
                {
                    strWhereClause += "AND OperatorID = '" + vwChangePPHFinal.OperatorID + "'";
                }
                if (vwChangePPHFinal.StartDateInvoice != null && vwChangePPHFinal.StartDateInvoice != DateTime.MinValue)
                {
                    strWhereClause += "AND StartDateInvoice = '" + vwChangePPHFinal.StartDateInvoice.Value.ToString("yyyy-MM-dd") + "'";
                }
                if (vwChangePPHFinal.EndDateInvoice != null && vwChangePPHFinal.EndDateInvoice != DateTime.MinValue)
                {
                    strWhereClause += "AND EndDateInvoice = '" + vwChangePPHFinal.EndDateInvoice.Value.ToString("yyyy-MM-dd") + "'";
                }

                if (vwChangePPHFinal.CompanyID == Constants.CompanyCode.PKP)
                {
                    strWhereClause += "AND CompanyID='" + Constants.CompanyCode.PKP + "'";
                }

                return vwChangePPHFinalRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "BapsDataService", "GetVwChangePPHFinalCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwChangePPHFinal> GetVwChangePPHFinalToList(string UserID, vwChangePPHFinal vwChangePPHFinal, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var vwChangePPHFinalRepo = new vwChangePPHFinalRepository(context);
            List<vwChangePPHFinal> listVwPPHFinal = new List<vwChangePPHFinal>();

            try
            {
                string strWhereClause = "1=1";

                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.InvoiceNumber))
                {
                    strWhereClause += "AND InvoiceNumber LIKE '%" + vwChangePPHFinal.InvoiceNumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.SONumber))
                {
                    strWhereClause += "AND SONumber LIKE '%" + vwChangePPHFinal.SONumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.BAPSNumber))
                {
                    strWhereClause += "AND BAPSNumber LIKE '%" + vwChangePPHFinal.BAPSNumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.PONumber))
                {
                    strWhereClause += "AND PONumber LIKE '%" + vwChangePPHFinal.PONumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(vwChangePPHFinal.OperatorID))
                {
                    strWhereClause += "AND OperatorID = '" + vwChangePPHFinal.OperatorID + "'";
                }
                if (vwChangePPHFinal.StartDateInvoice != null && vwChangePPHFinal.StartDateInvoice != DateTime.MinValue)
                {
                    strWhereClause += "AND StartDateInvoice = '" + vwChangePPHFinal.StartDateInvoice.Value.ToString("yyyy-MM-dd") + "'";
                }
                if (vwChangePPHFinal.EndDateInvoice != null && vwChangePPHFinal.EndDateInvoice != DateTime.MinValue)
                {
                    strWhereClause += "AND EndDateInvoice = '" + vwChangePPHFinal.EndDateInvoice.Value.ToString("yyyy-MM-dd") + "'";
                }

                if (vwChangePPHFinal.CompanyID == Constants.CompanyCode.PKP)
                {
                    strWhereClause += "AND CompanyID='" + Constants.CompanyCode.PKP + "'";
                }

                if (intPageSize > 0)
                    listVwPPHFinal = vwChangePPHFinalRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listVwPPHFinal = vwChangePPHFinalRepo.GetList(strWhereClause, strOrderBy);

                return listVwPPHFinal;
            }
            catch (Exception ex)
            {
                listVwPPHFinal.Add(new vwChangePPHFinal((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "BapsDataService", "GetVwChangePPHFinalToList", UserID)));
                return listVwPPHFinal;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int UpdatePPHFinal(string UserID, List<vwChangePPHFinal> vwChangePPHFinalList, int intPPHType)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new ChangePPHFinalRepository(context);

            try
            {
                foreach (vwChangePPHFinal vwChangePPHFinal in vwChangePPHFinalList)
                {
                    if (vwChangePPHFinal.IsPPHFinal != null)
                    {
                        int intIsPPHFinal = vwChangePPHFinal.IsPPHFinal.Value == true ? 1 : 0;
                        if (intIsPPHFinal != intPPHType)
                        {
                            repository.UpdatePPHFinal(UserID, vwChangePPHFinal, intPPHType);
                        }
                    }
                    else
                    {
                        repository.UpdatePPHFinal(UserID, vwChangePPHFinal, intPPHType);
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "BapsDataService", "UpdatePPHFinal", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
