using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a byte
    /// </summary>
    public class ByteVP : ValueProviderResource
    {
        static ByteVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static ByteVP Singleton()
        {
            if (singleton == null)
                singleton = new ByteVP();

            return singleton;
        }

        /// <summary>
        /// Produces a byte object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is byte)
            {
                return (byte)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a byte object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return byte.Parse(input);
        }
    }
}
