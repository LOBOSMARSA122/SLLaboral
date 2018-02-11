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

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmServicioFacturado : Form
    {
        string strFilterExpression;
        List<FacturacionList> _objData = new List<FacturacionList>();
        FacturacionBL _objFacturacionBL = new FacturacionBL();
        public frmServicioFacturado()
        {
            InitializeComponent();
        }

        private void frmServicioFacturado_Load(object sender, EventArgs e)
        {

            OperationResult objOperationResult = new OperationResult();

            Utils.LoadDropDownList(cboEstadoFacturacion, "Value1", "Id", BLL.Utils.GetDataHierarchyForCombo(ref objOperationResult, 117, null), DropDownListAction.All);        

            int nodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));
            var dataListOrganization1 = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, nodeId);
            Utils.LoadDropDownList(cbCustomerOrganization,
                "Value1",
                "Id",
                dataListOrganization1,
                DropDownListAction.All);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            frmServicioFacturadoEditar frm = new frmServicioFacturadoEditar("0", "New");
            frm.ShowDialog();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            string strServicioFacturadoId = grdData.Selected.Rows[0].Cells["v_FacturacionId"].Value.ToString();

            frmServicioFacturadoEditar frm = new frmServicioFacturadoEditar(strServicioFacturadoId, "Edit");
            frm.ShowDialog();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            // Obtener los IDs de la fila seleccionada

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == System.Windows.Forms.DialogResult.Yes)
            {
                // Delete the item
                string strFacturacionId = grdData.Selected.Rows[0].Cells["v_FacturacionId"].Value.ToString();
                _objFacturacionBL.DeleteFacturacion(ref objOperationResult, strFacturacionId, Globals.ClientSession.GetAsList());

                btnFilter_Click(sender, e);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            List<string> Filters = new List<string>();
            if (cboEstadoFacturacion.SelectedValue.ToString() != "-1") Filters.Add("i_EstadoFacturacion==" + cboEstadoFacturacion.SelectedValue);

            if (!string.IsNullOrEmpty(txtNroFactura.Text)) Filters.Add("v_NumeroFactura==" + "\"" + txtNroFactura.Text.Trim() + "\"");
          if (cbCustomerOrganization.SelectedValue.ToString() != "-1")
          {
              var id2 = cbCustomerOrganization.SelectedValue.ToString().Split('|');
              Filters.Add("v_EmpresaCliente==" + "\"" + id2[0] + "\"&&v_EmpresaSede==" + "\"" + id2[1] + "\"");

          }
          Filters.Add("i_IsDeleted==0");

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
            var objData = GetData(0, null, null, strFilterExpression);

            grdData.DataSource = objData;
        }

        private List<FacturacionList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);
            string tipoFecha= "F";

            if (rbFacturacion.Checked)
            {
                tipoFecha = "F";
            }
            else 
            {
                tipoFecha = "C";
            }

            _objData = _objFacturacionBL.GetFacturacionPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate, tipoFecha);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void grdData_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            btnEditar.Enabled=
            btnEliminar.Enabled=

                  (grdData.Selected.Rows.Count > 0);

            if (grdData.Selected.Rows.Count == 0)
                return;   

        }
        
    }
}
