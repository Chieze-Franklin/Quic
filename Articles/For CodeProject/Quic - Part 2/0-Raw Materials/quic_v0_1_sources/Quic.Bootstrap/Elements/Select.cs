using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents an element that can have child elements, where one can be selected
/// </summary>
public class Select : BootstrapContainerElement
{
    public Select()
    {
        IsFormControl = true;
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //text
            outputFile.Write("<label>");
            if (!string.IsNullOrEmpty(Name))
            {
                outputFile.Write(Name);
            }
            outputFile.WriteLine("</label>");

            //select
            outputFile.Write("<select");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            //custom properties
            outputFile.Write(EmitCustomProperties());
            if (Multiple)
                outputFile.Write(" multiple");
            if (Disabled)
                outputFile.Write(" disabled");
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</select>");
        }
    }

    /// <summary>
    /// Gets or sets a value to determine if multiple child elements should be displayed
    /// </summary>
    public bool Multiple { get; set; }
}