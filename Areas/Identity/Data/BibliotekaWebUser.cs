using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BibliotekaWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace BibliotekaWeb.Areas.Identity.Data;


public class BibliotekaWebUser : IdentityUser
{
    public string Ime { get; set; }
    public string Prezime { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Novcanik { get; set; }

    public List<Posudba> Posudba { get; set; }

    public List<Kupnja> Kupnja { get; set; }

    public List<Vracanje> Vracanje { get; set; }

    public List<Transakcija> Transakcija { get; set; }


    public BibliotekaWebUser()
    {
        Ime = "";
        Prezime = "";
        Novcanik = 100;
    }
}

