using MNPOSTAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MNPOSTAPI.Controllers.mobile.mailer
{
    public class MailerAPIController : BaseMobileController
    {

        MailerPresenter presenter = new MailerPresenter();


        [HttpGet]
        public ResultInfo GetDeliveryByEmployee(string employeeId)
        {

            var result = presenter.GetListDelivery(employeeId);



            return result;

        }

        [HttpGet]
        public ResultInfo GetDeliveryEmployeeHistory(string employeeId, string date)
        {
            return presenter.GetListHistoryDelivery(employeeId, date);
        }

        [HttpGet]
        public ResultInfo GetReturnReasons()
        {

            return presenter.GetListReturnReason();
        }

        [HttpPost]
        public ResultInfo UpdateDelivery()
        {

            var result = new ResultInfo()
            {
                error = 0,
                msg = "success"
            };

            try
            {

                var requestContent = Request.Content.ReadAsStringAsync().Result;
                var jsonserializer = new JavaScriptSerializer();
                var paser = jsonserializer.Deserialize<UpdateDeliveryReceive>(requestContent);

                var user = User.Identity.Name;

                result = presenter.UpdateDelivery(paser, user);

            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;
        }


        ///lay hang
        ///
        [HttpGet]
        public ResultInfo GetTakeMailerInDay()
        {
            var user = User.Identity.Name;
            var data = presenter.GetTakeMailer(user, DateTime.Now.ToString("yyyy-MM-dd"), 7);

            return data;
        }

        [HttpGet]
        public ResultInfo GetTakeMailery(int status, string date)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = DateTime.ParseExact(date, "dd/M/yyyy", null);
            }
            catch
            {
                dateTime = DateTime.Now;
            }

            var user = User.Identity.Name;
            var data = presenter.GetTakeMailer(user, dateTime.ToString("yyyy-MM-dd"), status);

            return data;
        }

        [HttpGet]
        public ResultInfo GetDetails(string documentID)
        {
            return presenter.GetTakeMailerDetail(documentID);
        }

        [HttpPost]
        public ResultInfo UpdateTakeMailer()
        {
            var result = new ResultInfo()
            {
                error = 0,
                msg = "success"
            };
            try
            {

                var requestContent = Request.Content.ReadAsStringAsync().Result;
                var jsonserializer = new JavaScriptSerializer();
                var paser = jsonserializer.Deserialize<UpdateTakeMailerReceive>(requestContent);

                var user = User.Identity.Name;

                result = presenter.UpdateTakeMailer(user, paser);

            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;

        }

        [HttpGet]
        public ResponseInfo GetReportDelivery(string employeeId, string codeTime)
        {
            // Day 
            // Yesterday
            // Week
            // Month
            // LastMonth

            var fDate = DateTime.Now;
            var tDate = DateTime.Now;

            switch (codeTime)
            {
                case "Day":
                    break;
                case "Yesterday":
                    fDate.AddDays(-1);
                    tDate.AddDays(-1);
                    break;
                case "Week":
                    int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
                    var firstWeekCreate = FirstDateOfWeekISO8601(DateTime.Now.Year, CurrentWeek);
                    fDate = firstWeekCreate;
                    tDate = firstWeekCreate.AddDays(7);
                    break;
                case "Month":
                    fDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    tDate = fDate.AddMonths(1).AddDays(-1);
                    break;
                case "LastMonth":
                    fDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    tDate = fDate.AddMonths(1).AddDays(-1);
                    break;
                default:
                    break;
            }

            return presenter.GetReportDelivert(employeeId, fDate.ToString("yyyy-MM-dd"), tDate.ToString("yyyy-MM-dd"));
        }


        //
        private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }


        private List<DateTime> DateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                             .Select(d => fromDate.AddDays(d)).ToList();
        }

        private int GetDaysInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);

            return days;
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
