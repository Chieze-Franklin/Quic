using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a drop menu
/// </summary>
public class DropMenu : DropButton
{
    public DropMenu()
    {
        if (CssClasses.Contains("btn"))
            CssClasses.Remove("btn");
        if (CssClasses.Contains("btn-default"))
            CssClasses.Remove("btn-default");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            string dropstyle = "dropdown";
            if (DropStyle == DropStyle.Up)
                dropstyle = "dropup";
            outputFile.WriteLine(string.Format("<li class=\"{0}\">", dropstyle));

            //drop down button
            outputFile.Write("<a");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            outputFile.Write(" href=\"#\" data-toggle=\"dropdown\"");
            //custom properties
            outputFile.Write(EmitCustomProperties());
            outputFile.WriteLine(">");

            //text
            outputFile.WriteLine(EmitText());
            outputFile.WriteLine("<b class=\"caret\"></b>");
            outputFile.WriteLine("</a>");

            //checks
            if (Elements.Count > 0 && !(Elements[0] is Drop))
            {
                outputFile.WriteLine("<ul class=\"dropdown-menu\" role=\"menu\">");
            }

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //checks
            if (Elements.Count > 0 && !(Elements[0] is Drop))
            {
                outputFile.WriteLine("</ul>");
            }

            //closing tag
            outputFile.WriteLine("</li>");
        }
    }
}