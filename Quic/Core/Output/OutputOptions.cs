using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// Holds the options to be used for the current output session
    /// </summary>
    public class OutputOptions
    {
        /// <summary>
        /// Gets or sets a value to determine if unknown attributes are allowed
        /// </summary>
        public bool AllowUnknownAttributes { get; set; }
        /// <summary>
        /// Gets or sets a value to determine if unknown tags are allowed
        /// </summary>
        public bool AllowUnknownTags { get; set; }
        /// <summary>
        /// Gets or sets a value to determine if the case of attributes is ignored
        /// </summary>
        public bool IgnoreAttributeCase { get; set; }
        /// <summary>
        /// Gets or sets a value to determine if the case of tags is ignored
        /// </summary>
        public bool IgnoreTagCase { get; set; }
    }
}
