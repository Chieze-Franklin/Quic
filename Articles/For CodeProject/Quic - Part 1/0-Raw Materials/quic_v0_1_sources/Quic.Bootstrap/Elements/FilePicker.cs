using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a control that let's you pick a file
/// </summary>
public class FilePicker : InputElement
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //prefix
            if (HasAddon)
            {
                outputFile.WriteLine("<div class=\"input-group\">");
                outputFile.WriteLine(EmitPrefixAddon());
            }

            //opening tag
            outputFile.Write("<input type=\"file\"");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            if (Disabled)
                outputFile.Write(" disabled");//disabled

            //closing tag
            outputFile.Write("/>");

            //suffix
            if (HasAddon)
            {
                outputFile.WriteLine();
                outputFile.WriteLine(EmitSuffixAddon());
                outputFile.WriteLine("</div>");
            }
        }
    }
}