using BibliotekaWeb.Models.ViewModeli;
using BibliotekaWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BibliotekaWeb.Data;
using BibliotekaWeb.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BibliotekaWeb.Controllers
{
    [Authorize(Roles = "Moderator")]

    public class ModeratorController : Controller
    {
        private readonly BibliotekaWebContext _context;
        private readonly UserManager<BibliotekaWebUser> _userManager;
        private BibliotekaWebUser _user;

        public ModeratorController(BibliotekaWebContext context, UserManager<BibliotekaWebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Knjige()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Knjiga> knjige = new List<Knjiga>();
            KnjigaViewModel knjigeViewModel = new KnjigaViewModel();
            knjige = _context.Knjige.Include(k => k.Zanr).Include(k => k.Autor).ToList();
            knjigeViewModel.Autori = _context.Autori.ToList();
            knjigeViewModel.Zanrovi = _context.Zanrovi.ToList();
            knjigeViewModel.Knjige = knjige;
            return View(knjigeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Knjige(KnjigaViewModel model, string Zanar, string Autor)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rezultat = _context.Knjige.FirstOrDefault(k => k.Naziv.ToLower() == model.Naziv.ToLower());

            if (rezultat != null)
            {
                return RedirectToAction("Knjige");
            }

            if (ModelState.IsValid)
            {

            //Zapis nove knjige i njezinih podataka
            Knjiga knjiga = new Knjiga();
            knjiga.Naziv = model.Naziv;
            //Povlacanje oznacenog autora iz baze
            Autor autor = _context.Autori.FirstOrDefault(a => a.FullIme == Autor);
            knjiga.Autor = autor;
            //Povlacenje oznacenog zanra iz baze
            Zanr zanar = _context.Zanrovi.FirstOrDefault(z => z.Naziv == Zanar);
            knjiga.Zanr = zanar;
            knjiga.Cijena = model.Cijena;
            //Dodavanje knjige u bazu
            _context.Knjige.Add(knjiga);
            //Spremanje promjena
            _context.SaveChanges();
        }
            return RedirectToAction("Knjige");
        }

        [HttpGet]
        public async Task<IActionResult> Skladiste()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Skladiste> skladiste = new List<Skladiste>();
            SkladisteViewModel skladisteViewModel = new SkladisteViewModel();
            skladisteViewModel.Knjige = _context.Knjige.ToList();
            skladiste = _context.Skladiste.Include(k => k.Knjiga).Include(k => k.Knjiga.Zanr).Include(k => k.Knjiga.Autor).ToList();
            skladisteViewModel.Skladiste = skladiste;
            return View(skladisteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Skladiste(int knjigaId)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var knjiga = await _context.Knjige.FindAsync(knjigaId);
            if (knjiga == null)
            {
                return NotFound();
            }

            Skladiste skladiste = new Skladiste
            {
                KnjigaId = knjigaId,
                Knjiga = knjiga
            };

            _context.Skladiste.Add(skladiste);
            await _context.SaveChangesAsync();

            return RedirectToAction("Skladiste");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteKnjiga(int knjigaId)
        {
            var skladiste = await _context.Skladiste.FirstOrDefaultAsync(s => s.Knjiga.Id == knjigaId);
            if (skladiste != null)
            {
                _context.Skladiste.Remove(skladiste);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Skladiste");
        }


        [HttpGet]
        public async Task<IActionResult> Zanrovi()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Zanr> zanrovi = new List<Zanr>();
            ZanrViewModel zanroviViewModel = new ZanrViewModel();
            zanrovi = _context.Zanrovi.ToList();
            zanroviViewModel.Zanrovi = zanrovi;

            return View(zanroviViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Zanrovi(ZanrViewModel model)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rezultat = _context.Zanrovi.FirstOrDefault(z => z.Naziv.ToLower() == model.Naziv.ToLower());

            if (rezultat != null)
            {
                return RedirectToAction("Zanrovi");
            }

            Zanr zanar = new Zanr();

            zanar.Naziv = model.Naziv;
            _context.Zanrovi.Add(zanar);
            _context.SaveChanges();

            return RedirectToAction("Zanrovi");
        }

        [HttpGet]
        public async Task<IActionResult> Autori()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Autor> autori = new List<Autor>();
            AutorViewModel autoriViewModel = new AutorViewModel();
            autori = _context.Autori.ToList();
            autoriViewModel.Autori = autori;

            return View(autoriViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Autori(AutorViewModel model)
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rezultat = _context.Autori.FirstOrDefault(a => a.FullIme.ToLower() == model.ImeFull.ToLower());

            if (rezultat != null)
            {
                return RedirectToAction("Autori");
            }

            Autor autor = new Autor();

            autor.FullIme = model.ImeFull;
            _context.Autori.Add(autor);
            _context.SaveChanges();


            return RedirectToAction("Autori");
        }

        [HttpGet]
        public async Task<IActionResult> PregledTransakcija()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Transakcija> transakcije = new List<Transakcija>();

            transakcije = _context.Transakcije.Include(u => u.User).ToList();

            return View(transakcije);
        }

        [HttpGet]
        public async Task<IActionResult> PregledVracanja()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Vracanje> vracanja = new List<Vracanje>();

            vracanja = _context.Vracanja.Include(u => u.User).Include(k => k.Knjiga).ToList();

            return View(vracanja);
        }

        [HttpGet]
        public async Task<IActionResult> PregledPosudbi()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Posudba> posudbe = new List<Posudba>();

            posudbe = _context.Posudbe.Include(u => u.User).Include(k => k.Knjiga).ToList();

            return View(posudbe);
        }

        [HttpGet]
        public async Task<IActionResult> PregledKupnji()
        {
            if (_user == null)
                _user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Kupnja> kupnje = new List<Kupnja>();

            kupnje = _context.Kupnje.Include(u => u.User).Include(k => k.Knjiga).ToList();

            return View(kupnje);
        }
    }
}
