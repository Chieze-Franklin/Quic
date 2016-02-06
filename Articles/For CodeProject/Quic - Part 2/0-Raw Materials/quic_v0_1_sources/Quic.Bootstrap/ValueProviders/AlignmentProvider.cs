using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class AlignmentProvider : ValueProvider
{
    static AlignmentProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static AlignmentProvider Singleton()
    {
        if (singleton == null)
            singleton = new AlignmentProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is Alignment)
        {
            return (Alignment)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Left", StringComparison.CurrentCultureIgnoreCase))
            return Alignment.Left;
        else if (input.Equals("Right", StringComparison.CurrentCultureIgnoreCase))
            return Alignment.Right;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised alignment '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return Alignment.Left;
        }
    }
}

/// <summary>
/// A list of the possible alignments
/// </summary>
public enum Alignment
{
    Left = 0,
    Right
}