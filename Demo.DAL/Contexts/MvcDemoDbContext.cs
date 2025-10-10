using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcDemoDbContext : IdentityDbContext<AuthUser>
    {
        public MvcDemoDbContext(DbContextOptions<MvcDemoDbContext> options):base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=>
        //    optionsBuilder.UseSqlServer("Server = JOHN; Database = MvcDemoDB; Trusted_Connection = true; TrustServerCertificate=True; "); // MultipleActiveResultSets = true;
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
    }
}
