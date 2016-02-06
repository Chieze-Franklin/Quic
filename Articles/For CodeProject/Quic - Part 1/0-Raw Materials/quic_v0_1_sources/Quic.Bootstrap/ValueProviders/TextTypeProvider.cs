using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class TextTypeProvider : ValueProvider
{
    static TextTypeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static TextTypeProvider Singleton()
    {
        if (singleton == null)
            singleton = new TextTypeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is TextType)
        {
            return (TextType)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Text", StringComparison.CurrentCultureIgnoreCase))
            return TextType.Text;
        else if (input.Equals("Password", StringComparison.CurrentCultureIgnoreCase))
            return TextType.Password;
        else if (input.Equals("Email", StringComparison.CurrentCultureIgnoreCase))
            return TextType.Email;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised text type '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return TextType.Text;
        }
    }
}

/// <summary>
/// A list of the possible text types
/// </summary>
public enum TextType
{
    Text = 0,
    Email,
    Password,
}