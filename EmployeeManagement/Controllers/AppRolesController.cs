using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "User , Admin")]
    public class AppRolesController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        //List All the Roles create by user
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        [Authorize(Roles = " Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
       
        public async Task< IActionResult> Create(IdentityRole model)
        {
            //avoid duplicate role

            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {

                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
               
            }

            return RedirectToAction("Index");
        }
    }

}
