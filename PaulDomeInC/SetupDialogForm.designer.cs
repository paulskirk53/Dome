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
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.LBLcontrolBox = new System.Windows.Forms.Label();
            this.comboboxcontrol_box = new System.Windows.Forms.ComboBox();
            this.numericUpDownParkAzimuth = new System.Windows.Forms.NumericUpDown();
            this.lblparkazimuth = new System.Windows.Forms.Label();
            this.LBLShutter = new System.Windows.Forms.Label();
            this.comboBoxComPortShutter = new System.Windows.Forms.ComboBox();
            this.numericUpDownHomeAzimuth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.BTNidcomports = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParkAzimuth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHomeAzimuth)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(465, 239);
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
            this.cmdCancel.Location = new System.Drawing.Point(465, 269);
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
            this.label1.Size = new System.Drawing.Size(458, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = "Driver settings for Azimuth and stepper ports.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.GowerCDome.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(476, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
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
            // LBLcontrolBox
            // 
            this.LBLcontrolBox.AutoSize = true;
            this.LBLcontrolBox.Location = new System.Drawing.Point(16, 138);
            this.LBLcontrolBox.Name = "LBLcontrolBox";
            this.LBLcontrolBox.Size = new System.Drawing.Size(139, 13);
            this.LBLcontrolBox.TabIndex = 8;
            this.LBLcontrolBox.Text = "Dome drive Control Box  on ";
            // 
            // comboboxcontrol_box
            // 
            this.comboboxcontrol_box.FormattingEnabled = true;
            this.comboboxcontrol_box.Location = new System.Drawing.Point(259, 138);
            this.comboboxcontrol_box.Name = "comboboxcontrol_box";
            this.comboboxcontrol_box.Size = new System.Drawing.Size(88, 21);
            this.comboboxcontrol_box.TabIndex = 9;
            this.comboboxcontrol_box.Text = "Choose Port";
            // 
            // numericUpDownParkAzimuth
            // 
            this.numericUpDownParkAzimuth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownParkAzimuth.Location = new System.Drawing.Point(201, 183);
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
            // 
            // lblparkazimuth
            // 
            this.lblparkazimuth.AutoSize = true;
            this.lblparkazimuth.Location = new System.Drawing.Point(56, 186);
            this.lblparkazimuth.Name = "lblparkazimuth";
            this.lblparkazimuth.Size = new System.Drawing.Size(66, 13);
            this.lblparkazimuth.TabIndex = 11;
            this.lblparkazimuth.Text = "ParkAzimuth";
            // 
            // LBLShutter
            // 
            this.LBLShutter.AutoSize = true;
            this.LBLShutter.Location = new System.Drawing.Point(16, 87);
            this.LBLShutter.Name = "LBLShutter";
            this.LBLShutter.Size = new System.Drawing.Size(117, 13);
            this.LBLShutter.TabIndex = 12;
            this.LBLShutter.Text = "Shutter Radio on          ";
            // 
            // comboBoxComPortShutter
            // 
            this.comboBoxComPortShutter.FormattingEnabled = true;
            this.comboBoxComPortShutter.Location = new System.Drawing.Point(259, 84);
            this.comboBoxComPortShutter.Name = "comboBoxComPortShutter";
            this.comboBoxComPortShutter.Size = new System.Drawing.Size(88, 21);
            this.comboBoxComPortShutter.TabIndex = 13;
            this.comboBoxComPortShutter.Text = "Choose Port";
            // 
            // numericUpDownHomeAzimuth
            // 
            this.numericUpDownHomeAzimuth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownHomeAzimuth.Location = new System.Drawing.Point(201, 215);
            this.numericUpDownHomeAzimuth.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownHomeAzimuth.Name = "numericUpDownHomeAzimuth";
            this.numericUpDownHomeAzimuth.Size = new System.Drawing.Size(85, 20);
            this.numericUpDownHomeAzimuth.TabIndex = 14;
            this.numericUpDownHomeAzimuth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownHomeAzimuth.Value = new decimal(new int[] {
            262,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Home Position";
            // 
            // BTNidcomports
            // 
            this.BTNidcomports.Location = new System.Drawing.Point(409, 129);
            this.BTNidcomports.Name = "BTNidcomports";
            this.BTNidcomports.Size = new System.Drawing.Size(114, 57);
            this.BTNidcomports.TabIndex = 17;
            this.BTNidcomports.Text = "Identify COM Ports";
            this.BTNidcomports.UseVisualStyleBackColor = true;
            this.BTNidcomports.Click += new System.EventHandler(this.BTNidcomports_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 302);
            this.Controls.Add(this.BTNidcomports);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownHomeAzimuth);
            this.Controls.Add(this.comboBoxComPortShutter);
            this.Controls.Add(this.LBLShutter);
            this.Controls.Add(this.lblparkazimuth);
            this.Controls.Add(this.numericUpDownParkAzimuth);
            this.Controls.Add(this.comboboxcontrol_box);
            this.Controls.Add(this.LBLcontrolBox);
            this.Controls.Add(this.chkTrace);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHomeAzimuth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Label LBLcontrolBox;
        private System.Windows.Forms.ComboBox comboboxcontrol_box;
        private System.Windows.Forms.NumericUpDown numericUpDownParkAzimuth;
        private System.Windows.Forms.Label lblparkazimuth;
        private System.Windows.Forms.Label LBLShutter;
        private System.Windows.Forms.ComboBox comboBoxComPortShutter;
        private System.Windows.Forms.NumericUpDown numericUpDownHomeAzimuth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BTNidcomports;
    }
}