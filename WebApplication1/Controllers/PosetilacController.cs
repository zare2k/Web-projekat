using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PrijavaNaTrening(string naziv)
        {
            Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            //List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["GrupniTreninzi"];
            //Dictionary<string, Trener> treneri = (Dictionary<string, Trener>)HttpContext.Application["Treneri"];
            List<GrupniTrening> grupniTreninzi = Data.CitanjeGrupnihTreninga();
            Dictionary<string, Trener> treneri = Data.CitanjeTrenera();
            Trener trener = new Trener();
            Posetilac posetilac = posetioci[korisnik.KorisnickoIme];
            foreach(var trening in grupniTreninzi)
            {
                if(trening.Naziv==naziv && trening.SpisakPosetilaca.Count < trening.MaksimalanBrojPosetilaca)
                {
                    foreach(var trening1 in posetilac.GrupniTreninzi)
                    {
                        if(trening1.Naziv == naziv)
                        {
                            return RedirectToAction("Povratak", "Home");
                        }
                    }
                    trening.SpisakPosetilaca.Add(posetilac);
                    posetilac.GrupniTreninzi.Add(trening);
                    foreach (var trener1 in treneri.Values)
                    {
                        foreach(var grupniTrening in trener1.GrupniTreninzi)
                        {
                            if(grupniTrening.Naziv == naziv)
                            {
                                trener = trener1;
                                break;
                            }
                        }
                    }
                    foreach(var trening2 in trener.GrupniTreninzi)
                    {
                        if(trening2.Naziv == naziv)
                        {
                            trening2.SpisakPosetilaca.Add(posetilac);
                        }
                    }
                    Data.AzuriranjeGrupnihTreninga(trening);
                    Data.AzuriranjePosetioca(posetilac);
                    Data.AzuriranjeTrenera(trener);
                    break;
                }
            }
            //posetioci[korisnik.KorisnickoIme]=posetilac;
            posetioci[posetilac.KorisnickoIme] = posetilac;
            HttpContext.Application["Posetioci"] = posetioci;
            HttpContext.Application["GrupniTreninzi"] = grupniTreninzi;
            return RedirectToAction("Povratak", "Home");
        }
        public ActionResult PrethodniTreninzi()
        {
            Dictionary<string, Posetilac> posetioci = (Dictionary<string, Posetilac>)HttpContext.Application["Posetioci"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["Korisnik"];
            Posetilac posetilac = new Posetilac();
            List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();
            foreach(string korisnickoIme in posetioci.Keys)
            {
                if (korisnickoIme == korisnik.KorisnickoIme)
                {
                    posetilac = posetioci[korisnickoIme];
                    break;
                }
            }
            foreach(var grupniTrening in posetilac.GrupniTreninzi)
            {
                if(grupniTrening.DatumIVremeTreninga < DateTime.Now && grupniTrening.Obrisan == false)
                {
                    grupniTreninzi.Add(grupniTrening);
                }
            }
            HttpContext.Application["PretragaTreninzi"] = grupniTreninzi;
            HttpContext.Application["SortiranjeTreninzi"] = grupniTreninzi;
            return View();
        }
        [HttpPost]
        public ActionResult Sortiranje(string parametar, string vrednost)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["SortiranjeTreninzi"];
            IEnumerable<GrupniTrening> kastovanje = new List<GrupniTrening>();
            if (parametar == "naziv")
            {
                if (vrednost == "rastuce")
                {
                    kastovanje = grupniTreninzi.OrderBy(x => x.Naziv);
                    HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    return View("PrethodniTreninzi");
                }
                else if (vrednost == "opadajuce")
                {
                    kastovanje = grupniTreninzi.OrderByDescending(x => x.Naziv);
                    HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    return View("PrethodniTreninzi");
                }
            }
            else if (parametar == "tipTreninga")
            {
                if (vrednost == "rastuce")
                {
                    kastovanje = grupniTreninzi.OrderBy(x => x.TipTreninga);
                    HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    return View("PrethodniTreninzi");
                }
                else if (vrednost == "opadajuce")
                {
                    kastovanje = grupniTreninzi.OrderByDescending(x => x.TipTreninga);
                    HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    return View("PrethodniTreninzi");
                }
            }
            else if (parametar == "datumIVremeOdrzavanja")
            {
                if (vrednost == "rastuce")
                {
                    kastovanje = grupniTreninzi.OrderBy(x => x.DatumIVremeTreninga);
                    HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    return View("PrethodniTreninzi");
                }
                else if (vrednost == "opadajuce")
                {
                    kastovanje = grupniTreninzi.OrderByDescending(x => x.DatumIVremeTreninga);
                    HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    return View("PrethodniTreninzi");
                }
            }
            return View("PrethodniTreninzi");
        }
        [HttpPost]
        public ActionResult Pretraga(string naziv, string tipTreninga, string fitnesCentar)
        {
            /*List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["PretragaTreninzi"];
            IEnumerable<GrupniTrening> kastovanje = new List<GrupniTrening>();
            if (naziv != "")
            {
                List<GrupniTrening> gt = new List<GrupniTrening>();
                foreach (var g in kastovanje)
                {
                    if (g.Naziv.Contains(naziv))
                    {
                        gt.Add(g);
                        HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    }
                    kastovanje = gt;
                    return View("PrethodniTreninzi");
                }
            }
            if (tipTreninga != "")
            {
                List<GrupniTrening> gt = new List<GrupniTrening>();
                foreach (var g in kastovanje)
                {
                    if (g.TipTreninga.ToString().Equals(tipTreninga))
                    {
                        gt.Add(g);
                        HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    }
                    kastovanje = gt;
                    return View("PrethodniTreninzi");
                }
            }
            if (fitnesCentar != "")
            {
                List<GrupniTrening> gt = new List<GrupniTrening>();
                foreach (var g in kastovanje)
                {
                    if (g.FitnesCentar.Naziv.Equals(fitnesCentar))
                    {
                         gt.Add(g);
                        HttpContext.Application["PretragaTreninzi"] = kastovanje;
                    }
                    kastovanje = gt;
                    return View("PrethodniTreninzi");
                }
            }         
            HttpContext.Application["PretragaTreninzi"] = kastovanje;
            if (naziv == "" && tipTreninga == "" && fitnesCentar == "")
            {
                return RedirectToAction("PrethodniTreninzi", "Posetilac");
            }
            return View("PrethodniTreninzi");*/
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["PretragaTreninzi"];
            if (naziv != "")
            {
                grupniTreninzi = NazivFilter(grupniTreninzi, naziv);
            }
            if (tipTreninga != "")
            {
                grupniTreninzi = TipTreninga(grupniTreninzi, tipTreninga);
            }
            if (fitnesCentar != "")
            {
                grupniTreninzi = NazivCentra(grupniTreninzi, fitnesCentar);
            }
            HttpContext.Application["PretragaTreninzi"] = grupniTreninzi;

            if (naziv == "" && fitnesCentar == "" && tipTreninga == "")
                return RedirectToAction("PrethodniTreninzi", "Posetilac");
            return View("PrethodniTreninzi");
        }
        public List<GrupniTrening> NazivFilter(List<GrupniTrening> grupniTreninzi, string naziv)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in grupniTreninzi)
            {
                if (f.Naziv.Contains(naziv))
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }

        public List<GrupniTrening> NazivCentra(List<GrupniTrening> grupniTreninzi, string naziv)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in grupniTreninzi)
            {
                if (f.FitnesCentar.Naziv.Contains(naziv))
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }

        public List<GrupniTrening> TipTreninga(List<GrupniTrening> grupniTreninzi, string tip)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in grupniTreninzi)
            {
                if (f.TipTreninga.ToString() == tip)
                    tempTreninzi.Add(f);
            }
            return tempTreninzi;
        }
        public ActionResult UnosKomentara()
        {
            return View();
        }
    }
}