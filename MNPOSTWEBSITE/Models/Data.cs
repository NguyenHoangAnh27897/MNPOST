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
        public string ProvinceName { get; set; }
        public string ProvinceID { get; set; }
    }

    public class District
    {
        public string DistrictName { get; set; }
        public string DistrictID { get; set; }
    }

    public class Ward
    {
        public string WardName { get; set; }
        public string WardID { get; set; }
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
}