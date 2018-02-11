using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class ucFileUpload : UserControl
    {
        #region Declarations

        private List<FileInfoDto> _multimediaFiles = null;
        /// <summary>
        /// Flag para no disparar el evento SelectionChanged al cargar la propiedad DataSource del grid dgvFiles.
        /// </summary>
        private bool _isLoadingData; 
        private string _fileName;
        private string _filePath;
        private Action _Action;
        private int _currentRowIndex;
        private string _fileDescription;
        private byte[] _byteArrayFile;
        private MultimediaFileBL _multimediaFileBL = new MultimediaFileBL();
        private string _multimediaFileId;
        private string _serviceComponentMultimediaId;
        #endregion
      
        #region Properties 

        public List<FileInfoDto> DataSource
        {
            get { return _multimediaFiles; }
            set
            {
                _multimediaFiles = value;
                _isLoadingData = false;
                dgvFiles.DataSource = value;
                _isLoadingData = true;
            }
           
        }

        /// <summary>
        /// Gets or Sets the value of the progress bar.
        /// </summary>
        //public int? Progress
        //{
        //    get
        //    {
        //        if (progressBar1.Style == ProgressBarStyle.Marquee)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return progressBar1.Value;
        //        }
        //    }

        //    set
        //    {
        //        if (value == null)
        //        {
        //            progressBar1.Style = ProgressBarStyle.Marquee;
        //            progressBar1.Value = 100;

        //            lblPercent.Visible = false;
        //        }
        //        else
        //        {
        //            progressBar1.Style = ProgressBarStyle.Continuous;
        //            progressBar1.Value = value.Value;

        //            lblPercent.Text = string.Format("{0}%", value);
        //            lblPercent.Visible = true;
        //        }
        //    }
        //}

        /// <summary>
        /// ID tabla person
        /// </summary>
        public string PersonId { get; set; }

        ///// <summary>
        ///// ID tabla multimediafile
        ///// </summary>
        //public string MultimediaFileId { get; set; }

        public string ServiceComponentId { get; set; }

        #endregion

        public ucFileUpload()
        {
            InitializeComponent();          
        }   

        private void LoadDataGridView()
        {
            OperationResult operationResult = new OperationResult();
            _multimediaFiles = _multimediaFileBL.GetMultimediaFiles(ref operationResult, ServiceComponentId);

            // Analizar el resultado de la operación
            if (operationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvFiles.DataSource = _multimediaFiles;

            // setear nro de archivos agregados
            lblRecordCount.Text = string.Format("{0} Archivo(s) agregado(s)", dgvFiles.Rows.Count);

            // Limpiar grilla
            if (dgvFiles.RowCount > 0)
                dgvFiles.Rows[0].Selected = true;

        }

        private void ucFileUpload_Load(object sender, EventArgs e)
        {
            dgvFiles.AutoGenerateColumns = false;

            LoadDataGridView();
            _multimediaFiles = new List<FileInfoDto>();
        }

        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            GetDataFromDataGridView();                  
        }

        private void GetDataFromDataGridView()
        {
            //if (!_isLoadingData) return;

            if (dgvFiles.SelectedRows.Count == 0)         
                return;

            ButtonsEnabled();

            // Setear variable global fila actual
            _currentRowIndex = dgvFiles.CurrentRow.Index;

            // Buscar Archivo y mostrar 
            _fileDescription = dgvFiles.SelectedRows[0].Cells["Description"].Value.ToString();
            _fileName = dgvFiles.SelectedRows[0].Cells["FileName"].Value.ToString();
            _byteArrayFile = (byte[])dgvFiles.SelectedRows[0].Cells["ThumbnailFile"].Value;
            _multimediaFileId = dgvFiles.SelectedRows[0].Cells["MultimediaFileId"].Value.ToString();
            _serviceComponentMultimediaId = dgvFiles.SelectedRows[0].Cells["ServiceComponentMultimediaId"].Value.ToString();

            if (_byteArrayFile != null)
                pbProductImage.Image = Sigesoft.Common.Utils.byteArrayToImage(_byteArrayFile);

        }     

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            frmFileUploadEdit frm = new frmFileUploadEdit();
            frm.PersonId = PersonId;
            frm.ServiceComponentId = ServiceComponentId;
            frm.Action = ActionForm.Add;
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;

            LoadDataGridView();


            //if (frm.FileName != null)
            //{
            //    if (CheckFileExistsInList(frm.FileName))
            //        return;
            //}

            //if (frm.FileEntity != null)
            //{
            //    var searchResult = _files.Find(p => p.FileName == frm.FileEntity.FileName);

            //    if (searchResult != null)   // se encontro una coincidencia
            //    {
            //        if (searchResult.Action == (int)ActionForm.Delete)
            //        {
            //            searchResult.Description = frm.FileEntity.Description;
            //            searchResult.ByteArrayFile = frm.FileEntity.ByteArrayFile;
            //            searchResult.Action = frm.FileEntity.Action;
            //        }
            //        else
            //        {
            //            MessageBox.Show("El archivo [" + searchResult.FileName + "] ya está agregado. corrija por favor.", "FileUpload::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
                  
            //    }
            //    else  // ok se agrega con normalidad
            //    {
            //        _files.Add(frm.FileEntity);
            //    }              

            //    // Cargar grilla
            //    LoadDataGridView();
                _isLoadingData = true;
            //    ButtonsEnabled();
            //}
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            frmFileUploadEdit frm = new frmFileUploadEdit();
            frm.Action = ActionForm.Edit;

            frm.FileEntity = new FileInfoDto { 
                FileName = _fileName, 
                Description = _fileDescription, 
                ThumbnailFile = _byteArrayFile,
                MultimediaFileId = _multimediaFileId,
                ServiceComponentMultimediaId = _serviceComponentMultimediaId
            };
           
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;

            LoadDataGridView();
           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvFiles.SelectedRows.Count > 0)
            {
                string _fileName = dgvFiles.SelectedRows[0].Cells["FileName"].Value.ToString();
                DialogResult rp = MessageBox.Show("¿ Desea realmente remover el archivo " + _fileName + " ?", "FileUpload::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rp == DialogResult.OK)
                {
                    OperationResult operationResult = new OperationResult();
                    _multimediaFileBL.DeleteMultimediaFileComponent(ref operationResult, _multimediaFileId, Globals.ClientSession.GetAsList());

                    // Analizar el resultado de la operación
                    if (operationResult.Success != 1)
                    {
                        MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ImageDisposing();
                    // Cargar grilla
                    LoadDataGridView();          
                    ButtonsEnabled();
                                  
                    // setear nro de archivos agregados
                    lblRecordCount.Text = string.Format("{0} Archivo(s) agregado(s)", dgvFiles.Rows.Count);
                }
            }
            else
            {
                MessageBox.Show("Debe de Seleccionar un registro.", "FileUpload::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }           

        private void ButtonsEnabled()
        {
            btnEliminar.Enabled = btnModificar.Enabled = btnDescargar.Enabled = dgvFiles.SelectedRows.Count > 0;
        }

        private void pbProductImage_Click(object sender, EventArgs e)
        {
            if (_byteArrayFile == null) return;

            var frm = new frmPreviewImagePerson(_byteArrayFile, _fileName);
            frm.ShowDialog();
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            OperationResult operationResult = new OperationResult();
            var multimediaFile = _multimediaFileBL.GetMultimediaFileById(ref operationResult, _multimediaFileId);

            // Analizar el resultado de la operación
            if (operationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #region Download file
            // 
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = multimediaFile.FileName;
                sfd.FileName = multimediaFile.FileName;

                DialogResult dialogResult = sfd.ShowDialog();
                
                if (dialogResult == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(sfd.FileName))
                    {
                        MessageBox.Show("Escriba un nombre para el archivo.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string path = sfd.FileName;
                    File.WriteAllBytes(path, multimediaFile.ByteArrayFile);

                }
                else
                {
                    //Inform the user
                }
            }

            #endregion

        }

        private void ImageDisposing()
        {
            if (pbProductImage.Image != null)
            {
                pbProductImage.Image.Dispose();
                pbProductImage.Image = null;

               
            }
        }     
       
    }
}
