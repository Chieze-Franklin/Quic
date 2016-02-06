using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class ButtonSizeProvider : ValueProvider
{
    static ButtonSizeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ButtonSizeProvider Singleton()
    {
        if (singleton == null)
            singleton = new ButtonSizeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is ButtonSize)
        {
            return (ButtonSize)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase))
            return ButtonSize.Normal;
        else if (input.Equals("Xl", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("ExtraLarge", StringComparison.CurrentCultureIgnoreCase))
            return ButtonSize.ExtraLarge;
        else if (input.Equals("Xs", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("ExtraSmall", StringComparison.CurrentCultureIgnoreCase))
            return ButtonSize.ExtraSmall;
        else if (input.Equals("Lg", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Large", StringComparison.CurrentCultureIgnoreCase))
            return ButtonSize.Large;
        else if (input.Equals("Sm", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Small", StringComparison.CurrentCultureIgnoreCase))
            return ButtonSize.Small;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised button size '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return ButtonSize.Normal;
        }
    }
}

/// <summary>
/// A list of the possible button sizes
/// </summary>
public enum ButtonSize
{
    Normal = 0,
    ExtraLarge,
    ExtraSmall,
    Large,
    Small
}