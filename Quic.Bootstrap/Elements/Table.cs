using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a table element
/// </summary>
public class Table : BootstrapContainerElement
{
    bool bordered, condensed, hover, striped;

    public Table()
    {
        CssClasses.Add("table");
        CssClasses.Add("table-responsive");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<table");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //caption
            if (!string.IsNullOrEmpty(Text))
            {
                outputFile.WriteLine(string.Format("<caption>{0}</caption>", EmitText()));
            }

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }

            //closing tag
            outputFile.WriteLine("</table>");
        }
    }

    public bool Bordered
    {
        get { return bordered; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("table-bordered"))
                    CssClasses.Add("table-bordered");
            }
            else
            {
                if (CssClasses.Contains("table-bordered"))
                    CssClasses.Remove("table-bordered");
            }

            bordered = value;
        }
    }
    public bool Condensed
    {
        get { return condensed; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("table-condensed"))
                    CssClasses.Add("table-condensed");
            }
            else
            {
                if (CssClasses.Contains("table-condensed"))
                    CssClasses.Remove("table-condensed");
            }

            condensed = value;
        }
    }
    public bool Hover
    {
        get { return hover; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("table-hover"))
                    CssClasses.Add("table-hover");
            }
            else
            {
                if (CssClasses.Contains("table-hover"))
                    CssClasses.Remove("table-hover");
            }

            hover = value;
        }
    }
    public bool Striped
    {
        get { return striped; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("table-striped"))
                    CssClasses.Add("table-striped");
            }
            else
            {
                if (CssClasses.Contains("table-striped"))
                    CssClasses.Remove("table-striped");
            }

            striped = value;
        }
    }
}