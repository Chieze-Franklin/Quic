using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and System.Drawing.Color
    /// </summary>
    public class ColorVP : ValueProviderResource
    {
        static ColorVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static ColorVP Singleton() 
        {
            if (singleton == null)
                singleton = new ColorVP();

            return singleton;
        }

        /// <summary>
        /// Produces a System.Drawing.Color object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is System.Drawing.Color)
            {
                return (System.Drawing.Color)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a System.Drawing.Color object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            if (input.StartsWith("#")) 
            {
                int a = 255, r = 0, g = 0, b = 0;
                input = input.TrimStart('#');
                if (input.Length == 3) 
                {
                    r = ParseHex(input[0]);
                    g = ParseHex(input[1]);
                    b = ParseHex(input[2]);
                }
                else if (input.Length == 4)
                {
                    a = ParseHex(input[0]);
                    r = ParseHex(input[1]);
                    g = ParseHex(input[2]);
                    b = ParseHex(input[3]);
                }
                else if (input.Length == 6)
                {
                    r = ParseHex(input[0], input[1]);
                    g = ParseHex(input[2], input[3]);
                    b = ParseHex(input[4], input[5]);
                }
                else if (input.Length == 8)
                {
                    a = ParseHex(input[0], input[1]);
                    r = ParseHex(input[2], input[3]);
                    g = ParseHex(input[4], input[5]);
                    b = ParseHex(input[6], input[7]);
                }
                else if (input.Length == 9)
                {
                    r = int.Parse(input.Substring(0, 3));
                    g = int.Parse(input.Substring(3, 3));
                    b = int.Parse(input.Substring(6, 3));
                }
                else if (input.Length == 12)
                {
                    a = int.Parse(input.Substring(0, 3));
                    r = int.Parse(input.Substring(3, 3));
                    g = int.Parse(input.Substring(6, 3));
                    b = int.Parse(input.Substring(9, 3));
                }

                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            else
                return System.Drawing.Color.FromName(input);
        }

        ///// <summary>
        ///// Gets a possible string that represents the System.Drawing.Color object
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public string EvaluateBack(object obj)
        //{
        //    if (!(obj is Color))
        //        throw new ArgumentException(string.Format("Cannot convert type '{0}'", obj.GetType()));

        //    Color color = (Color)obj;
        //    if (color.IsNamedColor)
        //        return color.Name;
        //    return color.ToString();
        //}

        int ParseHex(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            else if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;
            else if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            throw new ArgumentException();
        }
        int ParseHex(params char[] cc)
        {
            int result = 0;
            foreach (char c in cc)
            {
                if (c >= '0' && c <= '9')
                    result = result * 16 + c - '0';
                else if (c >= 'a' && c <= 'f')
                    result = result * 16 + c - 'a' + 10;
                else if (c >= 'A' && c <= 'F')
                    result = result * 16 + c - 'A' + 10;
                else
                    throw new ArgumentException();
            }

            return result;
        }
    }
}
