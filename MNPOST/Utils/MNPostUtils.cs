using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;

namespace MNPOST.Utils
{
    public static class MNPostUtils
    {
        private static string sendRequestFirebase(string json)
        {
            string url = @"https://fcm.googleapis.com/fcm/send";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Headers["Authorization"] = "key=AAAA1AAQLPs:APA91bFHEIuwLIxqPGCE1C80hUHMY7HriBgy0S-Q727JfC97baSIJLbIE4GQH20ZkmLGDNuqT8EDK6tSNC6E4CHoTUp-aMx1ia_j9z9W_ww81exZhFsudD_YLmWnO_AAx2aP02D-44hG";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(json);

            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            long length = 0;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    var stream = response.GetResponseStream();
                    var reader = new StreamReader(stream, encoding);
                    var responseString = reader.ReadToEnd();

                    return responseString;
                }
            }
            catch
            {
                return null;
            }

        }


        public static void SendUser(string title, string content, string user)
        {
            MNPOSTEntities db = new MNPOSTEntities();

            var firebaseInfo = db.FirebaseIDSaves.Where(p => p.UserID == user).FirstOrDefault();
            title = title.ToUpper();
            if (firebaseInfo != null)
            {
                string json = "{ \"notification\": {\"click_action\": \"OPEN_ACTIVITY_1\" ,\"title\": \"" + title + "\",\"body\": \"" + content + "\"},\"data\": {\"title\": \"'" + title + "'\",\"message\": \"'" + content + "'\"},\"to\": \"" + firebaseInfo.FirebaseID + "\"}";
                var responseString = sendRequestFirebase(json);
            }

            NoticeSave ins = new NoticeSave()
            {
                Content = content,
                CreateTime = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                IsRead = false,
                NType = user,
                Title = title
            };

            db.NoticeSaves.Add(ins);
            db.SaveChanges();
        }
    }


}