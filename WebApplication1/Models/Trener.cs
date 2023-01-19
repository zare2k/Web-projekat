using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Trener : Korisnik
    {
        private FitnesCentar fitnesCentar = new FitnesCentar();
        private List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();
        private bool blokiran;
        public FitnesCentar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public List<GrupniTrening> GrupniTreninzi { get => grupniTreninzi; set => grupniTreninzi = value; }
        public bool Blokiran { get => blokiran; set => blokiran = value; }

        public Trener() : base()
        {
            this.Uloga = Uloga.TRENER;
        }

        public Trener(string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja, FitnesCentar fitnesCentar) : base(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja)
        {
            this.FitnesCentar = fitnesCentar;
            this.Uloga = Uloga.TRENER;
            Blokiran = false;
        }
    }
}