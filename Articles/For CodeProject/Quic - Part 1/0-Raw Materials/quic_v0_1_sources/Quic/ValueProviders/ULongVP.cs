using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and an unsigned long integer
    /// </summary>
    public class ULongVP : ValueProviderResource
    {
        static ULongVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static ULongVP Singleton()
        {
            if (singleton == null)
                singleton = new ULongVP();

            return singleton;
        }

        /// <summary>
        /// Produces an unsigned long integer object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is ulong)
            {
                return (ulong)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces an unsigned long integer object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return ulong.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and an unsigned long integer
    /// </summary>
    public class ULongIntegerVP : ULongVP { }
}
