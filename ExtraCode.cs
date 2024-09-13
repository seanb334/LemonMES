using LemonMES.Database;
using LemonMES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static LemonMES.Models.Roles;

namespace LemonMES
{
    public class ExtraCode
    {
        //var options = new DbContextOptionsBuilder<PlantDbContext>()
        //        .UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;;TrustServerCertificate=true")
        //        .Options;

        //    // Create an instance of PlantDbContext
        //    using (var context = new PlantDbContext(options))
        //    {
        //        // Create an instance of LogInFunctions
        //        var logInFunctions = new LogInFunctions(context);
        //        var plantOperations = new PlantOperations(context);
        //        var plantStatistics = new PlantStatistics(context);

                // Create a new user
                //var georgey = new User("georgeyrules", "wrc24", RoleType.BusinessUser);
                //var bryant = new User("BryantRuff1", "PastaPasta123", RoleType.Engineer);
                //var aidan = new User("AidanTer1", "codecode123", RoleType.Engineer);
                //var sean = new User("SeanB333", "operatortime", RoleType.LeadOperator);
                //logInFunctions.AddUser(kavya);
                //logInFunctions.AddUser(sean);
                //logInFunctions.AddUser(aidan);
                //logInFunctions.AddUser(georgey);
                //for (int i = 1; i < 15; i++)
                //{
                //    var userToAdd = new User("Operator" + i.ToString(), "Operator" + i.ToString(), RoleType.Operator, (Product.ProdStage)i);
                //    logInFunctions.AddUser(userToAdd);

                //}

                // Example: Add a new product
                //var newProduct1 = new Product("ISN-00013", "Kavya's Product");
                //plantOperations.AddProduct(newProduct1);

                //// Example: Complete some products
                //var completeProduct1 = plantOperations.GetProduct("ISN-00010");
                //if ((completeProduct1 != null)
                //{
                //    completeProduct1.UpdateStage(Product.ProdStage.ReactorFive, new Roles(Roles.RoleType.LeadOperator), Product.State.Completed);
                //    plantOperations.UpdateProduct(completeProduct1);
                //}

                //// Example: Make product defective
                //var defectiveProd = plantOperations.GetProduct("ISN-00008");
                //if (defectiveProd != null)
                //{
                //    defectiveProd.UpdateStage(Product.ProdStage.Crystalizer, new Roles(Roles.RoleType.LeadOperator), Product.State.AwaitingProduction);
                //    defectiveProd.UpdateState(Product.State.Defective, new Roles(Roles.RoleType.LeadOperator), Product.ProdStage.Crystalizer);
                //}

                // Example: Get statistics
                //int productsOutStorage = plantStatistics.GetProductsInOutStoragePerMonth(DateTime.Now.Month, DateTime.Now.Year);
                //int defectiveProducts = plantStatistics.GetDefectiveProductsPerMonth(DateTime.Now.Month, DateTime.Now.Year);
                //double averageTime = plantStatistics.GetAverageTimeForCompletion(DateTime.Now.Month, DateTime.Now.Year);

                //Console.WriteLine($"Products in OutStorage this month: {productsOutStorage}");
                //Console.WriteLine($"Defective products: {defectiveProducts}");
                //Console.WriteLine($"Average production time: {averageTime}");
            //}
    }
}

//var options = new DbContextOptionsBuilder<PlantDbContext>()
//        .UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;;TrustServerCertificate=true")
//        .Options;

//// Create an instance of PlantDbContext
//using (var context = new PlantDbContext(options))
//{
//    // Create an instance of LogInFunctions
//    var logInFunctions = new LogInFunctions(context);
//    var plantOperations = new PlantOperations(context);
//    var plantStatistics = new PlantStatistics(context);

//    //Create a new user

//    var mike = new User("MikeB", "BishTheCommish", RoleType.BusinessUser);
//    logInFunctions.AddUser(mike);
//}
