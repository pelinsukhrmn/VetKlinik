using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VetKlinik.Models;
using VetKlinik.Models.Entities;
using VetKlinik.Models.Enums;

namespace VetKlinik.Data;

public static class VeriTohumlayici
{
    // Ödev metninde istenen admin bilgileri
    private const string AdminEmail = "b241210104@sakarya.edu.tr";
    private const string AdminSifre = "sau";

    public static async Task TohumlaAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Bekleyen migration'ları uygular; veritabanı yoksa oluşturur
        await db.Database.MigrateAsync();

        foreach (var rol in new[] { Roller.Admin, Roller.Veteriner, Roller.Uye })
        {
            if (!await roleManager.RoleExistsAsync(rol))
                await roleManager.CreateAsync(new IdentityRole(rol));
        }

        await AdminOlusturAsync(userManager);
        await KlinikVerileriAsync(db);
        await OrnekKullanicilarAsync(db, userManager);
    }

    private static async Task AdminOlusturAsync(UserManager<ApplicationUser> userManager)
    {
        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true,
                Ad = "Sistem",
                Soyad = "Yöneticisi"
            };

            // "sau" şifre kuralına uymaz. Kural sadece şifre belirlenirken kontrol edildiği için
            // özeti (hash) kendimiz hesaplayıp şifresiz CreateAsync ile kullanıcıyı oluşturuyoruz.
            admin.PasswordHash = userManager.PasswordHasher.HashPassword(admin, AdminSifre);

            var sonuc = await userManager.CreateAsync(admin);
            if (!sonuc.Succeeded)
                throw new InvalidOperationException("Admin oluşturulamadı: " +
                    string.Join(", ", sonuc.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(admin, Roller.Admin))
            await userManager.AddToRoleAsync(admin, Roller.Admin);
    }

    private static async Task KlinikVerileriAsync(ApplicationDbContext db)
    {
        if (!await db.UzmanlikAlanlari.AnyAsync())
        {
            db.UzmanlikAlanlari.AddRange(
                new[] { "Küçük Hayvan", "Egzotik Hayvan", "Cerrahi", "Dermatoloji" }
                    .Select(ad => new UzmanlikAlani { Ad = ad }));
        }

        // Süre ve ücretler örnek değerlerdir; admin panelinden değiştirilebilir
        if (!await db.Hizmetler.AnyAsync())
        {
            db.Hizmetler.AddRange(
                new Hizmet { Ad = "Genel Muayene", SureDakika = 30, Ucret = 750 },
                new Hizmet { Ad = "Aşı Uygulaması", SureDakika = 15, Ucret = 500 },
                new Hizmet { Ad = "Kısırlaştırma", SureDakika = 120, Ucret = 4500 },
                new Hizmet { Ad = "Diş Temizliği", SureDakika = 60, Ucret = 2500 },
                new Hizmet { Ad = "Tıraş ve Bakım", SureDakika = 60, Ucret = 1000 });
        }

        if (!await db.KlinikCalismaSaatleri.AnyAsync())
        {
            foreach (var gun in Enum.GetValues<DayOfWeek>())
            {
                db.KlinikCalismaSaatleri.Add(gun switch
                {
                    DayOfWeek.Sunday => new KlinikCalismaSaati { Gun = gun, KapaliMi = true },
                    DayOfWeek.Saturday => new KlinikCalismaSaati
                        { Gun = gun, AcilisSaati = new TimeOnly(10, 0), KapanisSaati = new TimeOnly(14, 0) },
                    _ => new KlinikCalismaSaati
                        { Gun = gun, AcilisSaati = new TimeOnly(9, 0), KapanisSaati = new TimeOnly(18, 0) }
                });
            }
        }

        // Örnek şablonlardır; gerçek aşı adı ve aralıklarını admin panelinden veteriner belirler
        if (!await db.AsiSablonlari.AnyAsync())
        {
            db.AsiSablonlari.AddRange(
                new AsiSablonu { Tur = HayvanTuru.Kedi, AsiAdi = "Karma Aşı", TekrarAraligiGun = 365 },
                new AsiSablonu { Tur = HayvanTuru.Kedi, AsiAdi = "Kuduz", TekrarAraligiGun = 365 },
                new AsiSablonu { Tur = HayvanTuru.Kedi, AsiAdi = "Lösemi (FeLV)", TekrarAraligiGun = 365 },
                new AsiSablonu { Tur = HayvanTuru.Kopek, AsiAdi = "Karma Aşı", TekrarAraligiGun = 365 },
                new AsiSablonu { Tur = HayvanTuru.Kopek, AsiAdi = "Kuduz", TekrarAraligiGun = 365 },
                new AsiSablonu { Tur = HayvanTuru.Kopek, AsiAdi = "Bronşin", TekrarAraligiGun = 365 });
        }

        await db.SaveChangesAsync();
    }

    private static async Task OrnekKullanicilarAsync(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        if (await db.Veterinerler.AnyAsync()) return;

        var uzmanliklar = await db.UzmanlikAlanlari.ToListAsync();
        var hizmetler = await db.Hizmetler.ToListAsync();
        var bugun = DateOnly.FromDateTime(DateTime.Today);

        // 1. veteriner: hafta içi, öğle arası olan çalışma düzeni
        var ayse = await KullaniciOlusturAsync(userManager,
            "ayse.yilmaz@vetklinik.test", "Vet12345", "Ayşe", "Yılmaz", Roller.Veteriner);

        db.Veterinerler.Add(new Veteriner
        {
            KullaniciId = ayse.Id,
            Unvan = "Vet. Hek.",
            Biyografi = "Kedi ve köpeklerde iç hastalıkları ve deri hastalıkları.",
            Uzmanliklar = uzmanliklar.Where(u => u.Ad is "Küçük Hayvan" or "Dermatoloji").ToList(),
            Hizmetler = hizmetler.Where(h => h.Ad != "Kısırlaştırma").ToList(),
            Musaitlikler = MusaitlikOlustur(
                new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday },
                (9, 12), (13, 17))
        });

        // 2. veteriner: cerrah, haftanın belirli günleri; önümüzdeki hafta izinli
        var mehmet = await KullaniciOlusturAsync(userManager,
            "mehmet.demir@vetklinik.test", "Vet12345", "Mehmet", "Demir", Roller.Veteriner);

        db.Veterinerler.Add(new Veteriner
        {
            KullaniciId = mehmet.Id,
            Unvan = "Dr. Vet. Hek.",
            Biyografi = "Yumuşak doku cerrahisi ve egzotik hayvanlar.",
            Uzmanliklar = uzmanliklar.Where(u => u.Ad is "Cerrahi" or "Egzotik Hayvan").ToList(),
            Hizmetler = hizmetler.Where(h => h.Ad is "Kısırlaştırma" or "Genel Muayene" or "Diş Temizliği").ToList(),
            Musaitlikler = MusaitlikOlustur(
                new[] { DayOfWeek.Tuesday, DayOfWeek.Thursday }, (10, 16))
                .Concat(MusaitlikOlustur(new[] { DayOfWeek.Saturday }, (10, 14)))
                .ToList(),
            Izinler =
            {
                new VeterinerIzin
                {
                    BaslangicTarihi = bugun.AddDays(7),
                    BitisTarihi = bugun.AddDays(9),
                    Aciklama = "Kongre"
                }
            }
        });

        // Örnek üye ve ödev metnindeki örnekteki "Pamuk"
        var uye = await KullaniciOlusturAsync(userManager,
            "uye@vetklinik.test", "Uye12345", "Deniz", "Kaya", Roller.Uye);

        var kuduz = await db.AsiSablonlari.FirstAsync(s => s.Tur == HayvanTuru.Kedi && s.AsiAdi == "Kuduz");
        var karma = await db.AsiSablonlari.FirstAsync(s => s.Tur == HayvanTuru.Kedi && s.AsiAdi == "Karma Aşı");

        var kuduzTarihi = bugun.AddMonths(-11);   // bir sonraki doz yaklaşıyor
        var karmaTarihi = bugun.AddMonths(-14);   // bir sonraki dozun tarihi geçmiş

        db.EvcilHayvanlar.Add(new EvcilHayvan
        {
            Ad = "Pamuk",
            Tur = HayvanTuru.Kedi,
            Irk = "Van Kedisi",
            DogumTarihi = bugun.AddYears(-3),
            Kilo = 4.2m,
            SahipId = uye.Id,
            AsiKayitlari =
            {
                new AsiKaydi
                {
                    AsiAdi = kuduz.AsiAdi, AsiSablonuId = kuduz.Id,
                    YapilmaTarihi = kuduzTarihi,
                    SonrakiDozTarihi = kuduzTarihi.AddDays(kuduz.TekrarAraligiGun!.Value)
                },
                new AsiKaydi
                {
                    AsiAdi = karma.AsiAdi, AsiSablonuId = karma.Id,
                    YapilmaTarihi = karmaTarihi,
                    SonrakiDozTarihi = karmaTarihi.AddDays(karma.TekrarAraligiGun!.Value)
                }
            },
            KiloOlcumleri =
            {
                new KiloOlcumu { OlcumTarihi = bugun.AddMonths(-12), Kilo = 3.6m },
                new KiloOlcumu { OlcumTarihi = bugun.AddMonths(-6), Kilo = 4.0m },
                new KiloOlcumu { OlcumTarihi = bugun.AddMonths(-1), Kilo = 4.2m }
            }
        });

        await db.SaveChangesAsync();
    }

    private static async Task<ApplicationUser> KullaniciOlusturAsync(UserManager<ApplicationUser> userManager,
        string email, string sifre, string ad, string soyad, string rol)
    {
        var kullanici = await userManager.FindByEmailAsync(email);
        if (kullanici is null)
        {
            kullanici = new ApplicationUser
            {
                UserName = email, Email = email, EmailConfirmed = true, Ad = ad, Soyad = soyad
            };
            var sonuc = await userManager.CreateAsync(kullanici, sifre);
            if (!sonuc.Succeeded)
                throw new InvalidOperationException($"{email} oluşturulamadı: " +
                    string.Join(", ", sonuc.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(kullanici, rol))
            await userManager.AddToRoleAsync(kullanici, rol);

        return kullanici;
    }

    private static List<VeterinerMusaitlik> MusaitlikOlustur(DayOfWeek[] gunler, params (int Bas, int Bit)[] araliklar)
        => gunler.SelectMany(gun => araliklar.Select(a => new VeterinerMusaitlik
            {
                Gun = gun,
                BaslangicSaati = new TimeOnly(a.Bas, 0),
                BitisSaati = new TimeOnly(a.Bit, 0)
            }))
            .ToList();
}