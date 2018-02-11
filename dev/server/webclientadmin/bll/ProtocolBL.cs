using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Server.WebClientAdmin.DAL;
using Sigesoft.Common;

namespace Sigesoft.Server.WebClientAdmin.BLL
{
    public class ProtocolBL
    {
        public  List<KeyValueDTO> GetProtocolBySystemUser(ref OperationResult pobjOperationResult, int pintSystemUserId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocolsystemuser
                            join b in dbContext.protocol on a.v_ProtocolId equals b.v_ProtocolId
                            where (a.i_SystemUserId == pintSystemUserId) &&
                            (a.i_IsDeleted == 0)
                            select new KeyValueDTO {
                                Id = b.v_ProtocolId,
                                Value1 = b.v_Name
                            }).Distinct().ToList();
             
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public string GetNameOrganizationCustomer(string pstrProtocolId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocol
                            join b in dbContext.organization on a.v_CustomerOrganizationId equals b.v_OrganizationId
                             where (a.v_ProtocolId == pstrProtocolId) &&
                            (a.i_IsDeleted == 0)
                            select new KeyValueDTO {
                                Id = b.v_OrganizationId,
                                Value1 = b.v_Name
                            }).FirstOrDefault();

                return query.Value1; 

            }
            catch (Exception)
            {
                return null;
            }
        }

        public OrganizationShortList GetOrganizationCustumerByProtocolSystemUser(ref OperationResult pobjOperationResult, int pintSystemUserId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocolsystemuser
                             join b in dbContext.protocol on a.v_ProtocolId equals b.v_ProtocolId
                             join c in dbContext.organization on b.v_CustomerOrganizationId equals  c.v_OrganizationId
                             where (a.i_SystemUserId == pintSystemUserId) &&
                             (a.i_IsDeleted == 0)
                             select new OrganizationShortList
                             {
                               CustomerOrganizationName = c.v_Name,
                               IdEmpresaCliente = c.v_OrganizationId
                             }).FirstOrDefault();

                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

    }
}
