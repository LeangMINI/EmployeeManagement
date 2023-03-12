using EmployeeManagement.Controllers;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Title Title { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Position Position { get; set; }
        public string? Address { get; set; }
        
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set;}

        
    }
}
