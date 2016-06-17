using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HackFrontEnd.Database
{
    public class WeatherDbContext
    {
        private static WeatherDbContext _context;
        public List<Agent> Agents = new List<Agent>();
        public List<PolicyHolder> PolicyHolders = new List<PolicyHolder>();

        public WeatherDbContext()
        {
        }

        public static WeatherDbContext GetContext()
        {
            return _context ?? (_context = new WeatherDbContext());
        }
    }
}