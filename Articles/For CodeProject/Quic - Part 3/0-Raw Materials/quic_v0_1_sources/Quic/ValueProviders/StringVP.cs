using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that helps produce strings with characters that otherwise may not be producible in an XML attribute value.
    /// </summary>
    public class StringVP : ValueProviderResource
    {
        static StringVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static StringVP Singleton() 
        {
            if (singleton == null)
                singleton = new StringVP();

            return singleton;
        }

        /// <summary>
        /// Produces a string with unicode characters from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a string with unicode characters from a string where such characters are represented by their unicode numbers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return input;
        }
    }
}
