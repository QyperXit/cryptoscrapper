using CryptoScrapper.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoScrapper.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) { }        
    
    public DbSet<Coins> Coins { get; set; } = null!; // ! = not null>
}