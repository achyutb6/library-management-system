using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management_System.Controllers
{
    public class FinesController : Controller
    {
        // GET: Fines
        public ActionResult Index()
        {

            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
        
            return View(uf.GetFineDue());
        }

        public ActionResult PayFine()
        {

            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();

            return View(uf.GetAllFines());
        }

        public ActionResult Pay(string id)
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            uf.Pay(id);
            return RedirectToAction("PayFine");
        }

        public ActionResult UpdateFines()
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();

            uf.UpdateFines();
            return RedirectToAction("Index");
        }
    }
}