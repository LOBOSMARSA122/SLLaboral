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
   public class ServiceBL
    {

       List<int> ListaDiente = new List<int>();

       public List<ServiceList> GetService(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? FechaInico, DateTime? FechaFin)
       {
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = from A in dbContext.protocolsystemuser

                           join D in dbContext.protocol on A.v_ProtocolId equals    D.v_ProtocolId
                           join E in dbContext.organization on D.v_CustomerOrganizationId equals    E.v_OrganizationId
                           join F in dbContext.service on A.v_ProtocolId equals F.v_ProtocolId

                           join H in dbContext.person on F.v_PersonId equals H.v_PersonId

                           join G in dbContext.calendar on F.v_ServiceId equals     G.v_ServiceId
                           join J4 in dbContext.systemparameter on new { ItemId = F.i_AptitudeStatusId.Value, groupId = 124 }
                                     equals new { ItemId = J4.i_ParameterId, groupId = J4.i_GroupId } into J4_join
                           from J4 in J4_join.DefaultIfEmpty()

                           where F.i_IsDeleted == 0 
                           && F.i_StatusLiquidation ==2
                           //&& F.i_ServiceStatusId == (int)Common.ServiceStatus.Culminado
                          
                           select new ServiceList
                           {
                               v_ServiceId = F.v_ServiceId,
                               v_IdTrabajador = H.v_PersonId,
                               v_Trabajador = H.v_FirstLastName + " " + H.v_SecondLastName + " " + H.v_FirstName,
                               d_ServiceDate = F.d_ServiceDate.Value,
                               i_AptitudeId = F.i_AptitudeStatusId.Value,
                               i_TypeEsoId = F.i_MasterServiceId.Value,
                               v_ProtocolId = F.v_ProtocolId,
                               v_HCL = F.v_ServiceId,
                               v_AptitudeStatusName = J4.v_Value1,
                               v_ProtocolName = D.v_Name,
                               EmpresaCliente = E.v_Name,
                               v_CustomerOrganizationId = E.v_OrganizationId
                              
                           };

             

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (FechaInico.HasValue && FechaFin.HasValue)
                {
                    query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", FechaInico.Value, FechaFin.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }

                var query1 = (from A in query.ToList()
                             let x = GetRestricctionByServiceId(A.v_ServiceId)
                             select new ServiceList
                             {
                                 v_ServiceId = A.v_ServiceId,
                                 v_IdTrabajador = A.v_IdTrabajador,
                                 v_Trabajador = A.v_Trabajador,
                                 d_ServiceDate = A.d_ServiceDate,
                                 i_AptitudeId = A.i_AptitudeId,
                                 i_TypeEsoId =A.i_TypeEsoId,
                                 v_ProtocolId = A.v_ProtocolId,
                                 v_HCL =A.v_HCL,
                                 v_AptitudeStatusName = A.v_AptitudeStatusName,
                                 v_ProtocolName = A.v_ProtocolName,
                                 EmpresaCliente = A.EmpresaCliente,
                                 v_CustomerOrganizationId = A.v_CustomerOrganizationId,
                                 v_Restricction = x
                             }).ToList();



                var objData = query1.AsEnumerable()
                         .GroupBy(x => x.v_ServiceId)
                         .Select(group => group.First());

                List<ServiceList> objData1 = objData.ToList();
                pobjOperationResult.Success = 1; 
                return objData1;
           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = ex.Message;
               return null;
           }
       }

       public List<CalendarList> GetCalendarsPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? FechaInico, DateTime? FechaFin)
       {          
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = from A in dbContext.calendar
                           join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                           join D in dbContext.service on A.v_ServiceId equals D.v_ServiceId

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

                           join J4 in dbContext.systemparameter on new { ItemId = D.i_MasterServiceId.Value, groupId = 124 }
                                   equals new { ItemId = J4.i_ParameterId, groupId = J4.i_GroupId } into J4_join
                           from J4 in J4_join.DefaultIfEmpty()


                           where A.i_IsDeleted == 0
                           select new CalendarList
                           {
                               v_ServiceId = A.v_ServiceId,
                               v_IdTrabajador = B.v_PersonId,
                               v_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                               d_ServiceDate = D.d_ServiceDate.Value,
                               i_AptitudeId = D.i_AptitudeStatusId.Value,
                               i_TypeEsoId = D.i_MasterServiceId.Value,
                               v_ProtocolId = D.v_ProtocolId,
                               v_HCL = D.v_ServiceId,
                               v_AptitudeStatusName = J4.v_Value1,
                               v_ProtocolName = J.v_Name,
                               EmpresaCliente = oc.v_Name,
                               v_CalendarId = A.v_CalendarId,
                               d_DateTimeCalendar = A.d_DateTimeCalendar.Value,
                               v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                               v_NumberDocument = B.v_DocNumber,
                               v_PersonId = A.v_PersonId,
                               i_ServiceStatusId = D.i_ServiceStatusId.Value,

                               i_ServiceId = A.i_ServiceId.Value,
                               i_ServiceTypeId = A.i_ServiceTypeId.Value,
                               i_CalendarStatusId = A.i_CalendarStatusId.Value,
                               i_MasterServiceId = A.i_ServiceId.Value,

                               i_NewContinuationId = A.i_NewContinuationId.Value,
                               i_LineStatusId = A.i_LineStatusId.Value,
                               i_IsVipId = A.i_IsVipId.Value,

                               i_EsoTypeId = J.i_EsoTypeId.Value,
                               v_EsoTypeName = K.v_Value1,

                               v_CustomerOrganizationId = oc.v_OrganizationId,
                               v_CustomerLocationId = lc.v_LocationId,

                               v_DocNumber = B.v_DocNumber,
                               i_DocTypeId = B.i_DocTypeId.Value,
                               d_EntryTimeCM = A.d_EntryTimeCM.Value,

                           };

               if (!string.IsNullOrEmpty(pstrFilterExpression))
               {
                   query = query.Where(pstrFilterExpression);
               }
               if (FechaInico.HasValue && FechaFin.HasValue)
               {
                   query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", FechaInico.Value, FechaFin.Value);
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


       public List<DiagnosticRepositoryList> GetAptitudeCertificate(ref OperationResult pobjOperationResult, string pstrServiceId)
       {
           //mon.IsActive = true;
           var isDeleted = 0;
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = (from sss in dbContext.service
                            join ccc in dbContext.diagnosticrepository on sss.v_ServiceId equals ccc.v_ServiceId into ccc_join
                            from ccc in ccc_join.DefaultIfEmpty()  // ESO

                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                            from ddd in ddd_join.DefaultIfEmpty()  // Diagnosticos

                            join D in dbContext.person on sss.v_PersonId equals D.v_PersonId

                            join J in dbContext.systemparameter on new { a = D.i_SexTypeId.Value, b = 100 }
                                               equals new { a = J.i_ParameterId, b = J.i_GroupId }  // GENERO

                            join E in dbContext.protocol on sss.v_ProtocolId equals E.v_ProtocolId

                            join F in dbContext.groupoccupation on E.v_GroupOccupationId equals F.v_GroupOccupationId

                            join ooo in dbContext.organization on E.v_EmployerOrganizationId equals ooo.v_OrganizationId

                            join lll in dbContext.location on E.v_EmployerLocationId equals lll.v_LocationId

                            join H in dbContext.systemparameter on new { a = E.i_EsoTypeId.Value, b = 118 }
                                                equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                            join G in dbContext.systemparameter on new { a = sss.i_AptitudeStatusId.Value, b = 124 }
                                     equals new { a = G.i_ParameterId, b = G.i_GroupId }  // ESTADO APTITUD ESO                    

                            join J3 in dbContext.systemparameter on new { a = 119, b = sss.i_MasterServiceId.Value }  // DESCRIPCION DEL SERVICIO
                                                       equals new { a = J3.i_GroupId, b = J3.i_ParameterId } into J3_join
                            from J3 in J3_join.DefaultIfEmpty()

                            join J1 in dbContext.systemuser on new { i_InsertUserId = ccc.i_InsertUserId.Value }
                                            equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.systemuser on new { i_UpdateUserId = ccc.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join su in dbContext.systemuser on sss.i_UpdateUserOccupationalMedicaltId.Value equals su.i_SystemUserId into su_join
                            from su in su_join.DefaultIfEmpty()

                            join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                            from pr in pr_join.DefaultIfEmpty()

                            where (ccc.v_ServiceId == pstrServiceId) &&
                                  (ccc.i_IsDeleted == isDeleted) &&
                                  (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                                  ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo)

                            select new DiagnosticRepositoryList
                            {
                                v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                v_ServiceId = ccc.v_ServiceId,
                                v_DiseasesId = ccc.v_DiseasesId,
                                i_AutoManualId = ccc.i_AutoManualId,
                                i_PreQualificationId = ccc.i_PreQualificationId,
                                i_FinalQualificationId = ccc.i_FinalQualificationId,
                                i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,
                                d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,
                                v_DiseasesName = ddd.v_Name,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = J1.d_InsertDate,
                                d_UpdateDate = J2.d_UpdateDate,
                                i_IsDeleted = ccc.i_IsDeleted.Value,
                                v_ProtocolId = E.v_ProtocolId,
                                v_ProtocolName = E.v_Name,
                                v_PersonId = D.v_PersonId,
                                d_BirthDate = D.d_Birthdate,
                                v_EsoTypeName = H.v_Value1,
                                v_OrganizationPartialName = ooo.v_Name,
                                v_LocationName = lll.v_Name,
                                v_FirstName = D.v_FirstName,
                                v_FirstLastName = D.v_FirstLastName,
                                v_SecondLastName = D.v_SecondLastName,
                                v_DocNumber = D.v_DocNumber,
                                v_GenderName = J.v_Value1,
                                v_AptitudeStatusName = G.v_Value1,
                                v_OccupationName = D.v_CurrentOccupation,
                                g_Image = pr.b_SignatureImage,
                                d_ServiceDate = sss.d_ServiceDate,
                                i_AptitudeStatusId = sss.i_AptitudeStatusId,
                                i_EsoTypeId_Old = E.i_EsoTypeId.Value,
                            });

               var MedicalCenter = GetInfoMedicalCenter();

               var q = (from a in query.ToList()
                        select new DiagnosticRepositoryList
                        {
                            v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                            v_ServiceId = a.v_ServiceId,
                            v_DiseasesId = a.v_DiseasesId,
                            i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                            d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,
                            v_CreationUser = a.v_CreationUser,
                            v_UpdateUser = a.v_UpdateUser,
                            d_CreationDate = a.d_CreationDate,
                            d_UpdateDate = a.d_UpdateDate,
                            i_IsDeleted = a.i_IsDeleted,
                            i_EsoTypeId = a.i_EsoTypeId_Old.ToString(),
                            v_EsoTypeName = a.v_EsoTypeName,
                            v_OrganizationName = string.Format("{0} / {1}", a.v_OrganizationPartialName, a.v_LocationName),
                            v_PersonName = string.Format("{0} {1}, {2}", a.v_FirstLastName, a.v_SecondLastName, a.v_FirstName),
                            v_DocNumber = a.v_DocNumber,
                            i_Age = a.d_BirthDate == null ? (int?)null : DateTime.Today.AddTicks(-a.d_BirthDate.Value.Ticks).Year - 1,
                            v_GenderName = a.v_GenderName,
                            v_DiseasesName = a.v_DiseasesName,
                            v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                            v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                            v_AptitudeStatusName = a.v_AptitudeStatusName,
                            v_OccupationName = a.v_OccupationName,  // por ahora se muestra el GESO
                            g_Image = a.g_Image,
                            b_Logo = MedicalCenter.b_Image,
                            EmpresaPropietaria = MedicalCenter.v_Name,
                            EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                            EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                            EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                            v_ServiceDate = a.d_ServiceDate == null ? string.Empty : a.d_ServiceDate.Value.ToShortDateString(),
                            i_AptitudeStatusId = a.i_AptitudeStatusId
                        }).ToList();

               pobjOperationResult.Success = 1;
               return q;
           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
               return null;
           }
       }

       public Sigesoft.Node.WinClient.BE.organizationDto GetInfoMedicalCenter()
       {
           using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
           {

               var sql = (from o in dbContext.organization
                          where o.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                          select new Sigesoft.Node.WinClient.BE.organizationDto
                          {
                              v_Name = o.v_Name,
                              v_Address = o.v_Address,
                              b_Image = o.b_Image,
                              v_PhoneNumber = o.v_PhoneNumber,
                              v_Mail = o.v_Mail,

                          }).SingleOrDefault();


               return sql;
           }
       }

       private string ConcatenateRestriction(string pstrDiagnosticRepositoryId)
       {
           SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

           var qry = (from a in dbContext.restriction  // RESTRICCIONES POR Diagnosticos
                      join eee in dbContext.masterrecommendationrestricction on a.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId
                      where a.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                      a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Restricciones
                      select new
                      {
                          v_RestrictionsName = eee.v_Name
                      }).ToList();

           return string.Join(", ", qry.Select(p => p.v_RestrictionsName));
       }

       private string ConcatenateRecommendation(string pstrDiagnosticRepositoryId)
       {
           SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

           var qry = (from a in dbContext.recommendation  // RECOMENDACIONES
                      join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId
                      where a.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                      a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Recomendaciones
                      select new
                      {
                          v_RecommendationName = eee.v_Name
                      }).ToList();

           return string.Join(", ", qry.Select(p => p.v_RecommendationName));
       }

       public List<ServiceComponentList> GetServiceComponents(ref OperationResult pobjOperationResult, string pstrServiceId)
       {
          
           int isDeleted = (int)SiNo.NO;
           int isRequired = (int)SiNo.SI;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = (from A in dbContext.servicecomponent
                            join B in dbContext.systemparameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                     equals new { a = B.i_ParameterId, b = B.i_GroupId }
                            join C in dbContext.component on A.v_ComponentId equals C.v_ComponentId
                            join D in dbContext.systemparameter on new { a = A.i_QueueStatusId.Value, b = 128 }
                                     equals new { a = D.i_ParameterId, b = D.i_GroupId }
                            join E in dbContext.service on A.v_ServiceId equals E.v_ServiceId
                            join F in dbContext.systemparameter on new { a = C.i_CategoryId.Value, b = 116 }
                                     equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                            from F in F_join.DefaultIfEmpty()

                            where A.v_ServiceId == pstrServiceId &&
                                  A.i_IsDeleted == isDeleted &&
                                  A.i_IsRequiredId == isRequired

                            select new ServiceComponentList
                            {
                                v_ComponentId = A.v_ComponentId,
                                v_ComponentName = C.v_Name,
                                i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                v_ServiceComponentStatusName = B.v_Value1,
                                d_StartDate = A.d_StartDate.Value,
                                d_EndDate = A.d_EndDate.Value,
                                i_QueueStatusId = A.i_QueueStatusId.Value,
                                v_QueueStatusName = D.v_Value1,
                                ServiceStatusId = E.i_ServiceStatusId.Value,
                                v_Motive = E.v_Motive,
                                i_CategoryId = C.i_CategoryId.Value,
                                v_CategoryName = C.i_CategoryId.Value == -1 ? C.v_Name : F.v_Value1,
                                v_ServiceId = E.v_ServiceId
                            });

               var objData = query.AsEnumerable()
                            .Where(s => s.i_CategoryId != -1)
                            .GroupBy(x => x.i_CategoryId)
                            .Select(group => group.First());

               List<ServiceComponentList> obj = objData.ToList();

               obj.AddRange(query.Where(p => p.i_CategoryId == -1));

               pobjOperationResult.Success = 1;
               return obj;
           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
               return null;
           }
       }

       public string GetYearsAndMonth(DateTime? EndDate, DateTime? StartDate)
       {
           if (EndDate == null || StartDate == null)
           {
               return "0 años y 0 meses";
           }
           DateTime startDate = StartDate.Value;
           DateTime endDate = EndDate.Value;
           var totalDays = (endDate - startDate).TotalDays;
           var totalYears = Math.Truncate(totalDays / 365);
           var totalMonths = Math.Truncate((totalDays % 365) / 30);
           var remainingDays = Math.Truncate((totalDays % 365) % 30);

           if (totalYears == 0)
           {
               return string.Format("{1} mes(es)", totalMonths, remainingDays);
           }
           else if (totalMonths == 0)
           {
               return string.Format("{0} año(s)", totalYears, remainingDays);
           }
           else
           {
               return string.Format("{0} año(s), {1} mes(es)", totalYears, totalMonths, remainingDays);
           }
       }

       public Sigesoft.Node.WinClient.BE.ServiceList GetAnamnesisReport(string pstrServiceId)
       {
           //mon.IsActive = true;

           var isDeleted = 0;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = from A in dbContext.service

                           join K in dbContext.systemparameter on new { a = 135, b = A.i_HasSymptomId.Value }
                                                   equals new { a = K.i_GroupId, b = K.i_ParameterId } into K_join
                           from K in K_join.DefaultIfEmpty()

                           join L in dbContext.systemparameter on new { a = 135, b = A.i_UrineId.Value }
                                                   equals new { a = L.i_GroupId, b = L.i_ParameterId } into L_join
                           from L in L_join.DefaultIfEmpty()

                           join M in dbContext.systemparameter on new { a = 135, b = A.i_DepositionId.Value }
                                                 equals new { a = M.i_GroupId, b = M.i_ParameterId } into M_join
                           from M in M_join.DefaultIfEmpty()

                           join N in dbContext.systemparameter on new { a = 135, b = A.i_AppetiteId.Value }
                                       equals new { a = N.i_GroupId, b = N.i_ParameterId } into N_join
                           from N in N_join.DefaultIfEmpty()

                           join O in dbContext.systemparameter on new { a = 134, b = A.i_MacId.Value }
                                 equals new { a = O.i_GroupId, b = O.i_ParameterId } into O_join
                           from O in O_join.DefaultIfEmpty()

                           join P in dbContext.systemparameter on new { a = 135, b = A.i_AppetiteId.Value }
                                     equals new { a = P.i_GroupId, b = P.i_ParameterId } into P_join
                           from P in P_join.DefaultIfEmpty()

                           join su in dbContext.systemuser on A.i_UpdateUserMedicalAnalystId.Value equals su.i_SystemUserId into su_join
                           from su in su_join.DefaultIfEmpty()

                           join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                           from pr in pr_join.DefaultIfEmpty()

                           where A.v_ServiceId == pstrServiceId && A.i_IsDeleted == isDeleted

                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_ServiceId = A.v_ServiceId,
                               i_HasSymptomId = A.i_HasSymptomId,
                               v_MainSymptom = A.v_MainSymptom,
                               i_TimeOfDisease = A.i_TimeOfDisease,
                               i_TimeOfDiseaseTypeId = A.i_TimeOfDiseaseTypeId,
                               v_Story = A.v_Story,
                               v_PersonId = pr.v_PersonId,

                               i_DreamId = A.i_DreamId,
                               v_Dream = K.v_Value1,
                               i_UrineId = A.i_UrineId,
                               v_Urine = L.v_Value1,
                               i_DepositionId = A.i_DepositionId,
                               v_Deposition = M.v_Value1,
                               i_AppetiteId = A.i_AppetiteId,
                               v_Appetite = N.v_Value1,
                               i_ThirstId = A.i_ThirstId,
                               v_Thirst = P.v_Value1,
                               d_Fur = A.d_Fur.Value,
                               v_CatemenialRegime = A.v_CatemenialRegime,
                               i_MacId = A.i_MacId,
                               v_Mac = O.v_Value1,

                               // Antecedentes ginecologicos
                               d_PAP = A.d_PAP.Value,
                               d_Mamografia = A.d_Mamografia.Value,
                               v_CiruGine = A.v_CiruGine,
                               v_Gestapara = A.v_Gestapara,
                               v_Menarquia = A.v_Menarquia,
                               v_Findings = A.v_Findings,

                               // firma y sello del medico que analisa y califica los diagnosticos
                               FirmaDoctor = pr.b_SignatureImage

                           };

               Sigesoft.Node.WinClient.BE.ServiceList objData = query.FirstOrDefault();
               return objData;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public List<Sigesoft.Node.WinClient.BE.ServiceComponentList> GetServiceComponentsReport(string pstrServiceId)
       {
           //mon.IsActive = true;        
           int isDeleted = 0;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               #region serviceComponentFields

               var serviceComponentFields = (from A in dbContext.servicecomponent
                                             join B in dbContext.servicecomponentfields on A.v_ServiceComponentId equals B.v_ServiceComponentId
                                             join C in dbContext.servicecomponentfieldvalues on B.v_ServiceComponentFieldsId equals C.v_ServiceComponentFieldsId
                                             join cfs in dbContext.componentfields on B.v_ComponentFieldId equals cfs.v_ComponentFieldId
                                             join D in dbContext.componentfield on B.v_ComponentFieldId equals D.v_ComponentFieldId
                                             join cm in dbContext.component on cfs.v_ComponentId equals cm.v_ComponentId

                                             join dh in dbContext.datahierarchy on new { a = 105, b = D.i_MeasurementUnitId.Value }
                                                                equals new { a = dh.i_GroupId, b = dh.i_ItemId } into dh_join
                                             from dh in dh_join.DefaultIfEmpty()

                                             where (A.v_ServiceId == pstrServiceId) &&
                                                 //(cm.v_ComponentId == pstrComponentId) &&
                                                   (A.i_IsDeleted == isDeleted) &&
                                                   (B.i_IsDeleted == isDeleted) &&
                                                   (C.i_IsDeleted == isDeleted)

                                             select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList
                                             {
                                                 v_ServiceComponentFieldsId = B.v_ServiceComponentFieldsId,
                                                 v_ComponentFieldsId = B.v_ComponentFieldId,
                                                 v_ComponentFielName = D.v_TextLabel,
                                                 v_Value1 = C.v_Value1 == "" ? null : C.v_Value1,
                                                 i_GroupId = D.i_GroupId.Value,
                                                 v_MeasurementUnitName = dh.v_Value1,
                                                 v_ComponentId = cm.v_ComponentId,
                                                 v_ServiceComponentId = A.v_ServiceComponentId
                                             }).ToList();

               int rpta = 0;

               var _finalQuery = (from a in serviceComponentFields
                                  let value1 = int.TryParse(a.v_Value1, out rpta)
                                  join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                  equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                  from sp in sp_join.DefaultIfEmpty()

                                  select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList
                                  {
                                      v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                      v_ComponentFieldsId = a.v_ComponentFieldsId,
                                      v_ComponentFielName = a.v_ComponentFielName,
                                      i_GroupId = a.i_GroupId,
                                      v_Value1 = a.v_Value1,
                                      v_Value1Name = sp == null ? "" : sp.v_Value1,
                                      v_MeasurementUnitName = a.v_MeasurementUnitName,
                                      v_ComponentId = a.v_ComponentId,
                                      v_ConclusionAndDiagnostic = a.v_Value1 + " / " + GetServiceComponentDiagnosticsReport(pstrServiceId, a.v_ComponentId),
                                      v_ServiceComponentId = a.v_ServiceComponentId
                                  }).ToList();


               #endregion

               var components = (from aaa in dbContext.servicecomponent
                                 join bbb in dbContext.component on aaa.v_ComponentId equals bbb.v_ComponentId
                                 join J1 in dbContext.systemuser on new { i_InsertUserId = aaa.i_InsertUserId.Value }
                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = aaa.i_UpdateUserId.Value }
                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                 from J2 in J2_join.DefaultIfEmpty()

                                 join fff in dbContext.systemparameter on new { a = bbb.i_CategoryId.Value, b = 116 } // CATEGORIA DEL EXAMEN
                                                                              equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                 from fff in J5_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on aaa.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 //*********************************************************************

                                 where (aaa.v_ServiceId == pstrServiceId) &&
                                       (bbb.i_ComponentTypeId == (int?)ComponentType.Examen) &&
                                       (aaa.i_IsDeleted == 0) &&
                                       (aaa.i_IsRequiredId == (int?)SiNo.SI)

                                 //orderby bbb.i_CategoryId, bbb.v_Name

                                 select new
                                 {
                                     v_ComponentId = bbb.v_ComponentId,
                                     v_ComponentName = bbb.v_Name,
                                     v_ServiceComponentId = aaa.v_ServiceComponentId,
                                     v_CreationUser = J1.v_UserName,
                                     v_UpdateUser = J2.v_UserName,
                                     d_CreationDate = aaa.d_InsertDate,
                                     d_UpdateDate = aaa.d_UpdateDate,
                                     i_IsDeleted = aaa.i_IsDeleted.Value,
                                     i_CategoryId = bbb.i_CategoryId.Value,
                                     v_CategoryName = fff.v_Value1,
                                     DiagnosticRepository = (from dr in aaa.service.diagnosticrepository
                                                             where (dr.v_ServiceId == pstrServiceId) &&
                                                                   (dr.v_ComponentId == aaa.v_ComponentId)
                                                             select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                                                             {
                                                                 v_DiseasesId = dr.diseases.v_DiseasesId,
                                                                 v_DiseasesName = dr.diseases.v_Name
                                                             }),
                                     FirmaMedico = pme.b_SignatureImage
                                 }).AsEnumerable().Select(p => new Sigesoft.Node.WinClient.BE.ServiceComponentList
                                 {
                                     v_ComponentId = p.v_ComponentId,
                                     v_ComponentName = p.v_ComponentName,
                                     v_ServiceComponentId = p.v_ServiceComponentId,
                                     v_CreationUser = p.v_CreationUser,
                                     v_UpdateUser = p.v_UpdateUser,
                                     d_CreationDate = p.d_CreationDate,
                                     d_UpdateDate = p.d_UpdateDate,
                                     i_IsDeleted = p.i_IsDeleted,
                                     i_CategoryId = p.i_CategoryId,
                                     v_CategoryName = p.v_CategoryName,
                                     DiagnosticRepository = p.DiagnosticRepository.ToList(),
                                     FirmaMedico = p.FirmaMedico
                                 }).ToList();

               //var ff = _finalQuery.FindAll(p => p.v_ComponentId == Constants.GLUCOSA_ID);
               //var ff_ = _finalQuery.FindAll(p => p.v_ComponentId == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID);
               //var _ff_ = _finalQuery.FindAll(p => p.v_ComponentId == Constants.COLESTEROL_ID);

               components.Sort((x, y) => x.v_ComponentId.CompareTo(y.v_ComponentId));
               components.ForEach(a => a.ServiceComponentFields = _finalQuery.FindAll(p => p.v_ComponentId == a.v_ComponentId));

               return components;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public string GetServiceComponentDiagnosticsReport(string pstrServiceId, string pstrComponentId)
       {
           //mon.IsActive = true;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = (from ccc in dbContext.diagnosticrepository
                            join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId
                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos 
                            where (ccc.v_ServiceId == pstrServiceId) &&
                                  (ccc.v_ComponentId == pstrComponentId) &&
                                  (ccc.i_IsDeleted == 0)
                            select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                            {
                                v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                v_ServiceId = ccc.v_ServiceId,
                                v_ComponentId = ccc.v_ComponentId,
                                v_DiseasesId = ccc.v_DiseasesId,
                                v_DiseasesName = ddd.v_Name,

                            }).ToList();

               var concat = string.Join(", ", query.Select(p => p.v_DiseasesName));

               return concat;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetServiceComponentConclusionesDxServiceIdReport(string pstrServiceId)
       {
           //mon.IsActive = true;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = (from ccc in dbContext.diagnosticrepository
                            join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId into J7_join
                            from bbb in J7_join.DefaultIfEmpty()

                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos                       

                            join fff in dbContext.systemparameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                            from fff in J5_join.DefaultIfEmpty()

                            join ggg in dbContext.systemparameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                            from ggg in J4_join.DefaultIfEmpty()

                            join hhh in dbContext.systemparameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                    equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                            from hhh in J3_join.DefaultIfEmpty()

                            where (ccc.v_ServiceId == pstrServiceId) &&
                            (ccc.i_IsDeleted == 0) &&
                            (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                            ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo)
                            orderby bbb.v_Name

                            select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                            {
                                v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                v_ServiceId = ccc.v_ServiceId,
                                v_ComponentId = ccc.v_ComponentId,
                                v_DiseasesId = ccc.v_DiseasesId,
                                v_DiseasesName = ddd.v_Name,
                                v_ComponentName = bbb.v_Name,
                                v_PreQualificationName = fff.v_Value1,
                                v_FinalQualificationName = ggg.v_Value1,
                                v_DiagnosticTypeName = hhh.v_Value1,
                                v_ComponentFieldsId = ccc.v_ComponentFieldId
                            }).ToList();

               // add the sequence number on the fly
               var finalQuery = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
               {
                   i_Item = index + 1,
                   v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                   v_ServiceId = a.v_ServiceId,
                   v_ComponentId = a.v_ComponentId,
                   v_DiseasesId = a.v_DiseasesId,
                   v_DiseasesName = a.v_DiseasesName,
                   v_ComponentName = a.v_ComponentName,
                   v_DiagnosticTypeName = a.v_DiagnosticTypeName,
                   Recomendations = GetServiceRecommendationByDiagnosticRepositoryIdReport(a.v_DiagnosticRepositoryId),
                   Restrictions = GetServiceRestrictionByDiagnosticRepositoryIdReport(a.v_DiagnosticRepositoryId),
                   v_ComponentFieldsId = a.v_ComponentFieldsId
               }).ToList();

               return finalQuery;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public List<Sigesoft.Node.WinClient.BE.RecomendationList> GetServiceRecommendationByDiagnosticRepositoryIdReport(string pstrDiagnosticRepositoryId)
       {
           //mon.IsActive = true;
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               List<Sigesoft.Node.WinClient.BE.RecomendationList> query = (from ddd in dbContext.recommendation
                                                join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRecommendationId
                                                                        equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                                                              
                                                where (ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId) &&
                                                      (ddd.i_IsDeleted == 0)
                                                                           select new Sigesoft.Node.WinClient.BE.RecomendationList
                                                {
                                                    v_RecommendationId = ddd.v_RecommendationId,
                                                    v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                    v_ServiceId = ddd.v_ServiceId,
                                                    v_ComponentId = ddd.v_ComponentId,
                                                    v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                    v_RecommendationName = eee.v_Name,

                                                }).ToList();

               // add the sequence number on the fly
               var finalQuery = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.RecomendationList
               {
                   i_Item = index + 1,
                   v_RecommendationId = a.v_RecommendationId,
                   v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                   v_ServiceId = a.v_ServiceId,
                   v_ComponentId = a.v_ComponentId,
                   v_MasterRecommendationId = a.v_MasterRecommendationId,
                   v_RecommendationName = a.v_RecommendationName,
               }).ToList();

               return finalQuery;
           }
           catch (Exception ex)
           {

               return null;
           }
       }

       public Sigesoft.Node.WinClient.BE.ServiceList GetServiceReport(string pstrServiceId)
       {
           //mon.IsActive = true;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var objEntity = (from A in dbContext.service
                                join B in dbContext.protocol on A.v_ProtocolId equals B.v_ProtocolId into B_join
                                from B in B_join.DefaultIfEmpty()

                                join C in dbContext.organization on B.v_WorkingOrganizationId equals C.v_OrganizationId into C_join
                                from C in C_join.DefaultIfEmpty()

                                join D in dbContext.datahierarchy on new { a = C.i_SectorTypeId.Value, b = 104 }
                                                       equals new { a = D.i_ItemId, b = D.i_GroupId } into D_join
                                from D in D_join.DefaultIfEmpty()

                                join E in dbContext.datahierarchy on new { a = C.i_DepartmentId.Value, b = 113 }
                                                      equals new { a = E.i_ItemId, b = E.i_GroupId } into E_join
                                from E in E_join.DefaultIfEmpty()

                                join F in dbContext.datahierarchy on new { a = C.i_ProvinceId.Value, b = 113 }
                                                      equals new { a = F.i_ItemId, b = F.i_GroupId } into F_join
                                from F in F_join.DefaultIfEmpty()

                                join G in dbContext.datahierarchy on new { a = C.i_DistrictId.Value, b = 113 }
                                                      equals new { a = G.i_ItemId, b = G.i_GroupId } into G_join
                                from G in G_join.DefaultIfEmpty()

                                join H in dbContext.person on A.v_PersonId equals H.v_PersonId into H_join
                                from H in H_join.DefaultIfEmpty()

                                join I in dbContext.datahierarchy on new { a = H.i_DepartmentId.Value, b = 113 }
                                                      equals new { a = I.i_ItemId, b = I.i_GroupId } into I_join
                                from I in I_join.DefaultIfEmpty()

                                join J in dbContext.datahierarchy on new { a = H.i_ProvinceId.Value, b = 113 }
                                                      equals new { a = J.i_ItemId, b = J.i_GroupId } into J_join
                                from J in J_join.DefaultIfEmpty()

                                join K in dbContext.datahierarchy on new { a = H.i_DistrictId.Value, b = 113 }
                                                      equals new { a = K.i_ItemId, b = K.i_GroupId } into K_join
                                from K in K_join.DefaultIfEmpty()

                                join L in dbContext.systemparameter on new { a = H.i_TypeOfInsuranceId.Value, b = 188 }
                                                     equals new { a = L.i_ParameterId, b = L.i_GroupId } into L_join
                                from L in L_join.DefaultIfEmpty()

                                join M in dbContext.systemparameter on new { a = H.i_MaritalStatusId.Value, b = 101 }
                                             equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                from M in M_join.DefaultIfEmpty()

                                join N in dbContext.datahierarchy on new { a = H.i_LevelOfId.Value, b = 108 }
                                                equals new { a = N.i_ItemId, b = N.i_GroupId } into N_join
                                from N in N_join.DefaultIfEmpty()


                                join C1 in dbContext.organization on B.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                from C1 in C1_join.DefaultIfEmpty()


                                join su in dbContext.systemuser on A.i_UpdateUserMedicalAnalystId.Value equals su.i_SystemUserId into su_join
                                from su in su_join.DefaultIfEmpty()

                                join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                                from pr in pr_join.DefaultIfEmpty()


                                join P1 in dbContext.person on new { a = pr.v_PersonId }
                                        equals new { a = P1.v_PersonId } into P1_join
                                from P1 in P1_join.DefaultIfEmpty()

                                join O in dbContext.systemparameter on new { a = 134, b = A.i_MacId.Value }
                                                       equals new { a = O.i_GroupId, b = O.i_ParameterId } into O_join
                                from O in O_join.DefaultIfEmpty()

                                where A.v_ServiceId == pstrServiceId

                                select new Sigesoft.Node.WinClient.BE.ServiceList
                                {
                                    //-----------------CABECERA---------------------------------
                                    v_PersonId = H.v_PersonId,
                                    v_ServiceId = A.v_ServiceId,
                                    d_ServiceDate = A.d_ServiceDate,
                                    i_DiaV = A.d_ServiceDate.Value.Day,
                                    i_MesV = A.d_ServiceDate.Value.Month,
                                    i_AnioV = A.d_ServiceDate.Value.Year,
                                    i_EsoTypeId = B.i_EsoTypeId.Value, // tipo de ESO : Pre-Ocupacional ,  Periodico, etc 
                                    //---------------DATOS DE LA EMPRESA--------------------------------
                                    EmpresaTrabajo = C.v_Name,
                                    EmpresaEmpleadora = C1.v_Name,
                                    RubroEmpresaTrabajo = D.v_Value1,
                                    DireccionEmpresaTrabajo = C.v_Address,
                                    DepartamentoEmpresaTrabajo = E.v_Value1,
                                    ProvinciaEmpresaTrabajo = F.v_Value1,
                                    DistritoEmpresaTrabajo = G.v_Value1,
                                    v_CurrentOccupation = H.v_CurrentOccupation,
                                    //---------------DATOS DE FILIACIÓN TRABAJADOR--------------------------------
                                    i_DocTypeId = H.i_DocTypeId.Value,
                                    v_Pacient = H.v_FirstName + " " + H.v_FirstLastName + " " + H.v_SecondLastName,
                                    d_BirthDate = H.d_Birthdate,
                                    i_DiaN = H.d_Birthdate.Value.Day,
                                    i_MesN = H.d_Birthdate.Value.Month,
                                    i_AnioN = H.d_Birthdate.Value.Year,
                                    v_DocNumber = H.v_DocNumber,
                                    v_AdressLocation = H.v_AdressLocation,

                                    DepartamentoPaciente = I.v_Value1,
                                    ProvinciaPaciente = J.v_Value1,
                                    DistritoPaciente = K.v_Value1,
                                    i_ResidenceInWorkplaceId = H.i_ResidenceInWorkplaceId.Value,
                                    v_ResidenceTimeInWorkplace = H.v_ResidenceTimeInWorkplace,
                                    i_TypeOfInsuranceId = H.i_TypeOfInsuranceId.Value,
                                    Email = H.v_Mail,
                                    Telefono = H.v_TelephoneNumber,
                                    EstadoCivil = M.v_Value1,
                                    GradoInstruccion = N.v_Value1,
                                    v_Story = A.v_Story,
                                    i_AptitudeStatusId = A.i_AptitudeStatusId,

                                    HijosVivos = H.i_NumberLivingChildren,
                                    HijosMuertos = H.i_NumberDeadChildren,
                                    HijosDependientes = H.i_NumberDependentChildren,

                                    v_BirthPlace = H.v_BirthPlace,
                                    i_PlaceWorkId = H.i_PlaceWorkId.Value,
                                    v_ExploitedMineral = H.v_ExploitedMineral,
                                    i_AltitudeWorkId = H.i_AltitudeWorkId.Value,
                                    v_EmergencyPhone = H.v_EmergencyPhone,
                                    i_SexTypeId = H.i_SexTypeId,
                                    i_MaritalStatusId = H.i_MaritalStatusId.Value,
                                    i_LevelOfId = H.i_LevelOfId.Value,
                                    FirmaTrabajador = H.b_RubricImage,
                                    HuellaTrabajador = H.b_FingerPrintImage,

                                    //Datos del Doctor
                                    FirmaDoctor = pr.b_SignatureImage,
                                    NombreDoctor = P1.v_FirstName + " " + P1.v_FirstLastName + " " + P1.v_SecondLastName,
                                    CMP = pr.v_ProfessionalCode,

                                    d_Fur = A.d_Fur,
                                    v_CatemenialRegime = A.v_CatemenialRegime,
                                    i_MacId = A.i_MacId,
                                    v_Mac = O.v_Value1,

                                    // Antecedentes ginecologicos
                                    d_PAP = A.d_PAP.Value,
                                    d_Mamografia = A.d_Mamografia.Value,
                                    v_CiruGine = A.v_CiruGine,
                                    v_Gestapara = A.v_Gestapara,
                                    v_Menarquia = A.v_Menarquia,
                                    v_Findings = A.v_Findings,


                                });


               var sql = (from a in objEntity.ToList()
                          select new Sigesoft.Node.WinClient.BE.ServiceList
                          {
                              //-----------------CABECERA---------------------------------
                              v_ServiceId = a.v_ServiceId,
                              d_ServiceDate = a.d_ServiceDate,
                              i_DiaV = a.d_ServiceDate.Value.Day,
                              i_MesV = a.d_ServiceDate.Value.Month,
                              i_AnioV = a.d_ServiceDate.Value.Year,
                              i_EsoTypeId = a.i_EsoTypeId, // tipo de ESO : Pre-Ocupacional ,  Periodico, etc 
                              //---------------DATOS DE LA EMPRESA--------------------------------
                              EmpresaTrabajo = a.EmpresaTrabajo,
                              EmpresaEmpleadora = a.EmpresaEmpleadora,
                              RubroEmpresaTrabajo = a.RubroEmpresaTrabajo,
                              DireccionEmpresaTrabajo = a.DireccionEmpresaTrabajo,
                              DepartamentoEmpresaTrabajo = a.DepartamentoEmpresaTrabajo,
                              ProvinciaEmpresaTrabajo = a.ProvinciaEmpresaTrabajo,
                              DistritoEmpresaTrabajo = a.DistritoEmpresaTrabajo,
                              v_CurrentOccupation = a.v_CurrentOccupation,
                              //---------------DATOS DE FILIACIÓN TRABAJADOR--------------------------------
                              i_DocTypeId = a.i_DocTypeId,
                              v_Pacient = a.v_Pacient,
                              d_BirthDate = a.d_BirthDate,
                              i_DiaN = a.i_DiaN,
                              i_MesN = a.i_MesN,
                              i_AnioN = a.i_AnioN,
                              i_Edad = GetAge(a.d_BirthDate.Value),
                              //i_Edad =30,
                              v_DocNumber = a.v_DocNumber,
                              v_AdressLocation = a.v_AdressLocation,
                              DepartamentoPaciente = a.DepartamentoPaciente,
                              ProvinciaPaciente = a.ProvinciaPaciente,
                              DistritoPaciente = a.DistritoPaciente,
                              i_ResidenceInWorkplaceId = a.i_ResidenceInWorkplaceId,
                              v_ResidenceTimeInWorkplace = a.v_ResidenceTimeInWorkplace,
                              i_TypeOfInsuranceId = a.i_TypeOfInsuranceId,
                              Email = a.Email,
                              Telefono = a.Telefono,
                              EstadoCivil = a.EstadoCivil,
                              GradoInstruccion = a.GradoInstruccion,
                              v_Story = a.v_Story,
                              i_AptitudeStatusId = a.i_AptitudeStatusId,
                              v_OwnerOrganizationName = (from n in dbContext.organization
                                                         where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                                         select n.v_Name + " " + n.v_Address).SingleOrDefault<string>(),

                              HijosVivos = a.HijosVivos,
                              HijosMuertos = a.HijosMuertos,
                              HijosDependientes = a.HijosDependientes,
                              v_BirthPlace = a.v_BirthPlace,
                              i_PlaceWorkId = a.i_PlaceWorkId,
                              v_ExploitedMineral = a.v_ExploitedMineral,
                              i_AltitudeWorkId = a.i_AltitudeWorkId,
                              v_EmergencyPhone = a.v_EmergencyPhone,
                              i_SexTypeId = a.i_SexTypeId,
                              i_MaritalStatusId = a.i_MaritalStatusId,
                              i_LevelOfId = a.i_LevelOfId,

                              FirmaTrabajador = a.FirmaTrabajador,
                              HuellaTrabajador = a.HuellaTrabajador,

                              //Datos del Doctor
                              FirmaDoctor = a.FirmaDoctor,
                              NombreDoctor = a.NombreDoctor,
                              CMP = a.CMP,

                              d_Fur = a.d_Fur,
                              v_CatemenialRegime = a.v_CatemenialRegime,
                              i_MacId = a.i_MacId,
                              v_Mac = a.v_Mac,

                              //// Antecedentes ginecologicos
                              d_PAP = a.d_PAP,
                              d_Mamografia = a.d_Mamografia,
                              v_CiruGine = a.v_CiruGine,
                              v_Gestapara = a.v_Gestapara,
                              v_Menarquia = a.v_Menarquia,
                              v_Findings = a.v_Findings,

                          }).FirstOrDefault();

               return sql;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public int GetAge(DateTime FechaNacimiento)
       {
           return int.Parse((DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1).ToString());

       }

       public string GetCantidadCaries(string pstrServiceId, string pstrComponentId, string pstrFieldId)
       {
           try
           {
               string Retornar = "0";
               string[] componentId = null;
               ServiceBL oServiceBL = new ServiceBL();
               List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

               oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);
               var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

               componentId = xx.Split(';');
               if (componentId[0] == "")
               {
                   Retornar = "0";
               }
               else
               {
                   Retornar = componentId.Count().ToString();
               }
               return Retornar;
           }
           catch (Exception)
           {

               throw;
           }

       }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontograma1(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)

                                                                                     select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetCantidadAusentes(string pstrServiceId, string pstrComponentId, string pstrFieldId)
        {
            try
            {
                string retornar = "0";
                string[] componentId = null;
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

                componentId = xx.Split(';');

                if (componentId[0] == "")
                {
                    retornar = "0";
                }
                else
                {
                    retornar = componentId.Count().ToString();
                }
                return retornar;
            }
            catch (Exception)
            {

                throw;
            }

        }
       
        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontogramaValue1(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)

                                                                                     select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponente(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            int rpta = 0;

            try
            {
                var serviceComponentFieldValues = (from A in dbContext.service
                                                   join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                   join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                   join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                   join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
                                                   join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                                   join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                                   join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId

                                                   where A.v_ServiceId == pstrServiceId
                                                           && H.v_ComponentId == pstrComponentId
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                   {
                                                       v_ComponentFieldId = G.v_ComponentFieldId,
                                                       v_ComponentFielName = G.v_TextLabel,
                                                       v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                       v_Value1 = D.v_Value1,
                                                       i_GroupId = G.i_GroupId.Value
                                                   });

                var finalQuery = (from a in serviceComponentFieldValues.ToList()

                                  let value1 = int.TryParse(a.v_Value1, out rpta)
                                  join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                  equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                  from sp in sp_join.DefaultIfEmpty()

                                  select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                  {
                                      v_ComponentFieldId = a.v_ComponentFieldId,
                                      v_ComponentFielName = a.v_ComponentFielName,
                                      v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                      v_Value1 = a.v_Value1,
                                      v_Value1Name = sp == null ? "" : sp.v_Value1
                                  }).ToList();


                return finalQuery;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresExamenComponete(string pstrServiceId, string pstrComponentId, int pintParameter)
        {

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var systemParameters = (from a in dbContext.systemparameter
                                    where a.i_GroupId == pintParameter
                                    select a);


            List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                 join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                 join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                 join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                                                 join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
                                                                                 join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                                                                 join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                                                                 join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId
                                                                                 where A.v_ServiceId == pstrServiceId
                                                                                         && H.v_ComponentId == pstrComponentId
                                                                                         && B.i_IsDeleted == 0
                                                                                         && C.i_IsDeleted == 0

                                                                                                            select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                 {
                                                                                     v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                     v_ComponentFielName = G.v_TextLabel,
                                                                                     v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                     v_Value1 = D.v_Value1

                                                                                 }).ToList();


            var sql = (from A in serviceComponentFieldValues
                       join F in systemParameters on A.v_Value1 equals F.i_ParameterId.ToString() into F_join
                       from F in F_join.DefaultIfEmpty()
                       select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                       {
                           v_ComponentFieldId = A.v_ComponentFieldId,
                           v_ComponentFielName = A.v_ComponentFielName,
                           v_ServiceComponentFieldValuesId = A.v_ServiceComponentFieldValuesId,
                           v_ComponentFieldValuesId = A.v_ComponentFieldValuesId,
                           v_ServiceComponentFieldsId = A.v_ServiceComponentFieldsId,
                           v_Value1 = A.v_Value1,
                           v_Value2 = A.v_Value2,
                           i_Index = A.i_Index,
                           i_Value1 = A.i_Value1,
                           v_Value1Name = F == null ? "" : F.v_Value1
                       }).ToList();

            return sql;
        }

        public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetServiceDisgnosticsReports(string pstrServiceId)
        {
            //mon.IsActive = true;
            var isDeleted = 0;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> query = (from ccc in dbContext.diagnosticrepository
                                                        //join bbb in dbContext.servicecomponent on ccc.v_ServiceId equals bbb.v_ServiceId

                                                        join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos

                                                        join eee in dbContext.systemparameter on new { a = ccc.i_AutoManualId.Value, b = 136 } // Auto / Manual
                                                                                                equals new { a = eee.i_ParameterId, b = eee.i_GroupId }

                                                        join fff in dbContext.systemparameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                                            equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                                        from fff in J5_join.DefaultIfEmpty()

                                                        join ggg in dbContext.systemparameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                                            equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                                                        from ggg in J4_join.DefaultIfEmpty()

                                                        join hhh in dbContext.systemparameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                                                equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                                                        from hhh in J3_join.DefaultIfEmpty()

                                                        join iii in dbContext.systemparameter on new { a = ccc.i_IsSentToAntecedent.Value, b = 111 } // RESPUESTA SI/NO
                                                                                             equals new { a = iii.i_ParameterId, b = iii.i_GroupId } into J6_join
                                                        from iii in J6_join.DefaultIfEmpty()

                                                        join J1 in dbContext.systemuser on new { i_InsertUserId = ccc.i_InsertUserId.Value }
                                                                        equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                        from J1 in J1_join.DefaultIfEmpty()

                                                        join J2 in dbContext.systemuser on new { i_UpdateUserId = ccc.i_UpdateUserId.Value }
                                                                                        equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                        from J2 in J2_join.DefaultIfEmpty()

                                                        where (ccc.v_ServiceId == pstrServiceId) &&
                                                              (ccc.i_IsDeleted == isDeleted)
                                                        select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                                                        {
                                                            v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                                            v_ServiceId = ccc.v_ServiceId,
                                                            v_ComponentId = ccc.v_ComponentId,
                                                            //v_ComponentName = bbb.v_Name,
                                                            v_DiseasesId = ccc.v_DiseasesId,
                                                            i_AutoManualId = ccc.i_AutoManualId,
                                                            i_PreQualificationId = ccc.i_PreQualificationId,
                                                            i_FinalQualificationId = ccc.i_FinalQualificationId, //sirve
                                                            i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,//sirve
                                                            i_IsSentToAntecedent = ccc.i_IsSentToAntecedent,
                                                            d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,
                                                            i_GenerateMedicalBreak = ccc.i_GenerateMedicalBreak,
                                                            v_ComponentFieldsId = ccc.v_ComponentFieldId,
                                                            v_DiseasesName = ddd.v_Name,
                                                            v_Cie10 = ddd.v_CIE10Id,
                                                            v_AutoManualName = eee.v_Value1,

                                                            v_PreQualificationName = fff.v_Value1,
                                                            v_FinalQualificationName = ggg.v_Value1,
                                                            v_DiagnosticTypeName = hhh.v_Value1,
                                                            v_IsSentToAntecedentName = iii.v_Value1,
                                                            i_RecordStatus = (int)RecordStatus.Grabado,
                                                            i_RecordType = (int)RecordType.NoTemporal,

                                                            v_CreationUser = J1.v_UserName,
                                                            v_UpdateUser = J2.v_UserName,
                                                            d_CreationDate = J1.d_InsertDate,
                                                            d_UpdateDate = J2.d_UpdateDate,
                                                            i_IsDeleted = ccc.i_IsDeleted.Value
                                                        }).ToList();


                var q = new List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList>();
                q = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                {
                    i_Item = index + 1,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_ComponentName = a.v_ComponentName,
                    v_DiseasesId = a.v_DiseasesId,
                    i_AutoManualId = a.i_AutoManualId,
                    i_PreQualificationId = a.i_PreQualificationId,
                    i_FinalQualificationId = a.i_FinalQualificationId,
                    i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                    i_IsSentToAntecedent = a.i_IsSentToAntecedent,
                    d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,
                    i_GenerateMedicalBreak = a.i_GenerateMedicalBreak,
                    v_ComponentFieldsId = a.v_ComponentFieldsId,

                    v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                    v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                    v_DiseasesName = a.v_DiseasesName,
                    v_Cie10 = a.v_Cie10,
                    v_AutoManualName = a.v_AutoManualName,

                    v_PreQualificationName = a.v_PreQualificationName,
                    v_FinalQualificationName = a.v_FinalQualificationName,
                    v_DiagnosticTypeName = a.v_DiagnosticTypeName,
                    v_IsSentToAntecedentName = a.v_IsSentToAntecedentName,
                    i_RecordStatus = a.i_RecordStatus,
                    i_RecordType = a.i_RecordType,

                    v_CreationUser = a.v_CreationUser,
                    v_UpdateUser = a.v_UpdateUser,
                    d_CreationDate = a.d_CreationDate,
                    d_UpdateDate = a.d_UpdateDate,
                    i_IsDeleted = a.i_IsDeleted

                }).ToList();

                // Agregamos Restricciones / Recomendaciones
                OperationResult objOperationResult = new OperationResult();

                foreach (Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList dr in q)
                {
                    dr.Restrictions = GetServiceRestrictionsByDiagnosticRepositoryId(ref objOperationResult, dr.v_DiagnosticRepositoryId);
                    dr.Recomendations = GetServiceRecommendationByDiagnosticRepositoryId(ref objOperationResult, dr.v_DiagnosticRepositoryId);
                }

                return q;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RestrictionList> GetServiceRestrictionsByDiagnosticRepositoryId(ref OperationResult pobjOperationResult, string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RestrictionList> query = (from ddd in dbContext.restriction  // RESTRICCIONES 
                                               join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId // RESTRICIONES

                                               join J1 in dbContext.systemuser on new { i_InsertUserId = ddd.i_InsertUserId.Value }
                                                               equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                               from J1 in J1_join.DefaultIfEmpty()

                                               join J2 in dbContext.systemuser on new { i_UpdateUserId = ddd.i_UpdateUserId.Value }
                                                                               equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                               from J2 in J2_join.DefaultIfEmpty()

                                               where ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                                                     ddd.i_IsDeleted == 0
                                                                          select new Sigesoft.Node.WinClient.BE.RestrictionList
                                               {
                                                   //v_RestrictionByDiagnosticId = ddd.v_RestrictionByDiagnosticId,
                                                   v_RestrictionByDiagnosticId = ddd.v_RestrictionId,
                                                   v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                   v_ServiceId = ddd.v_ServiceId,
                                                   v_ComponentId = ddd.v_ComponentId,
                                                   v_MasterRestrictionId = ddd.v_MasterRestrictionId,
                                                   v_RestrictionName = eee.v_Name,
                                                   i_RecordStatus = (int)RecordStatus.Grabado,
                                                   i_RecordType = (int)RecordType.NoTemporal,
                                                   v_CreationUser = J1.v_UserName,
                                                   v_UpdateUser = J2.v_UserName,
                                                   d_CreationDate = J1.d_InsertDate,
                                                   d_UpdateDate = J2.d_UpdateDate,
                                                   i_IsDeleted = ddd.i_IsDeleted.Value
                                               }).ToList();



                //List<DiagnosticRepositoryList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RecomendationList> GetServiceRecommendationByDiagnosticRepositoryId(ref OperationResult pobjOperationResult, string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RecomendationList> query = (from ddd in dbContext.recommendation
                                                 join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                  

                                                 join J1 in dbContext.systemuser on new { i_InsertUserId = ddd.i_InsertUserId.Value }
                                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                 from J1 in J1_join.DefaultIfEmpty()

                                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = ddd.i_UpdateUserId.Value }
                                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                 from J2 in J2_join.DefaultIfEmpty()

                                                 where ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                                                       ddd.i_IsDeleted == 0
                                                select new Sigesoft.Node.WinClient.BE.RecomendationList
                                                 {
                                                     v_RecommendationId = ddd.v_RecommendationId,
                                                     v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                     v_ServiceId = ddd.v_ServiceId,
                                                     v_ComponentId = ddd.v_ComponentId,
                                                     v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                     v_RecommendationName = eee.v_Name,
                                                     i_RecordStatus = (int)RecordStatus.Grabado,
                                                     i_RecordType = (int)RecordType.NoTemporal,
                                                     v_CreationUser = J1.v_UserName,
                                                     v_UpdateUser = J2.v_UserName,
                                                     d_CreationDate = J1.d_InsertDate,
                                                     d_UpdateDate = J2.d_UpdateDate,
                                                     i_IsDeleted = ddd.i_IsDeleted.Value
                                                 }).ToList();



                //List<DiagnosticRepositoryList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RecomendationList> GetServiceRecommendationByServiceId(string pstrServiceId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RecomendationList> query = (from ddd in dbContext.recommendation
                                                 join fff in dbContext.diagnosticrepository on ddd.v_DiagnosticRepositoryId
                                                                                 equals fff.v_DiagnosticRepositoryId into J7_join
                                                 from fff in J7_join.DefaultIfEmpty()

                                                 join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId  // RECOMENDACIONES                                                                                                                                                                                                                                                         

                                                 join J1 in dbContext.systemuser on new { i_InsertUserId = ddd.i_InsertUserId.Value }
                                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                 from J1 in J1_join.DefaultIfEmpty()

                                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = ddd.i_UpdateUserId.Value }
                                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                 from J2 in J2_join.DefaultIfEmpty()
                                                 where (ddd.v_ServiceId == pstrServiceId) &&
                                                 (ddd.i_IsDeleted == 0) &&
                                                 (fff.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                                                 fff.i_FinalQualificationId == (int)FinalQualification.Presuntivo)
                                                                            select new Sigesoft.Node.WinClient.BE.RecomendationList
                                                 {
                                                     v_RecommendationId = ddd.v_RecommendationId,
                                                     v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                     v_ServiceId = ddd.v_ServiceId,
                                                     v_ComponentId = ddd.v_ComponentId,
                                                     v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                     v_RecommendationName = eee.v_Name,
                                                     i_RecordStatus = (int)RecordStatus.Grabado,
                                                     i_RecordType = (int)RecordType.NoTemporal
                                                 }).ToList();

                var query1 = new List<Sigesoft.Node.WinClient.BE.RecomendationList>();

                query1 = query.Select((x, index) => new Sigesoft.Node.WinClient.BE.RecomendationList
                {
                    i_Item = index + 1,
                    v_RecommendationId = x.v_RecommendationId,
                    v_DiagnosticRepositoryId = x.v_DiagnosticRepositoryId,
                    v_ServiceId = x.v_ServiceId,
                    v_ComponentId = x.v_ComponentId,
                    v_MasterRecommendationId = x.v_MasterRecommendationId,
                    v_RecommendationName = x.v_RecommendationName,
                    i_RecordStatus = x.i_RecordStatus,
                    i_RecordType = x.i_RecordType
                }).ToList();

                return query1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
              
        public List<ServiceComponentList> GetServiceComponentsForManagementReport(string pstrServiceId)
        {
            //mon.IsActive = true;        
            var isDeleted = 0;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var components = (from aaa in dbContext.servicecomponent
                                  join bbb in dbContext.component on aaa.v_ComponentId equals bbb.v_ComponentId
                                  join J1 in dbContext.systemuser on new { i_InsertUserId = aaa.i_InsertUserId.Value }
                                                  equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                  from J1 in J1_join.DefaultIfEmpty()

                                  join J2 in dbContext.systemuser on new { i_UpdateUserId = aaa.i_UpdateUserId.Value }
                                                                  equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                  from J2 in J2_join.DefaultIfEmpty()

                                  join fff in dbContext.systemparameter on new { a = bbb.i_CategoryId.Value, b = 116 } // CATEGORIA DEL EXAMEN
                                                                               equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                  from fff in J5_join.DefaultIfEmpty()

                                  where (aaa.v_ServiceId == pstrServiceId) &&
                                        (bbb.i_ComponentTypeId == (int?)ComponentType.Examen) &&
                                        (aaa.i_IsDeleted == isDeleted) &&
                                        (aaa.i_IsRequiredId == (int?)SiNo.SI)
                                  orderby bbb.i_CategoryId, bbb.v_Name

                                  select new ServiceComponentList
                                  {
                                      v_ComponentId = bbb.v_ComponentId,
                                      v_ComponentName = bbb.v_Name,
                                      v_ServiceComponentId = aaa.v_ServiceComponentId,
                                      i_CategoryId = bbb.i_CategoryId.Value,
                                      v_CategoryName = fff.v_Value1

                                  }).ToList();

                return components;
            }
            catch (Exception)
            {
                throw;
            }
        }

    
        public List<Sigesoft.Node.WinClient.BE.ServiceList> ReportFuncionesVitales(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId

                                 //join G in dbContext.professional on new { a = F.v_PersonId }
                                 //                                     equals new { a = G.v_PersonId } into G_join
                                 //from G in G_join.DefaultIfEmpty()

                                 //join H in dbContext.person on F.v_PersonId equals H.v_PersonId

                                 //join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 //join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DireccionPaciente = B.v_AdressLocation,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber


                                 });

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {

                               FC = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_FREC_CARDIACA_ID, "NOCOMBO", 0, "SI"),
                               PA = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_PAS_ID, "NOCOMBO", 0, "SI"),
                               FR = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_FREC_RESPIRATORIA_ID, "NOCOMBO", 0, "SI"),
                               //IMC = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_im_ID, "NOCOMBO", 0, "SI"),
                               Sat = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_SAT_O2_ID, "NOCOMBO", 0, "SI"),
                               PAD = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_PAD_ID, "NOCOMBO", 0, "SI")

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceList> ReportAntropometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId

                                 //join G in dbContext.professional on new { a = F.v_PersonId }
                                 //                                     equals new { a = G.v_PersonId } into G_join
                                 //from G in G_join.DefaultIfEmpty()

                                 //join H in dbContext.person on F.v_PersonId equals H.v_PersonId

                                 //join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 //join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DireccionPaciente = B.v_AdressLocation,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber


                                 }).ToList();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               IMC = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ANTROPOMETRIA_IMC_ID, "NOCOMBO", 0, "SI"),
                               Peso = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ANTROPOMETRIA_PESO_ID, "NOCOMBO", 0, "SI"),
                               talla = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ANTROPOMETRIA_TALLA_ID, "NOCOMBO", 0, "SI"),
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RestrictionList> GetServiceRestrictionByDiagnosticRepositoryIdReport(string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RestrictionList> query = (from ddd in dbContext.restriction
                                               join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRestrictionId
                                                                       equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                                                              
                                               where (ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId) &&
                                                     (ddd.i_IsDeleted == 0)
                                                select new Sigesoft.Node.WinClient.BE.RestrictionList
                                               {
                                                   v_RestrictionId = ddd.v_RestrictionId,
                                                   v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                   v_ServiceId = ddd.v_ServiceId,
                                                   v_ComponentId = ddd.v_ComponentId,
                                                   v_MasterRestrictionId = ddd.v_MasterRestrictionId,
                                                   v_RestrictionName = eee.v_Name,

                                               }).ToList();

                // add the sequence number on the fly
                var finalQuery = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.RestrictionList
                {
                    i_Item = index + 1,
                    v_RestrictionId = a.v_RestrictionId,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_MasterRestrictionId = a.v_MasterRestrictionId,
                    v_RestrictionName = a.v_RestrictionName,
                }).ToList();

                return finalQuery;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportOsteoMuscular> ReportOsteoMuscular(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 //**********************************************************************************************

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId

                                 join L in dbContext.systemparameter on new { a = I.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = L.i_ParameterId, b = L.i_GroupId } into L_join
                                 from L in L_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.ReportOsteoMuscular
                                 {
                                     IdServicio = A.v_ServiceId,
                                     IdSericioComponente = E.v_ServiceComponentId,
                                     Paciente = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Puesto = B.v_CurrentOccupation,
                                     Protocolo = I.v_Name,
                                     Empresa = J.v_Name,
                                     TipoExamen = L.v_Value1,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaMedico = pme.b_SignatureImage,
                                     d_BirthDate = B.d_Birthdate.Value,
                                     HuellaTrabajador = B.b_FingerPrintImage
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let OsteoMuscular = new ServiceBL().ValoresComponente(pstrserviceId, pstrComponentId)
                           select new Sigesoft.Node.WinClient.BE.ReportOsteoMuscular
                           {
                               IdServicio = a.IdServicio,
                               IdSericioComponente = a.IdSericioComponente,
                               Paciente = a.Paciente,
                               Puesto = a.Puesto,
                               Protocolo = a.Protocolo,
                               Empresa = a.Empresa,
                               TipoExamen = a.TipoExamen,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaMedico = a.FirmaMedico,
                               d_BirthDate = a.d_BirthDate,
                               Edad = GetAge(a.d_BirthDate.Value),

                               MetodoCarga = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_METODO_CARGA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_METODO_CARGA).v_Value1Name,
                               AntecedentesSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PRESENTA_ANTECEDENTES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PRESENTA_ANTECEDENTES).v_Value1,
                               AntecedentesDescripcion = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION).v_Value1,
                               PosturaSentado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_SENTADO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_SENTADO).v_Value1,
                               PosturaPie = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_PIE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_PIE).v_Value1,
                               PosturaForzadaSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_FORZADA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_FORZADA).v_Value1,
                               MovCargaManualSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MOVIMIENTO_MANUAL_CARGA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MOVIMIENTO_MANUAL_CARGA).v_Value1,
                               PesoCarga = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PESO_CARGA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PESO_CARGA).v_Value1,
                               MovRepetitivosSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MOVIMIENTOS_REPETITIVOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MOVIMIENTOS_REPETITIVOS).v_Value1,
                               UsuPantallaPVDSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_USUARIO_PANTALLA_PVD) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_USUARIO_PANTALLA_PVD).v_Value1,
                               HorasDia = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HORAS_DIA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HORAS_DIA).v_Value1,
                               LordisisCervical = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_CERVICAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_CERVICAL).v_Value1Name,
                               CifosisDorsal = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CIFOSIS_DORSAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CIFOSIS_DORSAL).v_Value1Name,
                               LordosisLumbar = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_LUMBAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_LUMBAR).v_Value1Name,
                               EscoliosisLumbar = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_LUMBAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_LUMBAR).v_Value1Name,
                               EscofiosisDorsal = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_DORSAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_DORSAL).v_Value1Name,
                               //DolorEspalda = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DOLOR_ESPALDA) == null? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DOLOR_ESPALDA).v_Value1Name,
                               ContracturaMuscular = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUSCULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUSCULAR).v_Value1Name,
                               Observaciones = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_OBSERVACIONES).v_Value1,
                               RodillaDerechaVaroSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DERECHA_VARO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DERECHA_VARO).v_Value1,
                               RodillaDerechaValgoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DERECHA_VALGO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DERECHA_VALGO).v_Value1,
                               RodillaIzquierdaVaroSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_IZQUIERDA_VARO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_IZQUIERDA_VARO).v_Value1,
                               RodillaIzquierdaValgoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_IZQUIERDA_VALGO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_IZQUIERDA_VALGO).v_Value1,
                               PieDerechoCavoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_DERECHO_CAVO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_DERECHO_CAVO).v_Value1,
                               PieDerechoPlanoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_DERECHO_PLANO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_DERECHO_PLANO).v_Value1,
                               PieIzquierdoCavoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_IZQUIERDO_CAVO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_IZQUIERDO_CAVO).v_Value1,

                               PieIzquierdoPlanoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_IZQUIERDO_PLANO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_IZQUIERDO_PLANO).v_Value1,
                               ReflejoTotulianoDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_DERECHO).v_Value1Name,
                               ReflejoTotulianoIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_IZQUIERDO).v_Value1Name,
                               ReflejoAquileoDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_AQUILEO_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_AQUILEO_DERECHO).v_Value1Name,
                               ReflejoAquileoIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_AQUILEO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_AQUILEO_IZQUIERDO).v_Value1Name,
                               TestPhalenDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_PHALEN_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_PHALEN_DERECHO).v_Value1,
                               TestPhalenIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_PHALEN_IZQUIERDA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_PHALEN_IZQUIERDA).v_Value1,
                               TestTinelDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_TINEL_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_TINEL_DERECHO).v_Value1,
                               TestTinelIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_TINEL_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEST_TINEL_IZQUIERDO).v_Value1,
                               SignoLasagueIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_LASAGUE_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_LASAGUE_IZQUIERDO).v_Value1,
                               SignoLasagueDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_LASAGUE_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_LASAGUE_DERECHO).v_Value1,
                               SignoBragardIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_BRAGARD_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_BRAGARD_IZQUIERDO).v_Value1,
                               SignoBragardDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_BRAGARD_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_SIGNO_BRAGARD_DERECHO).v_Value1,

                               TemporoMadibularNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEMPERO_MANDIBULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEMPERO_MANDIBULAR).v_Value1,
                               TemporoMadibularObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEMPERO_MANDIBULAR_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEMPERO_MANDIBULAR_DESCRIPCION).v_Value1,
                               HombroNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HOMBRO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HOMBRO).v_Value1,
                               HombroObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HOMBRO_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HOMBRO_DESCRIPCION).v_Value1,
                               CodoNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CODO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CODO).v_Value1,
                               CodoObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CODO_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CODO_DESCRIPCION).v_Value1,
                               MunecaNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUNECA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUNECA).v_Value1,
                               MunecaObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUNECA_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUNECA_DESCRIPCION).v_Value1,
                               InterfalangicaNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_INTERFALANGICAS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_INTERFALANGICAS).v_Value1,
                               InterfalangicaObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_INTERFALANGICAS_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_INTERFALANGICAS_DESCRIPCION).v_Value1,
                               CoxoFermoralNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COXOFEMORAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COXOFEMORAL).v_Value1,
                               CoxoFermoralObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COXOFEMORAL_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COXOFEMORAL_DESCRIPCION).v_Value1,
                               RodillaNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA).v_Value1,
                               RodillaObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DESCRIPCION).v_Value1,
                               TobilloPieNID = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOBILLO_PIE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOBILLO_PIE).v_Value1,
                               TobilloPieObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOBILLO_PIE_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOBILLO_PIE_DESCRIPCION).v_Value1,

                               ColumnaCervicalSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_CERVICAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_CERVICAL).v_Value1,
                               ColumnaCervicalObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_CERVICAL_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_CERVICAL_DESCRIPCION).v_Value1,
                               ColumnaDorsalSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSAL).v_Value1,
                               ColumnaDorsalObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSAL_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSAL_DESCRIPCION).v_Value1,
                               DorsoLumbarSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSO_LUMBAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSO_LUMBAR).v_Value1,
                               DorsoLumbarObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSO_LUMBAR_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSO_LUMBAR_DESCRIPCION).v_Value1,
                               LumbroSacraSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LUMBOSACRA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LUMBOSACRA).v_Value1,
                               LumbroSacraObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LUMBOSACRA_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LUMBOSACRA_DESCRIPCION).v_Value1,
                               CondralesSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_CONDRALES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_CONDRALES).v_Value1,
                               CondralesObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_CONDRALES_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_CONDRALES_DESCRIPCION).v_Value1,
                               CostoEsternalesSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_ESTERNALES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_ESTERNALES).v_Value1,
                               CostoEsternalesObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_ESTERNALES_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_ESTERNALES_DESCRIPCION).v_Value1,

                               Descripcion = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID).v_Value1,
                               Aptitud = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_2_APTITUD_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_2_APTITUD_ID).v_Value1Name,
                               Hallazgos = GetDiagnosticByServiceIdAndComponent(a.IdServicio, Constants.OSTEO_MUSCULAR_ID_1),
                               Recomendacion = GetRecommendationByServiceIdAndComponent(a.IdServicio, Constants.OSTEO_MUSCULAR_ID_1),
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetDiagnosticByServiceIdAndComponent(string pstrServiceId, string pstrComponent)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.diagnosticrepository
                         join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                         from ddd in ddd_join.DefaultIfEmpty()

                         join eee in dbContext.recommendation on new { a = pstrServiceId, b = pstrComponent }
                                                                    equals new { a = eee.v_ServiceId, b = eee.v_ComponentId } into eee_join
                         from eee in eee_join.DefaultIfEmpty()

                         join fff in dbContext.masterrecommendationrestricction on eee.v_MasterRecommendationId
                                                                equals fff.v_MasterRecommendationRestricctionId into fff_join
                         from fff in fff_join.DefaultIfEmpty()

                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name
                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_DiseasesName));
        }


        private string GetRecommendationByServiceIdAndComponent(string pstrServiceId, string pstrComponent)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.recommendation
                         join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId  // Diagnosticos      
                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_Recommendation = ddd.v_Name

                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_Recommendation));
        }

        public List<Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList> ReportHistoriaOcupacionalAudiometria(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var isDeleted = 0;

                var ruido = (int)PeligrosEnElPuesto.Ruido;

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join D in dbContext.history on B.v_PersonId equals D.v_PersonId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join wd in dbContext.workstationdangers on D.v_HistoryId equals wd.v_HistoryId

                                 //*****
                                 join ter in dbContext.systemparameter on new { a = wd.i_NoiseSource.Value, b = (int)SystemParameterGroups.TiempoExpsosicionRuido } // Tiempo exp. al ruido
                                               equals new { a = ter.i_ParameterId, b = ter.i_GroupId } into ter_join
                                 from ter in ter_join.DefaultIfEmpty()

                                 join nr in dbContext.systemparameter on new { a = wd.i_NoiseLevel.Value, b = (int)SystemParameterGroups.NivelRuido } // Nivel de ruido
                                              equals new { a = nr.i_ParameterId, b = nr.i_GroupId } into nr_join
                                 from nr in nr_join.DefaultIfEmpty()
                                 //************

                                 join pro in dbContext.protocol on A.v_ProtocolId equals pro.v_ProtocolId

                                 // Empresa / Sede Trabajo  ********************************************************
                                 join ow in dbContext.organization on new { a = pro.v_WorkingOrganizationId }
                                         equals new { a = ow.v_OrganizationId } into ow_join
                                 from ow in ow_join.DefaultIfEmpty()

                                 join lw in dbContext.location on new { a = pro.v_WorkingOrganizationId, b = pro.v_WorkingLocationId }
                                      equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                                 from lw in lw_join.DefaultIfEmpty()

                                 //************************************************************************************

                                 where (A.v_ServiceId == pstrserviceId) &&
                                       (D.i_IsDeleted == isDeleted) &&
                                       (wd.i_DangerId == ruido)

                                 select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                                 {
                                     IdHistory = D.v_HistoryId,
                                     //Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     IdServicio = A.v_ServiceId,
                                     FNacimiento = B.d_Birthdate,
                                     Genero = B.i_SexTypeId.Value,
                                     LugarNacimiento = B.v_BirthPlace,

                                     Puesto = B.v_CurrentOccupation,
                                     FechaInicio = D.d_StartDate,
                                     FechaFin = D.d_EndDate,
                                     Empresa = D.v_Organization,
                                     Altitud = D.i_GeografixcaHeight.Value,
                                     AreaTrabajo = D.v_TypeActivity,
                                     PuestoTrabajo = D.v_workstation,

                                     FuenteRuidoName = wd.v_TimeOfExposureToNoise,
                                     NivelRuidoName = nr.v_Value1,
                                     TiempoExpoRuidoName = ter.v_Value1,
                                     v_PersonId = B.v_PersonId,
                                     //
                                     v_FullPersonName = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     v_WorkingOrganizationName = ow.v_Name,
                                     v_FullWorkingOrganizationName = ow.v_Name + " / " + lw.v_Name,
                                     NroDocumento = B.v_DocNumber,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let date1 = a.FechaInicio == null ? "" : a.FechaInicio.Value.ToString("MMMM / yyyy")
                           let date2 = a.FechaFin == null ? "" : a.FechaFin.Value.ToString("MMMM / yyyy")

                           select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                           {
                               IdHistory = a.IdHistory,
                               //Trabajador = a.Trabajador,
                               IdServicio = a.IdServicio,
                               FechaNacimiento = a.FNacimiento == null ? "" : a.FNacimiento.Value.ToString("dd/MM/yyyy"),
                               Genero = a.Genero,
                               LugarNacimiento = a.LugarNacimiento,

                               Puesto = a.Puesto,
                               FechaInicio = a.FechaInicio,
                               FechaFin = a.FechaFin,
                               Fechas = "Fecha Ini. \n" + date1 + "\n" + "Fecha Fin. \n" + date2,
                               Empresa = a.Empresa,

                               AreaTrabajo = a.AreaTrabajo,
                               PuestoTrabajo = a.PuestoTrabajo,

                               //Peligros = ConcatenateExposiciones(a.IdHistory),
                               Epp = ConcatenateEppsAndPercentage(a.IdHistory),

                               FuenteRuidoName = a.FuenteRuidoName,
                               NivelRuidoName = a.NivelRuidoName,
                               TiempoExpoRuidoName = a.TiempoExpoRuidoName,
                               v_PersonId = a.v_PersonId,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               //
                               v_FullPersonName = a.v_FullPersonName,
                               v_WorkingOrganizationName = a.v_WorkingOrganizationName,
                               v_FullWorkingOrganizationName = a.v_FullWorkingOrganizationName,
                               NroDocumento = a.NroDocumento,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ConcatenateEppsAndPercentage(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            int[] tipoEPPRuido = { (int)TipoEPP.Orejeras, 
                                   (int)TipoEPP.TaponesAuditivosEspuma,
                                   (int)TipoEPP.TaponesAuditivosSilicona 
                                 };

            var qry = (from a in dbContext.typeofeep
                       join C1 in dbContext.systemparameter on new { a = a.i_TypeofEEPId.Value, b = 146 }
                                                                equals new { a = C1.i_ParameterId, b = C1.i_GroupId }
                       where a.v_HistoryId == pstrHistoryId &&
                             a.i_IsDeleted == 0 &&
                             tipoEPPRuido.Contains(a.i_TypeofEEPId.Value)
                       select new
                       {
                           v_Epps = C1.v_Value1,
                           r_Percentage = a.r_Percentage
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_Epps + " " + p.r_Percentage + " % "));

            //return string.Join(", ", qry.Select(p => 
            //    new { v_Epps = p.v_Epps, 
            //          r_Percentage = p.r_Percentage 

            //        }));

        }

        public List<Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList> ReportHistoriaOcupacional(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var groupUbigeo = 113;
                var isDeleted = 0;
                var exFisicoId = Constants.EXAMEN_FISICO_ID;
                var exFisico7C = Constants.EXAMEN_FISICO_7C_ID;

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join D in dbContext.history on B.v_PersonId equals D.v_PersonId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join sc in dbContext.servicecomponent on new { a = pstrserviceId, b = exFisicoId }
                                                                      equals new { a = sc.v_ServiceId, b = sc.v_ComponentId } into sc_join
                                 from sc in sc_join.DefaultIfEmpty()

                                 join E in dbContext.systemuser on sc.i_ApprovedUpdateUserId equals E.i_SystemUserId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join F in dbContext.professional on E.v_PersonId equals F.v_PersonId into F_join
                                 from F in F_join.DefaultIfEmpty()

                                 // Examen fisico 7C *******************************************************************
                                 join sc1 in dbContext.servicecomponent on new { a = pstrserviceId, b = exFisico7C }
                                                                    equals new { a = sc1.v_ServiceId, b = sc1.v_ComponentId } into sc1_join
                                 from sc1 in sc1_join.DefaultIfEmpty()

                                 join su7c in dbContext.systemuser on sc1.i_ApprovedUpdateUserId equals su7c.i_SystemUserId into su7c_join
                                 from su7c in su7c_join.DefaultIfEmpty()

                                 join p7c in dbContext.professional on su7c.v_PersonId equals p7c.v_PersonId into p7c_join
                                 from p7c in p7c_join.DefaultIfEmpty()

                                 //******************************************************************************

                                 // Ubigeo de la persona *******************************************************
                                 join dep in dbContext.datahierarchy on new { a = B.i_DepartmentId.Value, b = groupUbigeo }
                                                      equals new { a = dep.i_ItemId, b = dep.i_GroupId } into dep_join
                                 from dep in dep_join.DefaultIfEmpty()

                                 join prov in dbContext.datahierarchy on new { a = B.i_ProvinceId.Value, b = groupUbigeo }
                                                       equals new { a = prov.i_ItemId, b = prov.i_GroupId } into prov_join
                                 from prov in prov_join.DefaultIfEmpty()

                                 join distri in dbContext.datahierarchy on new { a = B.i_DistrictId.Value, b = groupUbigeo }
                                                       equals new { a = distri.i_ItemId, b = distri.i_GroupId } into distri_join
                                 from distri in distri_join.DefaultIfEmpty()
                                 //*********************************************************************************************

                                 let varDpto = dep.v_Value1 == null ? "" : dep.v_Value1
                                 let varProv = prov.v_Value1 == null ? "" : prov.v_Value1
                                 let varDistri = distri.v_Value1 == null ? "" : distri.v_Value1
                                 let del = D.i_IsDeleted == null ? 0 : D.i_IsDeleted

                                 where (A.v_ServiceId == pstrserviceId) &&
                                       (del == isDeleted)

                                 select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                                 {
                                     IdHistory = D.v_HistoryId,
                                     Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     IdServicio = A.v_ServiceId,
                                     FNacimiento = B.d_Birthdate,
                                     Genero = B.i_SexTypeId.Value,
                                     LugarNacimiento = B.v_BirthPlace,
                                     LugarProcedencia = varDistri + "-" + varProv + "-" + varDpto, // Santa Anita - Lima - Lima
                                     Puesto = B.v_CurrentOccupation,
                                     FechaInicio = D.d_StartDate,
                                     FechaFin = D.d_EndDate,
                                     Empresa = D.v_Organization,
                                     Altitud = D.i_GeografixcaHeight.Value,
                                     AreaTrabajo = D.v_TypeActivity,
                                     PuestoTrabajo = D.v_workstation,
                                     IdTipoOperacion = D.i_TypeOperationId.Value,
                                     Dia = A.d_ServiceDate.Value.Day,
                                     Mes = A.d_ServiceDate.Value.Month,
                                     Anio = A.d_ServiceDate.Value.Year,
                                     FirmaMedico = F.b_SignatureImage == null ? p7c.b_SignatureImage : F.b_SignatureImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let date1 = a.FechaInicio == null ? "" : a.FechaInicio.Value.ToString("MMMM / yyyy")
                           let date2 = a.FechaFin == null ? "" : a.FechaFin.Value.ToString("MMMM / yyyy")
                           let xxx = GetYearsAndMonth(a.FechaFin, a.FechaInicio)
                           select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                           {
                               IdHistory = a.IdHistory,
                               Trabajador = a.Trabajador,
                               IdServicio = a.IdServicio,
                               FechaNacimiento = a.FNacimiento == null ? "" : a.FNacimiento.Value.ToString("dd/MM/yyyy"),
                               Genero = a.Genero,
                               LugarNacimiento = a.LugarNacimiento,
                               LugarProcedencia = a.LugarProcedencia,
                               Puesto = a.Puesto,
                               FechaInicio = a.FechaInicio,
                               FechaFin = a.FechaFin,
                               Fechas = "Fecha Ini. \n" + date1 + "\n" + "Fecha Fin. \n" + date2,
                               Empresa = a.Empresa,
                               Altitud = a.Altitud,
                               AreaTrabajo = a.AreaTrabajo,
                               PuestoTrabajo = a.PuestoTrabajo,
                               IdTipoOperacion = a.IdTipoOperacion,
                               TiempoLabor = xxx,
                               Dia = a.Dia,
                               Mes = a.Mes,
                               Anio = a.Anio,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               Peligros = ConcatenateExposiciones(a.IdHistory),
                               Epp = ConcatenateEpps(a.IdHistory),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ConcatenateExposiciones(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.workstationdangers
                       join B1 in dbContext.systemparameter on new { a = a.i_DangerId.Value, b = 145 } equals new { a = B1.i_ParameterId, b = B1.i_GroupId }
                       where a.v_HistoryId == pstrHistoryId &&
                       a.i_IsDeleted == 0
                       select new
                       {
                           v_Exposicion = B1.v_Value1
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_Exposicion));
        }

        private string ConcatenateEpps(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.typeofeep
                       join C1 in dbContext.systemparameter on new { a = a.i_TypeofEEPId.Value, b = 146 } equals new { a = C1.i_ParameterId, b = C1.i_GroupId }
                       where a.v_HistoryId == pstrHistoryId &&
                       a.i_IsDeleted == 0
                       select new
                       {
                           v_Epps = C1.v_Value1
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_Epps));
        }

        public string GetValueOdontograma1(string pstrServiceId, string pstrComponentId, string pstrFieldId)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

                return xx;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ReportEstudioElectrocardiografico> GetReportEstudioElectrocardiografico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()
                                 join F in dbContext.systemuser on E.i_InsertUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId

                                 join M in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                     equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()



                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************       

                                 join X in dbContext.person on me.v_PersonId equals X.v_PersonId
                                 join Y in dbContext.person on tec.v_PersonId equals Y.v_PersonId into Y_join
                                 from Y in Y_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEstudioElectrocardiografico
                                 {
                                     NroFicha = E.v_ServiceComponentId,
                                     NroHistoria = A.v_ServiceId,
                                     DatosPaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = M.v_Value1,
                                     FirmaMedico = pme.b_SignatureImage,
                                     FirmaTecnico = ptec.b_SignatureImage,
                                     Fecha = A.d_ServiceDate.Value,
                                     Empresa = D.v_Name,
                                     Puesto = B.v_CurrentOccupation,
                                     NombreDoctor = X.v_FirstLastName + " " + X.v_SecondLastName + " " + X.v_FirstName,
                                     NombreTecnologo = Y.v_FirstLastName + " " + Y.v_SecondLastName + " " + Y.v_FirstName,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportEstudioElectrocardiografico
                           {
                               NroFicha = a.NroFicha,
                               NroHistoria = a.NroHistoria,
                               DatosPaciente = a.DatosPaciente,
                               FechaNacimiento = a.FechaNacimiento,
                               Genero = a.Genero,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTecnico = a.FirmaTecnico,
                               Fecha = a.Fecha,
                               Empresa = a.Empresa,
                               Puesto = a.Puesto,
                               Edad = GetAge(a.FechaNacimiento),
                               NombreDoctor = a.NombreDoctor,
                               NombreTecnologo = a.NombreTecnologo,
                               SoploSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_SOPLO_CARDIACO_ID, "NOCOMBO", 0, "SI"),

                               CansancioSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_CANSANCIO_RAPIDO_ID, "NOCOMBO", 0, "SI"),

                               MareosSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_MAREOS_ID, "NOCOMBO", 0, "SI"),

                               PresionAltaSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_PRESION_ALTA_ID, "NOCOMBO", 0, "SI"),

                               DolorPrecordialSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DOLOR_PRECORDIAL_ID, "NOCOMBO", 0, "SI"),

                               PalpitacionesSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_PALPITACIONES_ID, "NOCOMBO", 0, "SI"),

                               AtaquesCorazonSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_ATAQUE_CORAZON_ID, "NOCOMBO", 0, "SI"),

                               PerdidaConcienciaSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_PERDIDA_CONCIENCIA_ID, "NOCOMBO", 0, "SI"),

                               ObesidadSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_OBESIDAD_ID, "NOCOMBO", 0, "SI"),

                               TabaquismoSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_TABAQUISMO_ID, "NOCOMBO", 0, "SI"),

                               DisplidemiaSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DISLIPIDEMIA_ID, "NOCOMBO", 0, "SI"),

                               DiabetesSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DIABETES_ID, "NOCOMBO", 0, "SI"),

                               SedentarismoSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_SEDENTARISMO_ID, "NOCOMBO", 0, "SI"),

                               Otros1 = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_OTROS1_ESPECIFICAR_ID, "NOCOMBO", 0, "SI"),

                               DolorPrecordial2SiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DOLOR_PRECORDIAL1_ID, "NOCOMBO", 0, "SI"),

                               DesmayosSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DESMAYOS_ID, "NOCOMBO", 0, "SI"),

                               Palpitaciones2SiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_PALPITACIONES2_ID, "NOCOMBO", 0, "SI"),

                               DisneaSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DISNEA_PAROXISTICA_ID, "NOCOMBO", 0, "SI"),

                               Otros2 = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_OTROS2_ID, "NOCOMBO", 0, "SI"),

                               Mareos2SiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_MAREOS1_ID, "NOCOMBO", 0, "SI"),

                               VaricesSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_VARICES_PIERNAS_ID, "NOCOMBO", 0, "SI"),

                               ClaudicacSiNo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_CLAUDICAC_INTERMITENTE_ID, "NOCOMBO", 0, "SI"),

                               Ritmo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_RITMO_SINUAL_ID, "NOCOMBO", 0, "SI"),

                               IntervaloPR = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_INTERVALO_PR_ID, "NOCOMBO", 0, "SI"),

                               IntervaloQRS = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_INTERVALO_QRS_ID, "NOCOMBO", 0, "SI"),

                               IntervaloQT = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_INTERVALO_QT_ID, "NOCOMBO", 0, "SI"),

                               OndaP = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_ONDA_P_ID, "SICOMBO", 221, "NO"),
                               OndaPAnormal = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_ONDA_P_ANORMAL_ID, "NOCOMBO", 0, "SI"),

                               OndaT = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_ONDA_T_ID, "SICOMBO", 221, "NO"),
                               OndaTAnormal = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_ONDA_T_ANORMAL_ID, "NOCOMBO", 0, "SI"),

                               ComplejoQRS = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_COMPLEJO_QRS_ID, "SICOMBO", 221, "NO"),
                               ComplejoQRSAnormal = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_COMPLEJO_QRS_ANORMAL_ID, "NOCOMBO", 0, "SI"),

                               SegmentoST = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_SEGMENTO_ST_ID, "SICOMBO", 221, "NO"),
                               SegementoSTAnormal = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_SEGMENTO_ST_ANORMAL_ID, "NOCOMBO", 0, "SI"),

                               TrasntornoRitmo = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_TRANSTORNOS_RITMO_ID, "NOCOMBO", 0, "SI"),

                               TranstornoConduccion = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_TRANSTORNOS_CONDUCCION_ID, "NOCOMBO", 0, "SI"),

                               Conclusiones = GetServiceComponentFielValue(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID, Constants.ELECTROCARDIOGRAMA_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),

                               Hallazgos = GetDiagnosticByServiceIdAndComponent(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID),

                               Recomendaciones = GetRecommendationByServiceIdAndComponent(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               //Hallazgos = GetDiagnosticByServiceId(a.IdServicio),
                               //Descripcion = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.OSTEO_MUSCULAR_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               //Recomendacion = GetRecommendationByServiceId(a.IdServicio),



                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alberto
        public List<Sigesoft.Node.WinClient.BE.ReportEsfuerzoFisico> GetReportPruebaEsfuerzo(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()
                                 join F in dbContext.systemuser on E.i_InsertUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId

                                 join M in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                     equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************       


                                 join X in dbContext.person on me.v_PersonId equals X.v_PersonId
                                 join Y in dbContext.person on tec.v_PersonId equals Y.v_PersonId into Y_join
                                 from Y in Y_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEsfuerzoFisico
                                 {
                                     Ficha = E.v_ComponentId,
                                     HistoriaClinica = A.v_ServiceId,
                                     DatoPaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = M.v_Value1,
                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     FirmaMedico = pme.b_SignatureImage,

                                     NombreDoctor = X.v_FirstLastName + " " + X.v_SecondLastName + " " + X.v_FirstName,
                                     NombreTecnico = Y.v_FirstLastName + " " + Y.v_SecondLastName + " " + Y.v_FirstName,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportEsfuerzoFisico
                           {
                               Ficha = a.Ficha,
                               HistoriaClinica = a.HistoriaClinica,
                               DatoPaciente = a.DatoPaciente,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Genero = a.Genero,
                               FirmaTecnologo = a.FirmaTecnologo,
                               FirmaMedico = a.FirmaMedico,

                               NombreDoctor = a.NombreDoctor,
                               NombreTecnico = a.NombreTecnico,


                               FumadorSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_FUMADOR_ID, "NOCOMBO", 0, "SI"),
                               DiabeticoSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DIABETICO_ID, "NOCOMBO", 0, "SI"),
                               InfartoSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_PRIOR_ID, "NOCOMBO", 0, "SI"),
                               FamiliarSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ANGINA_ID, "NOCOMBO", 0, "SI"),

                               PriorSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_INFARTO_ID, "NOCOMBO", 0, "SI"),
                               AnginaSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ANTECEDENTE_FAMILIAR_ID, "NOCOMBO", 0, "SI"),

                               PReposoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_INICIO_ID, "NOCOMBO", 0, "SI"),
                               PReposoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_DURACION_ID, "NOCOMBO", 0, "SI"),
                               PReposoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),
                               PReposoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_INCLINACION_ID, "NOCOMBO", 0, "SI"),
                               PReposoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_MTS_ID, "NOCOMBO", 0, "SI"),
                               PReposoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_FC_ID, "NOCOMBO", 0, "SI"),
                               PReposoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_PAS_ID, "NOCOMBO", 0, "SI"),
                               PReposoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),
                               PReposoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               PEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               SEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               TEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               CEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               PRecuperacionInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_INICIO_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_DURACION_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_MTS_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_FC_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_PAS_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionProtocolo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               SRecuperacionInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_INICIO_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_DURACION_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_MTS_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_FC_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_PAS_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               SReposoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               SReposoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               SReposoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               SReposoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               SReposoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_MTS_ID, "NOCOMBO", 0, "SI"),

                               SReposoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_FC_ID, "NOCOMBO", 0, "SI"),

                               SReposoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_PAS_ID, "NOCOMBO", 0, "SI"),

                               SReposoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               SReposoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),





                               //-----------------------------------------------------------------



                               TiempoEjercicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_TIEMPO_EJERCICIO_ID, "NOCOMBO", 0, "SI"),

                               CPV = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CPVS_ID, "NOCOMBO", 0, "SI"),

                               Derv = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DERIV_100_UVST_ID, "NOCOMBO", 0, "SI"),

                               Velocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               Pendiente = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_PENDIENTE_ID, "NOCOMBO", 0, "SI"),

                               METS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_METS_ID, "NOCOMBO", 0, "SI"),

                               FCardiaca = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_F_CARDIACA_ID, "NOCOMBO", 0, "SI"),

                               PSistolica = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_SISTOLICA_ID, "NOCOMBO", 0, "SI"),

                               PDiastolica = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DIASTOLICA_ID, "NOCOMBO", 0, "SI"),

                               FCxTA = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_FCXTA_ID, "NOCOMBO", 0, "SI"),

                               IndiceSTFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_INDICE_STFC_ID, "NOCOMBO", 0, "SI"),

                               Objetivo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_OBJETIVO_ID, "NOCOMBO", 0, "SI"),

                               ElevacionST = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ELEVACION_ST_ID, "NOCOMBO", 0, "SI"),

                               ElevacionSTEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ELEVACION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               ElevacionSTAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ELEVACION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               DepresionST = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DEPRESION_ST_ID, "NOCOMBO", 0, "SI"),

                               DepresionSTEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DEPRESION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               DepresionSTAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DEPRESION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               CambioElevacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_ELEVACION_ST_ID, "NOCOMBO", 0, "SI"),

                               CambioElevacionEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_ELEVACION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               CambioElevacionAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_ELEVACION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               CambioDepresion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_DEPRESION_ST_ID, "NOCOMBO", 0, "SI"),

                               CambioDepresionEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_DEPRESION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               CambioDepresionAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_DEPRESION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               Razones = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_RAZONES_FINALIZAR_ID, "NOCOMBO", 0, "SI"),

                               Conclusiones = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CONCLUSIONES_ID, "NOCOMBO", 0, "SI"),

                               Sintomas = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_SINTOMAS_ID, "NOCOMBO", 0, "SI"),



                               Dx = GetDiagnosticByServiceIdAndComponent(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetServiceComponentFielValue(string pstrServiceId, string pstrComponentId, string pstrFieldId, string Type, int pintParameter, string pstrConX)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                string xx = "";
                if (Type == "NOCOMBO")
                {
                    oServiceComponentFieldValuesList = oServiceBL.ValoresComponente(pstrServiceId, pstrComponentId);
                    xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                }
                else
                {
                    oServiceComponentFieldValuesList = oServiceBL.ValoresExamenComponete(pstrServiceId, pstrComponentId, pintParameter);
                    if (pstrConX == "SI")
                    {
                        xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                    }
                    else
                    {
                        xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1Name;
                    }

                }

                return xx;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportOdontograma> ReportOdontograma(string pstrserviceId, string pstrComponentId, string Path)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 //**************************************************************************************

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportOdontograma
                                 {
                                     IdServicio = A.v_ServiceId,
                                     Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Fecha = A.d_ServiceDate.Value,
                                     Puesto = B.v_CurrentOccupation,
                                     Ficha = E.v_ServiceComponentId,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = J.v_Name

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportOdontograma
                           {
                               IdServicio = a.IdServicio,
                               Trabajador = a.Trabajador,
                               Fecha = a.Fecha,
                               Puesto = a.Puesto,
                               Ficha = a.Ficha,
                               FirmaMedico = a.FirmaMedico,
                               Empresa = a.Empresa,
                               Tabaco = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_TABACO_ID, "NOCOMBO", 0, "SI"),
                               Diabetes = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_DIABETES_ID, "NOCOMBO", 0, "SI"),
                               Tbc = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_TBC_ID, "NOCOMBO", 0, "SI"),
                               Ets = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_ETS_ID, "NOCOMBO", 0, "SI"),
                               Hematopatias = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_HEMATOPATIAS_ID, "NOCOMBO", 0, "SI"),
                               Obesidad = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_OBESIDAD_ID, "NOCOMBO", 0, "SI"),
                               Periodontitis = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PERIODONTITIS_ID, "NOCOMBO", 0, "SI"),
                               Movilidad = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_MOVILIDAD_ID, "NOCOMBO", 0, "SI"),
                               Recesion = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_RECESION_ID, "NOCOMBO", 0, "SI"),
                               Exudacion = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_EXUDACION_ID, "NOCOMBO", 0, "SI"),
                               Gingivitis = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_GINGIVITIS_ID, "NOCOMBO", 0, "SI"),
                               BolsaPeriodontales = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_BOLSA_PERIODONTALES_ID, "NOCOMBO", 0, "SI"),
                               Diagnosticos = GetDiagnosticByServiceIdAndComponent(a.IdServicio, pstrComponentId),
                               PiezasCaries = GetCantidadCaries(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PIEZAS_CARIES_ID),
                               PiezasAusentes = GetCantidadAusentes(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PIEZAS_AUSENTES_ID),

                               //PiezasCuracion = GetCantidad(a.IdServicio,pstrComponentId),
                               Corona = GetCantidad(a.IdServicio, pstrComponentId)[0].ToString(),
                               Exodoncia = GetCantidad(a.IdServicio, pstrComponentId)[1].ToString(),
                               Implante = GetCantidad(a.IdServicio, pstrComponentId)[2].ToString(),
                               Ppr = GetCantidad(a.IdServicio, pstrComponentId)[3].ToString(),
                               ProtesisTotal = GetCantidad(a.IdServicio, pstrComponentId)[4].ToString(),

                               PlacaBacteriana = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PLACA_BACTERIANA_ID),
                               RemanentesReticulares = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_REMANENTES_RETICULARES_ID),
                               OtrosExamen = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_OTROS_EXAMEN_ID),
                               Aptitud = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_APTITUD_ID),

                               Diente181 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_1, Path, "18"),
                               Diente182 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_2, Path, "18"),
                               Diente183 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_3, Path, "18"),
                               Diente184 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_4, Path, "18"),
                               Diente185 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_5, Path, "18"),
                               Diente186 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D18_6, Path),

                               Diente171 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_1, Path, "17"),
                               Diente172 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_2, Path, "17"),
                               Diente173 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_3, Path, "17"),
                               Diente174 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_4, Path, "17"),
                               Diente175 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_5, Path, "17"),
                               Diente176 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D17_6, Path),

                               Diente161 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_1, Path, "16"),
                               Diente162 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_2, Path, "16"),
                               Diente163 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_3, Path, "16"),
                               Diente164 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_4, Path, "16"),
                               Diente165 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_5, Path, "16"),
                               Diente166 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D16_6, Path),

                               Diente151 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_1, Path, "15"),
                               Diente152 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_2, Path, "15"),
                               Diente153 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_3, Path, "15"),
                               Diente154 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_4, Path, "15"),
                               Diente155 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_5, Path, "15"),
                               Diente156 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D15_6, Path),

                               Diente141 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_1, Path, "14"),
                               Diente142 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_2, Path, "14"),
                               Diente143 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_3, Path, "14"),
                               Diente144 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_4, Path, "14"),
                               Diente145 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_5, Path, "14"),
                               Diente146 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D14_6, Path),

                               Diente131 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_1, Path, "13"),
                               Diente132 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_2, Path, "13"),
                               Diente133 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_3, Path, "13"),
                               Diente134 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_4, Path, "13"),
                               Diente135 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_5, Path, "13"),
                               Diente136 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D13_6, Path),

                               Diente121 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_1, Path, "12"),
                               Diente122 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_2, Path, "12"),
                               Diente123 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_3, Path, "12"),
                               Diente124 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_4, Path, "12"),
                               Diente125 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_5, Path, "12"),
                               Diente126 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D12_6, Path),

                               Diente111 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_1, Path, "11"),
                               Diente112 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_2, Path, "11"),
                               Diente113 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_3, Path, "11"),
                               Diente114 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_4, Path, "11"),
                               Diente115 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_5, Path, "11"),
                               Diente116 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D11_6, Path),

                               //-------------------------------------------------------------------------------

                               Diente211 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_1, Path, "21"),
                               Diente212 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_2, Path, "21"),
                               Diente213 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_3, Path, "21"),
                               Diente214 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_4, Path, "21"),
                               Diente215 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_5, Path, "21"),
                               Diente216 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D21_6, Path),

                               Diente221 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_1, Path, "22"),
                               Diente222 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_2, Path, "22"),
                               Diente223 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_3, Path, "22"),
                               Diente224 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_4, Path, "22"),
                               Diente225 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_5, Path, "22"),
                               Diente226 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D22_6, Path),

                               Diente231 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_1, Path, "23"),
                               Diente232 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_2, Path, "23"),
                               Diente233 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_3, Path, "23"),
                               Diente234 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_4, Path, "23"),
                               Diente235 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_5, Path, "23"),
                               Diente236 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D23_6, Path),

                               Diente241 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_1, Path, "24"),
                               Diente242 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_2, Path, "24"),
                               Diente243 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_3, Path, "24"),
                               Diente244 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_4, Path, "24"),
                               Diente245 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_5, Path, "24"),
                               Diente246 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D24_6, Path),

                               Diente251 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_1, Path, "25"),
                               Diente252 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_2, Path, "25"),
                               Diente253 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_3, Path, "25"),
                               Diente254 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_4, Path, "25"),
                               Diente255 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_5, Path, "25"),
                               Diente256 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D25_6, Path),

                               Diente261 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_1, Path, "26"),
                               Diente262 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_2, Path, "26"),
                               Diente263 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_3, Path, "26"),
                               Diente264 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_4, Path, "26"),
                               Diente265 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_5, Path, "26"),
                               Diente266 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D26_6, Path),

                               Diente271 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_1, Path, "27"),
                               Diente272 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_2, Path, "27"),
                               Diente273 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_3, Path, "27"),
                               Diente274 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_4, Path, "27"),
                               Diente275 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_5, Path, "27"),
                               Diente276 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D27_6, Path),

                               Diente281 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_1, Path, "28"),
                               Diente282 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_2, Path, "28"),
                               Diente283 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_3, Path, "28"),
                               Diente284 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_4, Path, "28"),
                               Diente285 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_5, Path, "28"),
                               Diente286 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D28_6, Path),

                               //-------------------------------------------------------------------------------

                               Diente311 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_1, Path, "31"),
                               Diente312 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_2, Path, "31"),
                               Diente313 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_3, Path, "31"),
                               Diente314 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_4, Path, "31"),
                               Diente315 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_5, Path, "31"),
                               Diente316 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D31_6, Path),

                               Diente321 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_1, Path, "32"),
                               Diente322 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_2, Path, "32"),
                               Diente323 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_3, Path, "32"),
                               Diente324 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_4, Path, "32"),
                               Diente325 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_5, Path, "32"),
                               Diente326 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D32_6, Path),

                               Diente331 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_1, Path, "33"),
                               Diente332 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_2, Path, "33"),
                               Diente333 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_3, Path, "33"),
                               Diente334 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_4, Path, "33"),
                               Diente335 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_5, Path, "33"),
                               Diente336 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D33_6, Path),

                               Diente341 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_1, Path, "34"),
                               Diente342 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_2, Path, "34"),
                               Diente343 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_3, Path, "34"),
                               Diente344 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_4, Path, "34"),
                               Diente345 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_5, Path, "34"),
                               Diente346 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D34_6, Path),

                               Diente351 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_1, Path, "35"),
                               Diente352 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_2, Path, "35"),
                               Diente353 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_3, Path, "35"),
                               Diente354 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_4, Path, "35"),
                               Diente355 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_5, Path, "35"),
                               Diente356 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D35_6, Path),


                               Diente361 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_1, Path, "36"),
                               Diente362 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_2, Path, "36"),
                               Diente363 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_3, Path, "36"),
                               Diente364 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_4, Path, "36"),
                               Diente365 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_5, Path, "36"),
                               Diente366 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D36_6, Path),

                               Diente371 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_1, Path, "37"),
                               Diente372 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_2, Path, "37"),
                               Diente373 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_3, Path, "37"),
                               Diente374 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_4, Path, "37"),
                               Diente375 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_5, Path, "37"),
                               Diente376 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D37_6, Path),

                               Diente381 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_1, Path, "38"),
                               Diente382 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_2, Path, "38"),
                               Diente383 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_3, Path, "38"),
                               Diente384 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_4, Path, "38"),
                               Diente385 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_5, Path, "38"),
                               Diente386 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D38_6, Path),
                               //-------------------------------------------------------------------------------

                               Diente411 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_1, Path, "41"),
                               Diente412 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_2, Path, "41"),
                               Diente413 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_3, Path, "41"),
                               Diente414 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_4, Path, "41"),
                               Diente415 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_5, Path, "41"),
                               Diente416 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D41_6, Path),

                               Diente421 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_1, Path, "42"),
                               Diente422 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_2, Path, "42"),
                               Diente423 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_3, Path, "42"),
                               Diente424 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_4, Path, "42"),
                               Diente425 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_5, Path, "42"),
                               Diente426 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D42_6, Path),

                               Diente431 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_1, Path, "43"),
                               Diente432 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_2, Path, "43"),
                               Diente433 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_3, Path, "43"),
                               Diente434 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_4, Path, "43"),
                               Diente435 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_5, Path, "43"),
                               Diente436 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D43_6, Path),

                               Diente441 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_1, Path, "44"),
                               Diente442 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_2, Path, "44"),
                               Diente443 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_3, Path, "44"),
                               Diente444 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_4, Path, "44"),
                               Diente445 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_5, Path, "44"),
                               Diente446 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D44_6, Path),

                               Diente451 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_1, Path, "45"),
                               Diente452 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_2, Path, "45"),
                               Diente453 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_3, Path, "45"),
                               Diente454 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_4, Path, "45"),
                               Diente455 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_5, Path, "45"),
                               Diente456 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D45_6, Path),

                               Diente461 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_1, Path, "46"),
                               Diente462 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_2, Path, "46"),
                               Diente463 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_3, Path, "46"),
                               Diente464 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_4, Path, "46"),
                               Diente465 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_5, Path, "46"),
                               Diente466 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D46_6, Path),

                               Diente471 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_1, Path, "47"),
                               Diente472 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_2, Path, "47"),
                               Diente473 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_3, Path, "47"),
                               Diente474 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_4, Path, "47"),
                               Diente475 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_5, Path, "47"),
                               Diente476 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D47_6, Path),

                               Diente481 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_1, Path, "48"),
                               Diente482 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_2, Path, "48"),
                               Diente483 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_3, Path, "48"),
                               Diente484 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_4, Path, "48"),
                               Diente485 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_5, Path, "48"),
                               Diente486 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D48_6, Path),

                               PiezasCuracion = NroDientesCurados(ListaDiente),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private int NroDientesCurados(List<int> ListaD)
        {

            var x = ListaD.Distinct();

            return x.Count();
        }
        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontograma(string pstrServiceId, string pstrComponentId, string pstrPath)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)
                                                                                     let range = (
                                                                                                     D.v_Value1 == "2" ? pstrPath + "\\Resources\\caries.png" :
                                                                                                      D.v_Value1 == "3" ? pstrPath + "\\Resources\\curacion.png" :
                                                                                                      string.Empty
                                                                                          )
                                                                                    select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = range,
                                                                                         v_Value2 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetValueOdontograma(string pstrServiceId, string pstrComponentId, string pstrFieldId, string pstrpath, string NroDiente)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList1 = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                oServiceComponentFieldValuesList1 = ValoresComponenteOdontogramaValue1(pstrServiceId, pstrComponentId);
                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma(pstrServiceId, pstrComponentId, pstrpath);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                var valorDiente = oServiceComponentFieldValuesList1.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList1.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList1.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;


                switch (NroDiente)
                {
                    case "18":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(18);
                        }
                        break;
                    case "17":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(17);
                        }
                        break;
                    case "16":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(16);
                        }
                        break;
                    case "15":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(15);
                        }
                        break;
                    case "14":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(14);
                        }
                        break;
                    case "13":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(13);
                        }
                        break;
                    case "12":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(12);
                        }
                        break;
                    case "11":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(11);
                        }
                        break;

                    //--------------------------------------

                    case "21":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(21);
                        }
                        break;
                    case "22":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(22);
                        }
                        break;
                    case "23":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(23);
                        }
                        break;
                    case "24":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(24);
                        }
                        break;
                    case "25":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(25);
                        }
                        break;
                    case "26":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(26);
                        }
                        break;
                    case "27":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(27);
                        }
                        break;
                    case "28":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(28);
                        }
                        break;

                    //------------------------------

                    case "31":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(31);
                        }
                        break;
                    case "32":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(32);
                        }
                        break;
                    case "33":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(33);
                        }
                        break;
                    case "34":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(34);
                        }
                        break;
                    case "35":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(35);
                        }
                        break;
                    case "36":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(36);
                        }
                        break;
                    case "37":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(37);
                        }
                        break;
                    case "38":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(38);
                        }
                        break;

                    //------------------------------

                    case "41":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(41);
                        }
                        break;
                    case "42":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(42);
                        }
                        break;
                    case "43":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(43);
                        }
                        break;
                    case "44":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(44);
                        }
                        break;
                    case "45":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(45);
                        }
                        break;
                    case "46":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(46);
                        }
                        break;
                    case "47":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(47);
                        }
                        break;
                    case "48":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(48);
                        }
                        break;
                    default:
                        break;
                }

                return xx;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int[] GetCantidad(string pstrServiceId, string pstrComponentId)
        {
            try
            {
                int ContadorCorona = 0;
                int ContadorExodoncia = 0;
                int ContadorImplante = 0;
                int ContadorPPR = 0;
                int ContadorProtesisTotal = 0;

                int[] xxx = new int[5];

                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);

                for (int i = 0; i < oServiceComponentFieldValuesList.Count(); i++)
                {

                    #region Region 1

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D11_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D12_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D13_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D14_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D15_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D16_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D17_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D18_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion

                    #region Region 2

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D21_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D22_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D23_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D24_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D25_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D26_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D27_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D28_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion

                    #region Region 3

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D31_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D32_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D33_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D34_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D35_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D36_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D37_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D38_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion

                    #region Region 4

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D41_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D42_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D43_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D44_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D45_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D46_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D47_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D48_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion
                }

                xxx[0] = ContadorCorona;
                xxx[1] = ContadorExodoncia;
                xxx[2] = ContadorImplante;
                xxx[3] = ContadorPPR;
                xxx[4] = ContadorProtesisTotal;


                return xxx;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ReportRadiologico> ReportRadiologico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }
                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************                            

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.ReportRadiologico
                                 {
                                     v_ServiceId = A.v_ServiceId,
                                     Paciente = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     ExamenSolicitado = "Radiografia de Torax (P-A)",
                                     Empresa = J.v_Name,
                                     Fecha = A.d_ServiceDate.Value,
                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     FirmaMedicoEva = pme.b_SignatureImage,
                                     d_BirthDate = B.d_Birthdate.Value,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportRadiologico
                           {
                               v_ServiceId = a.v_ServiceId,
                               Paciente = a.Paciente,
                               ExamenSolicitado = a.ExamenSolicitado,
                               Empresa = a.Empresa,
                               Fecha = a.Fecha,
                               FirmaTecnologo = a.FirmaTecnologo,
                               FirmaMedicoEva = a.FirmaMedicoEva,
                               d_BirthDate = a.d_BirthDate,
                               Edad = GetAge(a.d_BirthDate.Value),

                               Vertices = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_VERTICES_ID, "NOCOMBO", 0, "SI"),
                               CamposPulmonares = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CAMPOS_PULMONARES_ID, "NOCOMBO", 0, "SI"),
                               SenosCosto = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_COSTO_ODIAFRAGMATICO_ID, "NOCOMBO", 0, "SI"),
                               SenosCardio = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SENOS_CARDIOFRENICOS_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Mediastinos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_MEDIASTINOS_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Silueta = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SILUETA_CARDIACA_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Indice = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_INDICE_CARDIACO_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               PartesBlandas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PARTES_BLANDAS_OSEAS_ID, "NOCOMBO", 0, "SI"),
                               Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Hilos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_HILOS_ID, "NOCOMBO", 0, "SI"),
                               Hallazgos = GetDiagnosticByServiceIdAndComponent(a.v_ServiceId, pstrComponentId),


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportInformeRadiografico> ReportInformeRadiografico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId into F_join
                                 //from F in F_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()


                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************      

                                 join me1 in dbContext.person on me.v_PersonId equals me1.v_PersonId


                                 join G in dbContext.professional on new { a = me.v_PersonId }
                                                                      equals new { a = G.v_PersonId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                                 from J in J_join.DefaultIfEmpty()



                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeRadiografico
                                 {
                                     Nombre = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     FirmaMedico = pme.b_SignatureImage,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     Lector = me1.v_FirstName + " " + me1.v_FirstLastName + " " + me1.v_SecondLastName,
                                     Hcl = A.v_ServiceId,
                                     FirmaTecnologo = ptec.b_SignatureImage,
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportInformeRadiografico
                           {
                               Nombre = a.Nombre,
                               FechaNacimiento = a.FechaNacimiento,
                               d_ServiceDate = a.d_ServiceDate,
                               v_ServiceId = a.v_ServiceId,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTecnologo = a.FirmaTecnologo,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               Lector = a.Lector,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Placa = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_NRO_PLACA_ID, "NOCOMBO", 0, "SI"),
                               CalidaRadio = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CALIDAD_ID, "NOCOMBO", 0, "SI"),
                               Causas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CAUSAS_ID, "NOCOMBO", 0, "SI"),
                               Comentario = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_COMENTARIOS_ID, "NOCOMBO", 0, "SI"),
                               Hcl = a.Hcl,
                               FechaLectura = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_FECHA_LECTURA_ID, "NOCOMBO", 0, "SI"),
                               FechaRadiografia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_FECHA_TOMA_ID, "NOCOMBO", 0, "SI"),

                               SuperiorDerecho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SUPERIOR_DERECHO_ID, "NOCOMBO", 0, "SI"),
                               SuperiorIzquierda = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SUPERIOR_IZQUIERDO_ID, "NOCOMBO", 0, "SI"),
                               MedioDerecho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_MEDIO_DERECHO_ID, "NOCOMBO", 0, "SI"),
                               MedioIzquierda = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_MEDIO_IZQUIERDO_ID, "NOCOMBO", 0, "SI"),
                               InferiorDerecho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_INFERIOR_DERECHO_ID, "NOCOMBO", 0, "SI"),
                               InferiorIzquierdo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_INFERIOR_IZQUIERDO_ID, "NOCOMBO", 0, "SI"),

                               SimboloSi = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SIMBOLO_SI_ID, "NOCOMBO", 0, "SI"),
                               SimboloNo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SIMBOLO_NO_ID, "NOCOMBO", 0, "SI"),

                               CeroNada = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_0_NADA_ID, "NOCOMBO", 0, "SI"),
                               CeroCero = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_0_0_ID, "NOCOMBO", 0, "SI"),
                               CeroUno = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_0_1_ID, "NOCOMBO", 0, "SI"),

                               UnoCero = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_1_0_ID, "NOCOMBO", 0, "SI"),
                               UnoUno = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_1_1_ID, "NOCOMBO", 0, "SI"),
                               UnoDos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_1_2_ID, "NOCOMBO", 0, "SI"),

                               DosUno = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_2_1_ID, "NOCOMBO", 0, "SI"),
                               DosDos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_2_2_ID, "NOCOMBO", 0, "SI"),
                               DosTres = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_2_3_ID, "NOCOMBO", 0, "SI"),

                               TresDos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_3_2_ID, "NOCOMBO", 0, "SI"),
                               TresTres = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_3_3_ID, "NOCOMBO", 0, "SI"),
                               TresMas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_3_MAS_ID, "NOCOMBO", 0, "SI"),

                               p = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_P_ID, "NOCOMBO", 0, "SI"),
                               q = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_Q_ID, "NOCOMBO", 0, "SI"),
                               r = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_R_ID, "NOCOMBO", 0, "SI"),
                               s = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_S_ID, "NOCOMBO", 0, "SI"),
                               t = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_T_ID, "NOCOMBO", 0, "SI"),
                               u = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_U_ID, "NOCOMBO", 0, "SI"),
                               p1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_P1_ID, "NOCOMBO", 0, "SI"),
                               q1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_Q1_ID, "NOCOMBO", 0, "SI"),
                               r1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_R1_ID, "NOCOMBO", 0, "SI"),
                               s1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_S1_ID, "NOCOMBO", 0, "SI"),
                               t1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_T1_ID, "NOCOMBO", 0, "SI"),
                               u1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_U1_ID, "NOCOMBO", 0, "SI"),


                               O = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_D_ID, "NOCOMBO", 0, "SI"),
                               A = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_A_ID, "NOCOMBO", 0, "SI"),
                               B = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_B_ID, "NOCOMBO", 0, "SI"),
                               C = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_C_ID, "NOCOMBO", 0, "SI"),
                               //SimboloSiNo= GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SIM, "NOCOMBO", 0, "SI"), 
                               aa = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_AA_ID, "NOCOMBO", 0, "SI"),
                               at = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_AT_ID, "NOCOMBO", 0, "SI"),
                               ax = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_AX_ID, "NOCOMBO", 0, "SI"),
                               bu = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_BU_ID, "NOCOMBO", 0, "SI"),
                               ca = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CA_ID, "NOCOMBO", 0, "SI"),
                               cg = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CG_ID, "NOCOMBO", 0, "SI"),
                               cn = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CN_ID, "NOCOMBO", 0, "SI"),
                               co = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CO_ID, "NOCOMBO", 0, "SI"),
                               cp = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CP_ID, "NOCOMBO", 0, "SI"),
                               cv = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CV_ID, "NOCOMBO", 0, "SI"),



                               di = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_DI_ID, "NOCOMBO", 0, "SI"),
                               ef = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_EF_ID, "NOCOMBO", 0, "SI"),
                               em = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_EM_ID, "NOCOMBO", 0, "SI"),
                               es = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_ES_ID, "NOCOMBO", 0, "SI"),
                               fr = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_FR_ID, "NOCOMBO", 0, "SI"),
                               hi = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_HI_ID, "NOCOMBO", 0, "SI"),
                               ho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_HO_ID, "NOCOMBO", 0, "SI"),
                               id = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_ID_ID, "NOCOMBO", 0, "SI"),
                               ih = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_IH_ID, "NOCOMBO", 0, "SI"),
                               kl = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_KL_ID, "NOCOMBO", 0, "SI"),
                               me = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_ME_ID, "NOCOMBO", 0, "SI"),
                               od = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_OD_ID, "NOCOMBO", 0, "SI"),
                               pa = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PA_ID, "NOCOMBO", 0, "SI"),
                               pb = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PB_ID, "NOCOMBO", 0, "SI"),
                               pi = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PI_ID, "NOCOMBO", 0, "SI"),
                               px = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PX_ID, "NOCOMBO", 0, "SI"),
                               ra = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_RA_ID, "NOCOMBO", 0, "SI"),
                               rp = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_RP_ID, "NOCOMBO", 0, "SI"),
                               tb = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_TB_ID, "NOCOMBO", 0, "SI"),
                               Comentario_Od = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_COMENTARIO_OD_ID, "NOCOMBO", 0, "SI"),


                               Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CONCLUSIONES_OIT_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Dx = GetDiagnosticByServiceIdAndComponent(a.v_ServiceId, pstrComponentId),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportTamizajeDermatologico> ReportTamizajeDermatologico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId into F_join
                                 //from F in F_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join G in dbContext.professional on new { a = me.v_PersonId }
                                                                      equals new { a = G.v_PersonId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                                 from J in J_join.DefaultIfEmpty()



                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportTamizajeDermatologico
                                 {
                                     Ficha = A.v_ServiceId,
                                     Hc = E.v_ServiceComponentId,
                                     NombreTrabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Fecha = A.d_ServiceDate.Value,
                                     FirmaMedico = pme.b_SignatureImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage
                                 });


                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportTamizajeDermatologico
                           {
                               Ficha = a.Ficha,
                               Hc = a.Hc,
                               NombreTrabajador = a.NombreTrabajador,
                               Fecha = a.Fecha,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               SufreEnfermedadPielSiNo = GetServiceComponentFielValue(pstrserviceId, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SUFRE_UD_ENFERMEDAD_PIEL_ID, "NOCOMBO", 0, "SI"),
                               SiQueDxTiene = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_QUE_DX_TIENE_ID, "NOCOMBO", 0, "SI"),
                               ActualmenteLesionSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ACTUALMENTE_LESION_ID, "NOCOMBO", 0, "SI"),
                               DondeLocalizaLesion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_DONDE_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               CuantoTieneLesion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_CUANTO_TIENE_LESION_ID, "NOCOMBO", 0, "SI"),
                               PresentaColoracionPielSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_PRESENTA_COLORACION_PIEL_ID, "NOCOMBO", 0, "SI"),
                               LesionRepiteVariasAniosSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_LESIONES_REPITE_VARIAS_ANIOS_ID, "NOCOMBO", 0, "SI"),
                               EnrrojecimientoParteCuerpoSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ENRROJECIMIENTO_PARTE_CUERPO_ID, "NOCOMBO", 0, "SI"),
                               EnrrojecimientoLocaliza = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ENRROJECIMIENTO_SI_DONDE_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               TieneComezonSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_TIENE_COMEZON_ID, "NOCOMBO", 0, "SI"),
                               ComezonLocaliza = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_COMEZON_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               HinchazonParteCuerpoSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_HINCHAZON_PARTE_CUERPO_ID, "NOCOMBO", 0, "SI"),
                               HinchazonParteCuerpoLocaliza = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_HINCHAZON_PARTE_CUERPO_DONDE_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               AlergiaAsmaSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ALERGIA_ASMA_ID, "NOCOMBO", 0, "SI"),
                               EppSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_USA_EPP_ID, "NOCOMBO", 0, "SI"),
                               TipoProteccionUsa = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_TIPO_PROTECCION_USA_ID, "NOCOMBO", 0, "SI"),
                               CambioUnasSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_PRESENTA_CAMBIO_UNAS_ID, "NOCOMBO", 0, "SI"),
                               TomandoMedicacionSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_TOMANDO_ALGUNA_MEDICACION_ID, "NOCOMBO", 0, "SI"),
                               ComoLlamaMedicacion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_COMO_SE_LLAMA_ID, "NOCOMBO", 0, "SI"),
                               DosisFrecuencia = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DOSIS_FRECUENCIA_ID, "NOCOMBO", 0, "SI"),
                               Descripcion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DESCRIPCION1_ID, "NOCOMBO", 0, "SI"),
                               DermatopiaSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DERMATOPIA_ID, "NOCOMBO", 0, "SI"),
                               NikolskySiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DERMATOPIA_ID, "NOCOMBO", 0, "SI"),
                               v_OwnerOrganizationName = (from n in dbContext.organization
                                                          where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                                          select n.v_Name).SingleOrDefault<string>(),


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportEvaGinecologico> GetReportEvaluacionGinecologico(string pstrserviceId, string pstrComponentId)
        {

            string[] excludeComponents = {   Sigesoft.Common.Constants.GINECOLOGIA_ID,
                                                 Sigesoft.Common.Constants.EXAMEN_MAMA_ID,
                                                 Sigesoft.Common.Constants.ECOGRAFIA_MAMA_ID ,
                                                 Sigesoft.Common.Constants.PAPANICOLAU_ID,
                                                 Sigesoft.Common.Constants.RESULTADOS_MAMOGRAFIA_ID
                                             };
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join M in dbContext.systemparameter on new { a = B.i_TypeOfInsuranceId.Value, b = 188 }
                                     equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()


                                 //join F in dbContext.component on new { a = E.v_ComponentId, b = 12 }
                                 //                                       equals new { a = F.v_ComponentId, b = F.i_CategoryId.Value } into F_join
                                 //from F in F_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId


                                 join I in dbContext.systemparameter on new { a = A.i_MacId.Value, b = 134 }
                                   equals new { a = I.i_ParameterId, b = I.i_GroupId } into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join ccc in dbContext.diagnosticrepository on A.v_ServiceId equals ccc.v_ServiceId into ccc_join
                                 from ccc in ccc_join.DefaultIfEmpty()  // ESO

                                 join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                                 from ddd in ddd_join.DefaultIfEmpty()  // Diagnosticos


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEvaGinecologico
                                 {
                                     v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                     Ficha = E.v_ServiceComponentId,
                                     Historia = A.v_ServiceId,
                                     NombreTrabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Seguro = M.v_Value1,
                                     EmpresaCliente = D.v_Name,
                                     EmpresaEmpleadora = C1.v_Name,
                                     Medico = y.v_FirstName + " " + y.v_FirstLastName + " " + y.v_SecondLastName,
                                     FechaAtencion = A.d_ServiceDate.Value,
                                     Fum = A.d_Fur.Value,
                                     Gestapara = A.v_Gestapara,
                                     MAC = I.v_Value1,
                                     Menarquia = A.v_Menarquia,
                                     RegimenCatamenial = A.v_CatemenialRegime,
                                     CirugiaGinecologica = A.v_CiruGine,
                                     FirmaDoctor = pme.b_SignatureImage,
                                     FotoPaciente = B.b_PersonImage,
                                     Diagnosticos = ddd.v_Name,
                                     v_ComponentId = ccc.v_ComponentId

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Gine = ValoresComponente(pstrserviceId, Constants.GINECOLOGIA_ID)


                           let ExamMama = ValoresComponente(pstrserviceId, Constants.EXAMEN_MAMA_ID)
                           let LogoEmpresa = GetLogoMedicalCenter()
                           //let ResultadoEcografiaMama = ValoresComponente(pstrserviceId, Constants.ECOGRAFIA_MAMA_ID)
                           let AntePersonales = ValoresComponente(pstrserviceId, Constants.ISTAS_21_ABREVIADA)

                           //let ResultadoPapanicolao = ValoresComponente(pstrserviceId, Constants.PAPANICOLAU_ID)
                           let GineAuxiliares = ValoresComponente(pstrserviceId, Constants.PAPANICOLAU_ID)
                           let ResultadoEcografia = ValoresComponente(pstrserviceId, Constants.RESULTADOS_MAMOGRAFIA_ID)
                           let ResultadoEcografiaMama = ValoresComponente(pstrserviceId, Constants.ECOGRAFIA_MAMA_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportEvaGinecologico
                           {
                               v_ComponentId = a.v_ComponentId,
                               v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                               Ficha = a.Ficha,
                               Historia = a.Historia,
                               Logo = LogoEmpresa,
                               NombreTrabajador = a.NombreTrabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Seguro = a.Seguro,
                               EmpresaCliente = a.EmpresaCliente,
                               EmpresaEmpleadora = a.EmpresaEmpleadora,
                               CentroMedico = (from n in dbContext.organization
                                               where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                               select n.v_Name + ", " + n.v_Address).SingleOrDefault<string>(),
                               Medico = a.Medico,
                               FechaAtencion = a.FechaAtencion,
                               Fum = a.Fum,
                               Gestapara = a.Gestapara,
                               FechaPAP = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_GINECOLOGICA_ANTECEDENTES_FECHA_ULTIMO_PAP).v_Value1,
                               MAC = a.MAC,
                               Menarquia = a.Menarquia,
                               FechaMamografia = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_GINECOLOGICA_ANTECEDENTES_FECHA_ULTIMA_MAMOGRAFIA).v_Value1,
                               RegimenCatamenial = a.RegimenCatamenial,
                               CirugiaGinecologica = a.CirugiaGinecologica,

                               Leucorrea = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_LEUCORREA).v_Value1,
                               LeucorreaDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_LEUCORREA_COMENTARIO).v_Value1,

                               Dipareunia = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_DISPAREUNIA).v_Value1,
                               DipareuniaDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_DISPAREUNIA_COMENTARIO).v_Value1,

                               Incontinencia = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_INCONTINENCIA_URINARIA).v_Value1,
                               IncontinenciaDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_INCONTINENCIA_URINARIA_COMENTARIO).v_Value1,

                               Otros = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_OTROS).v_Value1,
                               OtrosDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_OTROS_COMENTARIO).v_Value1,

                               EvaluacionGinecologica = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_HALLAZGOS_HALLAZGOS).v_Value1,

                               ExamenMama = ExamMama.Count == 0 ? string.Empty : ExamMama.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_EX_MAMA_HALLAZGOS_HALLAZGOS).v_Value1.Replace("\n", " "),




                               ResultadoPAP = GineAuxiliares.Count == 0 ? "N/A" : GineAuxiliares.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_HALLAZGOS).v_Value1 == "" ? "" : GineAuxiliares.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_HALLAZGOS).v_Value1,
                               ResultadoMamografia = ResultadoEcografia.Count == 0 ? "N/A" : ResultadoEcografia.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_RADIOGRAFIA_HALLAZGOS).v_Value1 == "" ? "" : ResultadoEcografia.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_RADIOGRAFIA_HALLAZGOS).v_Value1,
                               ResultadoMama = ResultadoEcografiaMama.Count == 0 ? "N/A" : ResultadoEcografiaMama.Find(p => p.v_ComponentFieldId == Constants.RESULTADOS_DE_ECOGRAFIA_HALLAZGOS).v_Value1 == "" ? "" : ResultadoEcografiaMama.Find(p => p.v_ComponentFieldId == Constants.RESULTADOS_DE_ECOGRAFIA_HALLAZGOS).v_Value1,






                               Diagnosticos = a.Diagnosticos,
                               Recomendaciones = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),

                               FotoPaciente = a.FotoPaciente,
                               FirmaDoctor = a.FirmaDoctor,
                               AntecedentesPersonales = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_ANTECEDENTES_PERSONALES_ANTECEDENTES).v_Value1,

                               AntecendentesFamiliares = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_ANTECEDENTES_FAMILIARES).v_Value1,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                var otherExams = sql.FindAll(p => excludeComponents.Contains(p.v_ComponentId));


                return otherExams;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public byte[] GetLogoMedicalCenter()
        {
            using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
            {

                var nameMedicalCenter = (from n in dbContext.organization
                                         where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                         select n.b_Image).SingleOrDefault();

                return nameMedicalCenter;
            }
        }


        public List<Sigesoft.Node.WinClient.BE.ReportCuestionarioEspirometria> GetReportCuestionarioEspirometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join SP1 in dbContext.systemparameter on new { a = D.i_SectorTypeId.Value, b = 104 }
                                        equals new { a = SP1.i_ParameterId, b = SP1.i_GroupId } into SP1_join
                                 from SP1 in SP1_join.DefaultIfEmpty()


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportCuestionarioEspirometria
                                 {
                                     IdServicio = A.v_ServiceId,
                                     IdComponent = B.v_ServiceComponentId,
                                     Fecha = A.d_ServiceDate.Value,
                                     NombreTrabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FechaNacimineto = E.d_Birthdate,
                                     Genero = E.i_SexTypeId.Value,
                                     FirmaTrabajador = E.b_RubricImage,
                                     HuellaTrabajador = E.b_FingerPrintImage


                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()

                           let Espirometria = ValoresComponente(pstrserviceId, Constants.ESPIROMETRIA_ID)
                           let age = GetAge(a.FechaNacimineto.Value)
                           let LogoEmpresa = GetLogoMedicalCenter()
                           select new Sigesoft.Node.WinClient.BE.ReportCuestionarioEspirometria
                           {
                               IdServicio = a.IdServicio,
                               IdComponent = a.IdComponent,
                               Fecha = a.Fecha,
                               NombreTrabajador = a.NombreTrabajador,
                               FechaNacimineto = a.FechaNacimineto,
                               Edad = age,
                               Logo = LogoEmpresa,
                               Pregunta1ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_1).v_Value1,
                               Pregunta2ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_2).v_Value1,
                               Pregunta3ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_3).v_Value1,
                               Pregunta4ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_4).v_Value1,
                               Pregunta5ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_5).v_Value1,

                               HemoptisisSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.PIROMETRIA_ANTECEDENTES_HEMOSTISIS).v_Value1,
                               PseumotoraxSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_PNEUMOTORAX).v_Value1,
                               TraqueostomiaSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_TRAQUEOSTOMIA).v_Value1,
                               SondaPleuralSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_SONDA_PLEURAL).v_Value1,
                               AneurismaSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_ANEURISMA_CEREBRAL).v_Value1,
                               EmboliaSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_EMBOLIA_PULMONAR).v_Value1,
                               InfartoSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_INFARTO_RECIENTE).v_Value1,
                               InestabilidadSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_INESTABILIDAD_CV).v_Value1,
                               FiebreSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_FIEBRE_NAUSEAS).v_Value1,
                               EmbarazoAvanzadoSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_EMBARAZO_AVANZADO).v_Value1,
                               EmbarazoComplicadoSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_EMBARAZO_COMPLICADO).v_Value1,

                               Pregunta1BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESESPIROMETRIA_CUESTIONARIO_PARA_1).v_Value1,
                               Pregunta2BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_2).v_Value1,
                               Pregunta3BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_3).v_Value1,
                               Pregunta4BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_4).v_Value1,
                               Pregunta5BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_5).v_Value1,
                               Pregunta6BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_6).v_Value1,
                               Pregunta7BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_7).v_Value1,

                               Genero = a.Genero,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportInformeEspirometria> GetReportInformeEspirometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join SP1 in dbContext.datahierarchy on new { a = D.i_SectorTypeId.Value, b = 104 }
                                        equals new { a = SP1.i_ItemId, b = SP1.i_GroupId } into SP1_join
                                 from SP1 in SP1_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on B.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************  

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeEspirometria
                                 {
                                     EspirometriaNro = B.v_ServiceComponentId,
                                     Fecha = A.d_ServiceDate.Value,
                                     HCL = A.v_ServiceId,
                                     i_TipoEvaluacion = C.i_MasterServiceTypeId.Value,
                                     RazonSocial = D.v_Name,
                                     ActividadEconomica = SP1.v_Value1,
                                     PuestoTrabajo = E.v_CurrentOccupation,
                                     NombreTrabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FechaNacimiento = E.d_Birthdate.Value,
                                     i_Sexo = E.i_SexTypeId.Value,
                                     FirmaRealizaEspirometria = ptec.b_SignatureImage,
                                     FirmaMedicoInterpreta = pme.b_SignatureImage
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let nameMedicalCenter = GetNameMedicalCenter()
                           let Espirometria = ValoresComponente(pstrserviceId, Constants.ESPIROMETRIA_ID)
                           let age = GetAge(a.FechaNacimiento.Value)
                           let Antropometria = ValoresComponente(pstrserviceId, Constants.ANTROPOMETRIA_ID)

                           let DxEspirometria = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.ESPIROMETRIA_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportInformeEspirometria
                           {
                               Logo = MedicalCenter.b_Image,
                               EspirometriaNro = a.EspirometriaNro,
                               Fecha = a.Fecha,
                               HCL = a.HCL,
                               TipoEvaluacion = a.i_TipoEvaluacion.ToString(),
                               LugarExamen = nameMedicalCenter,
                               RazonSocial = a.RazonSocial,
                               ActividadEconomica = a.ActividadEconomica,
                               PuestoTrabajo = a.PuestoTrabajo,
                               TiempoTrabajo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_TIEMPO_TRABAJO_ID).v_Value1,
                               NombreTrabajador = a.NombreTrabajador,
                               Edad = age,
                               Sexo = a.i_Sexo.ToString(),
                               Talla = Antropometria.Count == 0 ? string.Empty : Antropometria.Find(p => p.v_ComponentFieldId == Constants.ANTROPOMETRIA_TALLA_ID).v_Value1,
                               Peso = Antropometria.Count == 0 ? string.Empty : Antropometria.Find(p => p.v_ComponentFieldId == Constants.ANTROPOMETRIA_PESO_ID).v_Value1,
                               OrigenEtnico = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ORIGEN_ETNICO).v_Value1,
                               FumadorSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_TABAQUISMO).v_Value1,
                               CVF = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_ABS_CVF).v_Value1,
                               VEF1 = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCIÓN_RESPIRATORIA_ABS_VEF_1).v_Value1,
                               VEF1CVF = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCIÓN_RESPIRATORIA_ABS_VEF_1_CVF).v_Value1,
                               FET = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_FET).v_Value1,
                               FEV2575 = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCIÓN_RESPIRATORIA_ABS_FEF_25_75).v_Value1,
                               PEF = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_PEF).v_Value1,
                               CVFDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_CVF).v_Value1,
                               VEF1Des = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_VEF_1).v_Value1,
                               VEF1CVFDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_VEF_1_CVF).v_Value1,
                               FETDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_FET).v_Value1,
                               FEV2575Des = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_F_25_75).v_Value1,
                               PEFDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_PEF).v_Value1,
                               EdadPulmonar = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_EDAD_PULMONAR_ESTIMADA).v_Value1,
                               Resultado = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_RESULTADO_ID).v_Value1,
                               Observacion = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_OBSERVACION_ID).v_Value1,
                               FirmaRealizaEspirometria = a.FirmaRealizaEspirometria,
                               FirmaMedicoInterpreta = a.FirmaMedicoInterpreta,
                               Dx = DxEspirometria,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportOsteo> GetReportOsteo_(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join J in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                                             equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                                 from J in J_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportOsteo
                                 {
                                     ServiceId = A.v_ServiceId,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaMedico = pme.b_SignatureImage

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Osteo = ValoresComponente(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1)


                           select new Sigesoft.Node.WinClient.BE.ReportOsteo
                           {

                               ServiceId = a.ServiceId,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaMedico = a.FirmaMedico,

                               TareasHorasDias = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_DIAS).v_Value1,
                               TareasFrecuencia = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_FRECUENCIA).v_Value1,
                               TareasHorasSemana = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_SEMANA).v_Value1,
                               TareasTipo = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_TIPO).v_Value1,
                               TareasCiclo = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CICLO).v_Value1,
                               TareasCarga = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CARGA).v_Value1,
                               LateralCervical = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_CERVICAL).v_Value1,
                               LateralLumbar = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_LUMBAR).v_Value1,
                               LateralDorsal = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_DORSAL).v_Value1,
                               LordosisCervical = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_CERVICAL).v_Value1,
                               LordosisLumbar = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_LUMBAR).v_Value1,
                               EscoliosisLumbar = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CIFOSIS_DORSAL).v_Value1,
                               ContracturaMuscular = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUSCULAR).v_Value1,
                               //DolorEspalda = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DOLOR_ESPALDA).v_Value1,
                               ConclusionDescripcion = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID).v_Value1,
                               Dx = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1),
                               Aptitud = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_APTITUD_ID).v_Value1,
                               Recomendaciones = GetRecommendationByServiceIdAndComponent(a.ServiceId, Constants.OSTEO_MUSCULAR_ID_1),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportOsteo> GetReportOsteo(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join J in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                                             equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                                 from J in J_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportOsteo
                                 {
                                     ServiceId = A.v_ServiceId,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaMedico = pme.b_SignatureImage

                                 });

                var MedicalCenter = GetInfoMedicalCenter();
                var valorDx = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1);

                var sql = (from a in objEntity.ToList()
                           let Osteo = ValoresComponente(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1)
                           let OsteoDina = ValoresComponente(pstrserviceId, Constants.Eva_Dinamica_Osteomuscular)

                           select new Sigesoft.Node.WinClient.BE.ReportOsteo
                           {

                               ServiceId = a.ServiceId,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaMedico = a.FirmaMedico,

                               TareasHorasDias = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_DIAS) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_DIAS).v_Value1,
                               TareasFrecuencia = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_FRECUENCIA) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_FRECUENCIA).v_Value1,
                               TareasHorasSemana = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_SEMANA) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_SEMANA).v_Value1,
                               TareasTipo = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_TIPO) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_TIPO).v_Value1,
                               TareasCiclo = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CICLO) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CICLO).v_Value1,
                               TareasCarga = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CARGA) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CARGA).v_Value1,
                               LateralCervical = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_CERVICAL) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_CERVICAL).v_Value1,
                               LateralLumbar = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_LUMBAR) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_LUMBAR).v_Value1,
                               LateralDorsal = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_DORSAL) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LATERAL_DORSAL).v_Value1,
                               LordosisCervical = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_CERVICAL) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_CERVICAL).v_Value1,
                               LordosisLumbar = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_LUMBAR) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_LORDOSIS_LUMBAR).v_Value1,
                               EscoliosisLumbar = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_LUMBAR) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_LUMBAR).v_Value1Name,
                               EscoliosisDorsal = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_DORSAL) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_DORSAL).v_Value1Name,

                               ContracturaMuscular = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUSCULAR) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUSCULAR).v_Value1Name,
                               DolorEspalda = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DOLOR_ESPALDA) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DOLOR_ESPALDA).v_Value1Name,
                               ConclusionDescripcion = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID).v_Value1,
                               Dx = valorDx,
                               Aptitud = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_APTITUD_ID) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_APTITUD_ID).v_Value1,
                               Recomendaciones = GetRecommendationByServiceIdAndComponent(a.ServiceId, Constants.OSTEO_MUSCULAR_ID_1),


                               OSTEO_MUSCULAR_CONTRACTURA_MUS_CERVICAL = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUS_CERVICAL) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUS_CERVICAL).v_Value1Name,

                               OSTEO_MUSCULAR_CONTRACTURA_MUS_DORSAL = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUS_DORSAL) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUS_DORSAL).v_Value1Name,

                               OSTEO_MUSCULAR_CONTRACTURA_MUS_LUMBAR = Osteo.Count == 0 || Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUS_LUMBAR) == null ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CONTRACTURA_MUS_LUMBAR).v_Value1Name,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,



                               EVA_1 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_1) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_1).v_Value1Name,
                               EVA_2 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_2) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_2).v_Value1Name,
                               EVA_3 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_3) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_3).v_Value1Name,
                               EVA_4 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_4) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_4).v_Value1Name,
                               EVA_5 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_5) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_5).v_Value1Name,
                               EVA_6 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_6) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_6).v_Value1Name,
                               EVA_7 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_7) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_7).v_Value1Name,
                               EVA_8 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_8) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_8).v_Value1Name,
                               EVA_9 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_9) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_9).v_Value1Name,
                               EVA_10 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_10) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_10).v_Value1Name,

                               EVA_11 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_11) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_11).v_Value1Name,
                               EVA_12 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_12) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_12).v_Value1Name,
                               EVA_13 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_13) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_13).v_Value1Name,
                               EVA_14 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_14) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_14).v_Value1Name,
                               EVA_15 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_15) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_15).v_Value1Name,
                               EVA_16 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_16) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_16).v_Value1Name,
                               EVA_17 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_17) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_17).v_Value1Name,
                               EVA_18 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_18) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_18).v_Value1Name,
                               EVA_19 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_19) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_19).v_Value1Name,
                               EVA_20 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_20) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_20).v_Value1Name,

                               EVA_21 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_21) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_21).v_Value1Name,
                               EVA_22 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_22) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_22).v_Value1Name,
                               EVA_23 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_23) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_23).v_Value1Name,
                               EVA_24 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_24) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_24).v_Value1Name,
                               EVA_25 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_25) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_25).v_Value1Name,
                               EVA_26 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_26) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_26).v_Value1Name,
                               EVA_27 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_27) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_27).v_Value1Name,
                               EVA_28 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_28) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_28).v_Value1Name,
                               EVA_29 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_29) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_29).v_Value1Name,
                               EVA_30 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_30) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_30).v_Value1Name,

                               EVA_31 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_31) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_31).v_Value1Name,
                               EVA_32 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_32) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_32).v_Value1Name,
                               EVA_33 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_33) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_33).v_Value1Name,
                               EVA_34 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_34) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_34).v_Value1Name,
                               EVA_35 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_35) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_35).v_Value1Name,
                               EVA_36 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_36) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_36).v_Value1Name,
                               EVA_37 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_37) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_37).v_Value1Name,
                               EVA_38 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_38) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_38).v_Value1Name,
                               EVA_39 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_39) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_39).v_Value1Name,
                               EVA_40 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_40) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_40).v_Value1Name,

                               EVA_41 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_41) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_41).v_Value1Name,
                               EVA_42 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_42) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_42).v_Value1Name,
                               EVA_43 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_43) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_43).v_Value1Name,
                               EVA_44 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_44) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_44).v_Value1Name,
                               EVA_45 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_45) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_45).v_Value1Name,
                               EVA_46 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_46) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_46).v_Value1Name,
                               EVA_47 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_47) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_47).v_Value1Name,
                               EVA_48 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_48) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_48).v_Value1Name,
                               EVA_49 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_49) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_49).v_Value1Name,
                               EVA_50 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_50) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_50).v_Value1Name,

                               EVA_51 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_51) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_51).v_Value1Name,
                               EVA_52 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_52) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_52).v_Value1Name,
                               EVA_53 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_53) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_53).v_Value1Name,
                               EVA_54 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_54) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_54).v_Value1Name,
                               EVA_55 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_55) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_55).v_Value1Name,
                               EVA_56 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_56) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_56).v_Value1Name,
                               EVA_57 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_57) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_57).v_Value1Name,
                               EVA_58 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_58) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_58).v_Value1Name,
                               EVA_59 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_59) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_59).v_Value1Name,
                               EVA_60 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_60) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_60).v_Value1Name,

                               EVA_61 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_61) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_61).v_Value1Name,
                               EVA_62 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_62) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_62).v_Value1Name,
                               EVA_63 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_63) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_63).v_Value1Name,
                               EVA_64 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_64) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_64).v_Value1Name,
                               EVA_65 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_65) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_65).v_Value1Name,
                               EVA_66 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_66) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_66).v_Value1Name,
                               EVA_67 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_67) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_67).v_Value1Name,
                               EVA_68 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_68) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_68).v_Value1Name,
                               EVA_69 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_69) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_69).v_Value1Name,
                               EVA_70 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_70) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_70).v_Value1Name,

                               EVA_71 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_71) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_71).v_Value1Name,
                               EVA_72 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_72) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_72).v_Value1Name,
                               EVA_73 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_73) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_73).v_Value1Name,
                               EVA_74 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_74) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_74).v_Value1Name,
                               EVA_75 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_75) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_75).v_Value1Name,
                               EVA_76 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_76) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_76).v_Value1Name,
                               EVA_77 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_77) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_77).v_Value1Name,
                               EVA_78 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_78) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_78).v_Value1Name,
                               EVA_79 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_79) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_79).v_Value1Name,
                               EVA_80 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_80) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_80).v_Value1Name,

                               EVA_81 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_81) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_81).v_Value1Name,
                               EVA_82 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_82) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_82).v_Value1Name,
                               EVA_83 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_83) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_83).v_Value1Name,
                               EVA_84 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_84) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_84).v_Value1Name,
                               EVA_85 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_85) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_85).v_Value1Name,
                               EVA_86 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_86) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_86).v_Value1Name,
                               EVA_87 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_87) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_87).v_Value1Name,
                               EVA_88 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_88) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_88).v_Value1Name,
                               EVA_89 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_89) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_89).v_Value1Name,
                               EVA_90 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_90) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_90).v_Value1Name,

                               EVA_91 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_91) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_91).v_Value1Name,
                               EVA_92 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_92) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_92).v_Value1Name,
                               EVA_93 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_93) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_93).v_Value1Name,
                               EVA_94 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_94) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_94).v_Value1Name,
                               EVA_95 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_95) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_95).v_Value1Name,
                               EVA_96 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_96) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_96).v_Value1Name,
                               EVA_97 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_97) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_97).v_Value1Name,
                               EVA_98 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_98) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_98).v_Value1Name,
                               EVA_99 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_99) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_99).v_Value1Name,
                               EVA_100 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_100) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_100).v_Value1Name,

                               EVA_101 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_101) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_101).v_Value1Name,
                               EVA_102 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_102) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_102).v_Value1Name,
                               EVA_103 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_103) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_103).v_Value1Name,
                               EVA_104 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_104) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_104).v_Value1Name,
                               EVA_105 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_105) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_105).v_Value1Name,
                               EVA_106 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_106) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_106).v_Value1Name,
                               EVA_107 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_107) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_107).v_Value1Name,
                               EVA_108 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_108) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_108).v_Value1Name,
                               EVA_109 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_109) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_109).v_Value1Name,
                               EVA_110 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_110) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_110).v_Value1Name,

                               EVA_111 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_111) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_111).v_Value1Name,
                               EVA_112 = OsteoDina.Count == 0 || OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_112) == null ? "0" : OsteoDina.Find(p => p.v_ComponentFieldId == Constants.EVA_112).v_Value1Name,



                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string FormatearFecha(DateTime? nullable)
        {
            return nullable.Value.ToString("dd/MM/yyyy");
        }
        public List<Sigesoft.Node.WinClient.BE.ReportCuestionarioOjoSeco> GetReportCuestionarioOjoSeco(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 //join F in dbContext.organization on C.v_ equals D.v_OrganizationId into F_join
                                 //from F in F_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()



                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportCuestionarioOjoSeco
                                 {
                                     FechaSinFormatear = A.d_ServiceDate.Value,
                                     Trabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = D.v_Name

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()

                           let Test = ValoresComponente(pstrserviceId, Constants.TESTOJOSECO_ID)
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let Fecha = FormatearFecha(a.FechaSinFormatear)
                           select new Sigesoft.Node.WinClient.BE.ReportCuestionarioOjoSeco
                           {
                               Fecha = Fecha,
                               Trabajador = a.Trabajador,
                               FirmaMedico = a.FirmaMedico,
                               Empresa = a.Empresa,
                               LogoClinica = LogoEmpresa,
                               EnrojOcular = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_ENROJECIMIENTO_OCULAR).v_Value1,
                               ParpaInflama = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_BORDE_PARPADOS_IMFLAMADOS).v_Value1,
                               CostraParpa = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_COSTRAS_PARPADOS).v_Value1,
                               OjosPegados = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_OJO_PEGADOS_LEVANTARSE).v_Value1,
                               Secreciones = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_SECRECION).v_Value1,
                               SequedadOjo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_SEQUEDAD_OJO).v_Value1,
                               SensaArenilla = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_SEMSACION_ARENILLA).v_Value1,
                               SensaCuerpExtra = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_SENSACION_CUERPO_EXTRANO).v_Value1,
                               Ardor = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_ARDOR).v_Value1,
                               Picor = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_PICOR).v_Value1,
                               MalestarOjo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_MALESTAR_OJO).v_Value1,
                               DolorAgudo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_DOLO_AGUDO).v_Value1,
                               Lagrimeo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TESTOJOSECO_LAGRIMEO).v_Value1,
                               OjosLlorosos = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_SENTADO_LEYENDO).v_Value1,
                               SensibilidadLuz = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_MIRANDO_TV).v_Value1,
                               VisionBorrosa = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_SENTADO_QUIETO).v_Value1,
                               CansancioOjo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_VIAJANDO).v_Value1,
                               SensaPesadez = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_CONVERSANDO).v_Value1,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<Sigesoft.Node.WinClient.BE.ReportEvaluacionPsicolaboralPersonal> GetReportEvaluacionPsicolaborlaPersonal(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join J in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                                             equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                                 from J in J_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId



                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEvaluacionPsicolaboralPersonal
                                 {
                                     Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     Genero = J.v_Value1,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     PuestoPostula = B.v_CurrentOccupation,
                                     EmpresaCliente = D.v_Name,
                                     FechaEvaluacion = A.d_ServiceDate.Value,
                                     Evaluador = y.v_FirstName + " " + y.v_FirstLastName + " " + y.v_SecondLastName,
                                     Cpsp = pme.v_ProfessionalCode,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaProfesional = pme.b_SignatureImage


                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let EvaPsicolaborla = ValoresComponente(pstrserviceId, Constants.EVALUACION_PSICOLABORAL)
                           select new Sigesoft.Node.WinClient.BE.ReportEvaluacionPsicolaboralPersonal
                           {

                               Trabajador = a.Trabajador,
                               Genero = a.Genero,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               PuestoPostula = a.PuestoPostula,
                               EmpresaCliente = a.EmpresaCliente,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Cs = (from n in dbContext.organization
                                     where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                     select n.v_Name).SingleOrDefault<string>(),
                               Evaluador = a.Evaluador,
                               Cpsp = a.Cpsp,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaProfesional = a.FirmaProfesional,

                               _1 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._1).v_Value1,
                               _2 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._2).v_Value1,
                               _3 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._3).v_Value1,
                               _4 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._4).v_Value1,
                               _5 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._5).v_Value1,
                               _6 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._6).v_Value1,
                               _7 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._7).v_Value1,
                               _8 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._8).v_Value1,
                               _9 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._9).v_Value1,
                               _10 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._10).v_Value1,
                               _11 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._11).v_Value1,
                               _12 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._12).v_Value1,
                               _13 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._13).v_Value1,
                               _14 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._14).v_Value1,
                               _15 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._15).v_Value1,
                               _16 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._16).v_Value1,
                               _17 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._17).v_Value1,
                               _18 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._18).v_Value1,
                               _19 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._19).v_Value1,
                               _20 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._20).v_Value1,
                               _21 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._21).v_Value1,
                               _22 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._22).v_Value1,
                               _23 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._23).v_Value1,
                               _24 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._24).v_Value1,
                               _25 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._25).v_Value1,
                               _26 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._26).v_Value1,
                               _27 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._27).v_Value1,
                               _28 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._28).v_Value1,

                               Fatiga = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_ESCALA_FATIGA_OBSERVACIONES).v_Value1,
                               Recomendacion = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_ESCALA_FATIGA_RECOMENDACIONES).v_Value1,
                               Somnolencia = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_ESCALA_SOMNOLENCIA).v_Value1,
                               ConclusionFinal = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_CONCLUSION_FINAL_CONCLUSION).v_Value1,
                               Conclusion = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_CONCLUSION_FINAL_APTITUD).v_Value1,
                               RiesgoEstres = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_RIESGO_ESTRES).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetDiagnosticRepositoryByComponent(string pstrServiceId, string componentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var isDeleted = 0;
                var recomId = (int)Typifying.Recomendaciones;

                var qryRecom = (from dr in dbContext.diagnosticrepository
                                join a in dbContext.recommendation on dr.v_DiagnosticRepositoryId equals a.v_DiagnosticRepositoryId  // RECOMENDACIONES
                                join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId

                                where dr.v_ServiceId == pstrServiceId &&
                                      dr.v_ComponentId == componentId &&
                                      a.i_IsDeleted == isDeleted &&
                                      eee.i_TypifyingId == recomId
                                select new Sigesoft.Node.WinClient.BE.RecomendationList
                                {
                                    v_DiagnosticRepositoryId = dr.v_DiagnosticRepositoryId,
                                    v_RecommendationName = eee.v_Name,
                                    v_ServiceId = dr.v_ServiceId
                                }).ToList();

                var restricId = (int)Typifying.Restricciones;

                var qryRestric = (from dr in dbContext.diagnosticrepository
                                  join a in dbContext.restriction on dr.v_DiagnosticRepositoryId equals a.v_DiagnosticRepositoryId  // RECOMENDACIONES
                                  join eee in dbContext.masterrecommendationrestricction on a.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId

                                  where dr.v_ServiceId == pstrServiceId &&
                                        dr.v_ComponentId == componentId &&
                                        a.i_IsDeleted == isDeleted &&
                                        eee.i_TypifyingId == restricId
                                  select new Sigesoft.Node.WinClient.BE.RestrictionList
                                  {
                                      v_DiagnosticRepositoryId = dr.v_DiagnosticRepositoryId,
                                      v_RestrictionName = eee.v_Name,
                                      v_ServiceId = dr.v_ServiceId
                                  }).ToList();


                var query = (from ccc in dbContext.diagnosticrepository

                             //join sss in dbContext.service on ccc.v_ServiceId equals sss.v_ServiceId  // ESO

                             join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos

                             where (ccc.v_ServiceId == pstrServiceId) &&
                                   (ccc.v_ComponentId == componentId) &&
                                   (ccc.i_IsDeleted == 0) &&
                                   (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                                   ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo)

                             select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                             {
                                 v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                 v_ServiceId = ccc.v_ServiceId,
                                 v_DiseasesId = ccc.v_DiseasesId,
                                 i_AutoManualId = ccc.i_AutoManualId,
                                 i_PreQualificationId = ccc.i_PreQualificationId,
                                 i_FinalQualificationId = ccc.i_FinalQualificationId,
                                 i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,

                                 d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,

                                 v_DiseasesName = ddd.v_Name,
                                 v_ComponentFieldsId = ccc.v_ComponentFieldId

                             }).ToList();

                query.ForEach(a =>
                {
                    a.Recomendations = qryRecom.FindAll(p => p.v_DiagnosticRepositoryId == a.v_DiagnosticRepositoryId);
                    a.Restrictions = qryRestric.FindAll(p => p.v_DiagnosticRepositoryId == a.v_DiagnosticRepositoryId);
                });


                //var q = (from a in query
                //         select new DiagnosticRepositoryList
                //         {
                //             v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                //             v_ServiceId = a.v_ServiceId,
                //             v_DiseasesId = a.v_DiseasesId,
                //             i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                //             d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,



                //             v_DiseasesName = a.v_DiseasesName,
                //             v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                //             v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                //             v_AptitudeStatusName = a.v_AptitudeStatusName,
                //             v_OccupationName = a.v_OccupationName  // por ahora se muestra el GESO
                //         }).ToList();


                return query;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceList> ReportAscensoGrandesAlturas(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId into F_join
                                 //from F in F_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join G in dbContext.professional on new { a = me.v_PersonId }
                                                                      equals new { a = G.v_PersonId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                                 from J in J_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DireccionPaciente = B.v_AdressLocation,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber,
                                     i_SexTypeId = B.i_SexTypeId.Value,
                                     FirmaMedico = pme.b_SignatureImage,
                                     ApellidosDoctor = H.v_FirstLastName + " " + H.v_SecondLastName,
                                     NombreDoctor = H.v_FirstName,
                                     CMP = pme.v_ProfessionalCode,
                                     DireccionDoctor = H.v_AdressLocation,
                                     EmpresaEmpleadora = J.v_Name,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     v_ServiceComponentId = E.v_ServiceComponentId
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var funcionesVitales = ReportFuncionesVitales(pstrserviceId, Constants.FUNCIONES_VITALES_ID);
                var antropometria = ReportAntropometria(pstrserviceId, Constants.ANTROPOMETRIA_ID);

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_ServiceId = a.v_ServiceId,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               v_PersonId = a.v_PersonId,
                               v_NamePacient = a.v_NamePacient,
                               DireccionPaciente = a.DireccionPaciente,
                               v_Surnames = a.v_Surnames,
                               d_BirthDate = a.d_BirthDate,
                               i_AgePacient = GetAge(a.d_BirthDate.Value),
                               d_ServiceDate = a.d_ServiceDate,
                               v_DocNumber = a.v_DocNumber,
                               i_SexTypeId = a.i_SexTypeId,
                               FirmaMedico = a.FirmaMedico,
                               Anemia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ALTURA_7D_ANEMIA_ID, "NOCOMBO", 0, "SI"),
                               Cirugia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_CIRUGIA_MAYOR_CRECIENTE_ID, "NOCOMBO", 0, "SI"),
                               Desordenes = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_DESORDENES_COAGULACION_ID, "NOCOMBO", 0, "SI"),
                               Diabetes = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_DIABETES_MELLITUS_ID, "NOCOMBO", 0, "SI"),
                               Hipertension = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_HIPERTENSION_ARTERIAL_ID, "NOCOMBO", 0, "SI"),
                               Embarazo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_EMBARAZO_ID, "NOCOMBO", 0, "SI"),
                               ProbNeurologicos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_NEUROLOGICOS_ID, "NOCOMBO", 0, "SI"),
                               Infecciones = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_INFECCIONES_RECIENTES_ID, "NOCOMBO", 0, "SI"),
                               Obesidad = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_OBESIDAD_MORBIDA_ID, "NOCOMBO", 0, "SI"),
                               ProCardiacos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_CARDIACOS_ID, "NOCOMBO", 0, "SI"),
                               ProRespiratorios = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_RESPIRATORIOS_ID, "NOCOMBO", 0, "SI"),
                               ProOftalmologico = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_OFTALMOLOGICOS_ID, "NOCOMBO", 0, "SI"),
                               ProDigestivo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_DIGESTIVOS_ID, "NOCOMBO", 0, "SI"),
                               Apnea = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_APNEA_SUEÑO_ID, "NOCOMBO", 0, "SI"),
                               Otra = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_OTRA_CONDICON_ID, "NOCOMBO", 0, "SI"),
                               Alergia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_ALERGIAS_ID, "NOCOMBO", 0, "SI"),
                               MedicacionActual = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_USO_MEDICACION_ACTUAL_ID, "NOCOMBO", 0, "SI"),
                               AptoAscenderAlturas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_APTO_ASCENDER_GRANDES_ALTURAS_ID, "SICOMBO", 163, "NO"),
                               ActividadRealizar = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_ACTIVIDAD_REALIZAR_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               ApellidosDoctor = a.ApellidosDoctor,
                               NombreDoctor = a.NombreDoctor,
                               CMP = a.CMP,
                               DireccionDoctor = a.DireccionDoctor,
                               EmpresaEmpleadora = a.EmpresaEmpleadora,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               Descripcion = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ALTURA_7D_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetValueOdontogramaAusente(string pstrServiceId, string pstrComponentId, string pstrFieldId, string pstrPath)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontogramaAusente(pstrServiceId, pstrComponentId, pstrPath);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? String.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

                return xx;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontogramaAusente(string pstrServiceId, string pstrComponentId, string pstrPath)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)
                                                                                     let range = (D.v_Value1 == "1" ? pstrPath + "\\Resources\\ausent.png" :
                                                                                                 string.Empty
                                                                                                 )
                                                                                    select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         //v_Value1 =  D.v_Value1 == "1" ? pstrPath + "\\caries.png" ? D.v_Value1 == "2" ? pstrPath + "\\curacion.png" ?D.v_Value1 == "3" ? pstrPath + "\\ausent.png" : D.v_Value1, 
                                                                                         //v_Value1 = D.v_Value1
                                                                                         v_Value1 = range
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetNameMedicalCenter()
        {
            using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
            {

                var nameMedicalCenter = (from n in dbContext.organization
                                         where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                         select n.v_Name + " " + n.v_Address).SingleOrDefault<string>();

                return nameMedicalCenter;
            }
        }


        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.AudiometriaUserControlList> ReportAudiometriaUserControl(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var audiometriaList = new List<Sigesoft.Node.WinClient.BE.AudiometriaUserControlList>();

                var __sql = ValoresComponentesUserControl(pstrserviceId, pstrComponentId);

                if (__sql.Count == 0)
                    return audiometriaList;

                var multimediaFileId_OD = string.Empty;
                var multimediaFileId_OI = string.Empty;

                var xMultimediaFileId_OD = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_MULTIMEDIA_FILE_OD);
                if (xMultimediaFileId_OD != null)
                    multimediaFileId_OD = xMultimediaFileId_OD.v_Value1;

                var xMultimediaFileId_OI = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_MULTIMEDIA_FILE_OI);
                if (xMultimediaFileId_OI != null)
                    multimediaFileId_OI = xMultimediaFileId_OI.v_Value1;

                var img_OD = (from mf in dbContext.multimediafile where mf.v_MultimediaFileId == multimediaFileId_OD select mf.b_File).SingleOrDefault();
                var img_OI = (from mf in dbContext.multimediafile where mf.v_MultimediaFileId == multimediaFileId_OI select mf.b_File).SingleOrDefault();

                var ent = new Sigesoft.Node.WinClient.BE.AudiometriaUserControlList();
                // OD
                var xVA_OD_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_500);
                if (xVA_OD_500 != null)
                    ent.VA_OD_500 = xVA_OD_500.v_Value1;

                var xVA_OD_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_1000);
                if (xVA_OD_1000 != null)
                    ent.VA_OD_1000 = xVA_OD_1000.v_Value1;

                var xVA_OD_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_2000);
                if (xVA_OD_2000 != null)
                    ent.VA_OD_2000 = xVA_OD_2000.v_Value1;

                var xVA_OD_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_3000);
                if (xVA_OD_3000 != null)
                    ent.VA_OD_3000 = xVA_OD_3000.v_Value1;

                var xVA_OD_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_4000);
                if (xVA_OD_4000 != null)
                    ent.VA_OD_4000 = xVA_OD_4000.v_Value1;

                var xVA_OD_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_6000);
                if (xVA_OD_6000 != null)
                    ent.VA_OD_6000 = xVA_OD_6000.v_Value1;

                var xVA_OD_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_8000);
                if (xVA_OD_8000 != null)
                    ent.VA_OD_8000 = xVA_OD_8000.v_Value1;

                var xVO_OD_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_500);
                if (xVO_OD_500 != null)
                    ent.VO_OD_500 = xVO_OD_500.v_Value1;

                var xVO_OD_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_1000);
                if (xVO_OD_1000 != null)
                    ent.VO_OD_1000 = xVO_OD_1000.v_Value1;

                var xVO_OD_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_2000);
                if (xVO_OD_2000 != null)
                    ent.VO_OD_2000 = xVO_OD_2000.v_Value1;

                var xVO_OD_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_3000);
                if (xVO_OD_3000 != null)
                    ent.VO_OD_3000 = xVO_OD_3000.v_Value1;

                var xVO_OD_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_4000);
                if (xVO_OD_4000 != null)
                    ent.VO_OD_4000 = xVO_OD_4000.v_Value1;

                var xVO_OD_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_6000);
                if (xVO_OD_6000 != null)
                    ent.VO_OD_6000 = xVO_OD_6000.v_Value1;

                var xVO_OD_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_8000);
                if (xVO_OD_8000 != null)
                    ent.VO_OD_8000 = xVO_OD_8000.v_Value1;

                // OI
                var xVA_OI_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_500);
                if (xVA_OI_500 != null)
                    ent.VA_OI_500 = xVA_OI_500.v_Value1;

                var xVA_OI_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_1000);
                if (xVA_OI_1000 != null)
                    ent.VA_OI_1000 = xVA_OI_1000.v_Value1;

                var xVA_OI_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_2000);
                if (xVA_OI_2000 != null)
                    ent.VA_OI_2000 = xVA_OI_2000.v_Value1;

                var xVA_OI_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_3000);
                if (xVA_OI_3000 != null)
                    ent.VA_OI_3000 = xVA_OI_3000.v_Value1;

                var xVA_OI_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_4000);
                if (xVA_OI_4000 != null)
                    ent.VA_OI_4000 = xVA_OI_4000.v_Value1;

                var xVA_OI_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_6000);
                if (xVA_OI_6000 != null)
                    ent.VA_OI_6000 = xVA_OI_6000.v_Value1;

                var xVA_OI_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_8000);
                if (xVA_OI_8000 != null)
                    ent.VA_OI_8000 = xVA_OI_8000.v_Value1;

                var xVO_OI_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_500);
                if (xVO_OI_500 != null)
                    ent.VO_OI_500 = xVO_OI_500.v_Value1;

                var xVO_OI_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_1000);
                if (xVO_OI_1000 != null)
                    ent.VO_OI_1000 = xVO_OI_1000.v_Value1;

                var xVO_OI_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_2000);
                if (xVO_OI_2000 != null)
                    ent.VO_OI_2000 = xVO_OI_2000.v_Value1;

                var xVO_OI_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_3000);
                if (xVO_OI_3000 != null)
                    ent.VO_OI_3000 = xVO_OI_3000.v_Value1;

                var xVO_OI_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_4000);
                if (xVO_OI_4000 != null)
                    ent.VO_OI_4000 = xVO_OI_4000.v_Value1;

                var xVO_OI_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_6000);
                if (xVO_OI_6000 != null)
                    ent.VO_OI_6000 = xVO_OI_6000.v_Value1;

                var xVO_OI_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_8000);
                if (xVO_OI_8000 != null)
                    ent.VO_OI_8000 = xVO_OI_8000.v_Value1;


                ent.b_AudiogramaOD = img_OD;
                ent.b_AudiogramaOI = img_OI;

                audiometriaList.Add(ent);

                return audiometriaList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.AudiometriaList> ReportAudiometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************  

                                 join pro in dbContext.protocol on A.v_ProtocolId equals pro.v_ProtocolId

                                 join H in dbContext.systemparameter on new { a = pro.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                                 // Empresa / Sede Trabajo  ********************************************************
                                 join ow in dbContext.organization on new { a = pro.v_WorkingOrganizationId }
                                         equals new { a = ow.v_OrganizationId } into ow_join
                                 from ow in ow_join.DefaultIfEmpty()

                                 join lw in dbContext.location on new { a = pro.v_WorkingOrganizationId, b = pro.v_WorkingLocationId }
                                      equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                                 from lw in lw_join.DefaultIfEmpty()

                                 //************************************************************************************


                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.AudiometriaList
                                 {
                                     v_PersonId = A.v_PersonId,

                                     v_FullPersonName = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,

                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,

                                     v_DocNumber = B.v_DocNumber,
                                     i_SexTypeId = B.i_SexTypeId.Value,

                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     FirmaMedico = pme.b_SignatureImage,

                                     Puesto = B.v_CurrentOccupation,
                                     v_SexType = B.i_SexTypeId == (int)Gender.MASCULINO ? "M" : "F",
                                     //
                                     v_EsoTypeName = H.v_Value1,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     v_WorkingOrganizationName = ow.v_Name,
                                     v_FullWorkingOrganizationName = ow.v_Name + " / " + lw.v_Name,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,


                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var __sql = ValoresComponente(pstrserviceId, pstrComponentId);

                //// Requisitos para la Audiometria                         
                //          var CambiosAltitud = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CAMBIOS_ALTITUD).v_Value1;
                //           var ExpuestoRuido = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_EXPUESTO_RUIDO).v_Value1;
                //           var ProcesoInfeccioso = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_PROCESO_INFECCIOSO).v_Value1;
                //           var DurmioNochePrevia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_DURMIO_NOCHE_PREVIA).v_Value1;
                //           var ConsumioAlcoholDiaPrevio = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CONSUMIO_ALCOHOL_DIA_PREVIO).v_Value1;

                //           //// Antecedentes Medicos de importancia

                //           var RinitisSinusitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_RINITIS_SINUSITIS).v_Value1;
                //           var UsoMedicamentos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_USO_MEDICAMENTOS).v_Value1;
                //           var Sarampion = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SARAMPION).v_Value1;
                //           var Tec = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_TEC).v_Value1;
                //           var OtitisMediaCronica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_OTITIS_MEDIA_CRONICA).v_Value1;
                //          var  DiabetesMellitus = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DIABETES_MELLITUS).v_Value1;
                //           var SorderaAntecedente = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA).v_Value1;
                //           var SorderaFamiliar = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA_FAMILIAR).v_Value1;
                //          var  Meningitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_MENINGITIS).v_Value1;
                //           var Dislipidemia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DISLIPIDEMIA).v_Value1;
                //           var EnfTiroidea = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_ENF_TIROIDEA).v_Value1;
                //           var SustQuimicas = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SUST_QUIMICAS).v_Value1;

                //           //// Hobbies

                //           var UsoMP3 = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_USO_MP3).v_Value1;
                //           var PracticaTiro = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_PRACTICA_TIRO).v_Value1;
                //           var Otros = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_OTROS).v_Value1;

                //           //// Sintomas actuales

                //           var Sordera = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SORDERA).v_Value1;
                //           var Otalgia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_OTALGIA).v_Value1;
                //           var Acufenos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_ACUFENOS).v_Value1;
                //           var SecrecionOtica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SECRECION_OTICA).v_Value1;
                //           var Vertigos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_VERTIGOS).v_Value1;

                //           //// Otoscopia

                //           var OidoIzquierdo = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_IZQUIERDO).v_Value1;
                //           var OidoDerecho = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_DERECHO).v_Value1;


                var sql = (from a in objEntity.ToList()

                           select new Sigesoft.Node.WinClient.BE.AudiometriaList
                           {

                               v_PersonId = a.v_PersonId,
                               v_FullPersonName = a.v_FullPersonName,

                               d_BirthDate = a.d_BirthDate,
                               i_AgePacient = GetAge(a.d_BirthDate.Value),
                               d_ServiceDate = a.d_ServiceDate,
                               v_DocNumber = a.v_DocNumber,
                               i_SexTypeId = a.i_SexTypeId,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTecnologo = a.FirmaTecnologo,
                               Puesto = a.Puesto,
                               v_SexType = a.v_SexType,

                               // Requisitos para la Audiometria                         
                               CambiosAltitud = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CAMBIOS_ALTITUD).v_Value1,
                               ExpuestoRuido = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_EXPUESTO_RUIDO).v_Value1,
                               ProcesoInfeccioso = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_PROCESO_INFECCIOSO).v_Value1,
                               DurmioNochePrevia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_DURMIO_NOCHE_PREVIA).v_Value1,
                               ConsumioAlcoholDiaPrevio = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CONSUMIO_ALCOHOL_DIA_PREVIO).v_Value1,

                               //// Antecedentes Medicos de importancia

                               RinitisSinusitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_RINITIS_SINUSITIS).v_Value1,
                               UsoMedicamentos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_USO_MEDICAMENTOS).v_Value1,
                               Sarampion = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SARAMPION).v_Value1,
                               Tec = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_TEC).v_Value1,
                               OtitisMediaCronica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_OTITIS_MEDIA_CRONICA).v_Value1,
                               DiabetesMellitus = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DIABETES_MELLITUS).v_Value1,
                               SorderaAntecedente = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA).v_Value1,
                               SorderaFamiliar = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA_FAMILIAR).v_Value1,
                               Meningitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_MENINGITIS).v_Value1,
                               Dislipidemia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DISLIPIDEMIA).v_Value1,
                               EnfTiroidea = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_ENF_TIROIDEA).v_Value1,
                               SustQuimicas = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SUST_QUIMICAS).v_Value1,

                               //// Hobbies

                               UsoMP3 = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_USO_MP3).v_Value1,
                               PracticaTiro = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_PRACTICA_TIRO).v_Value1,
                               Otros = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_OTROS).v_Value1,

                               //// Sintomas actuales

                               Sordera = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SORDERA).v_Value1,
                               Otalgia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_OTALGIA).v_Value1,
                               Acufenos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_ACUFENOS).v_Value1,
                               SecrecionOtica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SECRECION_OTICA).v_Value1,
                               Vertigos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_VERTIGOS).v_Value1,

                               //// Otoscopia

                               OidoIzquierdo = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_IZQUIERDO).v_Value1,
                               OidoDerecho = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_DERECHO).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               //
                               v_EsoTypeName = a.v_EsoTypeName,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               v_WorkingOrganizationName = a.v_WorkingOrganizationName,
                               v_FullWorkingOrganizationName = a.v_FullWorkingOrganizationName,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               MarcaAudiometria = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_DATOS_DEL_AUDIOMETRO_MARCA).v_Value1,
                               ModeloAudiometria = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_DATOS_DEL_AUDIOMETRO_MODELO).v_Value1,
                               CalibracionAudiometria = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_DATOS_DEL_AUDIOMETRO_CALIBRACION).v_Value1,
                               TiempoTrabajo = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_TIEMPO_DE_TRABAJO).v_Value1,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponentesUserControl(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            int rpta = 0;

            try
            {
                var serviceComponentFieldValues = (from A in dbContext.service
                                                   join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                   join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                   join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                   where A.v_ServiceId == pstrServiceId
                                                           && B.v_ComponentId == pstrComponentId
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                   {
                                                       v_ComponentFieldId = C.v_ComponentFieldId,
                                                       //v_ComponentFielName = G.v_TextLabel,
                                                       v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                       v_Value1 = D.v_Value1,
                                                       //i_GroupId = G.i_GroupId.Value
                                                   });


                return serviceComponentFieldValues.ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Sigesoft.Node.WinClient.BE.ServiceList GetDoctoPhisicalExam(string serviceId)
        {
            using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
            {
                var sql = (from x in dbContext.servicecomponent
                           join me in dbContext.systemuser on x.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                           from me in me_join.DefaultIfEmpty()

                           join Pe in dbContext.person on me.v_PersonId equals Pe.v_PersonId

                           join pr in dbContext.professional on Pe.v_PersonId equals pr.v_PersonId into pr_join
                           from pr in pr_join.DefaultIfEmpty()

                           where x.v_ComponentId == Constants.EXAMEN_FISICO_ID &&
                                 x.v_ServiceId == serviceId

                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_Pacient = Pe.v_FirstName + " " + Pe.v_FirstLastName + " " + Pe.v_SecondLastName,
                               FirmaDoctor = pr.b_SignatureImage

                           }).FirstOrDefault();

                return sql;
            }
        }

        private string GetRestricctionByServiceId(string pstrServiceId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.restriction
                         join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRestrictionId equals ddd.v_MasterRecommendationRestricctionId  // Diagnosticos      
                         where ccc.v_ServiceId == pstrServiceId &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name

                         }).ToList();


            return string.Join("/ ", query.Select(p => p.v_DiseasesName));
        }

        public string GetDiagnosticForAudiometria(string pstrServiceId, string pstrComponent)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.diagnosticrepository
                         join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                         from ddd in ddd_join.DefaultIfEmpty()

                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name
                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_DiseasesName));
        }

        public List<Sigesoft.Node.WinClient.BE.ReportAutorizacionDosajeDrogas> GetReportAutorizacionDosajeDrogas(string pstrserviceId, string pstrComponentId, List<ServiceComponentList> pListaDosaje)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportAutorizacionDosajeDrogas
                                 {
                                     FechaSinformatear = A.d_ServiceDate.Value,
                                     Trabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     EmpresaTrabajador = D.v_Name,
                                     FechaNacimiento = E.d_Birthdate.Value,
                                     FirmaTrabajador = E.b_RubricImage,
                                     HuellaTrabajador = E.b_FingerPrintImage,
                                     Puesto = E.v_CurrentOccupation,
                                     Empresa = D.v_Name,
                                     Dni = E.v_DocNumber
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()

                           let Test = ValoresComponente(pstrserviceId, Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID)
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let Edad = GetAge(a.FechaNacimiento)
                           let ValorFecha = FormatearFecha(a.FechaSinformatear)
                           let ValoresDosaje = DevolverExamensConResultados(pstrserviceId, pListaDosaje)

                           select new Sigesoft.Node.WinClient.BE.ReportAutorizacionDosajeDrogas
                           {
                               LogoEmpresa = LogoEmpresa,
                               Fecha = ValorFecha,
                               Trabajador = a.Trabajador,
                               Dni = a.Dni,
                               Clinica = MedicalCenter.v_Name,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               EmpresaTrabajador = a.EmpresaTrabajador,
                               Edad = Edad,
                               Puesto = a.Puesto,
                               Empresa = a.Empresa,
                               SufreEnfermedadSiNo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_SUFRE_ENFER).v_Value1,
                               EnfermedadCual = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_SUFRE_ENFER_CUAL).v_Value1,
                               ConsumeMedicamentoSiNo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_LUEGO_COMIDA).v_Value1,
                               ConsumeMedicamentoCual = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_CONDUCIENDO).v_Value1,
                               TomaCocaSiNo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_DESCANSAR).v_Value1,
                               TomaCocaCuantas = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_TOMA_MATECOCA_CUANTAS_VECES_SEMANA1).v_Value1,
                               ConsumioUltimo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_TOMA_MATECOCA_ULTIMA_VEZ).v_Value1,
                               ConsumeProductoCocaSiNo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_CONSUME_HABITUALMENTE_PROD_COCA).v_Value1,
                               ConsumeProductoCocaCuantas = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_CUANTAS_VECES_SEMANA2).v_Value1,
                               ConsumeProductoCocaUltima = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_CONSUME_HABITUALMENTE_PROD_COCA_CONSUMIO_ULTIMA_VEZ).v_Value1,
                               TratamientoAnestesiaSiNo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_TRATAMIENTO_ANESTESIA).v_Value1,
                               Metodo = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_METODO).v_Value1,
                               Muestra = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MUESTRA).v_Value1,
                               Cocaina = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_COCAINA).v_Value1Name,
                               Marihuana = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MARIHUANA).v_Value1Name,
                               Resultado = ValoresDosaje
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string DevolverExamensConResultados(string seviceId, List<ServiceComponentList> pListaDosaje)
        {
            try
            {


                string Resultado = "";
                string[] Examenes = new string[] 
            {
            Constants.TOXICOLOGICO_COLINESTERASA,
            Constants.TOXICOLOGICO_CARBOXIHEMOGLOBINA,
            Constants.TOXICOLOGICO_BENZODIAZEPINAS,
            Constants.TOXICOLOGICO_ALCOHOLEMIA,
            Constants.TOXICOLOGICO_ANFETAMINAS,
            Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID,
            };


                var Lista = pListaDosaje.FindAll(P => Examenes.Contains(P.v_ComponentId));


                foreach (var item in Lista)
                {
                    var Valor = ValoresComponente(seviceId, item.v_ComponentId);

                    if (item.v_ComponentId == Constants.TOXICOLOGICO_COLINESTERASA)
                    {
                        Resultado = Resultado + "COLINESTERASA                                            " + Valor.Find(p => p.v_ComponentFieldId == Constants.TOXICOLOGICO_COLINESTERASA_RESULTADO).v_Value1Name + "\n";
                    }
                    else if (item.v_ComponentId == Constants.TOXICOLOGICO_CARBOXIHEMOGLOBINA)
                    {
                        Resultado = Resultado + "CARBOXIHEMOGLOBINA                                      " + Valor.Find(p => p.v_ComponentFieldId == Constants.TOXICOLOGICO_CARBOXIHEMOGLOBINA_RESULTADO).v_Value1Name + "\n";
                    }
                    else if (item.v_ComponentId == Constants.TOXICOLOGICO_BENZODIAZEPINAS)
                    {
                        Resultado = Resultado + "BENZODIAZEPINAS                                          " + Valor.Find(p => p.v_ComponentFieldId == Constants.TOXICOLOGICO_BENZODIAZEPINAS_RESULTADO).v_Value1Name + "\n";
                    }
                    else if (item.v_ComponentId == Constants.TOXICOLOGICO_ALCOHOLEMIA)
                    {
                        Resultado = Resultado + "ALCOHOLEMIA                                              " + Valor.Find(p => p.v_ComponentFieldId == Constants.TOXICOLOGICO_ALCOHOLEMIA_RESULTADO).v_Value1Name + "\n";
                    }
                    else if (item.v_ComponentId == Constants.TOXICOLOGICO_ANFETAMINAS)
                    {
                        Resultado = Resultado + "ANFETAMINAS                                              " + Valor.Find(p => p.v_ComponentFieldId == Constants.TOXICOLOGICO_ANFETAMINAS_RESULTADO).v_Value1Name + "\n";
                    }
                    else if (item.v_ComponentId == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID)
                    {
                        Resultado = Resultado + "MARIHUANA                                                " + Valor.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MARIHUANA).v_Value1Name + "\n";
                        Resultado = Resultado + "COCAINA                                                  " + Valor.Find(p => p.v_ComponentFieldId == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_COCAINA).v_Value1Name + "\n";

                    }
                }

                return Resultado;
            }
            catch (Exception)
            {

                throw;
            }






        }

        public List<Sigesoft.Node.WinClient.BE.ReportAntigenosFebriles> GetReportAntigenosFebriles(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportAntigenosFebriles
                                 {
                                     FechaSinformatear = A.d_ServiceDate.Value,
                                     Trabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     EmpresaTrabajador = D.v_Name,
                                     Puesto = E.v_CurrentOccupation
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let ValorFecha = FormatearFecha(a.FechaSinformatear)
                           let Test = ValoresComponente(pstrserviceId, Constants.AGLUTINACIONES_LAMINA_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportAntigenosFebriles
                           {
                               LogoEmpresa = LogoEmpresa,
                               Trabajador = a.Trabajador,
                               EmpresaTrabajador = a.EmpresaTrabajador,
                               Puesto = a.Puesto,
                               Fecha = ValorFecha,
                               FirmaMedico = a.FirmaMedico,

                               ParatificoA = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_PARATIFICO_A).v_Value1Name,
                               ParatificoADeseable = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_PARATIFICO_A_DESEABLE).v_Value1,

                               ParatificoB = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_PARATIFICO_B).v_Value1Name,
                               ParatificoBDeseable = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_PARATIFICO_B_DESEABLE).v_Value1,

                               TificoH = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_TIFICO_H).v_Value1Name,
                               TificoHDeseable = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_TIFICO_H_DESEABLE).v_Value1,

                               TificoO = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_TIFICO_O).v_Value1Name,
                               TificoODeseable = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_TIFICO_O_DESEABLE).v_Value1,

                               Brucella = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_BRUCELLA).v_Value1Name,
                               BrucellaDeseable = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.AGLUTINACIONES_LAMINA_REACTIVOS_BRUCELLA_DESEABLE).v_Value1,


                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportCuestionarioActividadFisica> GetReportCuestionarioActividadFisica(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportCuestionarioActividadFisica
                                 {
                                     Nombre_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     IdServicio = A.v_ServiceId

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.CUESTIONARIO_ACTIVIDAD_FISICA)

                           select new Sigesoft.Node.WinClient.BE.ReportCuestionarioActividadFisica
                           {
                               Nombre_Trabajador = a.Nombre_Trabajador,
                               FechaServicio = a.FechaServicio,
                               IdServicio = a.IdServicio,


                               CUESTIONARIO_ACTIVIDAD_FISICA_1 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_1) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_1).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_2 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_2) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_2).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_3 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_3) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_3).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_4 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_4) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_4).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_5 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_5) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_5).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_6 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_6) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_6).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_7 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_7) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_7).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_8 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_8) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_8).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_9 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_9) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_9).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_10 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_10) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_10).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_11 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_11) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_11).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_12 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_12) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_12).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_13 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_13) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_13).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_14 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_14) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_14).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_15 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_15) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_15).v_Value1,
                               CUESTIONARIO_ACTIVIDAD_FISICA_16 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_16) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.CUESTIONARIO_ACTIVIDAD_FISICA_16).v_Value1,


                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportCuestionarioNordico> GetReportCuestionarioNordico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportCuestionarioNordico
                                 {
                                     Nombre_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = B.i_SexTypeId.Value

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.C_N_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportCuestionarioNordico
                           {
                               Nombre_Trabajador = a.Nombre_Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Genero = a.Genero,
                               Edad = GetAge(a.FechaNacimiento),

                               C_N_CABECERA_TIPO_TRABAJO_REALIZA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_TIPO_TRABAJO_REALIZA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_TIPO_TRABAJO_REALIZA_ID).v_Value1,
                               C_N_CABECERA_TIEMPO_LABOR_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_HORAS_TRABAJO_SEMANAL_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_TIEMPO_LABOR_ID).v_Value1,
                               C_N_CABECERA_HORAS_TRABAJO_SEMANAL_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_HORAS_SEMANA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_HORAS_TRABAJO_SEMANAL_ID).v_Value1,
                               C_N_CABECERA_DIESTRO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_DIESTRO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_DIESTRO_ID).v_Value1,
                               C_N_CABECERA_ZURDO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_ZURDO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_ZURDO_ID).v_Value1,

                               C_N_LOCOMOCION_TODOS_CUELLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CUELLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CUELLOS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_HOMBROS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_CODOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CODOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CODOS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_MUÑECA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_MUÑECA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_MUÑECA_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_ESPALDA_ALTA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_ALTA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_ALTA_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_BAJA_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_CADERAS_MUSLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CADERAS_MUSLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CADERAS_MUSLOS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_RODILLAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_RODILLAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_RODILLAS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_TOBILLOS_PIES_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_TOBILLOS_PIES_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_TOBILLOS_PIES_ID).v_Value1,

                               C_N_LOCOMOCION_12_MESES_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CUELLO_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_HOMBROS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_CODOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CODOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CODOS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_MUÑECA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_MUÑECA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_MUÑECA_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_ESPALDA_ALTA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_ALTA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_ALTA_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_BAJA_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_CADERAS_MUSLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CADERAS_MUSLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CADERAS_MUSLOS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_RODILLAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_RODILLAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_RODILLAS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_TOBILLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_TOBILLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_TOBILLOS_ID).v_Value1,

                               C_N_LOCOMOCION_7_DIAS_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CUELLO_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_HOMBROS_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_CODOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CODOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CODOS_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_MUÑECA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_MUÑECA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_MUÑECA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_ESPALDA_ALTA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_ALTA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_ALTA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_BAJA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_CADERA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CADERA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CADERA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_RODILLAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_RODILLAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_RODILLAS_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_TOBILLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_TOBILLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_TOBILLOS_ID).v_Value1,


                               C_N_ESPALDA_BAJA_PROBLEMAS_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_PROBLEMAS_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_PROBLEMAS_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_HOSPITALIZADO_PROBLEMA_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_HOSPITALIZADO_PROBLEMA_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_HOSPITALIZADO_PROBLEMA_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_CAMBIOS_TRABAJO_ACTIVIDAD_PROBLEMA_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CAMBIOS_TRABAJO_ACTIVIDAD_PROBLEMA_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CAMBIOS_TRABAJO_ACTIVIDAD_PROBLEMA_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_CURACION_TOTAL_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CURACION_TOTAL_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CURACION_TOTAL_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_ACTIVIDAD_TRABAJO_1_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_TRABAJO_1_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_TRABAJO_1_ID).v_Value1,
                               C_N_ESPALDA_BAJA_ACTIVIDAD_RECREATIVA_1_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_RECREATIVA_1_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_RECREATIVA_1_ID).v_Value1,

                               C_N_DURACION_PRBLEMAS_IMPEDIR_RUTINA_1_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_DURACION_PRBLEMAS_IMPEDIR_RUTINA_1_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_DURACION_PRBLEMAS_IMPEDIR_RUTINA_1_ID).v_Value1,
                               C_N_VISTO_PROFESIONAL_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_VISTO_PROFESIONAL_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_VISTO_PROFESIONAL_ESPALDA_BAJA_ID).v_Value1,
                               C_N_PROBLEMAS_ESPALDA_BAJA_7_DIAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_ESPALDA_BAJA_7_DIAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_ESPALDA_BAJA_7_DIAS_ID).v_Value1,


                               C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_LESION_HOMBROS_ACCIDENTES_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_LESION_HOMBROS_ACCIDENTES_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_LESION_HOMBROS_ACCIDENTES_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_CAMBIO_TRABAJO_ACTIVIDAD_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_CAMBIO_TRABAJO_ACTIVIDAD_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_CAMBIO_TRABAJO_ACTIVIDAD_HOMBROS_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ULTIMOS_12_MESES_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ULTIMOS_12_MESES_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ULTIMOS_12_MESES_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_TIEMPO_TOTAL_PROBLEMAS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_TIEMPO_TOTAL_PROBLEMAS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_TIEMPO_TOTAL_PROBLEMAS_HOMBROS_ID).v_Value1,


                               C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_TRABAJO_2_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_TRABAJO_2_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_TRABAJO_2_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_RECREATIVA_2_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_RECREATIVA_2_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_RECREATIVA_2_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_DURACION_PRBLEMAS_IMPEDIR_RUTINA_2_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_DURACION_PRBLEMAS_IMPEDIR_RUTINA_2_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_DURACION_PRBLEMAS_IMPEDIR_RUTINA_2_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_VISTO_PROFESIONAL_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_VISTO_PROFESIONAL_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_VISTO_PROFESIONAL_HOMBROS_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_DURANTE_7_DIAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_DURANTE_7_DIAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_DURANTE_7_DIAS_ID).v_Value1,

                               C_N_PROBLEMA_CUELLO_PROBLEMA_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMA_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMA_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_LESIONADO_CUELLO_ACCIDENTE_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_LESIONADO_CUELLO_ACCIDENTE_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_LESIONADO_CUELLO_ACCIDENTE_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_CAMBIO_TRABAJO_PROBLEMA_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_CAMBIO_TRABAJO_PROBLEMA_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_CAMBIO_TRABAJO_PROBLEMA_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_DURACION_TOTAL_TIEMPO_PROBLEMA_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_TOTAL_TIEMPO_PROBLEMA_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_TOTAL_TIEMPO_PROBLEMA_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_DURACION_PRBLEMAS_IMPEDIR_RUTINA_3_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_PRBLEMAS_IMPEDIR_RUTINA_3_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_PRBLEMAS_IMPEDIR_RUTINA_3_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_VISTO_PROFESIONAL_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_VISTO_PROFESIONAL_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_VISTO_PROFESIONAL_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_PROBLEMAS_CUELLO_DURANTE_7_DIAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMAS_CUELLO_DURANTE_7_DIAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMAS_CUELLO_DURANTE_7_DIAS_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_ACTIVIDAD_TRABAJO_3_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_TRABAJO_3_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_TRABAJO_3_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_ACTIVIDAD_RECREATIVA_3_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_RECREATIVA_3_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_RECREATIVA_3_ID).v_Value1,


                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportEvaCardiologica> GetReportEvaluacionCardiologia(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on A.i_InsertUserOccupationalMedicalId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEvaCardiologica
                                 {
                                     NombreTrabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Fecha = A.d_ServiceDate.Value,
                                     FirmaMedico = pme.b_SignatureImage,
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let EvaCardio = ValoresComponente(pstrserviceId, Constants.EVA_CARDIOLOGICA_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportEvaCardiologica
                           {
                               NombreTrabajador = a.NombreTrabajador,
                               Fecha = a.Fecha,
                               FirmaMedico = a.FirmaMedico,
                               EVA_CARDIOLOGICA_SOPLO_CARDIACO = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_SOPLO_CARDIACO) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_SOPLO_CARDIACO).v_Value1,
                               EVA_CARDIOLOGICA_PRESION_ALTA = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PRESION_ALTA) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PRESION_ALTA).v_Value1,
                               EVA_CARDIOLOGICA_CANSANCIO_RAPIDO = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_CANSANCIO_RAPIDO) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_CANSANCIO_RAPIDO).v_Value1,
                               EVA_CARDIOLOGICA_MAREOS = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_MAREOS) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_MAREOS).v_Value1,
                               EVA_CARDIOLOGICA_DOLOR_PRECORDIAL = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DOLOR_PRECORDIAL) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DOLOR_PRECORDIAL).v_Value1,
                               EVA_CARDIOLOGICA_PALPITACIONES = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PALPITACIONES) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PALPITACIONES).v_Value1,
                               EVA_CARDIOLOGICA_ATAQUE_CORAZON = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ATAQUE_CORAZON) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ATAQUE_CORAZON).v_Value1,
                               EVA_CARDIOLOGICA_PERDIDA_CONCIENCIA = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PERDIDA_CONCIENCIA) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PERDIDA_CONCIENCIA).v_Value1,
                               EVA_CARDIOLOGICA_OBESIDAD = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_OBESIDAD) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_OBESIDAD).v_Value1,
                               EVA_CARDIOLOGICA_TABAQUISMO = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_TABAQUISMO) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_TABAQUISMO).v_Value1,
                               EVA_CARDIOLOGICA_DIABETES = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DIABETES) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DIABETES).v_Value1,
                               EVA_CARDIOLOGICA_DISLIPIDEMIA = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DISLIPIDEMIA) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DISLIPIDEMIA).v_Value1,
                               EVA_CARDIOLOGICA_VARICES_PIERNAS = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_VARICES_PIERNAS) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_VARICES_PIERNAS).v_Value1,
                               EVA_CARDIOLOGICA_SEDENTARISMO = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_SEDENTARISMO) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_SEDENTARISMO).v_Value1,
                               EVA_CARDIOLOGICA_OTROS = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_OTROS) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_OTROS).v_Value1,

                               EVA_CARDIOLOGICA_PRECORDIAL_1 = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PRECORDIAL_1) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PRECORDIAL_1).v_Value1,
                               EVA_CARDIOLOGICA_DESMAYOS = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DESMAYOS) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DESMAYOS).v_Value1,
                               EVA_CARDIOLOGICA_PALPITACIONES_1 = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PALPITACIONES_1) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PALPITACIONES_1).v_Value1,
                               EVA_CARDIOLOGICA_DISNEA_PAROXISTICA = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DISNEA_PAROXISTICA) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_DISNEA_PAROXISTICA).v_Value1,
                               EVA_CARDIOLOGICA_MAREOS_1 = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_MAREOS_1) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_MAREOS_1).v_Value1,
                               EVA_CARDIOLOGICA_CLAUDICACION = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_CLAUDICACION) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_CLAUDICACION).v_Value1,
                               EVA_CARDIOLOGICA_OTROS_1 = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_OTROS_1) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_OTROS_1).v_Value1,

                               EVA_CARDIOLOGICA_FREC_CARDIACA = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_FREC_CARDIACA) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_FREC_CARDIACA).v_Value1,
                               EVA_CARDIOLOGICA_PRESION_ARTERIAL = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PRESION_ARTERIAL) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PRESION_ARTERIAL).v_Value1,
                               EVA_CARDIOLOGICA_CHOQUE_PUNTA = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_CHOQUE_PUNTA) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_CHOQUE_PUNTA).v_Value1,

                               EVA_CARDIOLOGICA_RITMO = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_RITMO) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_RITMO).v_Value1,
                               EVA_CARDIOLOGICA_EJE = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_EJE) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_EJE).v_Value1,
                               EVA_CARDIOLOGICA_FC = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_FC) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_FC).v_Value1,
                               EVA_CARDIOLOGICA_PR = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PR) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_PR).v_Value1,
                               EVA_CARDIOLOGICA_QRS = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_QRS) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_QRS).v_Value1,
                               EVA_CARDIOLOGICA_QT = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_QT) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_QT).v_Value1,
                               EVA_CARDIOLOGICA_ONDA_Q = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_Q) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_Q).v_Value1,
                               EVA_CARDIOLOGICA_ONDA_P = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_P) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_P).v_Value1,
                               EVA_CARDIOLOGICA_ONDA_R = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_R) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_R).v_Value1,
                               EVA_CARDIOLOGICA_ONDA_S = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_S) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_S).v_Value1,
                               EVA_CARDIOLOGICA_ONDA_T = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_T) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_T).v_Value1,
                               EVA_CARDIOLOGICA_ONDA_U = EvaCardio.Count == 0 || EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_U) == null ? string.Empty : EvaCardio.Find(p => p.v_ComponentFieldId == Constants.EVA_CARDIOLOGICA_ONDA_U).v_Value1,


                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportExamenOrinaCompleto> GetReportExamenCompletoOrina(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportExamenOrinaCompleto
                                 {
                                     FechaSinformatear = A.d_ServiceDate.Value,
                                     Paciente = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = D.v_Name,
                                     Puesto = E.v_CurrentOccupation
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let ValorFecha = FormatearFecha(a.FechaSinformatear)
                           let Test = ValoresComponente(pstrserviceId, Constants.EXAMEN_COMPLETO_DE_ORINA_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportExamenOrinaCompleto
                           {
                               LogoEmpresa = LogoEmpresa,
                               Paciente = a.Paciente,
                               Empresa = a.Empresa,
                               Puesto = a.Puesto,
                               Fecha = ValorFecha,
                               FirmaMedico = a.FirmaMedico,

                               Color = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_COLOR).v_Value1Name,
                               Aspecto = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_ASPECTO).v_Value1Name,
                               Densidad = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_DENSIDAD).v_Value1,
                               Ph = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_PH).v_Value1,

                               CelulasEpiteliales = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CELULAS_EPITELIALES).v_Value1Name,
                               Leucocitos = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_LEUCOCITOS).v_Value1,
                               Hematies = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_HEMATIES).v_Value1,
                               Germenes = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_GERMENES).v_Value1Name,
                               Cilindros = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CILINDROS).v_Value1Name,
                               Cristales = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CRISTALES).v_Value1,

                               Nitritos = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_NITRITOS).v_Value1Name,
                               Proteinas = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_PROTEINAS).v_Value1Name,
                               Glucosa = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_GLUCOSA).v_Value1Name,
                               Cetonas = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_CETONAS).v_Value1Name,
                               Urobilinogeno = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_UROBILINOGENO).v_Value1Name,
                               Bilirrubinas = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_BILIRRUBINA).v_Value1Name,
                               Sangre = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_SANGRE).v_Value1Name,
                               Hemoglobina = Test.Count == 0 ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_HEMOGLOBINA).v_Value1Name,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportHemogramaCompleto> GetReportHemogramaCompleto(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportHemogramaCompleto
                                 {
                                     FechaSinFormato = A.d_ServiceDate.Value,
                                     Paciente = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = D.v_Name,
                                     Puesto = E.v_CurrentOccupation
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let ValorFecha = FormatearFecha(a.FechaSinFormato)
                           let Test = ValoresComponente(pstrserviceId, Constants.HEMOGRAMA_COMPLETO_ID)
                           let Sangre = ValoresComponente(pstrserviceId, Constants.GRUPO_Y_FACTOR_SANGUINEO_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportHemogramaCompleto
                           {
                               LogoEmpresa = LogoEmpresa,
                               Paciente = a.Paciente,
                               Empresa = a.Empresa,
                               Puesto = a.Puesto,
                               Fecha = ValorFecha,
                               FirmaMedico = a.FirmaMedico,

                               HematocritoReslt = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATOCRITO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATOCRITO).v_Value1,
                               HematocritoRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATOCRITO_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATOCRITO_DESEABLE).v_Value1,

                               HemoglobinaReslt = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA).v_Value1,
                               HemoglobinaRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA_DESEABLE).v_Value1,

                               GlobulosRojosReslt = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATIES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATIES).v_Value1,
                               GlobulosRojosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATIES_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_HEMATIES_DESEABLE).v_Value1,

                               LeucocitosReslt = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_LEUCOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_LEUCOCITOS).v_Value1,
                               LeucocitosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_LEUCOCITOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_LEUCOCITOS_DESEABLE).v_Value1,

                               RecuPlaquetasResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_PLAQUETAS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_PLAQUETAS).v_Value1,
                               RecuPlaquetasRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_PLAQUETAS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_PLAQUETAS_DESEABLE).v_Value1,

                               AbastonadosResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_ABASTONADOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_ABASTONADOS).v_Value1,
                               AbastonadosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_ABASTONADOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_ABASTONADOS_DESEABLE).v_Value1,

                               SegmentadosResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_SEGMENTADOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_SEGMENTADOS).v_Value1,
                               SegmentadoRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_SEGMENTADOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_SEGMENTADOS_DESEABLE).v_Value1,

                               EosinofilosResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_EOSINOFILOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_EOSINOFILOS).v_Value1,
                               EosinofilosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_EOSINOFILOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_EOSINOFILOS_DESEABLE).v_Value1,

                               BasofilosResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_BASOFILOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_BASOFILOS).v_Value1,
                               BasofilosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_BASOFILOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_BASOFILOS_DESEABLE).v_Value1,

                               MonocitosResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_MONOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_MONOCITOS).v_Value1,
                               MonocitosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_MONOCITOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_MONOCITOS_DESEABLE).v_Value1,

                               LinfocitosResult = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_LINFOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_LINFOCITOS).v_Value1,
                               LinfocitosRef = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_LINFOCITOS_DESEABLE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_LINFOCITOS_DESEABLE).v_Value1,

                               Sanguineo = Sangre.Count == 0 || Sangre.Find(p => p.v_ComponentFieldId == Constants.GRUPO_SANGUINEO_ID) == null ? string.Empty : Sangre.Find(p => p.v_ComponentFieldId == Constants.GRUPO_SANGUINEO_ID).v_Value1Name,
                               Factor = Sangre.Count == 0 || Sangre.Find(p => p.v_ComponentFieldId == Constants.FACTOR_SANGUINEO_ID) == null ? string.Empty : Sangre.Find(p => p.v_ComponentFieldId == Constants.FACTOR_SANGUINEO_ID).v_Value1Name,

                               Observaciones = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_CONCLUSIONES_DE_HEMOGRAMA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.HEMOGRAMA_COMPLETO_CONCLUSIONES_DE_HEMOGRAMA).v_Value1,


                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<Sigesoft.Node.WinClient.BE.ReportInformeEcograficoAbdominal> GetReportInformeEcograficoAbdominal(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeEcograficoAbdominal
                                 {
                                     Nombre_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     FechaServicio = A.d_ServiceDate.Value
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.ECOGRAFIA_ABDOMINAL_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportInformeEcograficoAbdominal
                           {
                               Nombre_Trabajador = a.Nombre_Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               FechaServicio = a.FechaServicio,
                               ECOGRAFIA_ABDOMINAL_MOTIVO_EXAMEN = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MOTIVO_EXAMEN) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MOTIVO_EXAMEN).v_Value1,
                               ECOGRAFIA_ABDOMINAL_MORFOLOGIA_MOVILIDAD = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MORFOLOGIA_MOVILIDAD) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MORFOLOGIA_MOVILIDAD).v_Value1,
                               ECOGRAFIA_ABDOMINAL_BORDES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_BORDES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_BORDES).v_Value1,
                               ECOGRAFIA_ABDOMINAL_MORFOLOGIA_MOVILIDAD_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MORFOLOGIA_MOVILIDAD_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MORFOLOGIA_MOVILIDAD_DESCRIPCION).v_Value1,
                               ECOGRAFIA_ABDOMINAL_BORDES_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_BORDES_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_BORDES_DESCRIPCION).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DIMENSIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIMENSIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIMENSIONES).v_Value1,
                               ECOGRAFIA_ABDOMINAL_PAREMQUIMA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREMQUIMA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREMQUIMA).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DIMENSIONES_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIMENSIONES_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIMENSIONES_DESCRIPCION).v_Value1,
                               ECOGRAFIA_ABDOMINAL_ECOGENICIDA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_ECOGENICIDA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_ECOGENICIDA).v_Value1,
                               ECOGRAFIA_ABDOMINAL_IMAGENES_EXPANSIVAS = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_IMAGENES_EXPANSIVAS) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_IMAGENES_EXPANSIVAS).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DILATACION_VIAS_BILIARES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DILATACION_VIAS_BILIARES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DILATACION_VIAS_BILIARES).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DIAMETRO_COLEDOCO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIAMETRO_COLEDOCO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIAMETRO_COLEDOCO).v_Value1,
                               ECOGRAFIA_ABDOMINAL_FORMA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_FORMA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_FORMA).v_Value1,
                               ECOGRAFIA_ABDOMINAL_FORMA_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_FORMA_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_FORMA_DESCRIPCION).v_Value1,
                               ECOGRAFIA_ABDOMINAL_PAREDES1 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREDES1) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREDES1).v_Value1,
                               ECOGRAFIA_ABDOMINAL_PAREDES2 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREDES2) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREDES2).v_Value1,
                               ECOGRAFIA_ABDOMINAL_CONT_ANECOICO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CONT_ANECOICO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CONT_ANECOICO).v_Value1,
                               ECOGRAFIA_ABDOMINAL_BARRO_BILIAR = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_BARRO_BILIAR) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_BARRO_BILIAR).v_Value1,
                               ECOGRAFIA_ABDOMINAL_CALCULOS_INTERIOR = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALCULOS_INTERIOR) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALCULOS_INTERIOR).v_Value1,
                               ECOGRAFIA_ABDOMINAL_CALCULOS_TAMAÑO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALCULOS_TAMAÑO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALCULOS_TAMAÑO).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DIAMETRO_TRANSVERSO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P1) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P1).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DIAMETRO_LOGITUDINAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P2) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P2).v_Value1,

                               ECOGRAFIA_ABDOMINAL_PANCREAS_MORFOLOGIA_MOVILIDAD = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P3) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P3).v_Value1,
                               ECOGRAFIA_ABDOMINAL_PANCREAS_MEDIDAS = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P4) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P4).v_Value1,
                               ECOGRAFIA_ABDOMINAL_PANCREAS_MEDIDAS_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P5) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P5).v_Value1,

                               ECOGRAFIA_ABDOMINAL_CABEZA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P6) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P6).v_Value1,

                               ECOGRAFIA_ABDOMINAL_CUELLO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P7) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P7).v_Value1,

                               ECOGRAFIA_ABDOMINAL_CUERNO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P8) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P8).v_Value1,
                               ECOGRAFIA_ABDOMINAL_COLA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_Obs) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_Obs).v_Value1,


                               //ECOGRAFIA_ABDOMINAL_MEDIDAD_NO_EVALUABLES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MEDIDAD_NO_EVALUABLES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_MEDIDAD_NO_EVALUABLES).v_Value1,
                               ECOGRAFIA_ABDOMINAL_ANORMAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_ANORMAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_ANORMAL).v_Value1,
                               ECOGRAFIA_ABDOMINAL_DIAMETRO_ANTOPOSTERIOR = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIAMETRO_ANTOPOSTERIOR) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_DIAMETRO_ANTOPOSTERIOR).v_Value1,
                               ECOGRAFIA_ABDOMINAL_LONGUITUD = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_LONGUITUD) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_LONGUITUD).v_Value1,
                               ECOGRAFIA_ABDOMINAL_CALIBRES_VASOS = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALIBRES_VASOS) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALIBRES_VASOS).v_Value1,
                               ECOGRAFIA_ABDOMINAL_CALIBRES_VASOS_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALIBRES_VASOS_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CALIBRES_VASOS_DESCRIPCION).v_Value1,

                               ECOGRAFIA_ABDOMINAL_LIQUIDO_LIBRE_ANDOMINAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_LIQUIDO_LIBRE_ANDOMINAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_LIQUIDO_LIBRE_ANDOMINAL).v_Value1,
                               ECOGRAFIA_ABDOMINAL_NINGUNA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_NINGUNA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_NINGUNA).v_Value1,
                               ECOGRAFIA_ABDOMINAL_OBSERVACIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_OBSERVACIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_OBSERVACIONES).v_Value1,
                               ECOGRAFIA_ABDOMINAL_CONCLUSIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CONCLUSIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_CONCLUSIONES).v_Value1,
                               ECOGRAFIA_ABDOMINAL_PAREMQUIMA_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREMQUIMA_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_PAREMQUIMA_DESCRIPCION).v_Value1,
                               ECOGRAFIA_ABDOMINAL_LIQUIDO_LIBRE_ANDOMINAL_DESCRIPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_LIQUIDO_LIBRE_ANDOMINAL_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_ABDOMINAL_LIQUIDO_LIBRE_ANDOMINAL_DESCRIPCION).v_Value1

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportInformeEcograficoProstata> GetReportInformeEcograficoProstata(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeEcograficoProstata
                                 {
                                     Nombre_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate.Value,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.INFORME_ECOGRAFICO_PROSTATA_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportInformeEcograficoProstata
                           {
                               Nombre_Trabajador = a.Nombre_Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),

                               INFORME_ECOGRAFICO_PROSTATA_MOTIVO_EXAMEN = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_MOTIVO_EXAMEN) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_MOTIVO_EXAMEN).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_RECEPCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_RECEPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_RECEPCION).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_PAREDES1 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_PAREDES1) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_PAREDES1).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_PAREDES2 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_PAREDES2) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_PAREDES2).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_CONT_ANECOICO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_CONT_ANECOICO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_CONT_ANECOICO).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_IAMGENES_EXPANSIVAS = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_IAMGENES_EXPANSIVAS) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_IAMGENES_EXPANSIVAS).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_CALCULOS_INTERIOR = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_CALCULOS_INTERIOR) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_CALCULOS_INTERIOR).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_TAMAÑO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_TAMAÑO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_TAMAÑO).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_VOL_PREMICCIONAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOL_PREMICCIONAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOL_PREMICCIONAL).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_VOL_POSMICCIONAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOL_POSMICCIONAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOL_POSMICCIONAL).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_RETECION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_RETECION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_RETECION).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_BORDES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_BORDES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_BORDES).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_DIAMETRO_TRANSVERSO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_DIAMETRO_TRANSVERSO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_DIAMETRO_TRANSVERSO).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_ANTERO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_ANTERO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_ANTERO).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_DIAMETRO_LONGITUDINAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_DIAMETRO_LONGITUDINAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_DIAMETRO_LONGITUDINAL).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_VOLUMEN = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOLUMEN) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOLUMEN).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_VOLUMEN_VN = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOLUMEN_VN) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_VOLUMEN_VN).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_ECOESTRUCTURA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_ECOESTRUCTURA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_ECOESTRUCTURA).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_DESCRIPCION_OTROS = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_DESCRIPCION_OTROS) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_DESCRIPCION_OTROS).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_NINGUNA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_NINGUNA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_NINGUNA).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_OBSERVACIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_OBSERVACIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_OBSERVACIONES).v_Value1,
                               INFORME_ECOGRAFICO_PROSTATA_CONCLUSIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_CONCLUSIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.INFORME_ECOGRAFICO_PROSTATA_CONCLUSIONES).v_Value1,

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportInformeEcograficoRenal> GetReportInformeEcograficoRenal(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeEcograficoRenal
                                 {
                                     Nombre_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     FechaServicio = A.d_ServiceDate.Value
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.ECOGRAFIA_ABDOMINAL_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportInformeEcograficoRenal
                           {
                               Nombre_Trabajador = a.Nombre_Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               FechaServicio = a.FechaServicio,
                               ECOGRAFIA_RENAL_MOTIVO_EXAMEN = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MOTIVO_EXAMEN) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MOTIVO_EXAMEN).v_Value1,
                               ECOGRAFIA_RENAL_MORFOLOGIA_MOVILIDAD_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MORFOLOGIA_MOVILIDAD_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MORFOLOGIA_MOVILIDAD_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_DESCRIPCION_ANORMAL_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_ANORMAL_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_ANORMAL_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_ECOGENICIDAD_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_ECOGENICIDAD_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_ECOGENICIDAD_RINION_DERECHO).v_Value1,


                               ECOGRAFIA_RENAL_LONGITUD_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_LONGITUD_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_LONGITUD_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_PARENQUIMA_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_PARENQUIMA_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_PARENQUIMA_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_IMG_EXP_SOLIDAS_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_IMG_EXP_SOLIDAS_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_IMG_EXP_SOLIDAS_RINION_DERECHO).v_Value1,


                               ECOGRAFIA_RENAL_QUISTICAS_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_QUISTICAS_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_QUISTICAS_RINION_DERECHO).v_Value1,


                               ECOGRAFIA_RENAL_HIDRONEFROSIS_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_RINION_DERECHO).v_Value1,


                               ECOGRAFIA_RENAL_MICROLITIAS_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MICROLITIAS_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MICROLITIAS_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_HIDRONEFROSIS_MEDIDAD_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_MEDIDAD_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_MEDIDAD_RINION_DERECHO).v_Value1,




                               ECOGRAFIA_RENAL_CALCULOS_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_CALCULOS_MEDIDA_RINION_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_MEDIDA_RINION_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_MEDIDA_RINION_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_MORFOLOGIA_MOVILIDAD_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MORFOLOGIA_MOVILIDAD_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MORFOLOGIA_MOVILIDAD_RINION_IZQUIERDO).v_Value1,

                               ECOGRAFIA_RENAL_DESCRIPCION_ANORMAL_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_ANORMAL_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_ANORMAL_RINION_IZQUIERDO).v_Value1,



                               ECOGRAFIA_RENAL_ECOGENICIDAD_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_ECOGENICIDAD_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_ECOGENICIDAD_RINION_IZQUIERDO).v_Value1,


                               ECOGRAFIA_RENAL_LONGITUD_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_LONGITUD_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_LONGITUD_RINION_IZQUIERDO).v_Value1,


                               ECOGRAFIA_RENAL_PARENQUIMA_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_PARENQUIMA_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_PARENQUIMA_RINION_IZQUIERDO).v_Value1,
                               ECOGRAFIA_RENAL_IMG_EXP_SOLIDAS_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_IMG_EXP_SOLIDAS_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_IMG_EXP_SOLIDAS_RINION_IZQUIERDO).v_Value1,
                               ECOGRAFIA_RENAL_QUISTICAS_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_QUISTICAS_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_QUISTICAS_RINION_IZQUIERDO).v_Value1,
                               ECOGRAFIA_RENAL_MICROLITIAS_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MICROLITIAS_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_MICROLITIAS_RINION_IZQUIERDO).v_Value1,
                               ECOGRAFIA_RENAL_CALCULOS_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_RINION_IZQUIERDO).v_Value1,

                               ECOGRAFIA_RENAL_HIDRONEFROSIS_MEDIDAD_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_MEDIDAD_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_MEDIDAD_RINION_IZQUIERDO).v_Value1,
                               ECOGRAFIA_RENAL_CALCULOS_MEDIDA_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_MEDIDA_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_MEDIDA_RINION_IZQUIERDO).v_Value1,
                               ECOGRAFIA_RENAL_DESCRIPCION_OTROS_RIÑON_DERECHO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_OTROS_RIÑON_DERECHO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_OTROS_RIÑON_DERECHO).v_Value1,

                               ECOGRAFIA_RENAL_DESCRIPCION_OTROS_RIÑON_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_OTROS_RIÑON_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_OTROS_RIÑON_IZQUIERDO).v_Value1,

                               ECOGRAFIA_RENAL_REPLICACION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_REPLICACION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_REPLICACION).v_Value1,

                               ECOGRAFIA_RENAL_PAREDES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_PAREDES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_PAREDES).v_Value1,
                               ECOGRAFIA_RENAL_CONT_ANECOICO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CONT_ANECOICO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CONT_ANECOICO).v_Value1,


                               ECOGRAFIA_RENAL_IMG_EXPANSIVAS = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_IMG_EXPANSIVAS) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_IMG_EXPANSIVAS).v_Value1,
                               ECOGRAFIA_RENAL_CALCULOS_INTERIOR = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_INTERIOR) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_INTERIOR).v_Value1,



                               ECOGRAFIA_RENAL_CALCULOS_INTERIOR_MEDIDA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_INTERIOR_MEDIDA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CALCULOS_INTERIOR_MEDIDA).v_Value1,
                               ECOGRAFIA_RENAL_VOL_PREMICCIONAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_VOL_PREMICCIONAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_VOL_PREMICCIONAL).v_Value1,
                               ECOGRAFIA_RENAL_VOL_POSMICCIONAL = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_VOL_POSMICCIONAL) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_VOL_POSMICCIONAL).v_Value1,

                               ECOGRAFIA_RENAL_RETENCION = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_RETENCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_RETENCION).v_Value1,

                               ECOGRAFIA_RENAL_DESCRIPCION_VEGIGA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_VEGIGA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_DESCRIPCION_VEGIGA).v_Value1,
                               ECOGRAFIA_RENAL_NIGUNA = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_NIGUNA) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_NIGUNA).v_Value1,
                               ECOGRAFIA_RENAL_OBSERVACIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_OBSERVACIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_OBSERVACIONES).v_Value1,
                               ECOGRAFIA_RENAL_CONCLUSIONES = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CONCLUSIONES) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_CONCLUSIONES).v_Value1,
                               ECOGRAFIA_RENAL_HIDRONEFROSIS_RINION_IZQUIERDO = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_RINION_IZQUIERDO) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.ECOGRAFIA_RENAL_HIDRONEFROSIS_RINION_IZQUIERDO).v_Value1

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportParasitologicoSimple> GetReportParasitologicoSimple(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportParasitologicoSimple
                                 {
                                     FechaSinFormato = A.d_ServiceDate.Value,
                                     Paciente = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = D.v_Name,
                                     Puesto = E.v_CurrentOccupation
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let ValorFecha = FormatearFecha(a.FechaSinFormato)
                           let Test = ValoresComponente(pstrserviceId, Constants.PARASITOLOGICO_SIMPLE_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportParasitologicoSimple
                           {
                               LogoEmpresa = LogoEmpresa,
                               Paciente = a.Paciente,
                               Empresa = a.Empresa,
                               Puesto = a.Puesto,
                               Fecha = ValorFecha,
                               FirmaMedico = a.FirmaMedico,

                               Color = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_COLOR) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_COLOR).v_Value1,
                               Consistencia = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_CONSISTENCIA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_CONSISTENCIA).v_Value1Name,

                               RestosAlimenticios = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESTOS_ALIMENTICIOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESTOS_ALIMENTICIOS).v_Value1Name,
                               Sangre = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_SANGRE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_SANGRE).v_Value1Name,
                               Moco = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_MOCO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_MOCO).v_Value1Name,
                               Quistes = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_QUISTES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_QUISTES).v_Value1Name,
                               Huevos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_HUEVOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_HUEVOS).v_Value1Name,
                               Trofozoitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_TROFOZOITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_TROFOZOITOS).v_Value1Name,
                               Hematies = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_HEMATIES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_HEMATIES).v_Value1Name,
                               Leucocitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_LEUCOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_LEUCOCITOS).v_Value1Name,
                               Resultados = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESULTADOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESULTADOS).v_Value1,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportTestSintomaticoResp> GetReportTestSintomaticoResp(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var groupUbigeo = 113;
                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId



                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()


                                 // Ubigeo de la persona *******************************************************
                                 join dep in dbContext.datahierarchy on new { a = B.i_DepartmentId.Value, b = groupUbigeo }
                                                      equals new { a = dep.i_ItemId, b = dep.i_GroupId } into dep_join
                                 from dep in dep_join.DefaultIfEmpty()

                                 join prov in dbContext.datahierarchy on new { a = B.i_ProvinceId.Value, b = groupUbigeo }
                                                       equals new { a = prov.i_ItemId, b = prov.i_GroupId } into prov_join
                                 from prov in prov_join.DefaultIfEmpty()

                                 join distri in dbContext.datahierarchy on new { a = B.i_DistrictId.Value, b = groupUbigeo }
                                                       equals new { a = distri.i_ItemId, b = distri.i_GroupId } into distri_join
                                 from distri in distri_join.DefaultIfEmpty()
                                 //*********************************************************************************************

                                 let varDpto = dep.v_Value1 == null ? "" : dep.v_Value1
                                 let varProv = prov.v_Value1 == null ? "" : prov.v_Value1
                                 let varDistri = distri.v_Value1 == null ? "" : distri.v_Value1

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportTestSintomaticoResp
                                 {
                                     Nombres = B.v_FirstName,
                                     Apellidos = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DNI = B.v_DocNumber,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = B.i_SexTypeId,
                                     Direccion = B.v_AdressLocation,
                                     Huella = B.b_FingerPrintImage,
                                     Firma = B.b_RubricImage,
                                     ApeMedico = H.v_FirstLastName + " " + H.v_SecondLastName,
                                     NomMedico = H.v_FirstName,
                                     DireMedico = H.v_AdressLocation,
                                     CMP = pme.v_ProfessionalCode,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     //FirmaMedico = pme.b_SignatureImage,
                                     LugarProcedencia = varDistri + "-" + varProv + "-" + varDpto, // Santa Anita - Lima - Lima

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Test = ValoresComponente(pstrserviceId, Constants.TEST_SINTOMATICO_RESP_ID)
                           let rx = ValoresComponente(pstrserviceId, Constants.RX_TORAX_ID)
                           let Lab = ValoresComponente(pstrserviceId, Constants.BK_DIRECTO_ID)
                           let FirmaMedicoMedicina = ObtenerFirmaMedicoExamen(pstrserviceId, Constants.EXAMEN_FISICO_ID, Constants.EXAMEN_FISICO_7C_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportTestSintomaticoResp
                           {
                               Nombres = a.Nombres,
                               Apellidos = a.Apellidos,
                               DNI = a.DNI,
                               FechaNacimiento = a.FechaNacimiento,
                               Genero = a.Genero,
                               Direccion = a.Direccion,
                               Huella = a.Huella,
                               Firma = a.Firma,
                               ApeMedico = a.ApeMedico,
                               NomMedico = a.NomMedico,
                               DireMedico = a.DireMedico,
                               CMP = a.CMP,
                               FechaServicio = a.FechaServicio,
                               FirmaMedico = FirmaMedicoMedicina,
                               Edad = GetAge(a.FechaNacimiento),
                               LugarProcedencia = a.LugarProcedencia,
                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               TEST_SINTOMATICO_P1 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P1) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P1).v_Value1,
                               TEST_SINTOMATICO_P2 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P2) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P2).v_Value1,
                               TEST_SINTOMATICO_P3 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P3) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P3).v_Value1,
                               TEST_SINTOMATICO_P4 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P4) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P4).v_Value1,
                               TEST_SINTOMATICO_P5 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P5) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P5).v_Value1,
                               TEST_SINTOMATICO_P6 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P6) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P6).v_Value1,
                               TEST_SINTOMATICO_P7 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P7) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P7).v_Value1,
                               TEST_SINTOMATICO_P8 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P8) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_P8).v_Value1,
                               TEST_SINTOMATICO_Obs = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_Obs) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_Obs).v_Value1,

                               TEST_SINTOMATICO_EL_LA = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_EL_LA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_EL_LA).v_Value1,
                               TEST_SINTOMATICO_REQ_EST_AMP = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_REQ_EST_AMP) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_REQ_EST_AMP).v_Value1,
                               TEST_SINTOMATICO_BK_ESPUTO = Lab.Count == 0 || Lab.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_BK_ESPUTO) == null ? string.Empty : Lab.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_BK_ESPUTO).v_Value1,
                               TEST_SINTOMATICO_BK_ESPUTO_2 = Lab.Count == 0 || Lab.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_BK_ESPUTO_2) == null ? string.Empty : Lab.Find(p => p.v_ComponentFieldId == Constants.TEST_SINTOMATICO_BK_ESPUTO_2).v_Value1,

                               TEST_SINTOMATICO_RX = rx.Count == 0 || rx.Find(p => p.v_ComponentFieldId == Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID) == null ? string.Empty : rx.Find(p => p.v_ComponentFieldId == Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID).v_Value1,
                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private byte[] ObtenerFirmaMedicoExamen(string pstrServiceId, string p1, string p2)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var objEntity = (from E in dbContext.servicecomponent

                             join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                             from me in me_join.DefaultIfEmpty()

                             join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                             from pme in pme_join.DefaultIfEmpty()


                             where E.v_ServiceId == pstrServiceId &&
                             (E.v_ComponentId == p1 || E.v_ComponentId == p2)
                             select new
                             {

                                 FirmaMedicoMedicina = pme.b_SignatureImage
                             }).FirstOrDefault();

            return objEntity.FirmaMedicoMedicina;
        }

        public List<Sigesoft.Node.WinClient.BE.ReportParasitologicoSeriado> GetReportParasitologicoSeriado(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportParasitologicoSeriado
                                 {
                                     FechaSinFormato = A.d_ServiceDate.Value,
                                     Paciente = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = D.v_Name,
                                     Puesto = E.v_CurrentOccupation
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let LogoEmpresa = GetLogoMedicalCenter()
                           let ValorFecha = FormatearFecha(a.FechaSinFormato)
                           let Test = ValoresComponente(pstrserviceId, Constants.PARASITOLOGICO_SERIADO_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportParasitologicoSeriado
                           {
                               LogoEmpresa = LogoEmpresa,
                               Paciente = a.Paciente,
                               Empresa = a.Empresa,
                               Puesto = a.Puesto,
                               Fecha = ValorFecha,
                               FirmaMedico = a.FirmaMedico,

                               Muestra1Color = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_COLOR) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_COLOR).v_Value1,
                               Muestra1Consistencia = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_CONSISTENCIA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_CONSISTENCIA).v_Value1Name,
                               Muestra1RestosAlim = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_RESTOS_ALIMENTICIOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_RESTOS_ALIMENTICIOS).v_Value1Name,
                               Muestra1Sangre = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_SANGRE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_SANGRE).v_Value1Name,
                               Muestra1Moco = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_MOCO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_MOCO).v_Value1Name,
                               Muestra1Quistes = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_QUISTES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_QUISTES).v_Value1Name,
                               Muestra1Huevos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_HUEVOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_HUEVOS).v_Value1Name,
                               Muestra1Trofozoitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_TROFOZOITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_TROFOZOITOS).v_Value1Name,
                               Muestra1Hematies = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_HEMATIES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_HEMATIES).v_Value1Name,
                               Muestra1Luecocitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_LEUCOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_LEUCOCITOS).v_Value1Name,

                               Muestra2Color = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_COLOR) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_COLOR).v_Value1,
                               Muestra2Consistencia = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_CONSISTENCIA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_CONSISTENCIA).v_Value1Name,
                               Muestra2RestosAlim = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_RESTOS_ALIMENTICIOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_RESTOS_ALIMENTICIOS).v_Value1Name,
                               Muestra2Sangre = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_SANGRE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_SANGRE).v_Value1Name,
                               Muestra2Moco = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_MOCO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_MOCO).v_Value1Name,
                               Muestra2Quistes = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_QUISTES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_QUISTES).v_Value1Name,
                               Muestra2Huevos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_HUEVOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_HUEVOS).v_Value1Name,
                               Muestra2Trofozoitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_TROFOZOITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_TROFOZOITOS).v_Value1Name,
                               Muestra2Hematies = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_HEMATIES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_HEMATIES).v_Value1Name,
                               Muestra2Leucocitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_LEUCOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_LEUCOCITOS).v_Value1Name,

                               Muestra3Color = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_COLOR) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_COLOR).v_Value1,
                               Muestra3Consistencia = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_CONSISTENCIA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_CONSISTENCIA).v_Value1Name,
                               Muestra3RestosAlim = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_RESTOS_ALIMENTICIOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_RESTOS_ALIMENTICIOS).v_Value1Name,
                               Muestra3Sangre = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_SANGRE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_SANGRE).v_Value1Name,
                               Muestra3Moco = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_MOCO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_MOCO).v_Value1Name,
                               Quistes = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_QUISTES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_QUISTES).v_Value1Name,
                               Muestra3Huevos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_HUEVOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_HUEVOS).v_Value1Name,
                               Muestra3Trofozoitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_TROFOZOITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_TROFOZOITOS).v_Value1Name,
                               Muestra3Hematies = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_HEMATIES) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_HEMATIES).v_Value1Name,
                               Muestra3Leucocitos = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_LEUCOCITOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_LEUCOCITOS).v_Value1Name,

                               Resultado = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_RESULTADOS) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_RESULTADOS).v_Value1,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.TestSomnolencia> GetReportTestSomnolencia(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                    equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 from D in D_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.TestSomnolencia
                                 {
                                     Nombre = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     FechaNacimiento = B.d_Birthdate,
                                     DNI = B.v_DocNumber,
                                     Empresa = D.v_Name,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaProfesional = pme.b_SignatureImage
                                 });

                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           let Test = ValoresComponente(pstrserviceId, Constants.TEST_SOMNOLENCIA_ID)

                           select new Sigesoft.Node.WinClient.BE.TestSomnolencia
                           {
                               Nombre = a.Nombre,
                               FechaServicio = a.FechaServicio,
                               DNI = a.DNI,
                               Empresa = a.Empresa,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               TEST_SOMNOLENCIA_SENTADO_LEYENDO = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_SENTADO_LEYENDO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_SENTADO_LEYENDO).v_Value1,
                               TEST_SOMNOLENCIA_MIRANDO_TV = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_MIRANDO_TV) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_MIRANDO_TV).v_Value1,
                               TEST_SOMNOLENCIA_SENTADO_QUIETO = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_SENTADO_QUIETO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_SENTADO_QUIETO).v_Value1,
                               TEST_SOMNOLENCIA_VIAJANDO = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_VIAJANDO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_VIAJANDO).v_Value1,
                               TEST_SOMNOLENCIA_CONVERSANDO = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_CONVERSANDO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_CONVERSANDO).v_Value1,
                               TEST_SOMNOLENCIA_LUEGO_COMIDA = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_LUEGO_COMIDA) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_LUEGO_COMIDA).v_Value1,
                               TEST_SOMNOLENCIA_CONDUCIENDO = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_CONDUCIENDO) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_CONDUCIENDO).v_Value1,
                               TEST_SOMNOLENCIA_DESCANSAR = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_DESCANSAR) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_DESCANSAR).v_Value1,
                               TEST_SOMNOLENCIA_PUNTAJE = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_PUNTAJE) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_PUNTAJE).v_Value1,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaProfesional = a.FirmaProfesional,
                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               Dxs = GetDiagnosticByServiceIdAndComponentAndField(pstrserviceId, pstrComponentId, Constants.TEST_SOMNOLENCIA_PUNTAJE) == null ? "Sin Somnolencia Diurna" : GetDiagnosticByServiceIdAndComponentAndField(pstrserviceId, pstrComponentId, Constants.TEST_SOMNOLENCIA_PUNTAJE),
                               Conclusiones = GetRecommendationByServiceIdAndComponentAndField(pstrserviceId, pstrComponentId, Constants.TEST_SOMNOLENCIA_PUNTAJE) == null ? "------" : GetRecommendationByServiceIdAndComponentAndField(pstrserviceId, pstrComponentId, Constants.TEST_SOMNOLENCIA_PUNTAJE)
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetDiagnosticByServiceIdAndComponentAndField(string pstrServiceId, string pstrComponent, string pstrFieldId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.diagnosticrepository
                         join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                         from ddd in ddd_join.DefaultIfEmpty()

                         join eee in dbContext.recommendation on new { a = pstrServiceId, b = pstrComponent }
                                                                    equals new { a = eee.v_ServiceId, b = eee.v_ComponentId } into eee_join
                         from eee in eee_join.DefaultIfEmpty()

                         join fff in dbContext.masterrecommendationrestricction on eee.v_MasterRecommendationId
                                                                equals fff.v_MasterRecommendationRestricctionId into fff_join
                         from fff in fff_join.DefaultIfEmpty()

                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0 && ccc.v_ComponentFieldId == pstrFieldId
                         select new
                         {
                             v_DiseasesName = ddd.v_Name
                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_DiseasesName));
        }

        private string GetRecommendationByServiceIdAndComponentAndField(string pstrServiceId, string pstrComponent, string pstrFieldId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.recommendation
                         join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId  // Diagnosticos      
                         join eee in dbContext.diagnosticrepository on ccc.v_DiagnosticRepositoryId equals eee.v_DiagnosticRepositoryId
                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0 && eee.v_ComponentFieldId == Constants.TEST_SOMNOLENCIA_PUNTAJE
                         select new
                         {
                             v_Recommendation = ddd.v_Name

                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_Recommendation));
        }

        public List<Sigesoft.Node.WinClient.BE.ReportTestChoferes> GetReportTestSueño(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportTestChoferes
                                 {
                                     Nombre = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     FechaNacimiento = B.d_Birthdate,
                                     DNI = B.v_DocNumber,
                                     Empresa = D.v_Name
                                 });

                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           let Test = ValoresComponente(pstrserviceId, Constants.TEST_CHOFERES_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportTestChoferes
                           {
                               Nombre = a.Nombre,
                               FechaServicio = a.FechaServicio,
                               DNI = a.DNI,
                               Empresa = a.Empresa,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               P_1 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_1_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_1_ID).v_Value1,
                               P_2 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_2_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_2_ID).v_Value1,
                               P_3 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_3_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_3_ID).v_Value1,
                               P_4 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_4_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_4_ID).v_Value1,
                               P_5 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_5_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_5_ID).v_Value1,
                               P_6 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_6_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_6_ID).v_Value1,
                               P_7 = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_7_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_7_ID).v_Value1,
                               Puntaje = Test.Count == 0 || Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_PUNTAJE_ID) == null ? string.Empty : Test.Find(p => p.v_ComponentFieldId == Constants.TEST_CHOFERES_PUNTAJE_ID).v_Value1,
                               Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


   }
}
