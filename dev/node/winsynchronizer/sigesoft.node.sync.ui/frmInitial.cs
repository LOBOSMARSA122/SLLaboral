using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Common;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using Sigesoft.Server.WebClientAdmin.BE;

namespace Sigesoft.Node.Sync.UI
{
    public partial class frmInitial : Form
    {
        int _intNodeId;
        bool _booProcessSuccessful = false;
        Version _MainLogicFileVersion;
        string _MainLogicFileFormatedVersion;
        ProxySync.SyncClient _objSyncClient;
        List<SoftwareComponentCheckDto> _objSoftwareComponentToCheck;

        public frmInitial()
        {
            InitializeComponent();
        }

        private void frmInitial_Load(object sender, EventArgs e)
        {

            // ***** Leer información de configuración
            // Obtener el ID del nodo del archivo de configuración
            _intNodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));


            // ***** Pruebas de version
            //OperatingSystem os = Environment.OSVersion;
            //Version ver = os.Version;
            //AddTextLog(txtInitialLog, string.Format("Operating System: {0} / {1}", os.VersionString, ver.ToString()), false);

            //ver = Environment.Version;
            //AddTextLog(txtInitialLog, string.Format("Environment: {0}", ver.ToString()), false);

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            //Version v2 = new Version(fvi.FileVersion);
            //AddTextLog(txtInitialLog, "Assembly Version: (v2)" + v2.ToString(), false);

            //Version v1 = new Version("1.0.1");
            //AddTextLog(txtInitialLog, "v1: " + v1.ToString(), false);

            //int res = v1.CompareTo(v2);
            //AddTextLog(txtInitialLog, "v1.CompareTo(v2):" + res.ToString(), false);
            //03.01.01.10
            
            //Version v1 = new Version("1.3.1");
            //Version v3 = new Version("01.02.03.04");
            //AddTextLog(txtInitialLog, "v1 = " + v1.ToString(), false);
            //AddTextLog(txtInitialLog, "v3 = " + v3.ToString(), false);
            //AddTextLog(txtInitialLog, "v3 (formateada) = " + Utils.FormatVersionString(v3), false);
            //AddTextLog(txtInitialLog, "v3.CompareTo(v1) = " + v3.CompareTo(v1).ToString(), false);

            // ***** Información de Configuración
            // Versión de la librería de lógica
            string LogicFileAssembly = Utils.GetFileNameInApplicationExecutingFolder("Sigesoft.Node.Sync.MainLogic.dll");
            _MainLogicFileVersion = new Version(Utils.GetFileVersion(LogicFileAssembly));
            _MainLogicFileFormatedVersion = Utils.FormatVersionString(_MainLogicFileVersion);
            // Mostrar info de configuración
            AddTextLog(txtInitialLog, string.Format("Nodo Id: {0}", _intNodeId), false);
            AddTextLog(txtInitialLog, string.Format("Versión de la Librería de Lógica: {0}", _MainLogicFileFormatedVersion), false);

            // ***** Creación de objeto para chekeo de version
            _objSoftwareComponentToCheck = new List<SoftwareComponentCheckDto>();
            _objSoftwareComponentToCheck.Add(new SoftwareComponentCheckDto() { i_SoftwareComponentId = 2, v_LocalVersion = _MainLogicFileFormatedVersion });

            // ***** Instanciación del proxy del webservice
            _objSyncClient = new ProxySync.SyncClient();

