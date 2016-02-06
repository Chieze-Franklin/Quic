using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and System.Drawing.Image
    /// </summary>
    public class ImageVP : ValueProviderResource
    {
        static ImageVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static ImageVP Singleton()
        {
            if (singleton == null)
                singleton = new ImageVP();

            return singleton;
        }

        /// <summary>
        /// Produces a System.Drawing.Image object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is System.Drawing.Image)
            {
                return (System.Drawing.Image)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a System.Drawing.Image object from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            if (this.Document != null)
                input = Utils.FileSystemServices.GetAbsolutePath(input, this.Document.SourcePath);
            else
                input = Utils.FileSystemServices.GetAbsolutePath(input);
            return System.Drawing.Image.FromFile(input);
        }
    }
}
