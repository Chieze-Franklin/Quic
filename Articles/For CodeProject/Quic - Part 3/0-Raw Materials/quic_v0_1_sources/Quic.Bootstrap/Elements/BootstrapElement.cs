using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

/// <summary>
/// The base class for all bootstrap elements
/// </summary>
public abstract class BootstrapElement : UIElement
{
    Alignment align;
    State state;
    ValidationState validation;
    bool isFormControl, disabled;
    string glyph;
    string[] supportedGlyphs = 
        {
            "adjust", "align-center", "align-justify", "align-left", "align-right", "arrow-down", "arrow-left", "arrow-right", "arrow-up",
            "asterisk", 
            "backward", "ban-circle", "barcode", "bell", "bold", "book", "bookmark", "briefcase", "bullhorn", 
            "calender", "camera", "certificate", "check", "chevron-down", "chevron-left", "chevron-right", "chevron-up",
            "circle-arrow-down", "circle-arrow-left", "circle-arrow-right", "circle-arrow-up", "cloud", "cloud-download", "cloud-upload",
            "cog", "collapse-down", "collapse-up", "comment", "compressed", "copyright-mark", "credit-card", "cutlery", 
            "dashboard", "download", "download-alt", 
            "earphone", "edit", "eject", "envelope", "euro", "exclamation-sign", "expand", "export", "eye-close", "eye-open",
            "facetime-video", "fast-backward", "fast-forward", "file", "film", "filter", "fire", "flag", "flash", "floppy-disk",
            "floppy-open", "floppy-remove", "floppy-save", "floppy-saved", "folder-close", "folder-open", "font", "forward", "fullscreen",
            "gbp", "gift", "glass", "globe", 
            "hand-down", "hand-left", "hand-right", "hand-up", "hd-video", "hdd", "header", "headphones", "heart", "heart-empty", "home",
            "import", "inbox", "indent-left", "indent-right", "info-sign", "italic",
            "leaf", "link", "list", "list-alt", "lock", "log-in", "log-out", 
            "magnet", "map-marker", "minus", "minus-sign", "move", "music",
            "new-window",
            "off", "ok", "ok-circle", "ok-sign", "open", 
            "paperclip", "pause", "pencil", "phone", "phone-alt", "picture", "plane", "play", "play-circle", "plus", "plus-sign", "print",
            "pushpin",
            "qrcode", "question-sign", "random", "record", "refresh", "registration-mark", "remove", "remove-circle", "remove-sign", "repeat",
            "resize-full", "resize-horizontal", "resize-small", "resize-vertical", "retweet", "road",
            "save", "saved", "screenshot", "sd-video", "search", "send", "share", "share-alt", "shopping-cart", "signal", "sort",
            "sort-by-alphabet", "sort-by-alphabet-alt", "sort-by-attributes", "sort-by-attributes-alt",
            "sort-by-order", "sort-by-order-alt", "sound-5-1", "sound-6-1", "sound-7-1", "sound-dolby", "sound-stereo", "star", "star-empty",
            "stats", "step-backward", "step-forward", "stop", "subtitles", 
            "tag", "tags", "tasks", "text-height", "text-width", "th", "th-large", "th-list", "thumbs-down", "thumbs-up", "time", "tint",
            "tower", "transfer", "trash", "tree-conifer", "tree-deciduous", 
            "unchecked", "upload", "usd", "user", 
            "volume-down", "volume-off" , "volume-up", 
            "warning-sign", "wrench", 
            "zoom-in", "zoom-out"
        };

