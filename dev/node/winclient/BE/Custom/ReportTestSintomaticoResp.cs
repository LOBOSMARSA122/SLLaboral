using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
   public  class ReportTestSintomaticoResp
    {
       public string Nombres { get; set; }
       public string Apellidos { get; set; }
       public string DNI { get; set; }
       public DateTime FechaNacimiento { get; set; }
       public int? Edad { get; set; }
       public int? Genero { get; set; }
       public string Direccion { get; set; }

       public byte[] Firma { get; set; }
       public byte[] Huella { get; set; }




       public string ApeMedico { get; set; }
       public string NomMedico { get; set; }
       public string DireMedico { get; set; }
       public string CMP { get; set; }
       public DateTime FechaServicio { get; set; }

       public byte[] FirmaMedico { get; set; }
       public byte[] SelloMedico { get; set; }




       public string TEST_SINTOMATICO_P1 { get; set; }
       public string TEST_SINTOMATICO_P2 { get; set; }
       public string TEST_SINTOMATICO_P3 { get; set; }
       public string TEST_SINTOMATICO_P4 { get; set; }
       public string TEST_SINTOMATICO_P5 { get; set; }
       public string TEST_SINTOMATICO_P6 { get; set; }
       public string TEST_SINTOMATICO_P7 { get; set; }
       public string TEST_SINTOMATICO_P8 { get; set; }

       public string TEST_SINTOMATICO_Obs { get; set; }

       public string TEST_SINTOMATICO_P9 { get; set; }
       public string TEST_SINTOMATICO_EL_LA { get; set; }
       public string TEST_SINTOMATICO_REQ_EST_AMP { get; set; }
       public string TEST_SINTOMATICO_BK_ESPUTO { get; set; }
       public string TEST_SINTOMATICO_BK_ESPUTO_2 { get; set; }
       
       public string TEST_SINTOMATICO_RX { get; set; }

       public string LugarProcedencia { get; set; }

       public byte[] b_Logo { get; set; }
       public string EmpresaPropietaria { get; set; }
       public string EmpresaPropietariaDireccion { get; set; }
       public string EmpresaPropietariaTelefono { get; set; }
       public string EmpresaPropietariaEmail { get; set; }

      


    }
}
