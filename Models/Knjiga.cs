using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotekaWeb.Models
{
	public class Knjiga
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Naziv { get; set; }

		[Required]
		[Range(0, 1000)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cijena { get; set; }

		public int AutorId { get; set; }
		public Autor Autor { get; set; }
		public int ZanrId { get; set; }
		public Zanr Zanr { get; set; }

		public List<Kupnja> Kupnja { get; set; }

		public List<Posudba> Posudba { get; set; }

		public List<Vracanje> Vracanje { get; set; }

        public List<Skladiste> Skladiste { get; set; }

        public Knjiga()
        {
			Naziv = "";
			Cijena = 0;
        }

    }
}
