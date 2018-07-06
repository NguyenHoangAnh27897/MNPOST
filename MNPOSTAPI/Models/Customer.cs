using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOSTAPI.Models
{
    public class Customer
    {
    }
    public class ParaLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        //public List<MaierDelivery> MailerDeliveryData { get; set; }
    }
    public class ResponeLogin
    {
        public string UserName { get; set; }
    }
}