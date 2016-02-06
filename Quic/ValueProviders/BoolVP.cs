using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a boolean
    /// </summary>
    public class BoolVP : ValueProviderResource
    {
        static BoolVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static BoolVP Singleton()
        {
            if (singleton == null)
                singleton = new BoolVP();

            return singleton;
        }

        /// <summary>
        /// Produces a boolean object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is bool)
            {
                return (bool)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a boolean object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            if (input.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                input = System.Boolean.TrueString;
            else if (input.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                input = System.Boolean.FalseString;
            return bool.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and a boolean
    /// </summary>
    public class BooleanVP : BoolVP { }
}
