
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class dwhARRevenueSysSummary : BaseClass
	{
        public string fYear;
        public int? fDataYear;
        public string fRegionName;

        public dwhARRevenueSysSummary()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public dwhARRevenueSysSummary(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public string SONumber { get; set; }
        public int DataYear { get; set; }
        public string SiteName { get; set; }
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public string RegionName { get; set; }
        public decimal? JanSourceAmrUnearned { get; set; }
        public decimal? JanSourceBalance { get; set; }
        public decimal? JanSourceAccrued { get; set; }
        public decimal? JanBalanceAccrued { get; set; }
        public decimal? JanBalanceUnearned { get; set; }
        public decimal? FebSourceAmrUnearned { get; set; }
        public decimal? FebSourceBalance { get; set; }
        public decimal? FebSourceAccrued { get; set; }
        public decimal? FebBalanceAccrued { get; set; }
        public decimal? FebBalanceUnearned { get; set; }
        public decimal? MarSourceAmrUnearned { get; set; }
        public decimal? MarSourceBalance { get; set; }
        public decimal? MarSourceAccrued { get; set; }
        public decimal? MarBalanceAccrued { get; set; }
        public decimal? MarBalanceUnearned { get; set; }
        public decimal? AprSourceAmrUnearned { get; set; }
        public decimal? AprSourceBalance { get; set; }
        public decimal? AprSourceAccrued { get; set; }
        public decimal? AprBalanceAccrued { get; set; }
        public decimal? AprBalanceUnearned { get; set; }
        public decimal? MaySourceAmrUnearned { get; set; }
        public decimal? MaySourceBalance { get; set; }
        public decimal? MaySourceAccrued { get; set; }
        public decimal? MayBalanceAccrued { get; set; }
        public decimal? MayBalanceUnearned { get; set; }
        public decimal? JunSourceAmrUnearned { get; set; }
        public decimal? JunSourceBalance { get; set; }
        public decimal? JunSourceAccrued { get; set; }
        public decimal? JunBalanceAccrued { get; set; }
        public decimal? JunBalanceUnearned { get; set; }
        public decimal? JulSourceAmrUnearned { get; set; }
        public decimal? JulSourceBalance { get; set; }
        public decimal? JulSourceAccrued { get; set; }
        public decimal? JulBalanceAccrued { get; set; }
        public decimal? JulBalanceUnearned { get; set; }
        public decimal? AugSourceAmrUnearned { get; set; }
        public decimal? AugSourceBalance { get; set; }
        public decimal? AugSourceAccrued { get; set; }
        public decimal? AugBalanceAccrued { get; set; }
        public decimal? AugBalanceUnearned { get; set; }
        public decimal? SepSourceAmrUnearned { get; set; }
        public decimal? SepSourceBalance { get; set; }
        public decimal? SepSourceAccrued { get; set; }
        public decimal? SepBalanceAccrued { get; set; }
        public decimal? SepBalanceUnearned { get; set; }
        public decimal? OctSourceAmrUnearned { get; set; }
        public decimal? OctSourceBalance { get; set; }
        public decimal? OctSourceAccrued { get; set; }
        public decimal? OctBalanceAccrued { get; set; }
        public decimal? OctBalanceUnearned { get; set; }
        public decimal? NovSourceAmrUnearned { get; set; }
        public decimal? NovSourceBalance { get; set; }
        public decimal? NovSourceAccrued { get; set; }
        public decimal? NovBalanceAccrued { get; set; }
        public decimal? NovBalanceUnearned { get; set; }
        public decimal? DecSourceAmrUnearned { get; set; }
        public decimal? DecSourceBalance { get; set; }
        public decimal? DecSourceAccrued { get; set; }
        public decimal? DecBalanceAccrued { get; set; }
        public decimal? DecBalanceUnearned { get; set; }
 

    }

}