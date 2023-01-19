using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FitnesCentar
    {
        private string naziv;
        private string adresa;
        private int godinaOtvaranja;
        private Vlasnik vlasnik;
        private double mesecnaCena;
        private double godisnjaCena;
        private double cenaJednogTreninga;
        private double cenaJednogGrupnogTreninga;
        private double cenaTreningaSaTrenerom;
        private bool obrisan;
        public string Naziv { get => naziv; set => naziv = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public int GodinaOtvaranja { get => godinaOtvaranja; set => godinaOtvaranja = value; }
        public Vlasnik Vlasnik { get => vlasnik; set => vlasnik = value; }
        public double MesecnaCena { get => mesecnaCena; set => mesecnaCena = value; }
        public double GodisnjaCena { get => godisnjaCena; set => godisnjaCena = value; }
        public double CenaJednogTreninga { get => cenaJednogTreninga; set => cenaJednogTreninga = value; }
        public double CenaJednogGrupnogTreninga { get => cenaJednogGrupnogTreninga; set => cenaJednogGrupnogTreninga = value; }
        public double CenaTreningaSaTrenerom { get => cenaTreningaSaTrenerom; set => cenaTreningaSaTrenerom = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }

        public FitnesCentar()
        {
        }
        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, Vlasnik vlasnik, double mesecnaCena, double godisnjaCena, double cenaJednogTreninga, double cenaJednogGrupnogTreninga, double cenaTreningaSaTrenerom)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            Vlasnik = vlasnik;
            MesecnaCena = mesecnaCena;
            GodisnjaCena = godisnjaCena;
            CenaJednogTreninga = cenaJednogTreninga;
            CenaJednogGrupnogTreninga = cenaJednogGrupnogTreninga;
            CenaTreningaSaTrenerom = cenaTreningaSaTrenerom;
        }
    }
}