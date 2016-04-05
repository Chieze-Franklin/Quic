using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class Next : MenuItem
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            if (ParentElement is Pager && ((Pager)ParentElement).AlignLinks)
                CssClasses.Add("next");

            //opening tag
            outputFile.Write("<li");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            //custom properties
            outputFile.Write(EmitCustomProperties());

            outputFile.Write("><a");

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
}