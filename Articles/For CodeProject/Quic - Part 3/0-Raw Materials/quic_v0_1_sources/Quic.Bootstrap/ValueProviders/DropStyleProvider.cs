using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class DropStyleProvider : ValueProvider
{
    static DropStyleProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static DropStyleProvider Singleton()
    {
        if (singleton == null)
            singleton = new DropStyleProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is DropStyle)
        {
            return (DropStyle)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Down", StringComparison.CurrentCultureIgnoreCase))
            return DropStyle.Down;
        else if (input.Equals("Up", StringComparison.CurrentCultureIgnoreCase))
            return DropStyle.Up;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised drop style '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return DropStyle.Down;
        }
    }
}

/// <summary>
/// A list of the possible drop styles
/// </summary>
public enum DropStyle
{
    Down = 0,
    Up
}