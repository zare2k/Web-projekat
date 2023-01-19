using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Data.PocetnoStanje();
            Korisnik korisnik = null;
            HttpContext.Session["korisnik"] = korisnik;
            Dictionary<string, Vlasnik> vlasnici = Data.CitanjeVlasnika();
            List<FitnesCentar> fitnessCentar = Data.CitanjeFitnesCentara(vlasnici);
            List<FitnesCentar> neobrisani = new List<FitnesCentar>();
            foreach (var centar in fitnessCentar)
            {
                if (centar.Obrisan == false)
                    neobrisani.Add(centar);
            }
            HttpContext.Application["FitnesCentri"] = neobrisani;
            HttpContext.Application["Centri"] = neobrisani.OrderBy(x => x.Naziv);
            HttpContext.Application["RezervaCentri"] = neobrisani;
            return View();
        }
        
        [HttpPost]
        public ActionResult Sortiranje(string parametar, string vrednost)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["RezervaCentri"];
            if(parametar=="naziv")
            {
                if(vrednost == "rastuce")
                {
                    HttpContext.Application["Centri"] = fitnesCentri.OrderBy(x => x.Naziv);
                    return View("Index");
                }
                else if (vrednost == "opadajuce")
                {
                    HttpContext.Application["Centri"] = fitnesCentri.OrderByDescending(x => x.Naziv);
                    return View("Index");
                }
            }
            else if (parametar == "adresa")
            {
                if (vrednost == "rastuce")
                {
                    HttpContext.Application["Centri"] = fitnesCentri.OrderBy(x => x.Adresa);
                    return View("Index");
                }
                else if (vrednost == "opadajuce")
                {
                    HttpContext.Application["Centri"] = fitnesCentri.OrderByDescending(x => x.Adresa);
                    return View("Index");
                }
            }
            else if (parametar == "godinaOtvaranja")
            {
                if (vrednost == "rastuce")
                {
                    HttpContext.Application["Centri"] = fitnesCentri.OrderBy(x => x.GodinaOtvaranja);
                    return View("Index");
                }
                else if (vrednost == "opadajuce")
                {
                    HttpContext.Application["Centri"] = fitnesCentri.OrderByDescending(x => x.GodinaOtvaranja);
                    return View("Index");
                }
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult Pretraga(string naziv, string adresa, string godinaOtvaranjaDonja, string godinaOtvaranjaGornja)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["RezervaCentri"];
            if (naziv != "")
            {
                List<FitnesCentar> fs = new List<FitnesCentar>();
                foreach(var f in fitnesCentri)
                {
                    if (f.Naziv.Contains(naziv))
                    {
                        fs.Add(f);
                        
                    }
                    fitnesCentri = fs;
                }
            }
            if (adresa != "")
            {
                List<FitnesCentar> fs = new List<FitnesCentar>();
                foreach(var f in fitnesCentri)
                {
                    if (f.Adresa.Contains(adresa))
                    {
                        fs.Add(f);
                        
                    }
                    fitnesCentri = fs;
                }              
            }
            if (godinaOtvaranjaDonja != "")
            {
                if(Int32.TryParse(godinaOtvaranjaDonja, out int godinaDonja))
                {
                    List<FitnesCentar> fs = new List<FitnesCentar>();
                    foreach(var f in fitnesCentri)
                    {
                        if (f.GodinaOtvaranja >= godinaDonja)
                        {
                            fs.Add(f);
                            
                        }
                        fitnesCentri = fs;
                    }                   
                }
            }
            if (godinaOtvaranjaGornja != "")
            {
                if (Int32.TryParse(godinaOtvaranjaGornja, out int godinaGornja))
                {
                    List<FitnesCentar> fs = new List<FitnesCentar>();
                    foreach (var f in fitnesCentri)
                    {
                        if (f.GodinaOtvaranja <= godinaGornja)
                        {
                            fs.Add(f);
                            
                        }
                        fitnesCentri = fs;
                    }                    
                }
            }
            HttpContext.Application["Centri"] = fitnesCentri;
            return View("Index");
        }
        public ActionResult Registracija()
        {
            return View("RegistracijaForma");
        }
        [HttpPost]
        public ActionResult RegistracijaForma(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, DateTime datumRodjenja)
        {
            ViewBag.Message = "";
            Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            if (posetioci.ContainsKey(korisnickoIme) || treneri.ContainsKey(korisnickoIme) || vlasnici.ContainsKey(korisnickoIme))
            {
                ViewBag.Message = "Korisnicko ime zauzeto, izaberite drugo.";
                return View("RegistracijaForma");
            }
            if(lozinka.Length < 5)
            {
                ViewBag.Message = "Lozinka mora sadrzati najmanje 5 karaktera.";
                return View("RegistracijaForma");
            }
            if(datumRodjenja.AddYears(16) > DateTime.Now)
            {
                ViewBag.Message = "Morate imati najmanje 16 godina da biste se prijavili.";
                return View("RegistracijaForma");
            }

            ViewBag.Message = "Uspesna registracija.";
            Pol polKorisnika = (Pol)Enum.Parse(typeof(Pol), pol);
            Posetilac posetilac = new Posetilac(korisnickoIme, lozinka, ime, prezime, polKorisnika, email, datumRodjenja.ToString());
            posetioci.Add(posetilac.KorisnickoIme, posetilac);
            //HttpContext.Application["Posetioci"] = posetioci;
            HttpContext.Session["Posetioci"] = posetioci;
            Data.UpisivanjePosetilaca(posetilac);
            return View("Index");
        }
        public ActionResult Prijava()
        {
            return View("PrijavaForma");
        }
        [HttpPost]
        public ActionResult PrijavaForma(string korisnickoIme, string lozinka)
        {
            ViewBag.Message = "";
            Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
            Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
            bool postoji = false;
            if (posetioci.ContainsKey(korisnickoIme))
            {
                postoji = true;
                if(posetioci[korisnickoIme].KorisnickoIme==korisnickoIme && posetioci[korisnickoIme].Lozinka == lozinka)
                {
                    Korisnik korisnik = (Korisnik)posetioci[korisnickoIme];
                    korisnik.Uloga = Uloga.POSETILAC;
                    HttpContext.Session["Korisnik"] = korisnik;
                }
                else
                {
                    ViewBag.Message = "Pogresno korisnicko ime ili lozinka, pokusajte ponovo.";
                    return View("PrijavaForma");
                }
            }
            if (treneri.ContainsKey(korisnickoIme))
            {
                postoji = true;
                if (treneri[korisnickoIme].KorisnickoIme == korisnickoIme && treneri[korisnickoIme].Lozinka == lozinka)
                {
                    Korisnik korisnik = (Korisnik)treneri[korisnickoIme];
                    korisnik.Uloga = Uloga.TRENER;
                    HttpContext.Session["Korisnik"] = korisnik;
                }
                else
                {
                    ViewBag.Message = "Pogresno korisnicko ime ili lozinka, pokusajte ponovo.";
                    return View("PrijavaForma");
                }
            }
            if (vlasnici.ContainsKey(korisnickoIme))
            {
                postoji = true;
                if (vlasnici[korisnickoIme].KorisnickoIme == korisnickoIme && vlasnici[korisnickoIme].Lozinka == lozinka)
                {
                    Korisnik korisnik = (Korisnik)vlasnici[korisnickoIme];
                    korisnik.Uloga = Uloga.VLASNIK;
                    HttpContext.Session["Korisnik"] = korisnik;
                }
                else
                {
                    ViewBag.Message = "Pogresno korisnicko ime ili lozinka, pokusajte ponovo.";
                    return View("PrijavaForma");
                }
            }
            if (postoji)
            {
                return View("Index");
            }
            else
            {
                ViewBag.Message = "Pogresno korisnicko ime ili lozinka, pokusajte ponovo.";
                return View("PrijavaForma");
            }        
        }
        public ActionResult Odjava()
        {
            Korisnik korisnik = null;
            HttpContext.Session["Korisnik"] = korisnik;
            return View("Index");
        }
        public ActionResult PregledProfila()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IzmeniProfil(string lozinka, string ime, string prezime)
        {
            //ViewBag.Message = "";
            //bool izmenjen = false;
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            if (korisnik.Uloga == Uloga.POSETILAC)
            {
                Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
                Posetilac posetilac = posetioci[korisnik.KorisnickoIme];
                
                posetilac.Lozinka = lozinka;
                posetilac.Ime = ime;
                posetilac.Prezime = prezime;
                Data.AzuriranjePosetioca(posetilac);
                HttpContext.Application["Posetioci"] = Data.CitanjePosetilaca();
            }
            if (korisnik.Uloga == Uloga.TRENER)
            {
                Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
                Trener trener = treneri[korisnik.KorisnickoIme];
                
                trener.Lozinka = lozinka;
                trener.Ime = ime;
                trener.Prezime = prezime;
                Data.AzuriranjeTrenera(trener);
                HttpContext.Application["Treneri"] = Data.CitanjeTrenera();
            }
            if (korisnik.Uloga == Uloga.VLASNIK)
            {
                Dictionary<string, Vlasnik> vlasnici = (Dictionary<string, Vlasnik>)HttpContext.Application["Vlasnici"];
                Vlasnik vlasnik = vlasnici[korisnik.KorisnickoIme];
                
                vlasnik.Lozinka = lozinka;
                vlasnik.Ime = ime;
                vlasnik.Prezime = prezime;
                Data.AzuriranjeVlasnika(vlasnik);
                HttpContext.Application["Vlasnici"] = Data.CitanjeVlasnika();
            }
            return View("Index");
        }
        public ActionResult Povratak()
        {
            return View("Index");
        }
    }
}