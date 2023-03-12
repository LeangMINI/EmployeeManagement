using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
//using EmployeeManagement.Migrations;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using EmployeeManagement.Migrations;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles="User , Admin")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EmployeesController(ApplicationDbContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this._hostEnvironment = webHostEnvironment;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
              return _context.Employees != null ? 
                          View(await _context.Employees.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }


        [Authorize(Roles = " Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            string uniqueFileName = UploadFile(employee);
            employee.Image= uniqueFileName;
            _context.Attach(employee);
           // _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = " Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            return View(employees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Title,Email,Phone,Position,Address,ImageFile")] Employee employees)
        {
            if (id != employees.Id)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(employees);
                employees.Image = uniqueFileName;
                _context.Attach(employees);
                //string uniqueFileName = UploadFile(employees);
                employees.Image = uniqueFileName;
                _context.Attach(employees);
                try
                {
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employees);
        }

        [Authorize(Roles = " Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /* if (_context.Employees == null)
             {
                 return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
             }*/
            var employees = await _context.Employees.FindAsync(id);
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "~/Images/Employee", employees.Image);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            if (employees != null)
            {
                _context.Employees.Remove(employees);

            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private string UploadFile(Employee employee)
        {
            string uniqueFileName = null;
            if (employee.ImageFile != null)
            {
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Employee/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + employee.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    employee.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
