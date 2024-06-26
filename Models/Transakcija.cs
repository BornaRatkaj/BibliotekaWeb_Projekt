using BibliotekaWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotekaWeb.Models
{
	public class Transakcija
	{
		[Key]
		public int Id { get; set; }

		[Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Iznos { get; set; }


        [Required]
		public bool UplataIsplata { get; set; }

		public string UserId { get; set; }
		public BibliotekaWebUser User { get; set; }

        public Transakcija()
        {
			Iznos = 0;
			UplataIsplata = true;
        }

    }
}
