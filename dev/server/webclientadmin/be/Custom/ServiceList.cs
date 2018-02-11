using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE
{
    public class ServiceList
    {
        public string v_ServiceId { get; set; }
        public string v_Trabajador { get; set; }
        public string v_IdTrabajador { get; set; }
        public DateTime d_ServiceDate { get; set; }
        public string v_AptitudeStatusName { get; set; }
        public string v_ProtocolName { get; set; }

        public int i_TypeEsoId { get; set; }
        public int i_AptitudeId { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_HCL { get; set; }
        public string EmpresaCliente { get; set; }

        public string EstadoCola { get; set; }
        public string v_CustomerOrganizationId { get; set; }

        public string v_Restricction { get; set; }
    }
}
