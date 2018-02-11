using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Sigesoft.Node.WinClient.DAL;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using System.Linq.Dynamic;
using System.Xml.Serialization;
using System.IO;

namespace Sigesoft.Node.WinSynchronizer.UI
{
    public partial class frmTest01 : Form
    {
        List<TableInfoDto> _objTableInfoList = null;
        List<TableInfoDto> _objSelectedTablesList = null;
        string _strTableInfoXmlFile;
        string _strTempFolder = @"d:\Sync_Temp";
        DateTime _datLastSyncDate = new DateTime(2012, 12, 30);
        int _intNodeId = 1; // 1: Servidor. Cualquier otro id es un nodo.

        public frmTest01()
        {
            InitializeComponent();
        }

        private void frmTest01_Load(object sender, EventArgs e)
        {
            // Obtener la lista de tablas del sistema
            Deserialize_TableInfoXmlFile();
            grdData.DataSource = _objTableInfoList;

            // Obtener las tablas a procesar
            var query = from a in _objTableInfoList
                        where a.i_Sync == 1
                        orderby a.i_SyncOrder
                        select a;
            _objSelectedTablesList = query.ToList();
            grdTablesToProcess.DataSource = _objSelectedTablesList;
        }

        private void Deserialize_TableInfoXmlFile()
        {
            _strTableInfoXmlFile = Path.Combine(Common.Utils.GetApplicationExecutingFolder(), "TableInfo.xml");
            _objTableInfoList = (List<TableInfoDto>) Common.Utils.DeSerialize(typeof(List<TableInfoDto>), _strTableInfoXmlFile);
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            this.Sync();
        }

        private void Sync()
        {
            string strPackageFile = @"d:\Sync_Temp\Paquete.zip";
            DateTime Tiempo1 = DateTime.Now;

            PackageCreator objPackageCreator = new PackageCreator();
            objPackageCreator.GenerateNodePackage(_intNodeId, 
                                                    _objSelectedTablesList,
                                                    strPackageFile, 
                                                    _strTempFolder, 
                                                    _datLastSyncDate);

            DateTime Tiempo2 = DateTime.Now;
            TimeSpan ts = Tiempo2.Subtract(Tiempo1);

            MessageBox.Show("Paquete Generado en " + strPackageFile + " (Tomó:  "+ts.ToString() + ")");
        }
    }

    public class PackageCreator
    {
        private int _intNodeId;
        private DateTime _datLastSyncDate;
        private string _strOutputFolder;
        private SigesoftEntitiesModel _dbContext = new SigesoftEntitiesModel();
        private List<string> _filesToPackage;

        public void GenerateNodePackage(int pintNodeId, List<TableInfoDto> pobjSelectedTables, string pstrPackageFileName, string pstrOutputFolder, DateTime pdatLastSyncDate)
        {
            _filesToPackage = new List<string>();
            _datLastSyncDate = pdatLastSyncDate;
            _intNodeId = pintNodeId;
            _strOutputFolder = pstrOutputFolder;

            // Analizar cada tabla y exportar sus datos a archivos XML
            foreach (var item in pobjSelectedTables)
            {
                AnalizeTable(item);                
            }

            // Crear el archivo comprimido del paquete
            Common.Utils.CompressFile(pstrPackageFileName, _filesToPackage);

            // Eliminar los archivos generados
            Common.Utils.DeleteFiles(_filesToPackage);
        }

