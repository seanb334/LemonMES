using LemonMES.Database;
using LemonMES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LemonMES.Models.Product;

namespace LemonMES.Pages
{
    public class ShopFloorManagementModel : PageModel
    {
        private readonly PlantDbContext _context;

        public ShopFloorManagementModel(PlantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult OnGetProductsByStage(string stage)
        {
            if (Enum.TryParse<ProdStage>(stage, out var parsedStage))
            {
                var products = _context.Products
                    .Where(p => p.CurrentStage == parsedStage)
                    .Select(p => new
                    {
                        serialnumber = p.SerialNumber,
                        currentstate = ((State)p.CurrentState).ToString(),
                        datecreated = p.DateCreated.ToString("MM-dd-yyyy, hh:mm:ss tt"),
                        dateupdated = p.DateUpdated.ToString("MM-dd-yyyy, hh:mm:ss tt"),
                        description = p.Description,
                    })
                    .ToList();

                var status = _context.UnitStatus
                    .Where(us => us.Stage == parsedStage)
                    .Select(us => us.Status)
                    .FirstOrDefault();

                return new JsonResult(new { products, status });
            }
            else
            {
                return new JsonResult(new { error = "Invalid stage value" });
            }
        }

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != Roles.RoleType.LeadOperator.ToString()&& userRole != Roles.RoleType.Operator.ToString())
            {
                
                return RedirectToPage("/AccessDenied");
            }

            return Page();
        }
    }
}
