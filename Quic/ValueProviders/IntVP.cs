using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and an integer
    /// </summary>
    public class IntVP : ValueProviderResource
    {
        static IntVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static IntVP Singleton()
        {
            if (singleton == null)
                singleton = new IntVP();

            return singleton;
        }

        /// <summary>
        /// Produces an integer object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is int)
            {
                return (int)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces an integer object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return int.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and an integer
    /// </summary>
    public class IntegerVP : IntVP { }
}
