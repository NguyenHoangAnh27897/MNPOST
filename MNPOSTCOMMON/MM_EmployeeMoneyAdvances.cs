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
    using System.Collections.Generic;
    
    public partial class MM_EmployeeMoneyAdvances
    {
        public string DocumentID { get; set; }
        public string AcceptID { get; set; }
        public Nullable<System.DateTime> AcceptDate { get; set; }
        public string EmployeeID { get; set; }
        public string PostOfficeID { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<bool> StatusID { get; set; }
        public Nullable<decimal> ReturnMoney { get; set; }
        public string MailerID { get; set; }
        public Nullable<int> Type { get; set; }
    }
}
