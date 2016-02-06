using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class NavBarPositionProvider : ValueProvider
{
    static NavBarPositionProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static NavBarPositionProvider Singleton()
    {
        if (singleton == null)
            singleton = new NavBarPositionProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is NavBarPosition)
        {
            return (NavBarPosition)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Top", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Static-Top", StringComparison.CurrentCultureIgnoreCase))
            return NavBarPosition.Top;
        else if (input.Equals("FixedBottom", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Fixed-Bottom", StringComparison.CurrentCultureIgnoreCase))
            return NavBarPosition.FixedBottom;
        else if (input.Equals("FixedTop", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Fixed-Top", StringComparison.CurrentCultureIgnoreCase))
            return NavBarPosition.FixedTop;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised navbar position '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return NavBarPosition.Top;
        }
    }
}

/// <summary>
/// A list of the possible navbar positions
/// </summary>
public enum NavBarPosition
{
    Top = 0,
    FixedTop,
    FixedBottom
}