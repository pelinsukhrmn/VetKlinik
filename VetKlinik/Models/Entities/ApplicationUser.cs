using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VetKlinik.Models.Entities;

public class ApplicationUser : IdentityUser
{
    [Required, StringLength(50)]
    public string Ad { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Soyad { get; set; } = string.Empty;

    public ICollection<EvcilHayvan> EvcilHayvanlar { get; set; } = new List<EvcilHayvan>();
}