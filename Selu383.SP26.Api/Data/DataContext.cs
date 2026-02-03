using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Models;

namespace Selu383.SP26.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Location> Locations { get; set; }
    }
}
