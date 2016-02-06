using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;
using Quic.Messaging;

/// <summary>
/// Represents a column in a grid row
/// </summary>
public class Column : BootstrapContainerElement
{
    int xsSpan, smSpan, mdSpan, lgSpan, xlSpan, span;

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "MdSpan" || propertyName == "LgSpan" || propertyName == "SmSpan" || propertyName == "XlSpan"
            || propertyName == "XsSpan" || propertyName == "Span")
        {
            return ColumnSpanProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<div");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</div>");
        }
    }

    /// <summary>
    /// Gets or sets the span of the column on a medium screen.
    /// Value must be between 1 and 12 (or 9% and 100%) inclusive.
    /// </summary>
    public int MdSpan
    {
        get { return mdSpan; }
        set
        {
            if (value < 1 || value > 12)
            {
                Messenger.Notify(
                    new QuicException("MdSpan value must be between 1 and 12, or between 9% and 100% (inclusive).",
                        this.Document.SourcePath, this.Line, this.Column));
            }

            mdSpan = value;
            //remove appropriate classes
            //classes to remove must start with "col-md-" and be followed by a digit
            //this is very important so that we dont remove classes like "col-md-offset-1"
            CssClasses = CssClasses.Where(c =>
            {
                if (c.StartsWith("col-md-") && c.Length > 7)
                {
                    if (char.IsDigit(c[7]))
                        return false;
                }

                return true;
            }).ToList();
            CssClasses.Add("col-md-" + mdSpan);
        }
    }
    /// <summary>
    /// Gets or sets the span of the column on a large screen.
    /// Value must be between 1 and 12 (or 9% and 100%) inclusive.
    /// </summary>
    public int LgSpan
    {
        get { return lgSpan; }
        set
        {
            if (value < 1 || value > 12)
            {
                Messenger.Notify(
                    new QuicException("LgSpan value must be between 1 and 12, or between 9% and 100% (inclusive).",
                        this.Document.SourcePath, this.Line, this.Column));
            }

            lgSpan = value;
            //remove appropriate classes
            //classes to remove must start with "col-lg-" and be followed by a digit
            //this is very important so that we dont remove classes like "col-lg-offset-1"
            CssClasses = CssClasses.Where(c =>
            {
                if (c.StartsWith("col-lg-") && c.Length > 7)
                {
                    if (char.IsDigit(c[7]))
                        return false;
                }

                return true;
            }).ToList();
            CssClasses.Add("col-lg-" + lgSpan);
        }
    }
    /// <summary>
    /// Gets or sets the span of the column on a small screen.
    /// Value must be between 1 and 12 (or 9% and 100%) inclusive.
    /// </summary>
    public int SmSpan
    {
        get { return smSpan; }
        set
        {
            if (value < 1 || value > 12)
            {
                Messenger.Notify(
                    new QuicException("SmSpan value must be between 1 and 12, or between 9% and 100% (inclusive).",
                        this.Document.SourcePath, this.Line, this.Column));
            }

            smSpan = value;
            //remove appropriate classes
            //classes to remove must start with "col-sm-" and be followed by a digit
            //this is very important so that we dont remove classes like "col-sm-offset-1"
            CssClasses = CssClasses.Where(c =>
            {
                if (c.StartsWith("col-sm-") && c.Length > 7)
                {
                    if (char.IsDigit(c[7]))
                        return false;
                }

                return true;
            }).ToList();
            CssClasses.Add("col-sm-" + smSpan);
        }
    }
    /// <summary>
    /// Gets or sets the span of the column on ay screen.
    /// Value must be between 1 and 12 (or 9% and 100%) inclusive.
    /// </summary>
    public int Span
    {
        get { return span; }
        set
        {
            if (value < 1 || value > 12)
            {
                Messenger.Notify(
                    new QuicException("Span value must be between 1 and 12, or between 9% and 100% (inclusive).",
                        this.Document.SourcePath, this.Line, this.Column));
            }

            span = mdSpan = lgSpan = smSpan = xlSpan = xsSpan = value;
            //remove appropriate classes
            //classes to remove must start with "col-*-" and be followed by a digit
            //where * is any of "md", "lg", "sm", "xs"
            //this is very important so that we dont remove classes like "col-*-offset-1"
            CssClasses = CssClasses.Where(c =>
            {
                if ((c.StartsWith("col-md-") || c.StartsWith("col-lg-") || c.StartsWith("col-sm-") || c.StartsWith("col-xs-"))
                    && c.Length > 7)
                {
                    if (char.IsDigit(c[7]))
                        return false;
                }

                return true;
            }).ToList();
            CssClasses.Add("col-md-" + span);
            CssClasses.Add("col-lg-" + span);
            CssClasses.Add("col-sm-" + span);
            CssClasses.Add("col-xs-" + span);
        }
    }
    /// <summary>
    /// Gets or sets the span of the column on an extra-large screen.
    /// Value must be between 1 and 12 (or 9% and 100%) inclusive.
    /// </summary>
    public int XlSpan
    {
        get { return xlSpan; }
        set
        {
            if (value < 1 || value > 12)
            {
                Messenger.Notify(
                    new QuicException("XlSpan value must be between 1 and 12, or between 9% and 100% (inclusive).",
                        this.Document.SourcePath, this.Line, this.Column));
            }

            xlSpan = value;
            //for now, this still uses the col-lg-* class
            //remove appropriate classes
            //classes to remove must start with "col-lg-" and be followed by a digit
            //this is very important so that we dont remove classes like "col-lg-offset-1"
            CssClasses = CssClasses.Where(c =>
            {
                if (c.StartsWith("col-lg-") && c.Length > 7)
                {
                    if (char.IsDigit(c[7]))
                        return false;
                }

                return true;
            }).ToList();
            CssClasses.Add("col-lg-" + xlSpan);
        }
    }
    /// <summary>
    /// Gets or sets the span of the column on an extra-small screen.
    /// Value must be between 1 and 12 (or 9% and 100%) inclusive.
    /// </summary>
    public int XsSpan
    {
        get { return xsSpan; }
        set
        {
            if (value < 1 || value > 12)
            {
                Messenger.Notify(
                    new QuicException("XsSpan value must be between 1 and 12, or between 9% and 100% (inclusive).",
                        this.Document.SourcePath, this.Line, this.Column));
            }

            xsSpan = value;
            //remove appropriate classes
            //classes to remove must start with "col-xs-" and be followed by a digit
            //this is very important so that we dont remove classes like "col-xs-offset-1"
            CssClasses = CssClasses.Where(c => 
            {
                if (c.StartsWith("col-xs-") && c.Length > 7) 
                {
                    if (char.IsDigit(c[7]))
                        return false;
                }

                return true;
            }).ToList();
            CssClasses.Add("col-xs-" + xsSpan);
        }
    }   
}