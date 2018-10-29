using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPOSTCOMMON
{
    public class MailerHandleCommon
    {
        MNPOSTEntities db;

        public MailerHandleCommon(MNPOSTEntities db)
        {
            this.db = db;
        }

        public string GeneralMailerCode(string postId)
        {
           

            var post = db.BS_PostOffices.Where(p => p.PostOfficeID == postId).FirstOrDefault();

            if (post == null)
            {
                return "";
            }

            var charFirst = post.AreaChar + DateTime.Now.ToString("ddMMyy");
            var codeSearch = "mailer" + post.AreaChar;

            var find = db.GeneralCodeInfoes.Where(p => p.Code == codeSearch && p.FirstChar == charFirst).FirstOrDefault();

            if (find == null)
            {
                var generalCode = new GeneralCodeInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    PreNumber = 0,
                    FirstChar = charFirst,
                    Code = codeSearch
                };
                db.GeneralCodeInfoes.Add(generalCode);
                db.SaveChanges();

                return GeneralMailerCode(postId);
            }

            var number = find.PreNumber + 1;

            string code = number.ToString();
            int count = 5;
            if (code.Count() < 5)
            {
                count = count - code.Count();

                while (count > 0)
                {
                    code = "0" + code;
                    count--;
                }
            }

            find.PreNumber = find.PreNumber + 1;
            db.Entry(find).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return find.FirstChar + code;

        }

    }
}
