using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Vlasnik : Korisnik
    {
        private List<FitnesCentar> fitnesCentri = new List<FitnesCentar>();
        public List<FitnesCentar> FitnesCentri { get => fitnesCentri; set => fitnesCentri = value; }
        public Vlasnik() : base()
        {
            this.Uloga = Uloga.VLASNIK;
        }

        public Vlasnik(string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja) : base(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja)
        {
            this.Uloga = Uloga.VLASNIK;
        }


    }
}