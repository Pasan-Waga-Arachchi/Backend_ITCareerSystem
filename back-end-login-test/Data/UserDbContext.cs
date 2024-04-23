using ITCareerSystem_Test1_.Models;
using Microsoft.EntityFrameworkCore;

namespace ITCareerSystem_Test1_.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public bool ValidateUser(string username, string password)
        {
            // Replace with your actual database access and hashing logic
            return User.Any(u => u.User_Name == username && u.Password == password); // Placeholder example
        }
    }
}
