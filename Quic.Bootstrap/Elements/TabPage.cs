using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class TabPage : BootstrapContainerElement
{
    public TabPage()
    {
        CssClasses.Add("tab-pane");
        CssClasses.Add("fade");
    }

    public override void Render() 
    {
        if (this.Document.OutputFile is TextFile) 
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            if (this.ParentElement != null && this.ParentElement.Elements.IndexOf(this) == 0) //if this is the first child of its parent
                this.CssClasses.Add("in");

            //opening tag
            outputFile.Write("<div");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }

            //closing tag
            outputFile.WriteLine("</div>");
        }
    }
}