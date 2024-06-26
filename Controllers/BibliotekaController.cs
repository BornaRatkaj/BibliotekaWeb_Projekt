using BibliotekaWeb.Models.ViewModeli;
using BibliotekaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaWeb.Areas.Identity.Data;
using BibliotekaWeb.Data;

namespace BibliotekaWeb.Controllers
{
    public class BibliotekaController : Controller
    {
        // Dependency injection varijable
        private readonly BibliotekaWebContext _context;
        private readonly UserManager<BibliotekaWebUser> _userManager;
        private BibliotekaWebUser _user;

        public string? Pretrazivanje { get; private set; }

        // Konstruktor
        public BibliotekaController(BibliotekaWebContext context, UserManager<BibliotekaWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // View koji prikazuje listu dostupnih knjiga za posudivanje

        public async Task<IActionResult> Index(string Pretrazivanje)
        {
            // Lista podataka iz skladišta
            List<Skladiste> skladiste = new List<Skladiste>();
            SkladisteViewModel skladisteViewModel = new SkladisteViewModel();
            // 4 tablice spojene u jedno iz baze
            var query = _context.Skladiste
                       .Include(k => k.Knjiga)
                       .Include(k => k.Knjiga.Zanr)
                       .Include(k => k.Knjiga.Autor)
                       .AsQueryable();

            if (!string.IsNullOrEmpty(Pretrazivanje))
            {
                query = query.Where(s => s.Knjiga.Naziv.Contains(Pretrazivanje));
            }

            skladiste = await query.ToListAsync();
            skladisteViewModel.Skladiste = skladiste;
            skladisteViewModel.Pretrazivanje = Pretrazivanje;

            return View(skladisteViewModel);
        }
        // Provjerava roles
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MojeKnjige()
        {
            // Provjerava ako ima spremljeni podatak o logiranom useru, ak nema povlači podatke o logiranom useru
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Prazna lista u kojoj su posudenje knjige tog usera
            List<Posudenje> posudenja = new List<Posudenje>();
            posudenja = _context.Posudenja.Include(p => p.User).Include(p => p.Knjiga).Where(p => p.User == _user).ToList();
            return View(posudenja);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> KupljeneKnjige()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var kupnje = await _context.Kupnje
                .Include(k => k.Knjiga)
                    .ThenInclude(k => k.Autor)
                .Include(k => k.Knjiga)
                    .ThenInclude(k => k.Zanr)
                .Where(k => k.User.Id == _user.Id)
                .ToListAsync();

            return View(kupnje);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Posudi(int knjigaID)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rezultat = _context.Skladiste.Include(s => s.Knjiga).FirstOrDefault(s => s.Knjiga.Id == knjigaID);

            if (rezultat == null)
            {
                return RedirectToAction("Index");
            }

            DateTime vrijemePosudbe = DateTime.Now;
            DateTime vrijemePotrebnogVracanja = vrijemePosudbe.AddDays(30);


            Posudenje posudenje = new Posudenje();
            posudenje.Knjiga = rezultat.Knjiga;
            posudenje.User = _user;
            posudenje.DatumPosudbe = vrijemePosudbe;
            posudenje.DatumPotrebnogVracanja = vrijemePotrebnogVracanja;
            _context.Posudenja.Add(posudenje);
            _context.SaveChanges();


            Posudba posudba = new Posudba();
            posudba.Knjiga = rezultat.Knjiga;
            posudba.User = _user;
            posudba.DatumPosudbe = vrijemePosudbe;
            posudba.DatumPotrebnogVracanja = vrijemePotrebnogVracanja;
            _context.Posudbe.Add(posudba);
            _context.SaveChanges();
            // Uzima van iz skladišta kad se posudi knjiga
            _context.Skladiste.Remove(rezultat);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Vrati(int knjigaID)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);
            decimal kasnina = 0;
            var rezultatKnjige = _context.Posudenja.Include(s => s.Knjiga).FirstOrDefault(s => s.Knjiga.Id == knjigaID && s.User == _user);

            if (rezultatKnjige == null)
            {
                return RedirectToAction("Index");
            }
            DateTime vrijemeVracanja = DateTime.Now;


            if (rezultatKnjige.DatumPotrebnogVracanja < vrijemeVracanja)
            {
                DateTime holder = rezultatKnjige.DatumPotrebnogVracanja;

                do
                {
                    kasnina += 2;
                    holder.AddDays(1);
                } while (holder < vrijemeVracanja);
            }


            Vracanje vratnja = new Vracanje();
            vratnja.Knjiga = rezultatKnjige.Knjiga;
            vratnja.User = _user;
            vratnja.DatumVracanja = vrijemeVracanja;
            _context.Vracanja.Add(vratnja);

            _user.Novcanik = _user.Novcanik - kasnina;

            // Zabilježi se PROMJENA na useru
            _context.Entry(_user).State = EntityState.Modified;
            _context.Posudenja.Remove(rezultatKnjige);
            _context.SaveChanges();


            var knjiga = _context.Knjige.FirstOrDefault(k => k.Id == knjigaID);


            //Zapis novog skladista i njihovih podataka
            Skladiste skladiste = new Skladiste();
            skladiste.Knjiga = knjiga;

            _context.Skladiste.Add(skladiste);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Kupi(int knjigaID)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rezultat = _context.Skladiste.Include(s => s.Knjiga).FirstOrDefault(s => s.Knjiga.Id == knjigaID);

            if (rezultat == null)
            {
                return RedirectToAction("Index");
            }

            if (rezultat.Knjiga.Cijena > _user.Novcanik)
            {
                return RedirectToAction("Index");
            }

            _context.Skladiste.Remove(rezultat);

            _user.Novcanik = _user.Novcanik - rezultat.Knjiga.Cijena;
            _context.Entry(_user).State = EntityState.Modified;

            Kupnja kupnja = new Kupnja();
            kupnja.Knjiga = rezultat.Knjiga;
            kupnja.User = _user;
            _context.Kupnje.Add(kupnja);

            Transakcija transakcija = new Transakcija();
            transakcija.Iznos = rezultat.Knjiga.Cijena;
            transakcija.User = _user;
            _context.Transakcije.Add(transakcija);

            _context.SaveChanges();



            return RedirectToAction("Index");
        }
    }
}
