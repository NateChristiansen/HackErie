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
        private readonly WeatherDbContext _database = new WeatherDbContext
        {
            Agents = new List<Agent>
            {
                new Agent
                {
                    Email = "natechristiansen42@gmail.com",
                    Firstname = "Nathan",
                    Lastname = "Christiansen",
                    Phone = "8144906855@tmomail.net"
                }
            },
            PolicyHolders = new List<PolicyHolder>
            {
                new PolicyHolder
                {
                    Email = "pro585g@gmail.com",
                    Firstname = "Michael",
                    Lastname = "Stumpf",
                    AgentId = 0,
                    Phone = "7248820177@vtext.com"
                },
                new PolicyHolder
                {
                    Firstname = "Stephen",
                    Lastname = "Caulfield",
                    AgentId = 0,
                    Email = "stevepc95@gmail.com",
                    Phone = "8148822774@txt.att.net"
                },
                new PolicyHolder
                {
                    Firstname = "Colin",
                    Lastname = "Kimball",
                    AgentId = 0,
                    Email = "ckimball11@gmail.com",
                    Phone = "8144906405@vtext.com"
                }
            }
        };
        // GET: Home
        public ActionResult Index()
        {
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
                    SendEmail(agent.Email, "Catastrophe Alert",
                        "Alert: " + agent.Firstname +
                        ", we are informing you that a catastrophe is about to strike your area.");
                    SendEmail(agent.Phone, "Catastrophe Alert",
                        "Alert: " + agent.Firstname +
                        ", we are informing you that a catastrophe is about to strike your area.");
                }
                foreach (var policyHolder in _database.PolicyHolders)
                {
                    SendEmail(policyHolder.Email, "Catastrophe Alert",
                        "Alert: " + policyHolder.Firstname +
                        ", we are informing you that a catastrophe is about to strike your area.");
                    SendEmail(policyHolder.Phone, "Catastrophe Alert",
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