using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class StateProvider : ValueProvider
{
    static StateProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static StateProvider Singleton()
    {
        if (singleton == null)
            singleton = new StateProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is State)
        {
            return (State)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase))
            return State.Normal;
        else if (input.Equals("Active", StringComparison.CurrentCultureIgnoreCase))
            return State.Active;
        else if (input.Equals("Disabled", StringComparison.CurrentCultureIgnoreCase))
            return State.Disabled;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised element state '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return State.Normal;
        }
    }
}

/// <summary>
/// A list of the possible element states
/// </summary>
public enum State
{
    Normal = 0,
    Active,
    Disabled
}