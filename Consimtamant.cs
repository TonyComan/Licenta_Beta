using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalProApp
{
   public class Consimtamant
    {
        public int id { get; set; }
        public int pacient_id { get; set; }
        public string nume_pacient { get; set; }
        public string cnp { get; set; }
        public string telefon { get; set; }
        public string nume_medic { get; set; }
        public string descriere { get; set; }
        public DateTime data_document { get; set; }

    }
}
