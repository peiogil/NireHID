namespace USB_HID_con_la_PICDEM_FSUSB_18F4550
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
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonParo = new System.Windows.Forms.RadioButton();
            this.radioButtonMarcha = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxSentidoGiro = new System.Windows.Forms.GroupBox();
            this.radioButtonCW = new System.Windows.Forms.RadioButton();
            this.radioButtonCCW = new System.Windows.Forms.RadioButton();
            this.textBoxFrecuenciaCLK = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBoxSentidoGiro.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "ToggleLED2_MovContCLK";
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
            // MovContCLK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFrecuenciaCLK);
            this.Controls.Add(this.groupBoxSentidoGiro);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
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
    }
}