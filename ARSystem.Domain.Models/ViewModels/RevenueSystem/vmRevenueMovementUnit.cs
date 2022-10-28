using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueMovementUnit : BaseClass
    {
        public vmRevenueMovementUnit()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmRevenueMovementUnit(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string Description { get; set; }
        public int Previous { get; set; }
        public int Current { get; set; }
        public int Movement { get; set; }
        public int Percentage { get; set; }
        public int prevAccrueOverquotaISAT { get; set; }
        public int prevCancelDiscount { get; set; }
        public int prevDiffMLnv { get; set; }
        public int prevDiskon { get; set; }
        public int prevHoldAccrueNew { get; set; }
        public int prevHoldAccrueRenewal { get; set; }
        public int prevInflasi { get; set; }
        public int prevKurs { get; set; }
        public int prevNewPriceNew { get; set; }
        public int prevNewPriceRenewal { get; set; }
        public int prevNewSLD { get; set; }
        public int prevOverBlast { get; set; }
        public int prevReAktifHoldAccrue { get; set; }
        public int prevRelokasi { get; set; }
        public int prevReStopAccrue { get; set; }
        public int prevSharingRevenue { get; set; }
        public int prevSLDBAPSInvoice { get; set; }
        public int prevStopAccrue { get; set; }
        public int prevTemporaryFree { get; set; }
        public int AccrueOverquotaISAT { get; set; }
        public int CancelDiscount { get; set; }
        public int DiffMLnv { get; set; }
        public int Diskon { get; set; }
        public int HoldAccrueNew { get; set; }
        public int HoldAccrueRenewal { get; set; }
        public int Inflasi { get; set; }
        public int Kurs { get; set; }
        public int NewPriceNew { get; set; }
        public int NewPriceRenewal { get; set; }
        public int NewSLD { get; set; }
        public int OverBlast { get; set; }
        public int ReAktifHoldAccrue { get; set; }
        public int Relokasi { get; set; }
        public int ReStopAccrue { get; set; }
        public int SharingRevenue { get; set; }
        public int SLDBAPSInvoice { get; set; }
        public int StopAccrue { get; set; }
        public int TemporaryFree { get; set; }
        public int Total { get; set; }
        public string GroupID { get; set; }
    }
}
