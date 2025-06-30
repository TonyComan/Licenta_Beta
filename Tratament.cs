using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalProApp
{
    public class Tratament
    {
        public int id { get; set; }

        public int pacient_id { get; set; }
        public string nume_pacient { get; set; }

        public int medic_id { get; set; }
        public string nume_medic { get; set; }

        public int serviciu_id { get; set; }
        public string nume_serviciu { get; set; }

        public string descriere { get; set; }
        public DateTime data_tratament { get; set; }
        public string observatii { get; set; }
    }
}
