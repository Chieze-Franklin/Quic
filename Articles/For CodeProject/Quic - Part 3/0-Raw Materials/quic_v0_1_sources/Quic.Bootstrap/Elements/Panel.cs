using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// A simple container for elements
/// </summary>
public class Panel : BootstrapContainerElement
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<fieldset");
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
            outputFile.WriteLine("</fieldset>");
        }
    }
}