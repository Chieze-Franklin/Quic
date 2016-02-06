using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a double
    /// </summary>
    public class DoubleVP : ValueProviderResource
    {
        static DoubleVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static DoubleVP Singleton()
        {
            if (singleton == null)
                singleton = new DoubleVP();

            return singleton;
        }

        /// <summary>
        /// Produces a double object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is double)
            {
                return (double)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a double object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return double.Parse(input);
        }
    }
}
