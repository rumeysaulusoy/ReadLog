namespace ReadLog.Data;

using Microsoft.EntityFrameworkCore;
using ReadLog.Models;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) 
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<ReadingProgress> ReadingProgresses { get; set; }

}
