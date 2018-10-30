﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MNPOSTEntities : DbContext
    {
        public MNPOSTEntities()
            : base("name=MNPOSTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AC_CODDebitVoucher> AC_CODDebitVoucher { get; set; }
        public virtual DbSet<AC_CODDebitVoucherDetails> AC_CODDebitVoucherDetails { get; set; }
        public virtual DbSet<AC_CustomerDebitVoucher> AC_CustomerDebitVoucher { get; set; }
        public virtual DbSet<AC_CustomerDebitVoucherDetail> AC_CustomerDebitVoucherDetail { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BS_Contracts> BS_Contracts { get; set; }
        public virtual DbSet<BS_Countries> BS_Countries { get; set; }
        public virtual DbSet<BS_CustomerGroups> BS_CustomerGroups { get; set; }
        public virtual DbSet<BS_Customers> BS_Customers { get; set; }
        public virtual DbSet<BS_Distants> BS_Distants { get; set; }
        public virtual DbSet<BS_Districts> BS_Districts { get; set; }
        public virtual DbSet<BS_Employees> BS_Employees { get; set; }
        public virtual DbSet<BS_MailerModeraters> BS_MailerModeraters { get; set; }
        public virtual DbSet<BS_Positions> BS_Positions { get; set; }
        public virtual DbSet<BS_PostOffices> BS_PostOffices { get; set; }
        public virtual DbSet<BS_PriceCustomers> BS_PriceCustomers { get; set; }
        public virtual DbSet<BS_PriceGroups> BS_PriceGroups { get; set; }
        public virtual DbSet<BS_PriceMaTrixs> BS_PriceMaTrixs { get; set; }
        public virtual DbSet<BS_PriceServiceTypes> BS_PriceServiceTypes { get; set; }
        public virtual DbSet<BS_Provinces> BS_Provinces { get; set; }
        public virtual DbSet<BS_RangeValues> BS_RangeValues { get; set; }
        public virtual DbSet<BS_RangeWeights> BS_RangeWeights { get; set; }
        public virtual DbSet<BS_RangeZones> BS_RangeZones { get; set; }
        public virtual DbSet<BS_ReturnReasons> BS_ReturnReasons { get; set; }
        public virtual DbSet<BS_RouteDetails> BS_RouteDetails { get; set; }
        public virtual DbSet<BS_Routes> BS_Routes { get; set; }
        public virtual DbSet<BS_Services> BS_Services { get; set; }
        public virtual DbSet<BS_ServiceTypes> BS_ServiceTypes { get; set; }
        public virtual DbSet<BS_Wards> BS_Wards { get; set; }
        public virtual DbSet<BS_Zones> BS_Zones { get; set; }
        public virtual DbSet<CData> CDatas { get; set; }
        public virtual DbSet<GeneralCodeInfo> GeneralCodeInfoes { get; set; }
        public virtual DbSet<MM_CustomerMoneyAdvances> MM_CustomerMoneyAdvances { get; set; }
        public virtual DbSet<MM_EmployeeDebitVoucher> MM_EmployeeDebitVoucher { get; set; }
        public virtual DbSet<MM_EmployeeDebitVoucherDetails> MM_EmployeeDebitVoucherDetails { get; set; }
        public virtual DbSet<MM_History> MM_History { get; set; }
        public virtual DbSet<MM_MailerDelivery> MM_MailerDelivery { get; set; }
        public virtual DbSet<MM_Mailers> MM_Mailers { get; set; }
        public virtual DbSet<MM_MailerServices> MM_MailerServices { get; set; }
        public virtual DbSet<MM_PackingList> MM_PackingList { get; set; }
        public virtual DbSet<MM_PackingListDetail> MM_PackingListDetail { get; set; }
        public virtual DbSet<MM_TakeDetails> MM_TakeDetails { get; set; }
        public virtual DbSet<MM_TakeMailers> MM_TakeMailers { get; set; }
        public virtual DbSet<MM_TroubleTickets> MM_TroubleTickets { get; set; }
        public virtual DbSet<UMS_GroupMenu> UMS_GroupMenu { get; set; }
        public virtual DbSet<UMS_Menu> UMS_Menu { get; set; }
        public virtual DbSet<UMS_MenuGroupUser> UMS_MenuGroupUser { get; set; }
        public virtual DbSet<UMS_UserGroups> UMS_UserGroups { get; set; }
        public virtual DbSet<UserLevel> UserLevels { get; set; }
        public virtual DbSet<UserPostOption> UserPostOptions { get; set; }
        public virtual DbSet<EmpployeeDebitCOD> EmpployeeDebitCODs { get; set; }
        public virtual DbSet<MM_Tracking> MM_Tracking { get; set; }
        public virtual DbSet<MailerImage> MailerImages { get; set; }
        public virtual DbSet<BS_Partners> BS_Partners { get; set; }
        public virtual DbSet<MM_MailerPartnerDetail> MM_MailerPartnerDetail { get; set; }
        public virtual DbSet<MM_MailerPartner> MM_MailerPartner { get; set; }
        public virtual DbSet<BS_PartnerMapInfo> BS_PartnerMapInfo { get; set; }
        public virtual DbSet<MM_TrackingPartner> MM_TrackingPartner { get; set; }
        public virtual DbSet<MM_MailerDeliveryDetail> MM_MailerDeliveryDetail { get; set; }
    
        public virtual ObjectResult<AC_CODDEBITVOUCHER_BYCUSTOMERID_Result> AC_CODDEBITVOUCHER_BYCUSTOMERID(string customerid, string fromdate, string todate)
        {
            var customeridParameter = customerid != null ?
                new ObjectParameter("customerid", customerid) :
                new ObjectParameter("customerid", typeof(string));
    
            var fromdateParameter = fromdate != null ?
                new ObjectParameter("fromdate", fromdate) :
                new ObjectParameter("fromdate", typeof(string));
    
            var todateParameter = todate != null ?
                new ObjectParameter("todate", todate) :
                new ObjectParameter("todate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AC_CODDEBITVOUCHER_BYCUSTOMERID_Result>("AC_CODDEBITVOUCHER_BYCUSTOMERID", customeridParameter, fromdateParameter, todateParameter);
        }
    
        public virtual ObjectResult<AC_CODDEBITVOUCHERDETAILS_BYDOCUMENTID_Result> AC_CODDEBITVOUCHERDETAILS_BYDOCUMENTID(string documentid)
        {
            var documentidParameter = documentid != null ?
                new ObjectParameter("documentid", documentid) :
                new ObjectParameter("documentid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AC_CODDEBITVOUCHERDETAILS_BYDOCUMENTID_Result>("AC_CODDEBITVOUCHERDETAILS_BYDOCUMENTID", documentidParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> CalPrice(Nullable<double> weight, string customerID, string provinceID, string serviceTypeID, string postOfficeID, string ngay)
        {
            var weightParameter = weight.HasValue ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(double));
    
            var customerIDParameter = customerID != null ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(string));
    
            var provinceIDParameter = provinceID != null ?
                new ObjectParameter("ProvinceID", provinceID) :
                new ObjectParameter("ProvinceID", typeof(string));
    
            var serviceTypeIDParameter = serviceTypeID != null ?
                new ObjectParameter("ServiceTypeID", serviceTypeID) :
                new ObjectParameter("ServiceTypeID", typeof(string));
    
            var postOfficeIDParameter = postOfficeID != null ?
                new ObjectParameter("PostOfficeID", postOfficeID) :
                new ObjectParameter("PostOfficeID", typeof(string));
    
            var ngayParameter = ngay != null ?
                new ObjectParameter("Ngay", ngay) :
                new ObjectParameter("Ngay", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("CalPrice", weightParameter, customerIDParameter, provinceIDParameter, serviceTypeIDParameter, postOfficeIDParameter, ngayParameter);
        }
    
        public virtual ObjectResult<CODCUSTOMER_TRANSFER_Result> CODCUSTOMER_TRANSFER(string customerid)
        {
            var customeridParameter = customerid != null ?
                new ObjectParameter("customerid", customerid) :
                new ObjectParameter("customerid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CODCUSTOMER_TRANSFER_Result>("CODCUSTOMER_TRANSFER", customeridParameter);
        }
    
        public virtual ObjectResult<COUNTRY_GETALL_Result> COUNTRY_GETALL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<COUNTRY_GETALL_Result>("COUNTRY_GETALL");
        }
    
        public virtual ObjectResult<DISTRICT_GETALL_Result> DISTRICT_GETALL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DISTRICT_GETALL_Result>("DISTRICT_GETALL");
        }
    
        public virtual ObjectResult<EMPLOYEE_GETALL_Result> EMPLOYEE_GETALL(string postId, string search)
        {
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            var searchParameter = search != null ?
                new ObjectParameter("search", search) :
                new ObjectParameter("search", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EMPLOYEE_GETALL_Result>("EMPLOYEE_GETALL", postIdParameter, searchParameter);
        }
    
        public virtual ObjectResult<EMPLOYEE_GETBYID_Result> EMPLOYEE_GETBYID(string employeeID)
        {
            var employeeIDParameter = employeeID != null ?
                new ObjectParameter("employeeID", employeeID) :
                new ObjectParameter("employeeID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EMPLOYEE_GETBYID_Result>("EMPLOYEE_GETBYID", employeeIDParameter);
        }
    
        public virtual ObjectResult<GROUPUSER_GETLISTMENU_Result> GROUPUSER_GETLISTMENU(string groupId)
        {
            var groupIdParameter = groupId != null ?
                new ObjectParameter("groupId", groupId) :
                new ObjectParameter("groupId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GROUPUSER_GETLISTMENU_Result>("GROUPUSER_GETLISTMENU", groupIdParameter);
        }
    
        public virtual ObjectResult<MAILER_DELIVERY_GETMAILER_EMPLOYEE_Result> MAILER_DELIVERY_GETMAILER_EMPLOYEE(string employeeId)
        {
            var employeeIdParameter = employeeId != null ?
                new ObjectParameter("employeeId", employeeId) :
                new ObjectParameter("employeeId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_DELIVERY_GETMAILER_EMPLOYEE_Result>("MAILER_DELIVERY_GETMAILER_EMPLOYEE", employeeIdParameter);
        }
    
        public virtual ObjectResult<MAILER_GET_ALL_DELIVERY_Result> MAILER_GET_ALL_DELIVERY(string fdate, string tdate, string postId)
        {
            var fdateParameter = fdate != null ?
                new ObjectParameter("fdate", fdate) :
                new ObjectParameter("fdate", typeof(string));
    
            var tdateParameter = tdate != null ?
                new ObjectParameter("tdate", tdate) :
                new ObjectParameter("tdate", typeof(string));
    
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_GET_ALL_DELIVERY_Result>("MAILER_GET_ALL_DELIVERY", fdateParameter, tdateParameter, postIdParameter);
        }
    
        public virtual ObjectResult<MAILER_GET_NOT_INVENTORY_Result> MAILER_GET_NOT_INVENTORY(string postId)
        {
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_GET_NOT_INVENTORY_Result>("MAILER_GET_NOT_INVENTORY", postIdParameter);
        }
    
        public virtual ObjectResult<MAILER_GETALL_Result> MAILER_GETALL(string fdate, string tdate, string postId, string mailerId)
        {
            var fdateParameter = fdate != null ?
                new ObjectParameter("fdate", fdate) :
                new ObjectParameter("fdate", typeof(string));
    
            var tdateParameter = tdate != null ?
                new ObjectParameter("tdate", tdate) :
                new ObjectParameter("tdate", typeof(string));
    
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            var mailerIdParameter = mailerId != null ?
                new ObjectParameter("mailerId", mailerId) :
                new ObjectParameter("mailerId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_GETALL_Result>("MAILER_GETALL", fdateParameter, tdateParameter, postIdParameter, mailerIdParameter);
        }
    
        public virtual ObjectResult<MAILER_GETINFO_BYID_Result> MAILER_GETINFO_BYID(string mailerId)
        {
            var mailerIdParameter = mailerId != null ?
                new ObjectParameter("mailerId", mailerId) :
                new ObjectParameter("mailerId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_GETINFO_BYID_Result>("MAILER_GETINFO_BYID", mailerIdParameter);
        }
    
        public virtual ObjectResult<MAILERDELIVERY_GETMAILER_Result> MAILERDELIVERY_GETMAILER(string documentID)
        {
            var documentIDParameter = documentID != null ?
                new ObjectParameter("documentID", documentID) :
                new ObjectParameter("documentID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILERDELIVERY_GETMAILER_Result>("MAILERDELIVERY_GETMAILER", documentIDParameter);
        }
    
        public virtual ObjectResult<MAILERDELIVERY_GETMAILER_BY_ID_Result> MAILERDELIVERY_GETMAILER_BY_ID(string documentID, string mailerID)
        {
            var documentIDParameter = documentID != null ?
                new ObjectParameter("documentID", documentID) :
                new ObjectParameter("documentID", typeof(string));
    
            var mailerIDParameter = mailerID != null ?
                new ObjectParameter("mailerID", mailerID) :
                new ObjectParameter("mailerID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILERDELIVERY_GETMAILER_BY_ID_Result>("MAILERDELIVERY_GETMAILER_BY_ID", documentIDParameter, mailerIDParameter);
        }
    
        public virtual ObjectResult<POSTOFFICE_GETALL_Result> POSTOFFICE_GETALL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<POSTOFFICE_GETALL_Result>("POSTOFFICE_GETALL");
        }
    
        public virtual ObjectResult<PROVINCE_GETALL_Result> PROVINCE_GETALL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PROVINCE_GETALL_Result>("PROVINCE_GETALL");
        }
    
        public virtual ObjectResult<ROUTE_GET_ALLEMPLOYEE_ROUTE_Result> ROUTE_GET_ALLEMPLOYEE_ROUTE(string postId)
        {
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ROUTE_GET_ALLEMPLOYEE_ROUTE_Result>("ROUTE_GET_ALLEMPLOYEE_ROUTE", postIdParameter);
        }
    
        public virtual ObjectResult<ROUTE_GETDETAIL_BYROUTEID_Result> ROUTE_GETDETAIL_BYROUTEID(string routeId)
        {
            var routeIdParameter = routeId != null ?
                new ObjectParameter("routeId", routeId) :
                new ObjectParameter("routeId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ROUTE_GETDETAIL_BYROUTEID_Result>("ROUTE_GETDETAIL_BYROUTEID", routeIdParameter);
        }
    
        public virtual ObjectResult<ROUTE_GETMAILER_BYEMPLOYEEID_Result> ROUTE_GETMAILER_BYEMPLOYEEID(string employeeId, string postId)
        {
            var employeeIdParameter = employeeId != null ?
                new ObjectParameter("employeeId", employeeId) :
                new ObjectParameter("employeeId", typeof(string));
    
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ROUTE_GETMAILER_BYEMPLOYEEID_Result>("ROUTE_GETMAILER_BYEMPLOYEEID", employeeIdParameter, postIdParameter);
        }
    
        public virtual ObjectResult<ROUTE_GETWARD_Result> ROUTE_GETWARD(string type, string districtId)
        {
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            var districtIdParameter = districtId != null ?
                new ObjectParameter("districtId", districtId) :
                new ObjectParameter("districtId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ROUTE_GETWARD_Result>("ROUTE_GETWARD", typeParameter, districtIdParameter);
        }
    
        public virtual ObjectResult<TAKEMAILER_GETDETAILs_Result> TAKEMAILER_GETDETAILs(string documentID)
        {
            var documentIDParameter = documentID != null ?
                new ObjectParameter("documentID", documentID) :
                new ObjectParameter("documentID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TAKEMAILER_GETDETAILs_Result>("TAKEMAILER_GETDETAILs", documentIDParameter);
        }
    
        public virtual ObjectResult<TAKEMAILER_GETLIST_Result> TAKEMAILER_GETLIST(string postId, Nullable<int> statusId, string date)
        {
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            var statusIdParameter = statusId.HasValue ?
                new ObjectParameter("statusId", statusId) :
                new ObjectParameter("statusId", typeof(int));
    
            var dateParameter = date != null ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TAKEMAILER_GETLIST_Result>("TAKEMAILER_GETLIST", postIdParameter, statusIdParameter, dateParameter);
        }
    
        public virtual ObjectResult<USER_CHECKACCESS_Result> USER_CHECKACCESS(string groupId, string menuCode)
        {
            var groupIdParameter = groupId != null ?
                new ObjectParameter("groupId", groupId) :
                new ObjectParameter("groupId", typeof(string));
    
            var menuCodeParameter = menuCode != null ?
                new ObjectParameter("menuCode", menuCode) :
                new ObjectParameter("menuCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_CHECKACCESS_Result>("USER_CHECKACCESS", groupIdParameter, menuCodeParameter);
        }
    
        public virtual ObjectResult<USER_GETMENU_Result> USER_GETMENU(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_GETMENU_Result>("USER_GETMENU", userParameter);
        }
    
        public virtual ObjectResult<USER_GETROLE_Result> USER_GETROLE(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_GETROLE_Result>("USER_GETROLE", userParameter);
        }
    
        public virtual ObjectResult<WARD_GETALL_Result> WARD_GETALL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<WARD_GETALL_Result>("WARD_GETALL");
        }
    
        public virtual ObjectResult<MAILER_DELIVERY_GETMAILER_EMPLOYEE_BYDATE_Result> MAILER_DELIVERY_GETMAILER_EMPLOYEE_BYDATE(string employeeId, string date)
        {
            var employeeIdParameter = employeeId != null ?
                new ObjectParameter("employeeId", employeeId) :
                new ObjectParameter("employeeId", typeof(string));
    
            var dateParameter = date != null ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_DELIVERY_GETMAILER_EMPLOYEE_BYDATE_Result>("MAILER_DELIVERY_GETMAILER_EMPLOYEE_BYDATE", employeeIdParameter, dateParameter);
        }
    
        public virtual ObjectResult<MAILER_GETTRACKING_Result> MAILER_GETTRACKING(string mailerId)
        {
            var mailerIdParameter = mailerId != null ?
                new ObjectParameter("mailerId", mailerId) :
                new ObjectParameter("mailerId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_GETTRACKING_Result>("MAILER_GETTRACKING", mailerIdParameter);
        }
    
        public virtual ObjectResult<TAKEMAILER_GETLIST_BY_EMPLOYEE_Result> TAKEMAILER_GETLIST_BY_EMPLOYEE(string employee, Nullable<int> statusId, string date)
        {
            var employeeParameter = employee != null ?
                new ObjectParameter("employee", employee) :
                new ObjectParameter("employee", typeof(string));
    
            var statusIdParameter = statusId.HasValue ?
                new ObjectParameter("statusId", statusId) :
                new ObjectParameter("statusId", typeof(int));
    
            var dateParameter = date != null ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TAKEMAILER_GETLIST_BY_EMPLOYEE_Result>("TAKEMAILER_GETLIST_BY_EMPLOYEE", employeeParameter, statusIdParameter, dateParameter);
        }
    
        public virtual ObjectResult<WIN_GET_MAILER_BY_DATE_POST_Result> WIN_GET_MAILER_BY_DATE_POST(string fromDate, string toDate, string postOfficeID)
        {
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            var postOfficeIDParameter = postOfficeID != null ?
                new ObjectParameter("PostOfficeID", postOfficeID) :
                new ObjectParameter("PostOfficeID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<WIN_GET_MAILER_BY_DATE_POST_Result>("WIN_GET_MAILER_BY_DATE_POST", fromDateParameter, toDateParameter, postOfficeIDParameter);
        }
    
        public virtual ObjectResult<MAILER_PARTNER_GETALL_Result> MAILER_PARTNER_GETALL(string fromDate, string toDate, string postId)
        {
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(string));
    
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_PARTNER_GETALL_Result>("MAILER_PARTNER_GETALL", fromDateParameter, toDateParameter, postIdParameter);
        }
    
        public virtual ObjectResult<MAILER_PARTNER_GETDETAIL_Result> MAILER_PARTNER_GETDETAIL(string documentId)
        {
            var documentIdParameter = documentId != null ?
                new ObjectParameter("documentId", documentId) :
                new ObjectParameter("documentId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_PARTNER_GETDETAIL_Result>("MAILER_PARTNER_GETDETAIL", documentIdParameter);
        }
    
        public virtual ObjectResult<MAILER_PARTNER_GETDETAIL_BY_MAILERID_Result> MAILER_PARTNER_GETDETAIL_BY_MAILERID(string documentId, string mailerId)
        {
            var documentIdParameter = documentId != null ?
                new ObjectParameter("documentId", documentId) :
                new ObjectParameter("documentId", typeof(string));
    
            var mailerIdParameter = mailerId != null ?
                new ObjectParameter("mailerId", mailerId) :
                new ObjectParameter("mailerId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_PARTNER_GETDETAIL_BY_MAILERID_Result>("MAILER_PARTNER_GETDETAIL_BY_MAILERID", documentIdParameter, mailerIdParameter);
        }
    
        public virtual ObjectResult<PARTNER_MAP_INFO_Result> PARTNER_MAP_INFO(string partnerCode, string form)
        {
            var partnerCodeParameter = partnerCode != null ?
                new ObjectParameter("partnerCode", partnerCode) :
                new ObjectParameter("partnerCode", typeof(string));
    
            var formParameter = form != null ?
                new ObjectParameter("form", form) :
                new ObjectParameter("form", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PARTNER_MAP_INFO_Result>("PARTNER_MAP_INFO", partnerCodeParameter, formParameter);
        }
    
        public virtual ObjectResult<MAILER_DELIVERY__EMPLOYEE_Result> MAILER_DELIVERY__EMPLOYEE(string postId)
        {
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_DELIVERY__EMPLOYEE_Result>("MAILER_DELIVERY__EMPLOYEE", postIdParameter);
        }
    
        public virtual ObjectResult<EMPLOYEE_DEBIT_ALL_Result> EMPLOYEE_DEBIT_ALL(string fdate, string tdate, string postId)
        {
            var fdateParameter = fdate != null ?
                new ObjectParameter("fdate", fdate) :
                new ObjectParameter("fdate", typeof(string));
    
            var tdateParameter = tdate != null ?
                new ObjectParameter("tdate", tdate) :
                new ObjectParameter("tdate", typeof(string));
    
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EMPLOYEE_DEBIT_ALL_Result>("EMPLOYEE_DEBIT_ALL", fdateParameter, tdateParameter, postIdParameter);
        }
    
        public virtual ObjectResult<REPORT_EMPLOYEE_DEBIT_COD_Result> REPORT_EMPLOYEE_DEBIT_COD(string postId)
        {
            var postIdParameter = postId != null ?
                new ObjectParameter("postId", postId) :
                new ObjectParameter("postId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<REPORT_EMPLOYEE_DEBIT_COD_Result>("REPORT_EMPLOYEE_DEBIT_COD", postIdParameter);
        }
    
        [DbFunction("MNPOSTEntities", "SplitList")]
        public virtual IQueryable<string> SplitList(string list, string separator)
        {
            var listParameter = list != null ?
                new ObjectParameter("list", list) :
                new ObjectParameter("list", typeof(string));
    
            var separatorParameter = separator != null ?
                new ObjectParameter("separator", separator) :
                new ObjectParameter("separator", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<string>("[MNPOSTEntities].[SplitList](@list, @separator)", listParameter, separatorParameter);
        }
    
        public virtual ObjectResult<MAILER_GETINFO_BYLISTID_Result> MAILER_GETINFO_BYLISTID(string mailers)
        {
            var mailersParameter = mailers != null ?
                new ObjectParameter("mailers", mailers) :
                new ObjectParameter("mailers", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MAILER_GETINFO_BYLISTID_Result>("MAILER_GETINFO_BYLISTID", mailersParameter);
        }
    }
}
