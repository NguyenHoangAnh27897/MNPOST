using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MNPOSTCOMMON;
using System.Web.Script.Serialization;
using MNPOSTWEBSITE.Models;
using MNPOSTWEBSITE.Controllers;
using MNPOSTWEBSITEMODEL;
using PagedList;
using PagedList.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace MNPOSTWEBSITE.Controllers
{
    public class CODController : Controller
    {
        // GET: COD
        public ActionResult Index()
        {
            return View();
        }
    }
}