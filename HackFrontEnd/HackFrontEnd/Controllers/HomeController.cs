using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HackFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult PotentialCat()
        {
            try
            {
                var smptClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("hackerie1234@gmail.com", "hackable"),
                    EnableSsl = true
                };
                smptClient.Send("hackerie1234@gmail.com", "natechristiansen42@gmail.com", "Testing Email", "testing the email");
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AlertEveryone()
        {
            return Json(null);
        }
    }
}