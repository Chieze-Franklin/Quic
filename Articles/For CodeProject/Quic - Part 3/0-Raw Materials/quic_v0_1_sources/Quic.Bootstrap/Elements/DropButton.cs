using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a button with drop menu
/// </summary>
public class DropButton : Button
{
    public DropButton()
    {
        CssClasses.Add("dropdown-toggle");
        IsContainer = true;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "DropStyle")
        {
            return DropStyleProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

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
            outputFile.WriteLine(string.Format("<div class=\"{0}\">", dropstyle));

            //opening tag
            outputFile.Write("<button type=\"button\"");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            outputFile.Write(" data-toggle=\"dropdown\"");
            //custom properties
            outputFile.Write(EmitCustomProperties());

            if (Disabled)
                outputFile.Write(" disabled");//disabled

            //opening tag 2
            outputFile.WriteLine(">");

            //text
            outputFile.WriteLine(EmitText());
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

    /// <summary>
    /// Gets or sets the drop style
    /// </summary>
    public DropStyle DropStyle { get; set; }
}