using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class BreadcrumbItem : NavItem
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

            outputFile.Write(">");

            //opening tag 2
            if (State != State.Active)
            {
                //unlike for an image where we "work on" the Source, here we don't alter the Link
                outputFile.Write(string.Format("<a{0}>", (Link != null ? " href=\"" + Link + "\"" : "")));
            }

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            if (State != State.Active)
            {
                outputFile.Write("</a>");
            }
            outputFile.WriteLine("</li>");
        }
    }
}

public class BI : BreadcrumbItem
{
}