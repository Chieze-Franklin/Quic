using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class ImageStyleProvider : ValueProvider
{
    static ImageStyleProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ImageStyleProvider Singleton()
    {
        if (singleton == null)
            singleton = new ImageStyleProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is ImageStyle)
        {
            return (ImageStyle)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Circle", StringComparison.CurrentCultureIgnoreCase))
            return ImageStyle.Circle;
        else if (input.Equals("Rounded", StringComparison.CurrentCultureIgnoreCase))
            return ImageStyle.Rounded;
        else if (input.Equals("Thumbnail", StringComparison.CurrentCultureIgnoreCase))
            return ImageStyle.Thumbnail;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised image style '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return ImageStyle.Rounded;
        }
    }
}

/// <summary>
/// A list of the possible image styles
/// </summary>
public enum ImageStyle
{
    Rounded,
    Circle,
    Thumbnail
}