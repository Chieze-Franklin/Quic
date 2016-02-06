using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents an area (usually large) into which text could be written
/// </summary>
public class TextArea : InputElement
{
    public TextArea() 
    {
        IsFormControl = true;
    }

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
            outputFile.Write("<textarea");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //hint
            outputFile.Write(EmitPlaceholder());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            if (Disabled)
                outputFile.Write(" disabled");

            //text, closing tag
            string txt = Text != null ? Text : (Content != null ? Content : "");
            outputFile.Write(string.Format(">{0}</textarea>", txt));//dont use EmitText() here

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