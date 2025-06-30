using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalProApp.Models
{
    public class ContractColaborare
    {
        public int id { get; set; }

        // Medic
        public string nume_medic { get; set; }

        // Tehnician dentar
        public string nume_tehnician { get; set; }
        public string email_tehnician { get; set; }
        public string telefon_tehnician { get; set; }
        public string specializare_tehnician { get; set; }

        // Durata colaborării
        public int durata_luni { get; set; }
    }
}
