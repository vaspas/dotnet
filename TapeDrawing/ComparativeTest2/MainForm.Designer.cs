namespace ComparativeTest2
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mmLaunch = new System.Windows.Forms.ToolStripMenuItem();
			this.mmLaunchAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lvShapes = new ComparativeTest2.Ui.ListViewTable();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsMainAdd = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsMainUp = new System.Windows.Forms.ToolStripButton();
			this.tsMainDown = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsMainDelete = new System.Windows.Forms.ToolStripButton();
			this.tsFile = new System.Windows.Forms.ToolStrip();
			this.tsFileLoad = new System.Windows.Forms.ToolStripButton();
			this.tsFileSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsFileRefresh = new System.Windows.Forms.ToolStripButton();
			this.tsFileArrangeWindows = new System.Windows.Forms.ToolStripButton();
			this.contextMenuShapes = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.menuStrip.SuspendLayout();
			this.toolStripContainer.ContentPanel.SuspendLayout();
			this.toolStripContainer.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tsFile.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "bullet_ball_glass_blue.png");
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmLaunch});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(884, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// mmLaunch
			// 
			this.mmLaunch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmLaunchAll,
            this.toolStripSeparator4});
			this.mmLaunch.Name = "mmLaunch";
			this.mmLaunch.Size = new System.Drawing.Size(57, 20);
			this.mmLaunch.Text = "Запуск";
			// 
			// mmLaunchAll
			// 
			this.mmLaunchAll.Name = "mmLaunchAll";
			this.mmLaunchAll.Size = new System.Drawing.Size(152, 22);
			this.mmLaunchAll.Text = "Все";
			this.mmLaunchAll.Click += new System.EventHandler(this.MmLaunchAllClick);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
			// 
			// toolStripContainer
			// 
			this.toolStripContainer.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer.ContentPanel
			// 
			this.toolStripContainer.ContentPanel.AutoScroll = true;
			this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer1);
			this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(884, 359);
			this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer.LeftToolStripPanelVisible = false;
			this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
			this.toolStripContainer.Name = "toolStripContainer";
			this.toolStripContainer.RightToolStripPanelVisible = false;
			this.toolStripContainer.Size = new System.Drawing.Size(884, 384);
			this.toolStripContainer.TabIndex = 4;
			this.toolStripContainer.Text = "toolStripContainer1";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.tsFile);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lvShapes);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer1.Size = new System.Drawing.Size(884, 359);
			this.splitContainer1.SplitterDistance = 570;
			this.splitContainer1.TabIndex = 2;
			// 
			// lvShapes
			// 
			this.lvShapes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvShapes.FullRowSelect = true;
			this.lvShapes.GridLines = true;
			this.lvShapes.LargeImageList = this.imageList;
			this.lvShapes.Location = new System.Drawing.Point(0, 0);
			this.lvShapes.MultiSelect = false;
			this.lvShapes.Name = "lvShapes";
			this.lvShapes.ObjectList = null;
			this.lvShapes.ShowGroups = false;
			this.lvShapes.ShowItemToolTips = true;
			this.lvShapes.Size = new System.Drawing.Size(566, 355);
			this.lvShapes.SmallImageList = this.imageList;
			this.lvShapes.TabIndex = 0;
			this.lvShapes.UseCompatibleStateImageBehavior = false;
			this.lvShapes.View = System.Windows.Forms.View.Details;
			this.lvShapes.ObjectSеlected += new System.Action<object>(this.LvShapesObjectSеlected);
			// 
			// propertyGrid
			// 
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(306, 355);
			this.propertyGrid.TabIndex = 1;
			this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGridPropertyValueChanged);
			this.propertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.PropertyGridSelectedGridItemChanged);
			this.propertyGrid.Leave += new System.EventHandler(this.PropertyGridLeave);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMainAdd,
            this.toolStripSeparator1,
            this.tsMainUp,
            this.tsMainDown,
            this.toolStripSeparator2,
            this.tsMainDelete});
			this.toolStrip1.Location = new System.Drawing.Point(3, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(116, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsMainAdd
			// 
			this.tsMainAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsMainAdd.Image = global::ComparativeTest2.Properties.Resources.add2;
			this.tsMainAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainAdd.Name = "tsMainAdd";
			this.tsMainAdd.Size = new System.Drawing.Size(23, 22);
			this.tsMainAdd.Text = "Добавить фигуру...";
			this.tsMainAdd.Click += new System.EventHandler(this.TsMainAddClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsMainUp
			// 
			this.tsMainUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsMainUp.Image = global::ComparativeTest2.Properties.Resources.arrow_up_green;
			this.tsMainUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainUp.Name = "tsMainUp";
			this.tsMainUp.Size = new System.Drawing.Size(23, 22);
			this.tsMainUp.Text = "Вверх";
			this.tsMainUp.Click += new System.EventHandler(this.TsMainUpClick);
			// 
			// tsMainDown
			// 
			this.tsMainDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsMainDown.Image = global::ComparativeTest2.Properties.Resources.arrow_down_green;
			this.tsMainDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainDown.Name = "tsMainDown";
			this.tsMainDown.Size = new System.Drawing.Size(23, 22);
			this.tsMainDown.Text = "Вниз";
			this.tsMainDown.Click += new System.EventHandler(this.TsMainDownClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tsMainDelete
			// 
			this.tsMainDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsMainDelete.Image = global::ComparativeTest2.Properties.Resources.delete2;
			this.tsMainDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainDelete.Name = "tsMainDelete";
			this.tsMainDelete.Size = new System.Drawing.Size(23, 22);
			this.tsMainDelete.Text = "Удалить фигуру";
			this.tsMainDelete.Click += new System.EventHandler(this.TsMainDeleteClick);
			// 
			// tsFile
			// 
			this.tsFile.Dock = System.Windows.Forms.DockStyle.None;
			this.tsFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFileLoad,
            this.tsFileSave,
            this.toolStripSeparator3,
            this.tsFileRefresh,
            this.tsFileArrangeWindows});
			this.tsFile.Location = new System.Drawing.Point(119, 0);
			this.tsFile.Name = "tsFile";
			this.tsFile.Size = new System.Drawing.Size(110, 25);
			this.tsFile.TabIndex = 4;
			// 
			// tsFileLoad
			// 
			this.tsFileLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsFileLoad.Image = global::ComparativeTest2.Properties.Resources.folder_out;
			this.tsFileLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsFileLoad.Name = "tsFileLoad";
			this.tsFileLoad.Size = new System.Drawing.Size(23, 22);
			this.tsFileLoad.Text = "Загрузить...";
			this.tsFileLoad.Click += new System.EventHandler(this.TsFileLoadClick);
			// 
			// tsFileSave
			// 
			this.tsFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsFileSave.Image = global::ComparativeTest2.Properties.Resources.disk_blue_window;
			this.tsFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsFileSave.Name = "tsFileSave";
			this.tsFileSave.Size = new System.Drawing.Size(23, 22);
			this.tsFileSave.Text = "Сохранить...";
			this.tsFileSave.Click += new System.EventHandler(this.TsFileSaveClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// tsFileRefresh
			// 
			this.tsFileRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsFileRefresh.Image = global::ComparativeTest2.Properties.Resources.refresh;
			this.tsFileRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsFileRefresh.Name = "tsFileRefresh";
			this.tsFileRefresh.Size = new System.Drawing.Size(23, 22);
			this.tsFileRefresh.Text = "Обновить окна";
			this.tsFileRefresh.Click += new System.EventHandler(this.TsFileRefreshClick);
			// 
			// tsFileArrangeWindows
			// 
			this.tsFileArrangeWindows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsFileArrangeWindows.Image = global::ComparativeTest2.Properties.Resources.windows;
			this.tsFileArrangeWindows.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsFileArrangeWindows.Name = "tsFileArrangeWindows";
			this.tsFileArrangeWindows.Size = new System.Drawing.Size(23, 22);
			this.tsFileArrangeWindows.Text = "Выровнять окна";
			this.tsFileArrangeWindows.Click += new System.EventHandler(this.TsFileArrangeWindowsClick);
			// 
			// contextMenuShapes
			// 
			this.contextMenuShapes.Name = "contextMenuShapes";
			this.contextMenuShapes.Size = new System.Drawing.Size(61, 4);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "xml files|*.xml";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.FileName = "shapeList";
			this.saveFileDialog.Filter = "xml files|*.xml";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 408);
			this.Controls.Add(this.toolStripContainer);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolStripContainer.ContentPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.PerformLayout();
			this.toolStripContainer.ResumeLayout(false);
			this.toolStripContainer.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tsFile.ResumeLayout(false);
			this.tsFile.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripContainer toolStripContainer;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private Ui.ListViewTable lvShapes;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsMainAdd;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsMainUp;
		private System.Windows.Forms.ToolStripButton tsMainDown;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsMainDelete;
		private System.Windows.Forms.ContextMenuStrip contextMenuShapes;
		private System.Windows.Forms.ToolStrip tsFile;
		private System.Windows.Forms.ToolStripButton tsFileLoad;
		private System.Windows.Forms.ToolStripButton tsFileSave;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolStripMenuItem mmLaunch;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton tsFileRefresh;
		private System.Windows.Forms.ToolStripMenuItem mmLaunchAll;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton tsFileArrangeWindows;
	}
}

