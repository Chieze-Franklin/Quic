using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a (drop-down) menu divider
/// </summary>
public class DividerMenuItem : MenuItem
{
    public DividerMenuItem()
    {
        CssClasses.Add("divider");
        IsContainer = false;
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<li role=\"presentation\"");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //closing tag
            outputFile.WriteLine("></li>");
        }
    }
}

//alias

/// <summary>
/// Represents a (drop-down) menu divider
/// </summary>
public class Divider : DividerMenuItem { }