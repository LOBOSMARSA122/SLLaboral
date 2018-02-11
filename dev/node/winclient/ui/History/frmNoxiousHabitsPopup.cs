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
    public partial class frmNoxiousHabitsPopup : Form
    {
        public string _Frequency;
        public string _Comment;

        public frmNoxiousHabitsPopup(string HabitsName, string Frecuency, string Comment)
        {
            OperationResult objOperationResult = new OperationResult();
            InitializeComponent();
            this.Text = this.Text + HabitsName;
            txtComment.Text = Comment;


            if (HabitsName.ToUpper() == "CONSUMO DE ALCOHOL" || HabitsName.ToUpper() == "CONSUMO DE DROGAS")
            {
                Utils.LoadDropDownList(ddlFrecuencyId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 205, null), DropDownListAction.Select);
            }
            else if(HabitsName.ToUpper() == "TABAQUISMO")
            {
                Utils.LoadDropDownList(ddlFrecuencyId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 206, null), DropDownListAction.Select);
            }
            else if (HabitsName.ToUpper() == "ACTIVIDAD FÍSICA")
            {
                Utils.LoadDropDownList(ddlFrecuencyId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 223, null), DropDownListAction.Select);
            }

            if (Frecuency != null)
            {
                ddlFrecuencyId.Text = Frecuency;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (uvNoxiousHabits.Validate(true, false).IsValid)
            {
                _Frequency = ddlFrecuencyId.Text;
                _Comment = txtComment.Text;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void frmNoxiousHabitsPopup_Load(object sender, EventArgs e)
        {

        }
    }
}
