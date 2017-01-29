using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedisWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session.Add("Today", DateTime.UtcNow.DayOfWeek);

            if (Session["Something"] == null)
                ViewBag.SomethingImportant = "Not from session";
            else
                ViewBag.SomethingImportant = Session["Something"];

            return View();
        }

        public ActionResult BasicExample()
        {
            if (Session["Something"] == null)
                ViewBag.SomethingImportant = "Not from session";
            else
                ViewBag.SomethingImportant = Session["Something"];

            return View();
        }

        public ActionResult SessionExample()
        {
            ViewBag.Today = Session["Today"];

            if (Session["Something"] == null)
                ViewBag.SomethingImportant = "Not from session";
            else
                ViewBag.SomethingImportant = Session["Something"];

            return View();
        }

        public ActionResult AbandonExample()
        {
            Session.Abandon();

            return View();
        }

        [HttpPost]
        public ActionResult SaveSession(string NewValue)
        {
            Session["Something"] = NewValue;

            return RedirectToAction("SessionExample");
        }
    }
}