using LemonMES.Models;
using Microsoft.AspNetCore.Mvc;
using static LemonMES.Models.Product;

namespace LemonMES.Database
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOperability : ControllerBase
    {
        private readonly PlantDbContext _context;

        public UnitOperability(PlantDbContext context)
        {
            _context = context;
        }

        [HttpPost("ToggleUnitStatus")]
        public IActionResult ToggleUnitStatus(Product.ProdStage stage)
        {
            var unitStatus = _context.UnitStatus.SingleOrDefault(u => u.Stage == stage);
            if (unitStatus == null)
            {
                return BadRequest(new { success = false });
            }

            unitStatus.Status = unitStatus.Status == 1 ? 0 : 1;
            _context.SaveChanges();
            
            return Ok(new { success = true, isActive = unitStatus.Status == 1 });
        }

        [HttpGet("GetStatus")]
        public IActionResult GetStatus(Product.ProdStage stage)
        {
            var unitStatus = _context.UnitStatus.SingleOrDefault(u => u.Stage == stage);
            if (unitStatus == null)
            {
                return NotFound(new { success = false });
            }

            return Ok(new { success = true, isActive = unitStatus.Status == 1 });
        }
    }
}
