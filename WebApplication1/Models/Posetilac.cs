using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Posetilac : Korisnik
    {
        private List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();

        public List<GrupniTrening> GrupniTreninzi { get => grupniTreninzi; set => grupniTreninzi = value; }
        public Posetilac() : base()
        {
            this.Uloga = Uloga.POSETILAC;
        }

        public Posetilac(string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja) : base(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja)
        {
            this.Uloga = Uloga.POSETILAC;
        }
    }
}