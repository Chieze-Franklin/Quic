using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Base class for table elements like table row, table data, table head data
/// </summary>
public abstract class TableElement : BootstrapContainerElement
{
    TableElementContext context;

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Context")
        {
            return ContextProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    /// <summary>
    /// Gets or sets the context
    /// </summary>
    public TableElementContext Context
    {
        get { return context; }
        set
        {
            if (value == TableElementContext.Active)
            {
                if (!CssClasses.Contains("active"))
                    CssClasses.Add("active");
            }
            else
            {
                if (CssClasses.Contains("active"))
                    CssClasses.Remove("active");
            }

            if (value == TableElementContext.Danger)
            {
                if (!CssClasses.Contains("danger"))
                    CssClasses.Add("danger");
            }
            else
            {
                if (CssClasses.Contains("danger"))
                    CssClasses.Remove("danger");
            }

            if (value == TableElementContext.Success)
            {
                if (!CssClasses.Contains("success"))
                    CssClasses.Add("success");
            }
            else
            {
                if (CssClasses.Contains("success"))
                    CssClasses.Remove("success");
            }

            if (value == TableElementContext.Warning)
            {
                if (!CssClasses.Contains("warning"))
                    CssClasses.Add("warning");
            }
            else
            {
                if (CssClasses.Contains("warning"))
                    CssClasses.Remove("warning");
            }

            context = value;
        }
    }

    public class ContextProvider : ValueProvider
    {
        static ContextProvider singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static ContextProvider Singleton()
        {
            if (singleton == null)
                singleton = new ContextProvider();

            return singleton;
        }

        public override object Evaluate(object input)
        {
            if (input is TableElementContext)
            {
                return (TableElementContext)input;
            }

            return Evaluate(input.ToString());
        }

        public override object Evaluate(string input)
        {
            if (input.Equals("Normal", StringComparison.CurrentCultureIgnoreCase))
                return TableElementContext.Normal;
            else if (input.Equals("Active", StringComparison.CurrentCultureIgnoreCase))
                return TableElementContext.Active;
            else if (input.Equals("Danger", StringComparison.CurrentCultureIgnoreCase))
                return TableElementContext.Danger;
            else if (input.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                return TableElementContext.Success;
            else if (input.Equals("Warning", StringComparison.CurrentCultureIgnoreCase))
                return TableElementContext.Warning;
            else
                //throw new QuicException(string.Format("Unrecognised context style '{0}'", input), this.Element.Document.SourcePath);
                throw new ArgumentException(string.Format("Unrecognised context style '{0}'", input));
        }
    }
}

/// <summary>
/// A list of the possible table element context styles
/// </summary>
public enum TableElementContext
{
    Normal = 0,
    Active,
    Danger,
    Success,
    Warning
}