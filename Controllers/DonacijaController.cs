using Microsoft.AspNetCore.Mvc;
using BibliotekaWeb.Models.ViewModeli;

namespace BibliotekaWeb.Controllers
{
    public class DonacijeController : Controller
    {
        [HttpGet]
        public IActionResult Donacije()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Kreiraj(DonacijaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Potvrda poslanog emaila
                TempData["Message"] = "Vaš upit za donaciju je podnesen.";
                return RedirectToAction("Donacije");
            }
            return View(model);
        }
    }
}
