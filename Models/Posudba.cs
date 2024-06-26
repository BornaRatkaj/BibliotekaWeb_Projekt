using BibliotekaWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BibliotekaWeb.Models
{
	public class Posudba
	{
		[Key]
		public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatumPosudbe { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatumPotrebnogVracanja { get; set; }


        public int KnjigaId { get; set; }
        public Knjiga Knjiga { get; set; }

        public string UserId { get; set; }
		public BibliotekaWebUser User { get; set; }

	}
}
