using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a short integer
    /// </summary>
    public class ShortVP : ValueProviderResource
    {
        static ShortVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static ShortVP Singleton()
        {
            if (singleton == null)
                singleton = new ShortVP();

            return singleton;
        }

        /// <summary>
        /// Produces a short integer object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is short)
            {
                return (short)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a short integer object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return short.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and a short integer
    /// </summary>
    public class ShortIntergerVP : ShortVP { }
}
