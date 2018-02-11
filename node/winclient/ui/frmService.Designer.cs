namespace Sigesoft.Node.WinClient.UI
{
    partial class frmService
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ServiceId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CreationUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_CreationDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_ServiceDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_UpdateUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_UpdateDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_MasterServiceName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ServiceStatusName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_OrganizationName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_LocationName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ServiceTypeName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ProtocolName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_Pacient");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_AptitudeStatusName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Liq", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmService));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddlConsultorio = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.ddlStatusAptitudId = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ddlMasterServiceId = new System.Windows.Forms.ComboBox();
            this.ddlCustomerOrganization = new System.Windows.Forms.ComboBox();
            this.ddlProtocolId = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ddlServiceTypeId = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ddServiceStatusId = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPacient = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ddlEsoType = new System.Windows.Forms.ComboBox();
            this.dptDateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpDateTimeStar = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRecordCountCalendar = new System.Windows.Forms.Label();
            this.grdDataService = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmService = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CertificadoAptitud = new System.Windows.Forms.ToolStripMenuItem();
            this.verEditarServicioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Examenes = new System.Windows.Forms.ToolStripMenuItem();
            this.vistaPreviaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTiempos = new System.Windows.Forms.Button();
            this.btnInterconsulta = new System.Windows.Forms.Button();
            this.btnGenerarLiquidacion = new System.Windows.Forms.Button();
            this.btnDermatologico = new System.Windows.Forms.Button();
            this.btnAdminReportes = new System.Windows.Forms.Button();
            this.btnEstudioEKG = new System.Windows.Forms.Button();
            this.btnInformeRadiologicoOIT = new System.Windows.Forms.Button();
            this.btnPruebaEsfuerzo = new System.Windows.Forms.Button();
            this.btnOsteomuscular = new System.Windows.Forms.Button();
            this.btnRadiologico = new System.Windows.Forms.Button();
            this.btnHistoriaOcupacional = new System.Windows.Forms.Button();
            this.btnOdontograma = new System.Windows.Forms.Button();
            this.btn7D = new System.Windows.Forms.Button();
            this.btnInformeMedicoTrabajador = new System.Windows.Forms.Button();
            this.btnInformeOftalmo = new System.Windows.Forms.Button();
            this.btnInformeAlturaEstructural = new System.Windows.Forms.Button();
            this.btnInformeMusculoEsqueletico = new System.Windows.Forms.Button();
            this.btnInforme312 = new System.Windows.Forms.Button();
            this.btnInformePsicologico = new System.Windows.Forms.Button();
            this.btnImprimirInformeMedicoEPS = new System.Windows.Forms.Button();
            this.btnImprimirCertificadoAptitud = new System.Windows.Forms.Button();
            this.btnEditarESO = new System.Windows.Forms.Button();
            this.btnFechaEntrega = new System.Windows.Forms.Button();
            this.btnBotonOculto = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataService)).BeginInit();
            this.cmService.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ddlConsultorio);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.btnFilter);
            this.groupBox1.Controls.Add(this.ddlStatusAptitudId);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.ddlMasterServiceId);
            this.groupBox1.Controls.Add(this.ddlCustomerOrganization);
            this.groupBox1.Controls.Add(this.ddlProtocolId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.ddlServiceTypeId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ddServiceStatusId);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtPacient);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.ddlEsoType);
            this.groupBox1.Controls.Add(this.dptDateTimeEnd);
            this.groupBox1.Controls.Add(this.dtpDateTimeStar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(1225, 97);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // ddlConsultorio
            // 
            this.ddlConsultorio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlConsultorio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlConsultorio.FormattingEnabled = true;
            this.ddlConsultorio.Location = new System.Drawing.Point(930, 67);
            this.ddlConsultorio.Margin = new System.Windows.Forms.Padding(2);
            this.ddlConsultorio.Name = "ddlConsultorio";
            this.ddlConsultorio.Size = new System.Drawing.Size(202, 21);
            this.ddlConsultorio.TabIndex = 108;
            this.ddlConsultorio.SelectedValueChanged += new System.EventHandler(this.ddlConsultorio_SelectedValueChanged);
            this.ddlConsultorio.TabIndexChanged += new System.EventHandler(this.ddlConsultorio_TabIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(867, 72);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 13);
            this.label11.TabIndex = 107;
            this.label11.Text = "Consultorio";
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.SystemColors.Control;
            this.btnFilter.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFilter.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnFilter.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ForeColor = System.Drawing.Color.Black;
            this.btnFilter.Image = global::Sigesoft.Node.WinClient.UI.Resources.find;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(1143, 65);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(2);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 24);
            this.btnFilter.TabIndex = 106;
            this.btnFilter.Text = "Filtrar";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // ddlStatusAptitudId
            // 
            this.ddlStatusAptitudId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlStatusAptitudId.Enabled = false;
            this.ddlStatusAptitudId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlStatusAptitudId.FormattingEnabled = true;
            this.ddlStatusAptitudId.Location = new System.Drawing.Point(719, 66);
            this.ddlStatusAptitudId.Margin = new System.Windows.Forms.Padding(2);
            this.ddlStatusAptitudId.Name = "ddlStatusAptitudId";
            this.ddlStatusAptitudId.Size = new System.Drawing.Size(123, 21);
            this.ddlStatusAptitudId.TabIndex = 34;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(667, 70);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Aptitud";
            // 
            // ddlMasterServiceId
            // 
            this.ddlMasterServiceId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMasterServiceId.Enabled = false;
            this.ddlMasterServiceId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlMasterServiceId.FormattingEnabled = true;
            this.ddlMasterServiceId.Location = new System.Drawing.Point(387, 43);
            this.ddlMasterServiceId.Margin = new System.Windows.Forms.Padding(2);
            this.ddlMasterServiceId.Name = "ddlMasterServiceId";
            this.ddlMasterServiceId.Size = new System.Drawing.Size(273, 21);
            this.ddlMasterServiceId.TabIndex = 32;
            this.ddlMasterServiceId.SelectedIndexChanged += new System.EventHandler(this.ddlMasterServiceId_SelectedIndexChanged_1);
            // 
            // ddlCustomerOrganization
            // 
            this.ddlCustomerOrganization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCustomerOrganization.DropDownWidth = 400;
            this.ddlCustomerOrganization.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlCustomerOrganization.FormattingEnabled = true;
            this.ddlCustomerOrganization.Location = new System.Drawing.Point(719, 16);
            this.ddlCustomerOrganization.Margin = new System.Windows.Forms.Padding(2);
            this.ddlCustomerOrganization.Name = "ddlCustomerOrganization";
            this.ddlCustomerOrganization.Size = new System.Drawing.Size(499, 21);
            this.ddlCustomerOrganization.TabIndex = 26;
            this.ddlCustomerOrganization.SelectedIndexChanged += new System.EventHandler(this.ddlOrganizationLocationId_SelectedIndexChanged);
            // 
            // ddlProtocolId
            // 
            this.ddlProtocolId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlProtocolId.Enabled = false;
            this.ddlProtocolId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlProtocolId.FormattingEnabled = true;
            this.ddlProtocolId.Location = new System.Drawing.Point(720, 41);
            this.ddlProtocolId.Margin = new System.Windows.Forms.Padding(2);
            this.ddlProtocolId.Name = "ddlProtocolId";
            this.ddlProtocolId.Size = new System.Drawing.Size(498, 21);
            this.ddlProtocolId.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(313, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Servicio";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(667, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Protocolo";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(667, 13);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 26);
            this.label10.TabIndex = 18;
            this.label10.Text = "Empresa Cliente";
            // 
            // ddlServiceTypeId
            // 
            this.ddlServiceTypeId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlServiceTypeId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlServiceTypeId.FormattingEnabled = true;
            this.ddlServiceTypeId.Location = new System.Drawing.Point(386, 18);
            this.ddlServiceTypeId.Margin = new System.Windows.Forms.Padding(2);
            this.ddlServiceTypeId.Name = "ddlServiceTypeId";
            this.ddlServiceTypeId.Size = new System.Drawing.Size(274, 21);
            this.ddlServiceTypeId.TabIndex = 30;
            this.ddlServiceTypeId.SelectedIndexChanged += new System.EventHandler(this.ddlServiceTypeId_SelectedIndexChanged_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(313, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Tipo Servicio";
            // 
            // ddServiceStatusId
            // 
            this.ddServiceStatusId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddServiceStatusId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddServiceStatusId.FormattingEnabled = true;
            this.ddServiceStatusId.Location = new System.Drawing.Point(96, 67);
            this.ddServiceStatusId.Margin = new System.Windows.Forms.Padding(2);
            this.ddServiceStatusId.Name = "ddServiceStatusId";
            this.ddServiceStatusId.Size = new System.Drawing.Size(208, 21);
            this.ddServiceStatusId.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(9, 71);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Estado Servicio";
            // 
            // txtPacient
            // 
            this.txtPacient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPacient.Location = new System.Drawing.Point(96, 42);
            this.txtPacient.Margin = new System.Windows.Forms.Padding(2);
            this.txtPacient.Name = "txtPacient";
            this.txtPacient.Size = new System.Drawing.Size(208, 20);
            this.txtPacient.TabIndex = 9;
            this.txtPacient.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPacient_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(313, 72);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Tipo E.S.O";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(9, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 28);
            this.label5.TabIndex = 8;
            this.label5.Text = "Paciente / Nro Documento";
            // 
            // ddlEsoType
            // 
            this.ddlEsoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEsoType.Enabled = false;
            this.ddlEsoType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlEsoType.FormattingEnabled = true;
            this.ddlEsoType.Location = new System.Drawing.Point(387, 68);
            this.ddlEsoType.Margin = new System.Windows.Forms.Padding(2);
            this.ddlEsoType.Name = "ddlEsoType";
            this.ddlEsoType.Size = new System.Drawing.Size(273, 21);
            this.ddlEsoType.TabIndex = 17;
            this.ddlEsoType.SelectedIndexChanged += new System.EventHandler(this.ddlEsoType_SelectedIndexChanged);
            // 
            // dptDateTimeEnd
            // 
            this.dptDateTimeEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dptDateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dptDateTimeEnd.Location = new System.Drawing.Point(209, 17);
            this.dptDateTimeEnd.Margin = new System.Windows.Forms.Padding(2);
            this.dptDateTimeEnd.Name = "dptDateTimeEnd";
            this.dptDateTimeEnd.Size = new System.Drawing.Size(95, 20);
            this.dptDateTimeEnd.TabIndex = 3;
            this.dptDateTimeEnd.Validating += new System.ComponentModel.CancelEventHandler(this.dptDateTimeEnd_Validating);
            // 
            // dtpDateTimeStar
            // 
            this.dtpDateTimeStar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDateTimeStar.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTimeStar.Location = new System.Drawing.Point(96, 17);
            this.dtpDateTimeStar.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDateTimeStar.Name = "dtpDateTimeStar";
            this.dtpDateTimeStar.Size = new System.Drawing.Size(95, 20);
            this.dtpDateTimeStar.TabIndex = 2;
            this.dtpDateTimeStar.Validating += new System.ComponentModel.CancelEventHandler(this.dtpDateTimeStar_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(194, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fecha Atención";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblRecordCountCalendar);
            this.groupBox2.Controls.Add(this.grdDataService);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox2.Location = new System.Drawing.Point(9, 111);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(1132, 498);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Lista de Servicios";
            // 
            // lblRecordCountCalendar
            // 
            this.lblRecordCountCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCountCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordCountCalendar.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblRecordCountCalendar.Location = new System.Drawing.Point(890, 8);
            this.lblRecordCountCalendar.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRecordCountCalendar.Name = "lblRecordCountCalendar";
            this.lblRecordCountCalendar.Size = new System.Drawing.Size(231, 19);
            this.lblRecordCountCalendar.TabIndex = 52;
            this.lblRecordCountCalendar.Text = "No se ha realizado la búsqueda aún.";
            this.lblRecordCountCalendar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grdDataService
            // 
            this.grdDataService.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDataService.CausesValidation = false;
            this.grdDataService.ContextMenuStrip = this.cmService;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.Silver;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdDataService.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.Caption = "Id Atención";
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.Width = 111;
            ultraGridColumn20.Header.Caption = "Usuario Crea.";
            ultraGridColumn20.Header.VisiblePosition = 11;
            ultraGridColumn20.Width = 125;
            ultraGridColumn21.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn21.Header.Caption = "Fecha Crea.";
            ultraGridColumn21.Header.VisiblePosition = 12;
            ultraGridColumn21.Width = 150;
            ultraGridColumn6.Header.Caption = "Fecha";
            ultraGridColumn6.Header.VisiblePosition = 3;
            ultraGridColumn6.Width = 74;
            ultraGridColumn22.Header.Caption = "Usuario Act.";
            ultraGridColumn22.Header.VisiblePosition = 13;
            ultraGridColumn22.Width = 125;
            ultraGridColumn23.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn23.Header.Caption = "Fecha Act.";
            ultraGridColumn23.Header.VisiblePosition = 14;
            ultraGridColumn23.Width = 150;
            ultraGridColumn3.Header.Caption = "Servicio";
            ultraGridColumn3.Header.VisiblePosition = 4;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 178;
            ultraGridColumn7.Header.Caption = "Estado Servicio";
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn8.Header.Caption = "Empresa";
            ultraGridColumn8.Header.VisiblePosition = 8;
            ultraGridColumn8.Width = 199;
            ultraGridColumn9.Header.Caption = "Sede";
            ultraGridColumn9.Header.VisiblePosition = 9;
            ultraGridColumn9.Width = 137;
            ultraGridColumn10.Header.Caption = "Tipo Servicio";
            ultraGridColumn10.Header.VisiblePosition = 5;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn4.Header.Caption = "Protocolo";
            ultraGridColumn4.Header.VisiblePosition = 10;
            ultraGridColumn4.Width = 239;
            ultraGridColumn2.Header.Caption = "Paciente";
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.Width = 234;
            ultraGridColumn5.Header.Caption = "Aptitud";
            ultraGridColumn5.Header.VisiblePosition = 7;
            appearance2.TextHAlignAsString = "Right";
            ultraGridColumn11.Header.Appearance = appearance2;
            ultraGridColumn11.Header.ToolTipText = "Pre-Liquidación";
            ultraGridColumn11.Header.VisiblePosition = 0;
            ultraGridColumn11.Width = 20;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn6,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn3,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn4,
            ultraGridColumn2,
            ultraGridColumn5,
            ultraGridColumn11});
            this.grdDataService.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDataService.DisplayLayout.InterBandSpacing = 10;
            this.grdDataService.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDataService.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDataService.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide;
            this.grdDataService.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDataService.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataService.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataService.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdDataService.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataService.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.grdDataService.DisplayLayout.Override.CardAreaAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdDataService.DisplayLayout.Override.CellAppearance = appearance4;
            this.grdDataService.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.LightGray;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderColor = System.Drawing.Color.DarkGray;
            appearance5.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDataService.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.grdDataService.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance6.AlphaLevel = ((short)(187));
            appearance6.BackColor = System.Drawing.Color.Gainsboro;
            appearance6.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForegroundAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDataService.DisplayLayout.Override.RowAlternateAppearance = appearance6;
            appearance7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdDataService.DisplayLayout.Override.RowSelectorAppearance = appearance7;
            this.grdDataService.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance8.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance8.BackColor2 = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance8.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            appearance8.BorderColor2 = System.Drawing.SystemColors.GradientActiveCaption;
            appearance8.FontData.BoldAsString = "False";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.grdDataService.DisplayLayout.Override.SelectedRowAppearance = appearance8;
            this.grdDataService.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDataService.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grdDataService.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this.grdDataService.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDataService.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDataService.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdDataService.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDataService.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.grdDataService.Location = new System.Drawing.Point(14, 28);
            this.grdDataService.Margin = new System.Windows.Forms.Padding(2);
            this.grdDataService.Name = "grdDataService";
            this.grdDataService.Size = new System.Drawing.Size(1107, 460);
            this.grdDataService.TabIndex = 44;
            this.grdDataService.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDataService_InitializeLayout);
            this.grdDataService.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdDataService_InitializeRow);
            this.grdDataService.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdDataService_AfterSelectChange);
            // 
            // cmService
            // 
            this.cmService.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CertificadoAptitud,
            this.verEditarServicioToolStripMenuItem,
            this.Examenes,
            this.vistaPreviaToolStripMenuItem});
            this.cmService.Name = "contextMenuStrip1";
            this.cmService.Size = new System.Drawing.Size(354, 92);
            this.cmService.Text = "a";
            // 
            // CertificadoAptitud
            // 
            this.CertificadoAptitud.Image = global::Sigesoft.Node.WinClient.UI.Resources.application;
            this.CertificadoAptitud.Name = "CertificadoAptitud";
            this.CertificadoAptitud.Size = new System.Drawing.Size(353, 22);
            this.CertificadoAptitud.Text = "Imprimir Certificado de Aptitud Medico Ocupacional";
            this.CertificadoAptitud.Click += new System.EventHandler(this.CertificadoAptitud_Click);
            // 
            // verEditarServicioToolStripMenuItem
            // 
            this.verEditarServicioToolStripMenuItem.Image = global::Sigesoft.Node.WinClient.UI.Resources.play_green;
            this.verEditarServicioToolStripMenuItem.Name = "verEditarServicioToolStripMenuItem";
            this.verEditarServicioToolStripMenuItem.Size = new System.Drawing.Size(353, 22);
            this.verEditarServicioToolStripMenuItem.Text = "Ver / Editar Servicio";
            // 
            // Examenes
            // 
            this.Examenes.Image = global::Sigesoft.Node.WinClient.UI.Resources.brick_go;
            this.Examenes.Name = "Examenes";
            this.Examenes.Size = new System.Drawing.Size(353, 22);
            this.Examenes.Text = "Imprimir Examenes";
            this.Examenes.Click += new System.EventHandler(this.Examenes_Click);
            // 
            // vistaPreviaToolStripMenuItem
            // 
            this.vistaPreviaToolStripMenuItem.Image = global::Sigesoft.Node.WinClient.UI.Resources.page_white_find;
            this.vistaPreviaToolStripMenuItem.Name = "vistaPreviaToolStripMenuItem";
            this.vistaPreviaToolStripMenuItem.Size = new System.Drawing.Size(353, 22);
            this.vistaPreviaToolStripMenuItem.Text = "Vista Previa";
            this.vistaPreviaToolStripMenuItem.Click += new System.EventHandler(this.vistaPreviaToolStripMenuItem_Click);
            // 
            // btnTiempos
            // 
            this.btnTiempos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTiempos.BackColor = System.Drawing.SystemColors.Control;
            this.btnTiempos.Enabled = false;
            this.btnTiempos.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnTiempos.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnTiempos.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnTiempos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTiempos.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiempos.ForeColor = System.Drawing.Color.Black;
            this.btnTiempos.Image = global::Sigesoft.Node.WinClient.UI.Resources.time;
            this.btnTiempos.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnTiempos.Location = new System.Drawing.Point(1147, 284);
            this.btnTiempos.Margin = new System.Windows.Forms.Padding(2);
            this.btnTiempos.Name = "btnTiempos";
            this.btnTiempos.Size = new System.Drawing.Size(85, 27);
            this.btnTiempos.TabIndex = 132;
            this.btnTiempos.Text = "Tiempos";
            this.btnTiempos.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTiempos.UseVisualStyleBackColor = false;
            this.btnTiempos.Click += new System.EventHandler(this.btnTiempos_Click);
            // 
            // btnInterconsulta
            // 
            this.btnInterconsulta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInterconsulta.BackColor = System.Drawing.SystemColors.Control;
            this.btnInterconsulta.Enabled = false;
            this.btnInterconsulta.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInterconsulta.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInterconsulta.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInterconsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInterconsulta.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInterconsulta.ForeColor = System.Drawing.Color.Black;
            this.btnInterconsulta.Image = global::Sigesoft.Node.WinClient.UI.Resources.note_add;
            this.btnInterconsulta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnInterconsulta.Location = new System.Drawing.Point(1147, 241);
            this.btnInterconsulta.Margin = new System.Windows.Forms.Padding(2);
            this.btnInterconsulta.Name = "btnInterconsulta";
            this.btnInterconsulta.Size = new System.Drawing.Size(85, 39);
            this.btnInterconsulta.TabIndex = 131;
            this.btnInterconsulta.Text = "Generar Interconsulta";
            this.btnInterconsulta.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnInterconsulta.UseVisualStyleBackColor = false;
            this.btnInterconsulta.Click += new System.EventHandler(this.btnInterconsulta_Click);
            // 
            // btnGenerarLiquidacion
            // 
            this.btnGenerarLiquidacion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerarLiquidacion.BackColor = System.Drawing.SystemColors.Control;
            this.btnGenerarLiquidacion.Enabled = false;
            this.btnGenerarLiquidacion.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnGenerarLiquidacion.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnGenerarLiquidacion.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnGenerarLiquidacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarLiquidacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarLiquidacion.ForeColor = System.Drawing.Color.Black;
            this.btnGenerarLiquidacion.Image = global::Sigesoft.Node.WinClient.UI.Resources.accept;
            this.btnGenerarLiquidacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerarLiquidacion.Location = new System.Drawing.Point(1147, 199);
            this.btnGenerarLiquidacion.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerarLiquidacion.Name = "btnGenerarLiquidacion";
            this.btnGenerarLiquidacion.Size = new System.Drawing.Size(85, 38);
            this.btnGenerarLiquidacion.TabIndex = 130;
            this.btnGenerarLiquidacion.Text = "&Marcar Generado";
            this.btnGenerarLiquidacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGenerarLiquidacion.UseVisualStyleBackColor = false;
            this.btnGenerarLiquidacion.Click += new System.EventHandler(this.btnGenerarLiquidacion_Click);
            // 
            // btnDermatologico
            // 
            this.btnDermatologico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDermatologico.BackColor = System.Drawing.SystemColors.Control;
            this.btnDermatologico.Enabled = false;
            this.btnDermatologico.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDermatologico.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnDermatologico.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnDermatologico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDermatologico.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDermatologico.ForeColor = System.Drawing.Color.Black;
            this.btnDermatologico.Image = ((System.Drawing.Image)(resources.GetObject("btnDermatologico.Image")));
            this.btnDermatologico.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDermatologico.Location = new System.Drawing.Point(841, 612);
            this.btnDermatologico.Margin = new System.Windows.Forms.Padding(2);
            this.btnDermatologico.Name = "btnDermatologico";
            this.btnDermatologico.Size = new System.Drawing.Size(102, 38);
            this.btnDermatologico.TabIndex = 129;
            this.btnDermatologico.Text = "&Dermatologico";
            this.btnDermatologico.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnDermatologico.UseVisualStyleBackColor = false;
            this.btnDermatologico.Visible = false;
            this.btnDermatologico.Click += new System.EventHandler(this.btnDermatologico_Click);
            // 
            // btnAdminReportes
            // 
            this.btnAdminReportes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdminReportes.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdminReportes.Enabled = false;
            this.btnAdminReportes.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnAdminReportes.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnAdminReportes.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnAdminReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminReportes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdminReportes.ForeColor = System.Drawing.Color.Black;
            this.btnAdminReportes.Image = global::Sigesoft.Node.WinClient.UI.Resources.color_swatch;
            this.btnAdminReportes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdminReportes.Location = new System.Drawing.Point(1147, 157);
            this.btnAdminReportes.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdminReportes.Name = "btnAdminReportes";
            this.btnAdminReportes.Size = new System.Drawing.Size(85, 38);
            this.btnAdminReportes.TabIndex = 128;
            this.btnAdminReportes.Text = "&Compagina  Exámenes";
            this.btnAdminReportes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnAdminReportes.UseVisualStyleBackColor = false;
            this.btnAdminReportes.Click += new System.EventHandler(this.btnConsolidadoReportes_Click);
            // 
            // btnEstudioEKG
            // 
            this.btnEstudioEKG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEstudioEKG.BackColor = System.Drawing.SystemColors.Control;
            this.btnEstudioEKG.Enabled = false;
            this.btnEstudioEKG.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnEstudioEKG.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnEstudioEKG.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnEstudioEKG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstudioEKG.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstudioEKG.ForeColor = System.Drawing.Color.Black;
            this.btnEstudioEKG.Image = ((System.Drawing.Image)(resources.GetObject("btnEstudioEKG.Image")));
            this.btnEstudioEKG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEstudioEKG.Location = new System.Drawing.Point(735, 612);
            this.btnEstudioEKG.Margin = new System.Windows.Forms.Padding(2);
            this.btnEstudioEKG.Name = "btnEstudioEKG";
            this.btnEstudioEKG.Size = new System.Drawing.Size(102, 38);
            this.btnEstudioEKG.TabIndex = 127;
            this.btnEstudioEKG.Text = "Estudio Elecgtrocardiografico";
            this.btnEstudioEKG.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnEstudioEKG.UseVisualStyleBackColor = false;
            this.btnEstudioEKG.Visible = false;
            this.btnEstudioEKG.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // btnInformeRadiologicoOIT
            // 
            this.btnInformeRadiologicoOIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInformeRadiologicoOIT.BackColor = System.Drawing.SystemColors.Control;
            this.btnInformeRadiologicoOIT.Enabled = false;
            this.btnInformeRadiologicoOIT.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInformeRadiologicoOIT.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInformeRadiologicoOIT.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInformeRadiologicoOIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformeRadiologicoOIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformeRadiologicoOIT.ForeColor = System.Drawing.Color.Black;
            this.btnInformeRadiologicoOIT.Image = ((System.Drawing.Image)(resources.GetObject("btnInformeRadiologicoOIT.Image")));
            this.btnInformeRadiologicoOIT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInformeRadiologicoOIT.Location = new System.Drawing.Point(629, 612);
            this.btnInformeRadiologicoOIT.Margin = new System.Windows.Forms.Padding(2);
            this.btnInformeRadiologicoOIT.Name = "btnInformeRadiologicoOIT";
            this.btnInformeRadiologicoOIT.Size = new System.Drawing.Size(102, 38);
            this.btnInformeRadiologicoOIT.TabIndex = 126;
            this.btnInformeRadiologicoOIT.Text = "Informe Radiografico OIT";
            this.btnInformeRadiologicoOIT.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInformeRadiologicoOIT.UseVisualStyleBackColor = false;
            this.btnInformeRadiologicoOIT.Visible = false;
            this.btnInformeRadiologicoOIT.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnPruebaEsfuerzo
            // 
            this.btnPruebaEsfuerzo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPruebaEsfuerzo.BackColor = System.Drawing.SystemColors.Control;
            this.btnPruebaEsfuerzo.Enabled = false;
            this.btnPruebaEsfuerzo.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnPruebaEsfuerzo.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnPruebaEsfuerzo.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnPruebaEsfuerzo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPruebaEsfuerzo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPruebaEsfuerzo.ForeColor = System.Drawing.Color.Black;
            this.btnPruebaEsfuerzo.Image = ((System.Drawing.Image)(resources.GetObject("btnPruebaEsfuerzo.Image")));
            this.btnPruebaEsfuerzo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPruebaEsfuerzo.Location = new System.Drawing.Point(523, 612);
            this.btnPruebaEsfuerzo.Margin = new System.Windows.Forms.Padding(2);
            this.btnPruebaEsfuerzo.Name = "btnPruebaEsfuerzo";
            this.btnPruebaEsfuerzo.Size = new System.Drawing.Size(102, 38);
            this.btnPruebaEsfuerzo.TabIndex = 125;
            this.btnPruebaEsfuerzo.Text = "Prueba Esfuerzo";
            this.btnPruebaEsfuerzo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnPruebaEsfuerzo.UseVisualStyleBackColor = false;
            this.btnPruebaEsfuerzo.Visible = false;
            this.btnPruebaEsfuerzo.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnOsteomuscular
            // 
            this.btnOsteomuscular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOsteomuscular.BackColor = System.Drawing.SystemColors.Control;
            this.btnOsteomuscular.Enabled = false;
            this.btnOsteomuscular.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOsteomuscular.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnOsteomuscular.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOsteomuscular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOsteomuscular.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOsteomuscular.ForeColor = System.Drawing.Color.Black;
            this.btnOsteomuscular.Image = ((System.Drawing.Image)(resources.GetObject("btnOsteomuscular.Image")));
            this.btnOsteomuscular.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOsteomuscular.Location = new System.Drawing.Point(416, 612);
            this.btnOsteomuscular.Margin = new System.Windows.Forms.Padding(2);
            this.btnOsteomuscular.Name = "btnOsteomuscular";
            this.btnOsteomuscular.Size = new System.Drawing.Size(102, 38);
            this.btnOsteomuscular.TabIndex = 124;
            this.btnOsteomuscular.Text = "OsteMuscular";
            this.btnOsteomuscular.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnOsteomuscular.UseVisualStyleBackColor = false;
            this.btnOsteomuscular.Visible = false;
            this.btnOsteomuscular.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnRadiologico
            // 
            this.btnRadiologico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRadiologico.BackColor = System.Drawing.SystemColors.Control;
            this.btnRadiologico.Enabled = false;
            this.btnRadiologico.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnRadiologico.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnRadiologico.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnRadiologico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRadiologico.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRadiologico.ForeColor = System.Drawing.Color.Black;
            this.btnRadiologico.Image = ((System.Drawing.Image)(resources.GetObject("btnRadiologico.Image")));
            this.btnRadiologico.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRadiologico.Location = new System.Drawing.Point(310, 612);
            this.btnRadiologico.Margin = new System.Windows.Forms.Padding(2);
            this.btnRadiologico.Name = "btnRadiologico";
            this.btnRadiologico.Size = new System.Drawing.Size(102, 38);
            this.btnRadiologico.TabIndex = 123;
            this.btnRadiologico.Text = "Radiologico";
            this.btnRadiologico.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnRadiologico.UseVisualStyleBackColor = false;
            this.btnRadiologico.Visible = false;
            this.btnRadiologico.Click += new System.EventHandler(this.btnRadiologico_Click);
            // 
            // btnHistoriaOcupacional
            // 
            this.btnHistoriaOcupacional.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistoriaOcupacional.BackColor = System.Drawing.SystemColors.Control;
            this.btnHistoriaOcupacional.Enabled = false;
            this.btnHistoriaOcupacional.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnHistoriaOcupacional.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnHistoriaOcupacional.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnHistoriaOcupacional.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistoriaOcupacional.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistoriaOcupacional.ForeColor = System.Drawing.Color.Black;
            this.btnHistoriaOcupacional.Image = ((System.Drawing.Image)(resources.GetObject("btnHistoriaOcupacional.Image")));
            this.btnHistoriaOcupacional.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistoriaOcupacional.Location = new System.Drawing.Point(204, 612);
            this.btnHistoriaOcupacional.Margin = new System.Windows.Forms.Padding(2);
            this.btnHistoriaOcupacional.Name = "btnHistoriaOcupacional";
            this.btnHistoriaOcupacional.Size = new System.Drawing.Size(102, 38);
            this.btnHistoriaOcupacional.TabIndex = 116;
            this.btnHistoriaOcupacional.Text = "Historia Ocupacional";
            this.btnHistoriaOcupacional.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnHistoriaOcupacional.UseVisualStyleBackColor = false;
            this.btnHistoriaOcupacional.Visible = false;
            this.btnHistoriaOcupacional.Click += new System.EventHandler(this.btnHistoriaOcupacional_Click);
            // 
            // btnOdontograma
            // 
            this.btnOdontograma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOdontograma.BackColor = System.Drawing.SystemColors.Control;
            this.btnOdontograma.Enabled = false;
            this.btnOdontograma.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOdontograma.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnOdontograma.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOdontograma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOdontograma.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOdontograma.ForeColor = System.Drawing.Color.Black;
            this.btnOdontograma.Image = ((System.Drawing.Image)(resources.GetObject("btnOdontograma.Image")));
            this.btnOdontograma.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOdontograma.Location = new System.Drawing.Point(98, 612);
            this.btnOdontograma.Margin = new System.Windows.Forms.Padding(2);
            this.btnOdontograma.Name = "btnOdontograma";
            this.btnOdontograma.Size = new System.Drawing.Size(102, 38);
            this.btnOdontograma.TabIndex = 115;
            this.btnOdontograma.Text = "Odontograma";
            this.btnOdontograma.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnOdontograma.UseVisualStyleBackColor = false;
            this.btnOdontograma.Visible = false;
            this.btnOdontograma.Click += new System.EventHandler(this.btnOdontograma_Click);
            // 
            // btn7D
            // 
            this.btn7D.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn7D.BackColor = System.Drawing.SystemColors.Control;
            this.btn7D.Enabled = false;
            this.btn7D.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btn7D.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn7D.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn7D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn7D.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7D.ForeColor = System.Drawing.Color.Black;
            this.btn7D.Image = ((System.Drawing.Image)(resources.GetObject("btn7D.Image")));
            this.btn7D.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn7D.Location = new System.Drawing.Point(9, 612);
            this.btn7D.Margin = new System.Windows.Forms.Padding(2);
            this.btn7D.Name = "btn7D";
            this.btn7D.Size = new System.Drawing.Size(85, 38);
            this.btn7D.TabIndex = 114;
            this.btn7D.Text = "7D";
            this.btn7D.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btn7D.UseVisualStyleBackColor = false;
            this.btn7D.Visible = false;
            this.btn7D.Click += new System.EventHandler(this.btn7D_Click);
            // 
            // btnInformeMedicoTrabajador
            // 
            this.btnInformeMedicoTrabajador.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInformeMedicoTrabajador.BackColor = System.Drawing.SystemColors.Control;
            this.btnInformeMedicoTrabajador.Enabled = false;
            this.btnInformeMedicoTrabajador.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInformeMedicoTrabajador.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInformeMedicoTrabajador.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInformeMedicoTrabajador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformeMedicoTrabajador.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformeMedicoTrabajador.ForeColor = System.Drawing.Color.Black;
            this.btnInformeMedicoTrabajador.Image = ((System.Drawing.Image)(resources.GetObject("btnInformeMedicoTrabajador.Image")));
            this.btnInformeMedicoTrabajador.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInformeMedicoTrabajador.Location = new System.Drawing.Point(1147, 368);
            this.btnInformeMedicoTrabajador.Margin = new System.Windows.Forms.Padding(2);
            this.btnInformeMedicoTrabajador.Name = "btnInformeMedicoTrabajador";
            this.btnInformeMedicoTrabajador.Size = new System.Drawing.Size(85, 52);
            this.btnInformeMedicoTrabajador.TabIndex = 113;
            this.btnInformeMedicoTrabajador.Text = "Informe Médico Trabajador";
            this.btnInformeMedicoTrabajador.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnInformeMedicoTrabajador.UseVisualStyleBackColor = false;
            this.btnInformeMedicoTrabajador.Visible = false;
            this.btnInformeMedicoTrabajador.Click += new System.EventHandler(this.btnInformeMedicoTrabajador_Click);
            // 
            // btnInformeOftalmo
            // 
            this.btnInformeOftalmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInformeOftalmo.BackColor = System.Drawing.SystemColors.Control;
            this.btnInformeOftalmo.Enabled = false;
            this.btnInformeOftalmo.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInformeOftalmo.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInformeOftalmo.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInformeOftalmo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformeOftalmo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformeOftalmo.ForeColor = System.Drawing.Color.Black;
            this.btnInformeOftalmo.Image = ((System.Drawing.Image)(resources.GetObject("btnInformeOftalmo.Image")));
            this.btnInformeOftalmo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInformeOftalmo.Location = new System.Drawing.Point(1147, 579);
            this.btnInformeOftalmo.Margin = new System.Windows.Forms.Padding(2);
            this.btnInformeOftalmo.Name = "btnInformeOftalmo";
            this.btnInformeOftalmo.Size = new System.Drawing.Size(85, 38);
            this.btnInformeOftalmo.TabIndex = 112;
            this.btnInformeOftalmo.Text = "Informe Oftalmo.";
            this.btnInformeOftalmo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInformeOftalmo.UseVisualStyleBackColor = false;
            this.btnInformeOftalmo.Visible = false;
            this.btnInformeOftalmo.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnInformeAlturaEstructural
            // 
            this.btnInformeAlturaEstructural.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInformeAlturaEstructural.BackColor = System.Drawing.SystemColors.Control;
            this.btnInformeAlturaEstructural.Enabled = false;
            this.btnInformeAlturaEstructural.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInformeAlturaEstructural.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInformeAlturaEstructural.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInformeAlturaEstructural.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformeAlturaEstructural.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformeAlturaEstructural.ForeColor = System.Drawing.Color.Black;
            this.btnInformeAlturaEstructural.Image = ((System.Drawing.Image)(resources.GetObject("btnInformeAlturaEstructural.Image")));
            this.btnInformeAlturaEstructural.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInformeAlturaEstructural.Location = new System.Drawing.Point(1147, 520);
            this.btnInformeAlturaEstructural.Margin = new System.Windows.Forms.Padding(2);
            this.btnInformeAlturaEstructural.Name = "btnInformeAlturaEstructural";
            this.btnInformeAlturaEstructural.Size = new System.Drawing.Size(85, 55);
            this.btnInformeAlturaEstructural.TabIndex = 111;
            this.btnInformeAlturaEstructural.Text = "Informe    Altura     Estructural";
            this.btnInformeAlturaEstructural.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInformeAlturaEstructural.UseVisualStyleBackColor = false;
            this.btnInformeAlturaEstructural.Visible = false;
            this.btnInformeAlturaEstructural.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnInformeMusculoEsqueletico
            // 
            this.btnInformeMusculoEsqueletico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInformeMusculoEsqueletico.BackColor = System.Drawing.SystemColors.Control;
            this.btnInformeMusculoEsqueletico.Enabled = false;
            this.btnInformeMusculoEsqueletico.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInformeMusculoEsqueletico.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInformeMusculoEsqueletico.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInformeMusculoEsqueletico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformeMusculoEsqueletico.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformeMusculoEsqueletico.ForeColor = System.Drawing.Color.Black;
            this.btnInformeMusculoEsqueletico.Image = ((System.Drawing.Image)(resources.GetObject("btnInformeMusculoEsqueletico.Image")));
            this.btnInformeMusculoEsqueletico.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInformeMusculoEsqueletico.Location = new System.Drawing.Point(1147, 466);
            this.btnInformeMusculoEsqueletico.Margin = new System.Windows.Forms.Padding(2);
            this.btnInformeMusculoEsqueletico.Name = "btnInformeMusculoEsqueletico";
            this.btnInformeMusculoEsqueletico.Size = new System.Drawing.Size(85, 50);
            this.btnInformeMusculoEsqueletico.TabIndex = 110;
            this.btnInformeMusculoEsqueletico.Text = "Informe  Musculo Esquelético";
            this.btnInformeMusculoEsqueletico.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInformeMusculoEsqueletico.UseVisualStyleBackColor = false;
            this.btnInformeMusculoEsqueletico.Visible = false;
            this.btnInformeMusculoEsqueletico.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnInforme312
            // 
            this.btnInforme312.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInforme312.BackColor = System.Drawing.SystemColors.Control;
            this.btnInforme312.Enabled = false;
            this.btnInforme312.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInforme312.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInforme312.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInforme312.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInforme312.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInforme312.ForeColor = System.Drawing.Color.Black;
            this.btnInforme312.Image = ((System.Drawing.Image)(resources.GetObject("btnInforme312.Image")));
            this.btnInforme312.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInforme312.Location = new System.Drawing.Point(1147, 424);
            this.btnInforme312.Margin = new System.Windows.Forms.Padding(2);
            this.btnInforme312.Name = "btnInforme312";
            this.btnInforme312.Size = new System.Drawing.Size(85, 38);
            this.btnInforme312.TabIndex = 109;
            this.btnInforme312.Text = "Informe       312";
            this.btnInforme312.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInforme312.UseVisualStyleBackColor = false;
            this.btnInforme312.Visible = false;
            this.btnInforme312.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnInformePsicologico
            // 
            this.btnInformePsicologico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInformePsicologico.BackColor = System.Drawing.SystemColors.Control;
            this.btnInformePsicologico.Enabled = false;
            this.btnInformePsicologico.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnInformePsicologico.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnInformePsicologico.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnInformePsicologico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformePsicologico.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformePsicologico.ForeColor = System.Drawing.Color.Black;
            this.btnInformePsicologico.Image = ((System.Drawing.Image)(resources.GetObject("btnInformePsicologico.Image")));
            this.btnInformePsicologico.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInformePsicologico.Location = new System.Drawing.Point(1147, 621);
            this.btnInformePsicologico.Margin = new System.Windows.Forms.Padding(2);
            this.btnInformePsicologico.Name = "btnInformePsicologico";
            this.btnInformePsicologico.Size = new System.Drawing.Size(85, 38);
            this.btnInformePsicologico.TabIndex = 108;
            this.btnInformePsicologico.Text = "Informe   Psicologico";
            this.btnInformePsicologico.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInformePsicologico.UseVisualStyleBackColor = false;
            this.btnInformePsicologico.Visible = false;
            this.btnInformePsicologico.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnImprimirInformeMedicoEPS
            // 
            this.btnImprimirInformeMedicoEPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImprimirInformeMedicoEPS.BackColor = System.Drawing.SystemColors.Control;
            this.btnImprimirInformeMedicoEPS.Enabled = false;
            this.btnImprimirInformeMedicoEPS.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnImprimirInformeMedicoEPS.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnImprimirInformeMedicoEPS.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnImprimirInformeMedicoEPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimirInformeMedicoEPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirInformeMedicoEPS.ForeColor = System.Drawing.Color.Black;
            this.btnImprimirInformeMedicoEPS.Image = ((System.Drawing.Image)(resources.GetObject("btnImprimirInformeMedicoEPS.Image")));
            this.btnImprimirInformeMedicoEPS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimirInformeMedicoEPS.Location = new System.Drawing.Point(1036, 614);
            this.btnImprimirInformeMedicoEPS.Margin = new System.Windows.Forms.Padding(2);
            this.btnImprimirInformeMedicoEPS.Name = "btnImprimirInformeMedicoEPS";
            this.btnImprimirInformeMedicoEPS.Size = new System.Drawing.Size(85, 36);
            this.btnImprimirInformeMedicoEPS.TabIndex = 107;
            this.btnImprimirInformeMedicoEPS.Text = "Informe             Médico   EPS";
            this.btnImprimirInformeMedicoEPS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnImprimirInformeMedicoEPS.UseVisualStyleBackColor = false;
            this.btnImprimirInformeMedicoEPS.Visible = false;
            this.btnImprimirInformeMedicoEPS.Click += new System.EventHandler(this.btnImprimirInformeMedicoEPS_Click);
            // 
            // btnImprimirCertificadoAptitud
            // 
            this.btnImprimirCertificadoAptitud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImprimirCertificadoAptitud.BackColor = System.Drawing.SystemColors.Control;
            this.btnImprimirCertificadoAptitud.Enabled = false;
            this.btnImprimirCertificadoAptitud.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnImprimirCertificadoAptitud.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnImprimirCertificadoAptitud.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnImprimirCertificadoAptitud.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimirCertificadoAptitud.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirCertificadoAptitud.ForeColor = System.Drawing.Color.Black;
            this.btnImprimirCertificadoAptitud.Image = ((System.Drawing.Image)(resources.GetObject("btnImprimirCertificadoAptitud.Image")));
            this.btnImprimirCertificadoAptitud.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimirCertificadoAptitud.Location = new System.Drawing.Point(947, 614);
            this.btnImprimirCertificadoAptitud.Margin = new System.Windows.Forms.Padding(2);
            this.btnImprimirCertificadoAptitud.Name = "btnImprimirCertificadoAptitud";
            this.btnImprimirCertificadoAptitud.Size = new System.Drawing.Size(85, 36);
            this.btnImprimirCertificadoAptitud.TabIndex = 104;
            this.btnImprimirCertificadoAptitud.Text = "Cerificado Aptitud";
            this.btnImprimirCertificadoAptitud.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnImprimirCertificadoAptitud.UseVisualStyleBackColor = false;
            this.btnImprimirCertificadoAptitud.Visible = false;
            this.btnImprimirCertificadoAptitud.Click += new System.EventHandler(this.btnImprimirCertificadoAptitud_Click);
            // 
            // btnEditarESO
            // 
            this.btnEditarESO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditarESO.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditarESO.Enabled = false;
            this.btnEditarESO.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnEditarESO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnEditarESO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnEditarESO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditarESO.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditarESO.ForeColor = System.Drawing.Color.Black;
            this.btnEditarESO.Image = global::Sigesoft.Node.WinClient.UI.Resources.application_edit;
            this.btnEditarESO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditarESO.Location = new System.Drawing.Point(1147, 117);
            this.btnEditarESO.Margin = new System.Windows.Forms.Padding(2);
            this.btnEditarESO.Name = "btnEditarESO";
            this.btnEditarESO.Size = new System.Drawing.Size(85, 36);
            this.btnEditarESO.TabIndex = 50;
            this.btnEditarESO.Text = "    &Ver / Editar Servicio";
            this.btnEditarESO.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnEditarESO.UseVisualStyleBackColor = false;
            this.btnEditarESO.Click += new System.EventHandler(this.btnEditarESO_Click);
            // 
            // btnFechaEntrega
            // 
            this.btnFechaEntrega.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechaEntrega.BackColor = System.Drawing.SystemColors.Control;
            this.btnFechaEntrega.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFechaEntrega.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnFechaEntrega.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnFechaEntrega.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechaEntrega.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechaEntrega.ForeColor = System.Drawing.Color.Black;
            this.btnFechaEntrega.Image = global::Sigesoft.Node.WinClient.UI.Resources.printer_go;
            this.btnFechaEntrega.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFechaEntrega.Location = new System.Drawing.Point(1147, 315);
            this.btnFechaEntrega.Margin = new System.Windows.Forms.Padding(2);
            this.btnFechaEntrega.Name = "btnFechaEntrega";
            this.btnFechaEntrega.Size = new System.Drawing.Size(85, 38);
            this.btnFechaEntrega.TabIndex = 135;
            this.btnFechaEntrega.Text = "&Fecha de Entrega";
            this.btnFechaEntrega.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFechaEntrega.UseVisualStyleBackColor = false;
            this.btnFechaEntrega.Click += new System.EventHandler(this.btnFechaEntrega_Click);
            // 
            // btnBotonOculto
            // 
            this.btnBotonOculto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBotonOculto.BackColor = System.Drawing.SystemColors.Control;
            this.btnBotonOculto.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBotonOculto.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnBotonOculto.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnBotonOculto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBotonOculto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBotonOculto.ForeColor = System.Drawing.Color.Black;
            this.btnBotonOculto.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.btnBotonOculto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBotonOculto.Location = new System.Drawing.Point(1036, 612);
            this.btnBotonOculto.Margin = new System.Windows.Forms.Padding(2);
            this.btnBotonOculto.Name = "btnBotonOculto";
            this.btnBotonOculto.Size = new System.Drawing.Size(85, 36);
            this.btnBotonOculto.TabIndex = 147;
            this.btnBotonOculto.Text = "botón Oculto";
            this.btnBotonOculto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBotonOculto.UseVisualStyleBackColor = false;
            this.btnBotonOculto.Click += new System.EventHandler(this.btnBotonOculto_Click);
            // 
            // frmService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1238, 660);
            this.Controls.Add(this.btnBotonOculto);
            this.Controls.Add(this.btnFechaEntrega);
            this.Controls.Add(this.btnTiempos);
            this.Controls.Add(this.btnInterconsulta);
            this.Controls.Add(this.btnGenerarLiquidacion);
            this.Controls.Add(this.btnDermatologico);
            this.Controls.Add(this.btnAdminReportes);
            this.Controls.Add(this.btnEstudioEKG);
            this.Controls.Add(this.btnInformeRadiologicoOIT);
            this.Controls.Add(this.btnPruebaEsfuerzo);
            this.Controls.Add(this.btnOsteomuscular);
            this.Controls.Add(this.btnRadiologico);
            this.Controls.Add(this.btnHistoriaOcupacional);
            this.Controls.Add(this.btnOdontograma);
            this.Controls.Add(this.btn7D);
            this.Controls.Add(this.btnInformeMedicoTrabajador);
            this.Controls.Add(this.btnInformeOftalmo);
            this.Controls.Add(this.btnInformeAlturaEstructural);
            this.Controls.Add(this.btnInformeMusculoEsqueletico);
            this.Controls.Add(this.btnInforme312);
            this.Controls.Add(this.btnInformePsicologico);
            this.Controls.Add(this.btnImprimirInformeMedicoEPS);
            this.Controls.Add(this.btnImprimirCertificadoAptitud);
            this.Controls.Add(this.btnEditarESO);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmService";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrador de Servicios";
            this.Load += new System.EventHandler(this.frmService_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDataService)).EndInit();
            this.cmService.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ddlEsoType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ddServiceStatusId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPacient;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dptDateTimeEnd;
        private System.Windows.Forms.DateTimePicker dtpDateTimeStar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDataService;
        private System.Windows.Forms.Label lblRecordCountCalendar;
        private System.Windows.Forms.ComboBox ddlMasterServiceId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlServiceTypeId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip cmService;
        private System.Windows.Forms.ToolStripMenuItem CertificadoAptitud;
        private System.Windows.Forms.ToolStripMenuItem Examenes;
        private System.Windows.Forms.ToolStripMenuItem verEditarServicioToolStripMenuItem;

        private System.Windows.Forms.Button btnEditarESO;
        private System.Windows.Forms.Button btnImprimirCertificadoAptitud;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ToolStripMenuItem vistaPreviaToolStripMenuItem;
        private System.Windows.Forms.ComboBox ddlStatusAptitudId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ddlCustomerOrganization;
        private System.Windows.Forms.ComboBox ddlProtocolId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnImprimirInformeMedicoEPS;
        private System.Windows.Forms.Button btnInformePsicologico;
        private System.Windows.Forms.Button btnInforme312;
        private System.Windows.Forms.Button btnInformeMusculoEsqueletico;
        private System.Windows.Forms.Button btnInformeAlturaEstructural;
        private System.Windows.Forms.Button btnInformeOftalmo;
        private System.Windows.Forms.Button btnInformeMedicoTrabajador;
        private System.Windows.Forms.Button btn7D;
        private System.Windows.Forms.Button btnOdontograma;
        private System.Windows.Forms.Button btnHistoriaOcupacional;
        private System.Windows.Forms.Button btnRadiologico;
        private System.Windows.Forms.Button btnOsteomuscular;
        private System.Windows.Forms.Button btnPruebaEsfuerzo;
        private System.Windows.Forms.Button btnInformeRadiologicoOIT;
        private System.Windows.Forms.Button btnEstudioEKG;
        private System.Windows.Forms.Button btnAdminReportes;
        private System.Windows.Forms.Button btnDermatologico;
        private System.Windows.Forms.Button btnGenerarLiquidacion;
        private System.Windows.Forms.ComboBox ddlConsultorio;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnInterconsulta;
        private System.Windows.Forms.Button btnTiempos;
        private System.Windows.Forms.Button btnFechaEntrega;
        private System.Windows.Forms.Button btnBotonOculto;
    }
}