using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace TestsSelector.Properties
{
    internal sealed partial class Settings
    {
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e) { }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e) { }
    }
}

namespace TestsSelector
{
    public enum Modes { PJS, MDS }

    public enum TCVersion { TC11, TC12 }

    internal class Settings
    {
        public const string Version_Assembly = "3.0.2.6";
        public const string Copyright = "Copyright © Mariusz Kowalski 2018";

        public Settings()
        {
            File_Path = "";
            TestComplete_Version = TCVersion.TC11;
            XML_Document = null;
            PJS_Projects = new Dictionary<string, XmlDocument>();
            Files_List = new Dictionary<string, XmlDocument>();
            MDS_Loaded = false;
            Modified = false;
            ModifiedNodes = new BindingList<TreeNode>();
            Load_Complete = false;
            LocalVariables_FilesList = new Dictionary<string, XmlDocument>();
            PersistentVariables_List = new BindingList<ProjectVariable>();
            TemporaryVariables_List = new BindingList<ProjectVariable>();
            Original_Selected = new Dictionary<TreeNode, bool>();
            Original_Iterations = new Dictionary<TreeNode, string>();
            Original_Value = new Dictionary<TreeNode, string>();
            Original_CheckLevel = new Dictionary<TreeNode, string>();
            Mode = Modes.MDS;
            Children_Selection = Properties.Settings.Default.Children;
            Parent_Selection = Properties.Settings.Default.Parents;
            Display_Iterations = Properties.Settings.Default.Iterations;
            Mark_Edited = Properties.Settings.Default.MarkEdited;
            TestComplete_Path = Properties.Settings.Default.TC_Path;
            Max_Level = 0;
            ScriptsForRun = 0;
            Search_CurrentNodeMatches = new List<TreeNode>();
            Search_LastNodeIndex = 0;
            Search_LastSearchText = "";
        }

        public string Version { get { return Version_Assembly; } }
        public string[] Arguments { get; set; }
        public Font XML_Font { get { return new Font("Courier New", 9); } }
        public Font MDS_Font { get { return new Font("Verdana", (float)8.8, FontStyle.Regular); } }

        public string File_Path { get; set; }
        public XmlDocument XML_Document { get; set; }
        public Dictionary<string, XmlDocument> PJS_Projects { get; set; }
        public Dictionary<string, XmlDocument> Files_List { get; set; }
        public bool MDS_Loaded { get; set; }
        public bool Load_Complete { get; set; }
        public TCVersion TestComplete_Version { get; set; }
        private bool _modified;
        public bool Modified { get { return _modified; } set { _modified = value; OnModifiedChanged(); } }

        public event EventHandler ModifiedChanged;

        public void OnModifiedChanged()
        {
            ModifiedChanged?.Invoke(this, EventArgs.Empty);
        }

        public BindingList<TreeNode> ModifiedNodes;

        public event ListChangedEventHandler ModifiedNodes_Changed { add { ModifiedNodes.ListChanged += value; } remove { ModifiedNodes.ListChanged -= value; } }

        public Dictionary<string, XmlDocument> LocalVariables_FilesList { get; set; }
        public BindingList<ProjectVariable> PersistentVariables_List { get; set; }
        public BindingList<ProjectVariable> TemporaryVariables_List { get; set; }
        private string[] _typesTemporary = new string[8] { "String", "Integer", "Double", "Boolean", "Object", "Table", "DB Table", "Password" };
        public string[] TemporaryVariables_Types { get { return _typesTemporary; } }
        private string[] _typesPersistent = new string[5] { "String", "Integer", "Double", "Boolean", "Password" };
        public string[] PersistentVariables_Types { get { return _typesPersistent; } }

        public Dictionary<TreeNode, bool> Original_Selected;
        public Dictionary<TreeNode, string> Original_Iterations;
        public Dictionary<TreeNode, string> Original_Value;
        public Dictionary<TreeNode, string> Original_CheckLevel;

        public TabPage Variables_Page { get; set; }
        public Modes Mode { get; set; }
        public bool Children_Selection { get; set; }
        public bool Parent_Selection { get; set; }
        public bool Display_Iterations { get; set; }
        public bool Mark_Edited { get; set; }
        public string TestComplete_Path { get; set; }
        private int _max_level;
        public int Max_Level { get { return _max_level; } set { _max_level = value > -1 ? value : 0; } }
        public int ScriptsForRun { get; set; }

        public List<TreeNode> Search_CurrentNodeMatches { get; set; }
        public int Search_LastNodeIndex { get; set; }
        public string Search_LastSearchText { get; set; }
    }
}