using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportTestChoferes
    {
        public string v_ServiceId { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Edad { get; set; }
        public string DNI { get; set; }
        public DateTime? FechaServicio { get; set; }
        public string Empresa { get; set; }
        public string P_1 { get; set; }
        public string P_2 { get; set; }
        public string P_3 { get; set; }
        public string P_4 { get; set; }
        public string P_5 { get; set; }
        public string P_6 { get; set; }
        public string P_7 { get; set; }
        public string Puntaje { get; set; }
        public byte[] Logo { get; set; }

        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }
    }
}
