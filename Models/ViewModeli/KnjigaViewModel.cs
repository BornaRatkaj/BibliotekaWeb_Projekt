using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models.ViewModeli
{
    public class KnjigaViewModel
    {
        [Required(ErrorMessage = "Ovo polje ne moze biti prazno.")]
        [StringLength(50, ErrorMessage = "Polje ne smije bit duze od {1} slova.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Ovo polje ne moze biti prazno.")]
        [Range(0, 1000)]
        public decimal Cijena { get; set; }

        public List<Knjiga>? Knjige { get; set; }

        public List<Autor>? Autori { get; set; }

        public List<Zanr>? Zanrovi { get; set; }
    }
}
