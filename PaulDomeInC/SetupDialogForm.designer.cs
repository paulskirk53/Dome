namespace ASCOM.GowerCDome
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxComPortStepper = new System.Windows.Forms.ComboBox();
            this.numericUpDownParkAzimuth = new System.Windows.Forms.NumericUpDown();
            this.lblparkazimuth = new System.Windows.Forms.Label();
            this.lblshutter = new System.Windows.Forms.Label();
            this.comboBoxComPortShutter = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParkAzimuth)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(347, 207);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(347, 237);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Driver settings for compass and stepper ports.";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.GowerCDome.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(358, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Comm Port for Compass";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(12, 237);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.DropDownWidth = 90;
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(168, 90);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(90, 21);
            this.comboBoxComPort.TabIndex = 7;
            this.comboBoxComPort.Text = " ";
            this.comboBoxComPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPort_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Com Port for Stepper";
            // 
            // comboBoxComPortStepper
            // 
            this.comboBoxComPortStepper.FormattingEnabled = true;
            this.comboBoxComPortStepper.Location = new System.Drawing.Point(169, 140);
            this.comboBoxComPortStepper.Name = "comboBoxComPortStepper";
            this.comboBoxComPortStepper.Size = new System.Drawing.Size(88, 21);
            this.comboBoxComPortStepper.TabIndex = 9;
            // 
            // numericUpDownParkAzimuth
            // 
            this.numericUpDownParkAzimuth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownParkAzimuth.Location = new System.Drawing.Point(173, 186);
            this.numericUpDownParkAzimuth.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownParkAzimuth.Name = "numericUpDownParkAzimuth";
            this.numericUpDownParkAzimuth.Size = new System.Drawing.Size(85, 20);
            this.numericUpDownParkAzimuth.TabIndex = 10;
            this.numericUpDownParkAzimuth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownParkAzimuth.Value = new decimal(new int[] {
            261,
            0,
            0,
            0});
            this.numericUpDownParkAzimuth.ValueChanged += new System.EventHandler(this.numericUpDownParkAzimuth_ValueChanged);
            // 
            // lblparkazimuth
            // 
            this.lblparkazimuth.AutoSize = true;
            this.lblparkazimuth.Location = new System.Drawing.Point(25, 183);
            this.lblparkazimuth.Name = "lblparkazimuth";
            this.lblparkazimuth.Size = new System.Drawing.Size(66, 13);
            this.lblparkazimuth.TabIndex = 11;
            this.lblparkazimuth.Text = "ParkAzimuth";
            this.lblparkazimuth.Visible = false;
            // 
            // lblshutter
            // 
            this.lblshutter.AutoSize = true;
            this.lblshutter.Location = new System.Drawing.Point(16, 119);
            this.lblshutter.Name = "lblshutter";
            this.lblshutter.Size = new System.Drawing.Size(110, 13);
            this.lblshutter.TabIndex = 12;
            this.lblshutter.Text = "Comm Port for Shutter";
            // 
            // comboBoxComPortShutter
            // 
            this.comboBoxComPortShutter.FormattingEnabled = true;
            this.comboBoxComPortShutter.Location = new System.Drawing.Point(169, 115);
            this.comboBoxComPortShutter.Name = "comboBoxComPortShutter";
            this.comboBoxComPortShutter.Size = new System.Drawing.Size(88, 21);
            this.comboBoxComPortShutter.TabIndex = 13;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 270);
            this.Controls.Add(this.comboBoxComPortShutter);
            this.Controls.Add(this.lblshutter);
            this.Controls.Add(this.lblparkazimuth);
            this.Controls.Add(this.numericUpDownParkAzimuth);
            this.Controls.Add(this.comboBoxComPortStepper);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gower Dome Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParkAzimuth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxComPortStepper;
        private System.Windows.Forms.NumericUpDown numericUpDownParkAzimuth;
        private System.Windows.Forms.Label lblparkazimuth;
        private System.Windows.Forms.Label lblshutter;
        private System.Windows.Forms.ComboBox comboBoxComPortShutter;
    }
}