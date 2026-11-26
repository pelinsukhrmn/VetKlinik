using System.ComponentModel.DataAnnotations;
using VetKlinik.Models.Enums;

namespace VetKlinik.Models.Entities;

public class RandevuDurumGecmisi
{
    public int Id { get; set; }

    public int RandevuId { get; set; }
    public Randevu? Randevu { get; set; }

    // Randevu ilk oluşturulduğunda eski durum yoktur
    public RandevuDurumu? EskiDurum { get; set; }
    public RandevuDurumu YeniDurum { get; set; }

    [Required]
    public string DegistirenKullaniciId { get; set; } = string.Empty;
    public ApplicationUser? DegistirenKullanici { get; set; }

    public DateTime Tarih { get; set; } = DateTime.Now;

    [StringLength(500)]
    public string? Aciklama { get; set; }
}