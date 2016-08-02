using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class Pagination : List
{
    PaginationSize pgSz;

    public Pagination()
    {
        CssClasses.Add("pagination");
    }

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "PaginationSize")
        {
            return PaginationSizeProvider.Singleton();
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
    /// Gets or sets the pagination size
    /// </summary>
    public PaginationSize PaginationSize
    {
        get { return pgSz; }
        set
        {
            if (value == PaginationSize.ExtraLarge || value == PaginationSize.Large)
            {
                if (!CssClasses.Contains("pagination-lg"))
                    CssClasses.Add("pagination-lg");
            }
            else
            {
                if (CssClasses.Contains("pagination-lg"))
                    CssClasses.Remove("pagination-lg");
            }

            if (value == PaginationSize.ExtraSmall || value == PaginationSize.Small)
            {
                if (!CssClasses.Contains("pagination-sm"))
                    CssClasses.Add("pagination-sm");
            }
            else
            {
                if (CssClasses.Contains("pagination-sm"))
                    CssClasses.Remove("pagination-sm");
            }

            pgSz = value;
        }
    }
}