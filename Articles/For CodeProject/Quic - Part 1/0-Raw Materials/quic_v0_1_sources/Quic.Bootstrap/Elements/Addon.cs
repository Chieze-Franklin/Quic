using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents an add-on in an input group
/// </summary>
public class Addon : BootstrapContainerElement
{
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            if (Elements.Count > 0 && !(Elements[0] is DropButton))
            {
                outputFile.Write("<span");
                outputFile.Write(EmitID());
                if (Elements.Count > 0 && (Elements[0] is Button))
                {
                    if (CssClasses.Contains("input-group-addon"))
                        CssClasses.Remove("input-group-addon");
                    if (!CssClasses.Contains("input-group-btn"))
                        CssClasses.Add("input-group-btn");
                }
                else
                {
                    if (CssClasses.Contains("input-group-btn"))
                        CssClasses.Remove("input-group-btn");
                    if (!CssClasses.Contains("input-group-addon"))
                        CssClasses.Add("input-group-addon");
                }
                outputFile.Write(EmitClasses());

                //custom properties
                outputFile.Write(EmitCustomProperties());

                //opening tag 2
                outputFile.WriteLine(">");
            }

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }

            //closing tag
            if (Elements.Count > 0 && !(Elements[0] is DropButton))
            {
                outputFile.WriteLine("</span>");
            }
        }
    }
}