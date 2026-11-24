using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class AsiKaydi
{
    public int Id { get; set; }

    public int EvcilHayvanId { get; set; }
    public EvcilHayvan? EvcilHayvan { get; set; }

    [Required, StringLength(100)]
    public string AsiAdi { get; set; } = string.Empty;

    public DateOnly YapilmaTarihi { get; set; }
    public DateOnly? SonrakiDozTarihi { get; set; }

    [StringLength(500)]
    public string? Notlar { get; set; }
}