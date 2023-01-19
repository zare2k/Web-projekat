using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class GrupniTrening
    {
        private string naziv;
        private TipTreninga tipTreninga;
        private FitnesCentar fitnesCentar;
        private int trajanjeTreninga;
        private DateTime datumIVremeTreninga;
        private int maksimalanBrojPosetilaca;
        private List<Posetilac> spisakPosetilaca;
        private string treningId;
        private bool obrisan;
        public string Naziv { get => naziv; set => naziv = value; }
        public TipTreninga TipTreninga { get => tipTreninga; set => tipTreninga = value; }
        public FitnesCentar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public int TrajanjeTreninga { get => trajanjeTreninga; set => trajanjeTreninga = value; }
        public DateTime DatumIVremeTreninga { get => datumIVremeTreninga; set => datumIVremeTreninga = value; }
        public int MaksimalanBrojPosetilaca { get => maksimalanBrojPosetilaca; set => maksimalanBrojPosetilaca = value; }
        public List<Posetilac> SpisakPosetilaca { get => spisakPosetilaca; set => spisakPosetilaca = value; }
        public string TreningId { get => treningId; set => treningId = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }

        public GrupniTrening()
        {
            TreningId = Guid.NewGuid().ToString();
        }

        public GrupniTrening(string naziv, TipTreninga tipTreninga, FitnesCentar fitnesCentar, int trajanjeTreninga, DateTime datumIVremeTreninga, int maksimalanBrojPosetilaca)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga;
            FitnesCentar = fitnesCentar;
            TrajanjeTreninga = trajanjeTreninga;
            DatumIVremeTreninga = datumIVremeTreninga;
            MaksimalanBrojPosetilaca = maksimalanBrojPosetilaca;
            SpisakPosetilaca = new List<Posetilac>();
            TreningId = Guid.NewGuid().ToString();
        }
    }
}