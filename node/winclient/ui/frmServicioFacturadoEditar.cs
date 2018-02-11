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
    public partial class frmServicioFacturadoEditar : Form
    {
        FacturacionBL oFacturacionBL = new FacturacionBL();
        List<FacturacionDetalleList> _ListaFacturacionDetalle = new List<FacturacionDetalleList>();
        facturacionDto ofacturacionDto = new facturacionDto(); 
        string _Mode;
        string _v_FacturacionId;
        string _EmpresaCliente;
        string _EmpresaSede;
        DateTime _FechaInicio;
        DateTime _FechaFin;
        public frmServicioFacturadoEditar(string pstrIdServicioFacturado, string pstrMode)
        {
            InitializeComponent();
            _Mode = pstrMode;
            _v_FacturacionId = pstrIdServicioFacturado;
        }

        private void frmServicioFacturadoEditar_Load(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            Utils.LoadDropDownList(cboEstadoFacturacion, "Value1", "Id", BLL.Utils.GetDataHierarchyForCombo(ref objOperationResult, 117, null), DropDownListAction.Select);

            int nodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));
            var dataListOrganization1 = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, nodeId);
            Utils.LoadDropDownList(cbCustomerOrganization,
                "Value1",
                "Id",
                dataListOrganization1,
                DropDownListAction.Select);

              if (_Mode == "New")
            {
                // Additional logic here.

            }
              else if (_Mode == "Edit")
              {

                cbCustomerOrganization.Enabled = false;
                btnFilter.Enabled = false;
                btnImprimir.Enabled = false;
                btnComision.Enabled = true;
                //btnschedule.Enabled = true;

                ofacturacionDto=  oFacturacionBL.GetFacturacion(ref objOperationResult, _v_FacturacionId);
                dtpDateTimeStar.Value = (DateTime)ofacturacionDto.d_FechaRegistro;
                dtFechaCobro.Value = (DateTime)ofacturacionDto.d_FechaCobro;
                txtNroFactura.Text = ofacturacionDto.v_NumeroFactura;
                cboEstadoFacturacion.SelectedValue = ofacturacionDto.i_EstadoFacturacion.ToString();
                cbCustomerOrganization.SelectedValue = string.Format("{0}|{1}", ofacturacionDto.v_EmpresaCliente, ofacturacionDto.v_EmpresaSede);
                txtTotalFacturar.Text = ofacturacionDto.d_MontoTotal.ToString();
                txtIgv.Text = ofacturacionDto.d_Igv.ToString();
                txtSubTotal.Text = ofacturacionDto.d_SubTotal.ToString();
                txtDetraccion.Text = ofacturacionDto.d_Detraccion.ToString();
                _ListaFacturacionDetalle = oFacturacionBL.GetListFacturacionDetalle(ref objOperationResult, _v_FacturacionId);
                _FechaInicio = ofacturacionDto.d_FechaInicio.Value;
                _FechaFin = ofacturacionDto.d_FechaFin.Value;
                
                  grdData.DataSource = _ListaFacturacionDetalle;

              }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (uvValidar.Validate(true, false).IsValid)
            {
                var id2 = cbCustomerOrganization.SelectedValue.ToString().Split('|');

                frmBuscarServicioPendiente frm = new frmBuscarServicioPendiente(id2[0], id2[1]);
                frm.ShowDialog();

                if (frm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    return;

                if (frm._ListaFacturacionList == null)
                    return;

                _ListaFacturacionDetalle = frm._ListaFacturacionList;
                DateTime? FechaInicio = frm._FechaInicio;
                DateTime? FechaFin = frm._FechaFin;

                var Result = oFacturacionBL.LlenarGrillaSigesfot("", id2[0], id2[1], FechaInicio.Value.Date, FechaFin.Value.Date, -1);

                _EmpresaCliente = id2[0];
                _EmpresaSede = id2[1];
                _FechaInicio = FechaInicio.Value.Date;
                _FechaFin = FechaFin.Value.Date;

                double Total = double.Parse(Result.Sum(p => p.Total).ToString());
                double Igv;
                double SubTotal;

                if (Total > 700)
                {
                    txtDetraccion.Text = (Total * 0.1).ToString();
                }
                else
                {
                    txtDetraccion.Text = "0.00";
                }

                Igv = (Total * 1.18) - Total;
                SubTotal = Total - Igv;

                txtTotalFacturar.Text = Total.ToString();
                txtIgv.Text = Igv.ToString();
                txtSubTotal.Text = SubTotal.ToString();
                grdData.DataSource = _ListaFacturacionDetalle;
               

            }
            else
            {
                MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnschedule_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            facturacionDto ofacturacionDto = new facturacionDto();

            facturaciondetalleDto ofacturaciondetalleDto;
            List<facturaciondetalleDto> ListaFacturacionDetalle = new List<facturaciondetalleDto>();
            if (uvValidarFormulario.Validate(true, false).IsValid)
            {
            if (_Mode == "New")
            {
                ofacturacionDto.d_FechaRegistro = dtpDateTimeStar.Value;
                ofacturacionDto.d_FechaCobro = dtFechaCobro.Value;
                ofacturacionDto.v_NumeroFactura = txtNroFactura.Text;
                ofacturacionDto.i_EstadoFacturacion = int.Parse(cboEstadoFacturacion.SelectedValue.ToString());
                var id2 = cbCustomerOrganization.SelectedValue.ToString().Split('|');
                ofacturacionDto.v_EmpresaCliente = id2[0];
                ofacturacionDto.v_EmpresaSede = id2[1];
                ofacturacionDto.d_MontoTotal = decimal.Parse(txtTotalFacturar.Text.ToString());
                ofacturacionDto.d_Detraccion = decimal.Parse(txtDetraccion.Text.ToString());
                ofacturacionDto.d_Igv = decimal.Parse(txtIgv.Text.ToString());
                ofacturacionDto.d_SubTotal = decimal.Parse(txtSubTotal.Text.ToString());
                ofacturacionDto.d_FechaInicio = _FechaInicio;
                ofacturacionDto.d_FechaFin = _FechaFin;

                foreach (var item in _ListaFacturacionDetalle)
                {
                    ofacturaciondetalleDto = new facturaciondetalleDto();
                    ofacturaciondetalleDto.v_ServicioId = item.v_ServicioId;
                    ofacturaciondetalleDto.d_Monto = item.d_Monto;

                    ListaFacturacionDetalle.Add(ofacturaciondetalleDto);
                }

              _v_FacturacionId =  oFacturacionBL.AddFacturacion(ref objOperationResult, ofacturacionDto, ListaFacturacionDetalle, Globals.ClientSession.GetAsList());
                
            }
            else if (_Mode == "Edit")
            {
                ofacturacionDto.v_FacturacionId = _v_FacturacionId;
                ofacturacionDto.d_FechaRegistro = dtpDateTimeStar.Value;
                ofacturacionDto.d_FechaCobro = dtFechaCobro.Value;
                ofacturacionDto.v_NumeroFactura = txtNroFactura.Text;
                ofacturacionDto.i_EstadoFacturacion = int.Parse(cboEstadoFacturacion.SelectedValue.ToString());
                ofacturacionDto.d_MontoTotal = decimal.Parse(txtTotalFacturar.Text.ToString());
                ofacturacionDto.d_Detraccion = decimal.Parse(txtDetraccion.Text.ToString());
                ofacturacionDto.d_Igv = decimal.Parse(txtIgv.Text.ToString());
                ofacturacionDto.d_SubTotal = decimal.Parse(txtSubTotal.Text.ToString());
                var id2 = cbCustomerOrganization.SelectedValue.ToString().Split('|');
                ofacturacionDto.v_EmpresaCliente = id2[0];
                ofacturacionDto.v_EmpresaSede = id2[1];

                foreach (var item in _ListaFacturacionDetalle)
                {
                    ofacturaciondetalleDto = new facturaciondetalleDto();
                    ofacturaciondetalleDto.v_ServicioId = item.v_ServicioId;
                    ofacturaciondetalleDto.d_Monto = item.d_Monto;

                    ListaFacturacionDetalle.Add(ofacturaciondetalleDto);
                }

                oFacturacionBL.UpdateFacturacion(ref objOperationResult, ofacturacionDto, Globals.ClientSession.GetAsList());
                

            }

            //// Analizar el resultado de la operación
            if (objOperationResult.Success == 1)  // Operación sin error
            {
                btnImprimir.Enabled = true;
                btnComision.Enabled = true;
                MessageBox.Show("Se grabo correctamente", "SISTEMAS!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.DialogResult = System.Windows.Forms.DialogResult.OK;
                //this.Close();
            }
            else  // Operación con error
            {
                btnImprimir.Enabled = false;
                btnComision.Enabled = false;
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Se queda en el formulario.
               
            }

           

           
                
            }
            else
            {
                MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Reports.frmFactura frm = new Reports.frmFactura(_v_FacturacionId,_EmpresaCliente,_EmpresaSede,_FechaInicio,_FechaFin);
            frm.ShowDialog();
        }

        private void btnComision_Click(object sender, EventArgs e)
        {
            var id2 = cbCustomerOrganization.SelectedValue.ToString().Split('|');
            frmComisionVendedor frm = new frmComisionVendedor(_v_FacturacionId, id2[0], id2[1], _FechaInicio, _FechaFin,cbCustomerOrganization.Text);
            frm.ShowDialog();
        }
    }
}
