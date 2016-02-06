using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

public class NavTypeProvider : ValueProvider
{
    static NavTypeProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static NavTypeProvider Singleton()
    {
        if (singleton == null)
            singleton = new NavTypeProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is NavType)
        {
            return (NavType)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        if (input.Equals("Tabs", StringComparison.CurrentCultureIgnoreCase))
            return NavType.Tabs;
        else if (input.Equals("Pills", StringComparison.CurrentCultureIgnoreCase))
            return NavType.Pills;
        else if (input.Equals("JustifiedTabs", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Just-Tabs", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Tabs-Just", StringComparison.CurrentCultureIgnoreCase))
            return NavType.JustifiedTabs;
        else if (input.Equals("JustifiedPills", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Just-Pills", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Pills-Just", StringComparison.CurrentCultureIgnoreCase))
            return NavType.JustifiedPills;
        else if (input.Equals("VerticalPills", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Vert-Pills", StringComparison.CurrentCultureIgnoreCase) ||
            input.Equals("Pills-Vert", StringComparison.CurrentCultureIgnoreCase))
            return NavType.VerticalPills;
        else
        {
            Messenger.Notify(
                       new QuicException(string.Format("Unrecognised nav type '{0}'", input),
                           this.Element.Document.SourcePath, this.Element.Line, this.Element.Column));
            return NavType.Tabs;
        }
    }
}

/// <summary>
/// A list of the possible nav types
/// </summary>
public enum NavType
{
    Tabs = 0,
    Pills,
    JustifiedTabs,
    JustifiedPills,
    VerticalPills
}