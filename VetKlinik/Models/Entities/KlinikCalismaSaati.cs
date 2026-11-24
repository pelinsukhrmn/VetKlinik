namespace VetKlinik.Models.Entities;

public class KlinikCalismaSaati
{
    public int Id { get; set; }
    public DayOfWeek Gun { get; set; }
    public TimeOnly AcilisSaati { get; set; }
    public TimeOnly KapanisSaati { get; set; }
    public bool KapaliMi { get; set; }
}