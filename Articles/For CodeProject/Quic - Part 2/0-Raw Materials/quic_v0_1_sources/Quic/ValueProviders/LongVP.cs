using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a long integer
    /// </summary>
    public class LongVP : ValueProviderResource
    {
        static LongVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static LongVP Singleton()
        {
            if (singleton == null)
                singleton = new LongVP();

            return singleton;
        }

        /// <summary>
        /// Produces a long integer object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is long)
            {
                return (long)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a long integer object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return long.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and a long integer
    /// </summary>
    public class LongIntegerVP : LongVP { }
}
