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
        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersViewModel user)
        {
            using (CompanyMNGEntities db = new CompanyMNGEntities())
            {
                var loginUser = db.User.Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password)).FirstOrDefault();

                if (loginUser != null)
                {
                    Session["IdUser"] = loginUser.IdUser.ToString();
                    Session["UserName"] = loginUser.UserName.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["State"] = "err";
                    return View(user);
                }
            }
        }
    }
}