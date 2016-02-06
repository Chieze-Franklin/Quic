using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class InputSizeProvider : ValueProvider
{
    static InputSizeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static InputSizeProvider Singleton()
    {
        if (singleton == null)
            singleton = new InputSizeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is InputSize)
        {
            return (InputSize)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase))
            return InputSize.Normal;
        else if (input.Equals("Xl", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("ExtraLarge", StringComparison.CurrentCultureIgnoreCase))
            return InputSize.ExtraLarge;
        else if (input.Equals("Xs", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("ExtraSmall", StringComparison.CurrentCultureIgnoreCase))
            return InputSize.ExtraSmall;
        else if (input.Equals("Lg", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Large", StringComparison.CurrentCultureIgnoreCase))
            return InputSize.Large;
        else if (input.Equals("Sm", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Small", StringComparison.CurrentCultureIgnoreCase))
            return InputSize.Small;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised input size '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return InputSize.Normal;
        }
    }
}

public enum InputSize
{
    Normal = 0,
    ExtraLarge,
    ExtraSmall,
    Large,
    Small
}