            // ***** Iniciar el proceso
            btnClose.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        #region Process 1
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartInitialProcess(backgroundWorker1);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1) // Mostrar mensaje con fecha
            {
                AddTextLog(txtInitialLog, e.UserState.ToString(), true);
            }
            else if (e.ProgressPercentage == 2)  // Mostrar mensaje sin fecha
            {
                AddTextLog(txtInitialLog, e.UserState.ToString(), false);
            }
        }

        private void StartInitialProcess(BackgroundWorker bw)
        {
            _booProcessSuccessful = false;
            bool IsUpdateRequired = false;

            bw.ReportProgress(1, "Inicio del Proceso.");

            // ***** Verificación de Internet
            bw.ReportProgress(1, "Verificación de conectividad a internet...");
            //Thread.Sleep(1000);
            if (!Sigesoft.Common.Utils.InternetConnectivityAvailable("http://www.google.com.pe"))
            {
                bw.ReportProgress(1, "NO se detectó conexión a internet. Se cancela el proceso.");
                return;
            }
            else
            {
                bw.ReportProgress(1, "Conexión a internet satisfactoria. Se continúa el proceso.");
            }

            // ***** Invocación de WebService de Verificación de Sincronización
            bw.ReportProgress(1, "Verificando si el nodo (" + _intNodeId.ToString() + ") está habilitado para sincronizar...");
            OperationResult objOperationResult = new OperationResult();
            bool IsAbletToSync = _objSyncClient.IsNodeAbleToSync(ref objOperationResult, _intNodeId);
            if (objOperationResult.Success == 1)
            {
                if (IsAbletToSync)
                {
                    bw.ReportProgress(1, "Nodo autorizado para sincronización. Se continúa el proceso.");
                }
                else
                {
                    bw.ReportProgress(1, "El nodo NO ESTÁ autorizado para sincronización o el servidor no permite sincronización en este momento. Se cancela el proceso.");
                    return;
                }
            }
            else
            {
                bw.ReportProgress(1, "Se generó un error en el servicio:");
                bw.ReportProgress(2, objOperationResult.ExceptionMessage);
                bw.ReportProgress(1, "Se cancela el proceso.");
                return;
            }

            // ***** Invocación de WebService de Verificación de Versiones
            bw.ReportProgress(1, "Verificando versiones...");
            objOperationResult = new OperationResult();
            List<softwarecomponentreleaseDto> data = _objSyncClient.SoftwareComponentCheck(ref objOperationResult, ref _objSoftwareComponentToCheck);
            if (objOperationResult.Success == 1)
            {
                if (data.Count > 0)
                {
                    bw.ReportProgress(1, "Se encontraron componentes de sofware a actualizar.");
                    bw.ReportProgress(2, "Versión Nueva = " + _objSoftwareComponentToCheck[0].v_ServerVersion);
                    bw.ReportProgress(2, "Requiere Actualización = " + _objSoftwareComponentToCheck[0].b_RequireUpdate.ToString());
                    bw.ReportProgress(2, "DeploymentFileId = " + _objSoftwareComponentToCheck[0].i_DeploymentFileId.ToString());

                    if (_objSoftwareComponentToCheck[0].i_DeploymentFileId.HasValue)
                    {
                        IsUpdateRequired = true;
                    }
                    else
                    {
                        bw.ReportProgress(1, "Error en Servidor: NO se especificó el ID del DeploymentFile a descargar. Contactar al administrador. Se cancela el proceso.");
                        return;
                    }
                }
                else
                {
                    bw.ReportProgress(1, "NO se encontraron componentes de sofware a actualizar. Se continúa al proceso.");
                }
            }
            else
            {
                bw.ReportProgress(1, "Se generó un error en el servicio:");
                bw.ReportProgress(2, objOperationResult.ExceptionMessage);
                bw.ReportProgress(1, "Se cancela el proceso.");
                return;
            }

            // ***** Invocación de WebService para Descargar e instalar la Actualización
            if (IsUpdateRequired)
            {
                bw.ReportProgress(1, String.Format("Descargando actualización (Id: {0}) Versión: {1} ...",
                    _objSoftwareComponentToCheck[0].i_DeploymentFileId.ToString(), _objSoftwareComponentToCheck[0].v_ServerVersion));

                objOperationResult = new OperationResult();
                List<string> decompressedFiles = DownloadAndPatchSoftwareComponent(ref objOperationResult, 
                                                    Utils.GetApplicationExecutingFolder(), 
                                                    _objSoftwareComponentToCheck[0].i_DeploymentFileId.Value);
                if (objOperationResult.Success == 1)
                {
                    bw.ReportProgress(1, "Paquete y archivos instalados:");
                    bw.ReportProgress(2, "Paquete: " + objOperationResult.ReturnValue);
                    for (int i = 0; i < decompressedFiles.Count; i++)
                    {
                        bw.ReportProgress(2, string.Format("Archivo {0}/{1} : {2}", i + 1, decompressedFiles.Count, decompressedFiles[i]));                                                
                    }
                }
                else
                {
                    bw.ReportProgress(1, "Se generó un error al descargar / instalar la actualización:");
                    bw.ReportProgress(2, objOperationResult.ExceptionMessage);
                    bw.ReportProgress(1, "Se cancela el proceso.");
                    return;
                }
            }

            // ***** Culminación del Proceso
            bw.ReportProgress(1, "Culminación satisfactoria del proceso de actualización.");
            _booProcessSuccessful = true;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnClose.Enabled = true;

            // Guardar todo el LOG en un archivo
            Utils.SaveTextToFile(Path.Combine(Utils.GetApplicationExecutingFolder(), "log.txt"), txtInitialLog.Text);

            // Verificación si el proceso fué satisfactorio
            if (_booProcessSuccessful)
            {
                // ***** Ejecución de la lógica 
                AddTextLog(txtInitialLog, "Fin del Proceso. Ejecutando lógica...", true);
                Sigesoft.Node.Sync.MainLogic.frmMain frm = new MainLogic.frmMain();
                frm.ShowDialog();

                // Cerrar al terminar la lógica principal
                this.Close();
            }
        }
        #endregion

        #region Utils
        public void AddTextLog(TextBox pobjTextBox, string pstrMessage, bool pbooShowDateTime)
        {
            string strTextToAppend;
            if (pbooShowDateTime)
            {
                strTextToAppend = string.Format("[{0:yyyy/MM/dd H:mm:ss}] - {1}", DateTime.Now, pstrMessage);
            }
            else
            {
                strTextToAppend = string.Format("{0}", pstrMessage);
            }

            if (pobjTextBox.Text.Trim() == "")
            {
                pobjTextBox.Text = strTextToAppend;
            }
            else
            {
                pobjTextBox.AppendText(System.Environment.NewLine + strTextToAppend);
                //pobjTextBox.Text = pobjTextBox.Text + System.Environment.NewLine + strTextToAppend;
            }
            
        }

        private List<string> DownloadAndPatchSoftwareComponent(ref OperationResult pobjOperationResult, string pstrFolder, int pintDeploymentFileId)
        {
            // ***** Invocación de WebService para descargar un DeploymentFile
            OperationResult objOperationResult = new OperationResult();
            List<string> DecompressedFiles;
            try
            {
                byte[] data = _objSyncClient.DownloadDeploymentFile(ref objOperationResult, pintDeploymentFileId);

                if (objOperationResult.Success == 1)
                {
                    string strFile = objOperationResult.ReturnValue;
                    // Convertir los bytes en archivo
                    Utils.ByteArrayToFile(strFile, data);

                    // Desempaquetar y sobreescribir archivos existentes
                    DecompressedFiles = Utils.DecompressFile(strFile, pstrFolder);
                }
                else
                {
                    throw new Exception(objOperationResult.OriginalExceptionMessage);
                }

                pobjOperationResult.Success = 1;
                pobjOperationResult.ReturnValue = objOperationResult.ReturnValue;
                return DecompressedFiles; 
            }
            catch(Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }
        #endregion

        private void btnRetry_Click(object sender, EventArgs e)
        {
            //txtInitialLog.Text = "";
            btnClose.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void btnAddDeploymentFile_Click(object sender, EventArgs e)
        {
            string PackageFile = Path.Combine(Utils.GetApplicationExecutingFolder(), "PKQ0001.ZIP");
            List<string> Files = new List<string>();
            Files.Add(Path.Combine(Utils.GetApplicationExecutingFolder(), "Sigesoft.Node.Sync.MainLogic.dll"));
            Files.Add(Path.Combine(Utils.GetApplicationExecutingFolder(), "Ejemplo - Costeo Rápido 1.0.xlsx"));
            //Files.Add(Path.Combine(Utils.GetApplicationExecutingFolder(), "Sigesoft.Node.Sync.UI.exe.config"));

            // Crear el archivo comprimido
            Utils.CompressFile(PackageFile, Files);
            byte[] PackageFileByteArray = Utils.GetBytesOfFile(PackageFile);

            // Enviar el archivo al servidor
            _objSyncClient.AddDeploymentFile(1000, PackageFileByteArray);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Process 2
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        #endregion
    }
}
