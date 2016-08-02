using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class BuildTypeProvider : ValueProvider
{
    static BuildTypeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static BuildTypeProvider Singleton()
    {
        if (singleton == null)
            singleton = new BuildTypeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is BuildType)
        {
            return (BuildType)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Local", StringComparison.CurrentCultureIgnoreCase))
            return BuildType.Local;
        else if (input.Equals("Web", StringComparison.CurrentCultureIgnoreCase))
            return BuildType.Web;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised build type '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return BuildType.Local;
        }
    }
}

/// <summary>
/// A list of the possible ways a page can be built
/// </summary>
public enum BuildType
{
    /// <summary>
    /// relies on local resources
    /// </summary>
    Local = 0,
    /// <summary>
    /// relies on web resources
    /// </summary>
    Web
}