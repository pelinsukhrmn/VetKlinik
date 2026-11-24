using System.ComponentModel.DataAnnotations;

namespace VetKlinik.Models.Entities;

public class UzmanlikAlani
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Ad { get; set; } = string.Empty;

    public ICollection<Veteriner> Veterinerler { get; set; } = new List<Veteriner>();
}