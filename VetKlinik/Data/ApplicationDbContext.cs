using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VetKlinik.Models.Entities;

namespace VetKlinik.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hizmet> Hizmetler => Set<Hizmet>();
    public DbSet<UzmanlikAlani> UzmanlikAlanlari => Set<UzmanlikAlani>();
    public DbSet<Veteriner> Veterinerler => Set<Veteriner>();
    public DbSet<VeterinerMusaitlik> VeterinerMusaitlikleri => Set<VeterinerMusaitlik>();
    public DbSet<KlinikCalismaSaati> KlinikCalismaSaatleri => Set<KlinikCalismaSaati>();
    public DbSet<EvcilHayvan> EvcilHayvanlar => Set<EvcilHayvan>();
    public DbSet<AsiKaydi> AsiKayitlari => Set<AsiKaydi>();
    public DbSet<Randevu> Randevular => Set<Randevu>();
    public DbSet<MuayeneNotu> MuayeneNotlari => Set<MuayeneNotu>();
    public DbSet<AsiSablonu> AsiSablonlari => Set<AsiSablonu>();
    public DbSet<VeterinerIzin> VeterinerIzinleri => Set<VeterinerIzin>();
    public DbSet<KiloOlcumu> KiloOlcumleri => Set<KiloOlcumu>();
    public DbSet<RandevuDurumGecmisi> RandevuDurumGecmisleri => Set<RandevuDurumGecmisi>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // SQLite decimal üzerinde Sum/Average/ORDER BY yapamaz; veritabanında double olarak saklıyoruz
        configurationBuilder.Properties<decimal>().HaveConversion<double>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Identity tablolarının (AspNetUsers, AspNetRoles...) kurulması için şart
        base.OnModelCreating(builder);

        builder.Entity<Veteriner>(e =>
        {
            e.HasOne(v => v.Kullanici)
             .WithOne()
             .HasForeignKey<Veteriner>(v => v.KullaniciId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(v => v.Uzmanliklar)
             .WithMany(u => u.Veterinerler)
             .UsingEntity(j => j.ToTable("VeterinerUzmanliklari"));

            e.HasMany(v => v.Hizmetler)
             .WithMany(h => h.Veterinerler)
             .UsingEntity(j => j.ToTable("VeterinerHizmetleri"));
        });

        builder.Entity<VeterinerMusaitlik>()
            .HasOne(m => m.Veteriner)
            .WithMany(v => v.Musaitlikler)
            .HasForeignKey(m => m.VeterinerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<KlinikCalismaSaati>()
            .HasIndex(k => k.Gun)
            .IsUnique();

        builder.Entity<UzmanlikAlani>()
            .HasIndex(u => u.Ad)
            .IsUnique();

        builder.Entity<EvcilHayvan>(e =>
        {
            e.Property(h => h.Tur).HasConversion<string>().HasMaxLength(20);

            e.HasOne(h => h.Sahip)
             .WithMany(u => u.EvcilHayvanlar)
             .HasForeignKey(h => h.SahipId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<AsiKaydi>()
            .HasOne(a => a.EvcilHayvan)
            .WithMany(h => h.AsiKayitlari)
            .HasForeignKey(a => a.EvcilHayvanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Randevu>(e =>
        {
            e.Property(r => r.Durum).HasConversion<string>().HasMaxLength(20);

            e.HasOne(r => r.EvcilHayvan)
             .WithMany(h => h.Randevular)
             .HasForeignKey(r => r.EvcilHayvanId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Veteriner)
             .WithMany(v => v.Randevular)
             .HasForeignKey(r => r.VeterinerId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Hizmet)
             .WithMany(h => h.Randevular)
             .HasForeignKey(r => r.HizmetId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.MuayeneNotu)
             .WithOne(m => m.Randevu)
             .HasForeignKey<MuayeneNotu>(m => m.RandevuId)
             .OnDelete(DeleteBehavior.Cascade);

            // Çakışma kontrolü sorgusu bu iki alana göre arayacak
            e.HasIndex(r => new { r.VeterinerId, r.Baslangic });
        });
                builder.Entity<AsiSablonu>(e =>
        {
            e.Property(s => s.Tur).HasConversion<string>().HasMaxLength(20);
            // Aynı tür için aynı adla iki şablon olamaz
            e.HasIndex(s => new { s.Tur, s.AsiAdi }).IsUnique();
        });

        builder.Entity<AsiKaydi>()
            .HasOne(a => a.AsiSablonu)
            .WithMany(s => s.AsiKayitlari)
            .HasForeignKey(a => a.AsiSablonuId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<VeterinerIzin>()
            .HasOne(i => i.Veteriner)
            .WithMany(v => v.Izinler)
            .HasForeignKey(i => i.VeterinerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<KiloOlcumu>(e =>
        {
            e.HasOne(k => k.EvcilHayvan)
             .WithMany(h => h.KiloOlcumleri)
             .HasForeignKey(k => k.EvcilHayvanId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(k => k.Randevu)
             .WithMany()
             .HasForeignKey(k => k.RandevuId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<RandevuDurumGecmisi>(e =>
        {
            e.Property(g => g.EskiDurum).HasConversion<string>().HasMaxLength(20);
            e.Property(g => g.YeniDurum).HasConversion<string>().HasMaxLength(20);

            e.HasOne(g => g.Randevu)
             .WithMany(r => r.DurumGecmisi)
             .HasForeignKey(g => g.RandevuId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(g => g.DegistirenKullanici)
             .WithMany()
             .HasForeignKey(g => g.DegistirenKullaniciId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}