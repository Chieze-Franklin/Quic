using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// The base class for output objects
    /// </summary>
    public abstract class OutputObject
    {
        string _name;

        /// <summary>
        /// Creates a new instance of Quic.OutputObject
        /// </summary>
        /// <param name="name"></param>
        public OutputObject(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Name cannot be null or empty.");

            Name = name;
        }

        /// <summary>
        /// Called to persist the object.
        /// </summary>
        /// <param name="canOverride"></param>
        public abstract void Commit(bool canOverride);

        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Gets the full name of the object.
        /// </summary>
        public string FullName 
        {
            get 
            {
                if (ParentDirectory == null)
                    return Name;
                else
                {
                    return ParentDirectory.FullName + "\\" + Name;
                }
            }
        }
        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public string Name 
        {
            get { return _name; }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Name cannot be null or empty.");

                _name = value;
            }
        }
        /// <summary>
        /// Gets or sets the parent directory of the object.
        /// </summary>
        public OutputDirectory ParentDirectory { get; set; }
    }
}
