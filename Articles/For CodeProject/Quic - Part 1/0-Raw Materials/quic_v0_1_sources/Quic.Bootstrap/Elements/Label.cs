using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a label
/// </summary>
public class Label : InputElement
{
    bool help;

    public Label()
    {
        CssClasses.Add("control-label");
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
            outputFile.Write("<label");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //text, closing tag
            outputFile.Write(string.Format(">{0}</label>", EmitText()));

            //suffix
            if (HasAddon)
            {
                outputFile.WriteLine();
                outputFile.WriteLine(EmitSuffixAddon());
                outputFile.WriteLine("</div>");
            }
        }
    }

    /// <summary>
    /// Gets or sets a value to determine if the text should serve as a help text.
    /// </summary>
    public bool IsHelp
    {
        get { return help; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("help-block"))
                    CssClasses.Add("help-block");
            }
            else
            {
                if (CssClasses.Contains("help-block"))
                    CssClasses.Remove("help-block");
            }

            help = value;
        }
    }
}