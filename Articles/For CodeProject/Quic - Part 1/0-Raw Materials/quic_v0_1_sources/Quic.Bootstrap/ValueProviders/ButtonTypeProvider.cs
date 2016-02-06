using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class ButtonTypeProvider : ValueProvider
{
    static ButtonTypeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ButtonTypeProvider Singleton()
    {
        if (singleton == null)
            singleton = new ButtonTypeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is ButtonType)
        {
            return (ButtonType)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Default", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Default;
        else if (input.Equals("Close", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Close;
        else if (input.Equals("Danger", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Danger;
        else if (input.Equals("Info", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Info;
        else if (input.Equals("Link", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Link;
        else if (input.Equals("Primary", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Primary;
        else if (input.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Success;
        else if (input.Equals("Warning", StringComparison.CurrentCultureIgnoreCase))
            return ButtonType.Warning;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised button type '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return ButtonType.Default;
        }
    }
}

/// <summary>
/// A list of the possible button types
/// </summary>
public enum ButtonType
{
    Default = 0,
    Close,
    Danger,
    Info,
    Link,
    Primary,
    Success,
    Warning
}