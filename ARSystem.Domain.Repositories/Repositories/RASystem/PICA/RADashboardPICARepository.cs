using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories
{
    public class RADashboardPICARepository : BaseRepository<vwRADashboardPICA>
    {
        private DbContext _context;
        public RADashboardPICARepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vwRADashboardPICA> GetListHeader(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            return pGetListHeader(post, intRowStart, intPageSize);
        }
        public int GetCountListHeader(vwRADashboardPICA post)
        {
            return pGetCountListHeader(post);
        }

        public List<vwRADashboardPICA> GetListDetail(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            return pGetListDetail(post, intRowStart, intPageSize);
        }
        public int GetCountDetail(vwRADashboardPICA post)
        {
            return pGetCountDetail(post);
        }

        public int GetCountHistoryPICA(vwRADashboardPICA post)
        {
            return pGetCountHistoryPICA(post);
        }

        public List<vwRADashboardPICA> GetViewHistoryPICA(vwRADashboardPICA post , int intRowStart = 0, int intPageSize = 0)
        {
            return pGetViewHistoryPICA(post, intRowStart, intPageSize);
        }

        public vwRADashboardPICA InsertPICA(string userID, vwRADashboardPICA post)
        {
            return pInsertPICA(userID,post);
        }

        #region Private

        private List<vwRADashboardPICA> pGetListHeader(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                if (post.CompanyId == "ALL")
                {
                    post.CompanyId = null;
                }
                if (post.CustomerId == "ALL")
                {
                    post.CustomerId = null;
                }
                
                if (post.StipSiro == "ALL")
                {
                    post.StipSiro = null;
                }
                if (post.CompanyId == "ALL")
                {
                    post.CompanyId = null;
                }
                if (post.ActivityName == "ALL")
                {
                    post.ActivityName = null;
                }
                if (post.BapsType == "ALL")
                {
                    post.BapsType = null;
                }
                if (post.SONumber == "null")
                {
                    post.SONumber = null;
                }

                command.Parameters.Add(command.CreateParameter("@vType", "GetListHeader"));
                command.Parameters.Add(command.CreateParameter("@start", intRowStart));
                command.Parameters.Add(command.CreateParameter("@lenght", intPageSize));
                command.Parameters.Add(command.CreateParameter("@Company", post.CompanyId));
                command.Parameters.Add(command.CreateParameter("@Customer", post.CustomerId));
                command.Parameters.Add(command.CreateParameter("@BapsType", post.BapsType));
                command.Parameters.Add(command.CreateParameter("@StipCategory", post.StipSiro));
                command.Parameters.Add(command.CreateParameter("@ActivityStatus", post.ActivityName));
                command.Parameters.Add(command.CreateParameter("@ProductID", post.ProductID));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));

                return this.ReadTransaction(command).ToList();
            }
        }

        private int pGetCountListHeader(vwRADashboardPICA post)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCountListHeader"));
                command.Parameters.Add(command.CreateParameter("@Company", post.CompanyId));
                command.Parameters.Add(command.CreateParameter("@Customer", post.CustomerId));
                command.Parameters.Add(command.CreateParameter("@BapsType", post.BapsType));
                command.Parameters.Add(command.CreateParameter("@StipCategory", post.StipSiro));
                command.Parameters.Add(command.CreateParameter("@ActivityStatus", post.ActivityName));
                command.Parameters.Add(command.CreateParameter("@ProductID", post.ProductID));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));

                return this.CountTransaction(command);
            }
        }

        private List<vwRADashboardPICA> pGetListDetail(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                if (post.CompanyId == "ALL")
                {
                    post.CompanyId = null;
                }
                if (post.CustomerId == "ALL")
                {
                    post.CustomerId = null;
                }

                if (post.StipCode == "ALL")
                {
                    post.StipCode = null;
                }
                if (post.CompanyId == "ALL")
                {
                    post.CompanyId = null;
                }
                if (post.ActivityName == "ALL")
                {
                    post.ActivityName = null;
                }
                if (post.BapsType == "ALL")
                {
                    post.BapsType = null;
                }
                if (post.SONumber == "ALL")
                {
                    post.SONumber = null;
                }

                command.Parameters.Add(command.CreateParameter("@vType", "GetListDetail"));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));
                command.Parameters.Add(command.CreateParameter("@ActivityStatus", post.ActivityName));
                command.Parameters.Add(command.CreateParameter("@start", intRowStart));
                command.Parameters.Add(command.CreateParameter("@lenght", intPageSize));
                command.Parameters.Add(command.CreateParameter("@Company", post.CompanyId));
                command.Parameters.Add(command.CreateParameter("@Customer", post.CustomerId));
                command.Parameters.Add(command.CreateParameter("@BapsType", post.BapsType));
                command.Parameters.Add(command.CreateParameter("@StipCategory", post.StipCode));
                command.Parameters.Add(command.CreateParameter("@StipSiro", post.StipSiro));
                command.Parameters.Add(command.CreateParameter("@ProductID", post.ProductID));


                return this.ReadTransaction(command).ToList();
            }
        }

        private int pGetCountDetail(vwRADashboardPICA post)
        {
            using (var command = _context.CreateCommand())
            {
                if (post.CompanyId == "ALL")
                {
                    post.CompanyId = null;
                }
                if (post.CustomerId == "ALL")
                {
                    post.CustomerId = null;
                }

                if (post.StipCode == "ALL")
                {
                    post.StipCode = null;
                }
                if (post.CompanyId == "ALL")
                {
                    post.CompanyId = null;
                }
                if (post.ActivityName == "ALL")
                {
                    post.ActivityName = null;
                }
                if (post.BapsType == "ALL")
                {
                    post.BapsType = null;
                }
                if (post.SONumber == "ALL")
                {
                    post.SONumber = null;
                }

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCountDetail"));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));
                command.Parameters.Add(command.CreateParameter("@Company", post.CompanyId));
                command.Parameters.Add(command.CreateParameter("@Customer", post.CustomerId));
                command.Parameters.Add(command.CreateParameter("@BapsType", post.BapsType));
                command.Parameters.Add(command.CreateParameter("@StipCategory", post.StipCode));
                command.Parameters.Add(command.CreateParameter("@StipSiro", post.StipSiro));
                command.Parameters.Add(command.CreateParameter("@ActivityStatus", post.ActivityName));
                command.Parameters.Add(command.CreateParameter("@ProductID", post.ProductID));

                return this.CountTransaction(command);
            }
        }

        private int pGetCountHistoryPICA(vwRADashboardPICA post)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ViewCountHistoryPICA"));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));
                command.Parameters.Add(command.CreateParameter("@StipSiro", post.StipSiro));

                return this.CountTransaction(command);
            }
        }

        private List<vwRADashboardPICA> pGetViewHistoryPICA(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            using (var command = _context.CreateCommand())
            {
                if (post.StipSiro == "null")
                {
                    post.StipSiro = null;
                }

                if (post.SONumber == "null")
                {
                    post.SONumber = null;
                }
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ViewHistoryPICA"));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));
                command.Parameters.Add(command.CreateParameter("@StipSiro", post.StipSiro));
                command.Parameters.Add(command.CreateParameter("@start", intRowStart));
                command.Parameters.Add(command.CreateParameter("@lenght", intPageSize));

                return this.ReadTransaction(command).ToList();
            }
        }

        private vwRADashboardPICA pInsertPICA(string userID, vwRADashboardPICA post)
        {
            using (var command = _context.CreateCommand())
            {
                String xml = Helper.XmlSerializer<vwRADashboardPICA>(post);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "InsertPICA"));
                command.Parameters.Add(command.CreateParameter("@SONumber", post.SONumber));
                command.Parameters.Add(command.CreateParameter("@SiteId", post.SiteId));
                command.Parameters.Add(command.CreateParameter("@SiteName", post.SiteName));
                command.Parameters.Add(command.CreateParameter("@SiteIdOpr", post.SiteIdOpr));
                command.Parameters.Add(command.CreateParameter("@SiteNameOpr", post.SiteNameOpr));
                command.Parameters.Add(command.CreateParameter("@BapsType", post.BapsType));
                command.Parameters.Add(command.CreateParameter("@ActivityID", post.ActivityID));

                command.Parameters.Add(command.CreateParameter("@Duration", post.Durasi));
                command.Parameters.Add(command.CreateParameter("@CategoryPICA", post.CategoryPICA));
                command.Parameters.Add(command.CreateParameter("@PICA", post.PICA));
                command.Parameters.Add(command.CreateParameter("@DetailPICA", post.DetailPICA));
                command.Parameters.Add(command.CreateParameter("@TargetPICA", post.TargetPICA));
                command.Parameters.Add(command.CreateParameter("@CreateBy", userID));
                command.Parameters.Add(command.CreateParameter("@StartTarget", post.StartTarget));
                command.Parameters.Add(command.CreateParameter("@EndTarget", post.EndTarget));
                command.Parameters.Add(command.CreateParameter("@StipSiro", Convert.ToInt32(post.StipSiro)));

                this.WriteTransaction(command);

                return post;
            }
        }

        #endregion
    }
}
