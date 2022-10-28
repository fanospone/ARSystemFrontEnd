using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueMovementAmount : BaseClass
    {
        public vmRevenueMovementAmount()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmRevenueMovementAmount(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string Description { get; set; }
        public decimal Previous { get; set; }
        public decimal Current { get; set; }
        public decimal Movement { get; set; }
        public decimal Percentage { get; set; }
        public decimal prevAccrueOverquotaISAT { get; set; }
        public decimal prevCancelDiscount { get; set; }
        public decimal prevDiffMLnv { get; set; }
        public decimal prevDiskon { get; set; }
        public decimal prevHoldAccrueNew { get; set; }
        public decimal prevHoldAccrueRenewal { get; set; }
        public decimal prevInflasi { get; set; }
        public decimal prevKurs { get; set; }
        public decimal prevNewPriceNew { get; set; }
        public decimal prevNewPriceRenewal { get; set; }
        public decimal prevNewSLD { get; set; }
        public decimal prevOverBlast { get; set; }
        public decimal prevReAktifHoldAccrue { get; set; }
        public decimal prevRelokasi { get; set; }
        public decimal prevReStopAccrue { get; set; }
        public decimal prevSharingRevenue { get; set; }
        public decimal prevSLDBAPSInvoice { get; set; }
        public decimal prevStopAccrue { get; set; }
        public decimal prevTemporaryFree { get; set; }
        public decimal AccrueOverquotaISAT { get; set; }
        public decimal CancelDiscount { get; set; }
        public decimal DiffMLnv { get; set; }
        public decimal Diskon { get; set; }
        public decimal HoldAccrueNew { get; set; }
        public decimal HoldAccrueRenewal { get; set; }
        public decimal Inflasi { get; set; }
        public decimal Kurs { get; set; }
        public decimal NewPriceNew { get; set; }
        public decimal NewPriceRenewal { get; set; }
        public decimal NewSLD { get; set; }
        public decimal OverBlast { get; set; }
        public decimal ReAktifHoldAccrue { get; set; }
        public decimal Relokasi { get; set; }
        public decimal ReStopAccrue { get; set; }
        public decimal SharingRevenue { get; set; }
        public decimal SLDBAPSInvoice { get; set; }
        public decimal StopAccrue { get; set; }
        public decimal TemporaryFree { get; set; }
        public decimal Total { get; set; }
        public string GroupID { get; set; }
    }
}
