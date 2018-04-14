using System;

namespace Sigesoft.Node.WinClient.BE.Custom
{
    public class ReporteCuestionarioNordico
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

        public string Cbo1Cuello { get; set; }// = "N009-CSN00000001";
        public string Cbo1Hombro { get; set; }// = "N009-CSN00000002";
        public string Cbo1Dorsal { get; set; }// = "N009-CSN00000003";
        public string Cbo1Codo { get; set; }// = "N009-CSN00000004";
        public string Cbo1Mano { get; set; }// = "N009-CSN00000005";

        public string Cbo1HombroDir { get; set; }// = "N009-CSN00000006";
        public string Cbo1CodoDir { get; set; }// = "N009-CSN00000007";
        public string Cbo1ManoDir { get; set; }// = "N009-CSN00000008";

        public string Txt2Cuello { get; set; }// = "N009-CSN00000009";
        public string Txt2Hombro { get; set; }// = "N009-CSN00000010";
        public string Txt2Dorsal { get; set; }// = "N009-CSN0000001";
        public string Txt2Codo { get; set; }// = "N009-CSN00000012";
        public string Txt2Mano { get; set; }// = "N009-CSN00000013";

        public string Cbo3Cuello { get; set; }// = "N009-CSN00000014";
        public string Cbo3Hombro { get; set; }// = "N009-CSN00000015";
        public string Cbo3Dorsal { get; set; }// = "N009-CSN00000016";
        public string Cbo3Codo { get; set; }// = "N009-CSN00000017";
        public string Cbo3Mano { get; set; }// = "N009-CSN00000018";

        public string Cbo4Cuello { get; set; }// = "N009-CSN00000019";
        public string Cbo4Hombro { get; set; }// = "N009-CSN00000020";
        public string Cbo4Dorsal { get; set; }// = "N009-CSN00000021";
        public string Cbo4Codo { get; set; }// = "N009-CSN00000022";
        public string Cbo4Mano { get; set; }// = "N009-CSN00000023";

        public string Cbo5Cuello { get; set; }// = "N009-CSN00000024";
        public string Cbo5Hombro { get; set; }// = "N009-CSN00000025";
        public string Cbo5Dorsal { get; set; }// = "N009-CSN00000026";
        public string Cbo5Codo { get; set; }// = "N009-CSN00000027";
        public string Cbo5Mano { get; set; }// = "N009-CSN00000028";

        public string Cbo6Cuello { get; set; }// = "N009-CSN00000029";
        public string Cbo6Hombro { get; set; }// = "N009-CSN00000030";
        public string Cbo6Dorsal { get; set; }// = "N009-CSN00000031";
        public string Cbo6Codo { get; set; }// = "N009-CSN00000032";
        public string Cbo6Mano { get; set; }// = "N009-CSN00000033";

        public string Cbo7Cuello { get; set; }// = "N009-CSN00000034";
        public string Cbo7Hombro { get; set; }// = "N009-CSN00000035";
        public string Cbo7Dorsal { get; set; }// = "N009-CSN00000036";
        public string Cbo7Codo { get; set; }// = "N009-CSN00000037";
        public string Cbo7Mano { get; set; }// = "N009-CSN00000038";

        public string Cbo8Cuello { get; set; }// = "N009-CSN00000039";
        public string Cbo8Hombro { get; set; }// = "N009-CSN00000040";
        public string Cbo8Dorsal { get; set; }// = "N009-CSN00000041";
        public string Cbo8Codo { get; set; }// = "N009-CSN00000042";
        public string Cbo8Mano { get; set; }// = "N009-CSN00000043";

        public string Cbo9Cuello { get; set; }// = "N009-CSN00000044";
        public string Cbo9Hombro { get; set; }// = "N009-CSN00000045";
        public string Cbo9Dorsal { get; set; }// = "N009-CSN00000046";
        public string Cbo9Codo { get; set; }// = "N009-CSN00000047";
        public string Cbo9Mano { get; set; }// = "N009-CSN00000048";

        public string Cbo10Cuello { get; set; }// = "N009-CSN00000049";
        public string Cbo10Hombro { get; set; }// = "N009-CSN00000050";
        public string Cbo10Dorsal { get; set; }// = "N009-CSN00000051";
        public string Cbo10Codo { get; set; }// = "N009-CSN00000052";
        public string Cbo10Mano { get; set; }// = "N009-CSN00000053";

        public string Txt11Cuello { get; set; }// = "N009-CSN00000054";
        public string Txt11Hombro { get; set; }// = "N009-CSN00000055";
        public string Txt11Dorsal { get; set; }// = "N009-CSN0000056";
        public string Txt11Codo { get; set; }// = "N009-CSN00000057";
        public string Txt11Mano { get; set; }// = "N009-CSN00000058";
    }
}
