using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class ThemeProvider : ValueProvider
{
    static ThemeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ThemeProvider Singleton()
    {
        if (singleton == null)
            singleton = new ThemeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is Theme)
        {
            return (Theme)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Dark", StringComparison.CurrentCultureIgnoreCase))
            return Theme.Dark;
        else if (input.Equals("Light", StringComparison.CurrentCultureIgnoreCase))
            return Theme.Light;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised theme '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return Theme.Light;
        }
    }
}

/// <summary>
/// A list of possible theme
/// </summary>
public enum Theme 
{
    Light = 0,
    Dark
}