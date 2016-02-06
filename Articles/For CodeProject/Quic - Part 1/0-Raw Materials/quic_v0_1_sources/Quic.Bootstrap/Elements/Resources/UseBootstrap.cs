using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Include this resource if your page depends on bootstrap to work.
/// </summary>
public class UseBootstrap : ResourceElement
{
    void CheckForNecessaryFiles()
    {
        //css
        var cssObj = this.Document.OutputDirectory.Get("css");
        if (cssObj == null) //ensure the css dir exists
        {
            cssObj = new OutputDirectory("css");
            this.Document.OutputDirectory.Add(cssObj, true);
        }
        if (cssObj is OutputDirectory)
        {
            var cssDir = (OutputDirectory)cssObj;

            //ensure the relevant css files exist
            var bootstrapCss = cssDir.Get("bootstrap.css");
            if (bootstrapCss == null)
            {
                bootstrapCss = new CssOutputFile("bootstrap.css");
                ((CssOutputFile)bootstrapCss).Text = Properties.Resources.BootstrapDotCss;
                cssDir.Add(bootstrapCss, true);
            }

            var bootstrapCssMap = cssDir.Get("bootstrap.css.map");
            if (bootstrapCssMap == null)
            {
                bootstrapCssMap = new CssOutputFile("bootstrap.css.map");
                ((CssOutputFile)bootstrapCssMap).Text = Properties.Resources.BootstrapDotCssDotMap;
                cssDir.Add(bootstrapCssMap, true);
            }

            var bootstrapMinCss = cssDir.Get("bootstrap.min.css");
            if (bootstrapMinCss == null)
            {
                bootstrapMinCss = new CssOutputFile("bootstrap.min.css");
                ((CssOutputFile)bootstrapMinCss).Text = Properties.Resources.BootstrapDotMinDotCss;
                cssDir.Add(bootstrapMinCss, true);
            }

            var bootstrapThemeCss = cssDir.Get("bootstrap-theme.css");
            if (bootstrapThemeCss == null)
            {
                bootstrapThemeCss = new CssOutputFile("bootstrap-theme.css");
                ((CssOutputFile)bootstrapThemeCss).Text = Properties.Resources.BootstrapDashThemeDotCss;
                cssDir.Add(bootstrapThemeCss, true);
            }

            var bootstrapThemeCssMap = cssDir.Get("bootstrap-theme.css.map");
            if (bootstrapThemeCssMap == null)
            {
                bootstrapThemeCssMap = new CssOutputFile("bootstrap-theme.css.map");
                ((CssOutputFile)bootstrapThemeCssMap).Text = Properties.Resources.BootstrapDashThemeDotCssDotMap;
                cssDir.Add(bootstrapThemeCssMap, true);
            }

            var bootstrapThemeMinCss = cssDir.Get("bootstrap-theme.min.css");
            if (bootstrapThemeMinCss == null)
            {
                bootstrapThemeMinCss = new CssOutputFile("bootstrap-theme.min.css");
                ((CssOutputFile)bootstrapThemeMinCss).Text = Properties.Resources.BootstrapDashThemeDotMinDotCss;
                cssDir.Add(bootstrapThemeMinCss, true);
            }
        }

        //javascript
        var jsObj = this.Document.OutputDirectory.Get("js");
        if (jsObj == null) //ensure the js dir exists
        {
            jsObj = new OutputDirectory("js");
            this.Document.OutputDirectory.Add(jsObj, true);
        }
        if (jsObj is OutputDirectory)
        {
            var jsDir = (OutputDirectory)jsObj;

            //ensure the relevant js files exist
            var bootstrapJs = jsDir.Get("bootstrap.js");
            if (bootstrapJs == null)
            {
                bootstrapJs = new JsOutputFile("bootstrap.js");
                ((JsOutputFile)bootstrapJs).Text = Properties.Resources.BootstrapDotJs;
                jsDir.Add(bootstrapJs, true);
            }

            var bootstrapMinJs = jsDir.Get("bootstrap.min.js");
            if (bootstrapMinJs == null)
            {
                bootstrapMinJs = new JsOutputFile("bootstrap.min.js");
                ((JsOutputFile)bootstrapMinJs).Text = Properties.Resources.BootstrapDotMinDotJs;
                jsDir.Add(bootstrapMinJs, true);
            }

            var jqueryJs = jsDir.Get("jquery.js");
            if (jqueryJs == null)
            {
                jqueryJs = new JsOutputFile("jquery.js");
                ((JsOutputFile)jqueryJs).Text = Properties.Resources.JqueryDotJs;
                jsDir.Add(jqueryJs, true);
            }

            var npmJs = jsDir.Get("npm.js");
            if (npmJs == null)
            {
                npmJs = new JsOutputFile("npm.js");
                ((JsOutputFile)npmJs).Text = Properties.Resources.NpmDotJs;
                jsDir.Add(npmJs, true);
            }
        }

        //fonts
        var fontsObj = this.Document.OutputDirectory.Get("fonts");
        if (fontsObj == null) //ensure the fonts dir exists
        {
            fontsObj = new OutputDirectory("fonts");
            this.Document.OutputDirectory.Add(fontsObj, true);
        }
        if (fontsObj is OutputDirectory)
        {
            var fontsDir = (OutputDirectory)fontsObj;

            //ensure the relevant font files exist
            var glyphEot = fontsDir.Get("glyphicons-halflings-regular.eot");
            if (glyphEot == null)
            {
                glyphEot = new BytesFile("glyphicons-halflings-regular.eot");
                ((BytesFile)glyphEot).Bytes = Properties.Resources.GlyphiconsDashHalflingsDashRegularDotEot;
                fontsDir.Add(glyphEot, true);
            }

            var glyphSvg = fontsDir.Get("glyphicons-halflings-regular.svg");
            if (glyphSvg == null)
            {
                glyphSvg = new BytesFile("glyphicons-halflings-regular.svg");
                ((BytesFile)glyphSvg).Bytes = Properties.Resources.GlyphiconsDashHalflingsDashRegularDotSvg;
                fontsDir.Add(glyphSvg, true);
            }

            var glyphTtf = fontsDir.Get("glyphicons-halflings-regular.ttf");
            if (glyphTtf == null)
            {
                glyphTtf = new BytesFile("glyphicons-halflings-regular.ttf");
                ((BytesFile)glyphTtf).Bytes = Properties.Resources.GlyphiconsDashHalflingsDashRegularDotTtf;
                fontsDir.Add(glyphTtf, true);
            }

            var glyphWoff = fontsDir.Get("glyphicons-halflings-regular.woff");
            if (glyphWoff == null)
            {
                glyphWoff = new BytesFile("glyphicons-halflings-regular.woff");
                ((BytesFile)glyphWoff).Bytes = Properties.Resources.GlyphiconsDashHalflingsDashRegularDotWoff;
                fontsDir.Add(glyphWoff, true);
            }

            var glyphWoff2 = fontsDir.Get("glyphicons-halflings-regular.woff2");
            if (glyphWoff2 == null)
            {
                glyphWoff2 = new BytesFile("glyphicons-halflings-regular.woff2");
                ((BytesFile)glyphWoff2).Bytes = Properties.Resources.GlyphiconsDashHalflingsDashRegularDotWoff2;
                fontsDir.Add(glyphWoff2, true);
            }
        }
    }

    public override void Render()
    {
        CheckForNecessaryFiles();

        if (this.Document.OutputFile is HtmlOutputFile)
        {
            HtmlOutputFile outputFile = (HtmlOutputFile)this.Document.OutputFile;

            outputFile.HeadSection.AppendLine();
            outputFile.HeadSection.AppendLine("<!-- It is recommended, as a thank you, we ask you to include an optional link back to");
            outputFile.HeadSection.AppendLine("     GLYPHICONS (http://glyphicons.com) whenever practical. -- Bootstrap Documentation -->");
        }
    }
}