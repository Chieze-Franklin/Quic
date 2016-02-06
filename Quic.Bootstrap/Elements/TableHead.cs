using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents the head of a table
/// </summary>
public class TableHead : BootstrapContainerElement
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<thead");
            outputFile.Write(EmitID());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }

            //closing tag
            outputFile.WriteLine("</thead>");
        }
    }
}

//alias

/// <summary>
/// Represents the head of a table
/// </summary>
public class THead : TableHead { }