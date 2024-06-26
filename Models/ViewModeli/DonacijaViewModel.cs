using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models.ViewModeli
{
    public class DonacijaViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string  Email { get; set; }  

        [Required]
        [Display(Name = "Ime osobe")]
        public string Ime { get; set; }

        [Required]
        [Display(Name = "Prezime osobe")]
        public string Prezime { get; set; }

        [Required]
        [Display(Name = "Broj mobitela")]
        public string BrojMobitela { get; set; }

        [Required]
        [Display(Name = "Tip knjige")]
        public string? TipKnjige { get; set; }

        [Required]
        [Display(Name = "Ime knjige")]
        public string? ImeKnjige { get; set; }

        [Display(Name = "Dodatne informacije o knjizi")]
        public string? DodatneInformacije { get; set; }
    }
}
