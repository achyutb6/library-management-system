using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management_System.Controllers
{
    public class BorrowerController : Controller
    {
        // GET: Borrower
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(Borrower borrower)
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            if (!uf.CreateBorrower(borrower))
            {
                ViewBag.Message = "Borrower with SSN : "+borrower.SSN + " already exists!";
            }
            return View();
        }
    }
}