        public void AnalizeTable(TableInfoDto tableInfo)
        {
            if (tableInfo.v_Table == "applicationhierarchy")
                AnalizeTable_applicationhierarchy(tableInfo);
            else if (tableInfo.v_Table == "systemparameter")
                AnalizeTable_systemparameter(tableInfo);
            else if (tableInfo.v_Table == "datahierarchy")
                AnalizeTable_datahierarchy(tableInfo);
            else if (tableInfo.v_Table == "organization")
                AnalizeTable_organization(tableInfo);
            else if (tableInfo.v_Table == "location")
                AnalizeTable_location(tableInfo);
            else if (tableInfo.v_Table == "warehouse")
                AnalizeTable_warehouse(tableInfo);
            else if (tableInfo.v_Table == "area")
                AnalizeTable_area(tableInfo);
            else if (tableInfo.v_Table == "groupoccupation")
                AnalizeTable_groupoccupation(tableInfo);
            else if (tableInfo.v_Table == "ges")
                AnalizeTable_ges(tableInfo);
            else if (tableInfo.v_Table == "occupation")
                AnalizeTable_occupation(tableInfo);
            else if (tableInfo.v_Table == "node")
                AnalizeTable_node(tableInfo);
            else if (tableInfo.v_Table == "nodeorganizationprofile")
                AnalizeTable_nodeorganizationprofile(tableInfo);
            else if (tableInfo.v_Table == "nodeorganizationlocationprofile")
                AnalizeTable_nodeorganizationlocationprofile(tableInfo);
            else if (tableInfo.v_Table == "nodeorganizationlocationwarehouseprofile")
                AnalizeTable_nodeorganizationlocationwarehouseprofile(tableInfo);
            else if (tableInfo.v_Table == "rolenode")
                AnalizeTable_rolenode(tableInfo);
            else if (tableInfo.v_Table == "rolenodeprofile")
                AnalizeTable_rolenodeprofile(tableInfo);
            else if (tableInfo.v_Table == "person")
                AnalizeTable_person(tableInfo);
            else if (tableInfo.v_Table == "professional")
                AnalizeTable_professional(tableInfo);
            else if (tableInfo.v_Table == "pacient")
                AnalizeTable_pacient(tableInfo);
            else if (tableInfo.v_Table == "systemuser")
                AnalizeTable_systemuser(tableInfo);
            else if (tableInfo.v_Table == "restrictedwarehouseprofile")
                AnalizeTable_restrictedwarehouseprofile(tableInfo);
            else if (tableInfo.v_Table == "systemusergobalprofile")
                AnalizeTable_systemusergobalprofile(tableInfo);
            else if (tableInfo.v_Table == "systemuserrolenode")
                AnalizeTable_systemuserrolenode(tableInfo);
            else if (tableInfo.v_Table == "pacientmultimediadata")
                AnalizeTable_pacientmultimediadata(tableInfo);
            else if (tableInfo.v_Table == "cie10")
                AnalizeTable_cie10(tableInfo);
            else if (tableInfo.v_Table == "product")
                AnalizeTable_product(tableInfo);
            else if (tableInfo.v_Table == "supplier")
                AnalizeTable_supplier(tableInfo);
            else if (tableInfo.v_Table == "productwarehouse")
                AnalizeTable_productwarehouse(tableInfo);
            else if (tableInfo.v_Table == "movement")
                AnalizeTable_movement(tableInfo);
            else if (tableInfo.v_Table == "movementdetail")
                AnalizeTable_movementdetail(tableInfo);

            else if (tableInfo.v_Table == "log")
            {
                AnalizeTable_log(tableInfo);
            }
        }

        private string GenerateOutputFileName(string name, int type)
        {
            string retValue = null;

            if (type == 1)
            {
                retValue = Path.Combine(_strOutputFolder, "list_" + name + "_created.xml");
            }
            else if (type == 2)
            {
                retValue = Path.Combine(_strOutputFolder, "list_" + name + "_updated.xml");
            }

            _filesToPackage.Add(retValue);
            return retValue;

        }

