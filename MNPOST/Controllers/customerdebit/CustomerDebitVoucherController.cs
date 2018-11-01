using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.customerdebit
{
    public class CustomerDebitVoucherController : BaseController
    {
        // GET: CustomerDebitVoucher
        [HttpGet]
        public ActionResult Show()
        {
            ViewBag.CustomerGroup = db.BS_CustomerGroups.Select(p => new
            {
                code = p.CustomerGroupID,
                name = p.CustomerGroupName
            }).ToList();
            ViewBag.ToDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
            ViewBag.FromDate = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");

            return View();
        }
       
    }
}