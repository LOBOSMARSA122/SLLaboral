using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE.Custom
{
   public class ReporteApnea
    {

        public string ServiceId { get; set; }
        public string ServiceComponentId { get; set; }
        public DateTime? FechaServicio { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string EmpresaCliente { get; set; }
        public string EmpresaTrabajo { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string Puesto { get; set; }
        public int GeneroId { get; set; }
        public string Genero { get; set; }
        public string LugarNacimiento { get; set; }
        public string LugarProcedencia { get; set; }
        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaUsuarioGraba { get; set; }
        public byte[] FirmaMedicina { get; set; }

        public string TrabajaDeNoche1 { get; set; }
        public string DiasDeTrabajo2 { get; set; }
        public string DiasDeDescanso3 { get; set; }
        public string Hta1 { get; set; }
        public string MedicacionRiesgo2 { get; set; }
        public string PolisomnografiaRealizada3 { get; set; }
        public string RptaSiFecha4 { get; set; }
        public string AntChoqueEnMina5 { get; set; }
        public string AntChoqueFuera6 { get; set; }
        public string SentadoLeyendo1 { get; set; }
        public string MirandoLaTelevision2 { get; set; }
        public string SentadoEnUnLugarPublico3 { get; set; }
        public string ComoPasajeroDeAutoMicro4 { get; set; }
        public string RecostadoEnLaTarde5 { get; set; }
        public string SentadoYHablando6 { get; set; }
        public string SentadoDepuesDeAlmorzar7 { get; set; }
        public string ManejandoElAutoCuando8 { get; set; }
        public string PuntajeTotal { get; set; }
        public string RoncaAiDormir1 { get; set; }
        public string HaceRuidosAlRespirar2 { get; set; }
        public string DejaDeRespirarDuerme3 { get; set; }
        public string ComparadoConSusCompa4 { get; set; }
        public string AccidenteCabeceo5 { get; set; }
        public string AccidenteFallaHumana6 { get; set; }
        public string EstaRecibiendoTratamiento7 { get; set; }
        public string SeLeHaRealzadoUnaPsg8 { get; set; }
    }
}
