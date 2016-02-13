using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class TabControl : BootstrapContainerElement
{
    NavType navType;

    public TabControl()
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

            //open <Nav>
            outputFile.Write("<ul");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            outputFile.WriteLine(">");

            //create <NavItem>s
            foreach (var element in Elements)
            {
                if (element is TabPage) 
                {
                    outputFile.Write("<li><a");
                    var dataToggle = "tab";
                    if (Type == NavType.JustifiedPills || Type == NavType.Pills || Type == NavType.VerticalPills)
                    {
                        dataToggle = "pill";
                    }
                    outputFile.Write(string.Format("{0} data-toggle=\"{1}\">",
                        (element.Name != null ? " href=\"#" + element.Name + "\"" : ""), dataToggle));
                    if (((TabPage)element).Text != null)
                        outputFile.Write(((TabPage)element).Text);
                    outputFile.WriteLine("</a></li>");
                }
            }

            //close <Nav>
            outputFile.WriteLine("</ul>");

            //open <TabPanes>
            outputFile.WriteLine("<div class=\"tab-content\">");

            //pages
            foreach (var element in Elements)
            {
                //if (element is TabPage)
                //{
                //    string inStr = "";
                //    if (this.Elements.IndexOf(element) == 0) //if this is its first child
                //        inStr = "in";

                //    outputFile.WriteLine(string.Format("<div class=\"tab-pane fade {0}\">", inStr));
                //    element.BeginRender();
                //    outputFile.WriteLine("</div>");
                //}
                //else
                    element.BeginRender();
            }

            //close <TabPanes>
            outputFile.WriteLine("</div>");
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