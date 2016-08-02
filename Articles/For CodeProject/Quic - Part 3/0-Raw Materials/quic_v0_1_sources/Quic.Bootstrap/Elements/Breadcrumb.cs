using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class Breadcrumb : List
{
    public Breadcrumb()
    {
        CssClasses.Add("breadcrumb");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<ol");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            outputFile.WriteLine(">");
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</ol>");
        }
    }
}