using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a nav item
/// </summary>
public class NavItem : MenuItem
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<li");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            outputFile.Write("><a");

            //data-toggle
            var dataToggle = "tab";
            if (ParentElement is Nav)
            {
                var navParent = (Nav)ParentElement;
                if (navParent.Type == NavType.JustifiedPills || navParent.Type == NavType.Pills || navParent.Type == NavType.VerticalPills)
                {
                    dataToggle = "pill";
                }
            }

            //opening tag 2
            //unlike for an image where we "work on" the Source, here we don't alter the Link
            outputFile.Write(string.Format("{0} data-toggle=\"{1}\">", (Link != null ? " href=\"" + Link + "\"" : ""), dataToggle));

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
}

/// <summary>
/// Represents a nav item
/// </summary>
public class NI : NavItem { }