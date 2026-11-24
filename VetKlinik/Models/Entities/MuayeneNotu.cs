using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class MuayeneNotu
{
    public int Id { get; set; }

    public int RandevuId { get; set; }
    public Randevu? Randevu { get; set; }

    [Required, StringLength(2000)]
    public string Bulgular { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Tedavi { get; set; }

    public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
}