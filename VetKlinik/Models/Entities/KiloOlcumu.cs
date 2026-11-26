using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class KiloOlcumu
{
    public int Id { get; set; }

    public int EvcilHayvanId { get; set; }
    public EvcilHayvan? EvcilHayvan { get; set; }

    public DateOnly OlcumTarihi { get; set; }

    [Range(typeof(decimal), "0.01", "500")]
    public decimal Kilo { get; set; }

    // Ölçüm bir muayene sırasında yapıldıysa ilgili randevu
    public int? RandevuId { get; set; }
    public Randevu? Randevu { get; set; }
}