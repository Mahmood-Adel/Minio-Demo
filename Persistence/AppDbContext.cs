using Microsoft.EntityFrameworkCore;
using Minio_Demo.Data;

namespace Minio_Demo.Persistence;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }

    public DbSet<LibraryFile> LibraryFiles { get; set; }
}