using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FitnesCentarController : Controller
    {
        // GET: FitnesCentar
        public ActionResult Index(string naziv)
        {
            Dictionary<string, Vlasnik> vlasnici = Data.CitanjeVlasnika();
            List<FitnesCentar> fitnesCentri = Data.CitanjeFitnesCentara(vlasnici);
            List<GrupniTrening> grupniTreninzi = Data.CitanjeGrupnihTreninga();
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["Komentari"];

            List<GrupniTrening> filterGrupniTreninzi = new List<GrupniTrening>();
            List<Komentar> filterKomentari = new List<Komentar>();
            foreach (var fc in fitnesCentri)
            {
                if (fc.Naziv == naziv)
                {
                    HttpContext.Application["Centar"] = fc;
                    foreach(Vlasnik vlasnik in vlasnici.Values)
                    {
                        if (vlasnik.FitnesCentri.Contains(fc))
                        {
                            HttpContext.Application["Vlasnik"] = vlasnik;
                        }
                    }
                    break;
                }
            }
            foreach (var grupniTrening in grupniTreninzi)
            {
                if (grupniTrening.FitnesCentar.Naziv.Equals(naziv) && grupniTrening.DatumIVremeTreninga > DateTime.Now && grupniTrening.Obrisan == false)
                {
                    filterGrupniTreninzi.Add(grupniTrening);
                }
            }
            foreach (var komentar in komentari)
            {
                if (komentar.FitnesCentar.Equals(naziv))
                {
                    filterKomentari.Add(komentar);
                }
            }
            HttpContext.Application["FilterGrupniTreninzi"] = filterGrupniTreninzi;
            HttpContext.Application["FilterKomentari"] = filterKomentari;
            return View();
        }
    }
}