using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public new virtual DbSet<Entry> Entry { get; set; }
}
