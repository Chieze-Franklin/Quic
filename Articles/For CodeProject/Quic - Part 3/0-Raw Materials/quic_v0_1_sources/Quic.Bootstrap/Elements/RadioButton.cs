using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a radio button
/// </summary>
public class RadioButton : CheckElement
{
    static int radioInstances = 1;

    public RadioButton()
    {
        //CssClasses.Add("radio-inline");
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

            //parent label tag
            outputFile.Write("<span");
            if (this.ParentElement is Addon)
            {
                //the attribute "radio-inline" doesnt play well here
                if (CssClasses.Contains("radio-inline"))
                    CssClasses.Remove("radio-inline");
            }
            outputFile.Write(EmitClasses());
            outputFile.Write(">");

            //input tag
            outputFile.Write("<input type=\"radio\""); //type attri
            outputFile.Write(EmitID());
            if (!string.IsNullOrEmpty(Group))
                outputFile.Write(string.Format(" name=\"{0}\"", Group)); //name (of group) attri
            outputFile.Write(string.Format(" value=\"radio{0}\"", radioInstances++)); //value attri

            //custom properties
            outputFile.Write(EmitCustomProperties());

            if (Checked)
                outputFile.Write(" checked");//checked

            if (Disabled)
                outputFile.Write(" disabled");//disabled

            //text, closing tag
            outputFile.Write(string.Format("/>{0}</span>", EmitText()));

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
    /// Gets or sets the group name
    /// </summary>
    public string Group { get; set; }
}

public class Radio : RadioButton { }