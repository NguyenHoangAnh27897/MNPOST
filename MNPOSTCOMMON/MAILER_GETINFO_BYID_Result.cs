//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MNPOSTCOMMON
{
    using System;
    
    public partial class MAILER_GETINFO_BYID_Result
    {
        public string MailerID { get; set; }
        public string PostOfficeAcceptID { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderWardID { get; set; }
        public string SenderDistrictID { get; set; }
        public string SenderProvinceID { get; set; }
        public string SenderPhone { get; set; }
        public string RecieverName { get; set; }
        public string RecieverAddress { get; set; }
        public string RecieverWardID { get; set; }
        public string RecieverDistrictID { get; set; }
        public string RecieverProvinceID { get; set; }
        public string RecieverPhone { get; set; }
        public string EmployeeAcceptID { get; set; }
        public Nullable<System.DateTime> AcceptDate { get; set; }
        public Nullable<System.DateTime> AcceptTime { get; set; }
        public string MailerTypeID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> ReWeight { get; set; }
        public Nullable<decimal> PriceDefault { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PriceService { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<decimal> BefVATAmount { get; set; }
        public Nullable<double> VATPercent { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountBefDiscount { get; set; }
        public string PaymentMethodID { get; set; }
        public string MailerDescription { get; set; }
        public string ThirdpartyDocID { get; set; }
        public Nullable<decimal> ThirdpartyCost { get; set; }
        public string ThirdpartyPaymentMethodID { get; set; }
        public Nullable<int> CurrentStatusID { get; set; }
        public string CurrentPostOfficeID { get; set; }
        public string PriceType { get; set; }
        public Nullable<bool> PriceIncludeVAT { get; set; }
        public Nullable<decimal> CommissionAmt { get; set; }
        public Nullable<double> CommissionPercent { get; set; }
        public Nullable<decimal> CostAmt { get; set; }
        public Nullable<System.DateTime> SalesClosingDate { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string MerchandiseID { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> COD { get; set; }
        public Nullable<double> LengthSize { get; set; }
        public Nullable<double> WidthSize { get; set; }
        public Nullable<double> HeightSize { get; set; }
        public Nullable<decimal> MerchandiseValue { get; set; }
        public string DeliveryTo { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string DeliveryNotes { get; set; }
    }
}
