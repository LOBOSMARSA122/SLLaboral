using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using Sigesoft.Common;

namespace Sigesoft.Node.WinClient.BLL
{
    public class FacturacionBL
    {
        public List<FacturacionList> GetFacturacionPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate, string TipoFecha)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.facturacion

                            // Empresa / Sede Cliente **************
                            join oc in dbContext.organization on new { a = A.v_EmpresaCliente }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join J4 in dbContext.datahierarchy on new { a = A.i_EstadoFacturacion.Value, b = 117 }
                                            equals new { a = J4.i_ItemId, b = J4.i_GroupId } into J4_join
                            from J4 in J4_join.DefaultIfEmpty()

                         
                            where A.i_IsDeleted == 0
                            select new FacturacionList
                            {
                                v_FacturacionId =  A.v_FacturacionId,
                                d_FechaRegistro = A.d_FechaRegistro.Value,
                                d_FechaCobro = A.d_FechaCobro,
                                v_NumeroFactura = A.v_NumeroFactura,
                                i_EstadoFacturacion = A.i_EstadoFacturacion.Value,
                                v_EstadoFacturacion =J4.v_Value1,
                                EmpresaCliente =  oc.v_Name,
                                v_EmpresaCliente = A.v_EmpresaCliente,
                                v_EmpresaSede = A.v_EmpresaSede,
                                d_MontoTotal = A.d_MontoTotal.Value,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = A.d_InsertDate,
                                d_UpdateDate = A.d_UpdateDate,
                                i_IsDeleted = A.i_IsDeleted,

                              
                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    if (TipoFecha == "F")
                    {
                        query = query.Where("d_FechaRegistro >= @0 && d_FechaRegistro <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                    }
                    else
                    {
                        query = query.Where("d_FechaCobro >= @0 && d_FechaCobro <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                    }
                  
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }
                if (pintPageIndex.HasValue && pintResultsPerPage.HasValue)
                {
                    int intStartRowIndex = pintPageIndex.Value * pintResultsPerPage.Value;
                    query = query.Skip(intStartRowIndex);
                }
                if (pintResultsPerPage.HasValue)
                {
                    query = query.Take(pintResultsPerPage.Value);
                }

                List<FacturacionList> objData = query.ToList();
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

        public string AddFacturacion(ref OperationResult pobjOperationResult, facturacionDto pobjDtoEntity,List<facturaciondetalleDto> ListaFacturacionDetalle , List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            string NewIdDetalle = "(No generado)";
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                serviceDto oserviceDto = new serviceDto();


                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                facturacion objEntity = facturacionAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 200), "UI");
                objEntity.v_FacturacionId = NewId;

                dbContext.AddTofacturacion(objEntity);
                dbContext.SaveChanges();

                if (ListaFacturacionDetalle != null)
                {
                    foreach (var item in ListaFacturacionDetalle)
                    {
                        // Crear el detalle del movimiento
                        facturaciondetalle objDetailEntity = facturaciondetalleAssembler.ToEntity(item);

                        NewIdDetalle = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 200), "UU");
                        objDetailEntity.v_FacturacionDetalleId = NewIdDetalle;
                        objDetailEntity.v_FacturacionId = NewId;

                        objDetailEntity.v_ServicioId = item.v_ServicioId;
                        objDetailEntity.d_Monto = 0;

                        objDetailEntity.d_InsertDate = DateTime.Now;
                        objDetailEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                        objDetailEntity.i_IsDeleted = 0;



                        // Agregar el detalle del movimiento a la BD
                        dbContext.AddTofacturaciondetalle(objDetailEntity);
                        oserviceDto.v_ServiceId =  item.v_ServicioId;
                        oServiceBL.UpdateFlagFacturacion(oserviceDto,1);
                    }
                    // Guardar todo en la BD
                    dbContext.SaveChanges();
                }

                pobjOperationResult.Success = 1;
                    return NewId;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                 return null;
            }
        }
        
        public facturacionDto GetFacturacion(ref OperationResult pobjOperationResult, string pstrFacturacionId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                facturacionDto objDtoEntity = null;

                var objEntity = (from a in dbContext.facturacion
                                 where a.v_FacturacionId == pstrFacturacionId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = facturacionAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<FacturacionDetalleList> GetListFacturacionDetalle(ref OperationResult pobjOperationResult, string pstrFacturacionId)
        {
            try
            {
                 SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                 var query = (from A in dbContext.facturaciondetalle
                              join C in dbContext.service on A.v_ServicioId  equals C.v_ServiceId
                              join B in dbContext.person on C.v_PersonId equals B.v_PersonId
                              join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                   equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                              from J1 in J1_join.DefaultIfEmpty()

                              join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                              equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                              from J2 in J2_join.DefaultIfEmpty()
                   

                              where A.v_FacturacionId == pstrFacturacionId && A.i_IsDeleted == 0
                              select new FacturacionDetalleList
                              {

                                  v_FacturacionDetalleId =  A.v_FacturacionDetalleId,
                                  v_FacturacionId = A.v_FacturacionId,
                                  v_ServicioId =  A.v_ServicioId,
                                  d_Monto = A.d_Monto,
                                  d_ServiceDate = C.d_ServiceDate,
                                  Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                  v_CreationUser = J1.v_UserName,
                                  v_UpdateUser = J2.v_UserName,
                                  d_CreationDate = A.d_InsertDate,
                                  d_UpdateDate = A.d_UpdateDate,
                                  i_IsDeleted = A.i_IsDeleted.Value,

                              }).ToList();



                 return query;
            }
            catch (Exception ex)
            {
                
                  pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public void UpdateFacturacion(ref OperationResult pobjOperationResult, facturacionDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.facturacion
                                       where a.v_FacturacionId == pobjDtoEntity.v_FacturacionId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                pobjDtoEntity.i_IsDeleted = 0;
                pobjDtoEntity.d_UpdateDate = DateTime.Now;
                pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                facturacion objEntity = facturacionAssembler.ToEntity(pobjDtoEntity);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.facturacion.ApplyCurrentValues(objEntity);

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                  return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                 return;
            }
        }

        public void DeleteFacturacion(ref OperationResult pobjOperationResult, string pstrFacturacionId, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                List<FacturacionDetalleList> Lista = new List<FacturacionDetalleList>();
                    OperationResult objOperationResult = new OperationResult();
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                ServiceBL oServiceBL = new ServiceBL();
                serviceDto oserviceDto = new serviceDto();


                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.facturacion
                                       where a.v_FacturacionId == pstrFacturacionId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.i_IsDeleted = 1;

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;


              Lista=  GetListFacturacionDetalle(ref objOperationResult, pstrFacturacionId);

              foreach (var item in Lista)
              {

                  DeleteFacturacionDetalle(ref objOperationResult, item.v_FacturacionDetalleId, ClientSession);
                  oserviceDto.v_ServiceId = item.v_ServicioId;
                  oServiceBL.UpdateFlagFacturacion(oserviceDto, 0);
              }
              
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return;
            }
        }

        public void DeleteFacturacionDetalle(ref OperationResult pobjOperationResult, string FacturacionDetalleId, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.facturaciondetalle
                                       where a.v_FacturacionDetalleId == FacturacionDetalleId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.i_IsDeleted = 1;

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
               return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return;
            }
        }

        public List<llenarConsultaSigesoft> LlenarGrillaSigesfot(string psrtDni, string pstrIdOrganization, string pstrIdLocation, DateTime FechaInico, DateTime FechaFin, int pintTipoExamen)
        {
            //mon.IsActive = true;

            string[] ExamenesLaboratorio = new string[] 
            { 
                Constants.GRUPO_Y_FACTOR_SANGUINEO_ID,
                Constants.LABORATORIO_HEMATOCRITO_ID,
                Constants.VDRL_ID,
                Constants.HEPATITIS_A_ID,
                Constants.HEPATITIS_C_ID,
                Constants.LABORATORIO_HEMOGLOBINA_ID,
                Constants.GLUCOSA_ID,
                Constants.ANTIGENO_PROSTATICO_ID,
                Constants.PARASITOLOGICO_SIMPLE_ID,
                Constants.ACIDO_URICO_ID,
                Constants.COLESTEROL_ID,
                Constants.TRIGLICERIDOS_ID,
                Constants.AGLUTINACIONES_LAMINA_ID,
                Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID,
                Constants.CREATININA_ID,
                Constants.EXAMEN_ELISA_ID,
                Constants.HEMOGRAMA_COMPLETO_ID,
                Constants.EXAMEN_COMPLETO_DE_ORINA_ID,
                Constants.PARASITOLOGICO_SERIADO_ID,
                Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID,
                Constants.TGO_ID,
                Constants.TGP_ID,
                Constants.PLOMO_SANGRE_ID,
                Constants.UREA_ID,
                Constants.COLESTEROL_HDL_ID,
                Constants.COLESTEROL_LDL_ID,
                Constants.COLESTEROL_VLDL_ID,
            };

            try
            {
                //SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                //var query = null;// dbContext.llenargrillasigesoft(psrtDni, pstrIdOrganization, pstrIdLocation, FechaInico, FechaFin, pintTipoExamen).ToList().OrderBy(p => p.IdComponente).ToList();
                //List<llenarConsultaSigesoft> Lista = new List<llenarConsultaSigesoft>();
                //llenarConsultaSigesoft o;

                //int iii = 0;
                //foreach (llenargrillasigesoftResult cs in query)
                //{
                //    iii++;
                //}

                //foreach (llenargrillasigesoftResult cs in query)
                //{
                 
                //    o = new llenarConsultaSigesoft();
                //    o.IdComponente = cs.IdComponente;
                //    o.IdService = cs.IdServicio;


                //    //VERIFICAR SI ES UN COMPONENTE DE LABORATORIO
                //    var EsLab = query.FindAll(p => ExamenesLaboratorio.Contains(o.IdComponente) && p.IdServicio == o.IdService).ToList();


                //    if (EsLab != null && EsLab.Count() != 0)
                //    {
                //        List<devolvervalorescomponenteResult> TieneValores = dbContext.devolvervalorescomponente(cs.IdComponente, cs.IdServicio).ToList();

                //        foreach (var item in TieneValores)
                //        {

                //        //    if (item.IDCAMPO == Constants.GLUCOSA_ID_REALIZADO && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.GLUCOSA_ID).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.GLUCOSA_ID).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID_REALIZADO && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.LABORATORIO_HEMATOCRITO_ID_REALIZADO && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMATOCRITO_ID).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMATOCRITO_ID).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.VDRL_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.VDRL_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.VDRL_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.HEPATITIS_A_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.HEPATITIS_A_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.HEPATITIS_A_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.HEPATITIS_C_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.HEPATITIS_C_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.HEPATITIS_C_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.LABORATORIO_HEMOGLOBINA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMOGLOBINA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMOGLOBINA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.ANTIGENO_PROSTATICO_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.ANTIGENO_PROSTATICO_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.ANTIGENO_PROSTATICO_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.PARASITOLOGICO_SIMPLE_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SIMPLE_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SIMPLE_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.ACIDO_URICO_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.ACIDO_URICO_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.ACIDO_URICO_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.COLESTEROL_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.TRIGLICERIDOS_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TRIGLICERIDOS_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TRIGLICERIDOS_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.AGLUTINACIONES_LAMINA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.AGLUTINACIONES_LAMINA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.AGLUTINACIONES_LAMINA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.CREATININA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.CREATININA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.CREATININA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.EXAMEN_ELISA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.EXAMEN_ELISA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.EXAMEN_ELISA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.HEMOGRAMA_COMPLETO_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.EXAMEN_COMPLETO_DE_ORINA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.EXAMEN_COMPLETO_DE_ORINA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.EXAMEN_COMPLETO_DE_ORINA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.PARASITOLOGICO_SERIADO_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SERIADO_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SERIADO_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }

                //        //    else if (item.IDCAMPO == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.TGO_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TGO_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TGO_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.TGP_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TGP_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TGP_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.PLOMO_SANGRE_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.PLOMO_SANGRE_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.PLOMO_SANGRE_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.UREA_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.UREA_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.UREA_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.COLESTEROL_HDL_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_HDL_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_HDL_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.COLESTEROL_LDL_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_LDL_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_LDL_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        //    else if (item.IDCAMPO == Constants.COLESTEROL_VLDL_ID && item.VALUE1 == "1")
                //        //    {
                //        //        o = new llenarConsultaSigesoft();                                
                //        //        o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_VLDL_ID_REALIZADO).Nombre_Componente;
                //        //        o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_VLDL_ID_REALIZADO).Total.ToString()));

                //        //        Lista.Add(o);
                //        //    }
                //        }
                //    }
                //    else
                //    {
                //        o = new llenarConsultaSigesoft();
                //        o.Nombre_Componente = cs.Nombre_Componente;
                //        o.Total = Math.Round(Decimal.Parse(cs.Total.Value.ToString()), 2);

                //        Lista.Add(o);
                //    }
                //}

                //var result_dt = (from r in Lista.AsEnumerable()
                //                 group r by r.Nombre_Componente into dtGroup
                //                 select new llenarConsultaSigesoft
                //                 {                                    
                //                     Nombre_Componente = dtGroup.Key,
                //                     Contador = dtGroup.Count(),
                //                     Total = dtGroup.Sum(r => r.Total)
                //                 }).ToList();

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public List<llenarConsultaSigesoft> ComisionVendedor(string psrtDni, string pstrIdOrganization, string pstrIdLocation, DateTime FechaInico, DateTime FechaFin, int pintTipoExamen)
        {
            ////mon.IsActive = true;

            //string[] ExamenesLaboratorio = new string[] 
            //{ 
            //    Constants.GRUPO_Y_FACTOR_SANGUINEO_ID,
            //    Constants.LABORATORIO_HEMATOCRITO_ID,
            //    Constants.VDRL_ID,
            //    Constants.HEPATITIS_A_ID,
            //    Constants.HEPATITIS_C_ID,
            //    Constants.LABORATORIO_HEMOGLOBINA_ID,
            //    Constants.GLUCOSA_ID,
            //    Constants.ANTIGENO_PROSTATICO_ID,
            //    Constants.PARASITOLOGICO_SIMPLE_ID,
            //    Constants.ACIDO_URICO_ID,
            //    Constants.COLESTEROL_ID,
            //    Constants.TRIGLICERIDOS_ID,
            //    Constants.AGLUTINACIONES_LAMINA_ID,
            //    Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID,
            //    Constants.CREATININA_ID,
            //    Constants.EXAMEN_ELISA_ID,
            //    Constants.HEMOGRAMA_COMPLETO_ID,
            //    Constants.EXAMEN_COMPLETO_DE_ORINA_ID,
            //    Constants.PARASITOLOGICO_SERIADO_ID,
            //    Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID,
            //    Constants.TGO_ID,
            //    Constants.TGP_ID,
            //    Constants.PLOMO_SANGRE_ID,
            //    Constants.UREA_ID,
            //    Constants.COLESTEROL_HDL_ID,
            //    Constants.COLESTEROL_LDL_ID,
            //    Constants.COLESTEROL_VLDL_ID,
            //};

            //try
            //{
            //    SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            //    var query = null;// dbContext.llenargrillasigesoft(psrtDni, pstrIdOrganization, pstrIdLocation, FechaInico, FechaFin, pintTipoExamen).ToList().OrderBy(p => p.IdComponente).ToList();
                
            //    List<llenarConsultaSigesoft> Lista = new List<llenarConsultaSigesoft>();
            //    llenarConsultaSigesoft o;

            //    foreach (llenargrillasigesoftResult cs in query)
            //    {
            //        o = new llenarConsultaSigesoft();
            //        o.IdComponente = cs.IdComponente;
            //        o.IdService = cs.IdServicio;


            //        //VERIFICAR SI ES UN COMPONENTE DE LABORATORIO
            //        var EsLab = query.FindAll(p => ExamenesLaboratorio.Contains(o.IdComponente)).ToList();


            //        if (EsLab != null && EsLab.Count() != 0)
            //        {
            //            List<devolvervalorescomponenteResult> TieneValores = dbContext.devolvervalorescomponente(cs.IdComponente, cs.IdServicio).ToList();

            //            foreach (var item in TieneValores)
            //            {

            //                if (item.IDCAMPO == Constants.GLUCOSA_ID_REALIZADO && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.GLUCOSA_ID).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.GLUCOSA_ID).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.GLUCOSA_ID).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID_REALIZADO && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.LABORATORIO_HEMATOCRITO_ID_REALIZADO && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMATOCRITO_ID).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMATOCRITO_ID).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMATOCRITO_ID).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.VDRL_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.VDRL_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.VDRL_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.VDRL_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.HEPATITIS_A_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.HEPATITIS_A_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.HEPATITIS_A_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.HEPATITIS_A_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.HEPATITIS_C_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.HEPATITIS_C_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.HEPATITIS_C_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.HEPATITIS_C_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.LABORATORIO_HEMOGLOBINA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMOGLOBINA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.LABORATORIO_HEMOGLOBINA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.ANTIGENO_PROSTATICO_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.ANTIGENO_PROSTATICO_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.ANTIGENO_PROSTATICO_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.ANTIGENO_PROSTATICO_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.PARASITOLOGICO_SIMPLE_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SIMPLE_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SIMPLE_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SIMPLE_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.ACIDO_URICO_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.ACIDO_URICO_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.ACIDO_URICO_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.ACIDO_URICO_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.COLESTEROL_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.COLESTEROL_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.TRIGLICERIDOS_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.TRIGLICERIDOS_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TRIGLICERIDOS_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TRIGLICERIDOS_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.AGLUTINACIONES_LAMINA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.AGLUTINACIONES_LAMINA_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.AGLUTINACIONES_LAMINA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.AGLUTINACIONES_LAMINA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.SUB_UNIDAD_BETA_CUALITATIVO_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.CREATININA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.CREATININA_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.CREATININA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.CREATININA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.EXAMEN_ELISA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.EXAMEN_ELISA_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.EXAMEN_ELISA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.EXAMEN_ELISA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.HEMOGRAMA_COMPLETO_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.EXAMEN_COMPLETO_DE_ORINA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.EXAMEN_COMPLETO_DE_ORINA_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.EXAMEN_COMPLETO_DE_ORINA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.EXAMEN_COMPLETO_DE_ORINA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.PARASITOLOGICO_SERIADO_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SERIADO_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SERIADO_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.PARASITOLOGICO_SERIADO_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }

            //                else if (item.IDCAMPO == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.TGO_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.TGO_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TGO_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TGO_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.TGP_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.TGP_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.TGP_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.TGP_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.PLOMO_SANGRE_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.PLOMO_SANGRE_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.PLOMO_SANGRE_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.PLOMO_SANGRE_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.UREA_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.UREA_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.UREA_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.UREA_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.COLESTEROL_HDL_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.COLESTEROL_HDL_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_HDL_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_HDL_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.COLESTEROL_LDL_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.COLESTEROL_LDL_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_LDL_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_LDL_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //                else if (item.IDCAMPO == Constants.COLESTEROL_VLDL_ID && item.VALUE1 == "1")
            //                {
            //                    o = new llenarConsultaSigesoft();
            //                    o.IdService = query.Find(p => p.IdComponente == Constants.COLESTEROL_VLDL_ID_REALIZADO).IdServicio;
            //                    o.Nombre_Componente = query.Find(p => p.IdComponente == Constants.COLESTEROL_VLDL_ID_REALIZADO).Nombre_Componente;
            //                    o.Total = Math.Round(Decimal.Parse(query.Find(p => p.IdComponente == Constants.COLESTEROL_VLDL_ID_REALIZADO).Total.ToString()));

            //                    Lista.Add(o);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            o = new llenarConsultaSigesoft();
            //            o.IdService = cs.IdServicio;
            //            o.Nombre_Componente = cs.Nombre_Componente;
            //            o.Total = Math.Round(Decimal.Parse(cs.Total.Value.ToString()), 2);

            //            Lista.Add(o);
            //        }
            //    }

            //    var result_dt = (from r in Lista.AsEnumerable()
            //                     group r by r.IdService into dtGroup
            //                     select new llenarConsultaSigesoft
            //                     {
            //                         IdService = dtGroup.Key,
            //                         Contador = dtGroup.Count(),
            //                         Total = dtGroup.Sum(r => r.Total)
            //                     }).ToList();

            //    return result_dt;

            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            return null;
        }

        public List<ImprimirFactura> CabeceraFactura(string pstrFacturacionId)
        {
            try
            {
                using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
                {
                   
                    var objEntity = (from A in dbContext.facturacion
                                     join B in dbContext.organization on A.v_EmpresaCliente equals B.v_OrganizationId
                                     where A.v_FacturacionId == pstrFacturacionId
                                     select new ImprimirFactura
                                     {
                                        
                                         NumeroFactura = A.v_NumeroFactura,
                                         RazonSocial = B.v_Name,
                                         Direccion = B.v_Address,
                                         Ruc = B.v_IdentificationNumber,
                                         FechaFacturacion = A.d_FechaRegistro,
                                         Igv = A.d_Igv.Value,
                                         SubTotal =A.d_SubTotal.Value,
                                         Total =A.d_MontoTotal.Value,
                                         Detraccion = A.d_Detraccion.Value
                                     }).ToList();

                    return objEntity;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    
    }
}
