//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.1 (entitiestodtos.codeplex.com).
//     Timestamp: 2016/12/21 - 09:57:44
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
    /// Assembler for <see cref="movementdetail"/> and <see cref="movementdetailDto"/>.
    /// </summary>
    public static partial class movementdetailAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="movementdetailDto"/> converted from <see cref="movementdetail"/>.</param>
        static partial void OnDTO(this movementdetail entity, movementdetailDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="movementdetail"/> converted from <see cref="movementdetailDto"/>.</param>
        static partial void OnEntity(this movementdetailDto dto, movementdetail entity);

        /// <summary>
        /// Converts this instance of <see cref="movementdetailDto"/> to an instance of <see cref="movementdetail"/>.
        /// </summary>
        /// <param name="dto"><see cref="movementdetailDto"/> to convert.</param>
        public static movementdetail ToEntity(this movementdetailDto dto)
        {
            if (dto == null) return null;

            var entity = new movementdetail();

            entity.v_MovementId = dto.v_MovementId;
            entity.v_ProductId = dto.v_ProductId;
            entity.v_WarehouseId = dto.v_WarehouseId;
            entity.r_StockMax = dto.r_StockMax;
            entity.r_StockMin = dto.r_StockMin;
            entity.i_MovementTypeId = dto.i_MovementTypeId;
            entity.r_Quantity = dto.r_Quantity;
            entity.r_Price = dto.r_Price;
            entity.r_SubTotal = dto.r_SubTotal;
            entity.d_UpdateDate = dto.d_UpdateDate;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="movementdetail"/> to an instance of <see cref="movementdetailDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="movementdetail"/> to convert.</param>
        public static movementdetailDto ToDTO(this movementdetail entity)
        {
            if (entity == null) return null;

            var dto = new movementdetailDto();

            dto.v_MovementId = entity.v_MovementId;
            dto.v_ProductId = entity.v_ProductId;
            dto.v_WarehouseId = entity.v_WarehouseId;
            dto.r_StockMax = entity.r_StockMax;
            dto.r_StockMin = entity.r_StockMin;
            dto.i_MovementTypeId = entity.i_MovementTypeId;
            dto.r_Quantity = entity.r_Quantity;
            dto.r_Price = entity.r_Price;
            dto.r_SubTotal = entity.r_SubTotal;
            dto.d_UpdateDate = entity.d_UpdateDate;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="movementdetailDto"/> to an instance of <see cref="movementdetail"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<movementdetail> ToEntities(this IEnumerable<movementdetailDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="movementdetail"/> to an instance of <see cref="movementdetailDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<movementdetailDto> ToDTOs(this IEnumerable<movementdetail> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
