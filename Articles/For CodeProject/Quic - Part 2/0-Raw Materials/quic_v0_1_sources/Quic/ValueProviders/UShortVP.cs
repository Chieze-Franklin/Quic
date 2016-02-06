using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and an unsigned short integer
    /// </summary>
    public class UShortVP  : ValueProviderResource
    {
        static UShortVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static UShortVP Singleton()
        {
            if (singleton == null)
                singleton = new UShortVP();

            return singleton;
        }

        /// <summary>
        /// Produces an unsigned short integer object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is ushort)
            {
                return (ushort)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces an unsigned short integer object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return ushort.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and an unsigned short integer
    /// </summary>
    public class UShortIntegerVP : UShortVP { }
}
