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
    public class PacientBL
    {
        #region Person

        public byte[] getPhoto(string pstrPersonId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.person
                                 where a.v_PersonId == pstrPersonId
                                 select new
                                    {
                                        Foto = a.b_PersonImage
                                    }
                                 ).FirstOrDefault();

                return objEntity.Foto;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public personDto GetPerson(ref OperationResult pobjOperationResult, string pstrPersonId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                personDto objDtoEntity = null;

                var objEntity = (from a in dbContext.person
                                 where a.v_PersonId == pstrPersonId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = personAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public string AddPerson(ref OperationResult pobjOperationResult, personDto pobjPerson, professionalDto pobjProfessional, systemuserDto pobjSystemUser, List<string> ClientSession)
        {
            //mon.IsActive = true;
            int SecuentialId = -1;
            string newId = string.Empty;
            person objEntity1 = null;

            try
            {
                #region Validations
                // Validar el DNI de la persona
                if (pobjPerson != null)
                {
                    OperationResult objOperationResult6 = new OperationResult();
                    string strfilterExpression1 = string.Format("v_DocNumber==\"{0}\"&&i_Isdeleted==0", pobjPerson.v_DocNumber);
                    var _recordCount1 = GetPersonCount(ref objOperationResult6, strfilterExpression1);

                    if (_recordCount1 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El número de documento  <font color='red'>" + pobjPerson.v_DocNumber + "</font> ya se encuentra registrado.<br> Por favor ingrese otro número de documento.";
                        return "-1";
                    }
                }

                // Validar existencia de UserName en la BD
                if (pobjSystemUser != null)
                {
                    OperationResult objOperationResult7 = new OperationResult();
                    string strfilterExpression2 = string.Format("v_UserName==\"{0}\"&&i_Isdeleted==0", pobjSystemUser.v_UserName);
                    var _recordCount2 = new SecurityBL().GetSystemUserCount(ref objOperationResult7, strfilterExpression2);

                    if (_recordCount2 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El nombre de usuario  <font color='red'>" + pobjSystemUser.v_UserName + "</font> ya se encuentra registrado.<br> Por favor ingrese otro nombre de usuario.";
                        return "-1";
                    }
                }
                #endregion

                // Grabar Persona
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                objEntity1 = personAssembler.ToEntity(pobjPerson);

                objEntity1.d_InsertDate = DateTime.Now;
                objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity1.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                SecuentialId = Utils.GetNextSecuentialId(Int32.Parse(ClientSession[0]), 8);
                newId = Common.Utils.GetNewId(int.Parse(ClientSession[0]), SecuentialId, "PP");
                objEntity1.v_PersonId = newId;

                dbContext.AddToperson(objEntity1);
                dbContext.SaveChanges();

                // Grabar Profesional
                if (pobjProfessional != null)
                {
                    OperationResult objOperationResult2 = new OperationResult();
                    pobjProfessional.v_PersonId = objEntity1.v_PersonId;
                    AddProfessional(ref objOperationResult2, pobjProfessional, ClientSession);
                }

                // Grabar Usuario
                if (pobjSystemUser != null)
                {
                    OperationResult objOperationResult3 = new OperationResult();
                    pobjSystemUser.v_PersonId = objEntity1.v_PersonId;
                    new SecurityBL().AddSystemUSer(ref objOperationResult3, pobjSystemUser, ClientSession);
                }

                ////Seteamos si el registro es agregado en el DataCenter o en un nodo
                //if (ClientSession[0] == "1")
                //{
                //    objEntity1.i_IsInMaster = 1;
                //}
                //else
                //{
                //    objEntity1.i_IsInMaster = 0;
                //}

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PERSONA", "v_PersonId=" + objEntity1.v_PersonId, Success.Ok, null);
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PERSONA", "v_PersonId=" + objEntity1.v_PersonId, Success.Failed, ex.Message);
            }

            return newId;
        }

        public void UpdatePerson(ref OperationResult pobjOperationResult, bool pbIsChangeDocNumber, personDto pobjPerson, professionalDto pobjProfessional, bool pbIsChangeUserName, systemuserDto pobjSystemUser, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                #region Validate SystemUSer
                // Validar existencia de UserName en la BD
                if (pobjSystemUser != null && pbIsChangeUserName == true)
                {
                    OperationResult objOperationResult7 = new OperationResult();
                    string strfilterExpression2 = string.Format("v_UserName==\"{0}\"&&i_Isdeleted==0", pobjSystemUser.v_UserName);
                    var _recordCount2 = new SecurityBL().GetSystemUserCount(ref objOperationResult7, strfilterExpression2);

                    if (_recordCount2 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El nombre de usuario  <font color='red'>" + pobjSystemUser.v_UserName + "</font> ya se encuentra registrado.<br> Por favor ingrese otro nombre de usuario.";
                        return;
                    }
                }

                #endregion

                #region Validate Document Number

                // Validar el DNI de la persona
                if (pobjPerson != null && pbIsChangeDocNumber == true)
                {
                    OperationResult objOperationResult6 = new OperationResult();
                    string strfilterExpression1 = string.Format("v_DocNumber==\"{0}\"&&i_Isdeleted==0", pobjPerson.v_DocNumber);
                    var _recordCount1 = GetPersonCount(ref objOperationResult6, strfilterExpression1);

                    if (_recordCount1 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El número de documento  <font color='red'>" + pobjPerson.v_DocNumber + "</font> ya se encuentra registrado.<br> Por favor ingrese otro número de documento.";
                        return;
                    }
                }

                #endregion

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                // Actualiza Persona
                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.person
                                       where a.v_PersonId == pobjPerson.v_PersonId
                                       select a).FirstOrDefault();

                pobjPerson.d_UpdateDate = DateTime.Now;
                pobjPerson.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                person objEntity = personAssembler.ToEntity(pobjPerson);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.person.ApplyCurrentValues(objEntity);

                // Guardar los cambios
                dbContext.SaveChanges();

                // Actualiza Profesional
                if (pobjProfessional != null)
                {
                    OperationResult objOperationResult2 = new OperationResult();
                    UpdateProfessional(ref objOperationResult2, pobjProfessional, ClientSession);
                }

                // Actualiza Usuario
                if (pobjSystemUser != null)
                {
                    OperationResult objOperationResult3 = new OperationResult();
                    new SecurityBL().UpdateSystemUSer(ref objOperationResult3, pobjSystemUser, ClientSession);
                }

                //if (ClientSession[0] == "1")
                //{
                //    objEntitySource.i_IsInMaster = 1;
                //}
                //else
                //{
                //    objEntitySource.i_IsInMaster = 0;
                //}

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PERSONA", "v_PersonId=" + pobjPerson.v_PersonId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PERSONA", "v_PersonId=" + pobjPerson.v_PersonId, Success.Failed, ex.Message);
                return;
            }
        }

        public void DeletePerson(ref OperationResult pobjOperationResult, string pstrPersonId, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.person
                                       where a.v_PersonId == pstrPersonId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.i_IsDeleted = 1;

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "PERSONA", "v_PersonId=" + objEntitySource.v_PersonId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "PERSONA", "", Success.Failed, null);
                return;
            }
        }

        public int GetPersonCount(ref OperationResult pobjOperationResult, string filterExpression)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from a in dbContext.person select a;

                if (!string.IsNullOrEmpty(filterExpression))
                    query = query.Where(filterExpression);

                int intResult = query.Count();

                pobjOperationResult.Success = 1;
                return intResult;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return 0;
            }
        }

        //public void AddPersonOrganization(ref OperationResult pobjOperationResult, int PersonId, int OrganizationId, List<string> ClientSession)
        //{
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        personorganization objEntity = new personorganization();

        //        objEntity.i_PersonId = PersonId;
        //        objEntity.i_OrganizationId = OrganizationId;
        //        objEntity.d_InsertDate = DateTime.Now;
        //        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);

        //        dbContext.AddTopersonorganization(objEntity);
        //        dbContext.SaveChanges();

        //        pobjOperationResult.Success = 1;
        //        // Llenar entidad Log
        //        //new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.CREACION, "ORGANIZACIÓN", "i_OrganizationId=" + objEntity.i_OrganizationId.ToString(), Constants.Success.Ok, null);
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        // Llenar entidad Log
        //        //new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.CREACION, "PERSONA", "i_PersonId=" + objEntity1.i_PersonId, Constants.Success.Failed, ex.Message);
        //    }
        //}

        #endregion

        #region Professional

        public professionalDto GetProfessional(ref OperationResult pobjOperationResult, string pstrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                professionalDto objDtoEntity = null;

                var objEntity = (from a in dbContext.professional
                                 where a.v_PersonId == pstrPersonId
                                 select a).FirstOrDefault();

                //if (objEntity != null)
                    objDtoEntity = professionalAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public void AddProfessional(ref OperationResult pobjOperationResult, professionalDto pobjDtoEntity, List<string> ClientSession)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                professional objEntity = professionalAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;

                dbContext.AddToprofessional(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PROFESIONAL", "i_ProfessionId=" + objEntity.i_ProfessionId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PROFESIONAL", "i_ProfessionId=" + pobjDtoEntity.i_ProfessionId, Success.Failed, ex.Message);
                return;
            }
        }

        public void UpdateProfessional(ref OperationResult pobjOperationResult, professionalDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                OperationResult objOperationResult1 = new OperationResult();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.professional
                                       where a.v_PersonId == pobjDtoEntity.v_PersonId
                                       select a).FirstOrDefault();

                if (objEntitySource == null)
                {
                    // Grabar
                    AddProfessional(ref objOperationResult1, pobjDtoEntity, ClientSession);
                }
                else
                {
                    // Crear la entidad con los datos actualizados

                    pobjDtoEntity.d_UpdateDate = DateTime.Now;
                    pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                    professional objProfessionalTyped = professionalAssembler.ToEntity(pobjDtoEntity);

                    // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                    dbContext.professional.ApplyCurrentValues(objProfessionalTyped);

                    // Guardar los cambios
                    dbContext.SaveChanges();
                }              

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PROFESIONAL", "i_ProfessionId=" + pobjDtoEntity.i_ProfessionId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PROFESIONAL", "i_ProfessionId=" + pobjDtoEntity.i_ProfessionId, Success.Failed, ex.Message);
                return;
            }
        }

        #endregion

        #region UserExternal

        public Sigesoft.Node.WinClient.BE.PacientList GetPacientReportEPS(string serviceId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                //PacientList objDtoEntity = null;

                var objEntity = (from s in dbContext.service
                                 join pr in dbContext.protocol on s.v_ProtocolId equals pr.v_ProtocolId
                                 join pe in dbContext.person on s.v_PersonId equals pe.v_PersonId

                                 join C in dbContext.systemparameter on new { a = pe.i_TypeOfInsuranceId.Value, b = 188 }  // Tipo de seguro
                                                              equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join d in dbContext.systemparameter on new { a = pe.i_Relationship.Value, b = 207 }  // Parentesco
                                                              equals new { a = d.i_ParameterId, b = d.i_GroupId } into d_join
                                 from d in d_join.DefaultIfEmpty()

                                 // Grupo sanguineo ****************************************************
                                 join gs in dbContext.systemparameter on new { a = pe.i_BloodGroupId.Value, b = 154 }  // AB
                                                             equals new { a = gs.i_ParameterId, b = gs.i_GroupId } into gs_join
                                 from gs in gs_join.DefaultIfEmpty()

                                 // Factor sanguineo ****************************************************
                                 join fs in dbContext.systemparameter on new { a = pe.i_BloodFactorId.Value, b = 155 }  // NEGATIVO
                                                           equals new { a = fs.i_ParameterId, b = fs.i_GroupId } into fs_join
                                 from fs in fs_join.DefaultIfEmpty()

                                 // Empresa / Sede Trabajo  ********************************************************
                                 join ow in dbContext.organization on new { a = pr.v_WorkingOrganizationId }
                                         equals new { a = ow.v_OrganizationId } into ow_join
                                 from ow in ow_join.DefaultIfEmpty()

                                 join lw in dbContext.location on new { a = pr.v_WorkingOrganizationId, b = pr.v_WorkingLocationId }
                                      equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                                 from lw in lw_join.DefaultIfEmpty()

                                 //************************************************************************************

                                 where s.v_ServiceId == serviceId
                                 select new Sigesoft.Node.WinClient.BE.PacientList
                                 {
                                     v_PersonId = pe.v_PersonId,
                                     v_FirstName = pe.v_FirstName,
                                     v_FirstLastName = pe.v_FirstLastName,
                                     v_SecondLastName = pe.v_SecondLastName,
                                     b_Photo = pe.b_PersonImage,
                                     v_TypeOfInsuranceName = C.v_Value1,
                                     v_FullWorkingOrganizationName = ow.v_Name + " / " + lw.v_Name,
                                     v_RelationshipName = d.v_Value1,
                                     v_OwnerName = "",
                                     d_ServiceDate = s.d_ServiceDate,
                                     d_Birthdate = pe.d_Birthdate,
                                     i_DocTypeId = pe.i_DocTypeId,
                                     i_NumberDependentChildren = pe.i_NumberDependentChildren,
                                     i_NumberLivingChildren = pe.i_NumberLivingChildren,
                                     FirmaTrabajador = pe.b_RubricImage,
                                     HuellaTrabajador = pe.b_FingerPrintImage,
                                     v_BloodGroupName = gs.v_Value1,
                                     v_BloodFactorName = fs.v_Value1
                                 });

                // Medico Examen fisico

                //var medico = (from sc in dbContext.servicecomponent
                //              join J1 in dbContext.systemuser on new { i_InsertUserId = sc.i_InsertUserId.Value }
                //                          equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                //              from J1 in J1_join.DefaultIfEmpty()
                //              join pe in dbContext.person on J1.v_PersonId equals pe.v_PersonId
                //              where (sc.v_ServiceId == serviceId) &&
                //                      (sc.v_ComponentId == Constants.EXAMEN_FISICO_ID)
                //              select pe.v_FirstName + " " + pe.v_FirstLastName).SingleOrDefault<string>();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.PacientList
                           {
                               v_PersonId = a.v_PersonId,
                               i_DocTypeId = a.i_DocTypeId,
                               v_FirstName = a.v_FirstName,
                               v_FirstLastName = a.v_FirstLastName,
                               v_SecondLastName = a.v_SecondLastName,
                               i_Age = GetAge(a.d_Birthdate.Value),
                               b_Photo = a.b_Photo,
                               v_TypeOfInsuranceName = a.v_TypeOfInsuranceName,
                               v_FullWorkingOrganizationName = a.v_FullWorkingOrganizationName,
                               v_RelationshipName = a.v_RelationshipName,
                               v_OwnerName = a.v_FirstName + " " + a.v_FirstLastName + " " + a.v_SecondLastName,
                               d_ServiceDate = a.d_ServiceDate,
                               i_NumberDependentChildren = a.i_NumberDependentChildren,
                               i_NumberLivingChildren = a.i_NumberLivingChildren,
                               v_OwnerOrganizationName = (from n in dbContext.organization
                                                          where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                                          select n.v_Name).SingleOrDefault<string>(),
                               v_DoctorPhysicalExamName = (from sc in dbContext.servicecomponent
                                                           join J1 in dbContext.systemuser on new { i_InsertUserId = sc.i_ApprovedUpdateUserId.Value }
                                                                      equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                           from J1 in J1_join.DefaultIfEmpty()
                                                           join pe in dbContext.person on J1.v_PersonId equals pe.v_PersonId
                                                           where (sc.v_ServiceId == serviceId) &&
                                                                 (sc.v_ComponentId == Constants.EXAMEN_FISICO_ID)
                                                           select pe.v_FirstName + " " + pe.v_FirstLastName).SingleOrDefault<string>(),
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               v_BloodGroupName = a.v_BloodGroupName,
                               v_BloodFactorName = a.v_BloodFactorName

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

        public List<Sigesoft.Node.WinClient.BE.ServiceList> GetMusculoEsqueletico(string pstrserviceId, string pstrComponentId)
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

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     d_ServiceDate = A.d_ServiceDate,
                                     EmpresaTrabajo = D.v_Name,
                                     v_ServiceId = A.v_ServiceId,
                                     v_ComponentId = E.v_ServiceComponentId

                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()

                           let OsteoMuscular = new ServiceBL().ValoresComponente(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1)
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_PersonId = a.v_PersonId,
                               v_Pacient = a.v_Pacient,
                               d_ServiceDate = a.d_ServiceDate,
                               EmpresaTrabajo = a.EmpresaTrabajo,
                               v_ServiceId = a.v_ServiceId,
                               v_ComponentId = a.v_ComponentId,

                               AbdomenExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_EXCELENTE_ID).v_Value1,
                               AbdomenPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_PROMEDIO_ID).v_Value1,
                               AbdomenRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_REGULAR_ID).v_Value1,
                               AbdomenPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_POBRE_ID).v_Value1,
                               AbdomenPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_PUNTOS_ID).v_Value1,
                               AbdomenObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_OBSERVACIONES_ID).v_Value1,
                               CaderaExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_EXCELENTE_ID).v_Value1,
                               CaderaPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_PROMEDIO_ID).v_Value1,
                               CaderaRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_REGULAR_ID).v_Value1,
                               CaderaPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_POBRE_ID).v_Value1,
                               CaderaPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_PUNTOS_ID).v_Value1,
                               CaderaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_OBSERVACIONES_ID).v_Value1,
                               MusloExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_EXCELENTE_ID).v_Value1,
                               MusloPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_PROMEDIO_ID).v_Value1,
                               MusloRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_REGULAR_ID).v_Value1,
                               MusloPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_POBRE_ID).v_Value1,
                               MusloPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_PUNTOS_ID).v_Value1,
                               MusloObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_OBSERVACIONES_ID).v_Value1,
                               AbdomenLateralExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_EXCELENTE_ID).v_Value1,
                               AbdomenLateralPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_PROMEDIO_ID).v_Value1,
                               AbdomenLateralRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_REGULAR_ID).v_Value1,
                               AbdomenLateralPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_POBRE_ID).v_Value1,
                               AbdomenLateralPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_PUNTOS_ID).v_Value1,
                               AbdomenLateralObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_OBSERVACIONES_ID).v_Value1,
                               AbduccionHombroNormalOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_OPTIMO_ID).v_Value1,
                               AbduccionHombroNormalLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_LIMITADO_ID).v_Value1,
                               AbduccionHombroNormalMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_MUY_LIMITADO_ID).v_Value1,
                               AbduccionHombroNormalPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_PUNTOS_ID).v_Value1,
                               AbduccionHombroNormalObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_DOLOR_ID).v_Value1Name,
                               AbduccionHombroOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_OPTIMO_ID).v_Value1,
                               AbduccionHombroLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_LIMITADO_ID).v_Value1,
                               AbduccionHombroMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_MUY_LIMITADO_ID).v_Value1,
                               AbduccionHombroPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_PUNTOS_ID).v_Value1,
                               AbduccionHombroObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_DOLOR_ID).v_Value1Name,
                               RotacionExternaOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_OPTIMO_ID).v_Value1,
                               RotacionExternaLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_LIMITADO_ID).v_Value1,
                               RotacionExternaMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_MUY_LIMITADO_ID).v_Value1,
                               RotacionExternaPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_PUNTOS_ID).v_Value1,
                               RotacionExternaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_DOLOR_ID).v_Value1Name,
                               RotacionExternaHombroInternoOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_OPTIMO_ID).v_Value1,
                               RotacionExternaHombroInternoLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_LIMITADO_ID).v_Value1,
                               RotacionExternaHombroInternoMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_MUY_LIMITADO_ID).v_Value1,
                               RotacionExternaHombroInternoPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_PUNTOS_ID).v_Value1,
                               RotacionExternaHombroInternoObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_DOLOR_ID).v_Value1Name,
                               Total1 = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOTAL1_ID).v_Value1,
                               Total2 = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOTAL2_ID).v_Value1,
                               AptitudMusculoEsqueletico = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_APTITUD_ID).v_Value1,
                               Conclusiones = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID).v_Value1,

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

        public List<Sigesoft.Node.WinClient.BE.ReportOftalmologia> GetOftalmologia(string pstrserviceId, string pstrComponentId)
        {
            //mon.IsActive = true;

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

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join prtec in dbContext.professional on tec.v_PersonId equals prtec.v_PersonId into prtec_join
                                 from prtec in prtec_join.DefaultIfEmpty()

                                 join petec in dbContext.person on tec.v_PersonId equals petec.v_PersonId into petec_join
                                 from petec in petec_join.DefaultIfEmpty()
                                 // *******************************************************                            

                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.ReportOftalmologia
                                 {
                                     v_ComponentId = E.v_ComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     ServicioId = A.v_ServiceId,
                                     NombrePaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     EmprresaTrabajadora = D.v_Name,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     FirmaDoctor = pme.b_SignatureImage,
                                     FirmaTecnologo = prtec.b_SignatureImage,
                                     NombreTecnologo = petec.v_FirstLastName + " " + petec.v_SecondLastName + " " + petec.v_FirstName,
                                     v_ServiceComponentId = E.v_ServiceComponentId
                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();


                var sql = (from a in objEntity.ToList()

                           let oftalmo = serviceBL.ValoresComponente(pstrserviceId, Constants.OFTALMOLOGIA_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportOftalmologia
                           {
                               v_ComponentId = a.v_ServiceComponentId,
                               v_ServiceId = a.v_ServiceId,
                               ServicioId = a.ServicioId,
                               NombrePaciente = a.NombrePaciente,
                               EmprresaTrabajadora = a.EmprresaTrabajadora,
                               FechaServicio = a.FechaServicio,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               PuestoTrabajo = a.PuestoTrabajo,

                               UsoCorrectoresSi = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORRECTORES_OCULARES_SI_ID).v_Value1,
                               UsoCorrectoresNo = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORRECTORES_OCULARES_NO_ID).v_Value1,
                               UltimaRefraccion = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORRECTORES_OCULARES_ULTIMA_REFRACCION_ID).v_Value1,
                               Hipertension = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_HIPERTENSION_ID).v_Value1,
                               DiabetesMellitus = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_DIABETES_ID).v_Value1,
                               Glaucoma = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_GLAUCOMA_ID).v_Value1,
                               TraumatismoOcular = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TRAUMATISMO_ID).v_Value1,
                               Ambliopia = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AMBLIOPIA_ID).v_Value1,
                               SustQuimica = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SUST_QUIMICAS_ID).v_Value1,
                               Soldadura = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SOLDADURA_ID).v_Value1,
                               Catarata = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CATARATAS_ID).v_Value1,
                               Otros = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_OTROS_ESPECIFICAR_ID).v_Value1,
                               AELejosOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_LEJOS_OJO_DERECHO_ID).v_Value1,
                               SCCercaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_CERCA_OJO_IZQUIERDO_ID).v_Value1,
                               //AELejosOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_LEJOS_OJO_IZQUIERDO_ID).v_Value1,
                               SCLejosOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_LEJOS_OJO_IZQUIERDO_ID).v_Value1,
                               CCLejosOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_LEJOS_OJO_DERECHO_ID).v_Value1,
                               SCCercaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_CERCA_OJO_DERECHO_ID).v_Value1,
                               SCLejosOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_LEJOS_OJO_DERECHO_ID).v_Value1,
                               CCCercasOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_CERCA_OJO_DERECHO_ID).v_Value1,
                               CCLejosOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_LEJOS_OJO_IZQUIERDO_ID).v_Value1,
                               //AECercaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_CERCA_OJO_IZQUIERDO_ID).v_Value1,
                               //AECercaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_CERCA_OJO_DERECHO_ID).v_Value1,
                               CCCercasOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_CERCA_OJO_IZQUIERDO_ID).v_Value1,
                               //MaculaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MACULA_OJO_DERECHO_ID).v_Value1,
                               //MaculaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MACULA_OJO_IZQUIERDO_ID).v_Value1,
                               //RetinaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_RETINA_OJO_DERECHO_ID).v_Value1,
                               //RetinaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_RETINA_OJO_IZQUIERDO_ID).v_Value1,
                               //NervioOpticoOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_NERVIO_OPTICO_DERECHO_ID).v_Value1,
                               //NervioOpticoOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_NERVIO_OPTICO_IZQUIERDO_ID).v_Value1,
                               ParpadoOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PARPADO_OJO_DERECHO_ID).v_Value1,
                               ParpadoOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PARPADO_OJO_IZQUIERDO_ID).v_Value1,
                               ConjuntivaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONJUNTIVA_OJO_DERECHO_ID).v_Value1,
                               ConjuntivaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONJUNTIVA_OJO_IZQUIERDO_ID).v_Value1,
                               CorneaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORNEA_OJO_DERECHO_ID).v_Value1,
                               CorneaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORNEA_OJO_IZQUIERDO_ID).v_Value1,
                               //CristalinoOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CRISTALINO_OJO_DERECHO_ID).v_Value1,
                               //CristalinoOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CRISTALINO_OJO_IZQUIERDO_ID).v_Value1,
                               IrisOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_IRIS_OJO_DERECHO_ID).v_Value1,
                               IrisOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_IRIS_OJO_IZQUIERDO_ID).v_Value1,
                               MovOcularesOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MOV_OCULARES_OJO_DERECHO_ID).v_Value1,
                               MovOcularesOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MOV_OCULARES_OJO_IZQUIERDO_ID).v_Value1,
                               ConfrontacionODCompleto = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_COMPLETO_OJO_DERECHO_ID).v_Value1,
                               ConfrontacionODRestringido = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_RESTRINGIDO_OJO_DERECHO_ID).v_Value1,
                               //ConfrontacionOICompleto = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_COMPLETO_OJO_IZQUIERDO_ID).v_Value1,
                               //ConfrontacionOIRestringido = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_RESTRINGIDO_OJO_IZQUIERDO_ID).v_Value1,
                               TestIshiharaNormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ISHIHARA_NORMAL_ID).v_Value1,
                               TestIshiharaAnormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ISHIHARA_ANORMAL_ID).v_Value1,
                               Discromatopsia = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_DICROMATOPSIA_ID).v_Value1Name,
                               TestEstereopsisTiempo = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ESTEREOPSIS_TIEMPO_ID).v_Value1,
                               TestEstereopsisNormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ESTEREOPSIS_NORMAL_ID).v_Value1,
                               TestEstereopsisAnormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ESTEREOPSIS_ANORMAL_ID).v_Value1,
                               PresionIntraocularOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PRESION_INTRAOCULAR_OJO_DERECHO_ID).v_Value1,
                               //PresionIntraocularOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PRESION_INTRAOCULAR_OJO_IZQUIERDO_ID).v_Value1,
                               Hallazgos = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_HALLAZGOS_ID).v_Value1,
                               Diagnosticos = GetDisgnosticsByServiceIdAndComponentConcatec(a.ServicioId, Constants.OFTALMOLOGIA_ID),
                               Recomendaciones = GetRecomendationByServiceIdAndComponentConcatec(a.ServicioId, Constants.OFTALMOLOGIA_ID),
                               FirmaDoctor = a.FirmaDoctor,
                               FirmaTecnologo = a.FirmaTecnologo,
                               NombreTecnologo = a.NombreTecnologo,

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

        public string GetDisgnosticsByServiceIdAndComponentConcatec(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from ccc in dbContext.diagnosticrepository
                             join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos                                                  


                             where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponentId &&
                                   ccc.i_IsDeleted == 0

                             select new
                             {

                                 v_DiseasesName = ddd.v_Name,

                             }).ToList();


                return string.Join(", ", query.Select(p => p.v_DiseasesName));
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public string GetRecomendationByServiceIdAndComponentConcatec(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from ccc in dbContext.recommendation
                             join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId


                             where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponentId &&
                                   ccc.i_IsDeleted == 0

                             select new
                             {

                                 Recomendations = ddd.v_Name,

                             }).ToList();


                return string.Join(", ", query.Select(p => p.Recomendations));
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        // Alberto
        public List<Sigesoft.Node.WinClient.BE.ServiceList> GetFichaPsicologicaOcupacional_(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = "N002-ME000000033" }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 //*********************************************************************

                                 join H in dbContext.systemparameter on new { a = C.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_BirthPlace = B.v_BirthPlace,
                                     i_DiaN = B.d_Birthdate.Value.Day,
                                     i_MesN = B.d_Birthdate.Value.Month,
                                     i_AnioN = B.d_Birthdate.Value.Year,
                                     i_DiaV = A.d_ServiceDate.Value.Day,
                                     i_MesV = A.d_ServiceDate.Value.Month,
                                     i_AnioV = A.d_ServiceDate.Value.Year,
                                     NivelInstruccion = J1.v_Value1,
                                     LugarResidencia = B.v_AdressLocation,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     EmpresaTrabajo = D.v_Name,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     Rubrica = pme.b_SignatureImage,
                                     RubricaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     v_EsoTypeName = H.v_Value1,
                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_ServiceId = a.v_ServiceId,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               v_PersonId = a.v_PersonId,
                               v_Pacient = a.v_Pacient,
                               i_Edad = GetAge(a.d_BirthDate.Value),
                               v_BirthPlace = a.v_BirthPlace,
                               i_DiaN = a.i_DiaN,
                               i_MesN = a.i_MesN,
                               i_AnioN = a.i_AnioN,
                               i_DiaV = a.i_DiaV,
                               i_MesV = a.i_MesV,
                               i_AnioV = a.i_AnioV,
                               NivelInstruccion = a.NivelInstruccion,
                               LugarResidencia = a.LugarResidencia,
                               PuestoTrabajo = a.PuestoTrabajo,
                               EmpresaTrabajo = a.EmpresaTrabajo,
                               MotivoEvaluacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000076", "NOCOMBO", 0, "NO"),
                               NivelIntelectual = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000630", "NOCOMBO", 0, "NO"),
                               CoordinacionVisomotriz = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000631", "NOCOMBO", 0, "NO"),
                               NivelMemoria = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000632", "NOCOMBO", 0, "NO"),
                               Personalidad = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000633", "NOCOMBO", 0, "NO"),
                               Afectividad = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000634", "NOCOMBO", 0, "NO"),
                               AreaCognitiva = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000336", "SICOMBO", 195, "NO"),
                               AreaEmocional = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000337", "SICOMBO", 195, "NO"),
                               Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000338", "NOCOMBO", 0, "NO"),
                               Restriccion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000081", "NOCOMBO", 0, "NO"),
                               Recomendacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000080", "SICOMBO", 190, "NO"),

                               Presentacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000283", "SICOMBO", 175, "SI"),

                               Postura = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000282", "SICOMBO", 173, "SI"),

                               DiscursoRitmo = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000280", "SICOMBO", 214, "SI"),
                               DiscursoTono = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000281", "SICOMBO", 215, "SI"),
                               DiscursoArticulacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000279", "SICOMBO", 216, "SI"),

                               OrientacionTiempo = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000077", "SICOMBO", 189, "SI"),
                               OrientacionEspacio = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000078", "SICOMBO", 189, "SI"),
                               OrientacionPersona = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000079", "SICOMBO", 189, "SI"),
                               Rubrica = a.Rubrica,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               AreaPersonal = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000001298", "NOCOMBO", 0, "NO"),
                               v_EsoTypeName = a.v_EsoTypeName,
                               RubricaTrabajador = a.RubricaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<Sigesoft.Node.WinClient.BE.ServiceList> GetFichaPsicologicaOcupacional(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = "N002-ME000000033" }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 //*********************************************************************

                                 join H in dbContext.systemparameter on new { a = C.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_BirthPlace = B.v_BirthPlace,
                                     i_DiaN = B.d_Birthdate.Value.Day,
                                     i_MesN = B.d_Birthdate.Value.Month,
                                     i_AnioN = B.d_Birthdate.Value.Year,
                                     i_DiaV = A.d_ServiceDate.Value.Day,
                                     i_MesV = A.d_ServiceDate.Value.Month,
                                     i_AnioV = A.d_ServiceDate.Value.Year,
                                     NivelInstruccion = J1.v_Value1,
                                     LugarResidencia = B.v_AdressLocation,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     EmpresaTrabajo = D.v_Name,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     Rubrica = pme.b_SignatureImage,
                                     RubricaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     v_EsoTypeName = H.v_Value1,
                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Psico = new ServiceBL().ValoresComponente(pstrserviceId, Constants.PSICOLOGIA_ID)
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_ServiceId = a.v_ServiceId,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               v_PersonId = a.v_PersonId,
                               v_Pacient = a.v_Pacient,
                               i_Edad = GetAge(a.d_BirthDate.Value),
                               v_BirthPlace = a.v_BirthPlace,
                               i_DiaN = a.i_DiaN,
                               i_MesN = a.i_MesN,
                               i_AnioN = a.i_AnioN,
                               i_DiaV = a.i_DiaV,
                               i_MesV = a.i_MesV,
                               i_AnioV = a.i_AnioV,
                               NivelInstruccion = a.NivelInstruccion,
                               LugarResidencia = a.LugarResidencia,
                               PuestoTrabajo = a.PuestoTrabajo,
                               EmpresaTrabajo = a.EmpresaTrabajo,
                               MotivoEvaluacion = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_APTITUD_PSICOLOGICA) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_APTITUD_PSICOLOGICA).v_Value1,
                               NivelIntelectual = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.NivelIntelectual) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.NivelIntelectual).v_Value1,
                               CoordinacionVisomotriz = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.CoordinacionVisomotriz) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.CoordinacionVisomotriz).v_Value1,
                               NivelMemoria = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.NivelMemoria) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.NivelMemoria).v_Value1,
                               Personalidad = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.Personalidad) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.Personalidad).v_Value1,
                               Afectividad = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.Afectividad) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.Afectividad).v_Value1,
                               Conclusiones = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_AREA_CONCLUSIONES_ID) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_AREA_CONCLUSIONES_ID).v_Value1,
                               Restriccion = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_RECOMENDACIONES_ESPECIFICAS) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_RECOMENDACIONES_ESPECIFICAS).v_Value1,
                               AreaPersonal = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.AreaPersonal) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.AreaPersonal).v_Value1,





                               AreaCognitiva = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_AREA_COGNITIVA_ID) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_AREA_COGNITIVA_ID).v_Value1,
                               AreaEmocional = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_AREA_EMOCIONAL_ID) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.PSICOLOGIA_AREA_EMOCIONAL_ID).v_Value1,
                               Recomendacion = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.Recomendacion) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.Recomendacion).v_Value1,
                               Presentacion = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.Presentacion) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.Presentacion).v_Value1,
                               Postura = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.Postura) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.Postura).v_Value1,
                               DiscursoRitmo = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.DiscursoRitmo) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.DiscursoRitmo).v_Value1,
                               DiscursoTono = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.DiscursoTono) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.DiscursoTono).v_Value1,
                               DiscursoArticulacion = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.DiscursoArticulacion) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.DiscursoArticulacion).v_Value1,
                               OrientacionTiempo = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.OrientacionTiempo) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.OrientacionTiempo).v_Value1,
                               OrientacionEspacio = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.OrientacionEspacio) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.OrientacionEspacio).v_Value1,
                               OrientacionPersona = Psico.Count == 0 || Psico.Find(p => p.v_ComponentFieldId == Constants.OrientacionPersona) == null ? string.Empty : Psico.Find(p => p.v_ComponentFieldId == Constants.OrientacionPersona).v_Value1,



                               Rubrica = a.Rubrica,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,


                               v_EsoTypeName = a.v_EsoTypeName,
                               RubricaTrabajador = a.RubricaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alberto
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
                    xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
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

        public List<Sigesoft.Node.WinClient.BE.ReportAlturaEstructural> GetAlturaEstructural(string pstrserviceId, string pstrComponentId)
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
                                 join F in dbContext.systemuser on E.i_ApprovedUpdateUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportAlturaEstructural
                                 {
                                     v_ComponentId = E.v_ServiceComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     NombrePaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     EmpresaTrabajadora = D.v_Name,
                                     Fecha = A.d_ServiceDate.Value,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     RubricaMedico = G.b_SignatureImage,
                                     RubricaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage
                                 });


                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var funcionesVitales = serviceBL.ReportFuncionesVitales(pstrserviceId, Constants.FUNCIONES_VITALES_ID);
                var antropometria = serviceBL.ReportAntropometria(pstrserviceId, Constants.ANTROPOMETRIA_ID);

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportAlturaEstructural
                           {
                               v_ComponentId = a.v_ComponentId,
                               v_ServiceId = a.v_ServiceId,
                               ServicioId = a.ServicioId,
                               NombrePaciente = a.NombrePaciente,
                               EmpresaTrabajadora = a.EmpresaTrabajadora,
                               Fecha = a.Fecha,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               PuestoTrabajo = a.PuestoTrabajo,

                               AntecedenteTecSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ANTECEDENTE_TEC_SI_ID, "NOCOMBO", 0, "SI"),
                               AntecedenteTecNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ANTECEDENTE_TEC_NO_ID, "NOCOMBO", 0, "SI"),
                               AntecedenteTecObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ANTECEDENTE_TEC_OBS_ID, "NOCOMBO", 0, "SI"),

                               ConvulsionesSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CONVULSIONES_EPILEPSIA_SI_ID, "NOCOMBO", 0, "SI"),
                               ConvulsionesNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CONVULSIONES_EPILEPSIA_NO_ID, "NOCOMBO", 0, "SI"),
                               ConvulsionesObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CONVULSIONES_EPILEPSIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               MareosSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_MAREOS_SI_ID, "NOCOMBO", 0, "SI"),
                               MareosNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_MAREOS_NO_ID, "NOCOMBO", 0, "SI"),
                               MareosObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_MAREOS_OBS_ID, "NOCOMBO", 0, "SI"),

                               AgorafobiaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_AGORAFOBIA_SI_ID, "NOCOMBO", 0, "SI"),
                               AgorafobiaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_AGORAFOBIA_NO_ID, "NOCOMBO", 0, "SI"),
                               AgorafobiaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_AGORAFOBIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               AcrofobiaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ACROFOBIA_SI_ID, "NOCOMBO", 0, "SI"),
                               AcrofobiaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ACROFOBIA_NO_ID, "NOCOMBO", 0, "SI"),
                               AcrofobiaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ACROFOBIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               InsuficienciaCardiacaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_INSUFICIENCIA_CARDIACA_SI_ID, "NOCOMBO", 0, "SI"),
                               InsuficienciaCardiacaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_INSUFICIENCIA_CARDIACA_NO_ID, "NOCOMBO", 0, "SI"),
                               InsuficienciaCardiacaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_INSUFICIENCIA_CARDIACA_OBS_ID, "NOCOMBO", 0, "SI"),

                               EstereopsiaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ESTEREOPSIA_SI_ID, "NOCOMBO", 0, "SI"),
                               EstereopsiaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ESTEREOPSIA_NO_ID, "NOCOMBO", 0, "SI"),
                               EstereopsiaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ESTEREOPSIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               NistagmusEspontaneo = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_NISTAGMUS_ESPONTANEO_ID, "NOCOMBO", 0, "SI"),
                               NistagmusProvocado = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_NISTAGMUS_PROVOCADO_ID, "NOCOMBO", 0, "SI"),

                               PrimerosAuxilios = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_PRIMEROS_AUXILIOS_ID, "NOCOMBO", 0, "SI"),
                               TrabajoNivelMar = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_TRABAJO_SOBRE_NIVEL_ID, "NOCOMBO", 0, "SI"),

                               Timpanos = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_TIMPANOS_ID, "NOCOMBO", 0, "SI"),
                               Equilibrio = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_EQUILIBRIO_ID, "NOCOMBO", 0, "SI"),
                               SustentacionPie20Seg = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_SUST_PIE_20_ID, "NOCOMBO", 0, "SI"),
                               CaminarLibre3Mts = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CAMINAR_LIBRE_RECTA_3_ID, "NOCOMBO", 0, "SI"),
                               CaminarLibreVendado3Mts = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CAMINAR_LIBRE_OJOS_VENDADOS_3_ID, "NOCOMBO", 0, "SI"),
                               CaminarLibreVendadoPuntaTalon3Mts = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CAMINAR_LIBRE_OJOS_VENDADOS_PUNTA_TALON_3_ID, "NOCOMBO", 0, "SI"),
                               Rotar = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ROTAR_SILLA_GIRATORIA_ID, "NOCOMBO", 0, "SI"),
                               AdiadocoquinesiaDirecta = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ADIADOCOQUINESIA_DIRECTA_ID, "NOCOMBO", 0, "SI"),
                               AdiadocoquinesiaCruzada = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ADIADOCOQUINESIA_CRUZADA_ID, "NOCOMBO", 0, "SI"),
                               Apto = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_APTO_ID, "NOCOMBO", 0, "SI"),
                               descripcion = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               RubricaMedico = a.RubricaMedico,
                               RubricaTrabajador = a.RubricaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               IMC = antropometria.Count == 0 ? string.Empty : antropometria[0].IMC,
                               Peso = antropometria.Count == 0 ? string.Empty : antropometria[0].Peso,
                               FC = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].PA,
                               PA = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].PA,
                               FR = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].FR,
                               Sat = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].Sat,
                               PAD = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].PAD,
                               talla = antropometria.Count == 0 ? string.Empty : antropometria[0].talla,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<FileInfoDto> GetMultimediaFileByPersonId(ref OperationResult pobjOperationResult, string psrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.multimediafile
                                 where a.v_PersonId == psrPersonId
                                 && a.i_IsDeleted == 0
                                 select new FileInfoDto
                                 {
                                     MultimediaFileId = a.v_MultimediaFileId,
                                     FileName = a.v_FileName
                                     //ByteArrayFile = a.b_File
                                 }).ToList();

                pobjOperationResult.Success = 1;
                return objEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public FileInfoDto GetMultimediaFileById(ref OperationResult pobjOperationResult, string pstrMultimediaFileId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.multimediafile
                                 where a.v_MultimediaFileId == pstrMultimediaFileId
                                 && a.i_IsDeleted == 0
                                 select new FileInfoDto
                                 {
                                     MultimediaFileId = a.v_MultimediaFileId,
                                     FileName = a.v_FileName,
                                     ByteArrayFile = a.b_File
                                 }).FirstOrDefault();

                pobjOperationResult.Success = 1;
                return objEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }
        #endregion
    }
}
