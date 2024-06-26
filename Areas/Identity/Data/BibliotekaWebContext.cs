using BibliotekaWeb.Areas.Identity.Data;
using BibliotekaWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaWeb.Data;

public class BibliotekaWebContext : IdentityDbContext<BibliotekaWebUser>
{
    public DbSet<Autor> Autori { get; set; }
    public DbSet<Zanr> Zanrovi { get; set; }
    public DbSet<Knjiga> Knjige { get; set; }
    public DbSet<Kupnja> Kupnje { get; set; }
    public DbSet<Posudba> Posudbe { get; set; }
    public DbSet<Posudenje> Posudenja { get; set; }
    public DbSet<Vracanje> Vracanja { get; set; }
    public DbSet<Transakcija> Transakcije { get; set; }
    public DbSet<Skladiste> Skladiste { get; set; }
    public BibliotekaWebContext(DbContextOptions<BibliotekaWebContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
