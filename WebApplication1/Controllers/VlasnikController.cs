using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PregledCentara()
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            List<FitnesCentar> fitnesCentri = new List<FitnesCentar>();
            foreach (var fitnesCentar in vlasnik.FitnesCentri)
            {
                if (fitnesCentar.Obrisan == false)
                {
                    fitnesCentri.Add(fitnesCentar);
                }
            }
            HttpContext.Application["fitnesCentri"] = fitnesCentri;
            return View();
        }
        public ActionResult KreiranjeFitnesCentra()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KreirajFitnesCentar(string naziv, string adresa, string godinaOtvaranja, string mesecnaCena, string godisnjaCena, string cenaJednogTreninga, string cenaGrupnogTreninga, string cenaTreningaSaTrenerom)
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            ViewBag.Message = "";
            if (!Int32.TryParse(godinaOtvaranja, out int godina))
            {
                ViewBag.Message = "Nepravilan unos godine otvaranja. Pokusajte ponovo.";
                return View("KreiranjeFitnesCentra");
            }
            if (!Double.TryParse(mesecnaCena, out double mesecCena) && mesecCena <= 0)
            {
                ViewBag.Message = "Nepravilan unos mesecne cene. Pokusajte ponovo.";
                return View("KreiranjeFitnesCentra");
            }
            if (!Double.TryParse(godisnjaCena, out double godinaCena) && godinaCena <= 0)
            {
                ViewBag.Message = "Nepravilan unos godisnje cene. Pokusajte ponovo.";
                return View("KreiranjeFitnesCentra");
            }
            if (!Double.TryParse(cenaJednogTreninga, out double cenaJednog) && cenaJednog <= 0)
            {
                ViewBag.Message = "Nepravilan unos cene treninga. Pokusajte ponovo.";
                return View("KreiranjeFitnesCentra");
            }
            if (!Double.TryParse(cenaGrupnogTreninga, out double cenaGrupnog) && cenaGrupnog <= 0)
            {
                ViewBag.Message = "Nepravilan unos cene grupnog treninga. Pokusajte ponovo.";
                return View("KreiranjeFitnesCentra");
            }
            if (!Double.TryParse(cenaTreningaSaTrenerom, out double cenaSaTrenerom) && cenaSaTrenerom <= 0)
            {
                ViewBag.Message = "Nepravilan unos cene treninga sa trenerom. Pokusajte ponovo.";
                return View("KreiranjeFitnesCentra");
            }

            FitnesCentar fitnesCentar = new FitnesCentar(naziv, adresa, godina, vlasnik, mesecCena, godinaCena, cenaJednog, cenaGrupnog, cenaSaTrenerom);
            vlasnik.FitnesCentri.Add(fitnesCentar);
            Data.AzuriranjeVlasnika(vlasnik);
            HttpContext.Application["Vlasnici"] = Data.CitanjeVlasnika();
            List<FitnesCentar> fitnesCentri = Data.CitanjeFitnesCentara(vlasnici);
            List<FitnesCentar> fc = new List<FitnesCentar>();
            foreach(var f in vlasnik.FitnesCentri)
            {
                if(f.Obrisan == false)
                {
                    fc.Add(f);
                }
            }
            HttpContext.Application["Centri"] = fitnesCentri.OrderBy(x => x.Naziv);
            HttpContext.Application["fitnesCentri"] = fc;
            return View("PregledCentara");
        }
        [HttpPost]
        public ActionResult RegistrovanjeTrenera(string naziv)
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            FitnesCentar fitnesCentar = new FitnesCentar();
            foreach (var fc in vlasnik.FitnesCentri)
            {
                if (fc.Naziv == naziv)
                {
                    fitnesCentar = fc;
                    break;
                }
            }
            HttpContext.Application["FitnesCentarMod"] = fitnesCentar;
            return View();
        }
        [HttpPost]
        public ActionResult RegistrujTrenera(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, DateTime datumRodjenja, string fitnesCentar)
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            FitnesCentar centar = new FitnesCentar();
            ViewBag.Message = "";
            if (datumRodjenja.AddYears(18) > DateTime.Now)
            {
                ViewBag.Message = "Trener mora biti punoletan. Pokusajte ponovo.";
                return View("RegistrovanjeTrenera");
            }
            foreach (var fc in vlasnik.FitnesCentri)
            {
                if (fc.Naziv == fitnesCentar)
                {
                    centar = fc;
                    break;
                }
            }
            Pol p = (Pol)Enum.Parse(typeof(Pol), pol);
            Trener trener = new Trener(korisnickoIme, lozinka, ime, prezime, p, email, datumRodjenja.ToString(), centar);
            Data.UpisivanjeTrenera(trener);
            HttpContext.Application["Treneri"] = Data.CitanjeTrenera();
            return View("PregledCentara");
        }
        [HttpPost]
        public ActionResult BrisanjeFitnesCentra(string naziv)
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            FitnesCentar fitnesCentar = new FitnesCentar();
            foreach (var fc in vlasnik.FitnesCentri)
            {
                if (fc.Naziv == naziv)
                {
                    fitnesCentar = fc;
                    break;
                }
            }
            List<GrupniTrening> grupniTreninzi = Data.CitanjeGrupnihTreninga();
            bool postojiTrening = false;
            foreach (var gt in grupniTreninzi)
            {
                if (gt.FitnesCentar == fitnesCentar && gt.SpisakPosetilaca.Count > 0 && gt.DatumIVremeTreninga > DateTime.Now)
                {
                    postojiTrening = true;
                }
            }
            if (postojiTrening == false)
            {
                foreach (var fCentar in vlasnik.FitnesCentri)
                {
                    if (fCentar.Naziv == naziv)
                    {
                        fCentar.Obrisan = true;
                        fitnesCentar.Obrisan = true;
                        break;
                    }
                }
                Data.AzuriranjeVlasnika(vlasnik);
                HttpContext.Application["Vlasnici"] = Data.CitanjeVlasnika();
                Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
                foreach (var trener in treneri.Values)
                {
                    if (trener.FitnesCentar.Naziv == naziv)
                    {
                        trener.Blokiran = true;
                        Data.AzuriranjeTrenera(trener);
                    }
                }
                HttpContext.Application["Treneri"] = Data.CitanjeTrenera();
            }
            else
            {
                ViewBag.Message = "Ne mozete da obrisete fitnes centar koji ima planirane treninge.";
                return View("PregledCentara");
            }
            return RedirectToAction("PregledCentara");
        }
        [HttpPost]
        public ActionResult ModifikovanjeFitnesCentra(string naziv)
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            FitnesCentar fitnesCentar = new FitnesCentar();
            foreach (var fc in vlasnik.FitnesCentri)
            {
                if (fc.Naziv == naziv)
                {
                    fitnesCentar = fc;
                    break;
                }
            }
            HttpContext.Application["FitnesCentarMod"] = fitnesCentar;
            return View();
        }
        [HttpPost]
        public ActionResult ModifikujFitnesCentar(string naziv, string adresa, string godinaOtvaranja, string mesecnaCena, string godisnjaCena, string cenaJednogTreninga, string cenaGrupnogTreninga, string cenaTreningaSaTrenerom)
        {
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
            ViewBag.Message = "";
            if (!Int32.TryParse(godinaOtvaranja, out int godina))
            {
                ViewBag.Message = "Nepravilan unos godine otvaranja. Pokusajte ponovo.";
                return View("ModifikovanjeFitnesCentra");
            }
            if (!Double.TryParse(mesecnaCena, out double mesecCena) && mesecCena <= 0)
            {
                ViewBag.Message = "Nepravilan unos mesecne cene. Pokusajte ponovo.";
                return View("ModifikovanjeFitnesCentra");
            }
            if (!Double.TryParse(godisnjaCena, out double godinaCena) && godinaCena <= 0)
            {
                ViewBag.Message = "Nepravilan unos godisnje cene. Pokusajte ponovo.";
                return View("ModifikovanjeFitnesCentra");
            }
            if (!Double.TryParse(cenaJednogTreninga, out double cenaJednog) && cenaJednog <= 0)
            {
                ViewBag.Message = "Nepravilan unos cene treninga. Pokusajte ponovo.";
                return View("ModifikovanjeFitnesCentra");
            }
            if (!Double.TryParse(cenaGrupnogTreninga, out double cenaGrupnog) && cenaGrupnog <= 0)
            {
                ViewBag.Message = "Nepravilan unos cene grupnog treninga. Pokusajte ponovo.";
                return View("ModifikovanjeFitnesCentra");
            }
            if (!Double.TryParse(cenaTreningaSaTrenerom, out double cenaSaTrenerom) && cenaSaTrenerom <= 0)
            {
                ViewBag.Message = "Nepravilan unos cene treninga sa trenerom. Pokusajte ponovo.";
                return View("ModifikovanjeFitnesCentra");
            }
            FitnesCentar fitnesCentar = (FitnesCentar)HttpContext.Application["FitnesCentarMod"];
            FitnesCentar fc = new FitnesCentar(naziv, adresa, godina, vlasnik, mesecCena, godinaCena, cenaJednog, cenaGrupnog, cenaSaTrenerom);
            vlasnik.FitnesCentri.Remove(fitnesCentar);
            vlasnik.FitnesCentri.Add(fc);
            Data.AzuriranjeVlasnika(vlasnik);
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["GrupniTreninzi"];
            foreach(var grupniTrening in grupniTreninzi)
            {
                if(grupniTrening.FitnesCentar.Naziv == naziv)
                {
                    grupniTrening.FitnesCentar = fitnesCentar;
                    Data.AzuriranjeGrupnihTreninga(grupniTrening);
                }
            }
            foreach(var trener in treneri.Values)
            {
                if (trener.FitnesCentar == fitnesCentar)
                {
                    trener.FitnesCentar = fc;
                    Data.AzuriranjeTrenera(trener);
                }
            }
            Dictionary<string, Vlasnik> modifikovaniVlasnici = Data.CitanjeVlasnika();
            HttpContext.Application["GrupniTreninzi"] = Data.CitanjeGrupnihTreninga();
            HttpContext.Application["Vlasnici"] = modifikovaniVlasnici;
            List<FitnesCentar> fitnesCentri = Data.CitanjeFitnesCentara(modifikovaniVlasnici);
            HttpContext.Application["FitnesCentri"] = fitnesCentri;
            HttpContext.Application["Centri"] = fitnesCentri.OrderBy(x => x.Naziv);
            return View("PregledCentara");
        }
        [HttpPost]
        public ActionResult PrikazTrenera(string naziv)
        {
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            List<Trener> treneriPrikaz = new List<Trener>();
            foreach(var trener in treneri.Values)
            {
                if(trener.FitnesCentar.Naziv == naziv && trener.Blokiran == false)
                {
                    treneriPrikaz.Add(trener);
                }
            }
            HttpContext.Application["TreneriPrikaz"] = treneriPrikaz;
            return View();
        }
        [HttpPost]
        public ActionResult BlokirajTrenera(string korisnickoIme)
        {
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Trener trener = treneri[korisnickoIme];
            trener.Blokiran = true;
            Data.AzuriranjeTrenera(trener);
            HttpContext.Application["Treneri"] = Data.CitanjeTrenera();
            return View("PregledCentara");
        }
    }
}