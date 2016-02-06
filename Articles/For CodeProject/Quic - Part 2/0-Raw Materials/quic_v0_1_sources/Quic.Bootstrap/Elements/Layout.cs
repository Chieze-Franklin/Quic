using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// A basic layout
/// </summary>
public class Layout : OrientationContainer
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<span");
            outputFile.Write(EmitID());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
                if (Orientation == Orientation.Vertical)
                {
                    outputFile.WriteLine("<br />");
                }
            }

            //closing tag
            outputFile.WriteLine("</span>");
        }
    }
}