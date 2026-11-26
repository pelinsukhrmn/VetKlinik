using System.ComponentModel.DataAnnotations;
using VetKlinik.Models.Enums;

namespace VetKlinik.Models.Entities;

public class AsiSablonu
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string AsiAdi { get; set; } = string.Empty;

    public HayvanTuru Tur { get; set; }

    // Boş bırakılırsa tek dozluk aşıdır; bir sonraki doz tarihi hesaplanmaz
    [Range(1, 3650)]
    public int? TekrarAraligiGun { get; set; }

    [StringLength(500)]
    public string? Aciklama { get; set; }

    public bool AktifMi { get; set; } = true;

    public ICollection<AsiKaydi> AsiKayitlari { get; set; } = new List<AsiKaydi>();
}