using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class VeterinerIzin
{
    public int Id { get; set; }

    public int VeterinerId { get; set; }
    public Veteriner? Veteriner { get; set; }

    public DateOnly BaslangicTarihi { get; set; }
    public DateOnly BitisTarihi { get; set; }

    [StringLength(200)]
    public string? Aciklama { get; set; }
}