using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOST.Models
{
    public class MailerRpt
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
    }
}