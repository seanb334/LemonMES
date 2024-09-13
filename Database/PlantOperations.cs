using LemonMES.Models;
using Microsoft.AspNetCore.Mvc;
namespace LemonMES.Database
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantOperations : ControllerBase
    {
        private readonly PlantDbContext _context = default!;

        public PlantOperations(PlantDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetProductsByStage")]
        public JsonResult GetProductsByStage(string stage)
        {
            if (Enum.TryParse<Product.ProdStage>(stage, out var parsedStage))
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddMonths(-1);
                if (parsedStage==Product.ProdStage.OutStorage)
                {
                    Console.WriteLine($"Stage received and parsed: {parsedStage}");
                    var products = _context.Products
                        .Where(p => p.CurrentStage == parsedStage && 
                                    p.DateUpdated >= startDate )
                        .Select(p => new
                        {
                            serialnumber = p.SerialNumber,
                            currentstate = ((Product.State)p.CurrentState).ToString(),
                            datecreated = p.DateCreated.ToString("MM-dd-yyyy, hh:mm:ss tt"),
                            dateupdated = p.DateUpdated.ToString("MM-dd-yyyy, hh:mm:ss tt"),
                            description = p.Description,
                        })
                        .ToList();
                    return new JsonResult(products);
                }
                else
                {
                    Console.WriteLine($"Stage received and parsed: {parsedStage}");
                    var products = _context.Products
                        .Where(p => p.CurrentStage == parsedStage)
                        .Select(p => new
                        {
                            serialnumber = p.SerialNumber,
                            currentstate = ((Product.State)p.CurrentState).ToString(),
                            datecreated = p.DateCreated.ToString("MM-dd-yyyy, hh:mm:ss tt"),
                            dateupdated = p.DateUpdated.ToString("MM-dd-yyyy, hh:mm:ss tt"),
                            description = p.Description,
                        })
                        .ToList();
                    return new JsonResult(products);
                }
            }
            else
            {
                Console.WriteLine("Failed to parse stage.");
                return new JsonResult(new { error = "Invalid stage value" });
            }
        }


        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] ProductCreateModel model)
        {
            var testPro = _context.Products.SingleOrDefault(p => p.SerialNumber == model.SerialNumber);
            var unitStatus = _context.UnitStatus.SingleOrDefault(u => u.Stage == Product.ProdStage.InStorage);
            if (testPro == null && unitStatus.Status>0 && model.SerialNumber != "" && model.SerialNumber != null)
            {
                var product = new Product(model.SerialNumber, model.Description);
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok(new { success = true });
            }
            else
            {
                if (testPro == null)
                {
                    ModelState.AddModelError(string.Empty, "Serial number already in use, please use another");
                    return BadRequest(new { success = false, message = "Serial number already in use" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid serial number.");
                    return BadRequest(new { success = false, message = "In-Storage is inactive" });
                }

            }
            
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] ProductUpdateModel model)
        {
            var product = _context.Products.SingleOrDefault(p => p.SerialNumber == model.SerialNumber);
            if (product == null)
            {
                return NotFound(new { success = false, message = "Product not found" });
            }
            var unitStatusNow = _context.UnitStatus.SingleOrDefault(u => u.Stage == product.CurrentStage);
            Enum.TryParse<Product.ProdStage>(model.Stage, out var stage);
            Enum.TryParse<Product.State>(model.State, out var state);
            var unitStatusNext = _context.UnitStatus.SingleOrDefault(u => u.Stage == stage);

            if (unitStatusNow.Status < 1)
            {
                return BadRequest(new { success = false, message = "Product is on inoperable unit" });
            }
            else if (unitStatusNext.Status < 1)
            {
                return BadRequest(new { success = false, message = "Product is moving to inoperable unit" });
            }
            else
            {
                var roles = new Roles(Roles.RoleType.Operator, product.CurrentStage); // already logged into page

                bool updateSuccess = product.UpdateDescription(model.Description, roles, product.CurrentStage);
                if (updateSuccess)
                {
                    product.CurrentState = state;
                    product.CurrentStage = stage;
                    product.DateUpdated = DateTime.Now;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    return Ok(new { success = true });
                }
                else
                {
                    return Forbid();
                }
            }
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct([FromQuery] string serialNumber)
        {
            var product = _context.Products.SingleOrDefault(p => p.SerialNumber == serialNumber);
            if (product == null)
            {
                return NotFound(new { success = false, message = "Product not found" });
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok(new { success = true });
        }


    }

    public class ProductCreateModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SerialNumber { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }

    public class ProductUpdateModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SerialNumber { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Stage { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string State { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
