using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models.ViewModeli
{
    public class ZanrViewModel
    {
        [Required(ErrorMessage = "Ovo polje ne moze biti prazno.")]
        [StringLength(50, ErrorMessage = "Polje ne smije bit duze od {1} slova.")]
        public string Naziv { get; set; }

        public List<Zanr> Zanrovi { get; set; }
    }
}
