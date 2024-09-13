using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LemonMES.Models;
using LemonMES.Database;

namespace LemonMES.Pages
{
    public class PlantOperationDataModel : PageModel
    {
        private readonly PlantStatistics _plantStatistics;

        public PlantOperationDataModel(PlantStatistics plantStatistics)
        {
            _plantStatistics = plantStatistics;
        }

        public JsonResult OnGetDefectiveProductsData()
        {
            var data = _plantStatistics.GetDefectiveProductsDataLastYear();
            return new JsonResult(data);
        }

        public JsonResult OnGetCompletedProductsData()
        {
            var data = _plantStatistics.GetCompletedProductsDataLastYear();
            return new JsonResult(data);
        }

        public JsonResult OnGetCarbonCapturedData()
        {
            var data = _plantStatistics.GetCarbonCapturedLastYear();
            return new JsonResult(data);
        }

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != Roles.RoleType.BusinessUser.ToString())
            {
                return RedirectToPage("/AccessDenied");
            }

            return Page();
        }
    }
}
