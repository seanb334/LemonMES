using System.ComponentModel.DataAnnotations;
using static LemonMES.Models.Roles;
namespace LemonMES.Models
{
    public class UnitStatus
    {
        [Key]
        public Product.ProdStage Stage { get; set; }
        public int Status { get; set; }

    }

    public class Roles
    {
        public enum RoleType
        {
            Engineer,
            LeadOperator,
            Operator,
            BusinessUser
        }

        [Key]
        public int RoleId { get; set; }
        public RoleType Role { get; set; }
        public Product.ProdStage AssignedStage { get; set; }

        public Roles(RoleType role, Product.ProdStage assignedStage = Product.ProdStage.InStorage)
        {
            Role = role;
            AssignedStage = assignedStage;
        }
    }


    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleType Role { get; set; }
        public Product.ProdStage ProdStage { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public User(string username, string password, RoleType role, Product.ProdStage prodstage = Product.ProdStage.OutStorage)
        {
            Username = username;
            Password = password;
            Role = role;
            ProdStage = prodstage;
        }

    }
    public class Product
    {
        public enum ProdStage
        {
            InStorage, MixingTank, ReactorOne, ReactorTwo, ReactorThree,
            ReactorFour, ReactorFive, RotaryFilter, HoldingTank, Concentrator,
            Extractor, Evaporator, Crystalizer, Dryer, OutStorage
        }

        public enum State
        {
            AwaitingProduction, InProduction, Completed, Defective
        }

        [Key]
        public string SerialNumber { get; set; }
        public ProdStage CurrentStage { get; set; }
        public State CurrentState { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Description { get; set; }

        // Constructor to initialize a new product
        public Product(string serialNumber, string description)
        {
            SerialNumber = serialNumber;
            CurrentStage = ProdStage.InStorage;
            CurrentState = State.AwaitingProduction;
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
            Description = description;
        }

        // Method to update product description, operators only
        public bool UpdateDescription(string newDescription, Roles role, ProdStage currentStage)
        {
            if ((role.Role == Roles.RoleType.LeadOperator) ||
                (role.Role == Roles.RoleType.Operator && role.AssignedStage == currentStage))
            {
                Description = newDescription;
                DateUpdated = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Method to provide product details as a string
        public override string ToString()
        {
            return $"SerialNumber: {SerialNumber}, Stage: {CurrentStage}, State: {CurrentState}, " +
                   $"DateCreated: {DateCreated}, DateUpdated: {DateUpdated}, Description: {Description}";
        }
    }
}
