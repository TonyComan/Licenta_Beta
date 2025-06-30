using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalProApp
{
    public class Programare
    {
        public int id { get; set; }

        public int pacient_id { get; set; }
        public string nume_pacient { get; set; }

        public int medic_id { get; set; }
        public string nume_medic { get; set; }

        public DateTime data_programare { get; set; }
        public TimeSpan ora_programare { get; set; }

        public string status { get; set; }
    }
}