        #region Lógica por cada tabla
        public void AnalizeTable_applicationhierarchy(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.applicationhierarchy
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.applicationhierarchy
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_systemparameter(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.systemparameter
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.systemparameter
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_datahierarchy(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.datahierarchy
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.datahierarchy
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_organization(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.organization
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.organization
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_location(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.location
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.location
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_warehouse(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.warehouse
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.warehouse
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_area(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.area
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.area
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_groupoccupation(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.groupoccupation
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.groupoccupation
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_ges(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.ges
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.ges
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_occupation(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.occupation
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.occupation
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_node(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.node
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.node
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_nodeorganizationprofile(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.nodeorganizationprofile
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.nodeorganizationprofile
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_nodeorganizationlocationprofile(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.nodeorganizationlocationprofile
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.nodeorganizationlocationprofile
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_nodeorganizationlocationwarehouseprofile(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.nodeorganizationlocationwarehouseprofile
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.nodeorganizationlocationwarehouseprofile
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_rolenode(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.rolenode
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.rolenode
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_rolenodeprofile(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.rolenodeprofile
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.rolenodeprofile
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_person(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.person
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.person
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_professional(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.professional
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.professional
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_pacient(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.pacient
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.pacient
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_systemuser(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.systemuser
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.systemuser
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_restrictedwarehouseprofile(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.restrictedwarehouseprofile
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.restrictedwarehouseprofile
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_systemusergobalprofile(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.systemusergobalprofile
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.systemusergobalprofile
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_systemuserrolenode(TableInfoDto tableInfo)
        {
            if (_intNodeId == 1) // Solo se obtiene data del servidor
            {
                var query_created = from a in _dbContext.systemuserrolenode
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.systemuserrolenode
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
                var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
                Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
            }
        }

        public void AnalizeTable_pacientmultimediadata(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.pacientmultimediadata
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.pacientmultimediadata
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_cie10(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.cie10
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.cie10
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_product(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.product
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.product
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_supplier(TableInfoDto tableInfo)
        {
            // Se obtiene del servidor o del nodo
            var query_created = from a in _dbContext.supplier
                                where a.d_InsertDate > _datLastSyncDate
                                select a;

            var query_updated = from a in _dbContext.supplier
                                where a.d_UpdateDate > _datLastSyncDate
                                select a;

            var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_productwarehouse(TableInfoDto tableInfo)
        {
            if (_intNodeId > 1) // Solo se obtiene data del nodo
            {
                var query_created = from a in _dbContext.productwarehouse
                                    where a.d_InsertDate > _datLastSyncDate
                                    select a;

                var query_updated = from a in _dbContext.productwarehouse
                                    where a.d_UpdateDate > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            }
        }

        public void AnalizeTable_movement(TableInfoDto tableInfo)
        {
            //// Se obtiene del servidor o del nodo
            //var query_created = from a in _dbContext.movement
            //                    where a.d_InsertDate > LastSyncDate
            //                    select a;

            //var query_updated = from a in _dbContext.movement
            //                    where a.d_UpdateDate > LastSyncDate
            //                    select a;

            //var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            //var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            //Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            //Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }

        public void AnalizeTable_movementdetail(TableInfoDto tableInfo)
        {
            //// Se obtiene del servidor o del nodo
            //var query_created = from a in _dbContext.movementdetail
            //                    where a.d_InsertDate > LastSyncDate
            //                    select a;

            //var query_updated = from a in _dbContext.movementdetail
            //                    where a.d_UpdateDate > LastSyncDate
            //                    select a;

            //var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.
            //var objDataListDtoUpdated = query_updated.ToList().ToDTOs(); // Obtener la data actualizada y pasarla a entidades simples.

            //Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            //Common.Utils.Serialize(objDataListDtoUpdated, GenerateOutputFileName(tableInfo.v_Table, 2)); // Serializar la Data actualizada
        }
        
        public void AnalizeTable_log(TableInfoDto tableInfo)
        {
            if (_intNodeId > 1) // Solo se obtiene data del nodo
            {
                var query_created = from a in _dbContext.log
                                    where a.d_Date > _datLastSyncDate
                                    select a;

                var objDataListDtoCreated = query_created.ToList().ToDTOs(); // Obtener la data creada y pasarla a entidades simples.

                Common.Utils.Serialize(objDataListDtoCreated, GenerateOutputFileName(tableInfo.v_Table, 1)); // Serializar la Data creada
            }
        }
        #endregion
    }
}
