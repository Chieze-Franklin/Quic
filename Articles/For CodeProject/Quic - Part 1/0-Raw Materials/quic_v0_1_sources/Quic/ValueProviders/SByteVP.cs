using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and an sbyte
    /// </summary>
    public class SByteVP : ValueProviderResource
    {
        static SByteVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static SByteVP Singleton()
        {
            if (singleton == null)
                singleton = new SByteVP();

            return singleton;
        }

        /// <summary>
        /// Produces an sbyte object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is sbyte)
            {
                return (sbyte)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces an sbyte object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return sbyte.Parse(input);
        }
    }
}
