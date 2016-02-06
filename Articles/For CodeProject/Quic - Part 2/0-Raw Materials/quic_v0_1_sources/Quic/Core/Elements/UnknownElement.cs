using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    public class UnknownElement : Element
    {
        public UnknownElement() 
        {
            IsContainer = true;
        }

        public override void Render()
        {
            if (this.Document.OutputFile is TextFile)
            {
                TextFile outputFile = (TextFile)this.Document.OutputFile;
                if (outputFile is SectionedTextFile)
                    outputFile = ((SectionedTextFile)outputFile).CurrentSection;

                //opening tag
                outputFile.Write(string.Format("<{0}", Tag));

                //custom properties
                outputFile.Write(EmitCustomProperties());

                if (IsEmptyTag)
                {
                    //closing tag
                    outputFile.Write(" />");
                }
                else
                {
                    //opening tag 2
                    outputFile.WriteLine(">");

                    //child elements
                    foreach (var element in Elements)
                    {
                        element.BeginRender();
                    }
                    outputFile.WriteLine();

                    //closing tag
                    outputFile.WriteLine(string.Format("</{0}>", Tag));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value to determine if the tag is empty (like "<br />")
        /// </summary>
        [NotQuicProperty]
        public bool IsEmptyTag { get; set; }
        /// <summary>
        /// Gets or sets the fullname of the unknown tag
        /// </summary>
        [NotQuicProperty]
        public string Tag { get; set; }
    }
}
