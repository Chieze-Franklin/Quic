using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;
using Quic.Utils;

/// <summary>
/// Root container for custom controls.
/// </summary>
public class Form : OrientationContainer
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
        //create the icon file
        if (Icon != null)
        {
            try
            {
                string absolutePath = FileSystemServices.GetAbsolutePath(Icon, this.Document.SourcePath);
                string imgName = Path.GetFileName(absolutePath);
                BytesFile iconOutput = new BytesFile(imgName);
                ((BytesFile)iconOutput).Bytes = FileSystemServices.ReadBytesFromFile(absolutePath);
                this.Document.OutputDirectory.Add(iconOutput, true);
            }
            catch (Exception ex)
            {
                Messenger.Notify(
                        new QuicException(ex.Message,
                            this.Document.SourcePath, this.Line, this.Column, ex));
            }
        }

        if (this.Document.OutputFile is HtmlOutputFile)
        {
            HtmlOutputFile outputFile = (HtmlOutputFile)this.Document.OutputFile;

            outputFile.HeadSection.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            outputFile.HeadSection.WriteLine("<!-- Bootstrap -->");
            outputFile.HeadSection.WriteLine("<link href=\"css/bootstrap.min.css\" rel=\"stylesheet\">");
            outputFile.HeadSection.WriteLine("<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->");
            outputFile.HeadSection.WriteLine("<!-- WARNING: Respond.js doesn't work if you view the page");
            outputFile.HeadSection.WriteLine("via file:// --> <!--[if lt IE 9]>");
            outputFile.HeadSection.WriteLine("<script src=\"https://oss.maxcdn.com/libs/html5shiv/3.7.0/ html5shiv.js\"></script>");
            outputFile.HeadSection.WriteLine("<script src=\"https://oss.maxcdn.com/libs/respond.js/1.3.0/ respond.min.js\"></script>");
            outputFile.HeadSection.WriteLine("<![endif]-->");
            if (Icon != null)
            {
                outputFile.HeadSection.WriteLine(string.Format("<link rel=\"icon\" href=\"{0}\">", Icon));
            }
        }
        else if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            outputFile.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            outputFile.WriteLine("<link href=\"css/bootstrap.min.css\" rel=\"stylesheet\">");
            if (Icon != null)
            {
                outputFile.WriteLine(string.Format("<link rel=\"icon\" href=\"{0}\">", Icon));
            }
        }
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            ImportNecessaryFiles();

            TextFile outputFile = (TextFile)this.Document.OutputFile;

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
            foreach (var element in Elements)
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
    /// <summary>
    /// Gets or sets the path to the image to use as the page icon
    /// </summary>
    public string Icon { get; set; }
}