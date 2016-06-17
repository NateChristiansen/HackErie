using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HackFrontEnd.Database;

namespace HackFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherDbContext _database = WeatherDbContext.GetContext();
        // GET: Home
        public ActionResult Index()
        {
            if (_database.Agents.Any()) return View();
            _database.Agents.Add(new Agent
            {
                Email = "natechristiansen42@gmail.com",
                Firstname = "Nathan",
                Lastname = "Christiansen"
            });
            _database.PolicyHolders.AddRange(new List<PolicyHolder>
            {
                new PolicyHolder
                {
                    Email = "pro585g@gmail.com",
                    Firstname = "Michael",
                    Lastname = "Stumpf",
                    AgentId = 0
                },
                new PolicyHolder
                {
                    Firstname = "Stephen",
                    Lastname = "Caulfield",
                    AgentId = 0,
                    Email = "stevepc95@gmail.com"
                },
                new PolicyHolder
                {
                    Firstname = "Colin",
                    Lastname = "Kimball",
                    AgentId = 0,
                    Email = "ckimball11@gmail.com"
                }
            });
            _database.SaveChanges();
            return View();
        }
        
        public ActionResult PotentialCat()
        {
            try
            {
                SendEmail("natechristiansen42@gmail.com", "Potential Catastrophe", "ALERT: There is a potential catastrophe, please log in to  to confirm");
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirmed()
        {
            try
            {
                foreach (var agent in _database.Agents)
                {
                    SendEmail(agent.Email, "Catastrophe Alert", "There is a catastrophe");
                }
                foreach (var policyHolder in _database.PolicyHolders)
                {
                    SendEmail(policyHolder.Email, "Catastrophe Alert",
                        "Alert: " + policyHolder.Firstname +
                        ", we are informing you that a catastrophe is about to strike your area.");
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private static void SendEmail(string email, string subject, string body)
        {
            var smptClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("hackerie1234@gmail.com", "hackable"),
                EnableSsl = true
            };
            smptClient.Send("hackerie1234@gmail.com", email, subject, body);
        }
    }
}