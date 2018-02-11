using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportOftalmologia
    {
        public string v_ComponentId { get; set; }
        public string v_ServiceId { get; set; }
        public string ServicioId { get; set; }
        public string NombrePaciente { get; set; }
        public int Edad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaServicio { get; set; }
        public string EmprresaTrabajadora { get; set; }
        public string PuestoTrabajo { get; set; }
        public string UsoCorrectoresSi { get; set; }
        public string UsoCorrectoresNo { get; set; }
        public string UltimaRefraccion { get; set; }

        public string Catarata { get; set; }
        public string Hipertension { get; set; }
        public string Glaucoma { get; set; }
        public string DiabetesMellitus { get; set; }
        public string TraumatismoOcular { get; set; }
        public string Soldadura { get; set; }
        public string SustQuimica { get; set; }
        public string Ambliopia { get; set; }
        public string Otros { get; set; }

        public string SCLejosOD { get; set; }
        public string CCLejosOD { get; set; }
        public string AELejosOD { get; set; }

        public string SCLejosOI { get; set; }
        public string CCLejosOI { get; set; }
        public string AELejosOI { get; set; }

        public string SCCercaOD { get; set; }
        public string CCCercasOD { get; set; }
        public string AECercaOD { get; set; }

        public string SCCercaOI { get; set; }
        public string CCCercasOI { get; set; }
        public string AECercaOI { get; set; }


        public string MaculaOD { get; set; }
        public string MaculaOI { get; set; }

        public string RetinaOD { get; set; }
        public string RetinaOI { get; set; }

        public string NervioOpticoOD { get; set; }
        public string NervioOpticoOI { get; set; }

        public string ParpadoOD { get; set; }
        public string ParpadoOI { get; set; }

        public string ConjuntivaOD { get; set; }
        public string ConjuntivaOI { get; set; }

        public string CorneaOD { get; set; }
        public string CorneaOI { get; set; }

        public string CristalinoOD { get; set; }
        public string CristalinoOI { get; set; }

        public string IrisOD { get; set; }
        public string IrisOI { get; set; }

        public string MovOcularesOD { get; set; }
        public string MovOcularesOI { get; set; }

        public string ConfrontacionODCompleto { get; set; }
        public string ConfrontacionODRestringido { get; set; }

        public string ConfrontacionOICompleto { get; set; }
        public string ConfrontacionOIRestringido { get; set; }

        public string TestIshiharaNormal { get; set; }
        public string TestIshiharaAnormal { get; set; }

        public string TestEstereopsisTiempo { get; set; }
        public string TestEstereopsisNormal { get; set; }
        public string TestEstereopsisAnormal { get; set; }


        public string PresionIntraocularOD { get; set; }
        public string PresionIntraocularOI { get; set; }


        public string Hallazgos { get; set; }

        public byte[] FirmaDoctor { get; set; }
        public byte[] FirmaTecnologo { get; set; }
        public string NombreTecnologo { get; set; }

        public string Diagnosticos { get; set; }
        public string Recomendaciones { get; set; }

        public string Discromatopsia { get; set; }

        public string v_ServiceComponentId { get; set; }
        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

    }
}
