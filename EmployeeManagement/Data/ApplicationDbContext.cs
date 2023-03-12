using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
       
    }
}
