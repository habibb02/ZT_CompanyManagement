using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyManagement.DAL;
using CompanyManagement.Models;


namespace CompanyManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Insert()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
    }
}