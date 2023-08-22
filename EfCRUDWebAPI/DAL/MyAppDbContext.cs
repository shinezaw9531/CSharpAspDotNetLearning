using EfCRUDWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCRUDWebAPI.DAL
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions options): base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
