using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Common;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmLogin3 : Form
    {
        private int _intNodeId;

        public frmLogin3()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Obtener el ID del nodo del archivo de configuración
            _intNodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));

            // Leer el nodo
            NodeBL objNodeBL = new NodeBL();
            OperationResult objOperationResult = new OperationResult();
            nodeDto objNodeDto = objNodeBL.GetNodeByNodeId(ref objOperationResult, _intNodeId);
            if (objOperationResult.Success == 1)
            {
                // Mostrar el nombre del nodo
                txtNode.Text = objNodeDto.v_Description;
            }
            else
            {
                MessageBox.Show(objOperationResult.ExceptionMessage, "Error al conectarse a Base de Datos !!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnOK.Enabled = false;
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;

                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.AuthenticateUser();
            ///
        }

        private void AuthenticateUser()
        {
            // Autenticación de usuario
            SecurityBL objSecurityBL = new SecurityBL();

            OperationResult objOperationResult = new OperationResult();
            var objUserDto = objSecurityBL.ValidateSystemUser(ref objOperationResult, _intNodeId, txtUserName.Text, Common.Utils.Encrypt(txtPassword.Text));

            if (objUserDto != null)
            {
                ClientSession objClientSession = new ClientSession();
                objClientSession.i_SystemUserId = objUserDto.i_SystemUserId;
                objClientSession.v_UserName = objUserDto.v_UserName;
                objClientSession.i_CurrentExecutionNodeId = _intNodeId;
                objClientSession.v_CurrentExecutionNodeName = txtNode.Text;
                //_ClientSession.i_CurrentOrganizationId = 57;
                objClientSession.v_PersonId = objUserDto.v_PersonId;

                // Pasar el objeto de sesión al gestor de objetos globales
                Globals.ClientSession = objClientSession;

                // Abrir el formulario principal
                this.Hide();
                frmMaster frm = new frmMaster();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show(objOperationResult.AdditionalInformation, "Advertencia-->>>>>>>>>>>>>>>", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            // Autenticar el usuario



        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            //btnOK.Enabled = (txtUserName.Text != string.Empty && txtPassword.Text != string.Empty);
        }


    }
}
