using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sigesoft.Node.WinClient.UI.History
{
    public partial class frmFamilyMedicPopup : Form
    {
        public string _CommentFamilyMedic;
        public string _Group;
        public frmFamilyMedicPopup(string DiagnosticName, string Comment, string Group)
        {
            InitializeComponent();
            this.Text = this.Text + Group  +" / " + DiagnosticName;
            txtComment.Text = Comment;
            _Group = Group;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _CommentFamilyMedic = txtComment.Text;
           
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
