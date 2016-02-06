using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// Represents an implicitly-created text element
    /// </summary>
    public class Text : Element
    {
        public override void Render()
        {
            if (this.Document.OutputFile is TextFile)
            {
                TextFile outputFile = (TextFile)this.Document.OutputFile;
                if (outputFile is SectionedTextFile)
                    outputFile = ((SectionedTextFile)outputFile).CurrentSection;

                if (Content != null)
                    outputFile.WriteLine(Content);
            }
        }
    }
}
