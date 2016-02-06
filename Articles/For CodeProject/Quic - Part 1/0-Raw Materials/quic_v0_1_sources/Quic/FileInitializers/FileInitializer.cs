using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// The base class for file initializers
    /// </summary>
    public abstract class FileInitializer
    {
        /// <summary>
        /// Initializers an output file with the specified file name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public abstract OutputFile InitializeFile(QuicDocument doc, string filename);
    }
}
