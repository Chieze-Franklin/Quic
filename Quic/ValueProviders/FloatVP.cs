using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a float
    /// </summary>
    public class FloatVP : ValueProviderResource
    {
        static FloatVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static FloatVP Singleton()
        {
            if (singleton == null)
                singleton = new FloatVP();

            return singleton;
        }

        /// <summary>
        /// Produces a float object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is float)
            {
                return (float)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a float object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return float.Parse(input);
        }
    }
}
