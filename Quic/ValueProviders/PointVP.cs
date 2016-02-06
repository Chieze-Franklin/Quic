using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a point
    /// </summary>
    public class PointVP : ValueProviderResource
    {
        static PointVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static PointVP Singleton()
        {
            if (singleton == null)
                singleton = new PointVP();

            return singleton;
        }

        /// <summary>
        /// Produces a point object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is System.Drawing.Point)
            {
                return (System.Drawing.Point)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a point object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            if (input.Contains(','))
            {
                string[] parts = input.Split(',');
                if (parts.Length != 2)
                    throw new Exception(string.Format("Badly formed point literal '{0}'", input));
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);
                return new System.Drawing.Point(x, y);
            }
            else 
            {
                int num = int.Parse(input);
                return new System.Drawing.Point(num);
            }
        }
    }
}
