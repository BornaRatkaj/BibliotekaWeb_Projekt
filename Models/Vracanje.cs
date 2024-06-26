using BibliotekaWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models
{
	public class Vracanje
	{
		[Key]
		public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatumVracanja { get; set; }

		public int KnjigaId { get; set; }
		public Knjiga Knjiga { get; set; }

		public string UserId { get; set; }
		public BibliotekaWebUser User { get; set; }

	}
}
