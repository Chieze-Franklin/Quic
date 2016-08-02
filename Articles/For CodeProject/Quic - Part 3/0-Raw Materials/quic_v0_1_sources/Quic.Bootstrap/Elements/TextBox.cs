using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a (usually narrow) box into which text can be typed
/// </summary>
public class TextBox : InputElement
{
    public TextBox() 
    {
        IsFormControl = true;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Type")
        {
            return TextTypeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
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
            string type = "text";
            if (Type == TextType.Email)
                type = "email";
            else if (Type == TextType.Password)
                type = "password";

            outputFile.Write(string.Format("<input type=\"{0}\"", type));
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //hint
            outputFile.Write(EmitPlaceholder());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //text
            string txt = Text != null ? Text : (Content != null ? Content : "");
            outputFile.Write(string.Format(" value=\"{0}\"", txt));//dont use EmitText() here

            if (Disabled)
                outputFile.Write(" disabled");

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

    /// <summary>
    /// Gets or sets the text type
    /// </summary>
    public TextType Type { get; set; }
}