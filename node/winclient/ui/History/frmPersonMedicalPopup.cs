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

namespace Sigesoft.Node.WinClient.UI.History
{
    public partial class frmPersonMedicalPopup : Form
    {
        public int _TypeDiagnosticId;
        public string _TypeDiagnosticName;
        public DateTime? _StartDate = null;
        public string _DiagnosticDetail;
        public DateTime _Date;
        public string _TreatmentSite;

        public frmPersonMedicalPopup(string DiagnosticName, int TypeDiagnosticId, DateTime StartDate, string DiagnosticDetail, DateTime? Date, string TreatmentSite)
        {
            InitializeComponent();

            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlTypeDiagnosticId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 139, null), DropDownListAction.Select);
        
            this.Text = this.Text + DiagnosticName;
            ddlTypeDiagnosticId.SelectedValue = TypeDiagnosticId.ToString();
            dtpDateTimeStar.Value = StartDate;
            txtDxDetail.Text = DiagnosticDetail;
            txtTreatmentSite.Text = TreatmentSite;
        }

        private void frmPersonMedicalPopup_Load(object sender, EventArgs e)
        {
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (uvPersonMedicalPopup.Validate(true, false).IsValid)
            {
                _TypeDiagnosticId = int.Parse(ddlTypeDiagnosticId.SelectedValue.ToString());
                _TypeDiagnosticName = ddlTypeDiagnosticId.Text;
                _StartDate = dtpDateTimeStar.Value.Date;
                _DiagnosticDetail = txtDxDetail.Text;
                _TreatmentSite = txtTreatmentSite.Text;

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
