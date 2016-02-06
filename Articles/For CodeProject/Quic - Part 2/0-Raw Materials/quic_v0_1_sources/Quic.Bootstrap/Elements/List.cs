using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a list
/// </summary>
public class List : BootstrapContainerElement
{
    ListType listType;

    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Type")
        {
            return ListTypeProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //tag type
            string tag = "ul";
            if (Type == ListType.Ordered)
                tag = "ol";

            //opening tag
            outputFile.Write(string.Format("<{0}", tag));
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());
            outputFile.WriteLine(">");
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine(string.Format("</{0}>", tag));
        }
    }

    /// <summary>
    /// Gets or sets the list type
    /// </summary>
    public ListType Type
    {
        get { return listType; }
        set
        {
            //if (value == ListType.Drop)
            //{
            //    if (!CssClasses.Contains("dropdown-menu"))
            //        CssClasses.Add("dropdown-menu");
            //}
            //else
            //{
            //    if (CssClasses.Contains("dropdown-menu"))
            //        CssClasses.Remove("dropdown-menu");
            //}

            listType = value;
        }
    }
}