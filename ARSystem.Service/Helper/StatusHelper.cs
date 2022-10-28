namespace ARSystem.Service
{
    public static class StatusHelper
    {
        public enum InvoiceStatus : int
        {
            NotProcessed = 0,
            StateBAPSConfirm = 1,
            StateCreated = 2,
            StatePosted = 3,
            StatePrinted = 4,
            StateWaitingCancel = 5,
            Create = 6,
            Posting = 7,
            Print = 8,
            CancelInvoice = 11,
            StateCancelApproved = 12,
            ApproveCancelInvoice = 13,
            SubmitChecklist = 14,
            BAPSReceive = 15,
            Create15 = 16,
            BAPSConfirm = 17,
            StateBAPSReceive = 18,
            StateCreated15 = 19,
            StateWaitingARCollectionVerification = 21,
            StateReceivedByARCollection = 22,
            StateRejectedByARCollection = 23,
            StateCNInvoice = 24,
            CNInvoice = 25,
            StateCNInvoicePrintApproved = 26,
            StateWaitingCNInvoicePrint = 27,
            CNInvoicePrint =28,
            ApproveCNInvoicePrint = 29,
            ExportAXARData = 30,
            CancelMergeInvoice = 31,
            ApproveChecklistDocument = 32,
            RejectChecklistDocument = 33,
            StateReceiptUploaded = 34,
            UploadReceipt = 35,
            RequestCNInvoice = 36,
            SPVApproveCNInvoice = 37,
            DeptHeadApproveCNInvoice = 38,
            StateWaitingSPVApprovalCNInvoice = 39,
            StateWaitingDeptHeadApprovalCNInvoice = 40,
            StateApprovedCNInvoice = 41,
            SPVRejectCNInvoice = 42,
            DeptHeadRejectCNInvoice = 43,
            StateInvoicePaid = 44,
            PayInvoice = 45,
            PrintCNNote = 49,
            StateGeneratedToCollection =50,
            StateGeneratedToAX =51,
            GenerateToCollection =52,
            GenerateToAX = 53,
            ReOpen = 54,
            ApproveCNInvoiceChecklistARData = 55,
            CNInvoiceChecklistARData = 56,
            StateApprovedCNInvoiceChecklist = 57,
            StateWaitingCNInvoiceChecklist = 58     ,       
            BackToARProcess = 59

        };

        public enum InvoiceCategory: int
        {
            Tower = 1,
            Building = 2,
            OtherProduct = 3,
            Tower15 = 4,
            Tower10 = 5,
            NonRevenue = 6
        }

        public enum PicaTypeID : int
        {
            ARCollectionExternal = 1,
            ARCollectionInternal = 2
        }

        public enum StatusCollectionToSAP : int
        {
            NotYet = 0,
            Posted = 1,
            Success = 2,
            Error = 3
        }

    }
}