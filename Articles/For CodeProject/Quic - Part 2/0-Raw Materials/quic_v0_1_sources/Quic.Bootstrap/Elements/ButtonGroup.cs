using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a group of buttons
/// </summary>
public class ButtonGroup : OrientationContainer
{
    ButtonSize btnSz;

    public ButtonGroup()
    {
        Orientation = Orientation.Horizontal;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "ButtonSize")
        {
            return ButtonSizeProvider.Singleton();
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
            if (Orientation == Orientation.Vertical)
                CssClasses.Add("btn-group-vertical");
            else
                CssClasses.Add("btn-group");
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                if (element is DropButton)
                {
                    //automatically wrap it in a button group
                    outputFile.Write("<div");
                    outputFile.Write(EmitClasses());
                    outputFile.WriteLine(">");
                }
                element.BeginRender();
                if (element is DropButton)
                {
                    //automatically wrap it in a button group
                    outputFile.WriteLine("</div>");
                }
            }

            //closing tag
            outputFile.WriteLine("</div>");
        }
    }

    /// <summary>
    /// Gets or sets the button size (to be applied to each button in the group)
    /// </summary>
    public ButtonSize ButtonSize
    {
        get { return btnSz; }
        set
        {
            if (value == ButtonSize.ExtraLarge || value == ButtonSize.Large)
            {
                if (!CssClasses.Contains("btn-group-lg"))
                    CssClasses.Add("btn-group-lg");
            }
            else
            {
                if (CssClasses.Contains("btn-group-lg"))
                    CssClasses.Remove("btn-group-lg");
            }

            if (value == ButtonSize.ExtraSmall)
            {
                if (!CssClasses.Contains("btn-group-xs"))
                    CssClasses.Add("btn-group-xs");
            }
            else
            {
                if (CssClasses.Contains("btn-group-xs"))
                    CssClasses.Remove("btn-group-xs");
            }

            if (value == ButtonSize.Small)
            {
                if (!CssClasses.Contains("btn-group-sm"))
                    CssClasses.Add("btn-group-sm");
            }
            else
            {
                if (CssClasses.Contains("btn-group-sm"))
                    CssClasses.Remove("btn-group-sm");
            }

            btnSz = value;
        }
    }
}