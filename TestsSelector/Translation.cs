namespace TestsSelector
{
  public static class Translation
  {
    public static string Add = "Add";
    public static string Delete = "Delete";
    public static string New = "New";
    public static string File = "File";
    public static string Open = "Open";
    public static string Save = "Save";
    public static string Settings = "Settings";
    public static string Run = "Run";
    public static string TestsFile_Import = "Import Tests File...";
    public static string TestsFile_Export = "Export Tests File...";
    public static string CollapseAll = "Collapse All";
    public static string ExpandAll = "Expand All";
    public static string SelectAll = "Select All";
    public static string UnselectAll = "Unselect All";
    public static string Search = "Search";
    public static string About = "About";
    public static string CollapseToLevel = "Collapse To Level:";
    public static string ScriptsForRun = "Scripts for run:";
    public static string Version = "Version";
    public static string AllRightsReserved = "All rights reserved";
    public static string Parent_Option = "Change parent items selection (when checking item)";
    public static string Children_Option = "Change children items selection (when checking/unchecking item)";
    public static string Iterations_Option = "Display iteration number with item name";
    public static string MarkModified_Option = "Mark edited items on the tree";
    public static string Variables = "Variables";
    public static string TestItems = "Test Items";
    public static string ChangeIterations = "Change iterations";
    public static string CloseQuestion = "You are trying to close file which was modified.\nDo you want to save it before close?";
    public static string Warning = "Warning";
    public static string CommandLineHelp = "Command line help:\n\n"
    + "-h\n"
    + "    Displays this help message.\n\n"
    + "-p\n"
    + "    Parents selection.\n    Used command - enabled, not used command - disabled.\n\n"
    + "-c\n"
    + "    Children selection.\n    Used command - enabled, not used command - disabled.\n\n"
    + "-di\n"
    + "    Display iterations.\n    Used command - enabled, not used command - disabled.\n\n"
    + "-f [path]\n"
    + "    This command is mandatory. Load file from given path.\n\n"
    + "-t [path]\n"
    + "    Load file with tests for selection from given path.\n\n"
    + "-tc [path]\n"
    + "    Path to TestExecute or TestComplete for run PJS/MDS files.\n\n"
    + "-r\n"
    + "    Run PJS/MDS file with TestExecute or TestComplete.\n\n"
    + "-s\n"
    + "    Save PJS/MDS/XML file.\n\n"
    + "-x\n"
    + "    Close application.";
    public static string FileReadOnly = "File is Read Only";
    public static string TypeNotSupported = "Type ({0}) is not supported";
    public static string NoLocalVariable = "No local variable file!";
    public static string VarColumn_Name = "Name";
    public static string VarColumn_Type = "Type";
    public static string VarColumn_Default = "Default Value";
    public static string VarColumn_Local = "Local Value";
    public static string VarColumn_Description = "Description";
    public static string VarColumn_Category = "Category";
    public static string VarNameInvalid = "The variable name \"{0}\" is invalid. A variable name can include letters, digits and underscores, and must start with a letter or an underscore.";
    public static string Language = "Language";
    public static string Exception_Handling = "Some exception occured in application :-(\nPlease use send button below to help fix it.\n\n\nYou may add some information or contact too:";
    public static string Exception_Title = "Exception Handling";
    public static string Send = "Send";
    public static string Help = "Help";
    public static string CommandLine = "Command Line";
    public static string AboutText = string.Format("Tests Selector\n{0} {1}\n{2}\n{3}", Version, "{0}", "{1}", AllRightsReserved);
  }
}
