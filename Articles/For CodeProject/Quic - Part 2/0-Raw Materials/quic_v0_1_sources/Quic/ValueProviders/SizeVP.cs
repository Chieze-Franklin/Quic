using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a size
    /// </summary>
    public class SizeVP : ValueProviderResource
    {
        static SizeVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static SizeVP Singleton()
        {
            if (singleton == null)
                singleton = new SizeVP();

            return singleton;
        }

        /// <summary>
        /// Produces a size object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is System.Drawing.Size)
            {
                return (System.Drawing.Size)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a size object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            string[] parts = input.Split(',');
            if (parts.Length != 2)
                throw new Exception(string.Format("Badly formed size literal '{0}'", input));
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            return new System.Drawing.Size(x, y);
        }
    }
}
