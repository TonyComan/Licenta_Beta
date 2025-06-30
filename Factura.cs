using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DentalProApp
{
    public class Factura
    {
        public int id { get; set; }

        public int tratament_id { get; set; }

        public string nume_pacient { get; set; }
        public string nume_medic { get; set; }
        public string nume_serviciu { get; set; }

        public DateTime data_emitere { get; set; }
        public decimal total { get; set; }
        public string metoda_plata { get; set; }
    }
}
