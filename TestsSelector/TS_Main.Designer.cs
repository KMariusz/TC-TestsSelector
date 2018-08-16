namespace TestsSelector
{
  partial class TS_Main
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TS_Main));
      this.Load_Dialog = new System.Windows.Forms.OpenFileDialog();
      this.Tree = new System.Windows.Forms.TreeView();
      this.Icons_ImageList = new System.Windows.Forms.ImageList(this.components);
      this.Expand_Button = new System.Windows.Forms.Button();
      this.Collapse_Button = new System.Windows.Forms.Button();
      this.Parent_CB = new System.Windows.Forms.CheckBox();
      this.Children_CB = new System.Windows.Forms.CheckBox();
      this.Select_Button = new System.Windows.Forms.Button();
      this.Unselect_Button = new System.Windows.Forms.Button();
      this.Search_Button = new System.Windows.Forms.Button();
      this.Search_TB = new System.Windows.Forms.TextBox();
      this.Iterations_CB = new System.Windows.Forms.CheckBox();
      this.Node_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.Collapse_NumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.CollapseLevel_Label = new System.Windows.Forms.Label();
      this.TC_Path_Label = new System.Windows.Forms.Label();
      this.Settings_TCPath_TextBox = new System.Windows.Forms.TextBox();
      this.Settings_TCPath_Button = new System.Windows.Forms.Button();
      this.Tabs_Control = new System.Windows.Forms.TabControl();
      this.Tests_Page = new System.Windows.Forms.TabPage();
      this.Variables_Page = new System.Windows.Forms.TabPage();
      this.PersistentVariables_DGView = new System.Windows.Forms.DataGridView();
      this.Persistent_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.TemporaryVariables_DGView = new System.Windows.Forms.DataGridView();
      this.Temporary_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.PersistentVariables_Label = new System.Windows.Forms.Label();
      this.TemporaryVariables_Label = new System.Windows.Forms.Label();
      this.Variables_File_Label = new System.Windows.Forms.Label();
      this.Variables_Combo = new System.Windows.Forms.ComboBox();
      this.TS_Menu = new System.Windows.Forms.MenuStrip();
      this.File_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.File_Open_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.File_Save_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.File_Run_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.File_TestsFile_Import_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.File_TestsFile_Export_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.Settings_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.Help_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CommandLine_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.About_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.Icons_ToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.Load_Button = new System.Windows.Forms.Button();
      this.Save_Button = new System.Windows.Forms.Button();
      this.Run_Button = new System.Windows.Forms.Button();
      this.TS_StatusStrip = new System.Windows.Forms.StatusStrip();
      this.Scripts_Label = new System.Windows.Forms.ToolStripStatusLabel();
      this.ScriptsCount_Label = new System.Windows.Forms.ToolStripStatusLabel();
      this.MarkModified_CB = new System.Windows.Forms.CheckBox();
      this.Save_Dialog = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.Collapse_NumericUpDown)).BeginInit();
      this.Tabs_Control.SuspendLayout();
      this.Tests_Page.SuspendLayout();
      this.Variables_Page.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PersistentVariables_DGView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TemporaryVariables_DGView)).BeginInit();
      this.TS_Menu.SuspendLayout();
      this.TS_StatusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // Load_Dialog
      // 
      this.Load_Dialog.FileName = "Load_Dialog";
      // 
      // Tree
      // 
      this.Tree.BackColor = System.Drawing.SystemColors.Window;
      this.Tree.CheckBoxes = true;
      this.Tree.Cursor = System.Windows.Forms.Cursors.Default;
      this.Tree.ImageIndex = 2;
      this.Tree.ImageList = this.Icons_ImageList;
      this.Tree.Location = new System.Drawing.Point(-1, -1);
      this.Tree.Name = "Tree";
      this.Tree.SelectedImageIndex = 0;
      this.Tree.Size = new System.Drawing.Size(750, 477);
      this.Tree.TabIndex = 3;
      this.Tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.Tree_AfterCheck);
      this.Tree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tree_KeyDown);
      this.Tree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Tree_KeyUp);
      this.Tree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Tree_MouseUp);
      // 
      // Icons_ImageList
      // 
      this.Icons_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons_ImageList.ImageStream")));
      this.Icons_ImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.Icons_ImageList.Images.SetKeyName(0, "project-icon.gif");
      this.Icons_ImageList.Images.SetKeyName(1, "test-item.GIF");
      this.Icons_ImageList.Images.SetKeyName(2, "container.GIF");
      this.Icons_ImageList.Images.SetKeyName(3, "projectsuite.GIF");
      this.Icons_ImageList.Images.SetKeyName(4, "run.png");
      this.Icons_ImageList.Images.SetKeyName(5, "TestItems.PNG");
      this.Icons_ImageList.Images.SetKeyName(6, "Variables.PNG");
      this.Icons_ImageList.Images.SetKeyName(7, "new.png");
      this.Icons_ImageList.Images.SetKeyName(8, "delete.png");
      // 
      // Expand_Button
      // 
      this.Expand_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.Expand_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Expand_Button.FlatAppearance.BorderSize = 0;
      this.Expand_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Expand_Button.Image = ((System.Drawing.Image)(resources.GetObject("Expand_Button.Image")));
      this.Expand_Button.Location = new System.Drawing.Point(99, 27);
      this.Expand_Button.Name = "Expand_Button";
      this.Expand_Button.Size = new System.Drawing.Size(23, 23);
      this.Expand_Button.TabIndex = 4;
      this.Expand_Button.TabStop = false;
      this.Expand_Button.UseVisualStyleBackColor = true;
      this.Expand_Button.Click += new System.EventHandler(this.Expand_Button_Click);
      // 
      // Collapse_Button
      // 
      this.Collapse_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Collapse_Button.FlatAppearance.BorderSize = 0;
      this.Collapse_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Collapse_Button.Image = ((System.Drawing.Image)(resources.GetObject("Collapse_Button.Image")));
      this.Collapse_Button.Location = new System.Drawing.Point(128, 27);
      this.Collapse_Button.Name = "Collapse_Button";
      this.Collapse_Button.Size = new System.Drawing.Size(23, 23);
      this.Collapse_Button.TabIndex = 5;
      this.Collapse_Button.TabStop = false;
      this.Collapse_Button.UseVisualStyleBackColor = true;
      this.Collapse_Button.Click += new System.EventHandler(this.Collapse_Button_Click);
      // 
      // Parent_CB
      // 
      this.Parent_CB.AutoSize = true;
      this.Parent_CB.Location = new System.Drawing.Point(17, 128);
      this.Parent_CB.Name = "Parent_CB";
      this.Parent_CB.Size = new System.Drawing.Size(99, 17);
      this.Parent_CB.TabIndex = 13;
      this.Parent_CB.Text = "Parents_Option";
      this.Parent_CB.UseVisualStyleBackColor = true;
      // 
      // Children_CB
      // 
      this.Children_CB.AutoSize = true;
      this.Children_CB.Location = new System.Drawing.Point(17, 151);
      this.Children_CB.Name = "Children_CB";
      this.Children_CB.Size = new System.Drawing.Size(101, 17);
      this.Children_CB.TabIndex = 14;
      this.Children_CB.Text = "Children_Option";
      this.Children_CB.UseVisualStyleBackColor = true;
      // 
      // Select_Button
      // 
      this.Select_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Select_Button.FlatAppearance.BorderSize = 0;
      this.Select_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Select_Button.Image = ((System.Drawing.Image)(resources.GetObject("Select_Button.Image")));
      this.Select_Button.Location = new System.Drawing.Point(157, 27);
      this.Select_Button.Name = "Select_Button";
      this.Select_Button.Size = new System.Drawing.Size(23, 23);
      this.Select_Button.TabIndex = 17;
      this.Select_Button.TabStop = false;
      this.Select_Button.UseVisualStyleBackColor = true;
      this.Select_Button.Click += new System.EventHandler(this.Select_Button_Click);
      // 
      // Unselect_Button
      // 
      this.Unselect_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Unselect_Button.FlatAppearance.BorderSize = 0;
      this.Unselect_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Unselect_Button.Image = ((System.Drawing.Image)(resources.GetObject("Unselect_Button.Image")));
      this.Unselect_Button.Location = new System.Drawing.Point(186, 27);
      this.Unselect_Button.Name = "Unselect_Button";
      this.Unselect_Button.Size = new System.Drawing.Size(23, 23);
      this.Unselect_Button.TabIndex = 18;
      this.Unselect_Button.TabStop = false;
      this.Unselect_Button.UseVisualStyleBackColor = true;
      this.Unselect_Button.Click += new System.EventHandler(this.Unselect_Button_Click);
      // 
      // Search_Button
      // 
      this.Search_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.Search_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.Search_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Search_Button.Image = ((System.Drawing.Image)(resources.GetObject("Search_Button.Image")));
      this.Search_Button.Location = new System.Drawing.Point(744, 25);
      this.Search_Button.Name = "Search_Button";
      this.Search_Button.Size = new System.Drawing.Size(23, 23);
      this.Search_Button.TabIndex = 19;
      this.Search_Button.UseVisualStyleBackColor = true;
      this.Search_Button.Click += new System.EventHandler(this.Search_Button_Click);
      // 
      // Search_TB
      // 
      this.Search_TB.Location = new System.Drawing.Point(551, 27);
      this.Search_TB.Name = "Search_TB";
      this.Search_TB.Size = new System.Drawing.Size(187, 20);
      this.Search_TB.TabIndex = 20;
      this.Search_TB.TextChanged += new System.EventHandler(this.Search_TB_TextChanged);
      // 
      // Iterations_CB
      // 
      this.Iterations_CB.AutoSize = true;
      this.Iterations_CB.Location = new System.Drawing.Point(17, 174);
      this.Iterations_CB.Name = "Iterations_CB";
      this.Iterations_CB.Size = new System.Drawing.Size(106, 17);
      this.Iterations_CB.TabIndex = 22;
      this.Iterations_CB.Text = "Iterations_Option";
      this.Iterations_CB.UseVisualStyleBackColor = true;
      // 
      // Node_ContextMenu
      // 
      this.Node_ContextMenu.Name = "Node_ContextMenu";
      this.Node_ContextMenu.Size = new System.Drawing.Size(61, 4);
      // 
      // Collapse_NumericUpDown
      // 
      this.Collapse_NumericUpDown.Location = new System.Drawing.Point(690, 53);
      this.Collapse_NumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.Collapse_NumericUpDown.Name = "Collapse_NumericUpDown";
      this.Collapse_NumericUpDown.Size = new System.Drawing.Size(48, 20);
      this.Collapse_NumericUpDown.TabIndex = 23;
      this.Collapse_NumericUpDown.ValueChanged += new System.EventHandler(this.Collapse_NumericUpDown_ValueChanged);
      // 
      // CollapseLevel_Label
      // 
      this.CollapseLevel_Label.AutoSize = true;
      this.CollapseLevel_Label.Location = new System.Drawing.Point(548, 55);
      this.CollapseLevel_Label.Name = "CollapseLevel_Label";
      this.CollapseLevel_Label.Size = new System.Drawing.Size(95, 13);
      this.CollapseLevel_Label.TabIndex = 24;
      this.CollapseLevel_Label.Text = "Collapse To Level:";
      // 
      // TC_Path_Label
      // 
      this.TC_Path_Label.AutoSize = true;
      this.TC_Path_Label.Location = new System.Drawing.Point(15, 108);
      this.TC_Path_Label.Name = "TC_Path_Label";
      this.TC_Path_Label.Size = new System.Drawing.Size(140, 13);
      this.TC_Path_Label.TabIndex = 28;
      this.TC_Path_Label.Text = "TestComplete/TestExecute:";
      // 
      // Settings_TCPath_TextBox
      // 
      this.Settings_TCPath_TextBox.Location = new System.Drawing.Point(161, 105);
      this.Settings_TCPath_TextBox.Name = "Settings_TCPath_TextBox";
      this.Settings_TCPath_TextBox.Size = new System.Drawing.Size(568, 20);
      this.Settings_TCPath_TextBox.TabIndex = 29;
      this.Settings_TCPath_TextBox.TextChanged += new System.EventHandler(this.Settings_TCPath_TextBox_TextChanged);
      // 
      // Settings_TCPath_Button
      // 
      this.Settings_TCPath_Button.Location = new System.Drawing.Point(735, 103);
      this.Settings_TCPath_Button.Name = "Settings_TCPath_Button";
      this.Settings_TCPath_Button.Size = new System.Drawing.Size(30, 23);
      this.Settings_TCPath_Button.TabIndex = 30;
      this.Settings_TCPath_Button.Text = "...";
      this.Settings_TCPath_Button.UseVisualStyleBackColor = true;
      this.Settings_TCPath_Button.Click += new System.EventHandler(this.Settings_TCPath_Button_Click);
      // 
      // Tabs_Control
      // 
      this.Tabs_Control.Controls.Add(this.Tests_Page);
      this.Tabs_Control.Controls.Add(this.Variables_Page);
      this.Tabs_Control.Cursor = System.Windows.Forms.Cursors.Default;
      this.Tabs_Control.ImageList = this.Icons_ImageList;
      this.Tabs_Control.Location = new System.Drawing.Point(13, 72);
      this.Tabs_Control.Multiline = true;
      this.Tabs_Control.Name = "Tabs_Control";
      this.Tabs_Control.SelectedIndex = 0;
      this.Tabs_Control.Size = new System.Drawing.Size(754, 501);
      this.Tabs_Control.TabIndex = 31;
      this.Tabs_Control.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tabs_Control_KeyDown);
      // 
      // Tests_Page
      // 
      this.Tests_Page.Controls.Add(this.Tree);
      this.Tests_Page.Location = new System.Drawing.Point(4, 23);
      this.Tests_Page.Name = "Tests_Page";
      this.Tests_Page.Padding = new System.Windows.Forms.Padding(3);
      this.Tests_Page.Size = new System.Drawing.Size(746, 474);
      this.Tests_Page.TabIndex = 0;
      this.Tests_Page.Text = "Test Items";
      this.Tests_Page.UseVisualStyleBackColor = true;
      // 
      // Variables_Page
      // 
      this.Variables_Page.Controls.Add(this.PersistentVariables_DGView);
      this.Variables_Page.Controls.Add(this.TemporaryVariables_DGView);
      this.Variables_Page.Controls.Add(this.PersistentVariables_Label);
      this.Variables_Page.Controls.Add(this.TemporaryVariables_Label);
      this.Variables_Page.Controls.Add(this.Variables_File_Label);
      this.Variables_Page.Controls.Add(this.Variables_Combo);
      this.Variables_Page.ImageIndex = 6;
      this.Variables_Page.Location = new System.Drawing.Point(4, 23);
      this.Variables_Page.Name = "Variables_Page";
      this.Variables_Page.Size = new System.Drawing.Size(746, 474);
      this.Variables_Page.TabIndex = 1;
      this.Variables_Page.Text = "Variables";
      this.Variables_Page.UseVisualStyleBackColor = true;
      // 
      // PersistentVariables_DGView
      // 
      this.PersistentVariables_DGView.BackgroundColor = System.Drawing.SystemColors.Window;
      this.PersistentVariables_DGView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.PersistentVariables_DGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.PersistentVariables_DGView.ContextMenuStrip = this.Persistent_ContextMenu;
      this.PersistentVariables_DGView.Location = new System.Drawing.Point(1, 243);
      this.PersistentVariables_DGView.Name = "PersistentVariables_DGView";
      this.PersistentVariables_DGView.Size = new System.Drawing.Size(740, 227);
      this.PersistentVariables_DGView.TabIndex = 0;
      this.PersistentVariables_DGView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PersistentVariables_DGView_CellClick);
      this.PersistentVariables_DGView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.PersistentVariables_DGView_CellEndEdit);
      this.PersistentVariables_DGView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PersistentVariables_DGView_CellValueChanged);
      this.PersistentVariables_DGView.CurrentCellChanged += new System.EventHandler(this.PersistentVariables_DGView_CurrentCellChanged);
      this.PersistentVariables_DGView.CurrentCellDirtyStateChanged += new System.EventHandler(this.PersistentVariables_DGView_CurrentCellDirtyStateChanged);
      this.PersistentVariables_DGView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.PersistentVariables_DGView_DataBindingComplete);
      this.PersistentVariables_DGView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.PersistentVariables_DGView_EditingControlShowing);
      this.PersistentVariables_DGView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PersistentVariables_DGView_KeyDown);
      this.PersistentVariables_DGView.Leave += new System.EventHandler(this.PersistentVariables_DGView_Leave);
      this.PersistentVariables_DGView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PersistentVariables_DGView_MouseUp);
      // 
      // Persistent_ContextMenu
      // 
      this.Persistent_ContextMenu.Name = "Persistent_ContextMenu";
      this.Persistent_ContextMenu.Size = new System.Drawing.Size(61, 4);
      // 
      // TemporaryVariables_DGView
      // 
      this.TemporaryVariables_DGView.BackgroundColor = System.Drawing.SystemColors.Window;
      this.TemporaryVariables_DGView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.TemporaryVariables_DGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.TemporaryVariables_DGView.ContextMenuStrip = this.Temporary_ContextMenu;
      this.TemporaryVariables_DGView.Location = new System.Drawing.Point(1, 52);
      this.TemporaryVariables_DGView.Name = "TemporaryVariables_DGView";
      this.TemporaryVariables_DGView.Size = new System.Drawing.Size(740, 162);
      this.TemporaryVariables_DGView.TabIndex = 3;
      this.TemporaryVariables_DGView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TemporaryVariables_DGView_CellClick);
      this.TemporaryVariables_DGView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.TemporaryVariables_DGView_CellEndEdit);
      this.TemporaryVariables_DGView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.TemporaryVariables_DGView_CellValueChanged);
      this.TemporaryVariables_DGView.CurrentCellChanged += new System.EventHandler(this.TemporaryVariables_DGView_CurrentCellChanged);
      this.TemporaryVariables_DGView.CurrentCellDirtyStateChanged += new System.EventHandler(this.TemporaryVariables_DGView_CurrentCellDirtyStateChanged);
      this.TemporaryVariables_DGView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.TemporaryVariables_DGView_DataBindingComplete);
      this.TemporaryVariables_DGView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.TemporaryVariables_DGView_EditingControlShowing);
      this.TemporaryVariables_DGView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TemporaryVariables_DGView_KeyDown);
      this.TemporaryVariables_DGView.Leave += new System.EventHandler(this.TemporaryVariables_DGView_Leave);
      this.TemporaryVariables_DGView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TemporaryVariables_DGView_MouseUp);
      // 
      // Temporary_ContextMenu
      // 
      this.Temporary_ContextMenu.Name = "Temporary_ContextMenu";
      this.Temporary_ContextMenu.Size = new System.Drawing.Size(61, 4);
      // 
      // PersistentVariables_Label
      // 
      this.PersistentVariables_Label.BackColor = System.Drawing.Color.Gray;
      this.PersistentVariables_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.PersistentVariables_Label.Location = new System.Drawing.Point(1, 218);
      this.PersistentVariables_Label.Name = "PersistentVariables_Label";
      this.PersistentVariables_Label.Size = new System.Drawing.Size(740, 23);
      this.PersistentVariables_Label.TabIndex = 5;
      this.PersistentVariables_Label.Text = "Persistent Variables";
      this.PersistentVariables_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // TemporaryVariables_Label
      // 
      this.TemporaryVariables_Label.BackColor = System.Drawing.Color.Gray;
      this.TemporaryVariables_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.TemporaryVariables_Label.Location = new System.Drawing.Point(1, 27);
      this.TemporaryVariables_Label.Name = "TemporaryVariables_Label";
      this.TemporaryVariables_Label.Size = new System.Drawing.Size(740, 23);
      this.TemporaryVariables_Label.TabIndex = 4;
      this.TemporaryVariables_Label.Text = "Temporary Variables";
      this.TemporaryVariables_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // Variables_File_Label
      // 
      this.Variables_File_Label.AutoSize = true;
      this.Variables_File_Label.Location = new System.Drawing.Point(3, 8);
      this.Variables_File_Label.Name = "Variables_File_Label";
      this.Variables_File_Label.Size = new System.Drawing.Size(26, 13);
      this.Variables_File_Label.TabIndex = 2;
      this.Variables_File_Label.Text = "File:";
      // 
      // Variables_Combo
      // 
      this.Variables_Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.Variables_Combo.FormattingEnabled = true;
      this.Variables_Combo.Location = new System.Drawing.Point(35, 3);
      this.Variables_Combo.Name = "Variables_Combo";
      this.Variables_Combo.Size = new System.Drawing.Size(706, 21);
      this.Variables_Combo.TabIndex = 1;
      this.Variables_Combo.SelectedIndexChanged += new System.EventHandler(this.Variables_Combo_SelectedIndexChanged);
      // 
      // TS_Menu
      // 
      this.TS_Menu.BackColor = System.Drawing.SystemColors.Control;
      this.TS_Menu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.TS_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_MenuItem,
            this.Settings_MenuItem,
            this.Help_MenuItem});
      this.TS_Menu.Location = new System.Drawing.Point(0, 0);
      this.TS_Menu.Name = "TS_Menu";
      this.TS_Menu.Size = new System.Drawing.Size(884, 24);
      this.TS_Menu.TabIndex = 33;
      this.TS_Menu.Text = "menuStrip1";
      // 
      // File_MenuItem
      // 
      this.File_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Open_MenuItem,
            this.File_Save_MenuItem,
            this.toolStripSeparator1,
            this.File_Run_MenuItem,
            this.toolStripSeparator2,
            this.File_TestsFile_Import_MenuItem,
            this.File_TestsFile_Export_MenuItem});
      this.File_MenuItem.Name = "File_MenuItem";
      this.File_MenuItem.Size = new System.Drawing.Size(37, 20);
      this.File_MenuItem.Text = "File";
      // 
      // File_Open_MenuItem
      // 
      this.File_Open_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("File_Open_MenuItem.Image")));
      this.File_Open_MenuItem.Name = "File_Open_MenuItem";
      this.File_Open_MenuItem.Size = new System.Drawing.Size(160, 22);
      this.File_Open_MenuItem.Text = "Open";
      this.File_Open_MenuItem.Click += new System.EventHandler(this.Load_Button_Click);
      // 
      // File_Save_MenuItem
      // 
      this.File_Save_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("File_Save_MenuItem.Image")));
      this.File_Save_MenuItem.Name = "File_Save_MenuItem";
      this.File_Save_MenuItem.Size = new System.Drawing.Size(160, 22);
      this.File_Save_MenuItem.Text = "Save";
      this.File_Save_MenuItem.Click += new System.EventHandler(this.Save_Button_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
      // 
      // File_Run_MenuItem
      // 
      this.File_Run_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("File_Run_MenuItem.Image")));
      this.File_Run_MenuItem.Name = "File_Run_MenuItem";
      this.File_Run_MenuItem.Size = new System.Drawing.Size(160, 22);
      this.File_Run_MenuItem.Text = "Run";
      this.File_Run_MenuItem.Click += new System.EventHandler(this.Run_Button_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
      // 
      // File_TestsFile_Import_MenuItem
      // 
      this.File_TestsFile_Import_MenuItem.Name = "File_TestsFile_Import_MenuItem";
      this.File_TestsFile_Import_MenuItem.Size = new System.Drawing.Size(160, 22);
      this.File_TestsFile_Import_MenuItem.Text = "Import Tests File";
      this.File_TestsFile_Import_MenuItem.Click += new System.EventHandler(this.TestsFiles_Import_Click);
      // 
      // File_TestsFile_Export_MenuItem
      // 
      this.File_TestsFile_Export_MenuItem.Name = "File_TestsFile_Export_MenuItem";
      this.File_TestsFile_Export_MenuItem.Size = new System.Drawing.Size(160, 22);
      this.File_TestsFile_Export_MenuItem.Text = "Export Tests File";
      this.File_TestsFile_Export_MenuItem.Click += new System.EventHandler(this.TestsFiles_Export_Click);
      // 
      // Settings_MenuItem
      // 
      this.Settings_MenuItem.CheckOnClick = true;
      this.Settings_MenuItem.Name = "Settings_MenuItem";
      this.Settings_MenuItem.Size = new System.Drawing.Size(61, 20);
      this.Settings_MenuItem.Text = "Settings";
      this.Settings_MenuItem.CheckedChanged += new System.EventHandler(this.Settings_Button_CheckedChanged);
      // 
      // Help_MenuItem
      // 
      this.Help_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CommandLine_MenuItem,
            this.About_MenuItem});
      this.Help_MenuItem.Name = "Help_MenuItem";
      this.Help_MenuItem.Size = new System.Drawing.Size(52, 20);
      this.Help_MenuItem.Text = "About";
      // 
      // CommandLine_MenuItem
      // 
      this.CommandLine_MenuItem.Name = "CommandLine_MenuItem";
      this.CommandLine_MenuItem.Size = new System.Drawing.Size(181, 22);
      this.CommandLine_MenuItem.Text = "Command Line";
      this.CommandLine_MenuItem.Click += new System.EventHandler(this.CommandLine_MenuItem_Click);
      // 
      // About_MenuItem
      // 
      this.About_MenuItem.Name = "About_MenuItem";
      this.About_MenuItem.Size = new System.Drawing.Size(181, 22);
      this.About_MenuItem.Text = "About Tests Selector";
      this.About_MenuItem.Click += new System.EventHandler(this.aboutTestsSelectorToolStripMenuItem_Click);
      // 
      // Load_Button
      // 
      this.Load_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Load_Button.FlatAppearance.BorderSize = 0;
      this.Load_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Load_Button.Image = ((System.Drawing.Image)(resources.GetObject("Load_Button.Image")));
      this.Load_Button.Location = new System.Drawing.Point(12, 27);
      this.Load_Button.Name = "Load_Button";
      this.Load_Button.Size = new System.Drawing.Size(23, 23);
      this.Load_Button.TabIndex = 34;
      this.Load_Button.UseVisualStyleBackColor = true;
      this.Load_Button.Click += new System.EventHandler(this.Load_Button_Click);
      // 
      // Save_Button
      // 
      this.Save_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Save_Button.FlatAppearance.BorderSize = 0;
      this.Save_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Save_Button.Image = ((System.Drawing.Image)(resources.GetObject("Save_Button.Image")));
      this.Save_Button.Location = new System.Drawing.Point(41, 27);
      this.Save_Button.Name = "Save_Button";
      this.Save_Button.Size = new System.Drawing.Size(23, 23);
      this.Save_Button.TabIndex = 35;
      this.Save_Button.UseVisualStyleBackColor = true;
      this.Save_Button.Click += new System.EventHandler(this.Save_Button_Click);
      // 
      // Run_Button
      // 
      this.Run_Button.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
      this.Run_Button.FlatAppearance.BorderSize = 0;
      this.Run_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.Run_Button.Image = ((System.Drawing.Image)(resources.GetObject("Run_Button.Image")));
      this.Run_Button.Location = new System.Drawing.Point(70, 27);
      this.Run_Button.Name = "Run_Button";
      this.Run_Button.Size = new System.Drawing.Size(23, 23);
      this.Run_Button.TabIndex = 36;
      this.Run_Button.UseVisualStyleBackColor = true;
      this.Run_Button.Click += new System.EventHandler(this.Run_Button_Click);
      // 
      // TS_StatusStrip
      // 
      this.TS_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Scripts_Label,
            this.ScriptsCount_Label});
      this.TS_StatusStrip.Location = new System.Drawing.Point(0, 639);
      this.TS_StatusStrip.Name = "TS_StatusStrip";
      this.TS_StatusStrip.Size = new System.Drawing.Size(884, 22);
      this.TS_StatusStrip.TabIndex = 37;
      this.TS_StatusStrip.Text = "statusStrip1";
      // 
      // Scripts_Label
      // 
      this.Scripts_Label.Name = "Scripts_Label";
      this.Scripts_Label.Size = new System.Drawing.Size(87, 17);
      this.Scripts_Label.Text = "Scripts for run: ";
      // 
      // ScriptsCount_Label
      // 
      this.ScriptsCount_Label.Name = "ScriptsCount_Label";
      this.ScriptsCount_Label.Size = new System.Drawing.Size(14, 17);
      this.ScriptsCount_Label.Text = "X";
      // 
      // MarkModified_CB
      // 
      this.MarkModified_CB.AutoSize = true;
      this.MarkModified_CB.Location = new System.Drawing.Point(17, 198);
      this.MarkModified_CB.Name = "MarkModified_CB";
      this.MarkModified_CB.Size = new System.Drawing.Size(127, 17);
      this.MarkModified_CB.TabIndex = 40;
      this.MarkModified_CB.Text = "MarkModified_Option";
      this.MarkModified_CB.UseVisualStyleBackColor = true;
      // 
      // TS_Main
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(884, 661);
      this.Controls.Add(this.MarkModified_CB);
      this.Controls.Add(this.TS_StatusStrip);
      this.Controls.Add(this.Run_Button);
      this.Controls.Add(this.Save_Button);
      this.Controls.Add(this.Load_Button);
      this.Controls.Add(this.TS_Menu);
      this.Controls.Add(this.Settings_TCPath_Button);
      this.Controls.Add(this.Settings_TCPath_TextBox);
      this.Controls.Add(this.CollapseLevel_Label);
      this.Controls.Add(this.Collapse_NumericUpDown);
      this.Controls.Add(this.Search_TB);
      this.Controls.Add(this.Search_Button);
      this.Controls.Add(this.Unselect_Button);
      this.Controls.Add(this.Select_Button);
      this.Controls.Add(this.Collapse_Button);
      this.Controls.Add(this.Expand_Button);
      this.Controls.Add(this.TC_Path_Label);
      this.Controls.Add(this.Iterations_CB);
      this.Controls.Add(this.Children_CB);
      this.Controls.Add(this.Parent_CB);
      this.Controls.Add(this.Tabs_Control);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.TS_Menu;
      this.MinimumSize = new System.Drawing.Size(795, 650);
      this.Name = "TS_Main";
      this.Text = "Tests Selector";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TS_Main_FormClosing);
      this.Load += new System.EventHandler(this.TS_Main_Load);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TS_Main_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TS_Main_DragEnter);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TS_Main_KeyDown);
      this.Resize += new System.EventHandler(this.TS_Main_Resize);
      ((System.ComponentModel.ISupportInitialize)(this.Collapse_NumericUpDown)).EndInit();
      this.Tabs_Control.ResumeLayout(false);
      this.Tests_Page.ResumeLayout(false);
      this.Variables_Page.ResumeLayout(false);
      this.Variables_Page.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PersistentVariables_DGView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TemporaryVariables_DGView)).EndInit();
      this.TS_Menu.ResumeLayout(false);
      this.TS_Menu.PerformLayout();
      this.TS_StatusStrip.ResumeLayout(false);
      this.TS_StatusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog Load_Dialog;
    private System.Windows.Forms.TreeView Tree;
    private System.Windows.Forms.Button Expand_Button;
    private System.Windows.Forms.Button Collapse_Button;
    private System.Windows.Forms.CheckBox Parent_CB;
    private System.Windows.Forms.CheckBox Children_CB;
    private System.Windows.Forms.Button Select_Button;
    private System.Windows.Forms.Button Unselect_Button;
    private System.Windows.Forms.Button Search_Button;
    private System.Windows.Forms.TextBox Search_TB;
        private System.Windows.Forms.CheckBox Iterations_CB;
        private System.Windows.Forms.ContextMenuStrip Node_ContextMenu;
        private System.Windows.Forms.NumericUpDown Collapse_NumericUpDown;
        private System.Windows.Forms.Label CollapseLevel_Label;
        private System.Windows.Forms.ImageList Icons_ImageList;
        private System.Windows.Forms.Label TC_Path_Label;
        private System.Windows.Forms.TextBox Settings_TCPath_TextBox;
        private System.Windows.Forms.Button Settings_TCPath_Button;
        private System.Windows.Forms.TabControl Tabs_Control;
        private System.Windows.Forms.TabPage Variables_Page;
        private System.Windows.Forms.DataGridView PersistentVariables_DGView;
        private System.Windows.Forms.ComboBox Variables_Combo;
        private System.Windows.Forms.Label Variables_File_Label;
        private System.Windows.Forms.DataGridView TemporaryVariables_DGView;
        private System.Windows.Forms.Label TemporaryVariables_Label;
        private System.Windows.Forms.Label PersistentVariables_Label;
        private System.Windows.Forms.ContextMenuStrip Temporary_ContextMenu;
        private System.Windows.Forms.ContextMenuStrip Persistent_ContextMenu;
        private System.Windows.Forms.MenuStrip TS_Menu;
        private System.Windows.Forms.ToolStripMenuItem File_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem File_Open_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem File_Save_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem File_Run_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem Settings_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem File_TestsFile_Import_MenuItem;
        private System.Windows.Forms.ToolTip Icons_ToolTip;
        private System.Windows.Forms.ToolStripMenuItem Help_MenuItem;
        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.Button Run_Button;
        private System.Windows.Forms.StatusStrip TS_StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Scripts_Label;
        private System.Windows.Forms.ToolStripStatusLabel ScriptsCount_Label;
        public System.Windows.Forms.TabPage Tests_Page;
        private System.Windows.Forms.CheckBox MarkModified_CB;
        private System.Windows.Forms.ToolStripMenuItem File_TestsFile_Export_MenuItem;
        private System.Windows.Forms.SaveFileDialog Save_Dialog;
    private System.Windows.Forms.ToolStripMenuItem CommandLine_MenuItem;
    private System.Windows.Forms.ToolStripMenuItem About_MenuItem;
  }
}

