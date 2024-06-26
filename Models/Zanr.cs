using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models
{
	public class Zanr
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Naziv { get; set; }

		public List<Knjiga> Knjiga { get; set; }

        public Zanr()
        {
			Naziv = "";
        }
    }
}
