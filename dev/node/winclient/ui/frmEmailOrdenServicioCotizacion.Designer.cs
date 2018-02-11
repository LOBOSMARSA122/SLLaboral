namespace Sigesoft.Node.WinClient.UI
{
    partial class frmEmailOrdenServicioCotizacion
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ultraValidator1 = new Infragistics.Win.Misc.UltraValidator(this.components);
            this.rbOrdenServicio = new System.Windows.Forms.RadioButton();
            this.rbCotizacion = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraValidator1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(459, 323);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 30);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Image = global::Sigesoft.Node.WinClient.UI.Resources.accept;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(382, 323);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(73, 30);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "Enviar";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtBody
            // 
            this.txtBody.Location = new System.Drawing.Point(58, 117);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(476, 195);
            this.txtBody.TabIndex = 24;
            this.txtBody.Text = "Se adjunta Cotización / Orden de Servicio, para las atenciones Médico Ocupacional" +
    "es en nuestra Clínica especializada en Salud Ocupacional";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(58, 91);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(476, 20);
            this.txtSubject.TabIndex = 23;
            this.txtSubject.Text = "Envío de Cotización / Orden de Servicio, Saludo Ocupacional:Salud";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(58, 65);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(476, 20);
            this.txtFrom.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Cuerpo : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Asunto :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Para :";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // rbOrdenServicio
            // 
            this.rbOrdenServicio.AutoSize = true;
            this.rbOrdenServicio.Checked = true;
            this.rbOrdenServicio.Location = new System.Drawing.Point(6, 19);
            this.rbOrdenServicio.Name = "rbOrdenServicio";
            this.rbOrdenServicio.Size = new System.Drawing.Size(110, 17);
            this.rbOrdenServicio.TabIndex = 25;
            this.rbOrdenServicio.TabStop = true;
            this.rbOrdenServicio.Text = "Orden de Servicio";
            this.rbOrdenServicio.UseVisualStyleBackColor = true;
            this.rbOrdenServicio.CheckedChanged += new System.EventHandler(this.rbOrdenServicio_CheckedChanged);
            // 
            // rbCotizacion
            // 
            this.rbCotizacion.AutoSize = true;
            this.rbCotizacion.Location = new System.Drawing.Point(122, 19);
            this.rbCotizacion.Name = "rbCotizacion";
            this.rbCotizacion.Size = new System.Drawing.Size(74, 17);
            this.rbCotizacion.TabIndex = 26;
            this.rbCotizacion.TabStop = true;
            this.rbCotizacion.Text = "Cotización";
            this.rbCotizacion.UseVisualStyleBackColor = true;
            this.rbCotizacion.CheckedChanged += new System.EventHandler(this.rbCotizacion_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOrdenServicio);
            this.groupBox1.Controls.Add(this.rbCotizacion);
            this.groupBox1.Location = new System.Drawing.Point(58, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 46);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Seleccionar archivo adjunto";
            // 
            // frmEmailOrdenServicioCotizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 364);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtBody);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmailOrdenServicioCotizacion";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enviar Email";
            this.Load += new System.EventHandler(this.frmEmailOrdenServicioCotizacion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraValidator1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Infragistics.Win.Misc.UltraValidator ultraValidator1;
        private System.Windows.Forms.RadioButton rbOrdenServicio;
        private System.Windows.Forms.RadioButton rbCotizacion;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}