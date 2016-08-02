using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a sizeF
    /// </summary>
    public class SizeFVP : ValueProviderResource
    {
        static SizeFVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static SizeFVP Singleton()
        {
            if (singleton == null)
                singleton = new SizeFVP();

            return singleton;
        }

        /// <summary>
        /// Produces a sizeF object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is System.Drawing.SizeF)
            {
                return (System.Drawing.SizeF)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a sizeF object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            string[] parts = input.Split(',');
            if (parts.Length != 2)
                throw new Exception(string.Format("Badly formed size literal '{0}'", input));
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);
            return new System.Drawing.SizeF(x, y);
        }
    }
}
