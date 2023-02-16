namespace  HidDemoWindowsForms
{
    partial class MovContCLK
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
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonParo = new System.Windows.Forms.RadioButton();
            this.radioButtonMarcha = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxSentidoGiro = new System.Windows.Forms.GroupBox();
            this.radioButtonCW = new System.Windows.Forms.RadioButton();
            this.radioButtonCCW = new System.Windows.Forms.RadioButton();
            this.textBoxFrecuenciaCLK = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxStepResolutionMode = new System.Windows.Forms.ListBox();
            this.AnalogBarMovCont = new System.Windows.Forms.ProgressBar();
            this.TimerMovCont = new System.Windows.Forms.Timer(this.components);
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.groupBox1.SuspendLayout();
            this.groupBoxSentidoGiro.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "ToggleLED4";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButtonParo
            // 
            this.radioButtonParo.AutoSize = true;
            this.radioButtonParo.Checked = true;
            this.radioButtonParo.Location = new System.Drawing.Point(22, 28);
            this.radioButtonParo.Name = "radioButtonParo";
            this.radioButtonParo.Size = new System.Drawing.Size(47, 17);
            this.radioButtonParo.TabIndex = 1;
            this.radioButtonParo.TabStop = true;
            this.radioButtonParo.Text = "Paro";
            this.radioButtonParo.UseVisualStyleBackColor = true;
            this.radioButtonParo.CheckedChanged += new System.EventHandler(this.radioButtonParo_CheckedChanged);
            // 
            // radioButtonMarcha
            // 
            this.radioButtonMarcha.AutoSize = true;
            this.radioButtonMarcha.Location = new System.Drawing.Point(22, 60);
            this.radioButtonMarcha.Name = "radioButtonMarcha";
            this.radioButtonMarcha.Size = new System.Drawing.Size(61, 17);
            this.radioButtonMarcha.TabIndex = 2;
            this.radioButtonMarcha.Text = "Marcha";
            this.radioButtonMarcha.UseVisualStyleBackColor = true;
            this.radioButtonMarcha.CheckedChanged += new System.EventHandler(this.radioButtonMarcha_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonParo);
            this.groupBox1.Controls.Add(this.radioButtonMarcha);
            this.groupBox1.Location = new System.Drawing.Point(46, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(109, 85);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paro/Marcha";
            // 
            // groupBoxSentidoGiro
            // 
            this.groupBoxSentidoGiro.Controls.Add(this.radioButtonCW);
            this.groupBoxSentidoGiro.Controls.Add(this.radioButtonCCW);
            this.groupBoxSentidoGiro.Location = new System.Drawing.Point(202, 25);
            this.groupBoxSentidoGiro.Name = "groupBoxSentidoGiro";
            this.groupBoxSentidoGiro.Size = new System.Drawing.Size(109, 85);
            this.groupBoxSentidoGiro.TabIndex = 4;
            this.groupBoxSentidoGiro.TabStop = false;
            this.groupBoxSentidoGiro.Text = "Sentido de Giro";
            // 
            // radioButtonCW
            // 
            this.radioButtonCW.AutoSize = true;
            this.radioButtonCW.Checked = true;
            this.radioButtonCW.Location = new System.Drawing.Point(22, 28);
            this.radioButtonCW.Name = "radioButtonCW";
            this.radioButtonCW.Size = new System.Drawing.Size(43, 17);
            this.radioButtonCW.TabIndex = 1;
            this.radioButtonCW.TabStop = true;
            this.radioButtonCW.Text = "CW";
            this.radioButtonCW.UseVisualStyleBackColor = true;
            // 
            // radioButtonCCW
            // 
            this.radioButtonCCW.AutoSize = true;
            this.radioButtonCCW.Location = new System.Drawing.Point(22, 60);
            this.radioButtonCCW.Name = "radioButtonCCW";
            this.radioButtonCCW.Size = new System.Drawing.Size(50, 17);
            this.radioButtonCCW.TabIndex = 2;
            this.radioButtonCCW.Text = "CCW";
            this.radioButtonCCW.UseVisualStyleBackColor = true;
            // 
            // textBoxFrecuenciaCLK
            // 
            this.textBoxFrecuenciaCLK.Location = new System.Drawing.Point(161, 149);
            this.textBoxFrecuenciaCLK.Name = "textBoxFrecuenciaCLK";
            this.textBoxFrecuenciaCLK.Size = new System.Drawing.Size(100, 20);
            this.textBoxFrecuenciaCLK.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Frecuencia CLK (Hz)";
            // 
            // StatusText
            // 
            this.StatusText.AutoSize = true;
            this.StatusText.Location = new System.Drawing.Point(411, 33);
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(216, 13);
            this.StatusText.TabIndex = 7;
            this.StatusText.Text = "18F4550 Connection Status: Not connected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(411, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Comunicación USB";
            // 
            // listBoxStepResolutionMode
            // 
            this.listBoxStepResolutionMode.AllowDrop = true;
            this.listBoxStepResolutionMode.FormattingEnabled = true;
            this.listBoxStepResolutionMode.Items.AddRange(new object[] {
            "STEP_RESOLUTION_FIXED_2",
            "STEP_RESOLUTION_FIXED_4",
            "STEP_RESOLUTION_FIXED_8",
            "STEP_RESOLUTION_FIXED_16",
            "STEP_RESOLUTION_FIXED_32",
            "STEP_RESOLUTION_FIXED_64",
            "STEP_RESOLUTION_FIXED_128",
            "STEP_RESOLUTION_FIXED_FULL",
            "STEP_RESOLUTION_VARIABLE_1_2",
            "STEP_RESOLUTION_VARIABLE_1_4",
            "STEP_RESOLUTION_VARIABLE_1_8",
            "STEP_RESOLUTION_VARIABLE_1_16",
            "STEP_RESOLUTION_VARIABLE_1_32",
            "STEP_RESOLUTION_VARIABLE_1_64",
            "STEP_RESOLUTION_VARIABLE_1_128"});
            this.listBoxStepResolutionMode.Location = new System.Drawing.Point(533, 53);
            this.listBoxStepResolutionMode.Margin = new System.Windows.Forms.Padding(6);
            this.listBoxStepResolutionMode.Name = "listBoxStepResolutionMode";
            this.listBoxStepResolutionMode.Size = new System.Drawing.Size(240, 199);
            this.listBoxStepResolutionMode.TabIndex = 9;
            this.listBoxStepResolutionMode.DoubleClick += new System.EventHandler(this.stepResolutionDoubleClick);
            // 
            // AnalogBarMovCont
            // 
            this.AnalogBarMovCont.BackColor = System.Drawing.Color.White;
            this.AnalogBarMovCont.Location = new System.Drawing.Point(334, 76);
            this.AnalogBarMovCont.Maximum = 1024;
            this.AnalogBarMovCont.Name = "AnalogBarMovCont";
            this.AnalogBarMovCont.Size = new System.Drawing.Size(192, 26);
            this.AnalogBarMovCont.Step = 1;
            this.AnalogBarMovCont.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.AnalogBarMovCont.TabIndex = 20;
            // 
            // TimerMovCont
            // 
            this.TimerMovCont.Enabled = true;
            this.TimerMovCont.Interval = 6;
            this.TimerMovCont.Tick += new System.EventHandler(this.FormUpdateTimer_Tick);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 239);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(491, 242);
            this.zedGraphControl1.TabIndex = 21;
            // 
            // MovContCLK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 493);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.AnalogBarMovCont);
            this.Controls.Add(this.listBoxStepResolutionMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StatusText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFrecuenciaCLK);
            this.Controls.Add(this.groupBoxSentidoGiro);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MovContCLK";
            this.Text = "MovContCLK";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxSentidoGiro.ResumeLayout(false);
            this.groupBoxSentidoGiro.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonParo;
        private System.Windows.Forms.RadioButton radioButtonMarcha;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxSentidoGiro;
        private System.Windows.Forms.RadioButton radioButtonCW;
        private System.Windows.Forms.RadioButton radioButtonCCW;
        private System.Windows.Forms.TextBox textBoxFrecuenciaCLK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label StatusText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxStepResolutionMode;
        private System.Windows.Forms.ProgressBar AnalogBarMovCont;
        private System.Windows.Forms.Timer TimerMovCont;
        private ZedGraph.ZedGraphControl zedGraphControl1;
    }
}