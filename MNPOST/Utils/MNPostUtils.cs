using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;
using System.Net;

namespace MNPOST.Utils
{
    public static class MNPostUtils
    {

    }

    public class SendPartnerHandle
    {

        public void SendViettelPost(MNPOSTEntities db, string documentId)
        {
            string url = @"https://api.viettelpost.vn/api/tmdt/InsertOrderDraft";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Headers["Authorization"] = "key=AAAAIr_-D4Q:APA91bHvZRkw_Y0UHBoPa3-HUq1iN41Y5Bhtta0MuSxMBEazsvxLPZM4kIgufKkmvp-3yWMKy6QK9l4wTJd-eymKdJtOFjVQEwJmswqp4YseGq-ylFPvkwOsE3NzpbV6kJEQubWBBUeP";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(json);

            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";

        }


        public 


    }


    public class 
}