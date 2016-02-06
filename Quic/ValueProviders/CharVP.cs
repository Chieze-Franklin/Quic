using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a character
    /// </summary>
    public class CharVP : ValueProviderResource
    {
        static CharVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static CharVP Singleton()
        {
            if (singleton == null)
                singleton = new CharVP();

            return singleton;
        }

        /// <summary>
        /// Produces a character object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is char)
            {
                return (char)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a character object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return char.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and a character
    /// </summary>
    public class CharacterVP : CharVP { }
}
