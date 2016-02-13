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
            Nav navParent = null;
            //----------
            if (ParentElement is Nav)
                navParent = (Nav)ParentElement;
            else if (ParentElement is DropMenu) 
            {
                if (ParentElement.ParentElement is Nav) //if the containing <DropMenu> is in a <Nav>
                    navParent = (Nav)ParentElement.ParentElement;
            }
            else if (ParentElement is Drop && ParentElement.ParentElement is DropMenu)
            {
                if (ParentElement.ParentElement.ParentElement is Nav) //if the containing <Drop> is in a <DropMenu> which is in a <Nav>
                    navParent = (Nav)ParentElement.ParentElement.ParentElement;
            }
            //----------
            if (navParent != null)
            {
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