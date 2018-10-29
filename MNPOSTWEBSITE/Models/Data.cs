using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOSTWEBSITE.Models
{
    public class Data
    {
    }

    public class Province
    {
        public string ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string CountryID { get; set; }
        public string PhoneCode { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string ZoneID { get; set; }
    }

    public class District
    {
        public string DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceID { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
    }

    public class Ward
    {
        public string WardID { get; set; }
        public string WardName { get; set; }
        public string DistrictID { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
    }

    public class Order
    {
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPhone { get; set; }
        public string SenderWardID { get; set; }
        public string SenderDistrictID { get; set; }
        public string SenderProvinceID { get; set; }
        public string RecieverName { get; set; }
        public string RecieverAddress { get; set; }
        public string RecieverPhone { get; set; }
        public string RecieverWardID { get; set; }
        public string RecieverDistrictID { get; set; }
        public string RecieverProvinceID { get; set; }
    }

    public class Customer
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
    }

    public class ResultInfo
    {
        public int error { get; set; }

        public string msg { get; set; }

        public Object data { get; set; }

    }

    public class CustomerInfo
    {
        public string CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> CustomerType { get; set; }
        public string CustomerGroupID { get; set; }
        public string Address { get; set; }
        public string DistrictID { get; set; }
        public string ProvinceID { get; set; }
        public string CountryID { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyPhone { get; set; }
        public string Mobile { get; set; }
        public string PersonalInfo { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string TaxCode { get; set; }
        public bool IsActive { get; set; }
        public string PostOfficeID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<byte> DebtDayInMonth { get; set; }
        public string MemberOf { get; set; }
        public string DebitObjectID { get; set; }
        public string CustomerPreID { get; set; }
        public string WardID { get; set; }
    }

    public class Mailer
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

    public class CODEntity
    {
        public string PostOfficeID { get; set; }
        public string DocumentID { get; set; }
        public DateTime DocumentDate { get; set; }
        public string PaymentID { get; set; }
        public float Total { get; set; }
    }
    public class CODDetail
    {
        public string PostOfficeID { get; set; }
        public string DocumentID { get; set; }
        public DateTime DocumentDate { get; set; }
        public string PaymentID { get; set; }
        public string MailerID { get; set; }
        public string RecieverName { get; set; }
        public decimal COD { get; set; }
    }
    public class CODTotal
    {
        public decimal SapChuyen { get; set; }
        public decimal ChuaGiao { get; set; }
        public decimal DaGiao { get; set; }
        public decimal DaChuyen { get; set; }
    }

    public class Tracking
    {
        public string MailerID { get; set; }
        public int StatusID { get; set; }
        public string Describe { get; set; }
        public string PostOffice { get; set; }
        public string CreateTime { get; set; }
        public string CreateDate { get; set; }
        public string StatusName { get; set; }
        public string StatusDescribe { get; set; }
    }
}