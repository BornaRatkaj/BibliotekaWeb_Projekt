using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models.ViewModeli
{
    public class AutorViewModel
    {
        [Required(ErrorMessage = "Ovo polje ne moze biti prazno.")]
        [StringLength(50, ErrorMessage = "Polje ne smije bit duze od {1} slova.")]
        public string ImeFull { get; set; }

        public List<Autor> Autori { get; set; }
    }
}
