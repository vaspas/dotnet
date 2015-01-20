namespace ComparativeTapeTest
{
    partial class Main
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
            this.cbWindowType = new System.Windows.Forms.ComboBox();
            this.bOpen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTapeType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudPeriod = new System.Windows.Forms.NumericUpDown();
            this.nudShift = new System.Windows.Forms.NumericUpDown();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShift)).BeginInit();
            this.SuspendLayout();
            // 
            // cbWindowType
            // 
            this.cbWindowType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbWindowType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWindowType.FormattingEnabled = true;
            this.cbWindowType.Location = new System.Drawing.Point(12, 25);
            this.cbWindowType.Name = "cbWindowType";
            this.cbWindowType.Size = new System.Drawing.Size(365, 21);
            this.cbWindowType.TabIndex = 6;
            // 
            // bOpen
            // 
            this.bOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOpen.Location = new System.Drawing.Point(302, 92);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(75, 23);
            this.bOpen.TabIndex = 7;
            this.bOpen.Text = "Open";
            this.bOpen.UseVisualStyleBackColor = true;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Представление";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Тип ленты";
            // 
            // cbTapeType
            // 
            this.cbTapeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTapeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTapeType.FormattingEnabled = true;
            this.cbTapeType.Location = new System.Drawing.Point(12, 65);
            this.cbTapeType.Name = "cbTapeType";
            this.cbTapeType.Size = new System.Drawing.Size(365, 21);
            this.cbTapeType.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.hScrollBar);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudPeriod);
            this.groupBox1.Controls.Add(this.nudShift);
            this.groupBox1.Location = new System.Drawing.Point(15, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 100);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "player";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Период(мс)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Смещение";
            // 
            // nudPeriod
            // 
            this.nudPeriod.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudPeriod.Location = new System.Drawing.Point(90, 51);
            this.nudPeriod.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPeriod.Name = "nudPeriod";
            this.nudPeriod.Size = new System.Drawing.Size(120, 20);
            this.nudPeriod.TabIndex = 5;
            this.nudPeriod.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudPeriod.ValueChanged += new System.EventHandler(this.nudPeriod_ValueChanged);
            // 
            // nudShift
            // 
            this.nudShift.DecimalPlaces = 3;
            this.nudShift.Location = new System.Drawing.Point(90, 19);
            this.nudShift.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudShift.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudShift.Name = "nudShift";
            this.nudShift.Size = new System.Drawing.Size(120, 20);
            this.nudShift.TabIndex = 4;
            this.nudShift.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.nudShift.ValueChanged += new System.EventHandler(this.nudShift_ValueChanged);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar.Location = new System.Drawing.Point(6, 74);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(356, 17);
            this.hScrollBar.TabIndex = 8;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 237);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbTapeType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bOpen);
            this.Controls.Add(this.cbWindowType);
            this.Name = "Main";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShift)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbWindowType;
        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTapeType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPeriod;
        private System.Windows.Forms.NumericUpDown nudShift;
        private System.Windows.Forms.HScrollBar hScrollBar;
    }
}

