using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Repositories.HTBGDWH01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.HTBGDWH01.TBGARSystem;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Service.ARSystem
{
    public class HistoryCNInvoiceService : BaseService
    {
        protected readonly DbContext context;
        protected readonly DbContext contextTrx;
        protected readonly DateTime _dtNow;
        private readonly vwHistoryCNInvoiceARDataRepository _vwHistoryCNInvoiceRepo;
        private readonly vwInvoiceHeaderListRepository _invHeaderRepo;
        private readonly idxFINInvoiceReplacementRepository _invReplaceRepo;

        

        public HistoryCNInvoiceService()
        {
            //_context = this.SetContext();
            contextTrx = new DbContext(Helper.GetConnection("ARSystem"));
            //context = new DbContext(Helper.GetConnection("HTBGDWH01_TBGARSystemWH"));
            context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            _dtNow = Helper.GetDateTimeNow();

            _vwHistoryCNInvoiceRepo = new vwHistoryCNInvoiceARDataRepository(context);
            _invHeaderRepo = new vwInvoiceHeaderListRepository(context);
            _invReplaceRepo = new idxFINInvoiceReplacementRepository(contextTrx);
            
        }

        public List<vwHistoryCNInvoiceARData> GetHistoryCNInvoiceARDataToList(string strToken, vmUserCredential userCredential, string strStartPeriod, string strEndPeriod, string strCompanyId, string strOperator, string InvNo, string TaxNo, string CNStatus, string InvoiceTypeId, int ProccessType, string ReplacementStatus, DateTime? ReplaceDate, string ReplaceInvoice,string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            //var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwHistoryCNInvoiceARData> listData = new List<vwHistoryCNInvoiceARData>();
            //ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(strToken);
            if (userCredential.ErrorType > 0)
            {
                listData.Add(new vwHistoryCNInvoiceARData(userCredential.ErrorType, userCredential.ErrorMessage));
                return listData;
            }

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "InvCompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "InvOperatorID = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "InvPrintDate >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "InvPrintDate <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(InvNo))
                {
                    strWhereClause += "InvNo LIKE '%" + InvNo + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(CNStatus))
                {
                    strWhereClause += "isCNApproved = '" + CNStatus + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(InvoiceTypeId))
                {
                    strWhereClause += "InvoiceTypeId = '" + InvoiceTypeId + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(ReplacementStatus))
                {
                    strWhereClause += "ReplacementStatus = '" + ReplacementStatus + "' AND ";
                }

                if (ReplaceDate != null)
                {
                    strWhereClause += $" ReplaceDate like '%{ReplaceDate}%' AND ";
                }
                //if (ReplaceDate != null)
                //{
                //    strWhereClause += $" CAST(ReplaceDate AS DATE)='{ReplaceDate?.ToSqlDate()}' AND ";
                //}

                if (!string.IsNullOrWhiteSpace(ReplaceInvoice))
                {
                    strWhereClause += "ReplaceInvoice = '" + ReplaceInvoice + "' AND ";
                }
                /*Edit by MTR*/
                if (ProccessType != 0)
                {
                    if (ProccessType == 1)
                        strWhereClause += "CNInfoCode = 'C2' AND ";
                    else
                        strWhereClause += "CNInfoCode = 'C1' AND ";
                }
                /*Edit by MTR*/

                strWhereClause += "InvNo IS NOT NULL AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";


                if (intPageSize > 0)
                    listData = _vwHistoryCNInvoiceRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listData = _vwHistoryCNInvoiceRepo.GetList(strWhereClause, strOrderBy);

                return listData;
            }
            catch (Exception ex)
            {
                listData.Add(new vwHistoryCNInvoiceARData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userCredential.UserID)));
                return listData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetHistoryCNInvoiceARDataCount(string strToken, vmUserCredential userCredential, string strStartPeriod, string strEndPeriod, string strCompanyId, string strOperator, string InvNo, string TaxNo, string CNStatus, string InvoiceTypeId, int ProccessType, string ReplacementStatus, DateTime? ReplaceDate, string ReplaceInvoice)
        {
            if (userCredential.ErrorType > 0)
                return 0;

            try
            {
                string strWhereClause = "";

                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "InvCompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "InvOperatorID = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "InvPrintDate >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "InvPrintDate <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(InvNo))
                {
                    strWhereClause += "InvNo LIKE '%" + InvNo + "%' AND ";
                }

                if (!string.IsNullOrWhiteSpace(CNStatus))
                {
                    strWhereClause += "isCNApproved = '" + CNStatus + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(InvoiceTypeId))
                {
                    strWhereClause += "InvoiceTypeId = '" + InvoiceTypeId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(ReplacementStatus))
                {
                    strWhereClause += "ReplacementStatus = '" + ReplacementStatus + "' AND ";
                }

                if (ReplaceDate != null)
                {
                    strWhereClause += $"CAST(ReplaceDate AS DATE)='{ReplaceDate?.ToSqlDate()}' AND ";
                }

                if (!string.IsNullOrWhiteSpace(ReplaceInvoice))
                {
                    strWhereClause += "ReplaceInvoice = '" + ReplaceInvoice + "' AND ";
                }

                /*Edit by MTR*/
                if (ProccessType != 0)
                {
                    if (ProccessType == 1)
                        strWhereClause += "CNInfoCode = 'C2' AND ";
                    else
                        strWhereClause += "CNInfoCode = 'C1' AND ";
                }
                /*Edit by MTR*/
                strWhereClause += "InvNo IS NOT NULL AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return _vwHistoryCNInvoiceRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString() + ";" + ex.StackTrace.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userCredential.UserID);
                return 0;
            }
            finally
            {
                //context.Dispose();
            }
        }

        public List<vwInvoiceHeaderList> GetInvHeaderToList(string strToken, vmUserCredential userCredential, string OperatorID, string strOrderBy)
        {
            //var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwInvoiceHeaderList> listInvHeader = new List<vwInvoiceHeaderList>();
            
            if (userCredential.ErrorType > 0)
            {
                listInvHeader.Add(new vwInvoiceHeaderList(userCredential.ErrorType, userCredential.ErrorMessage));
                return listInvHeader;
            }

            try
            {
                string strWhereClause = $"1=1";
                if (!string.IsNullOrEmpty(OperatorID))
                {
                    if (OperatorID == "SMART" || OperatorID == "SMART8" || OperatorID == "M8")
                    {
                        strWhereClause += $" AND OperatorID in ('SMART', 'SMART8', 'M8')";
                    } else
                    {
                        strWhereClause += $" AND OperatorID = '{OperatorID}'";
                    }
                }
                if (string.IsNullOrEmpty(OperatorID))
                {
                    strWhereClause += $" AND OperatorID is null";
                }
                
                listInvHeader = _invHeaderRepo.GetList(strWhereClause, strOrderBy);
                return listInvHeader;
            }
            catch (Exception ex)
            {
                listInvHeader.Add(new vwInvoiceHeaderList((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userCredential.UserID)));
                return listInvHeader;
            }
            finally
            {
                //context.Dispose();
            }
        }

        public List<idxFINInvoiceReplacement> ManageReplacement(string strToken, vmUserCredential userCredential, List<idxFINInvoiceReplacement> post, string cnInvoiceNo)
        {
            List<idxFINInvoiceReplacement> listInvReplacement = new List<idxFINInvoiceReplacement>();
            if (userCredential.ErrorType > 0)
            {
                listInvReplacement.Add(new idxFINInvoiceReplacement(userCredential.ErrorType, userCredential.ErrorMessage));
                return listInvReplacement;
            }

            try
            {
                //check if replacement alrdy exist, by CNInvoiceNo?
                string strWhereClause = $"1=1";
                if (!string.IsNullOrEmpty(cnInvoiceNo))
                {
                    strWhereClause += $" AND CNInvoiceNo = '{cnInvoiceNo}'";
                    _invReplaceRepo.DeleteByFilter(strWhereClause);
                }
                DateTime dtNow = DateTime.Now;
                foreach (var item in post)
                {
                    if (item.isReplaceable == true)
                    {
                        item.ReplacedBy = userCredential.UserID;
                        item.ReplacedDate = dtNow;
                    }
                }
                return _invReplaceRepo.CreateBulky(post);
            }
            catch (Exception ex)
            {
                listInvReplacement.Add(new idxFINInvoiceReplacement((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userCredential.UserID)));
                return listInvReplacement;
            }
            finally
            {
                this.Dispose();
            }
        }

        public List<idxFINInvoiceReplacement> GetMappedReplacementList(string strToken, vmUserCredential userCredential, string CNInvoiceNo, string strOrderBy)
        {
            List<idxFINInvoiceReplacement> listMappedReplacement = new List<idxFINInvoiceReplacement>();

            if (userCredential.ErrorType > 0)
            {
                listMappedReplacement.Add(new idxFINInvoiceReplacement(userCredential.ErrorType, userCredential.ErrorMessage));
                return listMappedReplacement;
            }

            try
            {
                string strWhereClause = $"1=1";
                if (!string.IsNullOrEmpty(CNInvoiceNo))
                {
                    strWhereClause += $" AND CNInvoiceNo = '{CNInvoiceNo}'";
                }

                listMappedReplacement = _invReplaceRepo.GetList(strWhereClause, strOrderBy);
                
                return listMappedReplacement;
            }
            catch (Exception ex)
            {
                listMappedReplacement.Add(new idxFINInvoiceReplacement((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userCredential.UserID)));
                return listMappedReplacement;
            }
            finally
            {
                context.Dispose();
            }
        }

        public bool GetValidate(string token, vmUserCredential userCredential, List<idxFINInvoiceReplacement> list)
        {
            try
            {
                IEnumerable<string> duplicates = list.GroupBy(x => x.InvoiceNo)
                                                .Where(g => g.Count() > 1)
                                                .Select(x => x.Key);
                
                var aaa = duplicates.Count() + " " + duplicates;
                bool invNotSelected = false;
                if (list.Any(i => (i.InvoiceNo == null || i.InvoiceNo == string.Empty)))
                {
                    invNotSelected = true;
                }
                bool isReplacing = true;
                if (list.Count() > 0)
                {
                    var noReplacableList = list.FirstOrDefault();
                    if (string.IsNullOrEmpty(noReplacableList.InvoiceNo) && noReplacableList.isReplaceable == false)
                    {
                        isReplacing = false;
                    }
                }
                
                return (
                    list.Count() == 0||
                    duplicates.Count() > 0 
                    || (invNotSelected && isReplacing)) ? true : false;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString() + ";" + ex.StackTrace.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userCredential.UserID);
                return false;
            }
        }
    }
}
