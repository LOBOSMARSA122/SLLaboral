using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.Reports
{
    public partial class frmInterconsulta : Form
    {
        private string _serviceId;
        private string _Altitud;
        private string _Especialidad;
        private string _Labor;
        private string _Solicita;
        private string _Observaciones;
        private List<DxCie10> _Lista;

        public frmInterconsulta(string pstServiceId, string pstrAltitud, string pstrEspecialidad, string pstrLabor, string pstrSolicita, List<DxCie10> Lista, string pstrObservaciones)
        {
            InitializeComponent();
            _serviceId = pstServiceId;
            _Altitud = pstrAltitud;
            _Especialidad = pstrEspecialidad;
            _Labor = pstrLabor;
            _Solicita = pstrSolicita;
            _Observaciones = pstrObservaciones;
            _Lista = Lista;
        }

        private void frmInterconsulta_Load(object sender, EventArgs e)
        {
            using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
            {
                ShowReport();
            }
        }

        private void ShowReport()
        {
            OperationResult objOperationResult = new OperationResult();

            var rp = new Reports.crReporteInterconsulta();

            var aptitudeCertificate = new ServiceBL().GetReportInterconsulta(_serviceId, _Altitud, _Especialidad, _Labor, _Solicita, _Observaciones);
            DataSet ds1 = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(aptitudeCertificate);
            dt.TableName = "dtInterconsulta";
            ds1.Tables.Add(dt);

            DataTable dt1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(_Lista);
            dt1.TableName = "dtDxs";
            ds1.Tables.Add(dt1);


            rp.SetDataSource(ds1);

            crystalReportViewer1.ReportSource = rp;
            crystalReportViewer1.Show();

        }
    }
}
