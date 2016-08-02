using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a decimal
    /// </summary>
    public class DecimalVP : ValueProviderResource
    {
        static DecimalVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static DecimalVP Singleton()
        {
            if (singleton == null)
                singleton = new DecimalVP();

            return singleton;
        }

        /// <summary>
        /// Produces a decimal object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is decimal)
            {
                return (decimal)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a decimal object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return decimal.Parse(input);
        }
    }
}
