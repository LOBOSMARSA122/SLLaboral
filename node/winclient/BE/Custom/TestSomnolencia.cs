using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class TestSomnolencia
    {
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public string Empresa { get; set; }
        public string DNI { get; set; }
        public DateTime? FechaServicio { get; set; }
        public string TEST_SOMNOLENCIA_ID {get;set;}

        public string TEST_SOMNOLENCIA_SENTADO_LEYENDO {get;set;}
        public string TEST_SOMNOLENCIA_MIRANDO_TV {get;set;} 
        public string TEST_SOMNOLENCIA_SENTADO_QUIETO {get;set;}
        public string TEST_SOMNOLENCIA_VIAJANDO {get;set;} 
        public string TEST_SOMNOLENCIA_CONVERSANDO {get;set;} 
        public string TEST_SOMNOLENCIA_LUEGO_COMIDA {get;set;} 
        public string TEST_SOMNOLENCIA_CONDUCIENDO {get;set;} 
        public string TEST_SOMNOLENCIA_DESCANSAR {get;set;}
        public string TEST_SOMNOLENCIA_PUNTAJE {get;set;}

        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaProfesional { get; set; }

        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }
        public string Dxs { get; set; }
        public string Conclusiones { get; set; }
    }
}
