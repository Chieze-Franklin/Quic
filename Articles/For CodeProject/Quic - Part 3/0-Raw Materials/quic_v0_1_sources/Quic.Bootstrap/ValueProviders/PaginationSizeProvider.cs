using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class PaginationSizeProvider : ValueProvider
{
    static PaginationSizeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static PaginationSizeProvider Singleton()
    {
        if (singleton == null)
            singleton = new PaginationSizeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is PaginationSize)
        {
            return (PaginationSize)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase))
            return PaginationSize.Normal;
        else if (input.Equals("Xl", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("ExtraLarge", StringComparison.CurrentCultureIgnoreCase))
            return PaginationSize.ExtraLarge;
        else if (input.Equals("Xs", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("ExtraSmall", StringComparison.CurrentCultureIgnoreCase))
            return PaginationSize.ExtraSmall;
        else if (input.Equals("Lg", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Large", StringComparison.CurrentCultureIgnoreCase))
            return PaginationSize.Large;
        else if (input.Equals("Sm", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Small", StringComparison.CurrentCultureIgnoreCase))
            return PaginationSize.Small;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised pagination size '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return PaginationSize.Normal;
        }
    }
}

/// <summary>
/// A list of the possible pagination sizes
/// </summary>
public enum PaginationSize
{
    Normal = 0,
    ExtraLarge,
    ExtraSmall,
    Large,
    Small
}