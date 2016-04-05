using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class Pager : List
{
    public Pager()
    {
        CssClasses.Add("pager");
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
            //custom properties
            outputFile.Write(EmitCustomProperties());
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

    public bool AlignLinks { get; set; }
}