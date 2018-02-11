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
    public partial class frmAddExamDiagnosticComponent : Form
    {
        #region Declarations
            
        string _mode = string.Empty;
        /// <summary>
        /// lista temporal (solo diagnosticos vinculados a examenes / componentes)
        /// </summary>
        public List<DiagnosticRepositoryList> _tmpExamDiagnosticComponentList = null;
        private DiagnosticRepositoryList _diagnosticRepository = null;
        public string _diagnosticId = null;
        /// <summary>
        /// PK de tabla Temporal para realizar una busqueda y saber que registro selecionar
        /// </summary>
        public string _diagnosticRepositoryId = null;
        private List<RestrictionList> _tmpRestrictionByDiagnosticList = null;
        private List<RecomendationList> _tmpRecomendationList = null;
        public string _componentId;
        public string _serviceId;
        private List<string> _componentIds;
        private List<KeyValueDTO> _componentListTemp = new List<KeyValueDTO>();

        string _selectedComponent = null;

        #endregion

        public frmAddExamDiagnosticComponent(string mode)
        {
            InitializeComponent();
            _mode = mode;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmAddExamDiagnosticComponent_Load(object sender, EventArgs e)
        {
            // Llenado de combos

            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cbPreCalificacion, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 137, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbAutoManual, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 136, null), DropDownListAction.Select);

            LoadComponentsByOffice();

            if (_mode == "New")
            {
                // Setear valores por defecto

                cbAutoManual.SelectedValue = ((int)AutoManual.Manual).ToString();
                cbPreCalificacion.SelectedValue = ((int)PreQualification.Aceptado).ToString();

                cbAutoManual.Enabled = false;
                cbPreCalificacion.Enabled = false;

            }
            else if (_mode == "Edit")
            {
                var findResult = _tmpExamDiagnosticComponentList.Find(p => p.v_DiagnosticRepositoryId == _diagnosticRepositoryId);
                
                // Cargar datos

                lblDiagnostico.Text = findResult.v_DiseasesName;
                cbAutoManual.SelectedValue = findResult.i_AutoManualId.ToString();
                cbPreCalificacion.SelectedValue = findResult.i_PreQualificationId.ToString();
                ddlComponentId.SelectedValue = findResult.v_ComponentId;
              
                cbAutoManual.Enabled = false;

                // Bloquear el boton de add DX cuando el DX k se va editar sea automático
                //if (findResult.i_AutoManualId == (int)AutoManual.Automático)
                    //btnAgregarDX.Enabled = false;

                // Refrescar mi lista temporal de restricciones [Todos los registros no importa su estado (eliminados, agregados,etc)]

                RefreshTmpRestrictionByDiagnosticList(findResult.Restrictions);
                RefreshTmpRecomendationList(findResult.Recomendations);

                // Cargar restricciones ya existentes si es k hubieran
                grdRestricciones.DataSource = new RestrictionList();

                // Mostrar en la grilla todos los registros que no se encuentran con el estado de [eliminados logicos]
                if (findResult.Restrictions != null && findResult.Restrictions.Count > 0)
                {
                    grdRestricciones.DataSource = findResult.Restrictions.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
                    grdRestricciones.Refresh();
                    lblRecordCountRestricciones.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionByDiagnosticList.Count());
                }
                else
                {
                    lblRecordCountRestricciones.Text = string.Format("Se encontraron {0} registros.", 0);
                }
                          
                // Cargar recomendaciones ya existentes si es k hubieran
                grdRecomendaciones.DataSource = new RecomendationList();
               
                // Mostrar en la grilla todos los registros que no se encuentran con el estado de [eliminados logicos]
                if (findResult.Recomendations != null && findResult.Recomendations.Count > 0)
                {
                    grdRecomendaciones.DataSource = findResult.Recomendations.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
                    grdRecomendaciones.Refresh();
                    lblRecordCountRecomendaciones.Text = string.Format("Se encontraron {0} registros.", _tmpRecomendationList.Count());
                }
                else
                {
                    lblRecordCountRecomendaciones.Text = string.Format("Se encontraron {0} registros.", 0);
                }
               

            }

            #region FormActions

            var formActions = Sigesoft.Node.WinClient.BLL.Utils.SetFormActionsInSession("frmEso",
                                                                                 Globals.ClientSession.i_CurrentExecutionNodeId,
                                                                                  Globals.ClientSession.i_RoleId.Value,
                                                                                 Globals.ClientSession.i_SystemUserId);
            // Setear privilegios / permisos

            btnRecomendaciones.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_ADDRECOME", formActions);
            btnRemoverRecomendacion.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_REMOVERECOME", formActions);
            btnAgregarRestriccion.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_ADDRESTRIC", formActions);
            btnRemoverRestriccion.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_REMOVERESTRIC", formActions);

            #endregion

        }

        private void RefreshTmpRestrictionByDiagnosticList(List<RestrictionList> prestrictions)
        {           
            _tmpRestrictionByDiagnosticList = new List<RestrictionList>();

            if (prestrictions == null) 
                return;

            foreach (var item in prestrictions)
            {
                // Agregar restricciones a la Lista mas lo que ya tiene
                RestrictionList restrictionByDiagnostic = new RestrictionList();

                restrictionByDiagnostic.v_RestrictionByDiagnosticId = item.v_RestrictionByDiagnosticId;
                restrictionByDiagnostic.v_ComponentId = item.v_ComponentId;
                restrictionByDiagnostic.v_DiagnosticRepositoryId = item.v_DiagnosticRepositoryId;
                restrictionByDiagnostic.v_MasterRestrictionId = item.v_MasterRestrictionId;
                restrictionByDiagnostic.v_RestrictionName = item.v_RestrictionName;
                restrictionByDiagnostic.i_RecordStatus = item.i_RecordStatus;
                restrictionByDiagnostic.i_RecordType = item.i_RecordType;
                restrictionByDiagnostic.v_ServiceId = item.v_ServiceId;

                _tmpRestrictionByDiagnosticList.Add(restrictionByDiagnostic);
            }
        }

        private void RefreshTmpRecomendationList(List<RecomendationList> precomendations)
        {
            _tmpRecomendationList = new List<RecomendationList>();

            if (precomendations == null)
                return;

            foreach (var item in precomendations)
            {
                // Agregar restricciones a la Lista mas lo que ya tiene
                RecomendationList recomendation = new RecomendationList();

                recomendation.v_RecommendationId = item.v_RecommendationId;
                recomendation.v_ComponentId = item.v_ComponentId;
                recomendation.v_DiagnosticRepositoryId = item.v_DiagnosticRepositoryId;
                recomendation.v_MasterRecommendationId = item.v_MasterRecommendationId;
                recomendation.v_RecommendationName = item.v_RecommendationName;
                recomendation.i_RecordStatus = item.i_RecordStatus;
                recomendation.i_RecordType = item.i_RecordType;
                recomendation.v_ServiceId = item.v_ServiceId;

                _tmpRecomendationList.Add(recomendation);
            }
        }

        private void btnAgregarRestriccion_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Restricciones",(int)Typifying.Restricciones, ModeOperation.Total);
            frm.ShowDialog();        

            if (_tmpRestrictionByDiagnosticList == null)
            {
                _tmpRestrictionByDiagnosticList = new List<RestrictionList>();
            }

            var restrictionId = frm._masterRecommendationRestricctionId;
            var restrictionName = frm._masterRecommendationRestricctionName;

            if (restrictionId != null && restrictionName != null)
            {
                var restriction = _tmpRestrictionByDiagnosticList.Find(p => p.v_MasterRestrictionId == restrictionId);

                if (_mode == "New" || _mode == "Edit")
                {
                    if (restriction == null)   // agregar con normalidad [insert]  a la bolsa  
                    {
                        // Agregar restricciones a la Lista
                        RestrictionList restrictionByDiagnostic = new RestrictionList();

                        restrictionByDiagnostic.v_RestrictionByDiagnosticId = Guid.NewGuid().ToString();
                        restrictionByDiagnostic.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                        restrictionByDiagnostic.v_MasterRestrictionId = restrictionId;
                        restrictionByDiagnostic.v_ServiceId = _serviceId;
                        restrictionByDiagnostic.v_ComponentId = ddlComponentId.SelectedValue.ToString(); // _componentId;
                        restrictionByDiagnostic.v_RestrictionName = restrictionName;
                        restrictionByDiagnostic.i_RecordStatus = (int)RecordStatus.Agregado;
                        restrictionByDiagnostic.i_RecordType = (int)RecordType.Temporal;

                        _tmpRestrictionByDiagnosticList.Add(restrictionByDiagnostic);
                    }
                    else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                    {
                        if (restriction.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                        {
                            if (restriction.i_RecordType == (int)RecordType.NoTemporal)   // El registro Tiene in ID de BD
                            {
                                restriction.v_MasterRestrictionId = restrictionId;
                                restriction.v_RestrictionName = restrictionName;
                                restriction.i_RecordStatus = (int)RecordStatus.Grabado;
                            }
                            else if (restriction.i_RecordType == (int)RecordType.Temporal)   // El registro tiene un ID temporal [GUID]
                            {
                                restriction.v_MasterRestrictionId = restrictionId;
                                restriction.v_RestrictionName = restrictionName;
                                restriction.i_RecordStatus = (int)RecordStatus.Agregado;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor seleccione otra Restriccón. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }


            var dataList = _tmpRestrictionByDiagnosticList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            // Cargar grilla
            grdRestricciones.DataSource = new RestrictionList();
            grdRestricciones.DataSource = dataList;
            grdRestricciones.Refresh();
            lblRecordCountRestricciones.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

        }

        private void btnRecomendaciones_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Recomendaciones", (int)Typifying.Recomendaciones, ModeOperation.Total);
            frm.ShowDialog(); 

            if (_tmpExamDiagnosticComponentList == null)
            {
                _tmpExamDiagnosticComponentList = new List<DiagnosticRepositoryList>();
            }

            if (_tmpRecomendationList == null)
            {
                _tmpRecomendationList = new List<RecomendationList>();
            }

            var recomendationId = frm._masterRecommendationRestricctionId;
            var recommendationName = frm._masterRecommendationRestricctionName;

            if (recomendationId != null && recommendationName != null)
            {
                var recomendation = _tmpRecomendationList.Find(p => p.v_MasterRecommendationId == recomendationId);

                if (_mode == "New" || _mode == "Edit")
                {
                    if (recomendation == null)   // agregar con normalidad [insert]  a la bolsa  
                    {
                        // Agregar restricciones a la Lista
                        RecomendationList recomendationList = new RecomendationList();

                        recomendationList.v_RecommendationId = Guid.NewGuid().ToString();
                        recomendationList.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                        recomendationList.v_MasterRecommendationId = recomendationId;
                        recomendationList.v_ServiceId = _serviceId;
                        recomendationList.v_ComponentId = ddlComponentId.SelectedValue.ToString(); //_componentId;
                        recomendationList.v_RecommendationName = recommendationName;
                        recomendationList.i_RecordStatus = (int)RecordStatus.Agregado;
                        recomendationList.i_RecordType = (int)RecordType.Temporal;

                        _tmpRecomendationList.Add(recomendationList);
                    }
                    else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                    {
                        if (recomendation.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                        {
                            if (recomendation.i_RecordType == (int)RecordType.NoTemporal)   // El registro Tiene in ID de BD
                            {
                                recomendation.v_MasterRecommendationId = recomendationId;
                                recomendation.v_RecommendationName = recommendationName;
                                recomendation.i_RecordStatus = (int)RecordStatus.Grabado;
                            }
                            else if (recomendation.i_RecordType == (int)RecordType.Temporal)   // El registro tiene un ID temporal [GUID]
                            {
                                recomendation.v_MasterRecommendationId = recomendationId;
                                recomendation.v_RecommendationName = recommendationName;
                                recomendation.i_RecordStatus = (int)RecordStatus.Agregado;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor seleccione otra Recomendación. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                }
            }

            var dataList = _tmpRecomendationList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            // Cargar grilla
            grdRecomendaciones.DataSource = new RecomendationList();
            grdRecomendaciones.DataSource = dataList;
            grdRecomendaciones.Refresh();
            lblRecordCountRecomendaciones.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (uvAddExamDiagnostic.Validate(true, false).IsValid)
            {
                if (_tmpExamDiagnosticComponentList == null)              
                    _tmpExamDiagnosticComponentList = new List<DiagnosticRepositoryList>();
              
                if (_mode == "New")
                {                   
                    var findResult = _tmpExamDiagnosticComponentList.Find(p => p.v_DiagnosticRepositoryId == _diagnosticRepositoryId);

                    _diagnosticRepository = new DiagnosticRepositoryList();

                    if (findResult == null)   // agregar con normalidad  a la bolsa de DX 
                    {
                        _diagnosticRepository.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                        _diagnosticRepository.v_DiseasesId = _diagnosticId;
                        _diagnosticRepository.i_AutoManualId = int.Parse(cbAutoManual.SelectedValue.ToString());
                        _diagnosticRepository.i_PreQualificationId = int.Parse(cbPreCalificacion.SelectedValue.ToString());
                        _diagnosticRepository.i_FinalQualificationId = (int)FinalQualification.SinCalificar;
                        _diagnosticRepository.v_ServiceId = _serviceId;
                        _diagnosticRepository.v_ComponentId = ddlComponentId.SelectedValue.ToString();  //_componentId;
                        _diagnosticRepository.v_DiseasesName = lblDiagnostico.Text;
                        _diagnosticRepository.v_AutoManualName = cbAutoManual.Text;
                        _diagnosticRepository.v_RestrictionsName = ConcatenateRestrictions();
                        _diagnosticRepository.v_RecomendationsName = ConcatenateRecommendations();
                        _diagnosticRepository.v_PreQualificationName = cbPreCalificacion.Text;
                        _diagnosticRepository.i_RecordStatus = (int)RecordStatus.Agregado;
                        _diagnosticRepository.i_RecordType = (int)RecordType.Temporal;
                        _diagnosticRepository.Restrictions = _tmpRestrictionByDiagnosticList;
                        _diagnosticRepository.Recomendations = _tmpRecomendationList;
                        
                        _tmpExamDiagnosticComponentList.Add(_diagnosticRepository);
                    }                   
                }
                else if (_mode == "Edit")
                {
                    var findResult = _tmpExamDiagnosticComponentList.Find(p => p.v_DiagnosticRepositoryId == _diagnosticRepositoryId);

                    findResult.v_DiseasesId = _diagnosticId == null ? findResult.v_DiseasesId : _diagnosticId;
                    findResult.i_AutoManualId = int.Parse(cbAutoManual.SelectedValue.ToString());
                    findResult.i_PreQualificationId = int.Parse(cbPreCalificacion.SelectedValue.ToString());
                    findResult.i_FinalQualificationId = (int)FinalQualification.SinCalificar;
                    findResult.v_DiseasesName = lblDiagnostico.Text;
                    findResult.v_AutoManualName = cbAutoManual.Text;
                    findResult.v_RestrictionsName = ConcatenateRestrictions();
                    findResult.v_RecomendationsName = ConcatenateRecommendations();
                    findResult.v_PreQualificationName = cbPreCalificacion.Text;
                    findResult.i_RecordStatus = (int)RecordStatus.Modificado;
                    findResult.Restrictions = _tmpRestrictionByDiagnosticList;
                    findResult.Recomendations = _tmpRecomendationList;
                    findResult.v_ServiceId = _serviceId;

                    findResult.v_ComponentId = ddlComponentId.SelectedValue.ToString(); 
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
        }

        private void btnRemoverRestriccion_Click(object sender, EventArgs e)
        {
            if (grdRestricciones.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Delete the item

                // Capturar id desde la grilla de restricciones
                var restrictionByDiagnosticId = grdRestricciones.Selected.Rows[0].Cells["v_RestrictionByDiagnosticId"].Value.ToString();

                // Buscar registro para remover
                var findResult = _tmpRestrictionByDiagnosticList.Find(p => p.v_RestrictionByDiagnosticId == restrictionByDiagnosticId);
                // Borrado logico
                findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;

                var dataList = _tmpRestrictionByDiagnosticList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                grdRestricciones.DataSource = new RestrictionList();
                grdRestricciones.DataSource = dataList;
                grdRestricciones.Refresh();
                lblRecordCountRestricciones.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionByDiagnosticList.Count());
            }
        }

        private void btnRemoverRecomendacion_Click(object sender, EventArgs e)
        {
            if (grdRecomendaciones.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (Result == DialogResult.Yes)
            {
                // Delete the item

                // Capturar id desde la grilla de restricciones
                var recomendationId = grdRecomendaciones.Selected.Rows[0].Cells["v_RecommendationId"].Value.ToString();

                // Buscar registro para remover
                var findResult = _tmpRecomendationList.Find(p => p.v_RecommendationId == recomendationId);

                findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;

                var dataList = _tmpRecomendationList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                grdRecomendaciones.DataSource = new RecomendationList();
                grdRecomendaciones.DataSource = dataList;
                grdRecomendaciones.Refresh();
                lblRecordCountRecomendaciones.Text = string.Format("Se encontraron {0} registros.", _tmpRecomendationList.Count());
            }

        }

        private void btnAgregarDX_Click(object sender, EventArgs e)
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

        private string ConcatenateRestrictions()
        {
            if (_tmpRestrictionByDiagnosticList == null) 
                return string.Empty;

            var qry = (from a in _tmpRestrictionByDiagnosticList  // RESTRICCIONES POR Diagnosticos                                           
                       where a.i_RecordStatus != (int)RecordStatus.EliminadoLogico
                       select new
                       {
                           v_RestrictionsName = a.v_RestrictionName
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RestrictionsName));
        }

        private string ConcatenateRecommendations()
        {
            if (_tmpRecomendationList == null)
                return string.Empty;

            var qry = (from a in _tmpRecomendationList  // RESTRICCIONES POR Diagnosticos                                           
                       where a.i_RecordStatus != (int)RecordStatus.EliminadoLogico
                       select new
                       {
                           v_RecommendationName = a.v_RecommendationName
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RecommendationName));
        }

        private void LoadComponentsByOffice()
        {
            var arrayComponentsId = _componentId.Split('|') ;
       
            OperationResult objOperationResult = new OperationResult();

            _componentListTemp = BLL.Utils.GetAllComponentsByRoleNodeId(Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_RoleId.Value);         
           
            var results = _componentListTemp.FindAll(f => arrayComponentsId.Contains(f.Value2));

            List<KeyValueDTO> kvdtoList = new List<KeyValueDTO>();
            foreach (var item in results)
            {
                kvdtoList.Add(new KeyValueDTO { Id = item.Value2 , Value1 = item.Value3 });
            }

            Utils.LoadDropDownList(ddlComponentId, "Value1", "Id", kvdtoList, DropDownListAction.Select);
         
        }      
       
    }
}
