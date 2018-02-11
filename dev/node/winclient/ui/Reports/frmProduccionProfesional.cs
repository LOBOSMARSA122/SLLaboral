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

namespace Sigesoft.Node.WinClient.UI.Reports
{
    public partial class frmProduccionProfesional : Form
    {
        ServiceBL _serviceBL = new ServiceBL();
        string strFilterExpression;

        List<KeyValueDTO> _componentListTemp = new List<KeyValueDTO>();
       
        public frmProduccionProfesional()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmProduccionProfesional_Load(object sender, EventArgs e)
        {
            LoadComboBox();
        }
        private void LoadComboBox()
        {
            OperationResult objOperationResult1 = new OperationResult();
            int nodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));
            var dataListOrganization2 = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult1, nodeId);

            Utils.LoadDropDownList(cbOrganizationInvoice,
             "Value1",
             "Id",
             dataListOrganization2,
             DropDownListAction.All);


            OperationResult objOperationResult = new OperationResult();

            _componentListTemp = BLL.Utils.GetAllComponents(ref objOperationResult);

            var xxx = _componentListTemp.FindAll(p => p.Value4 != -1);

            List<KeyValueDTO> groupComponentList = xxx.GroupBy(x => x.Value4).Select(group => group.First()).ToList();

            groupComponentList.AddRange(_componentListTemp.ToList().FindAll(p => p.Value4 == -1));

            Utils.LoadDropDownList(ddlComponentId, "Value1", "Value4", groupComponentList, DropDownListAction.Select);

            Utils.LoadDropDownList(ddlUsuario, "Value1", "Id", BLL.Utils.GetProfessional(ref objOperationResult,""), DropDownListAction.Select);

        }

        private void ddlUsuario_SelectedValueChanged(object sender, EventArgs e)
        {
            ProfessionalBL oProfessionalBL = new ProfessionalBL();
            OperationResult objOperationResult = new OperationResult();
            SystemUserList oSystemUserList = new SystemUserList();

            if (ddlUsuario.SelectedValue == null)
                return;

            if (ddlUsuario.SelectedValue.ToString() == "-1")
            {
                lblNombreProfesional.Text = "Nombres y Apellidos del Profesional";
                return;
            }

            oSystemUserList = oProfessionalBL.GetSystemUserName(ref objOperationResult, int.Parse(ddlUsuario.SelectedValue.ToString()));

            lblNombreProfesional.Text = oSystemUserList.v_PersonName;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (uvReporte.Validate(true, false).IsValid)
            {

                if (!chkProfesional.Checked )
                {
                    if (ddlComponentId.SelectedIndex == 0)
                    {
                        MessageBox.Show("Por favor seleccione consultorio", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }                    
                }
                using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                {
                    List<string> Filters = new List<string>();
                    DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
                    DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);

                    if (cbOrganizationInvoice.SelectedValue.ToString() != "-1")
                    {
                        var id3 = cbOrganizationInvoice.SelectedValue.ToString().Split('|');
                        Filters.Add("v_CustomerOrganizationId==" + "\"" + id3[0] + "\"&&v_CustomerLocationId==" + "\"" + id3[1] + "\"");
                    }


                    
                    if (chkProfesional.Checked)
                    {
                        if (ddlUsuario.SelectedValue.ToString() != "-1")
                            Filters.Add("i_UpdateUserOccupationalMedicaltId==" + ddlUsuario.SelectedValue);
                    }
                    else
                    {
                        if (ddlUsuario.SelectedValue.ToString() != "-1")
                            Filters.Add("i_ApprovedUpdateUserId==" + ddlUsuario.SelectedValue);
                    }
                 

                    if (!chkProfesional.Checked)
                    {
                        if (ddlComponentId.SelectedValue.ToString() != "-1")
                            Filters.Add("i_CategoryId==" + ddlComponentId.SelectedValue);
                    }
                   


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

                    if (chkProfesional.Checked)
                    {
                        var objData = _serviceBL.ReporteProduccionProfesionalSinConsultorio(pdatBeginDate, pdatEndDate, cbOrganizationInvoice.SelectedValue.ToString(), strFilterExpression, ddlUsuario.Text, lblNombreProfesional.Text, ddlComponentId.Text, int.Parse(ddlComponentId.SelectedValue.ToString()), cbOrganizationInvoice.Text);

                        grdData.DataSource = objData;
                        lblRecordCount.Text = string.Format("Se encontraron {0} registros.", objData.Count());
                    }
                    else
                    {
                        var objData = _serviceBL.ReporteProduccionProfesional(pdatBeginDate, pdatEndDate, cbOrganizationInvoice.SelectedValue.ToString(), strFilterExpression, ddlUsuario.Text, lblNombreProfesional.Text, ddlComponentId.Text, int.Parse(ddlComponentId.SelectedValue.ToString()), cbOrganizationInvoice.Text);

                        grdData.DataSource = objData;
                        lblRecordCount.Text = string.Format("Se encontraron {0} registros.", objData.Count());
                    }
                   
                   

                }

                if (grdData.Rows.Count > 0)
                {
                    grdData.Rows[0].Selected = true;
                    grdData.Select();
                }
            }
            else
            {
                MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> Filters = new List<string>();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);

            if (cbOrganizationInvoice.SelectedValue.ToString() != "-1")
            {
                var id3 = cbOrganizationInvoice.SelectedValue.ToString().Split('|');
                Filters.Add("v_CustomerOrganizationId==" + "\"" + id3[0] + "\"&&v_CustomerLocationId==" + "\"" + id3[1] + "\"");
            }

            if (ddlUsuario.SelectedValue.ToString() != "-1")
                Filters.Add("i_ApprovedUpdateUserId==" + ddlUsuario.SelectedValue);

            if (ddlComponentId.SelectedValue.ToString() != "-1")
                Filters.Add("i_CategoryId==" + ddlComponentId.SelectedValue);


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

            var frm = new Reports.frmProduccionProfesionalImprimir(pdatBeginDate.Value, pdatEndDate.Value, cbOrganizationInvoice.SelectedValue.ToString(), strFilterExpression, ddlUsuario.Text, lblNombreProfesional.Text, ddlComponentId.Text, int.Parse(ddlComponentId.SelectedValue.ToString()), cbOrganizationInvoice.Text);
            frm.ShowDialog();
        }

        private void chkProfesional_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProfesional.Checked)
            {
                ddlComponentId.SelectedIndex = 0;
                ddlComponentId.Enabled = false;
                
            }
            else
            {
                ddlComponentId.Enabled = true;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.Filter = "Files (*.xls;*.xlsx;*)|*.xls;*.xlsx;*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ultraGridExcelExporter1.Export(this.grdData, saveFileDialog1.FileName);
                MessageBox.Show("Se exportaron correctamente los datos.", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
        }

        private void grdData_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            btnExport.Enabled = grdData.Rows.Count > 0;
        }

    }
}
