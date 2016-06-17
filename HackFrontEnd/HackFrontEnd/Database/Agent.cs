using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackFrontEnd.Database
{
    public class Agent
    {
        public int AgentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}