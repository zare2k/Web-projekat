using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Komentar
    {
        private string posetilac;
        private string fitnesCentar;
        private string tekstKomentara;
        private int ocena;
        private string komentarId;

        public string Posetilac { get => posetilac; set => posetilac = value; }
        public string FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public string TekstKomentara { get => tekstKomentara; set => tekstKomentara = value; }
        public int Ocena { get => ocena; set => ocena = value; }
        public string KomentarId { get => komentarId; set => komentarId = value; }
        public Komentar()
        {
        }

        public Komentar(string posetilac, string fitnesCentar, string tekstKomentara, int ocena)
        {
            Posetilac = posetilac;
            FitnesCentar = fitnesCentar;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            KomentarId = Guid.NewGuid().ToString();
        }
    }
}