using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class LabelTypeProvider : ValueProvider
{
    static LabelTypeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static LabelTypeProvider Singleton()
    {
        if (singleton == null)
            singleton = new LabelTypeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is LabelType)
        {
            return (LabelType)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Default", StringComparison.CurrentCultureIgnoreCase))
            return LabelType.Default;
        else if (input.Equals("Danger", StringComparison.CurrentCultureIgnoreCase))
            return LabelType.Danger;
        else if (input.Equals("Info", StringComparison.CurrentCultureIgnoreCase))
            return LabelType.Info;
        else if (input.Equals("Primary", StringComparison.CurrentCultureIgnoreCase))
            return LabelType.Primary;
        else if (input.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
            return LabelType.Success;
        else if (input.Equals("Warning", StringComparison.CurrentCultureIgnoreCase))
            return LabelType.Warning;
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
/// A list of the possible label types
/// </summary>
public enum LabelType
{
    Default = 0,
    Danger,
    Info,
    Primary,
    Success,
    Warning
}