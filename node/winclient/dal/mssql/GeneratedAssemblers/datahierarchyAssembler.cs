//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.1 (entitiestodtos.codeplex.com).
//     Timestamp: 2016/12/21 - 09:57:42
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Node.WinClient.DAL;

namespace Sigesoft.Node.WinClient.BE
{

    /// <summary>
    /// Assembler for <see cref="datahierarchy"/> and <see cref="datahierarchyDto"/>.
    /// </summary>
    public static partial class datahierarchyAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="datahierarchyDto"/> converted from <see cref="datahierarchy"/>.</param>
        static partial void OnDTO(this datahierarchy entity, datahierarchyDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="datahierarchy"/> converted from <see cref="datahierarchyDto"/>.</param>
        static partial void OnEntity(this datahierarchyDto dto, datahierarchy entity);

        /// <summary>
        /// Converts this instance of <see cref="datahierarchyDto"/> to an instance of <see cref="datahierarchy"/>.
        /// </summary>
        /// <param name="dto"><see cref="datahierarchyDto"/> to convert.</param>
        public static datahierarchy ToEntity(this datahierarchyDto dto)
        {
            if (dto == null) return null;

            var entity = new datahierarchy();

            entity.i_GroupId = dto.i_GroupId;
            entity.i_ItemId = dto.i_ItemId;
            entity.v_Value1 = dto.v_Value1;
            entity.v_Value2 = dto.v_Value2;
            entity.v_Field = dto.v_Field;
            entity.i_ParentItemId = dto.i_ParentItemId;
            entity.i_Sort = dto.i_Sort;
            entity.i_IsDeleted = dto.i_IsDeleted;
            entity.i_InsertUserId = dto.i_InsertUserId;
            entity.d_InsertDate = dto.d_InsertDate;
            entity.i_UpdateUserId = dto.i_UpdateUserId;
            entity.d_UpdateDate = dto.d_UpdateDate;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="datahierarchy"/> to an instance of <see cref="datahierarchyDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="datahierarchy"/> to convert.</param>
        public static datahierarchyDto ToDTO(this datahierarchy entity)
        {
            if (entity == null) return null;

            var dto = new datahierarchyDto();

            dto.i_GroupId = entity.i_GroupId;
            dto.i_ItemId = entity.i_ItemId;
            dto.v_Value1 = entity.v_Value1;
            dto.v_Value2 = entity.v_Value2;
            dto.v_Field = entity.v_Field;
            dto.i_ParentItemId = entity.i_ParentItemId;
            dto.i_Sort = entity.i_Sort;
            dto.i_IsDeleted = entity.i_IsDeleted;
            dto.i_InsertUserId = entity.i_InsertUserId;
            dto.d_InsertDate = entity.d_InsertDate;
            dto.i_UpdateUserId = entity.i_UpdateUserId;
            dto.d_UpdateDate = entity.d_UpdateDate;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="datahierarchyDto"/> to an instance of <see cref="datahierarchy"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<datahierarchy> ToEntities(this IEnumerable<datahierarchyDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="datahierarchy"/> to an instance of <see cref="datahierarchyDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<datahierarchyDto> ToDTOs(this IEnumerable<datahierarchy> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
