namespace ComparativeTest
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
            this.nudLinesCount = new System.Windows.Forms.NumericUpDown();
            this.gbLines = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudLineWidth = new System.Windows.Forms.NumericUpDown();
            this.cbDotLine = new System.Windows.Forms.CheckBox();
            this.cbDashLine = new System.Windows.Forms.CheckBox();
            this.cbSolidLine = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbRectangles = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRectWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudRectCount = new System.Windows.Forms.NumericUpDown();
            this.gbFillRect = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudFillRectCount = new System.Windows.Forms.NumericUpDown();
            this.gbText = new System.Windows.Forms.GroupBox();
            this.cbTextAlignmentTop = new System.Windows.Forms.CheckBox();
            this.cbTextAlignmentLeft = new System.Windows.Forms.CheckBox();
            this.cbItalicFont = new System.Windows.Forms.CheckBox();
            this.cbBoldFont = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudTextSize = new System.Windows.Forms.NumericUpDown();
            this.tbTextString = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudTextCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbImageAlignmentTop = new System.Windows.Forms.CheckBox();
            this.cbImageAlignmentLeft = new System.Windows.Forms.CheckBox();
            this.tbImageFilePath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nudImagesCount = new System.Windows.Forms.NumericUpDown();
            this.cbWindowType = new System.Windows.Forms.ComboBox();
            this.bOpen = new System.Windows.Forms.Button();
            this.gbUseTranslator = new System.Windows.Forms.GroupBox();
            this.cbMirrorYAfter = new System.Windows.Forms.CheckBox();
            this.cbMirrorXAfter = new System.Windows.Forms.CheckBox();
            this.cbMirrorYBefore = new System.Windows.Forms.CheckBox();
            this.cbMirrorXBefore = new System.Windows.Forms.CheckBox();
            this.cbChangeAxels = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudPolygonCount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudLinesCount)).BeginInit();
            this.gbLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineWidth)).BeginInit();
            this.gbRectangles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectCount)).BeginInit();
            this.gbFillRect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFillRectCount)).BeginInit();
            this.gbText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImagesCount)).BeginInit();
            this.gbUseTranslator.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPolygonCount)).BeginInit();
            this.SuspendLayout();
            // 
            // nudLinesCount
            // 
            this.nudLinesCount.Location = new System.Drawing.Point(61, 19);
            this.nudLinesCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudLinesCount.Name = "nudLinesCount";
            this.nudLinesCount.Size = new System.Drawing.Size(120, 20);
            this.nudLinesCount.TabIndex = 0;
            this.nudLinesCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // gbLines
            // 
            this.gbLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLines.Controls.Add(this.label2);
            this.gbLines.Controls.Add(this.nudLineWidth);
            this.gbLines.Controls.Add(this.cbDotLine);
            this.gbLines.Controls.Add(this.cbDashLine);
            this.gbLines.Controls.Add(this.cbSolidLine);
            this.gbLines.Controls.Add(this.label1);
            this.gbLines.Controls.Add(this.nudLinesCount);
            this.gbLines.Location = new System.Drawing.Point(12, 12);
            this.gbLines.Name = "gbLines";
            this.gbLines.Size = new System.Drawing.Size(549, 100);
            this.gbLines.TabIndex = 1;
            this.gbLines.TabStop = false;
            this.gbLines.Text = "Lines";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ширина";
            // 
            // nudLineWidth
            // 
            this.nudLineWidth.Location = new System.Drawing.Point(239, 19);
            this.nudLineWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudLineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLineWidth.Name = "nudLineWidth";
            this.nudLineWidth.Size = new System.Drawing.Size(67, 20);
            this.nudLineWidth.TabIndex = 5;
            this.nudLineWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbDotLine
            // 
            this.cbDotLine.AutoSize = true;
            this.cbDotLine.Location = new System.Drawing.Point(118, 62);
            this.cbDotLine.Name = "cbDotLine";
            this.cbDotLine.Size = new System.Drawing.Size(43, 17);
            this.cbDotLine.TabIndex = 4;
            this.cbDotLine.Text = "Dot";
            this.cbDotLine.UseVisualStyleBackColor = true;
            // 
            // cbDashLine
            // 
            this.cbDashLine.AutoSize = true;
            this.cbDashLine.Location = new System.Drawing.Point(61, 62);
            this.cbDashLine.Name = "cbDashLine";
            this.cbDashLine.Size = new System.Drawing.Size(51, 17);
            this.cbDashLine.TabIndex = 3;
            this.cbDashLine.Text = "Dash";
            this.cbDashLine.UseVisualStyleBackColor = true;
            // 
            // cbSolidLine
            // 
            this.cbSolidLine.AutoSize = true;
            this.cbSolidLine.Checked = true;
            this.cbSolidLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSolidLine.Location = new System.Drawing.Point(6, 62);
            this.cbSolidLine.Name = "cbSolidLine";
            this.cbSolidLine.Size = new System.Drawing.Size(49, 17);
            this.cbSolidLine.TabIndex = 2;
            this.cbSolidLine.Text = "Solid";
            this.cbSolidLine.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Кол-во";
            // 
            // gbRectangles
            // 
            this.gbRectangles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRectangles.Controls.Add(this.label3);
            this.gbRectangles.Controls.Add(this.nudRectWidth);
            this.gbRectangles.Controls.Add(this.label4);
            this.gbRectangles.Controls.Add(this.nudRectCount);
            this.gbRectangles.Location = new System.Drawing.Point(12, 118);
            this.gbRectangles.Name = "gbRectangles";
            this.gbRectangles.Size = new System.Drawing.Size(549, 53);
            this.gbRectangles.TabIndex = 2;
            this.gbRectangles.TabStop = false;
            this.gbRectangles.Text = "DrawRectangles";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ширина";
            // 
            // nudRectWidth
            // 
            this.nudRectWidth.Location = new System.Drawing.Point(239, 19);
            this.nudRectWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudRectWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRectWidth.Name = "nudRectWidth";
            this.nudRectWidth.Size = new System.Drawing.Size(67, 20);
            this.nudRectWidth.TabIndex = 5;
            this.nudRectWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Кол-во";
            // 
            // nudRectCount
            // 
            this.nudRectCount.Location = new System.Drawing.Point(61, 19);
            this.nudRectCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudRectCount.Name = "nudRectCount";
            this.nudRectCount.Size = new System.Drawing.Size(120, 20);
            this.nudRectCount.TabIndex = 0;
            this.nudRectCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbFillRect
            // 
            this.gbFillRect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFillRect.Controls.Add(this.label6);
            this.gbFillRect.Controls.Add(this.nudFillRectCount);
            this.gbFillRect.Location = new System.Drawing.Point(12, 177);
            this.gbFillRect.Name = "gbFillRect";
            this.gbFillRect.Size = new System.Drawing.Size(261, 53);
            this.gbFillRect.TabIndex = 3;
            this.gbFillRect.TabStop = false;
            this.gbFillRect.Text = "FillRectangles";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Кол-во";
            // 
            // nudFillRectCount
            // 
            this.nudFillRectCount.Location = new System.Drawing.Point(61, 19);
            this.nudFillRectCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudFillRectCount.Name = "nudFillRectCount";
            this.nudFillRectCount.Size = new System.Drawing.Size(120, 20);
            this.nudFillRectCount.TabIndex = 0;
            this.nudFillRectCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbText
            // 
            this.gbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbText.Controls.Add(this.cbTextAlignmentTop);
            this.gbText.Controls.Add(this.cbTextAlignmentLeft);
            this.gbText.Controls.Add(this.cbItalicFont);
            this.gbText.Controls.Add(this.cbBoldFont);
            this.gbText.Controls.Add(this.label7);
            this.gbText.Controls.Add(this.nudTextSize);
            this.gbText.Controls.Add(this.tbTextString);
            this.gbText.Controls.Add(this.label5);
            this.gbText.Controls.Add(this.nudTextCount);
            this.gbText.Location = new System.Drawing.Point(12, 236);
            this.gbText.Name = "gbText";
            this.gbText.Size = new System.Drawing.Size(549, 99);
            this.gbText.TabIndex = 4;
            this.gbText.TabStop = false;
            this.gbText.Text = "Texts";
            // 
            // cbTextAlignmentTop
            // 
            this.cbTextAlignmentTop.AutoSize = true;
            this.cbTextAlignmentTop.Location = new System.Drawing.Point(113, 68);
            this.cbTextAlignmentTop.Name = "cbTextAlignmentTop";
            this.cbTextAlignmentTop.Size = new System.Drawing.Size(91, 17);
            this.cbTextAlignmentTop.TabIndex = 13;
            this.cbTextAlignmentTop.Text = "AlignmentTop";
            this.cbTextAlignmentTop.UseVisualStyleBackColor = true;
            // 
            // cbTextAlignmentLeft
            // 
            this.cbTextAlignmentLeft.AutoSize = true;
            this.cbTextAlignmentLeft.Location = new System.Drawing.Point(17, 68);
            this.cbTextAlignmentLeft.Name = "cbTextAlignmentLeft";
            this.cbTextAlignmentLeft.Size = new System.Drawing.Size(90, 17);
            this.cbTextAlignmentLeft.TabIndex = 12;
            this.cbTextAlignmentLeft.Text = "AlignmentLeft";
            this.cbTextAlignmentLeft.UseVisualStyleBackColor = true;
            // 
            // cbItalicFont
            // 
            this.cbItalicFont.AutoSize = true;
            this.cbItalicFont.Location = new System.Drawing.Point(70, 45);
            this.cbItalicFont.Name = "cbItalicFont";
            this.cbItalicFont.Size = new System.Drawing.Size(48, 17);
            this.cbItalicFont.TabIndex = 10;
            this.cbItalicFont.Text = "Italic";
            this.cbItalicFont.UseVisualStyleBackColor = true;
            // 
            // cbBoldFont
            // 
            this.cbBoldFont.AutoSize = true;
            this.cbBoldFont.Location = new System.Drawing.Point(17, 45);
            this.cbBoldFont.Name = "cbBoldFont";
            this.cbBoldFont.Size = new System.Drawing.Size(47, 17);
            this.cbBoldFont.TabIndex = 9;
            this.cbBoldFont.Text = "Bold";
            this.cbBoldFont.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(321, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Размер";
            // 
            // nudTextSize
            // 
            this.nudTextSize.Location = new System.Drawing.Point(373, 21);
            this.nudTextSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudTextSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nudTextSize.Name = "nudTextSize";
            this.nudTextSize.Size = new System.Drawing.Size(67, 20);
            this.nudTextSize.TabIndex = 7;
            this.nudTextSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // tbTextString
            // 
            this.tbTextString.Location = new System.Drawing.Point(190, 21);
            this.tbTextString.Name = "tbTextString";
            this.tbTextString.Size = new System.Drawing.Size(116, 20);
            this.tbTextString.TabIndex = 3;
            this.tbTextString.Text = "some text";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Кол-во";
            // 
            // nudTextCount
            // 
            this.nudTextCount.Location = new System.Drawing.Point(61, 19);
            this.nudTextCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTextCount.Name = "nudTextCount";
            this.nudTextCount.Size = new System.Drawing.Size(120, 20);
            this.nudTextCount.TabIndex = 0;
            this.nudTextCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbImageAlignmentTop);
            this.groupBox1.Controls.Add(this.cbImageAlignmentLeft);
            this.groupBox1.Controls.Add(this.tbImageFilePath);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.nudImagesCount);
            this.groupBox1.Location = new System.Drawing.Point(12, 341);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(549, 73);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Images";
            // 
            // cbImageAlignmentTop
            // 
            this.cbImageAlignmentTop.AutoSize = true;
            this.cbImageAlignmentTop.Location = new System.Drawing.Point(113, 45);
            this.cbImageAlignmentTop.Name = "cbImageAlignmentTop";
            this.cbImageAlignmentTop.Size = new System.Drawing.Size(91, 17);
            this.cbImageAlignmentTop.TabIndex = 11;
            this.cbImageAlignmentTop.Text = "AlignmentTop";
            this.cbImageAlignmentTop.UseVisualStyleBackColor = true;
            // 
            // cbImageAlignmentLeft
            // 
            this.cbImageAlignmentLeft.AutoSize = true;
            this.cbImageAlignmentLeft.Location = new System.Drawing.Point(17, 45);
            this.cbImageAlignmentLeft.Name = "cbImageAlignmentLeft";
            this.cbImageAlignmentLeft.Size = new System.Drawing.Size(90, 17);
            this.cbImageAlignmentLeft.TabIndex = 10;
            this.cbImageAlignmentLeft.Text = "AlignmentLeft";
            this.cbImageAlignmentLeft.UseVisualStyleBackColor = true;
            // 
            // tbImageFilePath
            // 
            this.tbImageFilePath.Location = new System.Drawing.Point(190, 21);
            this.tbImageFilePath.Name = "tbImageFilePath";
            this.tbImageFilePath.Size = new System.Drawing.Size(116, 20);
            this.tbImageFilePath.TabIndex = 3;
            this.tbImageFilePath.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbImageFilePath_MouseDoubleClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Кол-во";
            // 
            // nudImagesCount
            // 
            this.nudImagesCount.Location = new System.Drawing.Point(61, 19);
            this.nudImagesCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudImagesCount.Name = "nudImagesCount";
            this.nudImagesCount.Size = new System.Drawing.Size(120, 20);
            this.nudImagesCount.TabIndex = 0;
            // 
            // cbWindowType
            // 
            this.cbWindowType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbWindowType.FormattingEnabled = true;
            this.cbWindowType.Location = new System.Drawing.Point(12, 512);
            this.cbWindowType.Name = "cbWindowType";
            this.cbWindowType.Size = new System.Drawing.Size(468, 21);
            this.cbWindowType.TabIndex = 6;
            // 
            // bOpen
            // 
            this.bOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOpen.Location = new System.Drawing.Point(486, 510);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(75, 23);
            this.bOpen.TabIndex = 7;
            this.bOpen.Text = "Open";
            this.bOpen.UseVisualStyleBackColor = true;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // gbUseTranslator
            // 
            this.gbUseTranslator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbUseTranslator.Controls.Add(this.cbMirrorYAfter);
            this.gbUseTranslator.Controls.Add(this.cbMirrorXAfter);
            this.gbUseTranslator.Controls.Add(this.cbMirrorYBefore);
            this.gbUseTranslator.Controls.Add(this.cbMirrorXBefore);
            this.gbUseTranslator.Controls.Add(this.cbChangeAxels);
            this.gbUseTranslator.Location = new System.Drawing.Point(12, 420);
            this.gbUseTranslator.Name = "gbUseTranslator";
            this.gbUseTranslator.Size = new System.Drawing.Size(549, 84);
            this.gbUseTranslator.TabIndex = 8;
            this.gbUseTranslator.TabStop = false;
            this.gbUseTranslator.Text = "UseTranslator";
            // 
            // cbMirrorYAfter
            // 
            this.cbMirrorYAfter.AutoSize = true;
            this.cbMirrorYAfter.Location = new System.Drawing.Point(113, 61);
            this.cbMirrorYAfter.Name = "cbMirrorYAfter";
            this.cbMirrorYAfter.Size = new System.Drawing.Size(81, 17);
            this.cbMirrorYAfter.TabIndex = 4;
            this.cbMirrorYAfter.Text = "MirrorYAfter";
            this.cbMirrorYAfter.UseVisualStyleBackColor = true;
            // 
            // cbMirrorXAfter
            // 
            this.cbMirrorXAfter.AutoSize = true;
            this.cbMirrorXAfter.Location = new System.Drawing.Point(17, 61);
            this.cbMirrorXAfter.Name = "cbMirrorXAfter";
            this.cbMirrorXAfter.Size = new System.Drawing.Size(81, 17);
            this.cbMirrorXAfter.TabIndex = 3;
            this.cbMirrorXAfter.Text = "MirrorXAfter";
            this.cbMirrorXAfter.UseVisualStyleBackColor = true;
            // 
            // cbMirrorYBefore
            // 
            this.cbMirrorYBefore.AutoSize = true;
            this.cbMirrorYBefore.Location = new System.Drawing.Point(113, 19);
            this.cbMirrorYBefore.Name = "cbMirrorYBefore";
            this.cbMirrorYBefore.Size = new System.Drawing.Size(90, 17);
            this.cbMirrorYBefore.TabIndex = 2;
            this.cbMirrorYBefore.Text = "MirrorYBefore";
            this.cbMirrorYBefore.UseVisualStyleBackColor = true;
            // 
            // cbMirrorXBefore
            // 
            this.cbMirrorXBefore.AutoSize = true;
            this.cbMirrorXBefore.Location = new System.Drawing.Point(17, 19);
            this.cbMirrorXBefore.Name = "cbMirrorXBefore";
            this.cbMirrorXBefore.Size = new System.Drawing.Size(90, 17);
            this.cbMirrorXBefore.TabIndex = 1;
            this.cbMirrorXBefore.Text = "MirrorXBefore";
            this.cbMirrorXBefore.UseVisualStyleBackColor = true;
            // 
            // cbChangeAxels
            // 
            this.cbChangeAxels.AutoSize = true;
            this.cbChangeAxels.Location = new System.Drawing.Point(17, 42);
            this.cbChangeAxels.Name = "cbChangeAxels";
            this.cbChangeAxels.Size = new System.Drawing.Size(88, 17);
            this.cbChangeAxels.TabIndex = 0;
            this.cbChangeAxels.Text = "ChangeAxels";
            this.cbChangeAxels.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.nudPolygonCount);
            this.groupBox2.Location = new System.Drawing.Point(279, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 53);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Polygons";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Кол-во";
            // 
            // nudPolygonCount
            // 
            this.nudPolygonCount.Location = new System.Drawing.Point(61, 19);
            this.nudPolygonCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPolygonCount.Name = "nudPolygonCount";
            this.nudPolygonCount.Size = new System.Drawing.Size(120, 20);
            this.nudPolygonCount.TabIndex = 0;
            this.nudPolygonCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 545);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbUseTranslator);
            this.Controls.Add(this.bOpen);
            this.Controls.Add(this.cbWindowType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbText);
            this.Controls.Add(this.gbFillRect);
            this.Controls.Add(this.gbRectangles);
            this.Controls.Add(this.gbLines);
            this.Name = "Main";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nudLinesCount)).EndInit();
            this.gbLines.ResumeLayout(false);
            this.gbLines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineWidth)).EndInit();
            this.gbRectangles.ResumeLayout(false);
            this.gbRectangles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectCount)).EndInit();
            this.gbFillRect.ResumeLayout(false);
            this.gbFillRect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFillRectCount)).EndInit();
            this.gbText.ResumeLayout(false);
            this.gbText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImagesCount)).EndInit();
            this.gbUseTranslator.ResumeLayout(false);
            this.gbUseTranslator.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPolygonCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLines;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbRectangles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbFillRect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox gbText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbWindowType;
        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.GroupBox gbUseTranslator;
        public System.Windows.Forms.NumericUpDown nudLinesCount;
        public System.Windows.Forms.CheckBox cbSolidLine;
        public System.Windows.Forms.NumericUpDown nudLineWidth;
        public System.Windows.Forms.CheckBox cbDotLine;
        public System.Windows.Forms.CheckBox cbDashLine;
        public System.Windows.Forms.NumericUpDown nudRectWidth;
        public System.Windows.Forms.NumericUpDown nudRectCount;
        public System.Windows.Forms.NumericUpDown nudFillRectCount;
        public System.Windows.Forms.TextBox tbTextString;
        public System.Windows.Forms.NumericUpDown nudTextCount;
        public System.Windows.Forms.NumericUpDown nudTextSize;
        public System.Windows.Forms.CheckBox cbItalicFont;
        public System.Windows.Forms.CheckBox cbBoldFont;
        public System.Windows.Forms.TextBox tbImageFilePath;
        public System.Windows.Forms.NumericUpDown nudImagesCount;
        public System.Windows.Forms.CheckBox cbChangeAxels;
        public System.Windows.Forms.CheckBox cbMirrorYBefore;
        public System.Windows.Forms.CheckBox cbMirrorXBefore;
        public System.Windows.Forms.CheckBox cbImageAlignmentTop;
        public System.Windows.Forms.CheckBox cbImageAlignmentLeft;
        public System.Windows.Forms.CheckBox cbTextAlignmentTop;
        public System.Windows.Forms.CheckBox cbTextAlignmentLeft;
        public System.Windows.Forms.CheckBox cbMirrorYAfter;
        public System.Windows.Forms.CheckBox cbMirrorXAfter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.NumericUpDown nudPolygonCount;
    }
}

