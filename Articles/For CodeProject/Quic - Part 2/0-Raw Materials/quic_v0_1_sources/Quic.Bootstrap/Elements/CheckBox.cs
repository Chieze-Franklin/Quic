using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a check box
/// </summary>
public class CheckBox : CheckElement
{
    static int checkboxInstances = 1;

    public CheckBox()
    {
        //CssClasses.Add("checkbox-inline");
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
                //the attribute "checkbox-inline" doesnt play well here
                if (CssClasses.Contains("checkbox-inline"))
                    CssClasses.Remove("checkbox-inline");
            }
            outputFile.Write(EmitClasses());
            outputFile.Write(">");

            //input tag
            outputFile.Write("<input type=\"checkbox\""); //type attri
            outputFile.Write(EmitID());
            outputFile.Write(string.Format(" value=\"checkbox{0}\"", checkboxInstances++)); //value attri

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
}

/// <summary>
/// Represents a check box
/// </summary>
public class Check : CheckBox { }