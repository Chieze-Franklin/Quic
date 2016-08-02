using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// Initializes a HTML file
    /// </summary>
    public class HtmlFileInit : FileInitializer
    {
        /// <summary>
        /// Initializers an HTML output file with the specified file name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override OutputFile InitializeFile(QuicDocument doc, string filename)
        {
            HtmlOutputFile htmlOutputFile = new HtmlOutputFile(filename);
            htmlOutputFile.CurrentSection = htmlOutputFile.HeadSection;

            //put it in the output dir
            if (doc.OutputDirectory != null)
                doc.OutputDirectory.Add(htmlOutputFile, true);

            return htmlOutputFile;
        }
    }
}
