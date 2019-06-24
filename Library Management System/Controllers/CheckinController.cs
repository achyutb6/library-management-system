using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management_System.Controllers
{
    public class CheckinController : Controller
    {
        public ActionResult Index()
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            return View(uf.GetBookLoans(""));
        }
        [HttpPost]
        public ActionResult Index(String text)
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            return View(uf.GetBookLoans(text));
        }

        public ActionResult Checkin(string id)
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            uf.Checkin(id);
            return RedirectToAction("Index");
        }
    }
}