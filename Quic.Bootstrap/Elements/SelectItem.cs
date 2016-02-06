using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents an item/option in a Select container
/// </summary>
public class SelectItem : BootstrapContainerElement
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<option");
            outputFile.Write(EmitID());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            if (Disabled)
                outputFile.Write(" disabled");

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</option>");
        }
    }
}

//alias

/// <summary>
/// Represents an item/option in a Select container
/// </summary>
public class SI : SelectItem { }
/// <summary>
/// Represents an item/option in a Select container
/// </summary>
public class Option : SelectItem { }