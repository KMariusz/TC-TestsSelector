using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace TestsSelector
{
  public partial class TS_Main : Form
  {
    internal Settings _settings;

    private string _valueBeforeEdit;
    private bool _removeOperation = false;
    private bool _translateOperation = false;
    private bool _searchOperation = false;
    private BindingList<TreeNode> _modifiedCopy = new BindingList<TreeNode>();

    #region Initial state

    public TS_Main(string[] args)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      InitializeComponent();
      Init_Settings(args);
    }

    private void TS_Main_Load(object sender, EventArgs e)
    {
      Init_TreeMenu();
      Init_VariablesMenu();
      Init_StartState();
      Settings_Hide();
      CommandLine();
      InitTranslation(_settings, null);
    }

    private Control TS_Main_FindFocusedControl()
    {
      return TS_Main_FindFocusedControl(this);
    }

    private static Control TS_Main_FindFocusedControl(Control container)
    {
      foreach (Control childControl in container.Controls)
      {
        if (childControl.Focused)
        {
          return childControl;
        }
      }

      foreach (Control childControl in container.Controls)
      {
        Control maybeFocusedControl = TS_Main_FindFocusedControl(childControl);
        if (maybeFocusedControl != null)
        {
          return maybeFocusedControl;
        }
      }

      return null;
    }

    private void Init_Settings(string[] args)
    {
      _settings = new Settings() { Arguments = args, Variables_Page = Tabs_Control.TabPages["Variables_Page"], TestComplete_Path = Properties.Settings.Default.TC_Path };
      _settings.ModifiedChanged += Save_OnModification;
      _settings.ModifiedNodes_Changed += Tree_Modified_BackgroundColor;
      Init_TCPath();
      Settings_TCPath_TextBox.Text = _settings.TestComplete_Path;
      File_Run_MenuItem.Enabled = !string.IsNullOrEmpty(_settings.TestComplete_Path) && File.Exists(_settings.TestComplete_Path);
      Settings_Change_Parent(Properties.Settings.Default.Parents);
      Settings_Change_Children(Properties.Settings.Default.Children);
      Settings_Change_ShowIterations(Properties.Settings.Default.Iterations);
      Settings_Change_MarkModified(Properties.Settings.Default.MarkEdited);
    }

    private void Init_TCPath()
    {
      var _path = _settings.TestComplete_Path.Trim();
      if (string.IsNullOrEmpty(_path) || _path.Length < 1)
      {
        _path = Init_TCPath_RegistrySearch("TestExecute");
        if (_path != null)
        {
          var _path64 = _path + @"x64\Bin\TestExecute.exe";

          if (File.Exists(_path64))
          {
            _path = _path64;
          }
          else
          {
            _path += @"Bin\TestExecute.exe";
            if (!File.Exists(_path)) _path = null;
          }
        }
        if (_path == null)
        {
          _path = Init_TCPath_RegistrySearch("TestComplete");
          if (_path != null)
          {
            var _path64 = _path + @"x64\Bin\TestComplete.exe";

            if (File.Exists(_path64))
            {
              _path = _path64;
            }
            else
            {
              _path += @"Bin\TestComplete.exe";
              if (!File.Exists(_path)) _path = null;
            }
          }
        }
        if (_path != null)
        {
          _settings.TestComplete_Path = _path;
        }
        File_Run_MenuItem.Enabled = !string.IsNullOrEmpty(_settings.TestComplete_Path) && File.Exists(_settings.TestComplete_Path);
      }
    }

    private string Init_TCPath_RegistrySearch(string name)
    {
      string displayName;
      string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
      RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
      if (key != null)
      {
        foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
        {
          displayName = subkey.GetValue("DisplayName") as string;
          if (displayName != null && displayName.Contains(name))
          {
            return subkey.GetValue("InstallLocation").ToString();
          }
        }
        key.Close();
      }

      registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
      key = Registry.LocalMachine.OpenSubKey(registryKey);
      if (key != null)
      {
        foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
        {
          displayName = subkey.GetValue("DisplayName") as string;
          if (displayName != null && displayName.Contains(name))
          {
            return subkey.GetValue("InstallLocation").ToString();
          }
        }
        key.Close();
      }
      return null;
    }

    private void Init_TreeMenu()
    {
      Node_ContextMenu = new ContextMenuStrip();
      ToolStripMenuItem _iterations = new ToolStripMenuItem
      {
        Text = Translation.ChangeIterations,
        Name = "Iterations"
      };
      _iterations.Click += new System.EventHandler(this.Iterations_Change);
      Node_ContextMenu.Items.AddRange(new ToolStripMenuItem[] { _iterations });
    }

    private void Init_VariablesMenu()
    {
      Temporary_ContextMenu = new ContextMenuStrip();
      Persistent_ContextMenu = new ContextMenuStrip();
      ToolStripMenuItem _add_temporary = new ToolStripMenuItem
      {
        Text = Translation.Add,
        Name = "Add"
      };
      _add_temporary.Click += new System.EventHandler(this.Temporary_ContextMenu_Add_Clicked);
      _add_temporary.Image = Icons_ImageList.Images[7];
      ToolStripMenuItem _delete_temporary = new ToolStripMenuItem
      {
        Text = Translation.Delete,
        Name = "Delete"
      };
      _delete_temporary.Click += new System.EventHandler(this.Temporary_ContextMenu_Delete_Clicked);
      _delete_temporary.Image = Icons_ImageList.Images[8];
      ToolStripMenuItem _add_persistent = new ToolStripMenuItem
      {
        Text = Translation.Add,
        Name = "Add"
      };
      _add_persistent.Click += new System.EventHandler(this.Persistent_ContextMenu_Add_Clicked);
      _add_persistent.Image = Icons_ImageList.Images[7];
      ToolStripMenuItem _delete_persistent = new ToolStripMenuItem
      {
        Text = Translation.Delete,
        Name = "Delete"
      };
      _delete_persistent.Click += new System.EventHandler(this.Persistent_ContextMenu_Delete_Clicked);
      _delete_persistent.Image = Icons_ImageList.Images[8];
      Temporary_ContextMenu.Items.AddRange(new ToolStripMenuItem[] { _add_temporary, _delete_temporary });
      Persistent_ContextMenu.Items.AddRange(new ToolStripMenuItem[] { _add_persistent, _delete_persistent });
      TemporaryVariables_DGView.ContextMenuStrip = Temporary_ContextMenu;
      PersistentVariables_DGView.ContextMenuStrip = Persistent_ContextMenu;
    }

    private void Init_StartState()
    {
      _settings.Load_Complete = false;
      _settings.MDS_Loaded = false;
      _settings.Modified = false;
      _settings.ModifiedNodes.Clear();
      Search_TB.Text = "";
      File_Open_MenuItem.Enabled = !Settings_MenuItem.Checked;
      Load_Button.Enabled = File_Open_MenuItem.Enabled;
      Expand_Button.Enabled = false;
      Collapse_Button.Enabled = false;
      Select_Button.Enabled = false;
      Unselect_Button.Enabled = false;
      Tree.Visible = false;
      Tabs_Control.Visible = false;
      Search_Button.Enabled = false;
      Search_TB.Enabled = false;
      Collapse_NumericUpDown.Enabled = false;
      CollapseLevel_Label.Enabled = false;
      File_TestsFile_Import_MenuItem.Enabled = false;
      File_TestsFile_Export_MenuItem.Enabled = false;
      File_Run_MenuItem.Enabled = false;
      Run_Button.Enabled = File_Run_MenuItem.Enabled;
      Scripts_Label.Visible = false;
      ScriptsCount_Label.Visible = Scripts_Label.Visible;
      File_Run_MenuItem.Enabled = !string.IsNullOrEmpty(_settings.TestComplete_Path) && File.Exists(_settings.TestComplete_Path);
      TS_Main_Resize(null, null);
    }

    private void InitTranslation(object sender, EventArgs e)
    {
      _translateOperation = true;
      //Menu items
      File_MenuItem.Text = Translation.File;
      File_Open_MenuItem.Text = Translation.Open;
      File_Save_MenuItem.Text = Translation.Save;
      File_Run_MenuItem.Text = Translation.Run;
      File_TestsFile_Import_MenuItem.Text = Translation.TestsFile_Import;
      File_TestsFile_Export_MenuItem.Text = Translation.TestsFile_Export;
      Settings_MenuItem.Text = Translation.Settings;
      About_MenuItem.Text = Translation.About + " Tests Selector";
      Help_MenuItem.Text = Translation.Help;
      CommandLine_MenuItem.Text = Translation.CommandLine;

      //Clickable icons (buttons tooltips)
      this.Icons_ToolTip.SetToolTip(Load_Button, Translation.Open);
      this.Icons_ToolTip.SetToolTip(Save_Button, Translation.Save);
      this.Icons_ToolTip.SetToolTip(Run_Button, Translation.Run);
      this.Icons_ToolTip.SetToolTip(Collapse_Button, Translation.CollapseAll);
      this.Icons_ToolTip.SetToolTip(Expand_Button, Translation.ExpandAll);
      this.Icons_ToolTip.SetToolTip(Select_Button, Translation.SelectAll);
      this.Icons_ToolTip.SetToolTip(Unselect_Button, Translation.UnselectAll);
      this.Icons_ToolTip.SetToolTip(Search_Button, Translation.Search);

      //Labels
      CollapseLevel_Label.Text = Translation.CollapseToLevel;
      Scripts_Label.Text = Translation.ScriptsForRun;
      Variables_File_Label.Text = Translation.File + ":";

      //Check Boxes
      Parent_CB.Text = Translation.Parent_Option;
      Children_CB.Text = Translation.Children_Option;
      MarkModified_CB.Text = Translation.MarkModified_Option;
      Iterations_CB.Text = Translation.Iterations_Option;

      //Tabs
      _settings.Variables_Page.Text = Translation.Variables;
      Tabs_Control.TabPages[0].Text = Translation.TestItems;
      if (Tabs_Control.TabPages.Count > 1) { Tabs_Control.TabPages[1].Text = Translation.Variables; }
      Tabs_Control.Refresh();

      //Context menu
      if (Node_ContextMenu.Items["Iterations"] != null)
        Node_ContextMenu.Items["Iterations"].Text = Translation.ChangeIterations;
      if (Temporary_ContextMenu.Items["Add"] != null)
        Temporary_ContextMenu.Items["Add"].Text = Translation.Add;
      if (Temporary_ContextMenu.Items["Delete"] != null)
        Temporary_ContextMenu.Items["Delete"].Text = Translation.Delete;
      if (Persistent_ContextMenu.Items["Add"] != null)
        Persistent_ContextMenu.Items["Add"].Text = Translation.Add;
      if (Persistent_ContextMenu.Items["Delete"] != null)
        Persistent_ContextMenu.Items["Delete"].Text = Translation.Delete;

      //Variables
      if (TemporaryVariables_DGView.Columns.Count > 0)
      {
        TemporaryVariables_DGView.Columns[0].HeaderText = Translation.VarColumn_Name;
        TemporaryVariables_DGView.Columns[1].HeaderText = Translation.VarColumn_Type;
        TemporaryVariables_DGView.Columns[2].HeaderText = Translation.VarColumn_Default;
        TemporaryVariables_DGView.Columns[3].HeaderText = Translation.VarColumn_Description;
        TemporaryVariables_DGView.Columns[4].HeaderText = Translation.VarColumn_Category;
      }
      if (PersistentVariables_DGView.Columns.Count > 0)
      {
        PersistentVariables_DGView.Columns[0].HeaderText = Translation.VarColumn_Name;
        PersistentVariables_DGView.Columns[1].HeaderText = Translation.VarColumn_Type;
        PersistentVariables_DGView.Columns[2].HeaderText = Translation.VarColumn_Default;
        PersistentVariables_DGView.Columns[3].HeaderText = Translation.VarColumn_Local;
        PersistentVariables_DGView.Columns[4].HeaderText = Translation.VarColumn_Description;
        PersistentVariables_DGView.Columns[5].HeaderText = Translation.VarColumn_Category;
      }
      _translateOperation = false;
    }

    #endregion Initial state

    #region Command line

    private int CommandLine_CommandIndex(string text)
    {
      for (var x = 0; x < _settings.Arguments.Length; x++)
      {
        if (_settings.Arguments[x].ToLower() == text.ToLower())
        {
          return x;
        }
      }
      return -1;
    }

    private void CommandLine()
    {
      if (_settings.Arguments.Length > 0)
      {
        var index = -1;

        #region Help

        index = CommandLine_CommandIndex("-h");
        if (index > -1)
        {
          var _message = MessageBox.Show(Translation.CommandLineHelp);

          //This command only displays help message
          Application.Exit();
          return;
        }

        #endregion Help

        #region Parents

        index = CommandLine_CommandIndex("-p");
        Settings_Change_Parent(index > -1);

        #endregion Parents

        #region Children

        index = CommandLine_CommandIndex("-c");
        Settings_Change_Children(index > -1);

        #endregion Children

        #region Display iterations

        index = CommandLine_CommandIndex("-di");
        Settings_Change_ShowIterations(index > -1);

        #endregion Display iterations

        #region Load file

        index = CommandLine_CommandIndex("-f");
        if (index > -1)
        {
          _settings.File_Path = _settings.Arguments[index + 1];
          Load_File();
        }
        else
        {
          Application.Exit();
        }

        #endregion Load file

        #region Items to select

        index = CommandLine_CommandIndex("-t");
        if (index > -1)
        {
          Select_ItemsFromFile(_settings.Arguments[index + 1]);
        }

        #endregion Items to select

        #region TC path

        index = CommandLine_CommandIndex("-tc");
        if (index > -1)
        {
          Settings_Change_TCPath(_settings.Arguments[index + 1]);
        }

        #endregion TC path

        #region Run

        index = CommandLine_CommandIndex("-r");
        if (index > -1) { Run_Project(); }

        #endregion Run

        #region Save file

        index = CommandLine_CommandIndex("-s");
        if (index > -1)
        {
          Save_File();
        }

        #endregion Save file

        #region Close application

        index = CommandLine_CommandIndex("-x");
        if (index > -1)
        {
          Application.Exit();
        }

        #endregion Close application
      }
    }

    #endregion Command line

    #region Load file

    private void Load_Button_Click(object sender, EventArgs e)
    {
      Load_Operation();
    }

    private void Load_Operation(bool button = true, string _path = "")
    {
      var _continue = Save_File_OnExit(null);

      if (_continue)
      {
        //Reset state of form
        Init_StartState();
        Parent_CB.Enabled = true;
        Children_CB.Enabled = true;
        MarkModified_CB.Enabled = true;
        Iterations_CB.Enabled = true;
        Settings_TCPath_Button.Enabled = true;
        TC_Path_Label.Enabled = true;
        Settings_TCPath_TextBox.Enabled = true;

        //Reset loaded xml files
        _settings.XML_Document = null;
        _settings.PJS_Projects = new Dictionary<string, XmlDocument>();

        //Prepare load file dialog
        var _load = false;
        if (button)
        {
          Load_Dialog = new OpenFileDialog
          {
            Filter = "ProjectSuite/Project|*.pjs;*.mds|ProjectSuite|*.pjs|Project|*.mds",
            FilterIndex = 1,
            Multiselect = false,
            CheckFileExists = true,
            CheckPathExists = true,
            ShowReadOnly = false,
            ValidateNames = true
          };

          //Show dialog and perform load file when selected OK
          if (Load_Dialog.ShowDialog() == DialogResult.OK)
          {
            //File path and name to display
            _settings.File_Path = Load_Dialog.FileName;

            //Load file
            _load = true;
          }
        }
        else if (File.Exists(_path))
        {
          _settings.File_Path = _path;
          _load = true;
        }

        if (_load)
        {
          //Load file
          Load_File();
        }
      }
    }

    private bool Load_ValidateFile(string _filePath = "")
    {
      if (_filePath == "")
      {
        _filePath = _settings.File_Path;
      }

      //Remove Read Only or show warning
      FileInfo fi = new FileInfo(_filePath);
      fi.Attributes = FileAttributes.Normal;
      if (fi.Attributes == FileAttributes.ReadOnly || fi.IsReadOnly)
      {
        MessageBox.Show(Translation.FileReadOnly + "!\n" + _filePath);
        return false;
      }

      return true;
    }

    private void Load_File()
    {
      //Set mode
      switch (Path.GetExtension(_settings.File_Path).ToLower())
      {
        case ".mds":
          Settings_Change_Mode((int)Modes.MDS);
          break;

        case ".pjs":
          Settings_Change_Mode((int)Modes.PJS);
          break;

        default:
          MessageBox.Show(string.Format(Translation.TypeNotSupported, Path.GetExtension(_settings.File_Path)));
          return;
      }

      if (Load_ValidateFile())
      {
        //Remember and change Settings
        var _parents = _settings.Parent_Selection;
        var _children = _settings.Children_Selection;
        Settings_Change_Parent(false);
        Settings_Change_Children(false);
        Collapse_NumericUpDown.Value = 0;

        //Reset original values
        _settings.Original_Iterations.Clear();
        _settings.Original_Selected.Clear();
        _settings.Original_Value.Clear();
        _settings.Original_CheckLevel.Clear();

        //Disable tree painting
        Tree.BeginUpdate();

        //Load file
        switch (_settings.Mode)
        {
          case Modes.PJS:
            Load_ProjectSuiteFile();
            break;

          case Modes.MDS:
          default:
            Load_ProjectFile();
            break;
        }

        //Enable tree painting
        Tree.EndUpdate();

        //Restore Settings
        Settings_Change_Parent(_parents);
        Settings_Change_Children(_children);
      }
    }

    private void Load_ProjectFile()
    {
      try
      {
        this.Cursor = Cursors.WaitCursor;
        _settings.XML_Document = new XmlDocument();
        _settings.XML_Document.Load(_settings.File_Path);

        #region Select starting node

        XmlNode xNode = _settings.XML_Document.DocumentElement;

        //TC 11 root is named "Nodes", TC 12 "Root"
        switch (xNode.Name)
        {
          case "Nodes":
            _settings.TestComplete_Version = TCVersion.TC11;
            break;
          case "Root":
            _settings.TestComplete_Version = TCVersion.TC12;
            break;
          default:
            throw new Exception("Unrecognized Test Complete version in file! Root node name: " + xNode.Name);
        }

        if (_settings.TestComplete_Version == TCVersion.TC11)
        {
          //<Node name="root">
          xNode = xNode.ChildNodes[0];
          //<Node name="test data">
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Attributes["name"].Value == "test data")
            {
              xNode = element;
              break;
            }
          }
          //<Node name="child list">
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Attributes["name"].Value == "child list")
            {
              xNode = element;
              break;
            }
          }
        }

        if (_settings.TestComplete_Version == TCVersion.TC12)
        {
          //<testItems key="xxx">
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Name == "testItems")
            {
              xNode = element;
              break;
            }
          }
          //<children>
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Name == "children")
            {
              xNode = element;
              break;
            }
          }
        }

        #endregion

        Tree.Nodes.Clear();
        Tree.Font = _settings.MDS_Font;
        Tree.ImageList = Icons_ImageList;
        Tree.Nodes.Add(new TreeNode(Path.GetFileName(_settings.File_Path), 0, 0));
        TreeNode tNode = Tree.Nodes[0];
        Tree.Nodes[0].Checked = true;
        MDS_addTreeNode(xNode, tNode);
        Tree.ExpandAll();
      }
      catch (XmlException xExc)
      {
        MessageBox.Show("MDS loading exception:\n" + xExc.Message);
      }
      catch (Exception ex)
      {
        MessageBox.Show("General exception:\n" + ex.Message);
      }
      finally
      {
        this.Cursor = Cursors.Default;
        Load_Complete();
      }
    }

    private void Load_ProjectSuiteFile()
    {
      try
      {
        this.Cursor = Cursors.WaitCursor;
        _settings.XML_Document = new XmlDocument();
        _settings.XML_Document.Load(_settings.File_Path);

        #region Select starting node

        XmlNode xNode = _settings.XML_Document.DocumentElement;

        //TC 11 root is named "Nodes", TC 12 "Root"
        switch (xNode.Name)
        {
          case "Nodes":
            _settings.TestComplete_Version = TCVersion.TC11;
            break;
          case "Root":
            _settings.TestComplete_Version = TCVersion.TC12;
            break;
          default:
            throw new Exception("Unrecognized Test Complete version in file! Root node name: " + xNode.Name);
        }

        if (_settings.TestComplete_Version == TCVersion.TC11)
        {
          //<Node name="root">
          xNode = xNode.ChildNodes[0];
          //<Node name="child list">
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Attributes["name"].Value == "child list")
            {
              xNode = element;
              break;
            }
          }
        }

        if (_settings.TestComplete_Version == TCVersion.TC12)
        {
          //<children>
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Name == "children")
            {
              xNode = element;
              break;
            }
          }
        }

        #endregion
        Tree.Nodes.Clear();
        Tree.Font = _settings.MDS_Font;
        Tree.ImageList = Icons_ImageList;
        Tree.Nodes.Add(new TreeNode(Path.GetFileName(_settings.File_Path), 0, 0));
        TreeNode tNode = Tree.Nodes[0];
        Tree.Nodes[0].Checked = true;
        tNode.SelectedImageIndex = 3;
        tNode.ImageIndex = 3;

        foreach (XmlNode element in xNode.ChildNodes)
        {
          Load_Project_ToProjectSuite(element);
        }

        Tree.ExpandAll();
      }
      catch (XmlException xExc)
      {
        MessageBox.Show("PJS loading exception:\n" + xExc.Message);
      }
      catch (Exception ex)
      {
        MessageBox.Show("General exception:\n" + ex.Message);
      }
      finally
      {
        this.Cursor = Cursors.Default;
        Load_Complete();
      }
    }

    private void Load_Project_ToProjectSuite(XmlNode node)
    {
      //node item from ProjectSuite
      var _node = node;
      var _proper = false;

      #region Search for path to project file

      var _path = _settings.File_Path;
      _path = _path.Replace(Path.GetFileName(_path), "");

      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Attributes["name"].Value == "item data")
          {
            _node = element;
            break;
          }
        }
        //_node "item data" node from item
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Attributes["name"].Value == "relpath")
          {
            _path += element.Attributes["value"].Value;
            break;
          }
        }
      }

      if (_settings.TestComplete_Version == TCVersion.TC12)
      {
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Name == "data")
          {
            _path += element.Attributes["path"].Value;
            break;
          }
        }
      }

      #endregion Search for path to project file

      //Validate project file
      if (!Load_ValidateFile(_path))
      {
        return;
      }

      #region Search for key value to project file

      var _key = "";
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        foreach (XmlNode element in node.ChildNodes)
        {
          if (element.Attributes["name"].Value == "key")
          {
            _key = element.Attributes["value"].Value;
            break;
          }
        }
      }
      else
      {
        //tc 12
        _key = node.Attributes["key"].Value;
      }


      #endregion Search for key value to project file

      #region Search for node in test items for founded key

      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        _node = _settings.XML_Document.DocumentElement.ChildNodes[0];
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Attributes["name"].Value == "test data")
          {
            _node = element;
            break;
          }
        }
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Attributes["name"].Value == "child list")
          {
            _node = element;
            break;
          }
        }
        //_node is now "child list" from "test data"
        //iterate items
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Attributes["name"].Value.Contains("item"))
          {
            _node = element;
            foreach (XmlNode prp in _node.ChildNodes)
            {
              if (prp.Attributes["name"].Value == "key")
              {
                if (prp.Attributes["value"].Value == _key)
                {
                  _proper = true;
                  break;
                }
              }
            }

            if (_proper)
            {
              break;
            }
          }
        }
      }
      else
      {
        //TC 12
        _node = _settings.XML_Document.DocumentElement;
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Name == "testItems")
          {
            _node = element;
            break;
          }
        }
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Name == "children")
          {
            _node = element;
            break;
          }
        }
        foreach (XmlNode element in _node.ChildNodes)
        {
          if (element.Name == "testItem")
          {
            _node = element;
            if (element.Attributes["key"].Value == _key)
            {
              _proper = true;
              break;
            }
          }
        }
      }

      #endregion Search for node in test items for founded key

      try
      {
        _settings.PJS_Projects.Add(_path, new XmlDocument());
        _settings.PJS_Projects[_path].Load(_path);

        #region Select to start "child test" node under "test data"

        XmlNode xNode = _settings.PJS_Projects[_path].DocumentElement;
        if (_settings.TestComplete_Version == TCVersion.TC11)
        {
          xNode = xNode.ChildNodes[0];
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Attributes["name"].Value == "test data")
            {
              xNode = element;
              break;
            }
          }
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Attributes["name"].Value == "child list")
            {
              xNode = element;
              break;
            }
          }
        }
        else
        {
          //tc 12
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Name == "testItems")
            {
              xNode = element;
              break;
            }
          }
          foreach (XmlNode element in xNode.ChildNodes)
          {
            if (element.Name == "children")
            {
              xNode = element;
              break;
            }
          }
        }

        #endregion Select to start "child test" node under "test data"

        Tree.Nodes[0].Nodes.Add(new TreeNode(Path.GetFileName(_path), 0, 0));
        TreeNode tNode = Tree.Nodes[0].LastNode;
        tNode.Tag = _node;
        tNode.Name = Path.GetFileName(_path);
        tNode.ImageIndex = 0;
        tNode.SelectedImageIndex = 0;
        tNode.Checked = MDS_check_ItemSelected(_node);
        if (!_settings.Original_Selected.ContainsKey(tNode))
        {
          _settings.Original_Selected.Add(tNode, tNode.Checked);
        }
        MDS_addTreeNode(xNode, tNode);
      }
      catch (XmlException xExc)
      {
        MessageBox.Show("MDS loading exception:\n" + xExc.Message);
      }
      catch (Exception ex)
      {
        MessageBox.Show("General exception:\n" + ex.Message);
      }
    }

    private void Load_PrepareFileList()
    {
      //Loaded project and project suite files list
      _settings.Files_List = new Dictionary<string, XmlDocument>();
      _settings.Files_List.Add(_settings.File_Path, _settings.XML_Document);
      foreach (KeyValuePair<string, XmlDocument> item in _settings.PJS_Projects)
      {
        _settings.Files_List.Add(item.Key, item.Value);
      }

      //Laval variables files list
      _settings.LocalVariables_FilesList = new Dictionary<string, XmlDocument>();
      foreach (KeyValuePair<string, XmlDocument> item in _settings.Files_List)
      {
        var _path = item.Key;
        _path += ".tcLS"; //local variables file path
        if (File.Exists(_path))
        {
          try
          {
            var doc = new XmlDocument();
            doc.Load(_path);
            _settings.LocalVariables_FilesList.Add(item.Key, doc);
          }
          catch (XmlException xExc)
          {
            MessageBox.Show("tcLS loading exception:\n" + xExc.Message);
          }
          catch (Exception ex)
          {
            MessageBox.Show("General exception:\n" + ex.Message);
          }
        }
      }
    }

    private void Load_Complete()
    {
      Load_PrepareFileList();
      Tree.Visible = true;
      Tabs_Control.Visible = true;
      _settings.Modified = false;
      Expand_Button.Enabled = true;
      Collapse_Button.Enabled = true;
      Search_TB.Enabled = true;
      Search_Button.Enabled = true;
      _settings.Load_Complete = true;
      Select_Button.Enabled = true;
      Unselect_Button.Enabled = true;
      File_TestsFile_Import_MenuItem.Enabled = true;
      File_TestsFile_Export_MenuItem.Enabled = true;
      _settings.MDS_Loaded = true;
      Collapse_NumericUpDown.Enabled = true;
      CollapseLevel_Label.Enabled = true;
      Collapse_NumericUpDown.Value = 2;
      File_Run_MenuItem.Enabled = !string.IsNullOrEmpty(_settings.TestComplete_Path) && File.Exists(_settings.TestComplete_Path);
      Run_Button.Enabled = File_Run_MenuItem.Enabled;
      Variables_Combo.DisplayMember = "Key";
      Variables_Combo.ValueMember = "Value";
      Variables_Combo.DataSource = new BindingSource(_settings.Files_List, null);
      Tree_CountScripts();
      InitTranslation(_settings, null);
    }

    #endregion Load file

    #region Exit

    private void TS_Main_FormClosing(object sender, FormClosingEventArgs e)
    {
      var _continue = Save_File_OnExit(e);
      if (_continue)
      {
        //Saves Settings
        Settings_Hide();
        TS_Main_SaveSettings();
      }
    }

    private void TS_Main_SaveSettings()
    {
      TestsSelector.Properties.Settings.Default.TC_Path = _settings.TestComplete_Path;
      TestsSelector.Properties.Settings.Default.Parents = _settings.Parent_Selection;
      TestsSelector.Properties.Settings.Default.Children = _settings.Children_Selection;
      TestsSelector.Properties.Settings.Default.Iterations = _settings.Display_Iterations;
      TestsSelector.Properties.Settings.Default.MarkEdited = _settings.Mark_Edited;
      Properties.Settings.Default.Save();
    }

    #endregion Exit

    #region Search

    private void Search_Button_Click(object sender, EventArgs e)
    {
      //Remember and change Settings
      var _parents = _settings.Parent_Selection;
      var _children = _settings.Children_Selection;
      Settings_Change_Parent(false);
      Settings_Change_Children(false);
      _searchOperation = true;

      //Search and select
      Search_SearchAndSelect(this.Search_TB.Text);

      //Restore Settings
      _searchOperation = false;
      Settings_Change_Parent(_parents);
      Settings_Change_Children(_children);
    }

    private void Search_SearchAndSelect(string searchText)
    {
      Tree.SelectedNode = Tree.Nodes[0];
      if (!_searchOperation)
      {
        foreach (TreeNode node in _settings.Search_CurrentNodeMatches)
        {
          if (_settings.ModifiedNodes.Contains(node) && _settings.Mark_Edited)
          {
            Tree_Modified_BackgroundColor(node);
          }
          else
          {
            node.BackColor = Color.Transparent;
          }
        }
      }
      if (String.IsNullOrEmpty(searchText))
      {
        _settings.Search_LastSearchText = "";
        return;
      };

      if (_settings.Search_LastSearchText != searchText)
      {
        //It's a new Search
        _settings.Search_CurrentNodeMatches.Clear();
        _settings.Search_LastSearchText = searchText;
        _settings.Search_LastNodeIndex = 0;
        Search_GetNodes(searchText, Tree.Nodes[0]);
      }

      if (_settings.Search_LastNodeIndex == _settings.Search_CurrentNodeMatches.Count)
      {
        _settings.Search_LastNodeIndex = 0;
      }

      if (_settings.Search_LastNodeIndex >= 0 && _settings.Search_CurrentNodeMatches.Count > 0 && _settings.Search_LastNodeIndex < _settings.Search_CurrentNodeMatches.Count)
      {
        TreeNode selectedNode = _settings.Search_CurrentNodeMatches[_settings.Search_LastNodeIndex];
        _settings.Search_LastNodeIndex++;
        this.Tree.SelectedNode = selectedNode;
        this.Tree.Focus();
      }
    }

    private void Search_GetNodes(string SearchText, TreeNode StartNode)
    {
      while (StartNode != null)
      {
        var _nodeText = StartNode.Text;
        if (_nodeText.ToLower().Contains(SearchText.ToLower()))
        {
          StartNode.BackColor = Color.Yellow;
          _settings.Search_CurrentNodeMatches.Add(StartNode);
        };
        if (StartNode.Nodes.Count != 0)
        {
          Search_GetNodes(SearchText, StartNode.Nodes[0]);
        };
        StartNode = StartNode.NextNode;
      };
    }

    private void Search_TB_TextChanged(object sender, EventArgs e)
    {
      Tree.BeginUpdate();

      //Remember and change Settings
      var _parents = _settings.Parent_Selection;
      var _children = _settings.Children_Selection;
      Settings_Change_Parent(false);
      Settings_Change_Children(false);

      //Search and select
      Search_SearchAndSelect(this.Search_TB.Text);
      var _node = Tree.SelectedNode;
      if (String.IsNullOrEmpty(Search_TB.Text))
      { //empty search collapse to initial level tree
        var _level = Decimal.ToInt32(Collapse_NumericUpDown.Value);
        Collapse_NodesToLevel(Tree.Nodes[0], _level);
      }
      else if (_settings.Search_CurrentNodeMatches.Count > 0) //search not empty and there are some matching results
      {
        Tree.CollapseAll();
        for (var x = _settings.Search_CurrentNodeMatches.Count - 1; x > -1; x--)
        {
          TreeNode child = _settings.Search_CurrentNodeMatches[x];
          var _parent = child.Parent;
          while (_parent != null)
          {
            _parent.Expand();
            _parent = _parent.Parent;
          }
        }
        //Tree.SelectedNode = _settings.Search_CurrentNodeMatches[0];
        Tree.Nodes[0].EnsureVisible();
      }
      _settings.Search_LastNodeIndex = 0;
      Tree.SelectedNode = _node;

      //Restore Settings
      Tree.EndUpdate();
      Settings_Change_Parent(_parents);
      Settings_Change_Children(_children);
      Search_TB.Focus();
    }

    private void Search_RefreshSearch()
    {
      //Refresh highlight if search was used
      if (!string.IsNullOrEmpty(Search_TB.Text))
      {
        var _control = TS_Main_FindFocusedControl();
        var _node = Tree.SelectedNode;
        var _temp = Search_TB.Text;
        Search_TB.Text = "";
        Search_TB.Text = _temp;
        if (_control != null)
        {
          _control.Focus();
        }
        if (_node != null)
        {
          Tree.SelectedNode = _node;
        }
      }
    }

    #endregion Search

    #region Settings

    private void Settings_Hide()
    {
      //Disabled on long saving (mostly on init TC path)
      this.Cursor = Cursors.WaitCursor;
      Settings_TCPath_TextBox.Enabled = false;
      this.Enabled = false;

      //Change Settings
      Settings_Change_Parent(Parent_CB.Checked);
      Settings_Change_Children(Children_CB.Checked);
      Settings_Change_ShowIterations(Iterations_CB.Checked);
      Settings_Change_MarkModified(MarkModified_CB.Checked);
      Settings_Change_TCPath(Settings_TCPath_TextBox.Text);

      //Hide Settings
      Parent_CB.Visible = false;
      Children_CB.Visible = false;
      MarkModified_CB.Visible = false;
      Iterations_CB.Visible = false;
      TC_Path_Label.Visible = false;
      Settings_TCPath_TextBox.Visible = false;
      Settings_TCPath_Button.Visible = false;

      //Enable
      File_Open_MenuItem.Enabled = true;
      Load_Button.Enabled = File_Open_MenuItem.Enabled;

      //Any file loaded
      Tree.Visible = _settings.Load_Complete;
      Tabs_Control.Visible = _settings.Load_Complete;
      File_Save_MenuItem.Enabled = _settings.Modified;
      Save_Button.Enabled = File_Save_MenuItem.Enabled;
      Collapse_Button.Enabled = _settings.Load_Complete;
      Expand_Button.Enabled = _settings.Load_Complete;
      Search_Button.Enabled = _settings.Load_Complete;
      Search_TB.Enabled = _settings.Load_Complete;

      //MDS
      Unselect_Button.Enabled = _settings.MDS_Loaded;
      Select_Button.Enabled = _settings.MDS_Loaded;
      File_TestsFile_Import_MenuItem.Enabled = _settings.MDS_Loaded;
      File_TestsFile_Export_MenuItem.Enabled = _settings.MDS_Loaded;
      Collapse_NumericUpDown.Enabled = _settings.MDS_Loaded;
      CollapseLevel_Label.Enabled = _settings.MDS_Loaded;
      File_Run_MenuItem.Enabled = _settings.MDS_Loaded && !string.IsNullOrEmpty(_settings.TestComplete_Path) && File.Exists(_settings.TestComplete_Path);
      Run_Button.Enabled = File_Run_MenuItem.Enabled;

      //Enable after settings saved
      this.Enabled = true;
      Settings_TCPath_TextBox.Enabled = true;
      this.Cursor = Cursors.Default;
    }

    private void Settings_Show()
    {
      //Settings
      Parent_CB.Visible = true;
      Parent_CB.Checked = _settings.Parent_Selection;
      Children_CB.Visible = true;
      Children_CB.Checked = _settings.Children_Selection;
      MarkModified_CB.Visible = true;
      MarkModified_CB.Checked = _settings.Mark_Edited;
      Iterations_CB.Visible = true;
      Iterations_CB.Checked = _settings.Display_Iterations;
      TC_Path_Label.Visible = true;
      Settings_TCPath_TextBox.Visible = true;
      Settings_TCPath_Button.Visible = true;

      //Buttons and other elements
      Tree.Visible = false;
      Tabs_Control.Visible = false;
      File_Open_MenuItem.Enabled = false;
      Load_Button.Enabled = File_Open_MenuItem.Enabled;
      Unselect_Button.Enabled = false;
      Select_Button.Enabled = false;
      File_Save_MenuItem.Enabled = false;
      Save_Button.Enabled = File_Save_MenuItem.Enabled;
      File_TestsFile_Import_MenuItem.Enabled = false;
      File_TestsFile_Export_MenuItem.Enabled = false;
      Collapse_Button.Enabled = false;
      Expand_Button.Enabled = false;
      Search_TB.Enabled = false;
      Search_Button.Enabled = false;
      Collapse_NumericUpDown.Enabled = false;
      CollapseLevel_Label.Enabled = false;
      File_Run_MenuItem.Enabled = false;
      Run_Button.Enabled = File_Run_MenuItem.Enabled;
    }

    private void Settings_Button_CheckedChanged(object sender, EventArgs e)
    {
      if (Settings_MenuItem.Checked)
      {
        Settings_MenuItem.BackColor = SystemColors.ControlDark;
        Settings_Show();
      }
      else
      {
        Settings_MenuItem.BackColor = SystemColors.Control;
        Settings_Hide();
      }
      TS_Menu.Focus();
    }

    private void Settings_Change_Mode(int state)
    {
      if (state != (int)_settings.Mode)
      {
        Init_StartState();
        _settings.Mode = (Modes)state;
        Tree.CheckBoxes = true;
        Tree.ShowNodeToolTips = false;
        //Add Variables tab if it is removed
        if (!Tabs_Control.TabPages.ContainsKey("Variables_Page"))
        {
          Tabs_Control.TabPages.Add(_settings.Variables_Page);
        }
        //Rename items tab and change icon
        Tabs_Control.TabPages["Tests_Page"].ImageIndex = 5;
        Tabs_Control.TabPages["Tests_Page"].Text = "Test Items";

        Parent_CB.Enabled = true;
        Children_CB.Enabled = true;
        Iterations_CB.Enabled = true;
        Settings_TCPath_Button.Enabled = true;
        TC_Path_Label.Enabled = true;
        Settings_TCPath_TextBox.Enabled = true;
      }
    }

    private void Settings_Change_Parent(bool state)
    {
      _settings.Parent_Selection = state;
      Parent_CB.Checked = state;
    }

    private void Settings_Change_Children(bool state)
    {
      _settings.Children_Selection = state;
      Children_CB.Checked = state;
    }

    private void Settings_Change_MarkModified(bool state)
    {
      _settings.Mark_Edited = state;
      MarkModified_CB.Checked = state;
      foreach (TreeNode node in _settings.ModifiedNodes)
      {
        if (state)
        {
          Tree_Modified_BackgroundColor(node);
        }
        else
        {
          node.BackColor = Color.Transparent;
        }
      }
    }

    private void Settings_Change_ShowIterations(bool state)
    {
      _settings.Display_Iterations = state;
      Iterations_CB.Checked = state;
      if (_settings.MDS_Loaded)
      {
        this.Cursor = Cursors.WaitCursor;
        this.Enabled = false;
        Tree.BeginUpdate();
        Iterations_ChangeNodesText(Tree.Nodes[0]);
        Tree.EndUpdate();
        this.Enabled = true;
        this.Cursor = Cursors.Default;
      }
    }

    private void Settings_Change_TCPath(string _path)
    {
      _settings.TestComplete_Path = _path;
      Init_TCPath();
      Settings_TCPath_TextBox.Text = _settings.TestComplete_Path;
    }

    private void Settings_TCPath_Button_Click(object sender, EventArgs e)
    {
      //Prepare load file dialog
      Load_Dialog = new OpenFileDialog
      {
        Filter = "TestExecute/TestComplete|TestComplete.exe;TestExecute.exe|TestExecute|TestExecute.exe|TestComplete|TestComplete.exe",
        FilterIndex = 1,
        Multiselect = false,
        CheckFileExists = true,
        CheckPathExists = true,
        ShowReadOnly = false,
        ValidateNames = true
      };

      if (File.Exists(_settings.TestComplete_Path)) Load_Dialog.InitialDirectory = _settings.TestComplete_Path;

      //Show dialog
      if (Load_Dialog.ShowDialog() == DialogResult.OK)
      {
        Settings_Change_TCPath(Load_Dialog.FileName);
      }
    }

    private void Settings_TCPath_TextBox_TextChanged(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(Settings_TCPath_TextBox.Text))
      {
        Settings_Change_TCPath(Settings_TCPath_TextBox.Text);
      }
    }

    #endregion Settings

    #region Resize

    private void TS_Main_Resize(object sender, EventArgs e)
    {
      //Tabs size
      Tabs_Control.Size = new Size(this.Size.Width - 40, this.Size.Height - 150);
      //Tree with test items
      Tree.Size = new Size(Tabs_Control.Size.Width - 6, Tabs_Control.Size.Height - 24);

      //Variables page
      var _half = Tabs_Control.Size.Height / 2;
      //Temporary Variables
      TemporaryVariables_Label.Location = new Point(1, 27);
      TemporaryVariables_DGView.Location = new Point(1, 52);
      TemporaryVariables_Label.Size = new Size(Tabs_Control.Size.Width - 14, 23);
      TemporaryVariables_DGView.Size = new Size(Tabs_Control.Size.Width - 14, _half - 88);

      //Persistent Variables
      PersistentVariables_Label.Location = new Point(1, _half - 33);
      PersistentVariables_DGView.Location = new Point(1, _half - 8);
      PersistentVariables_DGView.Size = new Size(Tabs_Control.Size.Width - 14, _half - 25);
      PersistentVariables_Label.Size = new Size(Tabs_Control.Size.Width - 14, 23);
    }

    #endregion Resize

    #region Save

    private void Save_OnModification(object sender, EventArgs e)
    {
      File_Save_MenuItem.Enabled = _settings.Modified;
      Save_Button.Enabled = File_Save_MenuItem.Enabled;
    }

    private void Save_Button_Click(object sender, EventArgs e)
    {
      Save_File();
    }

    private void Save_XMLFile()
    {
      using (XmlTextWriter wr = new XmlTextWriter(_settings.File_Path, Encoding.UTF8))
      {
        wr.Formatting = Formatting.Indented;
        wr.Indentation = 2;
        wr.IndentChar = " ".ToCharArray()[0];
        _settings.XML_Document.Save(wr);
      }
      //Skip xml version info - not needed in GlobalConfig
      var _text = File.ReadAllLines(_settings.File_Path).Skip(1);
      File.WriteAllLines(_settings.File_Path, _text);
    }

    private void Save_ProjectFile()
    {
      #region Save each loaded project file

      foreach (KeyValuePair<string, XmlDocument> doc in _settings.Files_List)
      {
        var _file = doc.Key;

        //Remove read only
        if (File.Exists(_file))
        {
          FileInfo fi = new FileInfo(_file)
          {
            Attributes = FileAttributes.Normal
          };
        }

        using (XmlTextWriter wr = new XmlTextWriter(_file, Encoding.UTF8))
        {
          wr.Formatting = Formatting.Indented;
          wr.Indentation = 1;
          wr.IndentChar = "\t".ToCharArray()[0];
          doc.Value.Save(wr);
        }

        //Replace " />" to "/>" in file
        //Replace "utf-8" to "UTF-8"
        //Replace "<!DOCTYPE Nodes[" to "<!DOCTYPE Nodes ["
        var _text = File.ReadAllText(_file);
        _text = _text.Replace(" />", "/>");
        _text = _text.Replace("utf-8", "UTF-8");
        _text = _text.Replace("<!DOCTYPE Nodes[", "<!DOCTYPE Nodes [");
        File.WriteAllText(_file, _text);
      }

      #endregion Save each loaded project file

      #region Save each loaded local variables file

      foreach (KeyValuePair<string, XmlDocument> doc in _settings.LocalVariables_FilesList)
      {
        var _file = doc.Key + ".tcLS";

        //Remove read only
        if (File.Exists(_file))
        {
          FileInfo fi = new FileInfo(_file)
          {
            Attributes = FileAttributes.Normal
          };
        }

        using (XmlTextWriter wr = new XmlTextWriter(_file, Encoding.UTF8))
        {
          wr.Formatting = Formatting.Indented;
          wr.Indentation = 1;
          wr.IndentChar = "\t".ToCharArray()[0];
          doc.Value.Save(wr);
        }

        //Replace " />" to "/>" in file
        //Replace "utf-8" to "UTF-8"
        //Replace "<!DOCTYPE Nodes[" to "<!DOCTYPE Nodes ["
        var _text = File.ReadAllText(_file);
        _text = _text.Replace(" />", "/>");
        _text = _text.Replace("utf-8", "UTF-8");
        _text = _text.Replace("<!DOCTYPE Nodes[", "<!DOCTYPE Nodes [");
        File.WriteAllText(_file, _text);
      }

      #endregion Save each loaded local variables file
    }

    private void Save_File()
    {
      //Check if there is some file loaded and changed
      if (_settings.Load_Complete && _settings.Modified)
      {
        Save_ProjectFile();
      }

      //Update modified nodes and save state
      _settings.Modified = false;
      foreach (TreeNode node in _settings.ModifiedNodes)
      {
        Save_ReplaceOriginal(node);
        node.BackColor = Color.Transparent;
      }
      _settings.ModifiedNodes.Clear();

      //Refresh highlight if search was used
      Search_RefreshSearch();
    }

    private void Save_ReplaceOriginal(TreeNode node)
    {
      _settings.Original_Selected[node] = node.Checked;
      _settings.Original_Iterations[node] = node.ToolTipText;
    }

    private bool Save_File_OnExit(CancelEventArgs e)
    {
      var _return = true; //true if we could continue closing
      if (_settings.Modified)
      {
        var window = MessageBox.Show(Translation.CloseQuestion, Translation.Warning, MessageBoxButtons.YesNoCancel);
        switch (window)
        {
          case DialogResult.Yes:
            //Save before close
            Save_File();
            _return = true;
            break;

          case DialogResult.No:
            //Don't save but continue closing - return true
            _return = true;
            break;

          default: //case DialogResult.Cancel:
            if (e != null)
            {
              //Cancel closing operation
              e.Cancel = true;
            }
            _return = false;
            break;
        }
      }
      return _return;
    }

    private void TS_Main_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.S)
      {
        Save_File();
      }
    }

    private void Tabs_Control_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.S)
      {
        Save_File();
      }
    }

    private void TemporaryVariables_DGView_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.S)
      {
        Save_File();
      }
    }

    private void PersistentVariables_DGView_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.S)
      {
        Save_File();
      }
    }

    #endregion Save

    #region Tree operations

    #region Expand/Collapse

    private void Expand_Button_Click(object sender, EventArgs e)
    {
      //Remember and change Settings
      var _parents = _settings.Parent_Selection;
      var _children = _settings.Children_Selection;
      Settings_Change_Parent(false);
      Settings_Change_Children(false);

      //Expand
      Tree.BeginUpdate();
      Tree.ExpandAll();
      Collapse_NumericUpDown.Value = _settings.Max_Level;
      Tree.EndUpdate();

      //Restore Settings
      Settings_Change_Parent(_parents);
      Settings_Change_Children(_children);
    }

    private void Collapse_Button_Click(object sender, EventArgs e)
    {
      //Remember and change Settings
      var _parents = _settings.Parent_Selection;
      var _children = _settings.Children_Selection;
      Settings_Change_Parent(false);
      Settings_Change_Children(false);

      //Collapse
      Tree.BeginUpdate();
      Collapse_NodesToLevel(Tree.Nodes[0], 0);
      Collapse_NumericUpDown.Value = 0;
      Tree.EndUpdate();

      //Restore Settings
      Settings_Change_Parent(_parents);
      Settings_Change_Children(_children);
    }

    private void Collapse_NumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      //Remember and change Settings
      var _parents = _settings.Parent_Selection;
      var _children = _settings.Children_Selection;
      Settings_Change_Parent(false);
      Settings_Change_Children(false);

      //Collapse
      Tree.BeginUpdate();
      Collapse_NodesToLevel(Tree.Nodes[0], Decimal.ToInt32(Collapse_NumericUpDown.Value));
      Tree.EndUpdate();

      //Restore Settings
      Settings_Change_Parent(_parents);
      Settings_Change_Children(_children);
    }

    private void Collapse_NodesToLevel(TreeNode node, int level)
    {
      if (node.Level >= level)
      {
        node.Collapse(true);
      }
      else
      {
        node.Expand();
      }
      foreach (TreeNode child in node.Nodes)
      {
        Collapse_NodesToLevel(child, level);
      }
    }

    #endregion Expand/Collapse

    #region Select/Unselect

    private void Select_Button_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      Tree.BeginUpdate();
      Collapse_NumericUpDown.Value = _settings.Max_Level;
      Select_NodeWithChildren(Tree.Nodes[0]);
      Tree_CountScripts();
      Tree.EndUpdate();
      this.Cursor = Cursors.Default;
    }

    private void Select_NodeWithChildren(TreeNode node)
    {
      if (!node.Checked)
      {
        node.Checked = true;
        Select_StateChanged(node);
      }
      foreach (TreeNode child in node.Nodes)
      {
        Select_NodeWithChildren(child);
      }
    }

    private void Unselect_Button_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      Tree.BeginUpdate();
      Tree.Enabled = false;
      Collapse_NumericUpDown.Value = _settings.Max_Level;
      Unselect_NodeWithChildren(Tree.Nodes[0]);
      Tree_CountScripts();
      Tree.EndUpdate();
      Tree.Enabled = true;
      this.Cursor = Cursors.Default;
    }

    private void Unselect_NodeWithChildren(TreeNode node)
    {
      if (node.Checked)
      {
        node.Checked = false;
        Select_StateChanged(node);
      }
      foreach (TreeNode child in node.Nodes)
      {
        Unselect_NodeWithChildren(child);
      }
    }

    private void Select_CopyToChildren(TreeNode node)
    {
      if (node.Checked)
      {
        Select_NodeWithChildren(node);
      }
      else
      {
        Unselect_NodeWithChildren(node);
      }
    }

    private void Select_CopyToParents(TreeNode node)
    {
      TreeNode _parent;
      if (node.Checked && node.Parent != null)
      {
        _parent = node.Parent;
        if (!_parent.Checked)
        {
          _parent.Checked = true;
          Select_StateChanged(_parent);
        }
        Select_CopyToParents(_parent);
      }
    }

    private void Select_ItemsFromFile(string FilePath)
    {
      if (_settings.MDS_Loaded)
      {
        //Save settings
        var _children = _settings.Children_Selection;
        var _parent = _settings.Parent_Selection;

        //Prepare tree view and settings
        Unselect_NodeWithChildren(Tree.Nodes[0]); //none selected
        Settings_Change_Parent(true); //parents will be selected to run selected tests
        Settings_Change_Children(false); //children items won't be selected

        //Load all items from file
        var _tests = File.ReadAllLines(FilePath);

        foreach (string _test in _tests)
        {
          //Split test identifier from iterations
          var _line = _test.Split(";".ToCharArray()[0]);
          var _identifier = _line[0];
          var _iterations = "1"; //by default 1 iteration
          if (_line.Length > 1)
          {
            _iterations = _line[1];
          }

          Search_SearchAndSelect(_identifier);
          var _node = Tree.SelectedNode;
          if (_node != null)
          {
            _node.Checked = true;
            _node.ToolTipText = _iterations;
            Select_StateChanged(_node);
            Iterations_ChangeNode(_node);
            Select_CopyToParents(_node);
          }
        }

        //Restore settings
        Settings_Change_Parent(_parent);
        Settings_Change_Children(_children);

        //Count scripts
        Tree_CountScripts();
      }
    }

    private void Select_StateChanged(TreeNode node)
    {
      var _value = node.Checked ? "-1" : "0";
      XmlNode xmlNode = (XmlNode)node.Tag;
      if (xmlNode == null) return;

      //##### Change value in loaded XML document
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        foreach (XmlNode element in xmlNode.ChildNodes)
        {
          if (element.Attributes["name"].Value == "enabled")
          {
            element.Attributes["value"].Value = _value;
            break;
          }
        }
      }
      else
      {
        //TC 12
        xmlNode.Attributes["enabled"].Value = node.Checked ? "True" : "False";
      }

      Tree_Modified(node);
    }

    private void Select_SetForNode(TreeNode node)
    {
      this.Cursor = Cursors.WaitCursor;
      //Check node
      Select_StateChanged(node);

      //Parent selection option
      if (_settings.Parent_Selection)
      {
        Select_CopyToParents(node);
      }

      //Children selection option
      if (_settings.Children_Selection)
      {
        Select_CopyToChildren(node);
      }
      this.Cursor = Cursors.Default;
    }

    #endregion Select/Unselect

    #region Tests Files

    private void TestsFiles_Import_Click(object sender, EventArgs e)
    {
      //Prepare load file dialog
      Load_Dialog = new OpenFileDialog
      {
        Filter = "*.txt|*.txt",
        FilterIndex = 1,
        Multiselect = false,
        CheckFileExists = true,
        CheckPathExists = true,
        ShowReadOnly = false,
        ValidateNames = true
      };

      //Show dialog and perform load file when selected OK
      if (Load_Dialog.ShowDialog() == DialogResult.OK)
      {
        Select_ItemsFromFile(Load_Dialog.FileName);
      }
    }

    private void TestsFiles_Export_Click(object sender, EventArgs e)
    {
      //Prepare save dialog
      Save_Dialog = new SaveFileDialog
      {
        Filter = "*.txt|*.txt",
        FilterIndex = 1,
        CheckPathExists = true,
        ValidateNames = true,
        AddExtension = true,
        OverwritePrompt = true,
        CreatePrompt = true
      };

      //Show dialog and perform save file when selected OK
      if (Save_Dialog.ShowDialog() == DialogResult.OK)
      {
        TestsFiles_Export(Save_Dialog.FileName);
      }
    }

    private void TestsFiles_Export(string path)
    {
      if (_settings.MDS_Loaded)
      {
        var file = File.Create(path);
        file.Close();
        TestsFiles_Export_FillFile(Tree.Nodes[0], path);
      }
    }

    private void TestsFiles_Export_FillFile(TreeNode node, string path)
    {
      if (node.Checked)
      {
        if (!string.IsNullOrEmpty(node.Name) && !string.IsNullOrEmpty(node.ToolTipText) && MDS_getItemHaveScript((XmlNode)node.Tag))
        {
          string _line = node.Name.Trim() + ";" + node.ToolTipText.Trim();
          StreamWriter file = new StreamWriter(path, true, Encoding.UTF8);
          file.WriteLine(_line);
          file.Close();
        }
        foreach (TreeNode child in node.Nodes)
        {
          TestsFiles_Export_FillFile(child, path);
        }
      }
    }

    #endregion Tests Files

    #region Iterations

    private void Iterations_Change(object sender, EventArgs e)
    {
      var _node = Tree.SelectedNode;
      if (_node != Tree.Nodes[0])
      {
        var _edit = new Edit
        {
          _node = _node,
          _modeMDS = true
        };

        if (_edit.ShowDialog(this) == DialogResult.OK)
        {
          Iterations_ChangeNode(_node);
        }

        _edit.Dispose();
      }
    }

    private void Iterations_ChangeNodesText(TreeNode node)
    {
      var _level = _settings.Mode == Modes.MDS ? 0 : 1;
      if (node.Level > _level)
      {
        //Change text of node
        node.Text = _settings.Display_Iterations ? String.Format("[{0}] ::: {1}", node.ToolTipText, node.Name) : node.Name;
      }
      foreach (TreeNode child in node.Nodes)
      {
        Iterations_ChangeNodesText(child);
      }
    }

    private void Iterations_ChangeNode(TreeNode node)
    {
      XmlNode xmlNode = (XmlNode)node.Tag;
      if (xmlNode == null) return;

      //##### Change value in loaded XML document
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        foreach (XmlNode element in xmlNode.ChildNodes)
        {
          if (element.Attributes["name"].Value == "count")
          {
            element.Attributes["value"].Value = node.ToolTipText;
            break;
          }
        }
      }
      else
      {
        //tc 12
        if (xmlNode.Attributes != null && xmlNode.Attributes["count"] != null)
        {
          xmlNode.Attributes["count"].Value = node.ToolTipText;
        }
      }

      node.Text = _settings.Display_Iterations ? String.Format("[{0}] ::: {1}", node.ToolTipText, node.Name) : node.Name;
      Tree_Modified(node);
    }

    #endregion Iterations

    #region Events

    private void Tree_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (_settings.MDS_Loaded)
      {
        _settings.MDS_Loaded = false; //start selection = start loading
        Tree.BeginUpdate();
        Select_SetForNode(e.Node);
        Tree.EndUpdate();
        _settings.MDS_Loaded = true;
      }
    }

    private void Tree_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        var _level = _settings.Mode == Modes.MDS ? 0 : 1;

        // Select the clicked node
        Tree.SelectedNode = Tree.GetNodeAt(e.X, e.Y);

        if (Tree.SelectedNode != null && Tree.SelectedNode.Level > _level)
        {
          Node_ContextMenu.Show(Tree, e.Location);
        }
      }
      Tree_CountScripts();
    }

    private void Tree_KeyUp(object sender, KeyEventArgs e)
    {
      Tree_CountScripts();
    }

    private void Tree_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.S)
      {
        Save_File();
      }
    }

    #endregion Events

    #region Count scripts for run

    private void Tree_CountScripts()
    {
      var _count = Tree_CountScripts(Tree.Nodes[0]);
      ScriptsCount_Label.Text = _count.ToString();
      Scripts_Label.Visible = true;
      ScriptsCount_Label.Visible = Scripts_Label.Visible;
    }

    private int Tree_CountScripts(TreeNode node)
    {
      var _return = 0;
      if (node.Checked)
      {
        _return = MDS_getItemHaveScript((XmlNode)node.Tag) ? 1 : 0;
        foreach (TreeNode child in node.Nodes)
        {
          _return += Tree_CountScripts(child);
        }
      }
      return _return;
    }

    #endregion Count scripts for run

    #region Background color - modified

    private void Tree_Modified(TreeNode node)
    {
      bool _modified = true;

      if (node != null) //tree edition - check if value changed
      {
        //Compare selection and iterations
        var _modifiedIterations = false;
        if (_settings.Original_Iterations.ContainsKey(node))
        {
          _modifiedIterations = node.ToolTipText != _settings.Original_Iterations[node];
        }
        var _modifiedSelection = false;
        if (_settings.Original_Selected.ContainsKey(node))
        {
          _modifiedSelection = node.Checked != _settings.Original_Selected[node];
        }
        _modified = _modifiedSelection || _modifiedIterations;
      }

      //Set modification flag
      if (!_settings.Modified && _modified)
      {
        _settings.Modified = !_translateOperation;
      }

      if (node != null && _settings.Modified)
      {
        if (_modified && !_settings.ModifiedNodes.Contains(node))
        {
          _settings.ModifiedNodes.Add(node);
        }

        if (!_modified && _settings.ModifiedNodes.Contains(node))
        {
          _settings.ModifiedNodes.Remove(node);
        }
      }
    }

    private void Tree_Modified_BackgroundColor(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
      {
        if (_settings.Mark_Edited)
        {
          TreeNode node = ((BindingList<TreeNode>)sender)[e.NewIndex];
          Tree_Modified_BackgroundColor(node);
          _modifiedCopy.Add(node);
        }
      }

      if (e.ListChangedType == ListChangedType.ItemDeleted)
      {
        foreach (TreeNode node in _modifiedCopy)
        {
          if (!_settings.ModifiedNodes.Contains(node))
          {
            node.BackColor = Color.Transparent;
            _modifiedCopy.Remove(node);
            break; //only one item was deleted
          }
        }
      }

      //Refresh search
      Search_RefreshSearch();
    }

    private void Tree_Modified_BackgroundColor(TreeNode node)
    {
      if (node != Tree.Nodes[0])
      {
        node.BackColor = Color.Wheat;
      }
    }

    #endregion Background color - modified

    #endregion Tree operations

    #region MDS/PJS

    private string MDS_getNameForItem(XmlNode node)
    {
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        if (node.Attributes["name"].Value.Contains("item"))
        {
          foreach (XmlNode element in node.ChildNodes)
          {
            if (element.Attributes["name"].Value == "name")
            {
              return element.Attributes["value"].Value;
            }
          }
        }
        return node.Attributes["name"].Value;
      }

      //TC 12
      return node.Attributes["name"].Value;
    }

    private bool MDS_getItemHaveScript(XmlNode node)
    {
      if (node == null)
      {
        return false;
      }

      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        if (node.Attributes["name"].Value.Contains("item"))
        {
          foreach (XmlNode element in node.ChildNodes)
          {
            if (element.Attributes["name"].Value == "test moniker")
            {
              return element.Attributes["value"].Value != "";
            }
          }
        }
        return false;
      }
      //tc 12
      if (node.Name == "testItem" && node.Attributes != null && node.Attributes["testMoniker"] != null)
      {
        return node.Attributes["testMoniker"].Value != "";
      }
      return false;
    }

    private int MDS_getIterationsForItem(XmlNode node)
    {
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        if (node.Attributes["name"].Value.Contains("item"))
        {
          foreach (XmlNode element in node.ChildNodes)
          {
            if (element.Attributes["name"].Value == "count")
            {
              return Int32.Parse(element.Attributes["value"].Value);
            }
          }
        }
        return -1;
      }

      //tc 12
      if (node.Name == "testItem")
      {
        return Int32.Parse(node.Attributes["count"].Value);
      }
      return -1;
    }

    private bool MDS_check_NodeName(XmlNode node)
    {
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        var text = node.Attributes["name"].Value;
        return text.Contains("test data") || text.Contains("item") || text.Contains("child list");
      }
      //TC 12
      return node.Name == "testItem" || node.Name == "testItems" || node.Name == "children";
    }

    private bool MDS_check_NodeIsChildList(XmlNode node)
    {
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        var text = node.Attributes["name"].Value;
        return text.Contains("child list");
      }
      //TC 12
      return node.Name == "children";
    }

    private bool MDS_check_ItemSelected(XmlNode node)
    {
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        if (node.Attributes["name"].Value.Contains("item"))
        {
          foreach (XmlNode element in node.ChildNodes)
          {
            if (element.Attributes["name"].Value == "enabled")
            {
              return element.Attributes["value"].Value == "-1";
            }
          }
        }
        return false;
      }

      //TC 12
      if (node.Name == "testItem")
      {
        return node.Attributes["enabled"].Value == "True";
      }
      return false;
    }

    private void MDS_addTreeNode(XmlNode xmlNode, TreeNode treeNode)
    {
      //Update max level
      if (treeNode.Level > _settings.Max_Level)
      {
        _settings.Max_Level = treeNode.Level;
        Collapse_NumericUpDown.Maximum = _settings.Max_Level;
      }

      if (xmlNode.HasChildNodes)
      {
        XmlNodeList xNodeList = xmlNode.ChildNodes;
        for (int x = 0; x <= xNodeList.Count - 1; x++)
        {
          XmlNode xNode = xmlNode.ChildNodes[x];
          if (MDS_check_NodeName(xNode))
          {
            if (MDS_check_NodeIsChildList(xNode))
            {
              if (xNode.HasChildNodes)
              {
                MDS_addTreeNode(xNode, treeNode);
              }
            }
            else
            {
              var _name = MDS_getNameForItem(xNode);
              var _iterations = MDS_getIterationsForItem(xNode).ToString();
              var _display = _settings.Display_Iterations ? String.Format("[{0}] ::: {1}", _iterations, _name) : _name;
              var _imageIndex = MDS_getItemHaveScript(xNode) ? 1 : 2;
              treeNode.Nodes.Add(new TreeNode()); //
              TreeNode tNode = treeNode.LastNode; //last added node
                                                  //Node parameters
              tNode.Text = _display;                        //Displayed text on tree
              tNode.Checked = MDS_check_ItemSelected(xNode);   //Checkbox state
              tNode.Tag = xNode;                            //Linked XML node
              tNode.Name = _name;                           //Test item name
              tNode.ToolTipText = _iterations;              //Test item iterations
              tNode.ImageIndex = _imageIndex;               //Image of item
              tNode.SelectedImageIndex = _imageIndex;       //Image of selected item
              if (!_settings.Original_Iterations.ContainsKey(tNode))
              {
                _settings.Original_Iterations.Add(tNode, tNode.ToolTipText);
              }
              if (!_settings.Original_Selected.ContainsKey(tNode))
              {
                _settings.Original_Selected.Add(tNode, tNode.Checked);
              }
              MDS_addTreeNode(xNode, tNode);
            }
          }
        }
      }
      else
      {
        var _name = MDS_getNameForItem(xmlNode);
        var _iterations = MDS_getIterationsForItem(xmlNode).ToString();
        var _display = _settings.Display_Iterations ? String.Format("[{0}] ::: {1}", _iterations, _name) : _name;
        var _imageIndex = MDS_getItemHaveScript(xmlNode) ? 1 : 2;

        //Node parameters
        treeNode.Text = _display;                          //Displayed text on tree
        treeNode.Checked = MDS_check_ItemSelected(xmlNode);   //Checkbox state
        treeNode.Tag = xmlNode;                            //Linked XML node
        treeNode.Name = _name;                             //Test item name
        treeNode.ToolTipText = _iterations;                //Test item iterations
        treeNode.ImageIndex = _imageIndex;                 //Image of item
        treeNode.SelectedImageIndex = _imageIndex;         //Image of selected item
        if (!_settings.Original_Iterations.ContainsKey(treeNode))
        {
          _settings.Original_Iterations.Add(treeNode, treeNode.ToolTipText);
        }
        if (!_settings.Original_Selected.ContainsKey(treeNode))
        {
          _settings.Original_Selected.Add(treeNode, treeNode.Checked);
        }
      }
    }

    #endregion MDS/PJS

    #region Run

    private void Run_Button_Click(object sender, EventArgs e)
    {
      Run_Project();
    }

    private void Run_Project()
    {
      if (File_Run_MenuItem.Enabled && !string.IsNullOrEmpty(_settings.TestComplete_Path) && File.Exists(_settings.TestComplete_Path))//it is visible when we could run project
      {
        //Save
        Save_ProjectFile();
        //Run
        System.Diagnostics.Process.Start(_settings.TestComplete_Path, "\"" + _settings.File_Path + "\" /r");
      }
    }

    #endregion Run

    #region Variables operations

    #region Load

    private void Variables_Combo_SelectedIndexChanged(object sender, EventArgs e)
    {
      Variables_PrepareData(((KeyValuePair<string, XmlDocument>)Variables_Combo.SelectedItem).Key);
      Variables_PrepareCells();
    }

    private void Variables_PrepareData(string path)
    {
      //Variables
      var _projectPath = path; //project path is key to all dictionaries
      var _localPath = path + ".tcLS";
      var _projectXML = _settings.Files_List[_projectPath];
      XmlDocument _variablesXML = null;
      if (_settings.LocalVariables_FilesList.ContainsKey(_projectPath))
      {
        _variablesXML = _settings.LocalVariables_FilesList[_projectPath];
      }

      //Clear lists
      _settings.PersistentVariables_List = new BindingList<ProjectVariable>();
      _settings.TemporaryVariables_List = new BindingList<ProjectVariable>();

      //#######################################
      //##### Project variables ###############
      //#######################################
      //Get variables node from XML
      XmlNode _node = _projectXML.DocumentElement;
      var _datav3Founded = false;
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        _node = _node.ChildNodes[0]; ////root
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Attributes["name"].Value == "variables")
          {
            _node = child; //variables node
            break;
          }
        }
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Attributes["name"].Value == "datav3")
          {
            _node = child; //datav3 node
            _datav3Founded = true;
            break;
          }
        }
      }
      else
      {
        //TC 12
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Name == "variables")
          {
            _node = child; //variables node
            break;
          }
        }
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Name == "data")
          {
            _node = child.ChildNodes[0]; //data/Items node
            _datav3Founded = true;
            break;
          }
        }
      }

      if (_datav3Founded)
      {
        //Iterate variables nodes
        foreach (XmlNode child in _node.ChildNodes)
        {
          var _variable = new ProjectVariable()
          {
            Local_XML_Node = null,
            Project_XML_Node = child,
            Local = Translation.NoLocalVariable
          };

          if (_settings.TestComplete_Version == TCVersion.TC12)
          {
            //Type
            _variable.Type = Variables_Decode_Type_FromXML(child.Attributes["Type"].Value);
            //IsTemporary
            _variable.IsTemporary = child.Attributes["Local"].Value != "True";
            //Name
            _variable.Name = child.Attributes["Name"].Value;
            //Description
            _variable.Description = child.Attributes["Descr"] != null ? child.Attributes["Descr"].Value : "";
            //Category
            _variable.Category = child.Attributes["Category"] != null ? child.Attributes["Category"].Value : "";
            //Default
            _variable.Default = Variables_Decode_Value_FromXML(child, _variable);

          }
          else
          {
            //TC 11
            //Type
            foreach (XmlNode node in child.ChildNodes)
            {
              if (node.Attributes["name"].Value == "type")
              {
                _variable.Type = Variables_Decode_Type_FromXML(node.Attributes["value"].Value);
                break;
              }
            }
            //IsTemporary
            foreach (XmlNode node in child.ChildNodes)
            {
              if (node.Attributes["name"].Value == "local")
              {
                _variable.IsTemporary = node.Attributes["value"].Value == "0";
                break;
              }
            }
            //Name
            foreach (XmlNode node in child.ChildNodes)
            {
              if (node.Attributes["name"].Value == "name")
              {
                _variable.Name = node.Attributes["value"].Value;
                break;
              }
            }
            //Description
            foreach (XmlNode node in child.ChildNodes)
            {
              if (node.Attributes["name"].Value == "descr")
              {
                _variable.Description = Variables_Decode_Value_FromXML(node.Attributes["value"].Value, "string");
                break;
              }
            }
            //Category
            foreach (XmlNode node in child.ChildNodes)
            {
              if (node.Attributes["name"].Value == "category")
              {
                _variable.Category = Variables_Decode_Value_FromXML(node.Attributes["value"].Value, "string");
                break;
              }
            }
            //Default
            foreach (XmlNode node in child.ChildNodes)
            {
              if (node.Attributes["name"].Value == "defvalue")
              {
                _variable.Default = Variables_Decode_Value_FromXML(node, _variable);
                break;
              }
            }
          }

          if (!_variable.IsTemporary && _settings.PersistentVariables_Types.Contains(_variable.Type)) { _settings.PersistentVariables_List.Add(_variable); }
          else if (_settings.TemporaryVariables_Types.Contains(_variable.Type)) { _settings.TemporaryVariables_List.Add(_variable); _variable.IsTemporary = true; }
        }

        //#######################################
        //##### Local variables #################
        //#######################################
        if (_settings.LocalVariables_FilesList.ContainsKey(_projectPath)) //Local file exists
        {
          _node = _variablesXML.DocumentElement.ChildNodes[0].ChildNodes[0]; ////root/data
          foreach (XmlNode child in _node.ChildNodes)
          {
            if (child.Attributes["name"].Value == "variables")
            {
              _node = child; //variables node
              break;
            }
          }
          _datav3Founded = false;
          foreach (XmlNode child in _node.ChildNodes)
          {
            if (child.Attributes["name"].Value == "datav3")
            {
              _node = child; //datav3 node
              _datav3Founded = true;
              break;
            }
          }

          //Create default local values
          if (!_datav3Founded)
          {
            _node = _variablesXML.DocumentElement.ChildNodes[0].ChildNodes[0]; ////root/data
            foreach (XmlNode child in _node.ChildNodes)
            {
              if (child.Attributes["name"].Value == "variables")
              {
                _node = child;
                break;
              }
            }
            _node.RemoveAll();
            ((XmlElement)_node).SetAttribute("name", "variables");                //variables
                                                                                  //create "datav3"
            _node.AppendChild(_variablesXML.CreateElement("Node"));
            ((XmlElement)_node.LastChild).SetAttribute("name", "datav3");         //datav3
                                                                                  //create version node
            _node.AppendChild(_variablesXML.CreateElement("Prp"));
            ((XmlElement)_node.LastChild).SetAttribute("name", "version");
            ((XmlElement)_node.LastChild).SetAttribute("type", "I");
            ((XmlElement)_node.LastChild).SetAttribute("value", "3");

            _node = _node.ChildNodes[0]; //variables/datav3 node
                                         //Copy Persistent variables (names and values)
            for (var i = 0; i < _settings.PersistentVariables_List.Count; i++)
            {
              ProjectVariable _variable = _settings.PersistentVariables_List[i];
              var _name = _settings.TestComplete_Version == TCVersion.TC11 ? _variable.Project_XML_Node.Attributes["name"].Value : "var" + i.ToString();
              _node.AppendChild(_variablesXML.CreateElement("Node"));
              ((XmlElement)_node.LastChild).SetAttribute("name", _name);            //node varX
              _node.LastChild.AppendChild(_variablesXML.CreateElement("Node"));                                                 //node name value
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("name", "value");
              _node.LastChild.LastChild.AppendChild(_variablesXML.CreateElement("Prp"));                                        //Prp value
              ((XmlElement)_node.LastChild.LastChild.LastChild).SetAttribute("name", Variables_Decode_Type_ForXML(_variable.Type));
              ((XmlElement)_node.LastChild.LastChild.LastChild).SetAttribute("type", Variables_Decode_TypeLetter_ForXML(_variable.Type));
              ((XmlElement)_node.LastChild.LastChild.LastChild).SetAttribute("value", Variables_Decode_Value_ForXML(_variable.Default, _variable.Type));
              _node.LastChild.AppendChild(_variablesXML.CreateElement("Prp"));                                                 //prp name
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("name", "name");
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("type", "S");
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("value", _variable.Name);
            }

            //Copy Temporary variables (names)
            for (var i = _settings.PersistentVariables_List.Count; i < _settings.TemporaryVariables_List.Count + _settings.PersistentVariables_List.Count; i++)
            {
              ProjectVariable _variable = _settings.TemporaryVariables_List[i];
              var _name = _settings.TestComplete_Version == TCVersion.TC11 ? _variable.Project_XML_Node.Attributes["name"].Value : "var" + i.ToString();
              _node.AppendChild(_variablesXML.CreateElement("Node"));
              ((XmlElement)_node.LastChild).SetAttribute("name", _name);            //node varX
              _node.LastChild.AppendChild(_variablesXML.CreateElement("Prp"));                                                 //prp name
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("name", "name");
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("type", "S");
              ((XmlElement)_node.LastChild.LastChild).SetAttribute("value", _variable.Name);
            }
          }

          //Iterate variables nodes
          if (_node != null) //null if no local variables in file - all are the same as default
          {
            foreach (XmlNode child in _node.ChildNodes)
            {
              foreach (ProjectVariable _variable in _settings.PersistentVariables_List)
              {
                if (_settings.TestComplete_Version == TCVersion.TC11)
                {
                  if (child.Attributes["name"].Value == _variable.Project_XML_Node.Attributes["name"].Value)
                  {
                    _variable.Local_XML_Node = child;
                    //Local
                    foreach (XmlNode node in child.ChildNodes)
                    {
                      if (node.Attributes["name"].Value == "value")
                      {
                        _variable.Local = Variables_Decode_Value_FromXML(node, _variable);
                        break;
                      }
                    }
                    break;
                  }
                }
                else
                {
                  //TC 12
                  foreach (XmlElement _prp in child)
                  {
                    if (_prp.Attributes["name"].Value == "name")
                    {
                      if (_prp.Attributes["value"].Value == _variable.Project_XML_Node.Attributes["Name"].Value)
                      {
                        _variable.Local_XML_Node = child;
                        //Local
                        foreach (XmlNode node in child.ChildNodes)
                        {
                          if (node.Attributes["name"].Value == "value")
                          {
                            _variable.Local = Variables_Decode_Value_FromXML(node, _variable);
                            break;
                          }
                        }
                        break;
                      }
                    }
                  }
                }
              }
              foreach (ProjectVariable _variable in _settings.TemporaryVariables_List)
              {
                if (_settings.TestComplete_Version == TCVersion.TC11)
                {
                  if (child.Attributes["name"].Value == _variable.Project_XML_Node.Attributes["name"].Value)
                  {
                    _variable.Local_XML_Node = child;
                    //Local
                    foreach (XmlNode node in child.ChildNodes)
                    {
                      if (node.Attributes["name"].Value == "value")
                      {
                        _variable.Local = Variables_Decode_Value_FromXML(node, _variable);
                        break;
                      }
                    }
                    break;
                  }
                }
                else
                {
                  //TC 12
                  foreach (XmlNode _prp in child.ChildNodes)
                  {
                    if (_prp.Attributes["name"].Value == "name")
                    {
                      if (_prp.Attributes["value"].Value == _variable.Project_XML_Node.Attributes["Name"].Value)
                      {
                        _variable.Local_XML_Node = child;
                        //Local
                        foreach (XmlNode node in child.ChildNodes)
                        {
                          if (node.Attributes["name"].Value == "value")
                          {
                            _variable.Local = Variables_Decode_Value_FromXML(node, _variable);
                            break;
                          }
                        }
                        break;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      Variables_Init_PersistentDataGridView();
      Variables_Init_TemporaryDataGridView();
    }

    private void Variables_PrepareCells()
    {
      foreach (DataGridViewRow _row in PersistentVariables_DGView.Rows)
      {
        ProjectVariable item = (ProjectVariable)_row.DataBoundItem;
        if (item != null)
        {
          Type _currentType;

          switch (item.Type.ToLower())
          {
            case "boolean":
              _currentType = this.PersistentVariables_DGView[2, _row.Index].GetType();
              if (!_currentType.Name.Contains("CheckBox"))
              {
                this.PersistentVariables_DGView[2, _row.Index] = new DataGridViewCheckBoxCell
                {
                  Value = item.Default == "True"
                };
                if (item.Local_XML_Node != null)
                {
                  this.PersistentVariables_DGView[3, _row.Index] = new DataGridViewCheckBoxCell
                  {
                    Value = item.Local == "True"
                  };
                }
              }
              break;

            case "integer":
            case "double":
            case "string":
            case "password":
              _currentType = this.PersistentVariables_DGView[2, _row.Index].GetType();
              if (!_currentType.Name.Contains("TextBox"))
              {
                this.PersistentVariables_DGView[2, _row.Index] = new DataGridViewTextBoxCell
                {
                  Value = item.Default
                };
                if (item.Local_XML_Node != null)
                {
                  this.PersistentVariables_DGView[3, _row.Index] = new DataGridViewTextBoxCell
                  {
                    Value = item.Local
                  };
                }
              }
              break;
          }
        }
      }
      PersistentVariables_DGView.EndEdit();

      foreach (DataGridViewRow _row in TemporaryVariables_DGView.Rows)
      {
        ProjectVariable item = (ProjectVariable)_row.DataBoundItem;
        Type _currentType;
        if (item != null)
        {
          switch (item.Type.ToLower())
          {
            case "boolean":
              _currentType = this.TemporaryVariables_DGView[2, _row.Index].GetType();
              if (!_currentType.Name.Contains("CheckBox"))
              {
                this.TemporaryVariables_DGView[2, _row.Index] = new DataGridViewCheckBoxCell
                {
                  Value = item.Default == "True"
                };
              }
              break;

            case "integer":
            case "double":
            case "string":
            case "object":
            case "db table":
            case "password":
              _currentType = this.TemporaryVariables_DGView[2, _row.Index].GetType();
              if (!_currentType.Name.Contains("TextBox"))
              {
                this.TemporaryVariables_DGView[2, _row.Index] = new DataGridViewTextBoxCell
                {
                  Value = item.Default
                };
              }
              break;

            case "table":
              _currentType = this.TemporaryVariables_DGView[2, _row.Index].GetType();
              if (!_currentType.Name.Contains("Button"))
              {
                this.TemporaryVariables_DGView[2, _row.Index] = new DataGridViewButtonCell
                {
                  Value = item.Default
                };
              }
              break;
          }
        }
      }
      TemporaryVariables_DGView.EndEdit();
    }

    #endregion Load

    #region Init DataGridViews

    private void Variables_Init_PersistentDataGridView()
    {
      PersistentVariables_DGView.Columns.Clear();
      PersistentVariables_DGView.DataBindings.Clear();
      PersistentVariables_DGView.DataMember = null;
      PersistentVariables_DGView.DataSource = null;
      PersistentVariables_DGView.Rows.Clear();
      PersistentVariables_DGView.AllowUserToAddRows = false;
      PersistentVariables_DGView.AllowUserToDeleteRows = false;
      PersistentVariables_DGView.AutoGenerateColumns = false;
      PersistentVariables_DGView.AllowUserToOrderColumns = false;
      PersistentVariables_DGView.AllowUserToResizeRows = false;
      PersistentVariables_DGView.RowHeadersVisible = false;
      PersistentVariables_DGView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      PersistentVariables_DGView.MultiSelect = false;
      PersistentVariables_DGView.DataSource = _settings.PersistentVariables_List;

      DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn
      {
        Name = "Name",
        HeaderText = Translation.VarColumn_Name,
        DataPropertyName = "Name"
      };
      PersistentVariables_DGView.Columns.Add(column1);

      DataGridViewComboBoxColumn column2 = new DataGridViewComboBoxColumn
      {
        Name = "Type",
        HeaderText = Translation.VarColumn_Type,
        DataPropertyName = "Type",
        DataSource = _settings.PersistentVariables_Types
      };
      PersistentVariables_DGView.Columns.Add(column2);

      DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn
      {
        Name = "Default",
        HeaderText = Translation.VarColumn_Default,
        DataPropertyName = "Default"
      };
      PersistentVariables_DGView.Columns.Add(column3);

      DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn
      {
        Name = "Local",
        HeaderText = Translation.VarColumn_Local,
        DataPropertyName = "Local"
      };
      PersistentVariables_DGView.Columns.Add(column4);

      DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn
      {
        Name = "Description",
        HeaderText = Translation.VarColumn_Description,
        DataPropertyName = "Description"
      };
      PersistentVariables_DGView.Columns.Add(column5);

      DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn
      {
        Name = "Category",
        HeaderText = Translation.VarColumn_Category,
        DataPropertyName = "Category"
      };
      PersistentVariables_DGView.Columns.Add(column6);

      PersistentVariables_DGView.Refresh();
    }

    private void Variables_Init_TemporaryDataGridView()
    {
      TemporaryVariables_DGView.Columns.Clear();
      TemporaryVariables_DGView.DataBindings.Clear();
      TemporaryVariables_DGView.DataMember = null;
      TemporaryVariables_DGView.DataSource = null;
      TemporaryVariables_DGView.Rows.Clear();
      TemporaryVariables_DGView.AllowUserToAddRows = false;
      TemporaryVariables_DGView.AllowUserToDeleteRows = false;
      TemporaryVariables_DGView.AutoGenerateColumns = false;
      TemporaryVariables_DGView.AllowUserToOrderColumns = false;
      TemporaryVariables_DGView.AllowUserToResizeRows = false;
      TemporaryVariables_DGView.RowHeadersVisible = false;
      TemporaryVariables_DGView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      TemporaryVariables_DGView.MultiSelect = false;
      TemporaryVariables_DGView.DataSource = _settings.TemporaryVariables_List;

      DataGridViewTextBoxColumn column11 = new DataGridViewTextBoxColumn
      {
        Name = "Name",
        HeaderText = Translation.VarColumn_Name,
        DataPropertyName = "Name"
      };
      TemporaryVariables_DGView.Columns.Add(column11);

      DataGridViewComboBoxColumn column12 = new DataGridViewComboBoxColumn
      {
        Name = "Type",
        HeaderText = Translation.VarColumn_Type,
        DataPropertyName = "Type",
        DataSource = _settings.TemporaryVariables_Types
      };
      TemporaryVariables_DGView.Columns.Add(column12);

      DataGridViewTextBoxColumn column13 = new DataGridViewTextBoxColumn
      {
        Name = "Default",
        HeaderText = Translation.VarColumn_Default,
        DataPropertyName = "Default"
      };
      TemporaryVariables_DGView.Columns.Add(column13);

      DataGridViewTextBoxColumn column14 = new DataGridViewTextBoxColumn
      {
        Name = "Description",
        HeaderText = Translation.VarColumn_Description,
        DataPropertyName = "Description"
      };
      TemporaryVariables_DGView.Columns.Add(column14);

      DataGridViewTextBoxColumn column15 = new DataGridViewTextBoxColumn
      {
        Name = "Category",
        HeaderText = Translation.VarColumn_Category,
        DataPropertyName = "Category"
      };
      TemporaryVariables_DGView.Columns.Add(column15);

      TemporaryVariables_DGView.Refresh();
    }

    #endregion Init DataGridViews

    #region Decoding

    private string Variables_Decode_Type_FromXML(string xmlType)
    {
      string _return = "";
      switch (xmlType)
      {
        case "{D25FDC80-E78F-4209-88B6-6FE2B0771206}":
          _return = "Boolean";
          break;

        case "{88422C25-DDF4-4EA1-B7CC-95779A023F5D}":
          _return = "Integer";
          break;

        case "{8562FD50-0B6E-489C-95A2-9C144116BD78}":
          _return = "Double";
          break;

        case "{123F0C0F-44B4-4BAF-B0E6-F3F89FD873B5}":
          _return = "String";
          break;

        case "{B06407F3-6641-45A9-9692-DDE5B231F2CD}":
          _return = "Password";
          break;

        case "{4AD4881B-176C-4C96-8288-FBBB7E3D1FE3}":
          _return = "Table";
          break;

        case "{8ECBD24A-6DA1-4476-A473-EE3A52F06A83}":
          _return = "DB Table";
          break;

        case "{F38B9AD1-7B22-410F-95FC-6D9420FDE947}":
          _return = "Object";
          break;
      }
      return _return;
    }

    private string Variables_Decode_Type_ForXML(string xmlType, bool TC12 = false)
    {
      string _return = TC12 ? "StrValue" : "strvalue";
      switch (xmlType.ToLower())
      {
        case "boolean":
          _return = TC12 ? "BoolValue" : "boolvalue";
          break;

        case "integer":
          _return = TC12 ? "IntValue" : "intvalue";
          break;

        case "double":
          _return = TC12 ? "FloatValue" : "floatvalue";
          break;

        case "string":
          _return = TC12 ? "StrValue" : "strvalue";
          break;
      }
      return _return;
    }

    private string Variables_Decode_TypeCode_ForXML(string xmlType)
    {
      string _return = "{123F0C0F-44B4-4BAF-B0E6-F3F89FD873B5}";
      switch (xmlType.ToLower())
      {
        case "boolean":
          _return = "{D25FDC80-E78F-4209-88B6-6FE2B0771206}";
          break;

        case "integer":
          _return = "{88422C25-DDF4-4EA1-B7CC-95779A023F5D}";
          break;

        case "double":
          _return = "{8562FD50-0B6E-489C-95A2-9C144116BD78}";
          break;

        case "string":
          _return = "{123F0C0F-44B4-4BAF-B0E6-F3F89FD873B5}";
          break;

        case "table":
          _return = "{4AD4881B-176C-4C96-8288-FBBB7E3D1FE3}";
          break;
      }
      return _return;
    }

    private string Variables_Decode_TypeLetter_ForXML(string xmlType)
    {
      string _return = "H";
      switch (xmlType.ToLower())
      {
        case "boolean":
          _return = "B";
          break;

        case "integer":
          _return = "I";
          break;

        case "double":
          _return = "D";
          break;

        case "string":
          _return = "H"; //hex
          break;
      }
      return _return;
    }

    private string Variables_Decode_Value_FromXML(XmlNode node, ProjectVariable item)
    {
      string _return = "";
      switch (item.Type.ToLower())
      {
        case "table":
          item.Table_Columns = new List<string>();
          item.Table_Rows = new List<List<string>>();
          var _rows = 0;
          foreach (XmlNode child in node.ChildNodes)
          {
            if (_settings.TestComplete_Version == TCVersion.TC11)
            {
              if (child.Attributes["name"].Value == "rowcount")
              {
                Int32.TryParse(child.Attributes["value"].Value, out _rows);
                break;
              }
            }
            else
            {
              //TC 12
              if (child.Attributes["RowCount"] != null)
              {
                Int32.TryParse(child.Attributes["RowCount"].Value, out _rows);
                break;
              }
            }
          }
          var _columns = 0;
          foreach (XmlNode child in node.ChildNodes)
          {
            if (_settings.TestComplete_Version == TCVersion.TC11)
            {
              if (child.Attributes["name"].Value == "columncount")
              {
                Int32.TryParse(child.Attributes["value"].Value, out _columns);
                break;
              }
            }
            else
            {
              //TC 12
              if (child.Attributes["ColumnCount"] != null)
              {
                Int32.TryParse(child.Attributes["ColumnCount"].Value, out _columns);
                break;
              }
            }
          }
          //Add columns with their names
          for (var x = 0; x < _columns; x++)
          {
            var _name = "";
            foreach (XmlNode child in node.ChildNodes)
            {
              if (_settings.TestComplete_Version == TCVersion.TC11)
              {
                if (child.Attributes["name"].Value == "columnname" + x.ToString())
                {
                  _name = Variables_Decode_FromHexString(child.Attributes["value"].Value);
                  break;
                }
              }
              else
              {
                //TC 12
                if (child.Attributes["ColumnName" + x.ToString()] != null)
                {
                  _name = child.Attributes["ColumnName" + x.ToString()].Value;
                  break;
                }
              }
            }
            item.Table_Columns.Add(_name);
          }

          //Create rows
          for (var y = 0; y < _rows; y++)
          {
            item.Table_Rows.Add(new List<string>());
          }

          //Fill rows
          for (var x = 0; x < _columns; x++)
          {
            for (var y = 0; y < _rows; y++)
            {
              var _value = "";
              foreach (XmlNode child in node.ChildNodes)
              {
                if (_settings.TestComplete_Version == TCVersion.TC11)
                {
                  if (child.Attributes["name"].Value == "item" + x.ToString() + "_" + y.ToString())
                  {
                    _value = Variables_Decode_FromHexString(child.Attributes["value"].Value);
                    break;
                  }
                }
                else
                {
                  //TC 12
                  foreach (XmlNode _row in child.ChildNodes)
                  {
                    if (_row.Name == "Row")
                    {
                      if (_row.Attributes["No"].Value == y.ToString())
                      {
                        if (_row.Attributes["Cell" + x.ToString()] != null)
                        {
                          _value = Variables_Decode_FromHexString(_row.Attributes["Cell" + x.ToString()].Value);
                          break;
                        }
                      }
                    }
                  }
                }
              }
              item.Table_Rows[y].Add(_value);
            }
          }

          _return = "{" + _columns.ToString() + " columns x " + _rows.ToString() + " rows}";
          break;

        case "object":
        case "db table":
        case "password":
          _return = "Type (" + item.Type + ") is not supported.";
          break;

        default:
          if (_settings.TestComplete_Version == TCVersion.TC11 || node.Name == "Node")
          {
            _return = Variables_Decode_Value_FromXML(node.ChildNodes[0].Attributes["value"].Value, item.Type);
          }
          else
          {
            var _type = Variables_Decode_Type_ForXML(item.Type.ToLower(), true);
            if (node.ChildNodes[0].Attributes[_type] == null)
            {
              ((XmlElement)node.ChildNodes[0]).SetAttribute(_type, "");
            }
            _return = Variables_Decode_Value_FromXML(node.ChildNodes[0].Attributes[_type].Value, item.Type);
          }
          break;
      }
      return _return;
    }

    private string Variables_Decode_Value_FromXML(string xmlValue, string type)
    {
      string _return = "";
      switch (type.ToLower())
      {
        case "boolean":
        case "boolvalue":
          _return = xmlValue == "-1" ? "True" : "False";
          break;

        case "integer":
        case "double":
        case "intvalue":
        case "floatvalue":
          _return = xmlValue;
          break;

        case "string":
        case "strvalue":
          _return = Variables_Decode_FromHexString(xmlValue);
          break;

        default:
          _return = type;
          break;
      }
      return _return;
    }

    private string Variables_Decode_Value_ForXML(string xmlValue, string xmlType)
    {
      string _return = "";
      switch (xmlType.ToLower())
      {
        case "boolean":
        case "boolvalue":
          _return = xmlValue == "True" ? "-1" : "0";
          break;

        case "integer":
        case "double":
        case "intvalue":
        case "floatvalue":
          _return = xmlValue.Replace(",", ".");
          break;

        case "string":
        case "strvalue":
          _return = Variables_Decode_ToHexString(xmlValue);
          break;
      }
      return _return;
    }

    public static string Variables_Decode_ToHexString(string str)
    {
      if (str == null) return "";
      var sb = new StringBuilder();

      var bytes = Encoding.Unicode.GetBytes(str);
      foreach (var t in bytes)
      {
        sb.Append(t.ToString("X2"));
      }

      return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
    }

    public static string Variables_Decode_FromHexString(string hexString)
    {
      var bytes = new byte[hexString.Length / 2];
      for (var i = 0; i < bytes.Length; i++)
      {
        bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
      }

      return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
    }

    #endregion Decoding

    #region Edit

    private bool Variables_ValidateValue(ProjectVariable item, string newValue)
    {
      var _return = false;
      switch (item.Type.ToLower())
      {
        case "boolean":
          _return = newValue == "True" || newValue == "False";  //OK
          break;

        case "integer":
          int _result = -1;
          _return = Int32.TryParse(newValue, out _result);
          break;

        case "double":
          double _res = -1;
          _return = Double.TryParse(newValue, out _res);
          break;

        case "string":
        case "table":
          _return = true;
          break;
      }
      return _return;
    }

    private void Variables_SetDefaultValue(ProjectVariable item)
    {
      if (item.Type != _valueBeforeEdit)
      {
        Tree_Modified(null);
        switch (item.Type.ToLower())
        {
          case "boolean":
            Variables_ReplaceValue(item, "False");
            break;

          case "integer":
            Variables_ReplaceValue(item, "0");
            break;

          case "double":
            Variables_ReplaceValue(item, "0.0");
            break;

          case "string":
            Variables_ReplaceValue(item, "");
            break;

          case "table":
            Variables_ReplaceTable(item, new DataTable());
            break;

          default:
            item.Default = "Type (" + item.Type + ") is not supported.";
            break;
        }
      }
    }

    private void Variables_ReplaceValue(ProjectVariable item, string newValue)
    {
      Tree_Modified(null);
      //Reset XML node
      var _projectXML = item.Project_XML_Node.OwnerDocument;
      var _node = item.Project_XML_Node;
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        foreach (XmlNode child in item.Project_XML_Node.ChildNodes)
        {
          if (child.Attributes["name"].Value == "defvalue")
          {
            _node = child; //get table value node
            break;
          }
        }
        //Remove child nodes
        _node.RemoveAll();
        ((XmlElement)_node).SetAttribute("name", "defvalue");
        //######### Add new node
        _node.AppendChild(_projectXML.CreateElement("Prp"));
        ((XmlElement)_node.LastChild).SetAttribute("name", Variables_Decode_Type_ForXML(item.Type));
        ((XmlElement)_node.LastChild).SetAttribute("type", Variables_Decode_TypeLetter_ForXML(item.Type));
        ((XmlElement)_node.LastChild).SetAttribute("value", Variables_Decode_Value_ForXML(newValue, item.Type));
      }
      else
      {
        //TC 12
        item.Project_XML_Node.Attributes["Type"].Value = Variables_Decode_TypeCode_ForXML(item.Type);
        foreach (XmlNode child in item.Project_XML_Node.ChildNodes)
        {
          if (child.Name == "DefValue")
          {
            child.Attributes.RemoveAll();
            ((XmlElement)child).SetAttribute(Variables_Decode_Type_ForXML(item.Type.ToLower(), true), Variables_Decode_Value_ForXML(newValue, item.Type));
            break;
          }
        }
      }

      //Change item value
      item.Default = newValue;
      if (item.Local_XML_Node != null)
      {
        item.Local = item.Default;
      }
    }

    private void Variables_ReplaceTable(ProjectVariable item, DataTable table)
    {
      if (table != null)
      {
        Tree_Modified(null);
        //Get columns and rows count
        var _rowCount = table.Rows.Count;
        var _columnCount = table.Columns.Count;
        var _projectXML = item.Project_XML_Node.OwnerDocument;

        //Reset table in item
        item.Table_Columns = new List<string>();
        item.Table_Rows = new List<List<string>>();

        //Fill table in item
        for (var x = 0; x < table.Columns.Count; x++)
        {
          DataColumn col = table.Columns[x];
          item.Table_Columns.Add(col.ColumnName.Replace("[" + x.ToString() + "]", "").Trim());
        }
        for (var x = 0; x < table.Rows.Count; x++)
        {
          var _row = new List<string>();
          foreach (var cell in table.Rows[x].ItemArray)
          {
            string _value = cell.GetType().ToString() == "System.DBNull" ? "" : (string)cell;
            _row.Add(_value);
          }

          item.Table_Rows.Add(_row);
        }

        //Reset XML node
        var _node = item.Project_XML_Node;
        if (_settings.TestComplete_Version == TCVersion.TC11)
        {
          foreach (XmlNode child in item.Project_XML_Node.ChildNodes)
          {
            if (child.Attributes["name"].Value == "defvalue")
            {
              _node = child; //get table value node
              break;
            }
          }
          //Remove child nodes
          _node.RemoveAll();
          ((XmlElement)_node).SetAttribute("name", "defvalue");
          //######### Add new nodes
          //columncount
          _node.AppendChild(_projectXML.CreateElement("Prp"));
          ((XmlElement)_node.LastChild).SetAttribute("name", "columncount");
          ((XmlElement)_node.LastChild).SetAttribute("type", "I");
          ((XmlElement)_node.LastChild).SetAttribute("value", _columnCount.ToString());
          //columnnames
          for (var x = 0; x < item.Table_Columns.Count; x++)
          {
            var col = item.Table_Columns[x];
            if (!string.IsNullOrEmpty(col))
            {
              _node.AppendChild(_projectXML.CreateElement("Prp"));
              ((XmlElement)_node.LastChild).SetAttribute("name", "columnname" + x.ToString());
              ((XmlElement)_node.LastChild).SetAttribute("type", "H");
              ((XmlElement)_node.LastChild).SetAttribute("value", Variables_Decode_ToHexString(col));
            }
          }
          //items
          for (var y = 0; y < item.Table_Columns.Count; y++)
          {
            for (var x = 0; x < item.Table_Rows.Count; x++)
            {
              var cell = item.Table_Rows[x][y]; //rows then columns indexes
              if (!string.IsNullOrEmpty(cell))
              {
                _node.AppendChild(_projectXML.CreateElement("Prp"));
                ((XmlElement)_node.LastChild).SetAttribute("name", "item" + y.ToString() + "_" + x.ToString());
                ((XmlElement)_node.LastChild).SetAttribute("type", "H");
                ((XmlElement)_node.LastChild).SetAttribute("value", Variables_Decode_ToHexString(cell));
              }
            }
          }
          //rowcount
          _node.AppendChild(_projectXML.CreateElement("Prp"));
          ((XmlElement)_node.LastChild).SetAttribute("name", "rowcount");
          ((XmlElement)_node.LastChild).SetAttribute("type", "I");
          ((XmlElement)_node.LastChild).SetAttribute("value", _rowCount.ToString());
        }
        else
        {
          //TC 12
          //Remove child nodes
          foreach (XmlNode child in _node.ChildNodes)
          {
            _node.RemoveChild(child);
          }
          //Create DefValue node
          _node.AppendChild(_projectXML.CreateElement("DefValue"));
          _node = _node.LastChild;
          //Attributes
          ((XmlElement)_node).SetAttribute("RowCount", item.Table_Rows.Count.ToString());
          ((XmlElement)_node).SetAttribute("ColumnCount", item.Table_Columns.Count.ToString());
          //Columns node
          _node.AppendChild(_projectXML.CreateElement("Columns"));
          var _columns = _node.LastChild;
          for (var i = 0; i < item.Table_Columns.Count; i++)
          {
            ((XmlElement)_columns).SetAttribute("ColumnName" + i.ToString(), item.Table_Columns[i]);
          }
          for (var x = 0; x < item.Table_Rows.Count; x++)
          {
            _node.AppendChild(_projectXML.CreateElement("Row"));
            var _row = _node.LastChild;
            ((XmlElement)_row).SetAttribute("No", x.ToString());
            for (var y = 0; y < item.Table_Columns.Count; y++)
            {
              ((XmlElement)_row).SetAttribute("Cell" + y.ToString(), Variables_Decode_ToHexString(item.Table_Rows[x][y]));
            }
          }
        }
        //Change item value
        item.Default = "{" + _columnCount.ToString() + " columns x " + _rowCount.ToString() + " rows}";
      }
    }

    private void Variables_Edited(int ColumnIndex, int RowIndex, DataGridView dgv, bool duringEntering = false)
    {
      //Don't update cell on remove operation - last index error fix
      if (!_removeOperation && RowIndex > -1 && ColumnIndex > -1)
      {
        //Local should be locked now anyway
        var _row = dgv.Rows[RowIndex];
        var _cell = _row.Cells[ColumnIndex];
        ProjectVariable item = (ProjectVariable)_row.DataBoundItem;

        //Change item values depending on edited column
        dgv.InvalidateCell(_cell);//repaint cell - refreshing changing values
        switch (ColumnIndex)
        {
          case 0:

            #region Name

            bool validA = _cell.Value == null ? false : ((string)_cell.Value).All(c => Char.IsLetterOrDigit(c) || c.Equals('_'));
            validA = _cell.Value == null ? false : Char.IsLetter(((string)_cell.Value)[0]) || ((string)_cell.Value)[0].Equals('_');
            if (duringEntering) validA = true;
            if (!validA)
            {
              MessageBox.Show(string.Format(Translation.VarNameInvalid, (string)_cell.Value));
              if (_settings.TestComplete_Version == TCVersion.TC11)
              {
                foreach (XmlNode node in item.Project_XML_Node.ChildNodes)
                {
                  if (node.Attributes["name"].Value == "name")
                  {
                    node.Attributes["value"].Value = _valueBeforeEdit;
                    item.Name = node.Attributes["value"].Value;
                    _cell.Value = node.Attributes["value"].Value;
                    dgv.EndEdit();
                    dgv.ClearSelection();
                    break;
                  }
                }
              }
              else
              {
                //TC 12
                item.Project_XML_Node.Attributes["Name"].Value = _valueBeforeEdit;
                item.Name = item.Project_XML_Node.Attributes["Name"].Value;
                _cell.Value = item.Project_XML_Node.Attributes["Name"].Value;
                dgv.EndEdit();
                dgv.ClearSelection();
              }
              return;
            }
            //Project file
            if (_settings.TestComplete_Version == TCVersion.TC11)
            {
              foreach (XmlNode node in item.Project_XML_Node.ChildNodes)
              {
                if (node.Attributes["name"].Value == "name")
                {
                  node.Attributes["value"].Value = (string)_cell.Value;
                  break;
                }
              }
            }
            else
            {
              //TC 12
              item.Project_XML_Node.Attributes["Name"].Value = (string)_cell.Value;
            }
            //Local file
            if (item.Local_XML_Node != null)
            {
              foreach (XmlNode node in item.Local_XML_Node.ChildNodes)
              {
                if (node.Attributes["name"].Value == "name")
                {
                  node.Attributes["value"].Value = (string)_cell.Value;
                  break;
                }
              }
            }
            break;

          #endregion Name

          case 1:

            #region Type

            //Project file
            if (_settings.TestComplete_Version == TCVersion.TC11)
            {
              item.Project_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["name"].Value = Variables_Decode_Type_ForXML((string)_cell.Value);
              item.Project_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["type"].Value = Variables_Decode_TypeLetter_ForXML((string)_cell.Value);
              foreach (XmlNode node in item.Project_XML_Node.ChildNodes)
              {
                if (node.Attributes["name"].Value == "type")
                {
                  node.Attributes["value"].Value = Variables_Decode_TypeCode_ForXML((string)_cell.Value);
                  break;
                }
              }
            }
            else
            {
              //TC 12
              item.Project_XML_Node.Attributes["Type"].Value = Variables_Decode_TypeCode_ForXML((string)_cell.Value);
            }

            //Local file
            if (item.Local_XML_Node != null && !item.IsTemporary)
            {
              item.Local_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["name"].Value = Variables_Decode_Type_ForXML((string)_cell.Value);
              item.Local_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["type"].Value = Variables_Decode_TypeLetter_ForXML((string)_cell.Value);
              foreach (XmlNode node in item.Local_XML_Node.ChildNodes)
              {
                if (node.Attributes["name"].Value == "type")
                {
                  node.Attributes["value"].Value = Variables_Decode_TypeCode_ForXML((string)_cell.Value);
                  break;
                }
              }
            }

            //Variables change to default values
            Variables_SetDefaultValue(item);
            Variables_Edited(2, _row.Index, dgv);
            Variables_Edited(3, _row.Index, dgv);
            Variables_PrepareCells();

            break;

          #endregion Type

          case 2:

            #region Default

            if (!duringEntering)
            {
              if (Variables_ValidateValue(item, (string)_cell.Value))
              {
                if (_settings.TestComplete_Version == TCVersion.TC11)
                {
                  item.Project_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["value"].Value = Variables_Decode_Value_ForXML((string)_cell.Value, item.Project_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["name"].Value);
                }
                else
                {
                  //TC 12
                  item.Project_XML_Node.ChildNodes[0].Attributes.RemoveAll();
                  ((XmlElement)item.Project_XML_Node.ChildNodes[0]).SetAttribute(Variables_Decode_Type_ForXML(item.Type.ToLower(), true), Variables_Decode_Value_ForXML((string)_cell.Value, Variables_Decode_Type_FromXML(item.Project_XML_Node.Attributes["Type"].Value)));
                }
              }
              else
              {
                MessageBox.Show("The variable value \"" + (string)_cell.Value + "\" is invalid. Value must be valid for its type.");
                if (_settings.TestComplete_Version == TCVersion.TC11)
                {
                  item.Default = Variables_Decode_Value_FromXML(item.Project_XML_Node.ChildNodes[0], item);
                }
                else
                {
                  //TC 12
                  item.Default = Variables_Decode_Value_FromXML(item.Project_XML_Node, item);
                }
                _cell.Value = item.Default;
                dgv.EndEdit();
                dgv.ClearSelection();
              }
            }
            break;

          #endregion Default

          case 3:

            #region Local/Description

            if (item.IsTemporary) //Description
            {
              if (_settings.TestComplete_Version == TCVersion.TC11)
              {
                foreach (XmlNode node in item.Project_XML_Node.ChildNodes)
                {
                  if (node.Attributes["name"].Value == "descr")
                  {
                    node.Attributes["value"].Value = Variables_Decode_Value_ForXML((string)_cell.Value, "string");
                    break;
                  }
                }
              }
              else
              {
                //TC 12
                ((XmlElement)item.Project_XML_Node).SetAttribute("Descr", (string)_cell.Value);
              }
            }
            else //Local
            {
              if (!duringEntering)
              {
                if (item.Local_XML_Node != null)
                {
                  if (Variables_ValidateValue(item, (string)_cell.Value))
                  {
                    item.Local_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["value"].Value = Variables_Decode_Value_ForXML((string)_cell.Value, item.Local_XML_Node.ChildNodes[0].ChildNodes[0].Attributes["name"].Value);
                  }
                  else
                  {
                    MessageBox.Show("The variable value \"" + (string)_cell.Value + "\" is invalid. Value must be valid for its type.");
                    item.Local = Variables_Decode_Value_FromXML(item.Local_XML_Node.ChildNodes[0], item);
                    _cell.Value = item.Local;
                    dgv.EndEdit();
                    dgv.ClearSelection();
                  }
                }
              }
            }
            break;

          #endregion Local/Description

          case 4:

            #region Description/Category

            if (_settings.TestComplete_Version == TCVersion.TC11)
            {
              foreach (XmlNode node in item.Project_XML_Node.ChildNodes)
              {
                var _check = item.IsTemporary ? "category" : "descr";
                if (node.Attributes["name"].Value == _check) //Description //Category
                {
                  node.Attributes["value"].Value = Variables_Decode_Value_ForXML((string)_cell.Value, "string");
                  break;
                }
              }
            }
            else
            {
              //TC 12
              var _check = item.IsTemporary ? "Category" : "Descr";
              ((XmlElement)item.Project_XML_Node).SetAttribute(_check, (string)_cell.Value);
            }

            break;

          #endregion Description/Category

          case 5:

            #region Category

            if (_settings.TestComplete_Version == TCVersion.TC11)
            {
              foreach (XmlNode node in item.Project_XML_Node.ChildNodes)
              {
                if (node.Attributes["name"].Value == "category")
                {
                  node.Attributes["value"].Value = Variables_Decode_Value_ForXML((string)_cell.Value, "string");
                  break;
                }
              }
            }
            else
            {
              //TC 12
              ((XmlElement)item.Project_XML_Node).SetAttribute("Category", (string)_cell.Value);
            }
            break;

            #endregion Category
        }
      }
    }

    #endregion Edit

    #region Lock cells

    private void Variables_LockLocal(int ColumnIndex, int RowIndex, DataGridView dgv)
    {
      var _row = dgv.Rows[RowIndex];
      var _cell = _row.Cells[ColumnIndex];
      ProjectVariable item = (ProjectVariable)_row.DataBoundItem;

      //Check if local variables are loaded
      if (!item.IsTemporary && item.Local_XML_Node == null && ColumnIndex == 3)
      {
        _cell.ReadOnly = true;
      }
    }

    private void Variables_LockNotSupported(int ColumnIndex, int RowIndex, DataGridView dgv)
    {
      var _row = dgv.Rows[RowIndex];
      var _cell = _row.Cells[ColumnIndex];
      ProjectVariable item = (ProjectVariable)_row.DataBoundItem;

      switch (item.Type.ToLower())
      {
        case "password":
        case "object":
        case "db table":
          //Lock change of type, default or local cells
          if (ColumnIndex == 1 || ColumnIndex == 2 || (!item.IsTemporary && ColumnIndex == 3))
          {
            _cell.ReadOnly = true;
          }
          break;
      }
    }

    #endregion Lock cells

    #region Add

    private void Variables_Add(DataGridView dgv)
    {
      var _projectVariable = new ProjectVariable();

      #region Name prepare

      var _nameIndex = 1;
      foreach (var _item in _settings.TemporaryVariables_List)
      {
        var _value = 0;
        if (Int32.TryParse(_item.Name.Replace("Var", ""), out _value))
        {
          if (_value >= _nameIndex)
          {
            _nameIndex = _value + 1;
          }
        }
      }
      foreach (var _item in _settings.PersistentVariables_List)
      {
        var _value = 0;
        if (Int32.TryParse(_item.Name.Replace("Var", ""), out _value))
        {
          if (_value >= _nameIndex)
          {
            _nameIndex = _value + 1;
          }
        }
      }
      var _name = "Var" + _nameIndex.ToString();

      //Prepare name
      for (var x = 1; x <= _nameIndex; x++)
      {
        var _exists = false;
        foreach (var _item in _settings.TemporaryVariables_List)
        {
          if (_item.Name == "Var" + x.ToString())
          {
            _exists = true;
            break;
          }
        }
        if (!_exists)
        {
          foreach (var _item in _settings.PersistentVariables_List)
          {
            if (_item.Name == "Var" + x.ToString())
            {
              _exists = true;
              break;
            }
          }
        }
        if (!_exists)
        {
          _name = "Var" + x.ToString();
          break;
        }
      }

      #endregion Name prepare

      _projectVariable.Name = _name;
      _projectVariable.Category = "";
      _projectVariable.Default = "";
      _projectVariable.Description = "";
      _projectVariable.Type = "String";
      _projectVariable.Local = Translation.NoLocalVariable;
      _projectVariable.IsTemporary = dgv == TemporaryVariables_DGView;

      #region XML name prepare

      var _xmlName = "var0";
      _nameIndex = 0;
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        foreach (var _item in _settings.TemporaryVariables_List)
        {
          var _value = 0;

          if (Int32.TryParse(_item.Project_XML_Node.Attributes["name"].Value.Replace("var", ""), out _value))
          {
            if (_value >= _nameIndex)
            {
              _nameIndex = _value + 1;
            }
          }
        }
        foreach (var _item in _settings.PersistentVariables_List)
        {
          var _value = 0;
          if (Int32.TryParse(_item.Project_XML_Node.Attributes["name"].Value.Replace("var", ""), out _value))
          {
            if (_value >= _nameIndex)
            {
              _nameIndex = _value + 1;
            }
          }
        }
        _xmlName = "var" + _nameIndex.ToString();
      }
      else
      {
        foreach (var _item in _settings.TemporaryVariables_List)
        {
          if (_item.Local_XML_Node != null)
          {
            var _value = 0;
            if (Int32.TryParse(_item.Local_XML_Node.Attributes["name"].Value.Replace("var", ""), out _value))
            {
              if (_value >= _nameIndex)
              {
                _nameIndex = _value + 1;
              }
            }
          }
        }
        foreach (var _item in _settings.PersistentVariables_List)
        {
          if (_item.Local_XML_Node != null)
          {
            var _value = 0;
            if (Int32.TryParse(_item.Local_XML_Node.Attributes["name"].Value.Replace("var", ""), out _value))
            {
              if (_value >= _nameIndex)
              {
                _nameIndex = _value + 1;
              }
            }
          }
        }
        _xmlName = "var" + _nameIndex.ToString();
      }

      #endregion XML name prepare

      #region Create XML node

      //Get variables node from XML
      var _projectPath = ((KeyValuePair<string, XmlDocument>)Variables_Combo.SelectedItem).Key;
      var _projectXML = _settings.Files_List[_projectPath];
      XmlNode _node = _projectXML.DocumentElement;
      if (_settings.TestComplete_Version == TCVersion.TC11)
      {
        _node = _node.ChildNodes[0]; ////root
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Attributes["name"].Value == "variables")
          {
            _node = child; //variables node
            break;
          }
        }
        var _createDataV3 = true;
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Attributes["name"].Value == "datav3")
          {
            _node = child; //datav3 node
            _createDataV3 = false;
            break;
          }
        }
        if (_createDataV3) //first variable
        {
          var _variables = _node;
          //Add datav3
          _variables.AppendChild(_projectXML.CreateElement("Node"));
          ((XmlElement)_variables.LastChild).SetAttribute("name", "datav3");
          _node = _variables.LastChild;
          //Add prp
          _variables.AppendChild(_projectXML.CreateElement("Prp"));
          ((XmlElement)_variables.LastChild).SetAttribute("name", "version");
          ((XmlElement)_variables.LastChild).SetAttribute("type", "I");
          ((XmlElement)_variables.LastChild).SetAttribute("value", "3");
        }

        //######### Add new node
        _node.AppendChild(_projectXML.CreateElement("Node"));
        ((XmlElement)_node.LastChild).SetAttribute("name", _xmlName);
        _projectVariable.Project_XML_Node = (XmlElement)_node.LastChild;
        _projectVariable.Project_XML_Node.AppendChild(_projectXML.CreateElement("Node"));
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("name", "defvalue");
        Variables_ReplaceValue(_projectVariable, "");
        //category
        _projectVariable.Project_XML_Node.AppendChild(_projectXML.CreateElement("Prp"));
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("name", "category");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("type", "H");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("value", "");
        //description
        _projectVariable.Project_XML_Node.AppendChild(_projectXML.CreateElement("Prp"));
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("name", "descr");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("type", "H");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("value", "");
        //local
        _projectVariable.Project_XML_Node.AppendChild(_projectXML.CreateElement("Prp"));
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("name", "local");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("type", "B");
        var _val = _projectVariable.IsTemporary ? "0" : "-1";
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("value", _val);
        //name
        _projectVariable.Project_XML_Node.AppendChild(_projectXML.CreateElement("Prp"));
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("name", "name");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("type", "S");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("value", _projectVariable.Name);
        //type
        _projectVariable.Project_XML_Node.AppendChild(_projectXML.CreateElement("Prp"));
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("name", "type");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("type", "S");
        ((XmlElement)_projectVariable.Project_XML_Node.LastChild).SetAttribute("value", "{123F0C0F-44B4-4BAF-B0E6-F3F89FD873B5}");
      }
      else
      {
        //TC 12
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Name == "variables")
          {
            _node = child; //variables node
            break;
          }
        }
        var _createDataV3 = true;
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Name == "data")
          {
            _node = child; //datav3 node
            _createDataV3 = false;
            break;
          }
        }
        if (_createDataV3) //first variable
        {
          var _variables = _node;
          //Add data node
          _variables.AppendChild(_projectXML.CreateElement("data"));
          ((XmlElement)_variables.LastChild).SetAttribute("Version", "3");
          _node = _variables.LastChild;
          //Add Items
          _variables.AppendChild(_projectXML.CreateElement("Items"));
        }

        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Name == "Items")
          {
            _node = child; //Items node
            break;
          }
        }

        //######### Add new node
        _node.AppendChild(_projectXML.CreateElement("Variable"));
        var _newNode = (XmlElement)_node.LastChild;
        _projectVariable.Project_XML_Node = _newNode;
        _newNode.SetAttribute("Name", _projectVariable.Name); //Name
        _newNode.SetAttribute("Type", "{123F0C0F-44B4-4BAF-B0E6-F3F89FD873B5}"); //Type
        _newNode.SetAttribute("Local", _projectVariable.IsTemporary ? "False" : "True"); //Local
        _newNode.SetAttribute("Descr", ""); //Description
        _newNode.SetAttribute("Category", ""); //Category
        _newNode.AppendChild(_projectXML.CreateElement("DefValue"));
        _newNode = (XmlElement)_newNode.LastChild;
        _newNode.SetAttribute("StrValue", "");
        Variables_ReplaceValue(_projectVariable, "");
      }

      #endregion Create XML node

      #region Create local XML node

      XmlDocument _variablesXML = null;
      if (_settings.LocalVariables_FilesList.ContainsKey(_projectPath))
      {
        _variablesXML = _settings.LocalVariables_FilesList[_projectPath];
        _node = _variablesXML.DocumentElement.ChildNodes[0].ChildNodes[0]; ////root/data
        foreach (XmlNode child in _node.ChildNodes)
        {
          if (child.Attributes["name"].Value == "variables")
          {
            _node = child.ChildNodes[0]; //variables/datav3 node
            break;
          }
        }

        //######### Add new node
        _node.AppendChild(_variablesXML.CreateElement("Node"));
        ((XmlElement)_node.LastChild).SetAttribute("name", _xmlName);
        _projectVariable.Local_XML_Node = (XmlElement)_node.LastChild;
        _projectVariable.Local = "";
        if (!_projectVariable.IsTemporary)
        {
          _projectVariable.Local_XML_Node.AppendChild(_variablesXML.CreateElement("Node"));
          ((XmlElement)_projectVariable.Local_XML_Node.LastChild).SetAttribute("name", "value");
          ((XmlElement)_projectVariable.Local_XML_Node.LastChild).AppendChild(_variablesXML.CreateElement("Prp"));
          ((XmlElement)_projectVariable.Local_XML_Node.LastChild.LastChild).SetAttribute("name", "strvalue");
          ((XmlElement)_projectVariable.Local_XML_Node.LastChild.LastChild).SetAttribute("type", "H");
          ((XmlElement)_projectVariable.Local_XML_Node.LastChild.LastChild).SetAttribute("value", "");
        }
        _projectVariable.Local_XML_Node.AppendChild(_variablesXML.CreateElement("Prp"));
        ((XmlElement)_projectVariable.Local_XML_Node.LastChild).SetAttribute("name", "name");
        ((XmlElement)_projectVariable.Local_XML_Node.LastChild).SetAttribute("type", "S");
        ((XmlElement)_projectVariable.Local_XML_Node.LastChild).SetAttribute("value", _projectVariable.Name);
      }

      #endregion Create local XML node

      #region Add item and refresh bindings

      if (dgv == TemporaryVariables_DGView)
      {
        _settings.TemporaryVariables_List.Add(_projectVariable);
        Variables_Init_TemporaryDataGridView();
      }
      else
      {
        _settings.PersistentVariables_List.Add(_projectVariable);
        Variables_Init_PersistentDataGridView();
      }

      Variables_PrepareCells();

      #endregion Add item and refresh bindings
    }

    #endregion Add

    #region Delete

    private void Variables_Delete(DataGridView dgv)
    {
      var _row = dgv.CurrentRow;
      var _item = (ProjectVariable)_row.DataBoundItem;
      if (MessageBox.Show("Do you want to delete the variable \"" + _item.Name + "\"?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        Tree_Modified(null);
        //Remember some values
        _removeOperation = true;
        var _nameXML = "";
        var _idXML = 0;
        if (_settings.TestComplete_Version == TCVersion.TC11)
        {
          _nameXML = _item.Project_XML_Node.Attributes["name"].Value;
          Int32.TryParse(_nameXML.Replace("var", ""), out _idXML);
        }

        //Remove nodes from XML
        var _node = _item.Project_XML_Node.ParentNode;
        _node.RemoveChild(_item.Project_XML_Node);
        if (_item.Local_XML_Node != null)
        {
          _nameXML = _settings.TestComplete_Version == TCVersion.TC11 ? _nameXML : _item.Local_XML_Node.Attributes["name"].Value;
          Int32.TryParse(_nameXML.Replace("var", ""), out _idXML);
          _node = _item.Local_XML_Node.ParentNode;
          _node.RemoveChild(_item.Local_XML_Node);
        }

        //Remove item from list
        _settings.TemporaryVariables_List.Remove(_item);
        _settings.PersistentVariables_List.Remove(_item);

        //Rename all XML nodes
        foreach (var _it in _settings.TemporaryVariables_List)
        {
          if (_settings.TestComplete_Version == TCVersion.TC11)
          {
            var _val = 0;
            Int32.TryParse(_it.Project_XML_Node.Attributes["name"].Value.Replace("var", ""), out _val);
            if (_val > _idXML)
            {
              _it.Project_XML_Node.Attributes["name"].Value = "var" + (_val - 1).ToString();
              if (_it.Local_XML_Node != null)
              {
                _it.Local_XML_Node.Attributes["name"].Value = "var" + (_val - 1).ToString();
              }
            }
          }
          else
          {
            //TC 12
            if (_it.Local_XML_Node != null)
            {
              var _val = 0;
              Int32.TryParse(_it.Local_XML_Node.Attributes["name"].Value.Replace("var", ""), out _val);
              if (_val > _idXML)
              {
                _it.Local_XML_Node.Attributes["name"].Value = "var" + (_val - 1).ToString();
              }
            }
          }
        }
        foreach (var _it in _settings.PersistentVariables_List)
        {
          if (_settings.TestComplete_Version == TCVersion.TC11)
          {
            var _val = 0;
            Int32.TryParse(_it.Project_XML_Node.Attributes["name"].Value.Replace("var", ""), out _val);
            if (_val > _idXML)
            {
              _it.Project_XML_Node.Attributes["name"].Value = "var" + (_val - 1).ToString();
              if (_it.Local_XML_Node != null)
              {
                _it.Local_XML_Node.Attributes["name"].Value = "var" + (_val - 1).ToString();
              }
            }
          }

          else
          {
            //TC 12
            if (_it.Local_XML_Node != null)
            {
              var _val = 0;
              Int32.TryParse(_it.Local_XML_Node.Attributes["name"].Value.Replace("var", ""), out _val);
              if (_val > _idXML)
              {
                _it.Local_XML_Node.Attributes["name"].Value = "var" + (_val - 1).ToString();
              }
            }
          }
        }

        //refresh bindings
        var _temp = dgv.DataSource;
        dgv.DataSource = null;
        dgv.DataSource = _temp;
        _removeOperation = false;
      }
    }

    #endregion Delete

    #region Events

    private void Variables_ShowMenu(object sender, MouseEventArgs e, DataGridView dgv)
    {
      if (e.Button == MouseButtons.Right)
      {
        int _rowIndex = dgv.HitTest(e.X, e.Y).RowIndex;
        int _columnIndex = dgv.HitTest(e.X, e.Y).ColumnIndex;

        if (_rowIndex >= 0 && _columnIndex >= 0)
        {
          dgv.ClearSelection();
          dgv.Rows[_rowIndex].Selected = true;
          dgv.CurrentCell = dgv.Rows[_rowIndex].Cells[_columnIndex];
        }

        dgv.ContextMenuStrip.Items[1].Enabled = dgv.SelectedCells.Count > 0;
        dgv.ContextMenuStrip.Show(dgv, e.Location);
      }
    }

    private void Variables_OnCellChanged(DataGridView dgv)
    {
      if (dgv.CurrentCell != null)
      {
        Variables_LockLocal(dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.RowIndex, dgv);
        Variables_LockNotSupported(dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.RowIndex, dgv);
        _valueBeforeEdit = dgv.CurrentCell.Value.ToString();

        //Select row and start edit a cell
        dgv.CurrentRow.Selected = true;
        dgv.BeginEdit(false);
      }
    }

    private void Variables_Cell_OnClick(DataGridView dgv, int RowIndex, int ColumnIndex)
    {
      if (RowIndex >= 0 && ColumnIndex >= 0) //eliminate clicking on the index list or column names
      {
        var _row = dgv.Rows[RowIndex];
        _row.Selected = true;
        var _cell = _row.Cells[ColumnIndex];
        ProjectVariable item = (ProjectVariable)_row.DataBoundItem;
        _valueBeforeEdit = _cell.Value.ToString();
        if (ColumnIndex == 2) //click on default column cell
        {
          if (item.Type == "Table") //clik on default button with table type
          {
            var _edit = new Edit
            {
              _item = item,
              _editTable = true
            };

            if (_edit.ShowDialog(this) == DialogResult.OK)
            {
              Variables_ReplaceTable(item, _edit.table);
            }
            _edit.Dispose();
          }
        }
      }
    }

    private void Variables_TypeCell_SelectionChanged(object sender, EventArgs e)
    {
      var sendingCB = sender as DataGridViewComboBoxEditingControl;
      if (sendingCB.SelectedValue != null)
      {
        switch (sendingCB.SelectedValue.ToString().ToLower())
        {
          case "password":
          case "db table":
          case "object":
            var _value = sendingCB.SelectedValue;
            TemporaryVariables_DGView.CancelEdit();
            PersistentVariables_DGView.CancelEdit();
            TemporaryVariables_DGView.Focus();
            PersistentVariables_DGView.Focus();
            MessageBox.Show("Type (" + _value + ") is not supported.");
            break;
        }
      }
    }

    private void TemporaryVariables_DGView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      Variables_Cell_OnClick(TemporaryVariables_DGView, e.RowIndex, e.ColumnIndex);
    }

    private void PersistentVariables_DGView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      Variables_Cell_OnClick(PersistentVariables_DGView, e.RowIndex, e.ColumnIndex);
    }

    private void TemporaryVariables_DGView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Variables_Edited(e.ColumnIndex, e.RowIndex, TemporaryVariables_DGView);
    }

    private void PersistentVariables_DGView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Variables_Edited(e.ColumnIndex, e.RowIndex, PersistentVariables_DGView);
    }

    private void PersistentVariables_DGView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      PersistentVariables_DGView.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void TemporaryVariables_DGView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      TemporaryVariables_DGView.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void PersistentVariables_DGView_CurrentCellChanged(object sender, EventArgs e)
    {
      Variables_OnCellChanged(PersistentVariables_DGView);
    }

    private void TemporaryVariables_DGView_CurrentCellChanged(object sender, EventArgs e)
    {
      Variables_OnCellChanged(TemporaryVariables_DGView);
    }

    private void PersistentVariables_DGView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      Variables_PrepareCells();
      if (PersistentVariables_DGView.Rows.Count > 1)
      {
        PersistentVariables_DGView.CurrentCell = PersistentVariables_DGView.Rows[0].Cells[0];
      }
    }

    private void TemporaryVariables_DGView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      Variables_PrepareCells();
      if (TemporaryVariables_DGView.Rows.Count > 1)
      {
        TemporaryVariables_DGView.CurrentCell = TemporaryVariables_DGView.Rows[0].Cells[0];
      }
    }

    private void PersistentVariables_DGView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      Variables_Edited(e.ColumnIndex, e.RowIndex, PersistentVariables_DGView, e.ColumnIndex != 1);
      Tree_Modified(null);
    }

    private void TemporaryVariables_DGView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      Variables_Edited(e.ColumnIndex, e.RowIndex, TemporaryVariables_DGView, e.ColumnIndex != 1);
      Tree_Modified(null);
    }

    private void TemporaryVariables_DGView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      if (TemporaryVariables_DGView.CurrentCell.ColumnIndex == 1 && e.Control is ComboBox)
      {
        (e.Control as ComboBox).SelectedIndexChanged += Variables_TypeCell_SelectionChanged;
      }
    }

    private void PersistentVariables_DGView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      if (PersistentVariables_DGView.CurrentCell.ColumnIndex == 1 && e.Control is ComboBox)
      {
        (e.Control as ComboBox).SelectedIndexChanged += Variables_TypeCell_SelectionChanged;
      }
    }

    private void Temporary_ContextMenu_Add_Clicked(object sender, EventArgs e)
    {
      Variables_Add(TemporaryVariables_DGView);
    }

    private void Temporary_ContextMenu_Delete_Clicked(object sender, EventArgs e)
    {
      Variables_Delete(TemporaryVariables_DGView);
    }

    private void Persistent_ContextMenu_Add_Clicked(object sender, EventArgs e)
    {
      Variables_Add(PersistentVariables_DGView);
    }

    private void Persistent_ContextMenu_Delete_Clicked(object sender, EventArgs e)
    {
      Variables_Delete(PersistentVariables_DGView);
    }

    private void TemporaryVariables_DGView_MouseUp(object sender, MouseEventArgs e)
    {
      Variables_ShowMenu(sender, e, TemporaryVariables_DGView);
    }

    private void PersistentVariables_DGView_MouseUp(object sender, MouseEventArgs e)
    {
      Variables_ShowMenu(sender, e, PersistentVariables_DGView);
    }

    private void TemporaryVariables_DGView_Leave(object sender, EventArgs e)
    {
      TemporaryVariables_DGView.ClearSelection();
    }

    private void PersistentVariables_DGView_Leave(object sender, EventArgs e)
    {
      PersistentVariables_DGView.ClearSelection();
    }

    #endregion Events

    #endregion Variables operations

    #region Help    

    private void aboutTestsSelectorToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show(string.Format(Translation.AboutText, Settings.Version_Assembly, Settings.Copyright));
    }

    private void CommandLine_MenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show(Translation.CommandLineHelp);
    }

    #endregion

    #region Drag & Drop

    private void TS_Main_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
      if (files.Length != 1) return;
      var file = files[0];
      var ext = Path.GetExtension(file);
      if (ext.Equals(".pjs", StringComparison.CurrentCultureIgnoreCase) ||
          ext.Equals(".mds", StringComparison.CurrentCultureIgnoreCase) ||
          (ext.Equals(".txt", StringComparison.CurrentCultureIgnoreCase) && Tabs_Control.Visible))
      {
        e.Effect = DragDropEffects.Copy;
        return;
      }
    }

    private void TS_Main_DragDrop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
      if (files.Length != 1) return;
      var file = files[0];
      var ext = Path.GetExtension(file);
      if (ext.ToLower() == ".txt")
      {
        Select_ItemsFromFile(file);
      }
      else
      {
        Load_Operation(false, file);
      }
    }

    #endregion

  }
}