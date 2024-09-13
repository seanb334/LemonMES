using LemonMES.Models;

namespace LemonMES.Database
{
    public class LogInFunctions
    {
        private readonly PlantDbContext _context = default!;

        public LogInFunctions(PlantDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUser(string Username)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _context.Users.SingleOrDefault(p => p.Username == Username);
#pragma warning restore CS8603 // Possible null reference return.

        }

        public bool CheckPassword(string username, string inputPassword)
        {
            // Retrieve the user from the database
            User user = GetUser(username);
            if (user == null)
            {
                return false; // User not found
            }

            // Directly compare the input password with the stored password
            return inputPassword == user.Password;
        }


    }
}
