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
    public class CalendarBL
    {
        public List<CalendarList> GetCalendarsPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.calendar
                            join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                            join C in dbContext.systemparameter on new { a = A.i_LineStatusId.Value, b = 120 } equals new { a = C.i_ParameterId, b = C.i_GroupId }
                            join D in dbContext.service on A.v_ServiceId equals D.v_ServiceId
                            join E in dbContext.systemparameter on new { a = A.i_ServiceTypeId.Value, b = 119 } equals new { a = E.i_ParameterId, b = E.i_GroupId }
                            join F in dbContext.systemparameter on new { a = A.i_ServiceId.Value, b = 119 } equals new { a = F.i_ParameterId, b = F.i_GroupId }
                            join G in dbContext.systemparameter on new { a = A.i_NewContinuationId.Value, b = 121 } equals new { a = G.i_ParameterId, b = G.i_GroupId }
                            join H in dbContext.systemparameter on new { a = A.i_CalendarStatusId.Value, b = 122 } equals new { a = H.i_ParameterId, b = H.i_GroupId }
                            join I in dbContext.systemparameter on new { a = A.i_IsVipId.Value, b = 111 } equals new { a = I.i_ParameterId, b = I.i_GroupId }

                            join J in dbContext.protocol on new { a = D.v_ProtocolId }
                                         equals new { a = J.v_ProtocolId } into J_join
                            from J in J_join.DefaultIfEmpty()

                            join K in dbContext.systemparameter on new { a = J.i_EsoTypeId.Value, b = 118 }
                                         equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            // Empresa / Sede Cliente **************
                            join oc in dbContext.organization on new { a = J.v_CustomerOrganizationId }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join lc in dbContext.location on new { a = J.v_CustomerOrganizationId, b = J.v_CustomerLocationId }
                                  equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                            from lc in lc_join.DefaultIfEmpty()

                            // Empresa / Sede Trabajo  ********************************************************
                            join ow in dbContext.organization on new { a = J.v_WorkingOrganizationId }
                                    equals new { a = ow.v_OrganizationId } into ow_join
                            from ow in ow_join.DefaultIfEmpty()

                            join lw in dbContext.location on new { a = J.v_WorkingOrganizationId, b = J.v_WorkingLocationId }
                                 equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                            from lw in lw_join.DefaultIfEmpty()

                            //************************************************************************************

                            join N in dbContext.organization on new { a = D.v_OrganizationId }
                                    equals new { a = N.v_OrganizationId } into N_join
                            from N in N_join.DefaultIfEmpty()

                            join O in dbContext.location on new { a = N.v_OrganizationId, b = D.v_LocationId }
                                    equals new { a = O.v_OrganizationId, b = O.v_LocationId } into O_join
                            from O in O_join.DefaultIfEmpty()

                            join J3 in dbContext.systemparameter on new { a = D.i_ServiceStatusId.Value, b = 125 }
                                            equals new { a = J3.i_ParameterId, b = J3.i_GroupId } into J3_join
                            from J3 in J3_join.DefaultIfEmpty()

                            join J4 in dbContext.systemparameter on new { a = D.i_AptitudeStatusId.Value, b = 124 }
                                            equals new { a = J4.i_ParameterId, b = J4.i_GroupId } into J4_join
                            from J4 in J4_join.DefaultIfEmpty()

                            join J5 in dbContext.datahierarchy on new { a = B.i_DocTypeId.Value, b = 106 }
                                           equals new { a = J5.i_ItemId, b = J5.i_GroupId } into J5_join
                            from J5 in J5_join.DefaultIfEmpty()

                            join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                      equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            where A.i_IsDeleted == 0
                            select new CalendarList
                            {
                                v_CalendarId = A.v_CalendarId,
                                d_DateTimeCalendar = A.d_DateTimeCalendar.Value,
                                v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                v_NumberDocument = B.v_DocNumber,
                                v_LineStatusName = C.v_Value1,
                                v_ServiceId = A.v_ServiceId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_ProtocolName = J.v_Name,
                                v_ServiceStatusName = J3.v_Value1,
                                v_AptitudeStatusName = J4.v_Value1,
                                v_ServiceTypeName = E.v_Value1,
                                v_ServiceName = F.v_Value1,
                                v_NewContinuationName = G.v_Value1,

                                v_PersonId = A.v_PersonId,
                                v_CalendarStatusName = H.v_Value1,
                                i_ServiceStatusId = D.i_ServiceStatusId.Value,
                                v_IsVipName = I.v_Value1,

                                i_ServiceId = A.i_ServiceId.Value,
                                i_ServiceTypeId = A.i_ServiceTypeId.Value,
                                i_CalendarStatusId = A.i_CalendarStatusId.Value,
                                i_MasterServiceId = A.i_ServiceId.Value,

                                i_NewContinuationId = A.i_NewContinuationId.Value,
                                i_LineStatusId = A.i_LineStatusId.Value,
                                i_IsVipId = A.i_IsVipId.Value,

                                i_EsoTypeId = J.i_EsoTypeId.Value,
                                v_EsoTypeName = K.v_Value1,

                                v_OrganizationLocationProtocol = oc.v_Name + " / " + lc.v_Name,
                                v_OrganizationLocationService = N.v_Name + " / " + O.v_Name,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = A.d_InsertDate,
                                d_UpdateDate = A.d_UpdateDate,
                                i_IsDeleted = A.i_IsDeleted,

                                v_CustomerOrganizationId = oc.v_OrganizationId,
                                v_CustomerLocationId = lc.v_LocationId,

                                v_DocTypeName = J5.v_Value1,
                                v_DocNumber = B.v_DocNumber,
                                i_DocTypeId = B.i_DocTypeId.Value,
                                d_EntryTimeCM = A.d_EntryTimeCM.Value,

                                v_WorkingOrganizationName = ow.v_Name
                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    query = query.Where("d_DateTimeCalendar >= @0 && d_DateTimeCalendar <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }
                //if (pintPageIndex.HasValue && pintResultsPerPage.HasValue)
                //{
                //    int intStartRowIndex = pintPageIndex.Value * pintResultsPerPage.Value;
                //    query = query.Skip(intStartRowIndex);
                //}
                if (pintResultsPerPage.HasValue)
                {
                    query = query.Take(pintResultsPerPage.Value);
                }

                List<CalendarList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return objData;

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

    }
}
