using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class Button : InputElement
{
    bool isBlock;
    ButtonType btnType;
    ButtonSize btnSz;

    public Button()
    {
        CssClasses.Add("btn");
        CssClasses.Add("btn-default");
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "ButtonSize")
        {
            //ButtonSizeProvider bszVp = new ButtonSizeProvider();
            //bszVp.PropertyName = propertyName;
            //bszVp.Element = this;
            //return bszVp;
            return ButtonSizeProvider.Singleton();
        }
        else if (propertyName == "ButtonType")
        {
            return ButtonTypeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //prefix
            if (HasAddon)
            {
                outputFile.WriteLine("<div class=\"input-group\">");
                outputFile.WriteLine(EmitPrefixAddon());
            }

            //opening tag
            outputFile.Write("<button");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //type
            if (IsSubmit)
                outputFile.Write(" type=\"submit\"");
            else
                outputFile.Write(" type=\"button\"");

            //custom properties
            outputFile.Write(EmitCustomProperties());

            if (Disabled)
                outputFile.Write(" disabled");//disabled

            //text, closing tag
            outputFile.Write(string.Format(">{0}</button>", ButtonType == ButtonType.Close ? "&times;" : EmitText()));

            //suffix
            if (HasAddon)
            {
                outputFile.WriteLine();
                outputFile.WriteLine(EmitSuffixAddon());
                outputFile.WriteLine("</div>");
            }
        }
    }

    /// <summary>
    /// Gets or sets a value to determine if the button should act as a block element.
    /// That is, if it should span the full width of its parent.
    /// </summary>
    public bool IsBlock
    {
        get { return isBlock; }
        set
        {
            if (value)
            {
                if (!CssClasses.Contains("btn-block"))
                    CssClasses.Add("btn-block");
            }
            else
            {
                if (CssClasses.Contains("btn-block"))
                    CssClasses.Remove("btn-block");
            }

            isBlock = value;
        }
    }
    /// <summary>
    /// Gets or sets a value to determine if the button should act as a form submit button.
    /// </summary>
    public bool IsSubmit { get; set; }

    /// <summary>
    /// Gets or sets the button type
    /// </summary>
    public ButtonType ButtonType
    {
        get { return btnType; }
        set
        {
            if (value == ButtonType.Default)
            {
                if (!CssClasses.Contains("btn-default"))
                    CssClasses.Add("btn-default");
            }
            else
            {
                if (CssClasses.Contains("btn-default"))
                    CssClasses.Remove("btn-default");
            }

            if (value == ButtonType.Close)
            {
                if (!CssClasses.Contains("close"))
                    CssClasses.Add("close");
            }
            else
            {
                if (CssClasses.Contains("close"))
                    CssClasses.Remove("close");
            }

            if (value == ButtonType.Danger)
            {
                if (!CssClasses.Contains("btn-danger"))
                    CssClasses.Add("btn-danger");
            }
            else
            {
                if (CssClasses.Contains("btn-danger"))
                    CssClasses.Remove("btn-danger");
            }

            if (value == ButtonType.Info)
            {
                if (!CssClasses.Contains("btn-info"))
                    CssClasses.Add("btn-info");
            }
            else
            {
                if (CssClasses.Contains("btn-info"))
                    CssClasses.Remove("btn-info");
            }

            if (value == ButtonType.Link)
            {
                if (!CssClasses.Contains("btn-link"))
                    CssClasses.Add("btn-link");
            }
            else
            {
                if (CssClasses.Contains("btn-link"))
                    CssClasses.Remove("btn-link");
            }

            if (value == ButtonType.Primary)
            {
                if (!CssClasses.Contains("btn-primary"))
                    CssClasses.Add("btn-primary");
            }
            else
            {
                if (CssClasses.Contains("btn-primary"))
                    CssClasses.Remove("btn-primary");
            }

            if (value == ButtonType.Success)
            {
                if (!CssClasses.Contains("btn-success"))
                    CssClasses.Add("btn-success");
            }
            else
            {
                if (CssClasses.Contains("btn-success"))
                    CssClasses.Remove("btn-success");
            }

            if (value == ButtonType.Warning)
            {
                if (!CssClasses.Contains("btn-warning"))
                    CssClasses.Add("btn-warning");
            }
            else
            {
                if (CssClasses.Contains("btn-warning"))
                    CssClasses.Remove("btn-warning");
            }

            btnType = value;
        }
    }

    /// <summary>
    /// Gets or sets the button size
    /// </summary>
    public ButtonSize ButtonSize
    {
        get { return btnSz; }
        set
        {
            if (value == ButtonSize.ExtraLarge || value == ButtonSize.Large)
            {
                if (!CssClasses.Contains("btn-lg"))
                    CssClasses.Add("btn-lg");
            }
            else
            {
                if (CssClasses.Contains("btn-lg"))
                    CssClasses.Remove("btn-lg");
            }

            if (value == ButtonSize.ExtraSmall)
            {
                if (!CssClasses.Contains("btn-xs"))
                    CssClasses.Add("btn-xs");
            }
            else
            {
                if (CssClasses.Contains("btn-xs"))
                    CssClasses.Remove("btn-xs");
            }

            if (value == ButtonSize.Small)
            {
                if (!CssClasses.Contains("btn-sm"))
                    CssClasses.Add("btn-sm");
            }
            else
            {
                if (CssClasses.Contains("btn-sm"))
                    CssClasses.Remove("btn-sm");
            }

            btnSz = value;
        }
    }
}