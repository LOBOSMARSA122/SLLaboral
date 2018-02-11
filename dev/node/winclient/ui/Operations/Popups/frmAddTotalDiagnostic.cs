using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.Operations.Popups
{
    public partial class frmAddTotalDiagnostic : Form
    {
        #region Declarations

        private ServiceBL _serviceBL = new ServiceBL();
        string _mode = string.Empty;
        /// <summary>
        ///  Almacena Temporalmente la lista de los diagnósticos totales en el form [frmAddTotalDiagnostic]
        /// </summary>      
        public List<DiagnosticRepositoryList> _tmpTotalDiagnosticList = null;
        public string _diagnosticId = null;
        /// <summary>
        /// PK de tabla Temporal para realizar una busqueda y saber que registro selecionar
        /// </summary>
        public string _diagnosticRepositoryId = null;
        private List<RestrictionList> _tmpRestrictionList = null;
        private List<RecomendationList> _tmpRecomendationList = null;
        public string _componentId;
        public string _serviceId;
        private List<KeyValueDTO> _componentListTemp = new List<KeyValueDTO>();
        private List<string> _componentIds;

        #endregion

        public frmAddTotalDiagnostic()
        {
            InitializeComponent();
        }

        private void frmAddTotalDiagnostic_Load(object sender, EventArgs e)
        {
            // Llenado de combos

            OperationResult objOperationResult = new OperationResult();

            Utils.LoadDropDownList(cbCalificacionFinal, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 138, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbTipoDx, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 139, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbEnviarAntecedentes, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.Select);
            LoadOffice();
            // Setear valor x default 

            cbCalificacionFinal.SelectedValue = ((int)FinalQualification.Presuntivo).ToString();
            cbTipoDx.SelectedValue = ((int)TipoDx.Otros).ToString();
            cbEnviarAntecedentes.SelectedValue = ((int)SiNo.NO).ToString();

            if (_mode == "New")
            {
                // Setear valores por defecto             

            }
            else if (_mode == "Edit")
            {
               
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (uvAddTotalDiagnostic.Validate(true, false).IsValid)
            {
                OperationResult objOperationResult = new OperationResult();

                if (_tmpTotalDiagnosticList == null)             
                    _tmpTotalDiagnosticList = new List<DiagnosticRepositoryList>();
               
                DiagnosticRepositoryList diagnosticRepository = new DiagnosticRepositoryList();

                diagnosticRepository.v_ServiceId = _serviceId;
                diagnosticRepository.v_DiseasesId = _diagnosticId;
                diagnosticRepository.i_AutoManualId  = (int?)AutoManual.Manual;             
                diagnosticRepository.i_PreQualificationId = (int?)PreQualification.Aceptado;
                diagnosticRepository.i_FinalQualificationId = int.Parse(cbCalificacionFinal.SelectedValue.ToString());
                diagnosticRepository.i_DiagnosticTypeId = int.Parse(cbTipoDx.SelectedValue.ToString());
                diagnosticRepository.i_IsSentToAntecedent = int.Parse(cbEnviarAntecedentes.SelectedValue.ToString());
                diagnosticRepository.d_ExpirationDateDiagnostic = dtpFechaVcto.Checked ? dtpFechaVcto.Value.Date : (DateTime?)null;
                diagnosticRepository.v_ComponentId = _componentIds[0];

                diagnosticRepository.i_RecordStatus = (int)RecordStatus.Agregado;
                diagnosticRepository.i_RecordType = (int)RecordType.Temporal;
                diagnosticRepository.Restrictions = _tmpRestrictionList;
                diagnosticRepository.Recomendations = _tmpRecomendationList;

                _tmpTotalDiagnosticList.Add(diagnosticRepository);

                // Grabar DX + restricciones / recomendaciones
                _serviceBL.AddDiagnosticRepository(ref objOperationResult,
                                                    _tmpTotalDiagnosticList,
                                                    null,
                                                    Globals.ClientSession.GetAsList(),
                                                    null);

                // Analizar el resultado de la operación
                if (objOperationResult.Success == 1)  // Operación sin error
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else  // Operación con error
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Se queda en el formulario.
                }

            }
            else
            {
                MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAgregarDx_Click(object sender, EventArgs e)
        {
            DiseasesList returnDiseasesList = new DiseasesList();
            frmDiseases frm = new frmDiseases();

            frm.ShowDialog();
            returnDiseasesList = frm._objDiseasesList;

            if (returnDiseasesList.v_DiseasesId != null)
            {
                lblDiagnostico.Text = returnDiseasesList.v_Name + " / " + returnDiseasesList.v_CIE10Id;
                _diagnosticId = returnDiseasesList.v_DiseasesId;
            }
        }

        private void btnAgregarRestriccion_Click(object sender, EventArgs e)
        {
            var x = (KeyValueDTO)ddlComponentId.SelectedItem;
           

            if (x.Value4== 0.0)
            {
                MessageBox.Show("Por favor seleccione un consultorio", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var frm = new frmMasterRecommendationRestricction("Restricciones",(int)Typifying.Restricciones, ModeOperation.Total);
            frm.ShowDialog();   

            if (_tmpRestrictionList == null)
            {
                _tmpRestrictionList = new List<RestrictionList>();
            }

            var restrictionId = frm._masterRecommendationRestricctionId;
            var restrictionName = frm._masterRecommendationRestricctionName;

            if (restrictionId != null && restrictionName != null)
            {
                var restriction = _tmpRestrictionList.Find(p => p.v_MasterRestrictionId == restrictionId);

                if (restriction == null)   // agregar con normalidad [insert]  a la bolsa  
                {
                    // Agregar restricciones a la Lista
                    RestrictionList restrictionByDiagnostic = new RestrictionList();

                    restrictionByDiagnostic.v_RestrictionByDiagnosticId = Guid.NewGuid().ToString();
                    restrictionByDiagnostic.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                    restrictionByDiagnostic.v_MasterRestrictionId = restrictionId;
                    restrictionByDiagnostic.v_ServiceId = _serviceId;
                    restrictionByDiagnostic.v_ComponentId = _componentId;
                    restrictionByDiagnostic.v_RestrictionName = restrictionName;
                    restrictionByDiagnostic.i_RecordStatus = (int)RecordStatus.Agregado;
                    restrictionByDiagnostic.i_RecordType = (int)RecordType.Temporal;
                    restrictionByDiagnostic.v_ComponentId = _componentIds[0];

                    _tmpRestrictionList.Add(restrictionByDiagnostic);

                }
                else    // La restriccion ya esta agregado en la bolsa 
                {
                    MessageBox.Show("Por favor seleccione otra Restriccón. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Cargar grilla
            grdRestricciones.DataSource = new RestrictionList();
            grdRestricciones.DataSource = _tmpRestrictionList;
            grdRestricciones.Refresh();
            lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionList.Count());

        }

        private void btnAgregarRecomendaciones_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Recomendaciones" ,(int)Typifying.Recomendaciones, ModeOperation.Total);
            frm.ShowDialog(); 
           
            if (_tmpRecomendationList == null)
            {
                _tmpRecomendationList = new List<RecomendationList>();
            }

            var recomendationId = frm._masterRecommendationRestricctionId;
            var recommendationName = frm._masterRecommendationRestricctionName;

            if (recomendationId != null && recommendationName != null)
            {
                var recomendation = _tmpRecomendationList.Find(p => p.v_RecommendationId == recomendationId);

                if (recomendation == null)   // agregar con normalidad [insert]  a la bolsa  
                {
                    // Agregar restricciones a la Lista
                    RecomendationList recomendationList = new RecomendationList();

                    recomendationList.v_RecommendationId = Guid.NewGuid().ToString();
                    recomendationList.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                    recomendationList.v_MasterRecommendationId = recomendationId;
                    recomendationList.v_ServiceId = _serviceId;
                    recomendationList.v_ComponentId = _componentId;
                    recomendationList.v_RecommendationName = recommendationName;
                    recomendationList.i_RecordStatus = (int)RecordStatus.Agregado;
                    recomendationList.i_RecordType = (int)RecordType.Temporal;
                    recomendationList.v_ComponentId = _componentIds[0];

                    _tmpRecomendationList.Add(recomendationList);

                }
                else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                {
                    MessageBox.Show("Por favor seleccione otra Recomendación. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }
            }

            // Cargar grilla
            grdRecomendaciones.DataSource = new RecomendationList();
            grdRecomendaciones.DataSource = _tmpRecomendationList;
            grdRecomendaciones.Refresh();
            lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRecomendationList.Count());

        }

        private void btnRemoverRestriccion_Click(object sender, EventArgs e)
        {
            if (grdRestricciones.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Capturar id desde la grilla de restricciones
            var restrictionId = grdRestricciones.Selected.Rows[0].Cells["v_MasterRestrictionId"].Value.ToString();

            // Buscar registro para remover
            var findResult = _tmpRestrictionList.Find(p => p.v_MasterRestrictionId == restrictionId);
            // Borrado logico
            _tmpRestrictionList.Remove(findResult);
                
            grdRestricciones.DataSource = new RestrictionList();
            grdRestricciones.DataSource = _tmpRestrictionList;
            grdRestricciones.Refresh();
            lblRecordCountRestricciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionList.Count());         

        }

        private void btnRemoverRecomendacion_Click(object sender, EventArgs e)
        {
            if (grdRecomendaciones.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Capturar id desde la grilla de restricciones
            var recommendationId = grdRecomendaciones.Selected.Rows[0].Cells["v_MasterRecommendationId"].Value.ToString();

            // Buscar registro para remover
            var findResult = _tmpRecomendationList.Find(p => p.v_MasterRecommendationId == recommendationId);
            // Borrado logico
            _tmpRecomendationList.Remove(findResult);

            grdRecomendaciones.DataSource = new RecomendationList();
            grdRecomendaciones.DataSource = _tmpRecomendationList;
            grdRecomendaciones.Refresh();
            lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRecomendationList.Count());         

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void LoadOffice()
        {

            // Obtener permisos de cada examen de un rol especifico
            var componentProfile = _serviceBL.GetRoleNodeComponentProfileByRoleNodeId(Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_RoleId.Value);

            //*********************************************

            ddlComponentId.SelectedValueChanged -= ddlComponentId_SelectedValueChanged;

            OperationResult objOperationResult = new OperationResult();

            _componentListTemp = BLL.Utils.GetAllComponentsByRoleNodeId(Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_RoleId.Value);
            //_componentListTemp = _componentListTemp.FindAll(f => componentProfile.Any(t => t.v_ComponentId == f.Value2));

            var xxx = _componentListTemp.FindAll(p => p.Value4 != -1);

            List<KeyValueDTO> groupComponentList = xxx.GroupBy(x => x.Value4).Select(group => group.First()).ToList();

            groupComponentList.AddRange(_componentListTemp.ToList().FindAll(p => p.Value4 == -1));

            // Remover los componentes que no estan asignados al rol del usuario
            //var results = groupComponentList.FindAll(f => componentProfile.Any(t => t.v_ComponentId == f.Value2));
            //var dd = groupComponentList.FindAll(p => componentProfile.FindAll(o => o.v_ComponentId == p.Value2));

            Utils.LoadDropDownList(ddlComponentId, "Value1", "Id", groupComponentList, DropDownListAction.Select);

            ddlComponentId.SelectedValueChanged += ddlComponentId_SelectedValueChanged;
        }

        private void ddlComponentId_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ddlComponentId.SelectedIndex == 0)                     
                return;
                   
            _componentIds = new List<string>();
            var eee = (KeyValueDTO)ddlComponentId.SelectedItem;

            if (eee.Value4.ToString() == "-1")
            {
                _componentIds.Add(eee.Value2);
            }
            else
            {
                _componentIds = _componentListTemp.FindAll(p => p.Value4 == eee.Value4)
                                                .Select(s => s.Value2)
                                                .OrderBy(p => p).ToList();
            }

        }

    }
}
