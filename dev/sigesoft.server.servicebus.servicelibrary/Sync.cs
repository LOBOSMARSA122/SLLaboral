using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Server.WebClientAdmin.DAL;
using Sigesoft.Common;
//using Sigesoft.Common;

namespace Sigesoft.Server.ServiceBus.ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISync
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        bool IsNodeAbleToSync(ref OperationResult pobjOperationResult, int pintNodeId);

        [OperationContract]
        List<softwarecomponentreleaseDto> SoftwareComponentCheck(ref OperationResult pobjOperationResult, ref List<SoftwareComponentCheckDto> pobjSoftwareComponentsToCheck);

        [OperationContract]
        byte[] DownloadDeploymentFile(ref OperationResult pobjOperationResult, int pintDeploymentFileId);

        [OperationContract]
        bool AddDeploymentFile(int pintDeploymentFileId, byte[] filedata);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Sync : ISync
    {

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool IsNodeAbleToSync(ref OperationResult pobjOperationResult, int pintNodeId)
        {
            bool booAbleToSync = false;
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            try
            {
                // 1. Verificar si el servidor está habilitado para sincronización
                serversyncstatus objServerSyncStatus =
                    (from a in dbContext.serversyncstatus where a.i_NodeId == 1 select a).SingleOrDefault();

                if (objServerSyncStatus != null)
                {
                    if (objServerSyncStatus.i_Enabled == 1) // El servidor si permite la sincronización
                    {
                        // 2. Verificar si el servidor permite que la sincronización del NODO específicamente
                        servernodesync objServerNodeSync =
                            (from b in dbContext.servernodesync where b.i_NodeId == pintNodeId select b).SingleOrDefault();

                        if (objServerNodeSync != null)
                        {
                            if (objServerNodeSync.i_Enabled == 1) booAbleToSync = true;
                        }

                    }
                }

                pobjOperationResult.Success = 1;
                return booAbleToSync;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return false;
            }
        }

        public List<softwarecomponentreleaseDto> SoftwareComponentCheck(ref OperationResult pobjOperationResult, ref List<SoftwareComponentCheckDto> pobjSoftwareComponentsToCheck)
        {
            List<softwarecomponentreleaseDto> ServerComponentsToUpdate = new List<softwarecomponentreleaseDto>();

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Recorrer los componentes de software a verificar
                foreach (var NodeItem in pobjSoftwareComponentsToCheck)
                {
                    // Obtener la ultima version del componente de software
                    softwarecomponentrelease ServerItem =  (from a in dbContext.softwarecomponentrelease where a.i_SoftwareComponentId == NodeItem.i_SoftwareComponentId
                                  && a.i_IsPublished == 1 & a.i_IsLastVersion == 1
                                  select a).SingleOrDefault();

                    if (ServerItem != null) // Se encontró una versión publicada del componente
                    {                       
                        // Setear valores básicos
                        NodeItem.v_ServerVersion = ServerItem.v_SoftwareComponentVersion;
                        NodeItem.i_DeploymentFileId = ServerItem.i_DeploymentFileId;

                        // Comparar las versión del servidor del servidor contra la versión del nodo.
                        int intCompare = Utils.CompareVersionStrings(ServerItem.v_SoftwareComponentVersion, NodeItem.v_LocalVersion);
                        if (intCompare == 0) // Versiones son iguales (Server version = Node version)
                        {
                            NodeItem.b_RequireUpdate = false;
                        }
                        else if (intCompare > 0) // Server version > Node version
                        {
                            NodeItem.b_RequireUpdate = true;                            
                            NodeItem.i_SoftwareComponentId = ServerItem.i_SoftwareComponentId;
                            
                            // Agregarlo a la lista de componentes a devolver al invocador.
                            ServerComponentsToUpdate.Add(ServerItem.ToDTO());
                        }
                        else if (intCompare < 0) // Server version < Node version
                        {
                            // Esto NO puede ocurrir. Significaría que la versión del nodo ha sido alterada manualmente.

                            NodeItem.b_RequireUpdate = false;
                            throw new Exception("El Componente del nodo (" + NodeItem.v_LocalVersion +
                                ") tiene una versión superior a la del servidor (" + 
                                ServerItem.v_SoftwareComponentVersion + 
                                "). Debe haber un error o modificación no autorizada. Contactar con el administrador.");
                        }
                    }
                    else
                    {
                        // El componente no tiene versiones publicadas. Esto NO puede ocurrir por lo tanto debe notificarse.
                        throw new Exception("El Componente no tiene versiones publicadas. Contactar con el administrador.");
                    }
                }

                pobjOperationResult.Success = 1;
                return ServerComponentsToUpdate;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }

        }

        public byte[] DownloadDeploymentFile(ref OperationResult pobjOperationResult, int pintDeploymentFileId)
        {
            //List<softwarecomponentreleaseDto> ServerComponentsToUpdate = new List<softwarecomponentreleaseDto>();

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la ultima version del componente de software
                deploymentfile objDeploymentFile = (from a in dbContext.deploymentfile
                                                    where a.i_DeploymentFileId == pintDeploymentFileId
                                             select a).SingleOrDefault();

                if (objDeploymentFile != null) // Se encontró una versión publicada del componente
                {
                    if (objDeploymentFile.b_FileData != null)
                    {
                        // Si llegó hasta aquí entonces si hay archivo.
                    }
                    else
                    {
                        // El componente no tiene versiones publicadas. Esto NO puede ocurrir por lo tanto debe notificarse.
                        throw new Exception("No se encontró el contenido del archivo a descargar. Contactar con el administrador.");
                    }
                }
                else
                {
                    // El componente no tiene versiones publicadas. Esto NO puede ocurrir por lo tanto debe notificarse.
                    throw new Exception("No se encontró el release a descargar. Contactar con el administrador.");
                }

                pobjOperationResult.Success = 1;
                pobjOperationResult.ReturnValue = objDeploymentFile.v_FileName;
                return objDeploymentFile.b_FileData;  // Aqui se devuelve el conjunto de bytes
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                pobjOperationResult.OriginalExceptionMessage = ex.Message;
                return null;
            }

        }

        public bool AddDeploymentFile(int pintDeploymentFileId, byte[] filedata)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                deploymentfile newfile = new deploymentfile();
                newfile.i_DeploymentFileId = pintDeploymentFileId;
                newfile.v_Description = "Texto de descripción";
                newfile.b_FileData = filedata;
                newfile.v_FileName = "PKQ0001.ZIP";
                newfile.i_SoftwareComponentId = 2;
                newfile.v_TargetSoftwareComponentVersion = "1.0.0.1";

                dbContext.AddTodeploymentfile(newfile);
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
