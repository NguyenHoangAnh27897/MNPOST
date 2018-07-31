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
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BS_Contracts> BS_Contracts { get; set; }
        public virtual DbSet<BS_Countries> BS_Countries { get; set; }
        public virtual DbSet<BS_CustomerGroups> BS_CustomerGroups { get; set; }
        public virtual DbSet<BS_Customers> BS_Customers { get; set; }
        public virtual DbSet<BS_Districts> BS_Districts { get; set; }
        public virtual DbSet<BS_Employees> BS_Employees { get; set; }
        public virtual DbSet<BS_PostOffices> BS_PostOffices { get; set; }
        public virtual DbSet<BS_Provinces> BS_Provinces { get; set; }
        public virtual DbSet<BS_Wards> BS_Wards { get; set; }
        public virtual DbSet<BS_Zones> BS_Zones { get; set; }
        public virtual DbSet<MM_TroubleTickets> MM_TroubleTickets { get; set; }
        public virtual DbSet<UMS_GroupMenu> UMS_GroupMenu { get; set; }
        public virtual DbSet<UMS_Menu> UMS_Menu { get; set; }
        public virtual DbSet<UMS_UserGroups> UMS_UserGroups { get; set; }
        public virtual DbSet<UMS_MenuGroupUser> UMS_MenuGroupUser { get; set; }
        public virtual DbSet<BS_Positions> BS_Positions { get; set; }
        public virtual DbSet<GeneralCodeInfo> GeneralCodeInfoes { get; set; }
        public virtual DbSet<MM_CustomerMoneyAdvances> MM_CustomerMoneyAdvances { get; set; }
        public virtual DbSet<MM_EmployeeMoneyAdvances> MM_EmployeeMoneyAdvances { get; set; }
        public virtual DbSet<MM_History> MM_History { get; set; }
        public virtual DbSet<MM_MailerDeliveryDetail> MM_MailerDeliveryDetail { get; set; }
        public virtual DbSet<MM_Mailers> MM_Mailers { get; set; }
        public virtual DbSet<MM_PackingList> MM_PackingList { get; set; }
        public virtual DbSet<MM_PackingListDetail> MM_PackingListDetail { get; set; }
        public virtual DbSet<BS_PriceZones> BS_PriceZones { get; set; }
        public virtual DbSet<BS_Services> BS_Services { get; set; }
        public virtual DbSet<BS_ServiceTypes> BS_ServiceTypes { get; set; }
        public virtual DbSet<BS_Routes> BS_Routes { get; set; }
        public virtual DbSet<MM_MailerServices> MM_MailerServices { get; set; }
    
        public virtual ObjectResult<GROUPUSER_GETLISTMENU_Result> GROUPUSER_GETLISTMENU(string groupId)
        {
            var groupIdParameter = groupId != null ?
                new ObjectParameter("groupId", groupId) :
                new ObjectParameter("groupId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GROUPUSER_GETLISTMENU_Result>("GROUPUSER_GETLISTMENU", groupIdParameter);
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
    
        public virtual ObjectResult<USER_GETROLE_Result> USER_GETROLE(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_GETROLE_Result>("USER_GETROLE", userParameter);
        }
    
        public virtual ObjectResult<USER_GETMENU_Result> USER_GETMENU(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_GETMENU_Result>("USER_GETMENU", userParameter);
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
    
        public virtual ObjectResult<POSTOFFICE_GETALL_Result> POSTOFFICE_GETALL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<POSTOFFICE_GETALL_Result>("POSTOFFICE_GETALL");
        }
    }
}
