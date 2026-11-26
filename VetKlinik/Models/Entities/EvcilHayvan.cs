using System.ComponentModel.DataAnnotations;
using VetKlinik.Models.Enums;

namespace VetKlinik.Models.Entities;

public class EvcilHayvan
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Ad { get; set; } = string.Empty;

    public HayvanTuru Tur { get; set; }

    [StringLength(50)]
    public string? Irk { get; set; }

    public DateOnly DogumTarihi { get; set; }

    [Range(typeof(decimal), "0.01", "500")]
    public decimal Kilo { get; set; }

    [StringLength(260)]
    public string? FotografYolu { get; set; }

    [Required]
    public string SahipId { get; set; } = string.Empty;
    public ApplicationUser? Sahip { get; set; }

    public ICollection<AsiKaydi> AsiKayitlari { get; set; } = new List<AsiKaydi>();
    public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    public ICollection<KiloOlcumu> KiloOlcumleri { get; set; } = new List<KiloOlcumu>();
}