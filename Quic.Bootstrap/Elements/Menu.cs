using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a menu
/// </summary>
public class Menu : BootstrapContainerElement
{
    static int menuInstances = 1;
    NavBarPosition position;
    Theme theme;

    public Menu()
    {
        Collapsible = true;
        CssClasses.Add("navbar");
        Theme = Theme.Light;
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Position")
        {
            return NavBarPositionProvider.Singleton();
        }
        else if (propertyName == "Theme")
        {
            return ThemeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    bool inList = false;
    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<nav");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            outputFile.Write(" role=\"navigation\"");

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            string calcName = (Name != null ? Name : "menu" + menuInstances++);
            //menu text
            if (!string.IsNullOrEmpty(Text))
            {
                outputFile.WriteLine("<div class=\"navbar-header\">");
                if (Collapsible)
                {
                    outputFile.WriteLine(string.Format("<button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\"" +
                        " data-target=\"#{0}-navbar-collapse\">", calcName));
                    outputFile.WriteLine("<span class=\"sr-only\">Show/Hide</span>");
                    outputFile.WriteLine("<span class=\"icon-bar\"></span>");
                    outputFile.WriteLine("<span class=\"icon-bar\"></span>");
                    outputFile.WriteLine("<span class=\"icon-bar\"></span>");
                    outputFile.WriteLine("</button>");
                }
                outputFile.WriteLine(string.Format("<a class=\"navbar-brand\" href=\"#\">{0}</a>", EmitText()));
                outputFile.WriteLine("</div>");
            }

            string collapseText = "";
            if (Collapsible)
            {
                collapseText = string.Format(" class=\"collapse navbar-collapse\" id=\"{0}-navbar-collapse\"", calcName);
            }
            //child elements
            outputFile.WriteLine(string.Format("<div{0}>", collapseText));
            foreach (var element in Elements)
            {
                //if we encounter a menu item or a drop menu, and we are not in a list,
                if ((element is MenuItem || element is DropMenu) && inList == false) 
                {
                    outputFile.WriteLine("<ul class=\"nav navbar-nav\">");
                    inList = true;
                }
                //if we encounter a non-menu item or a non-drop menu, and we are in a list,
                else if (!(element is MenuItem) && !(element is DropMenu) && inList == true) 
                {
                    outputFile.WriteLine("</ul>");
                    inList = false;
                }

                if (element is LinkButton) 
                {
                    ((LinkButton)element).IsNavBarLink = true;
                }
                else if (element is Button)
                {
                    ((Button)element).IsNavBarButton = true;
                }
                else if (element is Text) 
                {
                    outputFile.Write("<span class=\"navbar-text\">");
                }

                element.BeginRender();

                //if the last element is a menu item or a drop menu, and we are in a list,
                if (Elements.IndexOf(element) == Elements.Count - 1 && (element is MenuItem || element is DropMenu) && inList == true)
                {
                    outputFile.WriteLine("</ul>");
                    inList = false; 
                }

                if (element is Text)
                {
                    outputFile.WriteLine("</span>");
                }
            }

            //closing tag
            outputFile.WriteLine("</div></nav>");
        }
    }

    /// <summary>
    /// Gets or sets a value to determine if the menu is collapsible
    /// </summary>
    public bool Collapsible { get; set; }

    /// <summary>
    /// Gets or sets the navbar position
    /// </summary>
    public NavBarPosition Position
    {
        get { return position; }
        set
        {
            if (value == NavBarPosition.FixedBottom)
            {
                if (!CssClasses.Contains("navbar-fixed-bottom"))
                    CssClasses.Add("navbar-fixed-bottom");
            }
            else
            {
                if (CssClasses.Contains("navbar-fixed-bottom"))
                    CssClasses.Remove("navbar-fixed-bottom");
            }
            if (value == NavBarPosition.FixedTop)
            {
                if (!CssClasses.Contains("navbar-fixed-top"))
                    CssClasses.Add("navbar-fixed-top");
            }
            else
            {
                if (CssClasses.Contains("navbar-fixed-top"))
                    CssClasses.Remove("navbar-fixed-top");
            }
            if (value == NavBarPosition.Top)
            {
                if (!CssClasses.Contains("navbar-static-top"))
                    CssClasses.Add("navbar-static-top");
            }
            else
            {
                if (CssClasses.Contains("navbar-static-top"))
                    CssClasses.Remove("navbar-static-top");
            }

            position = value;
        }
    }
    /// <summary>
    /// Gets or sets the theme
    /// </summary>
    public Theme Theme
    {
        get { return theme; }
        set
        {
            if (value == Theme.Dark)
            {
                if (!CssClasses.Contains("navbar-inverse"))
                    CssClasses.Add("navbar-inverse");
            }
            else
            {
                if (CssClasses.Contains("navbar-inverse"))
                    CssClasses.Remove("navbar-inverse");
            }
            if (value == Theme.Light)
            {
                if (!CssClasses.Contains("navbar-default"))
                    CssClasses.Add("navbar-default");
            }
            else
            {
                if (CssClasses.Contains("navbar-default"))
                    CssClasses.Remove("navbar-default");
            }

            theme = value;
        }
    }
}

/// <summary>
/// Represents a menu
/// </summary>
public class NavBar : Menu { }