using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Korisnik
    {
        private string korisnickoIme;
        private string lozinka;
        private string ime;
        private string prezime;
        private Pol pol;
        private string email;
        private string datumRodjenja;
        private Uloga uloga;

        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public Pol Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public string DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public Uloga Uloga { get => uloga; set => uloga = value; }

        public Korisnik() { }
        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
        }
    }
}