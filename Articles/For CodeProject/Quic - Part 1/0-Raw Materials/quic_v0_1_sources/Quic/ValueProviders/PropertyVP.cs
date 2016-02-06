using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that provides the value of the property of a document object
    /// </summary>
    public class PropertyVP : ValueProviderResource
    {
        static PropertyVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static PropertyVP Singleton() 
        {
            if (singleton == null)
                singleton = new PropertyVP();

            return singleton;
        }

        /// <summary>
        /// Returns the value of the property specified.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            return Evaluate(input.ToString());
        }
        /// <summary>
        /// Returns the value of the property specified.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            //A.B, parent.B, this.B or B

            input = input.Trim();

            Element obj = null;
            string prop = null;

            if (!input.Contains('.'))
            {
                obj = Element;
                prop = input;
            }
            else 
            {
                string[] inputParts = input.Split('.');
                if (inputParts.Length != 2)  //for now we support only a.b, nothing more complex (like a.b.c or a.b[c])
                    throw new Exception(string.Format("Badly formed property access '{0}'", input));
                string objName = inputParts[0].Trim();
                prop = inputParts[1].Trim();

                if (objName == "this" || objName == "self")
                {
                    obj = Element;
                }
                else if (objName == "parent")
                {
                    obj = ((UIElement)Element).ParentElement;
                }
                else 
                {
                    obj = Element.Document.GetNamedElement(objName);
                }
            }

            return obj.GetProperty(prop);
        }
    }
}
