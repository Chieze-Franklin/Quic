using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a clickable link
/// </summary>
public class LinkButton : Button, ILinkElement
{
    public LinkButton()
    {
        if (CssClasses.Contains("btn-default"))
            CssClasses.Remove("btn-default");
        CssClasses.Add("btn-link");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //prefix
            if (HasAddon)
            {
                outputFile.WriteLine("<div class=\"input-group\">");
                outputFile.WriteLine(EmitPrefixAddon());
            }

            //opening tag
            outputFile.Write("<a");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //text, closing tag
            //unlike for an image where we "work on" the Source, here we don't alter the Link
            outputFile.Write(string.Format("{0}>{1}</a>", (Link != null ? " href=\"" + Link + "\"" : ""),
                (ButtonType == ButtonType.Close ? "&times;" : EmitText())));

            //suffix
            if (HasAddon)
            {
                outputFile.WriteLine();
                outputFile.WriteLine(EmitSuffixAddon());
                outputFile.WriteLine("</div>");
            }
        }
    }

    /// <summary>
    /// Gets or sets the link of the element
    /// </summary>
    public string Link { get; set; }
}

/// <summary>
/// Represents a clickable link
/// </summary>
public class Link : LinkButton { }