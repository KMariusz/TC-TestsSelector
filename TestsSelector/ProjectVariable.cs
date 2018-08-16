using System.Collections.Generic;
using System.Xml;

namespace TestsSelector
{
    public class ProjectVariable
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string Local { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public XmlNode Local_XML_Node { get; set; }
        public XmlNode Project_XML_Node { get; set; }
        public bool IsTemporary { get; set; }
        public List<string> Table_Columns { get; set; }
        public List<List<string>> Table_Rows { get; set; }
    }
}