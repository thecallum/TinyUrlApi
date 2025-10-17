using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TinyUrl.Infrastructure;

public class TinyUrlDbContext : DbContext
{
    public TinyUrlDbContext(DbContextOptions<TinyUrlDbContext> options) 
        : base(options)
    {
    }
    
    public DbSet<TinyUrl> Urls { get; set; }
}

public class TinyUrl
{
    [Key]
    public int Id { get; set; }
    public string ShortUrl { get; set; }
    public string FullUrl { get; set; }
    
}