using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class ValidationStateProvider : ValueProvider
{
    static ValidationStateProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ValidationStateProvider Singleton()
    {
        if (singleton == null)
            singleton = new ValidationStateProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is ValidationState)
        {
            return (ValidationState)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("None", StringComparison.CurrentCultureIgnoreCase))
            return ValidationState.None;
        else if (input.Equals("Error", StringComparison.CurrentCultureIgnoreCase) ||
        input.Equals("HasError", StringComparison.CurrentCultureIgnoreCase) ||
        input.Equals("has-error", StringComparison.CurrentCultureIgnoreCase))
            return ValidationState.HasError;
        else if (input.Equals("Success", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("HasSuccess", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("has-success", StringComparison.CurrentCultureIgnoreCase))
            return ValidationState.HasSuccess;
        else if (input.Equals("Warning", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("HasWarning", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("has-warning", StringComparison.CurrentCultureIgnoreCase))
            return ValidationState.HasWarning;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised validation state '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return ValidationState.None;
        }
    }
}

/// <summary>
/// A list of the possible validation states
/// </summary>
public enum ValidationState
{
    None = 0,
    HasError,
    HasSuccess,
    HasWarning
}