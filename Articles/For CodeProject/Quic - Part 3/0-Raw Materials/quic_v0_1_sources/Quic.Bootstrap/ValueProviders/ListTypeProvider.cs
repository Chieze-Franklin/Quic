using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class ListTypeProvider : ValueProvider
{
    static ListTypeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ListTypeProvider Singleton()
    {
        if (singleton == null)
            singleton = new ListTypeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is ListType)
        {
            return (ListType)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("UnOrdered", StringComparison.CurrentCultureIgnoreCase))
            return ListType.UnOrdered;
        else if (input.Equals("Ordered", StringComparison.CurrentCultureIgnoreCase))
            return ListType.Ordered;
        //else if (input.Equals("Drop", StringComparison.CurrentCultureIgnoreCase) ||
        //    input.Equals("DropDown", StringComparison.CurrentCultureIgnoreCase) ||
        //    input.Equals("DropUp", StringComparison.CurrentCultureIgnoreCase))
        //    return ListType.Drop;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised list type '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return ListType.UnOrdered;
        }
    }
}

/// <summary>
/// A list of the possible list types
/// </summary>
public enum ListType
{
    UnOrdered = 0,
    Ordered,
}