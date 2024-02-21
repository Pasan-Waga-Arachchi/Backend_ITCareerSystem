using ITCareerSystem_Test1_.Models;
using Microsoft.EntityFrameworkCore;

namespace ITCareerSystem_Test1_.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
