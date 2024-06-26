using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models
{
	public class Autor
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string FullIme { get; set; }

        public List<Knjiga> Knjiga { get; set; }


        public Autor()
        {
			FullIme = "";
        }
    }
}
