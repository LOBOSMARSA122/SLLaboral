using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.BE;
using System.IO;
using NetPdf;
using Infragistics.Win.UltraWinGrid;

//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.draw;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmService : Form
    {
        List<string> _componentIds;
        PacientBL _pacientBL = new PacientBL();
        HistoryBL _historyBL = new HistoryBL();
        List<KeyValueDTO> _componentListTemp = new List<KeyValueDTO>();
        string strFilterExpression;
        ServiceBL _serviceBL = new ServiceBL();
        private string _serviceId;
        private string _pacientId;
        private string _protocolId;
        private string _customerOrganizationName;
        private string _personFullName;
        List<string> _ListaServicios;
        private SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        private SaveFileDialog saveFileDialog2 = new SaveFileDialog();

        public frmService()
        {
            InitializeComponent();
        }

        private void frmService_Load(object sender, EventArgs e)
        {
            #region Simular sesion
            //ClientSession objClientSession = new ClientSession();
            //objClientSession.i_SystemUserId = 1;
            //objClientSession.v_UserName = "sa";
            //objClientSession.i_CurrentExecutionNodeId = 9;
            //objClientSession.v_CurrentExecutionNodeName = "SALUS";
            ////_ClientSession.i_CurrentOrganizationId = 57;
            //objClientSession.v_PersonId = "N000-P0000000001";

            //// Pasar el objeto de sesión al gestor de objetos globales
            //Globals.ClientSession = objClientSession;
            #endregion     

            ddlConsultorio.SelectedValueChanged -= ddlConsultorio_SelectedValueChanged;


            dtpDateTimeStar.Value = dtpDateTimeStar.Value.AddDays(-1);

            OperationResult objOperationResult = new OperationResult();

            Utils.LoadDropDownList(ddlServiceTypeId, "Value1", "Id", BLL.Utils.GetServiceType(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, -1, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);

            Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);

            var clientOrganization = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId);

            Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);
            Utils.LoadDropDownList(ddServiceStatusId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 125, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlStatusAptitudId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 124, null), DropDownListAction.All);

            Utils.LoadDropDownList(ddlProtocolId, "Value1", "Id", BLL.Utils.GetProtocolsByOrganizationForCombo(ref objOperationResult, "-1", "-1", null), DropDownListAction.All);


            // Obtener permisos de cada examen de un rol especifico
            var componentProfile = _serviceBL.GetRoleNodeComponentProfileByRoleNodeId(Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_RoleId.Value);
            
            _componentListTemp = BLL.Utils.GetAllComponents(ref objOperationResult);

            var xxx = _componentListTemp.FindAll(p => p.Value4 != -1);

            List<KeyValueDTO> groupComponentList = xxx.GroupBy(x => x.Value4).Select(group => group.First()).ToList();

             groupComponentList.AddRange(_componentListTemp.ToList().FindAll(p => p.Value4 == -1));
             // Remover los componentes que no estan asignados al rol del usuario
             var results = groupComponentList.FindAll(f => componentProfile.Any(t => t.v_ComponentId == f.Value2));
         

          
            Utils.LoadDropDownList(ddlConsultorio, "Value1", "Id", results, DropDownListAction.Select);

            dtpDateTimeStar.CustomFormat = "dd/MM/yyyy";
            dptDateTimeEnd.CustomFormat = "dd/MM/yyyy";
            // Establecer el filtro inicial para los datos
            strFilterExpression = null;

            ddlConsultorio.SelectedValueChanged += ddlConsultorio_SelectedValueChanged;
     
        }

        private void ddlMasterServiceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            if (ddlMasterServiceId.SelectedValue !=null )
            {
                var clientOrganization = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId);

                if (ddlMasterServiceId.SelectedValue.ToString() == ((int)Common.MasterService.Eso).ToString())
                {
                    ddlEsoType.Enabled = true;
                    ddlProtocolId.Enabled = true;
                    Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);
          
                    //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);

                }
                else
                {
                    Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);
          
                    //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);
                    ddlEsoType.Enabled = false;
                    ddlProtocolId.Enabled = false;
                }
            }

        }

        private void ddlEsoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //OperationResult objOperationResult = new OperationResult();
            //string idOrg = String.Empty;
            //string idLoc = String.Empty;
            //if (ddlEsoType.SelectedValue !=null)
            //{
            //    if (ddlEsoType.SelectedValue.ToString() != "-1")
            //    {
            //        if (ddlCustomerOrganization.SelectedValue.ToString() == "-1") return;
            //        var dataList = ddlCustomerOrganization.SelectedValue.ToString().Split('|');
            //        idOrg = dataList[1];
            //        idLoc = dataList[2];
            //    }
            //    Utils.LoadDropDownList(ddlProtocolId, "Value1", "Id", BLL.Utils.GetProtocolByLocation(ref objOperationResult, idLoc, int.Parse(ddlEsoType.SelectedValue.ToString())), DropDownListAction.All);
       
            //}
          
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Get the filters from the UI
            List<string> Filters = new List<string>();
            if (!string.IsNullOrEmpty(txtPacient.Text)) Filters.Add("v_PacientDocument.Contains(\"" + txtPacient.Text.Trim() + "\")");
            if (ddServiceStatusId.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceStatusId==" + ddServiceStatusId.SelectedValue);

            var id1 = ddlCustomerOrganization.SelectedValue.ToString().Split('|');

            if (ddlCustomerOrganization.SelectedValue.ToString() != "-1")
            {
                var id3 = ddlCustomerOrganization.SelectedValue.ToString().Split('|');
                Filters.Add("v_CustomerOrganizationId==" + "\"" + id3[0] + "\"&&v_CustomerLocationId==" + "\"" + id3[1] + "\"");
            }

            //if (ddlCustomerOrganization.SelectedValue.ToString() != "-1") Filters.Add("v_LocationId==" + "\"" + id1[2] + "\"");

            if (ddlMasterServiceId.SelectedValue.ToString() != "-1") Filters.Add("i_MasterServiceId==" + ddlMasterServiceId.SelectedValue);
            if (ddlServiceTypeId.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceTypeId==" + ddlServiceTypeId.SelectedValue);
            if (ddlEsoType.SelectedValue.ToString() != "-1") Filters.Add("i_EsoTypeId==" + ddlEsoType.SelectedValue);
            if (ddlProtocolId.SelectedValue.ToString() != "-1") Filters.Add("v_ProtocolId=="+ "\"" + ddlProtocolId.SelectedValue + "\"");
            if (ddlStatusAptitudId.SelectedValue.ToString() != "-1") Filters.Add("i_AptitudeStatusId==" + ddlStatusAptitudId.SelectedValue);
            
            // Create the Filter Expression
            strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            this.BindGrid();
        }

        private void BindGrid()
        {
            var objData = GetData(0, null, "", strFilterExpression);

            grdDataService.DataSource = objData;
            lblRecordCountCalendar.Text = string.Format("Se encontraron {0} registros.", objData.Count());

            if (grdDataService.Rows.Count > 0)
                grdDataService.Rows[0].Selected = true;
        }

        private List<ServiceList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);

            var _objData = _serviceBL.GetServicesPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate, _componentIds);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void ddlOrganizationLocationId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomerOrganization.SelectedValue == null)
                return;

            if (ddlCustomerOrganization.SelectedValue.ToString() == "-1")
            {
                ddlProtocolId.SelectedValue = "-1";
                ddlProtocolId.Enabled = false;
                return;
            }

            ddlProtocolId.Enabled = true;

            OperationResult objOperationResult = new OperationResult();
                
            var id3 = ddlCustomerOrganization.SelectedValue.ToString().Split('|');

            //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlProtocolId, "Value1", "Id", BLL.Utils.GetProtocolsByOrganizationForCombo(ref objOperationResult, id3[0], id3[1], null), DropDownListAction.All);          
            
        }

        private void ddlServiceTypeId_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (ddlServiceTypeId.SelectedValue.ToString() == "-1")
            {
                ddlMasterServiceId.SelectedValue = "-1";
                ddlMasterServiceId.Enabled = false;
                return;
            }

            ddlMasterServiceId.Enabled = true;
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, int.Parse(ddlServiceTypeId.SelectedValue.ToString()), Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
          
        }

        private void ddlMasterServiceId_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (ddlMasterServiceId.SelectedValue == null)
                return;

            if (ddlMasterServiceId.SelectedValue.ToString() == "-1")
            {
                ddlEsoType.SelectedValue = "-1";
                ddlEsoType.Enabled = false;
                return;
            }

            OperationResult objOperationResult = new OperationResult();
          
            //var clientOrganization = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId);

            if (ddlMasterServiceId.SelectedValue.ToString() == ((int)Common.MasterService.Eso).ToString() ||
                ddlMasterServiceId.SelectedValue.ToString() == "12")
            {

                ddlEsoType.Enabled = true;

                //ddlProtocolId.Enabled = true;
                ddlStatusAptitudId.Enabled = true;
                //Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);

                //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);

            }
            else
            {

                ddlEsoType.SelectedValue = "-1";
                ddlEsoType.Enabled = false;

                //Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);

                ////Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);
                
                //ddlProtocolId.SelectedValue = "-1";
                //ddlProtocolId.Enabled = false;
                ddlStatusAptitudId.Enabled = false;
                ddlStatusAptitudId.SelectedValue = "-1";
            }
            
        }

        private void CertificadoAptitud_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOccupationalMedicalAptitudeCertificate(_serviceId);
            frm.ShowDialog();
        }

        private void btnEditarESO_Click(object sender, EventArgs e)
        {
            Form frm;
           int TserviceId = int.Parse(grdDataService.Selected.Rows[0].Cells["i_ServiceId"].Value.ToString());
           if (TserviceId == (int)MasterService.AtxMedicaParticular)
           {
               frm = new Operations.frmMedicalConsult(_serviceId, null, null);
               frm.ShowDialog();
           }
           else
           {
               this.Enabled = false;
               frm = new Operations.frmEso(_serviceId, null, "Service");
                frm.ShowDialog();
                this.Enabled = true;
           }
                  
           
        }

        private void grdDataService_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            btn7D.Enabled = 
            btnOdontograma.Enabled =
            btnHistoriaOcupacional.Enabled = 
            btnRadiologico.Enabled =
            btnOsteomuscular.Enabled = 
            btnPruebaEsfuerzo.Enabled = 
            btnInformeRadiologicoOIT.Enabled = 
            btnEstudioEKG.Enabled =
            btnDermatologico.Enabled = 
            btnEditarESO.Enabled = 
            btnImprimirCertificadoAptitud.Enabled = 
            btnInformeMedicoTrabajador.Enabled =
            btnImprimirInformeMedicoEPS.Enabled = 
            btnAdminReportes.Enabled = 
            btnInforme312.Enabled = 
            btnInformeMusculoEsqueletico.Enabled = 
            btnInformeAlturaEstructural.Enabled = 
            btnInformePsicologico.Enabled = 
            btnInformeOftalmo.Enabled = 
            btnGenerarLiquidacion.Enabled =
            btnInterconsulta.Enabled=
            btnTiempos.Enabled=
            //btnImprimirFichaOcupacional.Enabled = 
            (grdDataService.Selected.Rows.Count > 0);
       
            if (grdDataService.Selected.Rows.Count == 0)
                return;       

            _serviceId = grdDataService.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _pacientId = grdDataService.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();
            _protocolId = grdDataService.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString();
            _customerOrganizationName = grdDataService.Selected.Rows[0].Cells["v_OrganizationName"].Value.ToString();
            _personFullName = grdDataService.Selected.Rows[0].Cells["v_Pacient"].Value.ToString();
           
        }

        private void Examenes_Click(object sender, EventArgs e)
        {                        
            ReportCustom.FichaOcupacional frm = new ReportCustom.FichaOcupacional(_serviceId, _pacientId, _protocolId, (int)TypePrinter.Printer);
        }

        private void btnImprimirCertificadoAptitud_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOccupationalMedicalAptitudeCertificate(_serviceId);
            frm.ShowDialog();
        }

        private void btnImprimirFichaOcupacional_Click(object sender, EventArgs e)
        {

        }

        private void vistaPreviaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _serviceId = grdDataService.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _pacientId = grdDataService.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();
            _protocolId = grdDataService.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString();
            ReportCustom.FichaOcupacional frm = new ReportCustom.FichaOcupacional(_serviceId, _pacientId, _protocolId, (int)TypePrinter.Image);
        }

        private void dtpDateTimeStar_Validating(object sender, CancelEventArgs e)
        {
            if (dtpDateTimeStar.Value.Date > dptDateTimeEnd.Value.Date)
            {
                e.Cancel = true;
                MessageBox.Show("El campo fecha Inicial no puede ser Mayor a la fecha final.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void dptDateTimeEnd_Validating(object sender, CancelEventArgs e)
        {
            if (dptDateTimeEnd.Value.Date < dtpDateTimeStar.Value.Date)
            {
                e.Cancel = true;
                MessageBox.Show("El campo fecha Final no puede ser Menor a la fecha Inicial.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void txtPacient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFilter_Click(null, null);
            }
        }

        private void btnImprimirInformeMedicoEPS_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Informe Médico EPS";
            saveFileDialog1.Filter = "Files (*.pdf;)|*.pdf;";

            try
            {
                
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                   
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        this.Enabled = false;                    

                        var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);

                        var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(_pacientId);

                        var noxiousHabit = _historyBL.GetNoxiousHabitsReport(_pacientId);

                        var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);

                        var anamnesis = _serviceBL.GetAnamnesisReport(_serviceId);

                        var serviceComponents = _serviceBL.GetServiceComponentsReport(_serviceId);

                        var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(_serviceId);

                        ReportPDF.CreateMedicalReportEPS(filiationData,
                                                        personMedicalHistory,
                                                        noxiousHabit,
                                                        familyMedicalAntecedent,
                                                        anamnesis,
                                                        serviceComponents,
                                                        diagnosticRepository,
                                                        saveFileDialog1.FileName);

                        this.Enabled = true;
                    }

                  

                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }         
   
        }

        private void btnInformeMedicoTrabajador_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = string.Format("{0} Informe Resumen", _personFullName);
            saveFileDialog1.Filter = "Files (*.pdf;)|*.pdf;";

            //try
            //{

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {               
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        this.Enabled = false;
                      
                        var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);

                        var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(_pacientId);

                        var noxiousHabit = _historyBL.GetNoxiousHabitsReport(_pacientId);

                        var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);

                        var anamnesis = _serviceBL.GetAnamnesisReport(_serviceId);

                        var serviceComponents = _serviceBL.GetServiceComponentsReport(_serviceId);

                        var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(_serviceId);

                        var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

                        ReportPDF.CreateMedicalReportForTheWorker(filiationData,
                                                        personMedicalHistory,
                                                        noxiousHabit,
                                                        familyMedicalAntecedent,
                                                        anamnesis,
                                                        serviceComponents,
                                                        diagnosticRepository,
                                                        _customerOrganizationName,
                                                        MedicalCenter,                                                      
                                                        saveFileDialog1.FileName);

                        this.Enabled = true;
                    }
                                  
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Enabled = true;
            //}              
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var inside = _serviceBL.IsPsicoExamIntoServiceComponent(_serviceId);

            if (!inside)
            {
                MessageBox.Show("El examen de Psicologia no aplica a la atención", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var frm = new Reports.frmInformePsicologicoOcupacional(_serviceId);
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog2.FileName = string.Format("{0} 312", _personFullName);         
            saveFileDialog2.Filter = "Files (*.pdf;)|*.pdf;";

            //try
            //{
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        this.Enabled = false;

                        var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);
                        var _listAtecedentesOcupacionales = _historyBL.GetHistoryReport(_pacientId);
                        var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);
                        var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(_pacientId);
                        var _DataService = _serviceBL.GetServiceReport(_serviceId);
                        var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(_pacientId);

                        var Antropometria = _serviceBL.ValoresComponente(_serviceId, Constants.ANTROPOMETRIA_ID);
                        var FuncionesVitales = _serviceBL.ValoresComponente(_serviceId, Constants.FUNCIONES_VITALES_ID);
                        var ExamenFisico = _serviceBL.ValoresComponente(_serviceId, Constants.EXAMEN_FISICO_ID);
                        var Oftalmologia = _serviceBL.ValoresComponente(_serviceId, Constants.OFTALMOLOGIA_ID);
                        var Psicologia = _serviceBL.ValoresExamenComponete(_serviceId, Constants.PSICOLOGIA_ID, 195);
                        var RX = _serviceBL.ValoresExamenComponete(_serviceId, Constants.RX_TORAX_ID, 211);
                        var RX1 = _serviceBL.ValoresExamenComponete(_serviceId, Constants.RX_TORAX_ID, 135);
                        var Laboratorio = _serviceBL.ValoresComponente(_serviceId, Constants.LABORATORIO_ID);
                        //var Audiometria = _serviceBL.ValoresComponente(_serviceId, Constants.AUDIOMETRIA_ID);
                        var Audiometria = _serviceBL.GetDiagnosticForAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);
                        var Espirometria = _serviceBL.ValoresExamenComponete(_serviceId, Constants.ESPIROMETRIA_ID, 210);
                        var _DiagnosticRepository = _serviceBL.GetServiceDisgnosticsReports(_serviceId);
                        var _Recomendation = _serviceBL.GetServiceRecommendationByServiceId(_serviceId);
                        var _ExamenesServicio = _serviceBL.GetServiceComponentsReport(_serviceId);

                        var ElectroCardiograma = _serviceBL.ValoresComponente(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);
                        var PruebaEsfuerzo = _serviceBL.ValoresComponente(_serviceId, Constants.PRUEBA_ESFUERZO_ID);
                        var Altura7D = _serviceBL.ValoresComponente(_serviceId, Constants.ALTURA_7D_ID);
                        var AlturaEstructural = _serviceBL.ValoresComponente(_serviceId, Constants.ALTURA_ESTRUCTURAL_ID);
                        var Odontologia = _serviceBL.ValoresComponente(_serviceId, Constants.ODONTOGRAMA_ID);
                        var OsteoMuscular = _serviceBL.ValoresComponente(_serviceId, Constants.OSTEO_MUSCULAR_ID_1);

                        var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

                        FichaMedicaOcupacional312.CreateFichaMedicalOcupacional312Report(_DataService,
                                  filiationData, _listAtecedentesOcupacionales, _listaPatologicosFamiliares,
                                  _listMedicoPersonales, _listaHabitoNocivos, Antropometria, FuncionesVitales,
                                  ExamenFisico, Oftalmologia, Psicologia, RX, RX1, Laboratorio, Audiometria, Espirometria,
                                  _DiagnosticRepository, _Recomendation, _ExamenesServicio, MedicalCenter,                     
                                  saveFileDialog2.FileName);

                        this.Enabled = true;
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            //}

        }
     
        private void button3_Click(object sender, EventArgs e)
        {
           
            var frm = new Reports.frmMusculoesqueletico(_serviceId, Constants.OSTEO_MUSCULAR_ID_1);
            frm.ShowDialog();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmAlturaEstructural(_serviceId, Constants.ALTURA_ESTRUCTURAL_ID);
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOftalmologia(_serviceId, Constants.OFTALMOLOGIA_ID);
            frm.ShowDialog();
        }

        private void btn7D_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmAnexo7D(_serviceId, Constants.ALTURA_7D_ID);
            frm.ShowDialog();
        }

        private void btnOdontograma_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOdontograma(_serviceId, Constants.ODONTOGRAMA_ID);
            frm.ShowDialog();
        }

        private void btnHistoriaOcupacional_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmHistoriaOcupacional(_serviceId);
            frm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmRadiologico(_serviceId, Constants.RX_TORAX_ID);
            frm.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void btnRadiologico_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmRadiologico(_serviceId, Constants.RX_TORAX_ID);
            frm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOsteomuscular(_serviceId, Constants.OSTEO_MUSCULAR_ID_1);
            frm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmPruebaEsfuerzo(_serviceId, Constants.PRUEBA_ESFUERZO_ID);
            frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmInformeRadiograficoOIT(_serviceId, Constants.RX_TORAX_ID);
            frm.ShowDialog();
            
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            var frm = new Reports.frmEstudioElectrocardiografico(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);
            frm.ShowDialog();
        }

        private void btnConsolidadoReportes_Click(object sender, EventArgs e)
        {
            int flagPantalla = int.Parse(ddlServiceTypeId.SelectedValue.ToString());      

                var frm = new Reports.frmManagementReports(_serviceId, _pacientId, _customerOrganizationName, _personFullName, flagPantalla);
                frm.ShowDialog();

                btnFilter_Click(sender, e);

        }

        private void btnDermatologico_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmTamizajeDermatologico(_serviceId, Constants.TAMIZAJE_DERMATOLOGIO_ID);
            frm.ShowDialog();
        }

        private void btnGenerarLiquidacion_Click(object sender, EventArgs e)
        {
            var service = new ServiceList();

            service.v_ServiceId = _serviceId;
            service.v_ProtocolName = grdDataService.Selected.Rows[0].Cells["v_ProtocolName"].Value.ToString();
            service.v_CustomerOrganizationName = _customerOrganizationName;
            service.v_Pacient = _personFullName;
            service.v_MasterServiceName = grdDataService.Selected.Rows[0].Cells["v_MasterServiceName"].Value.ToString();
            service.v_EsoTypeName = grdDataService.Selected.Rows[0].Cells["v_EsoTypeName"].Value.ToString();
            service.i_StatusLiquidation = (int?)grdDataService.Selected.Rows[0].Cells["i_StatusLiquidation"].Value;

            var frm = new frmBeforeLiquidationProcess(service);
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;
            
            BindGrid();

        }

        private void grdDataService_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
          
        }

        private void grdDataService_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["i_StatusLiquidation"].Value == null)
                return;

            if ((int)e.Row.Cells["i_StatusLiquidation"].Value == (int)PreLiquidationStatus.Generada)
            {
                e.Row.Cells["Liq"].Value = Resources.accept;
                e.Row.Cells["Liq"].ToolTipText = "Generada";
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ddlConsultorio_TabIndexChanged(object sender, EventArgs e)
        {
          
           
        }

        private void ddlConsultorio_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ddlConsultorio.SelectedIndex == 0)
            {
                _componentIds = null;
                return;
            }

            _componentIds = new List<string>();
            var eee = (KeyValueDTO)ddlConsultorio.SelectedItem;

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

        private void btnInterconsulta_Click(object sender, EventArgs e)
        {
            frmInterconsulta frm = new frmInterconsulta(_serviceId);
            frm.ShowDialog();
        }

        private void btnTiempos_Click(object sender, EventArgs e)
        {
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);

            var frm = new Reports.TiemposTrabajadores(strFilterExpression, pdatBeginDate, pdatEndDate, _componentIds);
            frm.ShowDialog();
        }

        private void btnFechaEntrega_Click(object sender, EventArgs e)
        {

            _ListaServicios = new List<string>();
            foreach (var item in grdDataService.Rows)
            {
                //CheckBox ck = (CheckBox)item.Cells["b_FechaEntrega"].Value;

                if ((bool)item.Cells["b_FechaEntrega"].Value)
                {
                    string x = item.Cells["v_ServiceId"].Value.ToString();
                    _ListaServicios.Add(x);
                }
            }

            if (_ListaServicios.Count == 0)
            {
                MessageBox.Show("No hay ningún servicio con check, por favor seleccionar uno.", "VALIDACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmPopupFechaEntrega frm = new frmPopupFechaEntrega(_ListaServicios, "service");
            frm.ShowDialog();

            btnFilter_Click(sender, e);
        }

        private void btnBotonOculto_Click(object sender, EventArgs e)
        {
            using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
            {
                //Obtener Lista de MultifileId
                OperationResult operationResult = new OperationResult();
                MultimediaFileBL oMultimediaFileBL = new MultimediaFileBL();
                var Lista = oMultimediaFileBL.DevolverTodosArchivos();
                foreach (var item in Lista)
                {
                    var multimediaFile = oMultimediaFileBL.GetMultimediaFileById(ref operationResult, item.v_MultimediaFileId);

                    if (multimediaFile != null)
                    {
                        string mdoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {

                            string Fecha = multimediaFile.FechaServicio.Value.Day.ToString().PadLeft(2, '0') + multimediaFile.FechaServicio.Value.Month.ToString().PadLeft(2, '0') + multimediaFile.FechaServicio.Value.Year.ToString();

                            //Obtener la extensión del archivo
                            string Ext = multimediaFile.FileName.Substring(multimediaFile.FileName.Length - 3, 3);

                            sfd.Title = multimediaFile.dni + "-" + Fecha + "-" + multimediaFile.FileName + "." + Ext;
                            sfd.FileName = mdoc + "\\" + sfd.Title;

                            string path = sfd.FileName;
                            if (multimediaFile.ByteArrayFile != null)
                            {
                                File.WriteAllBytes(path, multimediaFile.ByteArrayFile);
                            }


                        }
                    }

                }
            }
        }

       
    
    }
}
