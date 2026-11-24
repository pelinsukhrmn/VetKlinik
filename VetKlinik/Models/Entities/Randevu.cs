using System.ComponentModel.DataAnnotations;
using VetKlinik.Models.Enums;

namespace VetKlinik.Models.Entities;

public class Randevu
{
    public int Id { get; set; }

    public int EvcilHayvanId { get; set; }
    public EvcilHayvan? EvcilHayvan { get; set; }

    public int VeterinerId { get; set; }
    public Veteriner? Veteriner { get; set; }

    public int HizmetId { get; set; }
    public Hizmet? Hizmet { get; set; }

    public DateTime Baslangic { get; set; }
    public DateTime Bitis { get; set; }

    // Randevu oluşturulduğu andaki değerler (hizmet fiyatı sonradan değişse de korunur)
    public int SureDakika { get; set; }
    public decimal Ucret { get; set; }

    public RandevuDurumu Durum { get; set; } = RandevuDurumu.Bekliyor;

    [StringLength(500)]
    public string? SahipNotu { get; set; }

    [StringLength(500)]
    public string? RedNedeni { get; set; }

    public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

    public MuayeneNotu? MuayeneNotu { get; set; }
}