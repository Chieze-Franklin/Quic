using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    public interface IResource
    {
        /// <summary>
        /// Gets or sets the key with which this resource will be stored
        /// in the resource dictionary of its parent document
        /// </summary>
        string Key { get; set; }
    }
}
