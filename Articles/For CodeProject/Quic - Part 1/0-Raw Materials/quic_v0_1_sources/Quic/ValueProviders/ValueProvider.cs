using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// Provides the default implementation for Quic.IValueProvider
    /// </summary>
    public abstract class ValueProvider : IValueProvider
    {
        /// <summary>
        /// Produces an object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract object Evaluate(object input);
        /// <summary>
        /// Produces an object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract object Evaluate(string input);

        /// <summary>
        /// Gets or sets the name of the property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the type of the property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        public Type PropertyType { get; set; }
        /// <summary>
        /// Gets or sets the Quic.Element whose property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        public Element Element { get; set; }
    }

    /// <summary>
    /// Provides the default implementation for Quic.IValueProvider,
    /// and acts as the base class for value providers that are document resources.
    /// </summary>
    public abstract class ValueProviderResource : ResourceElement, IValueProvider
    {
        /// <summary>
        /// Produces an object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract object Evaluate(object input);
        /// <summary>
        /// Produces an object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract object Evaluate(string input);

        /// <summary>
        /// Gets or sets the name of the property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the type of the property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        public Type PropertyType { get; set; }
        /// <summary>
        /// Gets or sets the Quic.Element whose property this value provider is trying to set.
        /// Can be null if this value provider is not acting on a specific object.
        /// </summary>
        public Element Element { get; set; }

        public override void Render()
        {
        }
    }
}
