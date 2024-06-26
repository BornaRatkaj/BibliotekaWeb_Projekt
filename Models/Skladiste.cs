using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models
{
    public class Skladiste
    {
        [Key]
        public int Id { get; set; }

        public int KnjigaId { get; set; }
        public Knjiga Knjiga { get; set; }


    }
}
