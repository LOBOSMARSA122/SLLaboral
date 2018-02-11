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
    public partial class frmOsteo : Form
    {
        private string _serviceId;
        private string _ComponentId;

        public frmOsteo(string pstServiceId, string ComponentId)
        {
            InitializeComponent();
            _serviceId = pstServiceId;
            _ComponentId = ComponentId;
        }

        private void frmOsteo_Load(object sender, EventArgs e)
        {
            using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
            {
                ShowReport();
            }
        }

        private void ShowReport()
        {
            OperationResult objOperationResult = new OperationResult();

            var rp = new Reports.crOsteo();

            var aptitudeCertificate = new ServiceBL().GetReportOsteo(_serviceId, _ComponentId);
            DataSet ds1 = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(aptitudeCertificate);

            dt.TableName = "dtOsteo";

            ds1.Tables.Add(dt);

            rp.SetDataSource(ds1);

            crystalReportViewer1.ReportSource = rp;
            crystalReportViewer1.Show();

        }
    }
}
