using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;
namespace MNPOSTAPI.Models
{
    public class MailerInfoResultbyID : ResultInfo
    {
        public MM_Mailers mailer { get; set; }
    }
    public class MailerInfoResult : ResultInfo
    {
        public List<MM_Mailers> mailer { get; set; }
    }
    public class AddMailerRequest : RequestInfo
    {
        public MM_Mailers mailer { get; set; }
    }

    public class RequestData : RequestInfo
    {
        public Object data { get; set; }
    }
    public class CalPrice : ResultInfo
    {
        public decimal Price { get; set; }
    }
}