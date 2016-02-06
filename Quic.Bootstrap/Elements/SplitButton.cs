using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class SplitButton : DropButton
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //parent div
            string dropstyle = "dropdown";
            if (DropStyle == DropStyle.Up)
                dropstyle = "dropup";
            if (this.ParentElement is Addon)
                dropstyle += " input-group-btn";
            else
                dropstyle += " btn-group";
            outputFile.WriteLine(string.Format("<div class=\"{0}\">", dropstyle));

            //LEFT BUTTON
            //opening tag
            outputFile.Write("<button type=\"button\"");
            outputFile.Write(EmitID());
            if (CssClasses.Contains("dropdown-toggle"))
                CssClasses.Remove("dropdown-toggle"); //remove the class "dropdown-toggle"
            outputFile.Write(EmitClasses());
            //custom properties
            outputFile.Write(EmitCustomProperties());
            if (Disabled)
                outputFile.Write(" disabled");//disabled
            outputFile.WriteLine(">");
            //text
            outputFile.WriteLine(EmitText());
            //closing tag
            outputFile.WriteLine("</button>");

            //RIGHT BUTTON
            //opening tag
            outputFile.Write("<button type=\"button\"");
            if (!CssClasses.Contains("dropdown-toggle"))
                CssClasses.Add("dropdown-toggle"); //add the class "dropdown-toggle"
            outputFile.Write(EmitClasses());
            outputFile.Write(" data-toggle=\"dropdown\"");
            if (Disabled)
                outputFile.Write(" disabled");//disabled
            outputFile.WriteLine(">");

            outputFile.WriteLine("<span class=\"caret\"></span>");
            outputFile.WriteLine("</button>");

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
            outputFile.WriteLine("</div>");
        }
    }
}