using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class OrientationProvider : ValueProvider
{
    static OrientationProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static OrientationProvider Singleton()
    {
        if (singleton == null)
            singleton = new OrientationProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is Orientation)
        {
            return (Orientation)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Vertical", StringComparison.CurrentCultureIgnoreCase))
            return Orientation.Vertical;
        else if (input.Equals("Horizontal", StringComparison.CurrentCultureIgnoreCase))
            return Orientation.Horizontal;
        //else if (input.Equals("InLine", StringComparison.CurrentCultureIgnoreCase))
        //    return LayoutOrientation.InLine;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised orientation '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return Orientation.Vertical;
        }
    }
}

/// <summary>
/// A list of the possible orientations
/// </summary>
public enum Orientation
{
    Vertical = 0,
    Horizontal,
    //InLine
}