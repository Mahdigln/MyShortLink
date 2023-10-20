using Microsoft.EntityFrameworkCore;
using ShortLink.Domain.Models.Account;
using ShortLink.Domain.Models.Link;

namespace ShortLink.Infra.Data.Context;

public class ShortLinkContext:DbContext
{
    public ShortLinkContext(DbContextOptions<ShortLinkContext> options) : base(options)
    {
        
    }

    #region Account

    public DbSet<User> Users { get; set; }

    #endregion

    #region link
    public DbSet<ShortUrl> ShortUrls { get; set; }
    public DbSet<Browser> Browsers { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Os> Os { get; set; }
    public DbSet<RequestUrl> RequestUrls { get; set; }
    #endregion


    #region on model creating

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s=>s.GetForeignKeys()))
        {
            relation.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}