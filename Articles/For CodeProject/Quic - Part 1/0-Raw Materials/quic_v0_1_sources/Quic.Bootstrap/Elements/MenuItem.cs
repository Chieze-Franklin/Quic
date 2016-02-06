using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a menu item
/// </summary>
public class MenuItem : ListItem, ILinkElement
{
    public MenuItem()
    {
        IsFormControl = false;
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<li role=\"presentation\"");

            outputFile.Write("><a role=\"menuitem\"");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            //unlike for an image where we "work on" the Source, here we don't alter the Link
            outputFile.Write(string.Format("{0}>", (Link != null ? " href=\"" + Link + "\"" : "")));

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</a></li>");
        }
    }

    /// <summary>
    /// Gets or sets the link of the control
    /// </summary>
    public string Link { get; set; }
}

//alias

/// <summary>
/// Represents a menu item
/// </summary>
public class MI : MenuItem { }