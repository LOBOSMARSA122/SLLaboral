//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.1 (entitiestodtos.codeplex.com).
//     Timestamp: 2015/07/02 - 17:00:24
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE
{
    [DataContract()]
    public partial class llenargrillasigesoftResultDto
    {
        [DataMember()]
        public String EmpresaCliente { get; set; }

        [DataMember()]
        public String Nombre_Componente { get; set; }

        [DataMember()]
        public String IdComponente { get; set; }

        [DataMember()]
        public String IdServicio { get; set; }

        [DataMember()]
        public Nullable<Double> Total { get; set; }

        public llenargrillasigesoftResultDto()
        {
        }

        public llenargrillasigesoftResultDto(String empresaCliente, String nombre_Componente, String idComponente, String idServicio, Nullable<Double> total)
        {
			this.EmpresaCliente = empresaCliente;
			this.Nombre_Componente = nombre_Componente;
			this.IdComponente = idComponente;
			this.IdServicio = idServicio;
			this.Total = total;
        }
    }
}