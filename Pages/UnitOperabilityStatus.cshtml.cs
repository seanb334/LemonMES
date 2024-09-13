using LemonMES.Database;
using LemonMES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LemonMES.Models.Product;
using static LemonMES.Pages.UnitOperabilityStatusModel;

namespace LemonMES.Pages
{
    public class UnitOperabilityStatusModel : PageModel
    {
        public Dictionary<Product.ProdStage, int> UnitStatuses { get; private set; }
        public Dictionary<ProdStage, ButtonPosition> ButtonPositions { get;  set; }
        public class ButtonPosition
        {
            public int Top { get; set; }
            public int Left { get; set; }
        }

        private readonly PlantDbContext _context;

        public UnitOperabilityStatusModel(PlantDbContext context)
        {
            _context = context;
            UnitStatuses = new Dictionary<ProdStage, int>();
            ButtonPositions = new Dictionary<ProdStage, ButtonPosition>
            {
                { ProdStage.InStorage, new ButtonPosition { Top = 38, Left = 7 } },
                { ProdStage.MixingTank, new ButtonPosition { Top = 85, Left = 15} },
                { ProdStage.ReactorOne, new ButtonPosition { Top = 156, Left = 20 } },
                { ProdStage.ReactorTwo, new ButtonPosition { Top = 150, Left = 100 } },
                { ProdStage.ReactorThree, new ButtonPosition { Top = 144, Left = 188 } },
                { ProdStage.ReactorFour, new ButtonPosition { Top = 138, Left = 288 } },
                { ProdStage.ReactorFive, new ButtonPosition { Top = 119, Left = 430 } },
                { ProdStage.RotaryFilter, new ButtonPosition { Top = 236, Left = 512 } },
                { ProdStage.HoldingTank, new ButtonPosition { Top = 300, Left = 330 } },
                { ProdStage.Concentrator, new ButtonPosition { Top = 310, Left = 128 } },
                { ProdStage.Extractor, new ButtonPosition { Top = 475, Left = 326 } },
                { ProdStage.Evaporator, new ButtonPosition { Top = 526, Left = 495 } },
                { ProdStage.Crystalizer, new ButtonPosition { Top = 418, Left = 668 } },
                { ProdStage.Dryer, new ButtonPosition { Top = 518, Left = 770 } },
                { ProdStage.OutStorage, new ButtonPosition { Top = 522, Left = 915 } }
            };

        }
        public IActionResult OnGet()
        {

            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != Roles.RoleType.Engineer.ToString())
            {
                return RedirectToPage("/AccessDenied");
            }

            UnitStatuses = _context.UnitStatus.ToDictionary(
                u => (Product.ProdStage)u.Stage,
                u => u.Status);
            return Page();
        }
    }
}
