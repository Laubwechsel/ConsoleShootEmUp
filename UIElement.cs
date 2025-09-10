using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp;

internal struct UIElement
{
    public int Order;
    public string Name;
    public string Value;
    public ConsoleColor NameColor;
    public ConsoleColor ValueColor;
    public UIElement()
    {
        Order = 0;
        Name = "";
        Value = "";
        NameColor = ConsoleColor.Gray;
        ValueColor = ConsoleColor.Gray;
    }
    public UIElement(int order, string name, string value, ConsoleColor nameColor = ConsoleColor.Gray, ConsoleColor valueColor = ConsoleColor.Gray)
    {
        Order = order;
        Name = name;
        Value = value;
        NameColor = nameColor;
        ValueColor = valueColor;
    }
}
