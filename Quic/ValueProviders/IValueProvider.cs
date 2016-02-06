using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// The interface to be implemented by any class that seeks to be a value provider.
    /// Typically a value provider can convert a raw string input into an output (probably of another type).
    /// </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// Produces an object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        object Evaluate(object input);
        /// <summary>
        /// Produces an object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        object Evaluate(string input);

        /// <summary>
        /// Gets or sets the name of the property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the type of the property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        Type PropertyType { get; set; }
        /// <summary>
        /// Gets or sets the Quic.Element whose property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        Element Element { get; set; }
    }
}
