using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class Drop : List
{
    public Drop()
    {
        CssClasses.Add("dropdown-menu");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<ul");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            outputFile.Write(" role=\"menu\"");
            outputFile.WriteLine(">");
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</ul>");
        }
    }
}