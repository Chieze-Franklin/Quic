using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    public class Comment : Element
    {
        public override void Render()
        {
            if (this.Document.OutputFile is TextFile)
            {
                TextFile outputFile = (TextFile)this.Document.OutputFile;
                if (outputFile is SectionedTextFile)
                    outputFile = ((SectionedTextFile)outputFile).CurrentSection;

                if (IsCData)
                {
                    if (Content != null)
                        outputFile.WriteLine(Content);
                }
                else if (ToBeOutput)
                {
                    outputFile.WriteLine("<!--(Output from QUIC):");
                    if (Content != null)
                        outputFile.WriteLine(Content);
                    outputFile.WriteLine("-->");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value to determine if the comment should write to the file as CDATA
        /// </summary>
        [NotQuicProperty]
        public bool IsCData { get; set; }
        /// <summary>
        /// Gets or sets a value to determine if the comment should write to the file as comment
        /// </summary>
        [NotQuicProperty]
        public bool ToBeOutput { get; set; }
    }
}
