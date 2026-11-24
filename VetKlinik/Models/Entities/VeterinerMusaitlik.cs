namespace VetKlinik.Models.Entities;

public class VeterinerMusaitlik
{
    public int Id { get; set; }

    public int VeterinerId { get; set; }
    public Veteriner? Veteriner { get; set; }

    public DayOfWeek Gun { get; set; }
    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }
}