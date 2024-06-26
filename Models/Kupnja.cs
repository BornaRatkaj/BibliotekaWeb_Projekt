using BibliotekaWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models
{
	public class Kupnja
	{
		[Key]
		public int Id { get; set; }

        public int KnjigaId { get; set; }
        public Knjiga Knjiga { get; set; }

        public string UserId { get; set; }
		public BibliotekaWebUser User { get; set; }

	}
}
