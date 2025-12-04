using Microsoft.EntityFrameworkCore;
using MissingPetFinder.Models;

namespace MissingPetFinder.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Pet> Pets => Set<Pet>();
    }
}