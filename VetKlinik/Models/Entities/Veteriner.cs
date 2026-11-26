using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class Veteriner
{
    public int Id { get; set; }

    [StringLength(30)]
    public string? Unvan { get; set; }

    [StringLength(1000)]
    public string? Biyografi { get; set; }

    [Required]
    public string KullaniciId { get; set; } = string.Empty;
    public ApplicationUser? Kullanici { get; set; }

    public ICollection<UzmanlikAlani> Uzmanliklar { get; set; } = new List<UzmanlikAlani>();
    public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    public ICollection<VeterinerMusaitlik> Musaitlikler { get; set; } = new List<VeterinerMusaitlik>();
    public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    public ICollection<VeterinerIzin> Izinler { get; set; } = new List<VeterinerIzin>();
}