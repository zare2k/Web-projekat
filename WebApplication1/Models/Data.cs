using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using static WebApplication1.Models.GrupniTrening;

namespace WebApplication1.Models
{
    public class Data
    {
        public static void UpisivanjePosetilaca(Posetilac posetilac)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Posetioci/");
            string obj = JsonConvert.SerializeObject(posetilac, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string fullPath = String.Format($"{path}{posetilac.KorisnickoIme}.json");
            File.WriteAllText(fullPath, obj);
        }
        public static Dictionary<string, Posetilac> CitanjePosetilaca()
        {
            Dictionary<string, Posetilac> posetiociRecnik = new Dictionary<string, Posetilac>();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Posetioci/");
            string[] posetioci = Directory.GetFiles(path);
            foreach (string str in posetioci)
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    string obj = reader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Posetilac posetilac = js.Deserialize<Posetilac>(obj);
                    posetiociRecnik.Add(posetilac.KorisnickoIme, posetilac);
                }
            }
            return posetiociRecnik;
        }
        public static void UpisivanjeTrenera(Trener trener)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Treneri/");
            string obj = JsonConvert.SerializeObject(trener, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string fullPath = String.Format($"{path}{trener.KorisnickoIme}.json");
            File.WriteAllText(fullPath, obj);
        }
        public static Dictionary<string, Trener> CitanjeTrenera()
        {
            Dictionary<string, Trener> treneriRecnik = new Dictionary<string, Trener>();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Treneri/");
            string[] treneri = Directory.GetFiles(path);
            foreach (string str in treneri)
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    string obj = reader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Trener trener = js.Deserialize<Trener>(obj);
                    treneriRecnik.Add(trener.KorisnickoIme, trener);
                }
            }
            return treneriRecnik;
        }
        public static void UpisivanjeGrupnihTreninga(GrupniTrening grupniTrening)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/GrupniTreninzi/");
            string obj = JsonConvert.SerializeObject(grupniTrening, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string fullPath = String.Format($"{path}{grupniTrening.TreningId}.json");
            File.WriteAllText(fullPath, obj);
        }
        public static List<GrupniTrening> CitanjeGrupnihTreninga()
        {
            List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/GrupniTreninzi/");
            string[] treninzi = Directory.GetFiles(path);
            foreach (string str in treninzi)
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    string obj = reader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    GrupniTrening grupniTrening = js.Deserialize<GrupniTrening>(obj);
                    grupniTreninzi.Add(grupniTrening);
                }
            }
            return grupniTreninzi;
        }
        public static void UpisivanjeKomentara(Komentar komentar)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Komentari/");
            string obj = JsonConvert.SerializeObject(komentar, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string fullPath = String.Format($"{path}{komentar.KomentarId}.json");
            File.WriteAllText(fullPath, obj);
        }
        public static List<Komentar> CitanjeKomentara()
        {
            List<Komentar> komentariLista = new List<Komentar>();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Komentari/");
            string[] komentari = Directory.GetFiles(path);
            foreach (string str in komentari)
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    string obj = reader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Komentar komentar = js.Deserialize<Komentar>(obj);
                    komentariLista.Add(komentar);
                }
            }
            return komentariLista;
        }
        public static void UpisivanjeVlasnika(Vlasnik vlasnik)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Vlasnici/");
            string obj = JsonConvert.SerializeObject(vlasnik, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string fullPath = String.Format($"{path}{vlasnik.KorisnickoIme}.json");
            File.WriteAllText(fullPath, obj);
        }
        public static Dictionary<string, Vlasnik> CitanjeVlasnika()
        {
            Dictionary<string, Vlasnik> vlasniciRecnik = new Dictionary<string, Vlasnik>();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Vlasnici/");
            string[] vlasnici = Directory.GetFiles(path);
            foreach (string str in vlasnici)
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    string obj = reader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Vlasnik vlasnik = js.Deserialize<Vlasnik>(obj);
                    vlasniciRecnik.Add(vlasnik.KorisnickoIme, vlasnik);
                }
            }
            return vlasniciRecnik;
        }
        public static List<FitnesCentar> CitanjeFitnesCentara(Dictionary<string, Vlasnik> vlasnici)
        {
            List<FitnesCentar> fitnesCentri = new List<FitnesCentar>();
            foreach (Vlasnik v in vlasnici.Values)
            {
                foreach (var fitnesCentar in v.FitnesCentri)
                    fitnesCentri.Add(fitnesCentar);
            }
            return fitnesCentri;
        }
        public static void PocetnoStanje()
        {
            Vlasnik vlasnik1 = new Vlasnik("Zare", "123456", "Aleksandar", "Zaric", Pol.MUSKI, "zare@gmail.com", "05.12.2000");
            Vlasnik vlasnik2 = new Vlasnik("Mica", "234567", "Milovan", "Aleksic", Pol.MUSKI, "mica@gmail.com", "15.07.2000");
            Vlasnik vlasnik3 = new Vlasnik("Aki", "345678", "Aleksandra", "Gajic", Pol.ZENSKI, "aki@gmail.com", "05.12.2000");
            FitnesCentar centar1 = new FitnesCentar("Energym", "Kosovska", 2010, vlasnik1, 2500, 25000, 300, 500, 1200);
            FitnesCentar centar2 = new FitnesCentar("Green life fitness", "Bulevar Cara Lazara", 2014, vlasnik2, 4000, 40000, 500, 800, 1800);
            FitnesCentar centar3 = new FitnesCentar("Flexgym", "Sutjeska", 2016, vlasnik3, 3000, 30000, 400, 600, 1500);
            centar1.Vlasnik = vlasnik1;
            centar2.Vlasnik = vlasnik2;
            centar3.Vlasnik = vlasnik3;
            vlasnik1.FitnesCentri.Add(centar1);
            vlasnik2.FitnesCentri.Add(centar2);
            vlasnik3.FitnesCentri.Add(centar3);
            UpisivanjeVlasnika(vlasnik1);
            UpisivanjeVlasnika(vlasnik2);
            UpisivanjeVlasnika(vlasnik3);

            DateTime datum1 = DateTime.Now.AddDays(15);
            GrupniTrening trening1 = new GrupniTrening("HIIT 1", TipTreninga.BODY_PUMP, centar1, 45, datum1, 20);
            DateTime datum2 = DateTime.Now.AddDays(25);
            GrupniTrening trening2 = new GrupniTrening("BW trening", TipTreninga.BODY_PUMP, centar1, 80, datum2, 30);
            DateTime datum3 = DateTime.Now.AddDays(30);
            GrupniTrening trening3 = new GrupniTrening("Hot yoga", TipTreninga.YOGA, centar2, 60, datum3, 10);
            DateTime datum4 = DateTime.Now.AddDays(25);
            GrupniTrening trening4 = new GrupniTrening("HIIT 2", TipTreninga.BODY_PUMP, centar2, 50, datum4, 25);
            DateTime datum5 = DateTime.Now.AddDays(20);
            GrupniTrening trening5 = new GrupniTrening("Aerobic", TipTreninga.LES_MILLS_TONE, centar3, 55, datum5, 5);
            DateTime datum6 = DateTime.Now.AddMinutes(5);
            GrupniTrening trening6 = new GrupniTrening("HIIT 3", TipTreninga.BODY_PUMP, centar1, 35, datum6, 3);
            DateTime datum7 = DateTime.Now.AddMinutes(5);
            GrupniTrening trening7 = new GrupniTrening("Weight lifting", TipTreninga.LES_MILLS_TONE, centar2, 75, datum7, 3);
            UpisivanjeGrupnihTreninga(trening1);
            UpisivanjeGrupnihTreninga(trening2);
            UpisivanjeGrupnihTreninga(trening3);
            UpisivanjeGrupnihTreninga(trening4);
            UpisivanjeGrupnihTreninga(trening5);
            UpisivanjeGrupnihTreninga(trening6);
            UpisivanjeGrupnihTreninga(trening7);

            Posetilac posetilac1 = new Posetilac("Pera", "123456", "Pera", "Peric", Pol.MUSKI, "pera@gmail.com", "01.01.2001");
            Posetilac posetilac2 = new Posetilac("Zika", "234567", "Zika", "Zikic", Pol.MUSKI, "zika@gmail.com", "02.02.2002");
            UpisivanjePosetilaca(posetilac1);
            UpisivanjePosetilaca(posetilac2);

            Trener trener1 = new Trener("Kum", "345678", "Masan", "Bulajic", Pol.MUSKI, "kum@gmail.com", "04.12.2000", centar1);
            Trener trener2 = new Trener("Musa", "456789", "Andrija", "Music", Pol.MUSKI, "musa@gmail.com", "07.03.2000", centar2);
            Trener trener3 = new Trener("Sara", "567890", "Sara", "Todorovic", Pol.ZENSKI, "sara@gmail.com", "12.10.2000", centar3);
            trener1.GrupniTreninzi.Add(trening1);
            trener1.GrupniTreninzi.Add(trening2);
            trener2.GrupniTreninzi.Add(trening3);
            trener2.GrupniTreninzi.Add(trening4);
            trener3.GrupniTreninzi.Add(trening5);
            trener1.GrupniTreninzi.Add(trening6);
            UpisivanjeTrenera(trener1);
            UpisivanjeTrenera(trener2);
            UpisivanjeTrenera(trener3);

            Komentar komentar1 = new Komentar(posetilac1.KorisnickoIme, centar1.Naziv, "Mnogo dobra teretana.", 5);
            Komentar komentar2 = new Komentar(posetilac2.KorisnickoIme, centar2.Naziv, "Poradite na higijeni.", 3);
            Komentar komentar3 = new Komentar(posetilac2.KorisnickoIme, centar3.Naziv, "Najbolji odnos cene i kvaliteta.", 4);
            UpisivanjeKomentara(komentar1);
            UpisivanjeKomentara(komentar2);
            UpisivanjeKomentara(komentar3);
        }
        public static void AzuriranjePosetioca(Posetilac posetilac)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Posetioci/");
            string fullPath = String.Format($"{path}{posetilac.KorisnickoIme}.json");
            File.Delete(fullPath);
            string obj = JsonConvert.SerializeObject(posetilac, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(fullPath, obj);
        }
        public static void AzuriranjeTrenera(Trener trener)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Treneri/");
            string fullPath = String.Format($"{path}{trener.KorisnickoIme}.json");
            File.Delete(fullPath);
            string obj = JsonConvert.SerializeObject(trener, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(fullPath, obj);
        }
        public static void AzuriranjeVlasnika(Vlasnik vlasnik)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Vlasnici/");
            string fullPath = String.Format($"{path}{vlasnik.KorisnickoIme}.json");
            File.Delete(fullPath);
            string obj = JsonConvert.SerializeObject(vlasnik, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(fullPath, obj);
        }
        public static void AzuriranjeGrupnihTreninga(GrupniTrening grupniTrening)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/GrupniTreninzi/");
            string fullPath = String.Format($"{path}{grupniTrening.TreningId}.json");
            File.Delete(fullPath);
            string obj = JsonConvert.SerializeObject(grupniTrening, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(fullPath, obj);
        }
    }
}