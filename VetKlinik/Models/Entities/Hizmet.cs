using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class Hizmet
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Ad { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Aciklama { get; set; }

    [Range(5, 480)]
    public int SureDakika { get; set; }

    [Range(typeof(decimal), "0", "100000")]
    public decimal Ucret { get; set; }

    public bool AktifMi { get; set; } = true;

    public ICollection<Veteriner> Veterinerler { get; set; } = new List<Veteriner>();
    public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
}