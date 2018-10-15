
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/10/2018 23:24:38
-- Generated from EDMX file: D:\work\Project\007MNPost\MNPOST\MNPOSTCOMMON\MNPOST.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MNPOST];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__MM_Mailer__Emplo__5BAD9CC8]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MM_MailerDelivery] DROP CONSTRAINT [FK__MM_Mailer__Emplo__5BAD9CC8];
GO
IF OBJECT_ID(N'[dbo].[FK__UMS_Menu__GroupM__628FA481]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UMS_Menu] DROP CONSTRAINT [FK__UMS_Menu__GroupM__628FA481];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[AC_CODDebitVoucher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AC_CODDebitVoucher];
GO
IF OBJECT_ID(N'[dbo].[AC_CODDebitVoucherDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AC_CODDebitVoucherDetails];
GO
IF OBJECT_ID(N'[dbo].[AC_CustomerDebitVoucher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AC_CustomerDebitVoucher];
GO
IF OBJECT_ID(N'[dbo].[AC_CustomerDebitVoucherDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AC_CustomerDebitVoucherDetail];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[BS_Contracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Contracts];
GO
IF OBJECT_ID(N'[dbo].[BS_Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Countries];
GO
IF OBJECT_ID(N'[dbo].[BS_CustomerGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_CustomerGroups];
GO
IF OBJECT_ID(N'[dbo].[BS_Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Customers];
GO
IF OBJECT_ID(N'[dbo].[BS_Distants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Distants];
GO
IF OBJECT_ID(N'[dbo].[BS_Districts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Districts];
GO
IF OBJECT_ID(N'[dbo].[BS_Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Employees];
GO
IF OBJECT_ID(N'[dbo].[BS_MailerModeraters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_MailerModeraters];
GO
IF OBJECT_ID(N'[dbo].[BS_Positions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Positions];
GO
IF OBJECT_ID(N'[dbo].[BS_PostOffices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_PostOffices];
GO
IF OBJECT_ID(N'[dbo].[BS_PriceCustomers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_PriceCustomers];
GO
IF OBJECT_ID(N'[dbo].[BS_PriceGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_PriceGroups];
GO
IF OBJECT_ID(N'[dbo].[BS_PriceMaTrixs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_PriceMaTrixs];
GO
IF OBJECT_ID(N'[dbo].[BS_PriceServiceTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_PriceServiceTypes];
GO
IF OBJECT_ID(N'[dbo].[BS_Provinces]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Provinces];
GO
IF OBJECT_ID(N'[dbo].[BS_RangeValues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_RangeValues];
GO
IF OBJECT_ID(N'[dbo].[BS_RangeWeights]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_RangeWeights];
GO
IF OBJECT_ID(N'[dbo].[BS_RangeZones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_RangeZones];
GO
IF OBJECT_ID(N'[dbo].[BS_ReturnReasons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_ReturnReasons];
GO
IF OBJECT_ID(N'[dbo].[BS_RouteDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_RouteDetails];
GO
IF OBJECT_ID(N'[dbo].[BS_Routes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Routes];
GO
IF OBJECT_ID(N'[dbo].[BS_Services]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Services];
GO
IF OBJECT_ID(N'[dbo].[BS_ServiceTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_ServiceTypes];
GO
IF OBJECT_ID(N'[dbo].[BS_Wards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Wards];
GO
IF OBJECT_ID(N'[dbo].[BS_Zones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BS_Zones];
GO
IF OBJECT_ID(N'[dbo].[CData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CData];
GO
IF OBJECT_ID(N'[dbo].[GeneralCodeInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GeneralCodeInfo];
GO
IF OBJECT_ID(N'[dbo].[MM_CustomerMoneyAdvances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_CustomerMoneyAdvances];
GO
IF OBJECT_ID(N'[dbo].[MM_EmployeeDebitVoucher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_EmployeeDebitVoucher];
GO
IF OBJECT_ID(N'[dbo].[MM_EmployeeDebitVoucherDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_EmployeeDebitVoucherDetails];
GO
IF OBJECT_ID(N'[dbo].[MM_EmployeeMoneyAdvances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_EmployeeMoneyAdvances];
GO
IF OBJECT_ID(N'[dbo].[MM_History]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_History];
GO
IF OBJECT_ID(N'[dbo].[MM_MailerDelivery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_MailerDelivery];
GO
IF OBJECT_ID(N'[dbo].[MM_MailerDeliveryDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_MailerDeliveryDetail];
GO
IF OBJECT_ID(N'[dbo].[MM_Mailers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_Mailers];
GO
IF OBJECT_ID(N'[dbo].[MM_MailerServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_MailerServices];
GO
IF OBJECT_ID(N'[dbo].[MM_PackingList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_PackingList];
GO
IF OBJECT_ID(N'[dbo].[MM_PackingListDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_PackingListDetail];
GO
IF OBJECT_ID(N'[dbo].[MM_TakeDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_TakeDetails];
GO
IF OBJECT_ID(N'[dbo].[MM_TakeMailers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_TakeMailers];
GO
IF OBJECT_ID(N'[dbo].[MM_TroubleTickets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MM_TroubleTickets];
GO
IF OBJECT_ID(N'[dbo].[UMS_GroupMenu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMS_GroupMenu];
GO
IF OBJECT_ID(N'[dbo].[UMS_Menu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMS_Menu];
GO
IF OBJECT_ID(N'[dbo].[UMS_MenuGroupUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMS_MenuGroupUser];
GO
IF OBJECT_ID(N'[dbo].[UMS_UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMS_UserGroups];
GO
IF OBJECT_ID(N'[dbo].[UserLevel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserLevel];
GO
IF OBJECT_ID(N'[dbo].[UserPostOption]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPostOption];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'AC_CODDebitVoucher'
CREATE TABLE [dbo].[AC_CODDebitVoucher] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [PostOfficeID] nvarchar(20)  NULL,
    [DocumentDate] datetime  NULL,
    [EmployeeID] nvarchar(20)  NULL,
    [PaymentID] nvarchar(30)  NULL,
    [CustomerID] nvarchar(50)  NULL,
    [IsPayment] bit  NULL,
    [Total] decimal(19,4)  NULL,
    [Notes] nvarchar(200)  NULL
);
GO

-- Creating table 'AC_CODDebitVoucherDetails'
CREATE TABLE [dbo].[AC_CODDebitVoucherDetails] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [MailerID] nvarchar(20)  NULL,
    [CreateDate] datetime  NULL,
    [UserID] nvarchar(20)  NULL
);
GO

-- Creating table 'AC_CustomerDebitVoucher'
CREATE TABLE [dbo].[AC_CustomerDebitVoucher] (
    [DocumentID] varchar(15)  NOT NULL,
    [DocumentDate] datetime  NULL,
    [DocumentTime] datetime  NULL,
    [PostOfficeID] varchar(15)  NULL,
    [UserAccountID] varchar(15)  NULL,
    [CustomerID] varchar(15)  NULL,
    [RepresentativeID] varchar(15)  NULL,
    [FromDate] datetime  NULL,
    [ToDate] datetime  NULL,
    [TotalAmount] float  NULL,
    [Description] nvarchar(200)  NULL,
    [PaymentMethodID] varchar(15)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [IsPayComplete] bit  NOT NULL,
    [PayDocumentID] varchar(50)  NULL,
    [PayDate] datetime  NULL,
    [PayNotes] nvarchar(500)  NULL,
    [InvoiceID] varchar(20)  NULL,
    [DebtMonth] datetime  NULL,
    [LastUpdDate] datetime  NULL,
    [RecordState] int  NOT NULL,
    [SyncFlag] bit  NOT NULL,
    [LastSyncDate] datetime  NULL,
    [Invoiced] bit  NOT NULL
);
GO

-- Creating table 'AC_CustomerDebitVoucherDetail'
CREATE TABLE [dbo].[AC_CustomerDebitVoucherDetail] (
    [DocumentID] varchar(15)  NOT NULL,
    [MailerID] varchar(20)  NOT NULL,
    [Amount] float  NULL,
    [Notes] nvarchar(100)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [Price] decimal(18,6)  NOT NULL,
    [PriceService] decimal(18,6)  NOT NULL,
    [DiscountPercent] decimal(6,2)  NOT NULL,
    [Discount] decimal(18,6)  NOT NULL,
    [VATpercent] decimal(6,2)  NOT NULL,
    [BfVATamount] decimal(18,6)  NOT NULL,
    [VATamount] decimal(18,6)  NOT NULL,
    [AfVATamount] decimal(18,6)  NOT NULL,
    [AcceptDate] datetime  NULL,
    [Quantity] int  NOT NULL,
    [Weight] decimal(18,6)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL,
    [User_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [UserId] nvarchar(128)  NOT NULL,
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [UserName] nvarchar(max)  NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [IsActivced] int  NULL,
    [FullName] nvarchar(max)  NULL,
    [AccountType] nvarchar(max)  NULL,
    [GroupId] nvarchar(max)  NULL,
    [Discriminator] nvarchar(128)  NOT NULL,
    [ULevel] nvarchar(128)  NULL
);
GO

-- Creating table 'BS_Contracts'
CREATE TABLE [dbo].[BS_Contracts] (
    [ContractID] varchar(15)  NOT NULL,
    [CustomerID] varchar(15)  NULL,
    [RepresentativeID] varchar(15)  NULL,
    [BeginDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [Value] decimal(19,4)  NULL,
    [CurrencyID] varchar(15)  NULL,
    [AppendDate] datetime  NOT NULL,
    [Description] varbinary(max)  NULL,
    [PostOfficeID] varchar(15)  NOT NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [MemberOf] varchar(15)  NULL,
    [Note] nvarchar(50)  NULL,
    [ContractNo] varchar(15)  NULL,
    [LastUpdUser] nvarchar(100)  NULL
);
GO

-- Creating table 'BS_Countries'
CREATE TABLE [dbo].[BS_Countries] (
    [CountryID] nvarchar(5)  NOT NULL,
    [CountryName] nvarchar(50)  NULL,
    [IsActive] bit  NULL,
    [CreateDate] datetime  NULL,
    [UpdateDate] datetime  NULL,
    [CountryCode] nvarchar(10)  NULL
);
GO

-- Creating table 'BS_CustomerGroups'
CREATE TABLE [dbo].[BS_CustomerGroups] (
    [CustomerGroupID] varchar(15)  NOT NULL,
    [CustomerGroupName] nvarchar(50)  NOT NULL,
    [Notes] nvarchar(200)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL
);
GO

-- Creating table 'BS_Customers'
CREATE TABLE [dbo].[BS_Customers] (
    [CustomerID] nvarchar(128)  NOT NULL,
    [CustomerCode] nvarchar(15)  NULL,
    [CustomerName] nvarchar(200)  NOT NULL,
    [CustomerType] int  NULL,
    [CustomerGroupID] varchar(15)  NULL,
    [Address] nvarchar(200)  NULL,
    [DistrictID] varchar(15)  NULL,
    [ProvinceID] varchar(15)  NULL,
    [CountryID] varchar(15)  NULL,
    [FaxNo] nvarchar(20)  NULL,
    [Email] nvarchar(50)  NULL,
    [Phone] nvarchar(20)  NULL,
    [CompanyPhone] nvarchar(20)  NULL,
    [Mobile] nvarchar(20)  NULL,
    [PersonalInfo] nvarchar(50)  NULL,
    [BankAccount] nvarchar(20)  NULL,
    [BankName] nvarchar(50)  NULL,
    [TaxCode] nvarchar(50)  NULL,
    [IsActive] bit  NOT NULL,
    [PostOfficeID] varchar(15)  NULL,
    [CreateDate] datetime  NOT NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [DebtDayInMonth] tinyint  NULL,
    [MemberOf] varchar(50)  NULL,
    [DebitObjectID] varchar(20)  NULL,
    [CustomerPreID] varchar(15)  NULL,
    [WardID] nvarchar(128)  NULL
);
GO

-- Creating table 'BS_Distants'
CREATE TABLE [dbo].[BS_Distants] (
    [GroupID] nvarchar(20)  NOT NULL,
    [ProvinceID] nvarchar(5)  NOT NULL,
    [ZoneID] nvarchar(20)  NULL
);
GO

-- Creating table 'BS_Districts'
CREATE TABLE [dbo].[BS_Districts] (
    [DistrictID] varchar(15)  NOT NULL,
    [DistrictName] nvarchar(50)  NOT NULL,
    [ProvinceID] varchar(15)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UpdateDate] datetime  NULL,
    [CreationDate] datetime  NULL
);
GO

-- Creating table 'BS_Employees'
CREATE TABLE [dbo].[BS_Employees] (
    [EmployeeID] nvarchar(128)  NOT NULL,
    [EmployeeName] nvarchar(max)  NULL,
    [DepartmentID] varchar(15)  NULL,
    [PostOfficeID] varchar(15)  NULL,
    [Birthday] datetime  NULL,
    [IsActive] bit  NULL,
    [Address] nvarchar(max)  NULL,
    [Phone] nvarchar(25)  NULL,
    [IdentifyCard] nvarchar(128)  NULL,
    [FaxNo] nvarchar(25)  NULL,
    [Email] nvarchar(512)  NULL,
    [Notes] nvarchar(max)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [MemberOf] varchar(50)  NULL,
    [ProvinceID] varchar(15)  NULL,
    [IsCollaborators] bit  NULL,
    [SGPEmployeeID] nvarchar(20)  NULL,
    [Sex] bit  NULL,
    [ApproveDate] datetime  NULL,
    [UserLogin] nvarchar(128)  NULL,
    [PositionID] nvarchar(20)  NULL
);
GO

-- Creating table 'BS_MailerModeraters'
CREATE TABLE [dbo].[BS_MailerModeraters] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [EmployeeID] nvarchar(20)  NOT NULL,
    [MailerID] nvarchar(20)  NOT NULL,
    [Price] decimal(19,4)  NULL
);
GO

-- Creating table 'BS_Positions'
CREATE TABLE [dbo].[BS_Positions] (
    [PositionID] nvarchar(20)  NOT NULL,
    [PositionName] nvarchar(100)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'BS_PostOffices'
CREATE TABLE [dbo].[BS_PostOffices] (
    [PostOfficeID] varchar(15)  NOT NULL,
    [PostOfficeName] nvarchar(50)  NULL,
    [Address] nvarchar(100)  NULL,
    [ZoneID] varchar(15)  NULL,
    [ProvinceID] varchar(15)  NULL,
    [Phone] nvarchar(25)  NULL,
    [FaxNo] nvarchar(25)  NULL,
    [Email] nvarchar(50)  NULL,
    [IsCollaborator] bit  NULL,
    [Notes] nvarchar(200)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [TaxCode] varchar(50)  NULL,
    [BankAccount] varchar(50)  NULL,
    [BankName] nvarchar(100)  NULL,
    [Type] int  NULL,
    [AreaChar] varchar(5)  NULL
);
GO

-- Creating table 'BS_PriceCustomers'
CREATE TABLE [dbo].[BS_PriceCustomers] (
    [PriceMatrixID] nvarchar(20)  NOT NULL,
    [CustomerID] nvarchar(20)  NULL
);
GO

-- Creating table 'BS_PriceGroups'
CREATE TABLE [dbo].[BS_PriceGroups] (
    [GroupID] nvarchar(20)  NOT NULL,
    [GroupName] nvarchar(50)  NULL,
    [ZoneNumber] int  NULL,
    [WeightNumber] int  NULL
);
GO

-- Creating table 'BS_PriceMaTrixs'
CREATE TABLE [dbo].[BS_PriceMaTrixs] (
    [PriceMatrixID] nvarchar(20)  NOT NULL,
    [CreateDate] datetime  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [PostOfficeID] nvarchar(10)  NULL,
    [GroupID] nvarchar(20)  NULL,
    [PriceType] nvarchar(2)  NULL,
    [StatusID] int  NULL,
    [ApplyAllCustomer] bit  NULL,
    [ApplyAllZone] nvarchar(5)  NULL,
    [Description] nvarchar(200)  NULL,
    [Service] bit  NULL
);
GO

-- Creating table 'BS_PriceServiceTypes'
CREATE TABLE [dbo].[BS_PriceServiceTypes] (
    [PriceMatrixID] nvarchar(20)  NOT NULL,
    [ServiceTypeID] nvarchar(2)  NOT NULL
);
GO

-- Creating table 'BS_Provinces'
CREATE TABLE [dbo].[BS_Provinces] (
    [ProvinceID] varchar(15)  NOT NULL,
    [ProvinceName] nvarchar(50)  NOT NULL,
    [CountryID] varchar(15)  NOT NULL,
    [PhoneCode] nvarchar(10)  NULL,
    [IsActive] bit  NOT NULL,
    [UpdateDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [ZoneID] nvarchar(10)  NULL
);
GO

-- Creating table 'BS_RangeValues'
CREATE TABLE [dbo].[BS_RangeValues] (
    [PriceMatrixID] nvarchar(20)  NOT NULL,
    [RowIndex] int  NOT NULL,
    [ColIndex] int  NOT NULL,
    [Value] float  NULL
);
GO

-- Creating table 'BS_RangeWeights'
CREATE TABLE [dbo].[BS_RangeWeights] (
    [GroupID] nvarchar(20)  NOT NULL,
    [FromWeight] float  NOT NULL,
    [ToWeight] float  NOT NULL,
    [IsNextWeight] bit  NULL,
    [No] int  NULL
);
GO

-- Creating table 'BS_RangeZones'
CREATE TABLE [dbo].[BS_RangeZones] (
    [GroupID] nvarchar(20)  NOT NULL,
    [ZoneID] nvarchar(20)  NOT NULL,
    [No] int  NULL
);
GO

-- Creating table 'BS_ReturnReasons'
CREATE TABLE [dbo].[BS_ReturnReasons] (
    [ReasonID] int  NOT NULL,
    [ReasonName] nvarchar(50)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'BS_RouteDetails'
CREATE TABLE [dbo].[BS_RouteDetails] (
    [RouteID] nvarchar(128)  NOT NULL,
    [WardID] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'BS_Routes'
CREATE TABLE [dbo].[BS_Routes] (
    [RouteID] nvarchar(128)  NOT NULL,
    [EmployeeID] nvarchar(20)  NULL,
    [Type] nvarchar(1)  NULL,
    [ProvinceID] nvarchar(10)  NULL,
    [DistrictID] nvarchar(10)  NULL,
    [IsDetail] bit  NULL
);
GO

-- Creating table 'BS_Services'
CREATE TABLE [dbo].[BS_Services] (
    [ServiceID] nvarchar(10)  NOT NULL,
    [ServiceName] nvarchar(50)  NULL,
    [Price] decimal(19,4)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'BS_ServiceTypes'
CREATE TABLE [dbo].[BS_ServiceTypes] (
    [ServiceID] nvarchar(5)  NOT NULL,
    [ServiceName] nvarchar(50)  NULL,
    [Type] nvarchar(1)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'BS_Wards'
CREATE TABLE [dbo].[BS_Wards] (
    [WardID] varchar(15)  NOT NULL,
    [WardName] nvarchar(50)  NOT NULL,
    [DistrictID] varchar(15)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UpdateDate] datetime  NULL,
    [CreationDate] datetime  NULL
);
GO

-- Creating table 'BS_Zones'
CREATE TABLE [dbo].[BS_Zones] (
    [ZoneID] nvarchar(5)  NOT NULL,
    [ZoneName] nvarchar(100)  NULL,
    [UpdateDate] datetime  NULL,
    [IsActive] bit  NULL,
    [CreateDate] datetime  NULL
);
GO

-- Creating table 'CDatas'
CREATE TABLE [dbo].[CDatas] (
    [Id] nvarchar(128)  NOT NULL,
    [Code] nvarchar(128)  NULL,
    [Name] nvarchar(max)  NULL,
    [CType] nvarchar(128)  NULL,
    [CNotes] nvarchar(max)  NULL,
    [Active] int  NULL
);
GO

-- Creating table 'GeneralCodeInfoes'
CREATE TABLE [dbo].[GeneralCodeInfoes] (
    [Id] nvarchar(128)  NOT NULL,
    [FirstChar] varchar(50)  NULL,
    [PreNumber] int  NULL,
    [Code] varchar(128)  NULL
);
GO

-- Creating table 'MM_CustomerMoneyAdvances'
CREATE TABLE [dbo].[MM_CustomerMoneyAdvances] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [AcceptID] nvarchar(20)  NOT NULL,
    [AcceptDate] datetime  NULL,
    [EmployeeID] nvarchar(20)  NULL,
    [PostOfficeID] nvarchar(5)  NULL,
    [CustomerID] nvarchar(20)  NULL,
    [Total] decimal(19,4)  NULL,
    [MailerID] nvarchar(20)  NULL,
    [Type] int  NULL,
    [StatusID] bit  NULL,
    [IsPercent] bit  NULL
);
GO

-- Creating table 'MM_EmployeeDebitVoucher'
CREATE TABLE [dbo].[MM_EmployeeDebitVoucher] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [DocumentNumber] nvarchar(20)  NULL,
    [DocumentDate] datetime  NULL,
    [MoneyColector] nvarchar(20)  NULL,
    [EmployeeID] nvarchar(20)  NULL,
    [MailerAccount] int  NULL,
    [TotalMoney] decimal(19,4)  NULL,
    [PostOfficeID] nvarchar(20)  NULL,
    [Status] nvarchar(20)  NULL
);
GO

-- Creating table 'MM_EmployeeDebitVoucherDetails'
CREATE TABLE [dbo].[MM_EmployeeDebitVoucherDetails] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [MailerID] nvarchar(20)  NOT NULL,
    [ReciveCOD] decimal(19,4)  NULL,
    [LastUpDate] datetime  NULL,
    [COD] decimal(19,4)  NULL
);
GO

-- Creating table 'MM_EmployeeMoneyAdvances'
CREATE TABLE [dbo].[MM_EmployeeMoneyAdvances] (
    [DocumentID] nvarchar(20)  NOT NULL,
    [AcceptID] nvarchar(20)  NOT NULL,
    [AcceptDate] datetime  NULL,
    [EmployeeID] nvarchar(20)  NULL,
    [PostOfficeID] nvarchar(5)  NULL,
    [Total] decimal(19,4)  NULL,
    [StatusID] bit  NULL,
    [ReturnMoney] decimal(19,4)  NULL,
    [MailerID] nvarchar(20)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'MM_History'
CREATE TABLE [dbo].[MM_History] (
    [MailerID] varchar(20)  NOT NULL,
    [StatusID] varchar(2)  NOT NULL,
    [PostOfficeID] varchar(15)  NOT NULL,
    [DocumentID] varchar(15)  NULL,
    [DateChange] datetime  NOT NULL,
    [UserGroupID] varchar(20)  NOT NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL
);
GO

-- Creating table 'MM_MailerDelivery'
CREATE TABLE [dbo].[MM_MailerDelivery] (
    [DocumentID] nvarchar(128)  NOT NULL,
    [DocumentDate] datetime  NULL,
    [EmployeeID] nvarchar(128)  NULL,
    [Notes] nvarchar(max)  NULL,
    [Quantity] int  NULL,
    [Weight] float  NULL,
    [CreateDate] datetime  NULL,
    [LastEditDate] datetime  NULL,
    [StatusID] int  NULL,
    [RouteID] nvarchar(20)  NULL,
    [NumberPlate] nvarchar(20)  NULL,
    [DocumentCode] nvarchar(128)  NULL
);
GO

-- Creating table 'MM_MailerDeliveryDetail'
CREATE TABLE [dbo].[MM_MailerDeliveryDetail] (
    [DocumentID] varchar(128)  NOT NULL,
    [MailerID] varchar(128)  NOT NULL,
    [IsDeliverOver] bit  NULL,
    [DeliveryTo] nvarchar(50)  NULL,
    [DeliveryDate] datetime  NULL,
    [DeliveryStatus] int  NULL,
    [PaymentFinished] bit  NULL,
    [DeliveryNotes] nvarchar(200)  NULL,
    [ConfirmDate] datetime  NULL,
    [ConfirmUserID] varchar(20)  NULL,
    [ConfirmIndex] varchar(30)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL,
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ReturnReasonID] int  NULL
);
GO

-- Creating table 'MM_Mailers'
CREATE TABLE [dbo].[MM_Mailers] (
    [MailerID] nvarchar(20)  NOT NULL,
    [PostOfficeAcceptID] nvarchar(10)  NULL,
    [SenderID] nvarchar(20)  NULL,
    [SenderName] nvarchar(100)  NULL,
    [SenderAddress] nvarchar(200)  NULL,
    [SenderWardID] nvarchar(10)  NULL,
    [SenderDistrictID] nvarchar(10)  NULL,
    [SenderProvinceID] nvarchar(10)  NULL,
    [SenderPhone] nvarchar(20)  NULL,
    [RecieverName] nvarchar(100)  NULL,
    [RecieverAddress] nvarchar(200)  NULL,
    [RecieverWardID] nvarchar(10)  NULL,
    [RecieverDistrictID] nvarchar(10)  NULL,
    [RecieverProvinceID] nvarchar(10)  NULL,
    [RecieverPhone] nvarchar(20)  NULL,
    [EmployeeAcceptID] nvarchar(20)  NULL,
    [AcceptDate] datetime  NULL,
    [AcceptTime] datetime  NULL,
    [MailerTypeID] nvarchar(10)  NULL,
    [Quantity] int  NULL,
    [Weight] float  NULL,
    [ReWeight] float  NULL,
    [PriceDefault] decimal(19,4)  NULL,
    [Price] decimal(19,4)  NULL,
    [PriceService] decimal(19,4)  NULL,
    [Discount] float  NULL,
    [BefVATAmount] decimal(19,4)  NULL,
    [VATPercent] float  NULL,
    [VATAmount] decimal(19,4)  NULL,
    [Amount] decimal(19,4)  NULL,
    [AmountBefDiscount] decimal(19,4)  NULL,
    [PaymentMethodID] nvarchar(5)  NULL,
    [MailerDescription] nvarchar(200)  NULL,
    [ThirdpartyDocID] nvarchar(20)  NULL,
    [ThirdpartyCost] decimal(19,4)  NULL,
    [ThirdpartyPaymentMethodID] nvarchar(5)  NULL,
    [CurrentStatusID] int  NULL,
    [CurrentPostOfficeID] nvarchar(10)  NULL,
    [PriceType] nvarchar(1)  NULL,
    [PriceIncludeVAT] bit  NULL,
    [CommissionAmt] decimal(19,4)  NULL,
    [CommissionPercent] float  NULL,
    [CostAmt] decimal(19,4)  NULL,
    [SalesClosingDate] datetime  NULL,
    [DiscountPercent] float  NULL,
    [CreationDate] datetime  NULL,
    [LastUpdateDate] datetime  NULL,
    [MerchandiseID] nvarchar(10)  NULL,
    [Notes] nvarchar(max)  NULL,
    [COD] decimal(19,4)  NULL,
    [LengthSize] float  NULL,
    [WidthSize] float  NULL,
    [HeightSize] float  NULL,
    [MerchandiseValue] decimal(19,4)  NULL,
    [DeliveryTo] nvarchar(max)  NULL,
    [DeliveryDate] datetime  NULL,
    [DeliveryNotes] nvarchar(max)  NULL,
    [CreateType] int  NULL
);
GO

-- Creating table 'MM_MailerServices'
CREATE TABLE [dbo].[MM_MailerServices] (
    [MailerID] nvarchar(20)  NOT NULL,
    [ServiceID] nvarchar(10)  NOT NULL,
    [PriceDefault] decimal(19,4)  NULL,
    [Price] decimal(19,4)  NULL,
    [LastUpDate] datetime  NULL
);
GO

-- Creating table 'MM_PackingList'
CREATE TABLE [dbo].[MM_PackingList] (
    [DocumentID] varchar(15)  NOT NULL,
    [DocumentDate] datetime  NULL,
    [PostOfficeID] varchar(15)  NULL,
    [PostOfficeIDAccept] varchar(15)  NULL,
    [NumberOfPackage] nvarchar(10)  NULL,
    [Weight] float  NULL,
    [TripNumber] nvarchar(10)  NULL,
    [ArrivedDate] datetime  NULL,
    [TransportObjectID] varchar(15)  NULL,
    [SendDescription] nvarchar(200)  NULL,
    [AcceptDescription] nvarchar(200)  NULL,
    [PackingListStatus] int  NULL,
    [MailerType] varchar(1)  NULL,
    [UserGroupSend] nvarchar(20)  NULL,
    [UserGroupAccept] nvarchar(20)  NULL,
    [AcceptDate] datetime  NULL,
    [EmployeeSend] nvarchar(10)  NULL,
    [EmployeeAccept] nvarchar(10)  NULL,
    [UserGroupLastModified] nvarchar(20)  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL
);
GO

-- Creating table 'MM_PackingListDetail'
CREATE TABLE [dbo].[MM_PackingListDetail] (
    [DocumentID] varchar(15)  NOT NULL,
    [MailerID] varchar(20)  NOT NULL,
    [Accept] bit  NOT NULL,
    [SendNotes] nvarchar(200)  NULL,
    [AcceptNotes] nvarchar(200)  NULL,
    [AcceptDate] datetime  NULL,
    [LastEditDate] datetime  NULL,
    [CreationDate] datetime  NULL
);
GO

-- Creating table 'MM_TroubleTickets'
CREATE TABLE [dbo].[MM_TroubleTickets] (
    [TicketID] nvarchar(10)  NOT NULL,
    [CustomerID] nvarchar(20)  NULL,
    [TicketName] nvarchar(500)  NULL,
    [TicketDate] datetime  NULL,
    [EmployeeID] nvarchar(20)  NULL,
    [StatusID] int  NULL,
    [PostOfficeID] nvarchar(10)  NULL,
    [CreationDate] datetime  NULL,
    [LastUpdateDate] datetime  NULL,
    [Contact] nvarchar(100)  NULL
);
GO

-- Creating table 'UMS_GroupMenu'
CREATE TABLE [dbo].[UMS_GroupMenu] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Position] int  NULL,
    [Icon] nvarchar(128)  NULL,
    [IsActive] int  NULL
);
GO

-- Creating table 'UMS_Menu'
CREATE TABLE [dbo].[UMS_Menu] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Link] nvarchar(max)  NULL,
    [Icon] nvarchar(128)  NULL,
    [GroupMenuId] nvarchar(128)  NULL,
    [Position] int  NULL,
    [Code] nvarchar(128)  NULL
);
GO

-- Creating table 'UMS_MenuGroupUser'
CREATE TABLE [dbo].[UMS_MenuGroupUser] (
    [Id] nvarchar(128)  NOT NULL,
    [MenuId] nvarchar(128)  NULL,
    [GroupUserId] nvarchar(128)  NULL,
    [CanEdit] int  NULL
);
GO

-- Creating table 'UMS_UserGroups'
CREATE TABLE [dbo].[UMS_UserGroups] (
    [GroupID] nvarchar(128)  NOT NULL,
    [GroupName] nvarchar(200)  NULL,
    [IsActive] int  NULL
);
GO

-- Creating table 'UserLevels'
CREATE TABLE [dbo].[UserLevels] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Describes] nvarchar(max)  NULL
);
GO

-- Creating table 'UserPostOptions'
CREATE TABLE [dbo].[UserPostOptions] (
    [Id] nvarchar(128)  NOT NULL,
    [TUser] nvarchar(128)  NULL,
    [TPostId] nvarchar(129)  NULL
);
GO

-- Creating table 'MM_TakeDetails'
CREATE TABLE [dbo].[MM_TakeDetails] (
    [DocumentID] nvarchar(128)  NOT NULL,
    [MailerID] nvarchar(128)  NOT NULL,
    [TimeTake] datetime  NULL,
    [StatusID] int  NULL
);
GO

-- Creating table 'MM_TakeMailers'
CREATE TABLE [dbo].[MM_TakeMailers] (
    [DocumentID] nvarchar(128)  NOT NULL,
    [DocumentCode] nvarchar(128)  NULL,
    [Content] nvarchar(max)  NULL,
    [UserCreate] nvarchar(128)  NULL,
    [EmployeeID] nvarchar(128)  NULL,
    [CreateTime] datetime  NULL,
    [CustomerID] nvarchar(128)  NULL,
    [CustomerName] nvarchar(max)  NULL,
    [CustomerAddress] nvarchar(max)  NULL,
    [CustomerProvinceID] nvarchar(128)  NULL,
    [CustomerDistrictID] nvarchar(128)  NULL,
    [CustomerWardID] nvarchar(128)  NULL,
    [CustomerPhone] nvarchar(20)  NULL,
    [StatusID] int  NULL,
    [PostID] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [DocumentID] in table 'AC_CODDebitVoucher'
ALTER TABLE [dbo].[AC_CODDebitVoucher]
ADD CONSTRAINT [PK_AC_CODDebitVoucher]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'AC_CODDebitVoucherDetails'
ALTER TABLE [dbo].[AC_CODDebitVoucherDetails]
ADD CONSTRAINT [PK_AC_CODDebitVoucherDetails]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'AC_CustomerDebitVoucher'
ALTER TABLE [dbo].[AC_CustomerDebitVoucher]
ADD CONSTRAINT [PK_AC_CustomerDebitVoucher]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID], [MailerID] in table 'AC_CustomerDebitVoucherDetail'
ALTER TABLE [dbo].[AC_CustomerDebitVoucherDetail]
ADD CONSTRAINT [PK_AC_CustomerDebitVoucherDetail]
    PRIMARY KEY CLUSTERED ([DocumentID], [MailerID] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [LoginProvider], [ProviderKey] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [ProviderKey] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ContractID] in table 'BS_Contracts'
ALTER TABLE [dbo].[BS_Contracts]
ADD CONSTRAINT [PK_BS_Contracts]
    PRIMARY KEY CLUSTERED ([ContractID] ASC);
GO

-- Creating primary key on [CountryID] in table 'BS_Countries'
ALTER TABLE [dbo].[BS_Countries]
ADD CONSTRAINT [PK_BS_Countries]
    PRIMARY KEY CLUSTERED ([CountryID] ASC);
GO

-- Creating primary key on [CustomerGroupID] in table 'BS_CustomerGroups'
ALTER TABLE [dbo].[BS_CustomerGroups]
ADD CONSTRAINT [PK_BS_CustomerGroups]
    PRIMARY KEY CLUSTERED ([CustomerGroupID] ASC);
GO

-- Creating primary key on [CustomerID] in table 'BS_Customers'
ALTER TABLE [dbo].[BS_Customers]
ADD CONSTRAINT [PK_BS_Customers]
    PRIMARY KEY CLUSTERED ([CustomerID] ASC);
GO

-- Creating primary key on [GroupID], [ProvinceID] in table 'BS_Distants'
ALTER TABLE [dbo].[BS_Distants]
ADD CONSTRAINT [PK_BS_Distants]
    PRIMARY KEY CLUSTERED ([GroupID], [ProvinceID] ASC);
GO

-- Creating primary key on [DistrictID] in table 'BS_Districts'
ALTER TABLE [dbo].[BS_Districts]
ADD CONSTRAINT [PK_BS_Districts]
    PRIMARY KEY CLUSTERED ([DistrictID] ASC);
GO

-- Creating primary key on [EmployeeID] in table 'BS_Employees'
ALTER TABLE [dbo].[BS_Employees]
ADD CONSTRAINT [PK_BS_Employees]
    PRIMARY KEY CLUSTERED ([EmployeeID] ASC);
GO

-- Creating primary key on [DocumentID], [EmployeeID], [MailerID] in table 'BS_MailerModeraters'
ALTER TABLE [dbo].[BS_MailerModeraters]
ADD CONSTRAINT [PK_BS_MailerModeraters]
    PRIMARY KEY CLUSTERED ([DocumentID], [EmployeeID], [MailerID] ASC);
GO

-- Creating primary key on [PositionID] in table 'BS_Positions'
ALTER TABLE [dbo].[BS_Positions]
ADD CONSTRAINT [PK_BS_Positions]
    PRIMARY KEY CLUSTERED ([PositionID] ASC);
GO

-- Creating primary key on [PostOfficeID] in table 'BS_PostOffices'
ALTER TABLE [dbo].[BS_PostOffices]
ADD CONSTRAINT [PK_BS_PostOffices]
    PRIMARY KEY CLUSTERED ([PostOfficeID] ASC);
GO

-- Creating primary key on [PriceMatrixID] in table 'BS_PriceCustomers'
ALTER TABLE [dbo].[BS_PriceCustomers]
ADD CONSTRAINT [PK_BS_PriceCustomers]
    PRIMARY KEY CLUSTERED ([PriceMatrixID] ASC);
GO

-- Creating primary key on [GroupID] in table 'BS_PriceGroups'
ALTER TABLE [dbo].[BS_PriceGroups]
ADD CONSTRAINT [PK_BS_PriceGroups]
    PRIMARY KEY CLUSTERED ([GroupID] ASC);
GO

-- Creating primary key on [PriceMatrixID] in table 'BS_PriceMaTrixs'
ALTER TABLE [dbo].[BS_PriceMaTrixs]
ADD CONSTRAINT [PK_BS_PriceMaTrixs]
    PRIMARY KEY CLUSTERED ([PriceMatrixID] ASC);
GO

-- Creating primary key on [PriceMatrixID], [ServiceTypeID] in table 'BS_PriceServiceTypes'
ALTER TABLE [dbo].[BS_PriceServiceTypes]
ADD CONSTRAINT [PK_BS_PriceServiceTypes]
    PRIMARY KEY CLUSTERED ([PriceMatrixID], [ServiceTypeID] ASC);
GO

-- Creating primary key on [ProvinceID] in table 'BS_Provinces'
ALTER TABLE [dbo].[BS_Provinces]
ADD CONSTRAINT [PK_BS_Provinces]
    PRIMARY KEY CLUSTERED ([ProvinceID] ASC);
GO

-- Creating primary key on [PriceMatrixID], [RowIndex], [ColIndex] in table 'BS_RangeValues'
ALTER TABLE [dbo].[BS_RangeValues]
ADD CONSTRAINT [PK_BS_RangeValues]
    PRIMARY KEY CLUSTERED ([PriceMatrixID], [RowIndex], [ColIndex] ASC);
GO

-- Creating primary key on [GroupID], [FromWeight], [ToWeight] in table 'BS_RangeWeights'
ALTER TABLE [dbo].[BS_RangeWeights]
ADD CONSTRAINT [PK_BS_RangeWeights]
    PRIMARY KEY CLUSTERED ([GroupID], [FromWeight], [ToWeight] ASC);
GO

-- Creating primary key on [GroupID], [ZoneID] in table 'BS_RangeZones'
ALTER TABLE [dbo].[BS_RangeZones]
ADD CONSTRAINT [PK_BS_RangeZones]
    PRIMARY KEY CLUSTERED ([GroupID], [ZoneID] ASC);
GO

-- Creating primary key on [ReasonID] in table 'BS_ReturnReasons'
ALTER TABLE [dbo].[BS_ReturnReasons]
ADD CONSTRAINT [PK_BS_ReturnReasons]
    PRIMARY KEY CLUSTERED ([ReasonID] ASC);
GO

-- Creating primary key on [RouteID], [WardID] in table 'BS_RouteDetails'
ALTER TABLE [dbo].[BS_RouteDetails]
ADD CONSTRAINT [PK_BS_RouteDetails]
    PRIMARY KEY CLUSTERED ([RouteID], [WardID] ASC);
GO

-- Creating primary key on [RouteID] in table 'BS_Routes'
ALTER TABLE [dbo].[BS_Routes]
ADD CONSTRAINT [PK_BS_Routes]
    PRIMARY KEY CLUSTERED ([RouteID] ASC);
GO

-- Creating primary key on [ServiceID] in table 'BS_Services'
ALTER TABLE [dbo].[BS_Services]
ADD CONSTRAINT [PK_BS_Services]
    PRIMARY KEY CLUSTERED ([ServiceID] ASC);
GO

-- Creating primary key on [ServiceID] in table 'BS_ServiceTypes'
ALTER TABLE [dbo].[BS_ServiceTypes]
ADD CONSTRAINT [PK_BS_ServiceTypes]
    PRIMARY KEY CLUSTERED ([ServiceID] ASC);
GO

-- Creating primary key on [WardID] in table 'BS_Wards'
ALTER TABLE [dbo].[BS_Wards]
ADD CONSTRAINT [PK_BS_Wards]
    PRIMARY KEY CLUSTERED ([WardID] ASC);
GO

-- Creating primary key on [ZoneID] in table 'BS_Zones'
ALTER TABLE [dbo].[BS_Zones]
ADD CONSTRAINT [PK_BS_Zones]
    PRIMARY KEY CLUSTERED ([ZoneID] ASC);
GO

-- Creating primary key on [Id] in table 'CDatas'
ALTER TABLE [dbo].[CDatas]
ADD CONSTRAINT [PK_CDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GeneralCodeInfoes'
ALTER TABLE [dbo].[GeneralCodeInfoes]
ADD CONSTRAINT [PK_GeneralCodeInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [DocumentID] in table 'MM_CustomerMoneyAdvances'
ALTER TABLE [dbo].[MM_CustomerMoneyAdvances]
ADD CONSTRAINT [PK_MM_CustomerMoneyAdvances]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'MM_EmployeeDebitVoucher'
ALTER TABLE [dbo].[MM_EmployeeDebitVoucher]
ADD CONSTRAINT [PK_MM_EmployeeDebitVoucher]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID], [MailerID] in table 'MM_EmployeeDebitVoucherDetails'
ALTER TABLE [dbo].[MM_EmployeeDebitVoucherDetails]
ADD CONSTRAINT [PK_MM_EmployeeDebitVoucherDetails]
    PRIMARY KEY CLUSTERED ([DocumentID], [MailerID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'MM_EmployeeMoneyAdvances'
ALTER TABLE [dbo].[MM_EmployeeMoneyAdvances]
ADD CONSTRAINT [PK_MM_EmployeeMoneyAdvances]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [ID] in table 'MM_History'
ALTER TABLE [dbo].[MM_History]
ADD CONSTRAINT [PK_MM_History]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'MM_MailerDelivery'
ALTER TABLE [dbo].[MM_MailerDelivery]
ADD CONSTRAINT [PK_MM_MailerDelivery]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID], [MailerID] in table 'MM_MailerDeliveryDetail'
ALTER TABLE [dbo].[MM_MailerDeliveryDetail]
ADD CONSTRAINT [PK_MM_MailerDeliveryDetail]
    PRIMARY KEY CLUSTERED ([DocumentID], [MailerID] ASC);
GO

-- Creating primary key on [MailerID] in table 'MM_Mailers'
ALTER TABLE [dbo].[MM_Mailers]
ADD CONSTRAINT [PK_MM_Mailers]
    PRIMARY KEY CLUSTERED ([MailerID] ASC);
GO

-- Creating primary key on [MailerID], [ServiceID] in table 'MM_MailerServices'
ALTER TABLE [dbo].[MM_MailerServices]
ADD CONSTRAINT [PK_MM_MailerServices]
    PRIMARY KEY CLUSTERED ([MailerID], [ServiceID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'MM_PackingList'
ALTER TABLE [dbo].[MM_PackingList]
ADD CONSTRAINT [PK_MM_PackingList]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [DocumentID], [MailerID] in table 'MM_PackingListDetail'
ALTER TABLE [dbo].[MM_PackingListDetail]
ADD CONSTRAINT [PK_MM_PackingListDetail]
    PRIMARY KEY CLUSTERED ([DocumentID], [MailerID] ASC);
GO

-- Creating primary key on [TicketID] in table 'MM_TroubleTickets'
ALTER TABLE [dbo].[MM_TroubleTickets]
ADD CONSTRAINT [PK_MM_TroubleTickets]
    PRIMARY KEY CLUSTERED ([TicketID] ASC);
GO

-- Creating primary key on [Id] in table 'UMS_GroupMenu'
ALTER TABLE [dbo].[UMS_GroupMenu]
ADD CONSTRAINT [PK_UMS_GroupMenu]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UMS_Menu'
ALTER TABLE [dbo].[UMS_Menu]
ADD CONSTRAINT [PK_UMS_Menu]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UMS_MenuGroupUser'
ALTER TABLE [dbo].[UMS_MenuGroupUser]
ADD CONSTRAINT [PK_UMS_MenuGroupUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GroupID] in table 'UMS_UserGroups'
ALTER TABLE [dbo].[UMS_UserGroups]
ADD CONSTRAINT [PK_UMS_UserGroups]
    PRIMARY KEY CLUSTERED ([GroupID] ASC);
GO

-- Creating primary key on [Id] in table 'UserLevels'
ALTER TABLE [dbo].[UserLevels]
ADD CONSTRAINT [PK_UserLevels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserPostOptions'
ALTER TABLE [dbo].[UserPostOptions]
ADD CONSTRAINT [PK_UserPostOptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [DocumentID], [MailerID] in table 'MM_TakeDetails'
ALTER TABLE [dbo].[MM_TakeDetails]
ADD CONSTRAINT [PK_MM_TakeDetails]
    PRIMARY KEY CLUSTERED ([DocumentID], [MailerID] ASC);
GO

-- Creating primary key on [DocumentID] in table 'MM_TakeMailers'
ALTER TABLE [dbo].[MM_TakeMailers]
ADD CONSTRAINT [PK_MM_TakeMailers]
    PRIMARY KEY CLUSTERED ([DocumentID] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id]
ON [dbo].[AspNetUserClaims]
    ([User_Id]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EmployeeID] in table 'MM_MailerDelivery'
ALTER TABLE [dbo].[MM_MailerDelivery]
ADD CONSTRAINT [FK__MM_Mailer__Emplo__5BAD9CC8]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[BS_Employees]
        ([EmployeeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__MM_Mailer__Emplo__5BAD9CC8'
CREATE INDEX [IX_FK__MM_Mailer__Emplo__5BAD9CC8]
ON [dbo].[MM_MailerDelivery]
    ([EmployeeID]);
GO

-- Creating foreign key on [GroupMenuId] in table 'UMS_Menu'
ALTER TABLE [dbo].[UMS_Menu]
ADD CONSTRAINT [FK__UMS_Menu__GroupM__628FA481]
    FOREIGN KEY ([GroupMenuId])
    REFERENCES [dbo].[UMS_GroupMenu]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__UMS_Menu__GroupM__628FA481'
CREATE INDEX [IX_FK__UMS_Menu__GroupM__628FA481]
ON [dbo].[UMS_Menu]
    ([GroupMenuId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------