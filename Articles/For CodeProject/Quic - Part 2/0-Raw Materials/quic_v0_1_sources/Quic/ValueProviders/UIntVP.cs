using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and an unsigned integer
    /// </summary>
    public class UIntVP : ValueProviderResource
    {
        static UIntVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static UIntVP Singleton()
        {
            if (singleton == null)
                singleton = new UIntVP();

            return singleton;
        }

        /// <summary>
        /// Produces an unsigned integer object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is uint)
            {
                return (uint)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces an unsigned integer object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return uint.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and an unsigned integer
    /// </summary>
    public class UIntegerVP : UIntVP { }
}
