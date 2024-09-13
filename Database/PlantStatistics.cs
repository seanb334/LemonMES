using LemonMES.Models;
namespace LemonMES.Database
{
    public class DefectiveProductsData
    {
        public string YearMonth { get; set; }
        public int Count { get; set; }
    }

    public class CompletedProductsData
    {
        public string YearMonth { get; set; }
        public double Count { get; set; }
    }

    public class CarbonCapturedData
    {
        public string YearMonth { get; set; }
        public double Count { get; set; }
    }

    public class PlantStatistics
    {
        private readonly PlantDbContext _context = default!;
        private const double CompletedProductMultiplier = 18.368;
        private const double CarbonCapturedMultiplier = 7.657;

        public PlantStatistics(PlantDbContext context)
        {
            _context = context;
        }

        // methods for the web page
        public List<DefectiveProductsData> GetDefectiveProductsDataLastYear()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddYears(-1);

            var defectiveProductsData = _context.Products
                .Where(p => p.CurrentState == Product.State.Defective &&
                            p.DateUpdated >= startDate && p.DateUpdated <= endDate)
                .GroupBy(p => new { p.DateUpdated.Year, p.DateUpdated.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .AsEnumerable() // Switch to client-side evaluation
                .Select(g => new DefectiveProductsData
                {
                    YearMonth = $"{g.Year}-{g.Month:D2}",
                    Count = g.Count
                })
                .ToList();

            return defectiveProductsData;
        }

        public List<CompletedProductsData> GetCompletedProductsDataLastYear()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddYears(-1);

            var completedProductsData = _context.Products
                .Where(p => p.CurrentStage == Product.ProdStage.OutStorage &&
                            p.CurrentState == Product.State.Completed &&
                            p.DateUpdated >= startDate && p.DateUpdated <= endDate)
                .GroupBy(p => new { p.DateUpdated.Year, p.DateUpdated.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .AsEnumerable() // Switch to client-side evaluation
                .Select(g => new CompletedProductsData
                {
                    YearMonth = $"{g.Year}-{g.Month:D2}",
                    Count = g.Count * CompletedProductMultiplier
                })
                .ToList();

            return completedProductsData;
        }

        public List<CarbonCapturedData> GetCarbonCapturedLastYear()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddYears(-1);

            var carbonCapturedData = _context.Products
                .Where(p => p.CurrentStage == Product.ProdStage.OutStorage &&
                            p.DateUpdated >= startDate && p.DateUpdated <= endDate)
                .GroupBy(p => new { p.DateUpdated.Year, p.DateUpdated.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .AsEnumerable() // Switch to client-side evaluation
                .Select(g => new CarbonCapturedData
                {
                    YearMonth = $"{g.Year}-{g.Month:D2}",
                    Count = g.Count * CarbonCapturedMultiplier
                })
                .ToList();

            return carbonCapturedData;
        }

        // console command functions for testing
        public int GetProductsInOutStoragePerMonth(int month, int year)
        {
            return _context.Products
                .Where(p => p.CurrentStage == Product.ProdStage.OutStorage &&
                            p.DateUpdated.Month == month &&
                            p.DateUpdated.Year == year)
                .Count();
        }

        public int GetDefectiveProductsPerMonth(int month, int year)
        {
            return _context.Products
                .Where(p => p.CurrentState == Product.State.Defective &&
                            p.DateUpdated.Month == month &&
                            p.DateUpdated.Year == year)
                .Count();
        }
        public double GetAverageTimeForCompletion(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var productsInOutStorage = _context.Products
                .Where(p => p.CurrentStage == Product.ProdStage.OutStorage && p.DateUpdated >= startDate && p.DateUpdated < endDate)
                .ToList();

            if (!productsInOutStorage.Any())
            {
                return 0;
            }

            var averageTime = productsInOutStorage
                .Average(p => (p.DateUpdated - p.DateCreated).TotalHours); // Calculate average time in hours

            return averageTime;
        }
    }
}
