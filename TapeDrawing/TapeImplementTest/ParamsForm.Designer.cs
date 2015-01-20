namespace TapeImplementTest
{
    partial class ParamsForm
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
            this.nudIndexLen = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudInterrupts = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudScale = new System.Windows.Forms.NumericUpDown();
            this.cbVertical = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudIndexLen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterrupts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
            this.SuspendLayout();
            // 
            // nudIndexLen
            // 
            this.nudIndexLen.Location = new System.Drawing.Point(144, 12);
            this.nudIndexLen.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudIndexLen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudIndexLen.Name = "nudIndexLen";
            this.nudIndexLen.Size = new System.Drawing.Size(120, 20);
            this.nudIndexLen.TabIndex = 0;
            this.nudIndexLen.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Индексов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Начальная координата";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Конечная координата";
            // 
            // nudInterrupts
            // 
            this.nudInterrupts.Location = new System.Drawing.Point(144, 90);
            this.nudInterrupts.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudInterrupts.Name = "nudInterrupts";
            this.nudInterrupts.Size = new System.Drawing.Size(120, 20);
            this.nudInterrupts.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Прерываний";
            // 
            // tbMin
            // 
            this.tbMin.Location = new System.Drawing.Point(144, 37);
            this.tbMin.Name = "tbMin";
            this.tbMin.Size = new System.Drawing.Size(120, 20);
            this.tbMin.TabIndex = 2;
            this.tbMin.Text = "0";
            // 
            // tbMax
            // 
            this.tbMax.Location = new System.Drawing.Point(144, 63);
            this.tbMax.Name = "tbMax";
            this.tbMax.Size = new System.Drawing.Size(120, 20);
            this.tbMax.TabIndex = 2;
            this.tbMax.Text = "10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Масштаб х";
            // 
            // nudScale
            // 
            this.nudScale.Location = new System.Drawing.Point(144, 116);
            this.nudScale.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScale.Name = "nudScale";
            this.nudScale.Size = new System.Drawing.Size(120, 20);
            this.nudScale.TabIndex = 3;
            this.nudScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbVertical
            // 
            this.cbVertical.AutoSize = true;
            this.cbVertical.Location = new System.Drawing.Point(144, 142);
            this.cbVertical.Name = "cbVertical";
            this.cbVertical.Size = new System.Drawing.Size(92, 17);
            this.cbVertical.TabIndex = 5;
            this.cbVertical.Text = "Вертикально";
            this.cbVertical.UseVisualStyleBackColor = true;
            this.cbVertical.CheckedChanged += new System.EventHandler(this.CbVerticalCheckedChanged);
            // 
            // ParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 186);
            this.Controls.Add(this.cbVertical);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudScale);
            this.Controls.Add(this.tbMax);
            this.Controls.Add(this.tbMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudInterrupts);
            this.Controls.Add(this.nudIndexLen);
            this.MaximizeBox = false;
            this.Name = "ParamsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Параметры";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ParamsFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.nudIndexLen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterrupts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudIndexLen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudInterrupts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudScale;
        private System.Windows.Forms.CheckBox cbVertical;
    }
}