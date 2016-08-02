using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Root container for custom controls.
/// Helps to setup the outputed file.
/// </summary>
public class Page : OrientationContainer
{
    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Build")
        {
            return BuildTypeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    void ImportNecessaryFiles()
    {
        if (this.Document.OutputFile is HtmlOutputFile)
        {
            HtmlOutputFile outputFile = (HtmlOutputFile)this.Document.OutputFile;

            outputFile.HeadSection.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            outputFile.HeadSection.AppendLine("<!-- Bootstrap -->");
            outputFile.HeadSection.AppendLine("<link href=\"css/bootstrap.min.css\" rel=\"stylesheet\">");
            outputFile.HeadSection.AppendLine("<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->");
            outputFile.HeadSection.AppendLine("<!-- WARNING: Respond.js doesn't work if you view the page");
            outputFile.HeadSection.AppendLine("via file:// --> <!--[if lt IE 9]>");
            outputFile.HeadSection.AppendLine("<script src=\"https://oss.maxcdn.com/libs/html5shiv/3.7.0/ html5shiv.js\"></script>");
            outputFile.HeadSection.AppendLine("<script src=\"https://oss.maxcdn.com/libs/respond.js/1.3.0/ respond.min.js\"></script>");
            outputFile.HeadSection.AppendLine("<![endif]-->");
            outputFile.HeadSection.AppendLine();
            outputFile.HeadSection.AppendLine("<!-- It is recommended, as a thank you, we ask you to include an optional link back to");
            outputFile.HeadSection.AppendLine("     GLYPHICONS (http://glyphicons.com) whenever practical. -- Bootstrap Documentation -->");
        }
        else if (this.Document.OutputFile is TextOutputFile)
        {
            TextOutputFile outputFile = (TextOutputFile)this.Document.OutputFile;

            outputFile.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            outputFile.WriteLine("<link href=\"css/bootstrap.min.css\" rel=\"stylesheet\">");
        }
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextOutputFile)
        {
            ImportNecessaryFiles();

            TextOutputFile outputFile = (TextOutputFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<form role=\"form\"");
            outputFile.Write(EmitID());
            if (this.Orientation == Orientation.Horizontal)
                CssClasses.Add("form-inline");//CssClasses.Add("form-horizontal");
            //else if (this.Orientation == LayoutOrientation.InLine)
            //    CssClasses.Add("form-inline");
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in UIElements)
            {
                //if (!(element is Menu))
                //{
                //    outputFile.WriteLine("<div class=\"form-group\">");
                //}
                element.BeginRender();
                //if (!(element is Menu))
                //{
                //    outputFile.WriteLine();
                //    outputFile.WriteLine("</div>");
                //}
            }

            //closing tag
            outputFile.WriteLine("</form>");

            //at the end of the file
            outputFile.WriteLine("<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->");
            if (Build == BuildType.Local)
                outputFile.WriteLine("<script src=\"js/jquery.js\"></script>");
            if (Build == BuildType.Web)
                outputFile.WriteLine("<script src=\"https://code.jquery.com/jquery.js\"></script>");
            outputFile.WriteLine("<!-- Include all compiled plugins (below), or include individual files as needed -->");
            outputFile.WriteLine("<script src=\"js/bootstrap.min.js\"></script>");
        }
    }

    /// <summary>
    /// Gets or sets the page build type
    /// </summary>
    public BuildType Build { get; set; }
}