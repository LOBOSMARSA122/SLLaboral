namespace Sigesoft.Node.Sync.UI
{
    partial class frmInitial
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
            this.txtInitialLog = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnAddDeploymentFile = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // txtInitialLog
            // 
            this.txtInitialLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInitialLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInitialLog.Location = new System.Drawing.Point(12, 12);
            this.txtInitialLog.Multiline = true;
            this.txtInitialLog.Name = "txtInitialLog";
            this.txtInitialLog.ReadOnly = true;
            this.txtInitialLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInitialLog.Size = new System.Drawing.Size(706, 288);
            this.txtInitialLog.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(609, 306);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(109, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Salir";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetry.Location = new System.Drawing.Point(12, 306);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(109, 28);
            this.btnRetry.TabIndex = 2;
            this.btnRetry.Text = "Reintentar";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Visible = false;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnAddDeploymentFile
            // 
            this.btnAddDeploymentFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDeploymentFile.Location = new System.Drawing.Point(143, 306);
            this.btnAddDeploymentFile.Name = "btnAddDeploymentFile";
            this.btnAddDeploymentFile.Size = new System.Drawing.Size(171, 28);
            this.btnAddDeploymentFile.TabIndex = 3;
            this.btnAddDeploymentFile.Text = "Establecer Deployment";
            this.btnAddDeploymentFile.UseVisualStyleBackColor = true;
            this.btnAddDeploymentFile.Visible = false;
            this.btnAddDeploymentFile.Click += new System.EventHandler(this.btnAddDeploymentFile_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // frmInitial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 341);
            this.ControlBox = false;
            this.Controls.Add(this.btnAddDeploymentFile);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtInitialLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmInitial";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sincronización - Verificación Inicial";
            this.Load += new System.EventHandler(this.frmInitial_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInitialLog;
        private System.Windows.Forms.Button btnClose;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Button btnAddDeploymentFile;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}