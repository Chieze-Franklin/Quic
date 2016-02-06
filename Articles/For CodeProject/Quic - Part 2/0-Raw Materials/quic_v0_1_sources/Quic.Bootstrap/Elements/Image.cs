using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Quic;
using Quic.Utils;
using Quic.Messaging;

/// <summary>
/// Represents an image element
/// </summary>
public class Image : BootstrapElement, ISourceElement
{
    ImageStyle imgStyle;

    public Image()
    {
        CssClasses.Add("img-responsive");
        IsFormControl = false;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "ImageStyle")
        {
            return ImageStyleProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<img");
            outputFile.Write(EmitID());

            if (Source != null)
            {
                try
                {
                    //get image absolute path
                    string absolutePath = FileSystemServices.GetAbsolutePath(Source, this.Document.SourcePath);
                    string finalPath = "";
                    //check if the image has already been saved
                    if (savedImages.ContainsKey(absolutePath.ToLower()))
                    {
                        finalPath = savedImages[absolutePath.ToLower()];
                    }
                    else
                    {
                        var imgObj = this.Document.OutputDirectory.Get("images");
                        if (imgObj == null)
                        {
                            imgObj = new OutputDirectory("images");
                            this.Document.OutputDirectory.Add(imgObj, true);
                        }
                        if (imgObj is OutputDirectory)
                        {
                            string imgName = Path.GetFileNameWithoutExtension(absolutePath);
                            string imgExt = Path.GetExtension(absolutePath);

                            var ImgDir = (OutputDirectory)imgObj;

                            string uniqueName = imgName + imgExt;
                            var imgOutput = ImgDir.Get(uniqueName);
                            int copyCount = 2;
                            while (imgOutput != null) //while an image with that name exists
                            {
                                //try names like "<img-name>2.png", "<img-name>3.png", "<img-name>4.png",...
                                uniqueName = imgName + copyCount + imgExt;
                                imgOutput = ImgDir.Get(uniqueName);
                                copyCount++;
                            }
                            //if (imgOutput == null) //not necessary since the while loop above ensures that it MUST be null on this line
                            {
                                imgOutput = new BytesFile(uniqueName);
                                //Bitmap image = new Bitmap(absolutePath);
                                //MemoryStream stream = new MemoryStream();
                                //image.Save(stream, ImageFormat.Png);
                                //((BytesOutputFile)imgOutput).Bytes = stream.GetBuffer();
                                ((BytesFile)imgOutput).Bytes = FileSystemServices.ReadBytesFromFile(absolutePath);
                                ImgDir.Add(imgOutput, true);

                                finalPath = "images\\" + uniqueName;
                                savedImages.Add(absolutePath.ToLower(), finalPath);
                            }
                        }
                    }

                    outputFile.Write(" src=\"" + finalPath + "\"");
                }
                catch (Exception ex)
                {
                    Messenger.Notify(
                            new QuicException(ex.Message,
                                this.Document.SourcePath, this.Line, this.Column, ex));
                }
            }
            outputFile.Write(EmitClasses());
            if (Hint != null)
            {
                outputFile.Write(string.Format(" alt=\"{0}\"", Hint));
            }

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //closing tag
            outputFile.Write(">");
        }
    }

    static Dictionary<string, string> savedImages = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the path to the image
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the image style
    /// </summary>
    public ImageStyle ImageStyle
    {
        get { return imgStyle; }
        set
        {
            if (value == ImageStyle.Circle)
            {
                if (!CssClasses.Contains("img-circle"))
                    CssClasses.Add("img-circle");
            }
            else
            {
                if (CssClasses.Contains("img-circle"))
                    CssClasses.Remove("img-circle");
            }

            if (value == ImageStyle.Rounded)
            {
                if (!CssClasses.Contains("img-rounded"))
                    CssClasses.Add("img-rounded");
            }
            else
            {
                if (CssClasses.Contains("img-rounded"))
                    CssClasses.Remove("img-rounded");
            }

            if (value == ImageStyle.Thumbnail)
            {
                if (!CssClasses.Contains("img-thumbnail"))
                    CssClasses.Add("img-thumbnail");
            }
            else
            {
                if (CssClasses.Contains("img-thumbnail"))
                    CssClasses.Remove("img-thumbnail");
            }

            imgStyle = value;
        }
    }
}

//alias

/// <summary>
/// Represents an image element
/// </summary>
public class Img : Image { }