using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Base class for input elements
/// </summary>
public abstract class InputElement : BootstrapElement
{
    bool isNavBtn, isNavLink;
    InputSize inputSz;

    public InputElement()
    {
        IsFormControl = false;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "InputSize")
        {
            return InputSizeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    /// <summary>
    /// Helper method to emit prefix add-on
    /// </summary>
    protected string EmitPrefixAddon()
    {
        string txt = "";

        if (!string.IsNullOrWhiteSpace(Prepend))
        {
            IsFormControl = true;
            txt = string.Format("<span class=\"input-group-addon\">{0}</span>", Prepend);
        }

        return txt;
    }
    /// <summary>
    /// Helper method to emit suffix add-on
    /// </summary>
    protected string EmitSuffixAddon()
    {
        string txt = "";

        if (!string.IsNullOrWhiteSpace(Append))
        {
            IsFormControl = true;
            txt = string.Format("<span class=\"input-group-addon\">{0}</span>", Append);
        }

        return txt;
    }

    /// <summary>
    /// Gets or sets the prefix add-on
    /// </summary>
    public string Prepend { get; set; }
    /// <summary>
    /// Gets or sets the suffix add-on
    /// </summary>
    public string Append { get; set; }
    /// <summary>
    /// Gets a value to determine if the element has an add-on defined directly on it
    /// </summary>
    [NotQuicProperty]
    public bool HasAddon
    {
        get { return !string.IsNullOrWhiteSpace(Append) || !string.IsNullOrWhiteSpace(Prepend); }
    }
    /// <summary>
    /// Gets or sets a value to determine if the element should act as a nav bar button.
    /// </summary>
    public bool IsNavBarButton
    {
        get { return isNavBtn; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("navbar-btn"))
                    CssClasses.Add("navbar-btn");
            }
            else
            {
                if (CssClasses.Contains("navbar-btn"))
                    CssClasses.Remove("navbar-btn");
            }

            isNavBtn = value;
        }
    }
    /// <summary>
    /// Gets or sets a value to determine if the element should act as a nav bar link.
    /// </summary>
    public bool IsNavBarLink
    {
        get { return isNavLink; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("navbar-link"))
                    CssClasses.Add("navbar-link");
            }
            else
            {
                if (CssClasses.Contains("navbar-link"))
                    CssClasses.Remove("navbar-link");
            }

            isNavLink = value;
        }
    }

    /// <summary>
    /// Gets or sets the input size
    /// </summary>
    public virtual InputSize InputSize
    {
        get { return inputSz; }
        set
        {
            if (value == InputSize.ExtraLarge || value == InputSize.Large)
            {
                if (!CssClasses.Contains("input-lg"))
                    CssClasses.Add("input-lg");
            }
            else
            {
                if (CssClasses.Contains("input-lg"))
                    CssClasses.Remove("input-lg");
            }

            if (value == InputSize.ExtraSmall || value == InputSize.Small)
            {
                if (!CssClasses.Contains("input-sm"))
                    CssClasses.Add("input-sm");
            }
            else
            {
                if (CssClasses.Contains("input-sm"))
                    CssClasses.Remove("input-sm");
            }

            inputSz = value;
        }
    }
}