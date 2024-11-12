using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneratorApp.Classes // Match your project namespace
{
    public class TemplateField
    {
        public string Name { get; }
        public string Label { get; }
        public FieldType Type { get; }

        public TemplateField(string name, string label, FieldType type)
        {
            Name = name;
            Label = label;
            Type = type;
        }
    }
}

