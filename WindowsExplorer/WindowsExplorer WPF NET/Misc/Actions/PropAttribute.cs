using System;
using WindowsExplorer_WPF_NET.Misc.Data;

namespace WindowsExplorer_WPF.Misc
{
    internal class PropAttribute : Attribute
    {
        public PropAttribute(FieldName name, FieldType @string)
        {
            Name = name;
            String = @string;
        }

        public FieldName Name { get; }
        public FieldType String { get; }
    }
}