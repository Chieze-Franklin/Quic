using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// Represents the default implementation of Quic.IQuicResourceElement
    /// </summary>
    public abstract class ResourceElement : Element, IResource
    {
        string key;

        /// <summary>
        /// Gets or sets the key with which this resource will be stored
        /// in the resource dictionary of its parent document
        /// </summary>
        public virtual string Key
        {
            get { return key; }
            set
            {
                if (IsValidIdentifier(value))
                    key = value;
                else
                    throw new ArgumentException(string.Format("Invalid resource key '{0}'", value));
            }
        }
    }
}
