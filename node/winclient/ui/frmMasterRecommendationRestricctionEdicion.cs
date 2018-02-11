﻿using System;
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
    public partial class frmMasterRecommendationRestricctionEdicion : Form
    {
        MasterRecommendationRestricctionBL _objBL = new MasterRecommendationRestricctionBL();
        int _TypifyingId;
        string _Mode;
        string _MasterRecommendationRestricctionId;
        masterrecommendationrestricctionDto _masterrecommendationrestricctionDto = new masterrecommendationrestricctionDto();

        public frmMasterRecommendationRestricctionEdicion(int pintTypifyingId, string pstrMode, string pstrMasterRecommendationRestricctionId)
        {
            InitializeComponent();
            this.Text = this.Text + " (" + pstrMasterRecommendationRestricctionId + ")";
          
            _TypifyingId = pintTypifyingId;
            _Mode = pstrMode;
            _MasterRecommendationRestricctionId = pstrMasterRecommendationRestricctionId;

        }

        private void frmMasterRecommendationRestricctionEdicion_Load(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            //Llenado de combos
           
            if (_Mode == "New")
            {
                // Additional logic here.

            }
            else if (_Mode == "Edit")
            {
                // Get the Entity Data

                _masterrecommendationrestricctionDto = _objBL.GetMasterRecommendationRestricction(ref objOperationResult, _MasterRecommendationRestricctionId);

               txtName.Text = _masterrecommendationrestricctionDto.v_Name;               

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            if (uvOrganization.Validate(true, false).IsValid)
            {               
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Por favor ingrese un nombre apropiado para la Razón Social.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
              
                if (_Mode == "New")
                {
                    _masterrecommendationrestricctionDto = new masterrecommendationrestricctionDto();
                    // Populate the entity                    
                    _masterrecommendationrestricctionDto.v_Name = txtName.Text.Trim();
                    _masterrecommendationrestricctionDto.i_TypifyingId = _TypifyingId;
                    
                    // Save the data
                    _objBL.AddMasterRecommendationRestricction(ref objOperationResult, _masterrecommendationrestricctionDto, Globals.ClientSession.GetAsList());
                }
                else if (_Mode == "Edit")
                {
                    // Populate the entity
                    _masterrecommendationrestricctionDto.v_Name = txtName.Text.Trim();    
                                    
                    // Save the data
                    _objBL.UpdateMasterRecommendationRestricction(ref objOperationResult, _masterrecommendationrestricctionDto, Globals.ClientSession.GetAsList());
                }

                //// Analizar el resultado de la operación
                if (objOperationResult.Success == 1)  // Operación sin error
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else  // Operación con error
                {
                    MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
