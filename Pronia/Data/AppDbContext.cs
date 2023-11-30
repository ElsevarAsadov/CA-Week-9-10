using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<ColorModel> Colors { get; set; }


        public AppDbContext(DbContextOptions options) : base(options)
        {


            
        }
    }
}
