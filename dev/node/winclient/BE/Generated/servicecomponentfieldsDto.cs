//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.1 (entitiestodtos.codeplex.com).
//     Timestamp: 2016/12/21 - 09:57:31
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    [DataContract()]
    public partial class servicecomponentfieldsDto
    {
        [DataMember()]
        public String v_ServiceComponentFieldsId { get; set; }

        [DataMember()]
        public String v_ServiceComponentId { get; set; }

        [DataMember()]
        public String v_ComponentId { get; set; }

        [DataMember()]
        public String v_ComponentFieldId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsDeleted { get; set; }

        [DataMember()]
        public Nullable<Int32> i_InsertUserId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_InsertDate { get; set; }

        [DataMember()]
        public Nullable<Int32> i_UpdateUserId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_UpdateDate { get; set; }

        [DataMember()]
        public List<servicecomponentfieldvaluesDto> servicecomponentfieldvalues { get; set; }

        public servicecomponentfieldsDto()
        {
        }

        public servicecomponentfieldsDto(String v_ServiceComponentFieldsId, String v_ServiceComponentId, String v_ComponentId, String v_ComponentFieldId, Nullable<Int32> i_IsDeleted, Nullable<Int32> i_InsertUserId, Nullable<DateTime> d_InsertDate, Nullable<Int32> i_UpdateUserId, Nullable<DateTime> d_UpdateDate, List<servicecomponentfieldvaluesDto> servicecomponentfieldvalues)
        {
			this.v_ServiceComponentFieldsId = v_ServiceComponentFieldsId;
			this.v_ServiceComponentId = v_ServiceComponentId;
			this.v_ComponentId = v_ComponentId;
			this.v_ComponentFieldId = v_ComponentFieldId;
			this.i_IsDeleted = i_IsDeleted;
			this.i_InsertUserId = i_InsertUserId;
			this.d_InsertDate = d_InsertDate;
			this.i_UpdateUserId = i_UpdateUserId;
			this.d_UpdateDate = d_UpdateDate;
			this.servicecomponentfieldvalues = servicecomponentfieldvalues;
        }
    }
}
