using LemonMES.Database;
using LemonMES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LemonMES.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LogInFunctions _loginFunctions;

        public IndexModel(LogInFunctions loginFunctions)
        {
            _loginFunctions = loginFunctions;
        }
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public bool IsLoggedIn { get; set; } = false;

        public string UserRole { get; set; }

        public IActionResult OnPost(string username, string password)
        {
            if (_loginFunctions.CheckPassword(username, password))
            {
                var user = _loginFunctions.GetUser(username);

                HttpContext.Session.SetString("UserRole",user.Role.ToString());
                IsLoggedIn = true;
                UserRole = user.Role.ToString();

                return Page();
                //if (user.Role == Roles.RoleType.Operator || user.Role == Roles.RoleType.LeadOperator)
                //{
                //    return RedirectToPage("/ShopFloorManagement");
                //}
                //else if (user.Role == Roles.RoleType.BusinessUser)
                //{
                //    return RedirectToPage("/PlantOperationData");
                //}
                //else if (user.Role == Roles.RoleType.Engineer)
                //{
                //    return RedirectToPage("/UnitOperabilityStatus");
                //}
                //else
                //{
                //    ViewData["ErrorMessage"] = "Access Denied. Unauthorized role.";
                //    return RedirectToPage("/Index");
                //}
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return Page();
            }
        }

        public void OnGet()
        {

        }
    }
}
