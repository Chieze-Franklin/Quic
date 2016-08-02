using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a label
/// </summary>
public class Label : InputElement
{
    LabelType lblType;

    public Label()
    {
        CssClasses.Add("label");
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "LabelType")
        {
            return LabelTypeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            ////prefix
            //if (HasAddon)
            //{
            //    outputFile.WriteLine("<div class=\"input-group\">");
            //    outputFile.WriteLine(EmitPrefixAddon());
            //}

            //opening tag
            outputFile.Write("<span");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //text, closing tag
            outputFile.Write(string.Format(">{0}</span>", EmitText()));

            ////suffix
            //if (HasAddon)
            //{
            //    outputFile.WriteLine();
            //    outputFile.WriteLine(EmitSuffixAddon());
            //    outputFile.WriteLine("</div>");
            //}
        }
    }

    /// <summary>
    /// Gets or sets the label type
    /// </summary>
    public LabelType LabelType
    {
        get { return lblType; }
        set
        {
            if (value == LabelType.Default)
            {
                if (!CssClasses.Contains("label-default"))
                    CssClasses.Add("label-default");
            }
            else
            {
                if (CssClasses.Contains("label-default"))
                    CssClasses.Remove("label-default");
            }

            if (value == LabelType.Danger)
            {
                if (!CssClasses.Contains("label-danger"))
                    CssClasses.Add("label-danger");
            }
            else
            {
                if (CssClasses.Contains("label-danger"))
                    CssClasses.Remove("label-danger");
            }

            if (value == LabelType.Info)
            {
                if (!CssClasses.Contains("label-info"))
                    CssClasses.Add("label-info");
            }
            else
            {
                if (CssClasses.Contains("label-info"))
                    CssClasses.Remove("label-info");
            }

            if (value == LabelType.Primary)
            {
                if (!CssClasses.Contains("label-primary"))
                    CssClasses.Add("label-primary");
            }
            else
            {
                if (CssClasses.Contains("label-primary"))
                    CssClasses.Remove("label-primary");
            }

            if (value == LabelType.Success)
            {
                if (!CssClasses.Contains("label-success"))
                    CssClasses.Add("label-success");
            }
            else
            {
                if (CssClasses.Contains("label-success"))
                    CssClasses.Remove("label-success");
            }

            if (value == LabelType.Warning)
            {
                if (!CssClasses.Contains("label-warning"))
                    CssClasses.Add("label-warning");
            }
            else
            {
                if (CssClasses.Contains("label-warning"))
                    CssClasses.Remove("label-warning");
            }

            lblType = value;
        }
    }
}