    public BootstrapElement()
    {
        CssClasses = new List<string>();
        //IsFormControl = true;
    }
    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Align")
        {
            return AlignmentProvider.Singleton();
        }
        else if (propertyName == "State")
        {
            return StateProvider.Singleton();
        }
        else if (propertyName == "Validation")
        {
            return ValidationStateProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    /// <summary>
    /// Helper method to emit classes
    /// </summary>
    protected string EmitClasses()
    {
        StringBuilder sb = new StringBuilder("");

        if (CssClasses.Count > 0)
        {
            sb.Append(" class=\"");
            foreach (var clss in CssClasses)
            {
                sb.Append(" " + clss);
            }
            sb.Append("\"");
        }

        return sb.ToString();
    }
    /// <summary>
    /// Helper method to emit id
    /// </summary>
    protected string EmitID()
    {
        string id = "";

        if (!string.IsNullOrWhiteSpace(Name))
        {
            id = string.Format(" id=\"{0}\"", Name);
        }

        return id;
    }
    /// <summary>
    /// Helper method to emit placeholder
    /// </summary>
    protected string EmitPlaceholder()
    {
        string placeholder = "";

        if (Hint != null)
        {
            placeholder = string.Format(" placeholder=\"{0}\"", Hint);
        }

        return placeholder;
    }
    /// <summary>
    /// Helper method to emit text
    /// </summary>
    protected string EmitText()
    {
        string txt = Text != null ? Text : (Content != null ? Content : "");

        if (!string.IsNullOrWhiteSpace(GlyphIcon))
        {
            txt = string.Format("<span class=\"glyphicon glyphicon-{0}\">{1}</span>", GlyphIcon, txt);
        }

        return txt;
    }

    /// <summary>
    /// Gets the CSS classes
    /// </summary>
    [NotQuicProperty]
    protected List<string> CssClasses { get; set; }
    /// <summary>
    /// Gets or sets a value to determine if the element is disabled
    /// </summary>
    public bool Disabled
    {
        get { return disabled; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("disabled"))
                    CssClasses.Add("disabled");
            }
            else
            {
                if (CssClasses.Contains("disabled"))
                    CssClasses.Remove("disabled");
            }

            disabled = value;
        }
    }
    /// <summary>
    /// Gets or sets the glyph icon 
    /// (http://www.tutorialspoint.com/bootstrap/bootstrap_glyph_icons.htm)
    /// </summary>
    public string GlyphIcon
    {
        get { return glyph; }
        set
        {
            value = value.ToLower();
            if (!supportedGlyphs.Contains(value))
            {
                Messenger.Notify( 
                    new QuicException(string.Format("Unsupported glyph icon '{0}'", value), this.Document.SourcePath, this.Line, this.Column));
            }

            glyph = value;
        }
    }
    /// <summary>
    /// Gets or sets a value to determine if the element is a form control.
    /// This property affects how an element is treated.
    /// For instance, form control elements may occupy the full available with, even in a horizontal layout,
    /// thereby making them arranged vertically.
    /// </summary>
    public bool IsFormControl
    {
        get { return isFormControl; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("form-control"))
                    CssClasses.Add("form-control");
            }
            else
            {
                if (CssClasses.Contains("form-control"))
                    CssClasses.Remove("form-control");
            }

            isFormControl = value;
        }
    }
    /// <summary>
    /// Gets or sets the hint text for this element.
    /// </summary>
    public string Hint { get; set; }

    /// <summary>
    /// Gets or sets the alignment
    /// </summary>
    public Alignment Align
    {
        get { return align; }
        set
        {
            if (value == Alignment.Right)
            {
                if (!CssClasses.Contains("pull-right"))
                    CssClasses.Add("pull-right");
            }
            else
            {
                if (CssClasses.Contains("pull-right"))
                    CssClasses.Remove("pull-right");
            }

            if (value == Alignment.Left)
            {
                if (!CssClasses.Contains("pull-left"))
                    CssClasses.Add("pull-left");
            }
            else
            {
                if (CssClasses.Contains("pull-left"))
                    CssClasses.Remove("pull-left");
            }

            align = value;
        }
    }

    /// <summary>
    /// Gets or sets the element state
    /// </summary>
    public State State
    {
        get { return state; }
        set
        {
            if (value == State.Active)
            {
                if (!CssClasses.Contains("active"))
                    CssClasses.Add("active");
            }
            else
            {
                if (CssClasses.Contains("active"))
                    CssClasses.Remove("active");
            }

            if (value == State.Disabled)
            {
                if (!CssClasses.Contains("disabled"))
                    CssClasses.Add("disabled");
            }
            else
            {
                if (CssClasses.Contains("disabled"))
                    CssClasses.Remove("disabled");
            }

            state = value;
        }
    }

    /// <summary>
    /// Gets or sets the validation state
    /// </summary>
    public ValidationState Validation
    {
        get { return validation; }
        set
        {
            if (value == ValidationState.HasError)
            {
                if (!CssClasses.Contains("has-error"))
                    CssClasses.Add("has-error");
            }
            else
            {
                if (CssClasses.Contains("has-error"))
                    CssClasses.Remove("has-error");
            }

            if (value == ValidationState.HasSuccess)
            {
                if (!CssClasses.Contains("has-success"))
                    CssClasses.Add("has-success");
            }
            else
            {
                if (CssClasses.Contains("has-success"))
                    CssClasses.Remove("has-success");
            }

            if (value == ValidationState.HasWarning)
            {
                if (!CssClasses.Contains("has-warning"))
                    CssClasses.Add("has-warning");
            }
            else
            {
                if (CssClasses.Contains("has-warning"))
                    CssClasses.Remove("has-warning");
            }

            validation = value;
        }
    }
}