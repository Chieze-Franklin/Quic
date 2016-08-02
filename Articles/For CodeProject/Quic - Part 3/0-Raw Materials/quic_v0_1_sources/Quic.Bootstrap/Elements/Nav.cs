using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a navigation control
/// </summary>
public class Nav : List
{
    NavType navType;

    public Nav()
    {
        CssClasses.Add("nav");
        Type = NavType.Tabs;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Type")
        {
            return NavTypeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<ul");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            outputFile.WriteLine(">");
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</ul>");
        }
    }

    /// <summary>
    /// Gets or sets the nav type
    /// </summary>
    public new NavType Type
    {
        get { return navType; }
        set
        {
            if (value == NavType.Tabs)
            {
                if (!CssClasses.Contains("nav-tabs"))
                    CssClasses.Add("nav-tabs");
            }
            else
            {
                if (CssClasses.Contains("nav-tabs"))
                    CssClasses.Remove("nav-tabs");
            }
            if (value == NavType.Pills)
            {
                if (!CssClasses.Contains("nav-pills"))
                    CssClasses.Add("nav-pills");
            }
            else
            {
                if (CssClasses.Contains("nav-pills"))
                    CssClasses.Remove("nav-pills");
            }
            if (value == NavType.JustifiedTabs)
            {
                if (!CssClasses.Contains("nav-tabs nav-justified"))
                    CssClasses.Add("nav-tabs nav-justified");
            }
            else
            {
                if (CssClasses.Contains("nav-tabs nav-justified"))
                    CssClasses.Remove("nav-tabs nav-justified");
            }
            if (value == NavType.JustifiedPills)
            {
                if (!CssClasses.Contains("nav-pills nav-justified"))
                    CssClasses.Add("nav-pills nav-justified");
            }
            else
            {
                if (CssClasses.Contains("nav-pills nav-justified"))
                    CssClasses.Remove("nav-pills nav-justified");
            }
            if (value == NavType.VerticalPills)
            {
                if (!CssClasses.Contains("nav-pills nav-stacked"))
                    CssClasses.Add("nav-pills nav-stacked");
            }
            else
            {
                if (CssClasses.Contains("nav-pills nav-stacked"))
                    CssClasses.Remove("nav-pills nav-stacked");
            }

            navType = value;
        }
    }
}