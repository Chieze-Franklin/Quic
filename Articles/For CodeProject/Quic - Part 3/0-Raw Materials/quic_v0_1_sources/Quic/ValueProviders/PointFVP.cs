using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a pointF
    /// </summary>
    public class PointFVP : ValueProviderResource
    {
        static PointFVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static PointFVP Singleton()
        {
            if (singleton == null)
                singleton = new PointFVP();

            return singleton;
        }

        /// <summary>
        /// Produces a pointF object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is System.Drawing.PointF)
            {
                return (System.Drawing.PointF)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a pointF object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            string[] parts = input.Split(',');
            if (parts.Length != 2)
                throw new Exception(string.Format("Badly formed point literal '{0}'", input));
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);
            return new System.Drawing.PointF(x, y);
        }
    }
}
