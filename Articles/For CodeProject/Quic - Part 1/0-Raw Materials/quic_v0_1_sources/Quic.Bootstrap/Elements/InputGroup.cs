using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// A container that can append or prepend elements to an input element
/// </summary>
public class InputGroup : InputElement
{
    InputSize inputSz;

    public InputGroup()
    {
        CssClasses.Add("input-group");
        IsContainer = true;
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<div");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //prefix
            if (HasAddon)
            {
                outputFile.WriteLine("<div class=\"input-group\">");
                outputFile.WriteLine(EmitPrefixAddon());
            }

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }

            //suffix
            if (HasAddon)
            {
                outputFile.WriteLine();
                outputFile.WriteLine(EmitSuffixAddon());
                outputFile.WriteLine("</div>");
            }

            //closing tag
            outputFile.WriteLine("</div>");
        }
    }

    /// <summary>
    /// Gets or sets the input size
    /// </summary>
    public override InputSize InputSize
    {
        get { return inputSz; }
        set
        {
            if (value == InputSize.ExtraLarge || value == InputSize.Large)
            {
                if (!CssClasses.Contains("input-group-lg"))
                    CssClasses.Add("input-group-lg");
            }
            else
            {
                if (CssClasses.Contains("input-group-lg"))
                    CssClasses.Remove("input-group-lg");
            }

            if (value == InputSize.ExtraSmall || value == InputSize.Small)
            {
                if (!CssClasses.Contains("input-group-sm"))
                    CssClasses.Add("input-group-sm");
            }
            else
            {
                if (CssClasses.Contains("input-group-sm"))
                    CssClasses.Remove("input-group-sm");
            }

            inputSz = value;
        }
    }
}