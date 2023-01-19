using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index()
        {
            HttpContext.Application["PosetiociTreninga"] = new List<Posetilac>();
            return View();
        }

        public ActionResult Treninzi()
        {
            HttpContext.Application["PosetiociTreninga"] = new List<Posetilac>();
            HttpContext.Application["Treneri"] = Data.CitanjeTrenera();
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Trener trener = treneri[korisnik.KorisnickoIme];
            List<GrupniTrening> prethodniTreninzi = new List<GrupniTrening>();
            List<GrupniTrening> buduciTreninzi = new List<GrupniTrening>();
            foreach(var trening in trener.GrupniTreninzi)
            {
                if(trening.DatumIVremeTreninga < DateTime.Now)
                {
                    prethodniTreninzi.Add(trening);
                }
            }
            foreach(var trening in trener.GrupniTreninzi)
            {
                if(trening.DatumIVremeTreninga > DateTime.Now && trening.Obrisan == false)
                {
                    buduciTreninzi.Add(trening);
                }
            }
            HttpContext.Application["PrethodniTreninzi"] = prethodniTreninzi;
            HttpContext.Application["BuduciTreninzi"] = buduciTreninzi;
            HttpContext.Application["RezervaTreninzi"] = prethodniTreninzi;
            return View();
        }
        public ActionResult Dodavanje()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DodavanjeTreninga(string naziv, string tipTreninga, string trajanjeTreninga, DateTime datumIVremeTreninga, string maksimalanBrojPosetilaca)
        {
            ViewBag.Message = "";
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["GrupniTreninzi"];
            if (grupniTreninzi.Any(x => x.Naziv == naziv))
            {
                ViewBag.Message = "Vec postoji trening sa zeljenim imenom. Pokusajte ponovo.";
                return View("Dodavanje");
            }
            if(!(Int32.TryParse(trajanjeTreninga, out int trajanje)) && trajanje <= 0)
            {
                ViewBag.Message = "Pogresan unos trajanja treninga. Pokusajte ponovo.";
                return View("Dodavanje");
            }
            if(datumIVremeTreninga < DateTime.Now.AddDays(3))
            {
                ViewBag.Message = "Trening mora biti zakazan najmanje 3 dana unapred.";
                return View("Dodavanje");
            }
            if (!(Int32.TryParse(maksimalanBrojPosetilaca, out int maksBroj)) && maksBroj <= 0)
            {
                ViewBag.Message = "Pogresan unos maksimalnog broja posetilaca. Pokusajte ponovo.";
                return View("Dodavanje");
            }
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Trener trener = treneri[korisnik.KorisnickoIme];
            TipTreninga tip = (TipTreninga)Enum.Parse(typeof(TipTreninga), tipTreninga);
            GrupniTrening trening = new GrupniTrening(naziv, tip, trener.FitnesCentar, trajanje, datumIVremeTreninga, maksBroj);
            trener.GrupniTreninzi.Add(trening);
            Data.AzuriranjeTrenera(trener);
            Data.UpisivanjeGrupnihTreninga(trening);
            return RedirectToAction("Treninzi");
        }
        [HttpPost]
        public ActionResult ModifikovanjeGrupnogTreninga(string naziv)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["BuduciTreninzi"];
            GrupniTrening grupniTrening = new GrupniTrening();
            foreach (var trening in treninzi)
            {
                if (trening.Naziv == naziv)
                {
                    grupniTrening = trening;
                    break;
                }

            }
            HttpContext.Application["GrupniTrening"] = grupniTrening;
            return View("Modifikovanje");
        }
        [HttpPost]
        public ActionResult ModifikacijaGrupnogTreninga(string naziv, string tipTreninga, string trajanjeTreninga, DateTime datumIVremeTreninga, string maksimalanBrojPosetilaca)
        {
            GrupniTrening grupniTrening = (GrupniTrening)HttpContext.Application["GrupniTrening"];
            if (datumIVremeTreninga < DateTime.Now)
            {
                ViewBag.Message = "Nemoguca modifikacija treninga. Vreme nije u ispravnom formatu.";
                return View("Modifikovanje");
            }
            if (naziv != grupniTrening.Naziv)
            {
                List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["GrupniTreninzi"];
                if (grupniTreninzi.Any(x => x.Naziv == naziv))
                {
                    ViewBag.Message = "Trening sa zeljenim nazivom vec postoji. Pokusajte ponovo.";
                    return View("Modifikovanje");
                }
            }
            if(!Int32.TryParse(trajanjeTreninga, out int trajanje))
            {
                ViewBag.Message = "Trajanje nije uneseno u ispravnom formatu. Pokusajte ponovo.";
                return View("Modifikovanje");
            }
            if(!Int32.TryParse(maksimalanBrojPosetilaca, out int maks))
            {
                ViewBag.Message = "Broj posetilaca nije unesen u ispravnom formatu. Pokusajte ponovo.";
                return View("Modifikovanje");
            }
            grupniTrening.Naziv = naziv;
            grupniTrening.TipTreninga = (TipTreninga)Enum.Parse(typeof(TipTreninga), tipTreninga);
            grupniTrening.TrajanjeTreninga = trajanje;
            grupniTrening.DatumIVremeTreninga = datumIVremeTreninga;
            grupniTrening.MaksimalanBrojPosetilaca = maks;

            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Trener trener = treneri[korisnik.KorisnickoIme];
            foreach(var trening in trener.GrupniTreninzi)
            {
                if(trening.TreningId == grupniTrening.TreningId)
                {
                    trener.GrupniTreninzi.Remove(trening);
                    trener.GrupniTreninzi.Add(grupniTrening);
                    treneri[trener.KorisnickoIme] = trener;
                    break;
                }
            }
            Data.AzuriranjeGrupnihTreninga(grupniTrening);
            Data.AzuriranjeTrenera(trener);

            Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
            foreach(var posetilac in posetioci.Values)
            {
                foreach(var trening in posetilac.GrupniTreninzi)
                {
                    if(trening.TreningId == grupniTrening.TreningId)
                    {
                        posetilac.GrupniTreninzi.Remove(trening);
                        posetilac.GrupniTreninzi.Add(grupniTrening);
                        Data.AzuriranjePosetioca(posetilac);
                        break;
                    }
                }
            }
            HttpContext.Application["Treneri"] = treneri;
            HttpContext.Application["Posetioci"] = Data.CitanjePosetilaca();
            return RedirectToAction("Treninzi");
        }
        [HttpPost]
        public ActionResult BrisanjeGrupnogTreninga(string naziv)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["BuduciTreninzi"];
            GrupniTrening grupniTrening = new GrupniTrening();
            foreach(var trening in grupniTreninzi)
            {
                if(trening.Naziv == naziv)
                {
                    grupniTrening = trening;
                    break;
                }
            }
            if (grupniTrening.SpisakPosetilaca.Count != 0)
            {
                ViewBag.Message = "Postoje prijavljeni posetioci za ovaj trening. Ne mozete ga obrisati.";
                return View("Treninzi");
            }
            grupniTrening.Obrisan = true;
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Trener trener = treneri[korisnik.KorisnickoIme];
            foreach(var trening in trener.GrupniTreninzi)
            {
                if(trening.TreningId == grupniTrening.TreningId)
                {
                    trener.GrupniTreninzi.Remove(trening);
                    trener.GrupniTreninzi.Add(grupniTrening);
                    treneri[korisnik.KorisnickoIme] = trener;
                    break;
                }
            }
            Data.AzuriranjeGrupnihTreninga(grupniTrening);
            Data.AzuriranjeTrenera(trener);
            Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
            foreach(var posetilac in posetioci.Values)
            {
                foreach(var trening in posetilac.GrupniTreninzi)
                {
                    if(trening.TreningId == grupniTrening.TreningId)
                    {
                        posetilac.GrupniTreninzi.Remove(trening);
                        posetilac.GrupniTreninzi.Add(grupniTrening);
                        Data.AzuriranjePosetioca(posetilac);
                        break;
                    }
                }
            }
            HttpContext.Application["Treneri"] = treneri;
            return RedirectToAction("Treninzi");
        }
        [HttpPost]
        public ActionResult PrikazPosetilaca(string naziv)
        {
            List<GrupniTrening> prethodniTreninzi = (List<GrupniTrening>)HttpContext.Application["RezervaTreninzi"];
            List<GrupniTrening> buduciTreninzi = (List<GrupniTrening>)HttpContext.Application["BuduciTreninzi"];
            List<Posetilac> posetioci = new List<Posetilac>();
            GrupniTrening grupniTrening = new GrupniTrening();
            foreach(var trening in prethodniTreninzi)
            {
                if(trening.Naziv == naziv)
                {
                    grupniTrening = trening;
                    break;
                }
            }
            foreach(var trening in buduciTreninzi)
            {
                if(trening.Naziv == naziv)
                {
                    grupniTrening = trening;
                    break;
                }
            }
            HttpContext.Application["PosetiociTreninga"] = grupniTrening.SpisakPosetilaca;
            return View("Treninzi");
        }
        [HttpPost]
        public ActionResult Sortiranje(string parametar, string vrednost)
        {
            IEnumerable<GrupniTrening> grupniTreninzi = (IEnumerable<GrupniTrening>)HttpContext.Application["RezervaTreninzi"];
            IEnumerable<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            if (parametar == "naziv")
            {
                if (vrednost == "rastuce")
                {
                    tempTreninzi = grupniTreninzi.OrderBy(x => x.Naziv);
                    HttpContext.Application["PrethodniTreninzi"] = tempTreninzi;
                }
                else if (vrednost == "opadajuce")
                {
                    tempTreninzi = grupniTreninzi.OrderByDescending(x => x.Naziv);
                    HttpContext.Application["PrethodniTreninzi"] = tempTreninzi;
                }
            }
            else if (parametar == "tipTreninga")
            {
                if (vrednost == "rastuce")
                {
                    tempTreninzi = grupniTreninzi.OrderBy(x => x.TipTreninga);
                    HttpContext.Application["PrethodniTreninzi"] = tempTreninzi;
                }
                else if (vrednost == "opadajuce")
                {
                    tempTreninzi = grupniTreninzi.OrderByDescending(x => x.TipTreninga);
                    HttpContext.Application["PrethodniTreninzi"] = tempTreninzi;
                }
            }
            else if (parametar == "datumIVremeOdrzavanja")
            {
                if (vrednost == "rastuce")
                {
                    tempTreninzi = grupniTreninzi.OrderBy(x => x.DatumIVremeTreninga);
                    HttpContext.Application["PrethodniTreninzi"] = tempTreninzi;
                }
                else if (vrednost == "opadajuce")
                {
                    tempTreninzi = grupniTreninzi.OrderByDescending(x => x.DatumIVremeTreninga);
                    HttpContext.Application["PrethodniTreninzi"] = tempTreninzi;
                }
            }
            return View("Treninzi");
        }
        [HttpPost]
        public ActionResult Pretraga(string naziv, string tipTreninga, FormCollection form)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["RezervaTreninzi"];
            DateTime dt = new DateTime();
            DateTime.TryParse(form.Get("donjaGranicaOdrzavanja"), out DateTime donjaGranicaOdrzavanja);
            DateTime.TryParse(form.Get("gornjaGranicaOdrzavanja"), out DateTime gornjaGranicaOdrzavanja);
            if (naziv != "")
            {
                grupniTreninzi = NazivFilter(grupniTreninzi, naziv);
            }
            if (tipTreninga != "")
            {
                grupniTreninzi = TipTreninga(grupniTreninzi, tipTreninga);
            }
            if (donjaGranicaOdrzavanja != dt)
            {
                grupniTreninzi = DonjaGranica(grupniTreninzi, donjaGranicaOdrzavanja);
            }
            if (gornjaGranicaOdrzavanja != dt)
            {
                grupniTreninzi = GornjaGranica(grupniTreninzi, gornjaGranicaOdrzavanja);
            }
            HttpContext.Application["ProtekliTreninzi"] = grupniTreninzi;
            return View("Treninzi");
        }
        public List<GrupniTrening> NazivFilter(List<GrupniTrening> treninzi, string naziv)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if (f.Naziv.Contains(naziv))
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }
        public List<GrupniTrening> TipTreninga(List<GrupniTrening> treninzi, string tip)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if (f.TipTreninga.ToString() == tip)
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }
        public List<GrupniTrening> DonjaGranica(List<GrupniTrening> treninzi, DateTime date)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if (f.DatumIVremeTreninga >= date)
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }
        public List<GrupniTrening> GornjaGranica(List<GrupniTrening> treninzi, DateTime date)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if (f.DatumIVremeTreninga <= date)
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }
    }
}