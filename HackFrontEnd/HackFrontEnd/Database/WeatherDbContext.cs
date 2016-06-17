using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HackFrontEnd.Database
{
    public class WeatherDbContext : DbContext
    {
        private static WeatherDbContext _context;
        public DbSet<Agent> Agents { get; set; }
        public DbSet<PolicyHolder> PolicyHolders { get; set; }

        private WeatherDbContext()
        {
        }

        public static WeatherDbContext GetContext()
        {
            return _context ?? (_context = new WeatherDbContext());
        }
    }
}