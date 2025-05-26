using System;

namespace DentalProApp
{
    public class Pacient
    {
        public int id { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string cnp { get; set; }
        public DateTime data_nasterii { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string adresa { get; set; }

        public string NumeComplet => $"{nume} {prenume}";
    }
}
