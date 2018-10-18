using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPOSTCOMMON
{
    public class MNHistory
    {
        MNPOSTEntities db = new MNPOSTEntities();

        public void AddHistory(int statusId, string postId, string employeeId, string employeeName)
        {

        }

        public void AddTracking(int statusId, string mailerId, string postId, string describe)
        {
            var tracking = new MM_Tracking()
            {
                Id = Guid.NewGuid().ToString(),
                MailerID = mailerId,
                CreateTime = DateTime.Now,
                PostOffice = postId,
                StatusID = statusId,
                Describe = describe
            };


            db.MM_Tracking.Add(tracking);

            db.SaveChanges();

        }
    }


}
