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

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmBuscarServicioPendiente : Form
    {
        string strFilterExpression;
        ServiceBL _serviceBL = new ServiceBL();
        List<ServiceList> _ListaServiceList = new List<ServiceList>();
        public  List<FacturacionDetalleList> _ListaFacturacionList = new List<FacturacionDetalleList>();
        public DateTime? _FechaInicio;
        public DateTime? _FechaFin;
        string _EmpresaId;
        string _SedeId;

        public frmBuscarServicioPendiente(string pstrEmpresaId, string pstrSedeId)
        {
            InitializeComponent();
            _EmpresaId = pstrEmpresaId;
            _SedeId = pstrSedeId;
        }

        private void frmBuscarServicioPendiente_Load(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            List<string> Filters = new List<string>();
            Filters.Add("v_CustomerOrganizationId==" + "\"" + _EmpresaId + "\"&&v_CustomerLocationId==" + "\"" + _SedeId + "\"");
            Filters.Add("i_IsFac==0");

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
            _ListaServiceList = objData;
            lblRecordCountCalendar.Text = string.Format("Se encontraron {0} registros.", objData.Count());

            if (grdDataService.Rows.Count > 0)
                grdDataService.Rows[0].Selected = true;
        }

        private List<ServiceList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);

            _FechaInicio = pdatBeginDate;
            _FechaFin = pdatEndDate;
            var _objData = _serviceBL.GetServicesPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate, null);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void btnAgregarFacturacion_Click(object sender, EventArgs e)
        {

            FacturacionDetalleList oFacturacionDetalleList;
        

            foreach (var item in _ListaServiceList)
            {
                oFacturacionDetalleList = new FacturacionDetalleList();
                oFacturacionDetalleList.v_ServicioId = item.v_ServiceId;
                oFacturacionDetalleList.d_ServiceDate = item.d_ServiceDate;
                oFacturacionDetalleList.Trabajador = item.v_Pacient;
                oFacturacionDetalleList.d_Monto = 0;

                _ListaFacturacionList.Add(oFacturacionDetalleList);
                
            }

            this.DialogResult = DialogResult.OK;

        }
    }
}
