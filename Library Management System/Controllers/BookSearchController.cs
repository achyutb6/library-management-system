using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management_System.Controllers
{
    public class BookSearchController : Controller
    {

        // GET: BookSearch
        public ActionResult Index()
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            return View(uf.BookSearch(""));
        }
        [HttpPost]
        public ActionResult Index(String text)
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            return View(uf.BookSearch(text));
        }

        public ActionResult Checkout(string id,bool? error)
        {
            if (error != null)
            {
                if (error == true)
                {
                    ViewBag.Message = "The borrower already has checked out 3 book. Cannot checkout any more books!";
                }
            }

            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            BookSearch book = uf.BookSearch(id).First(); 
            return View(book);
        }

        [HttpPost]
        public ActionResult Checkout(string cardid,string isbn)
        {
            Repo.UtilityFunctions uf = new Repo.UtilityFunctions();
            if(!uf.LoanBook(cardid, isbn))
            {
                return RedirectToAction("Checkout",new { id = isbn, error = true });
            }
            return RedirectToAction("Index");
        }
    }
}