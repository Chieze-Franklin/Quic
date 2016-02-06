using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// A class that converts between System.String and a date-time
    /// </summary>
    public class DateTimeVP : ValueProviderResource
    {
        static DateTimeVP singleton;

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns></returns>
        public static DateTimeVP Singleton()
        {
            if (singleton == null)
                singleton = new DateTimeVP();

            return singleton;
        }

        /// <summary>
        /// Produces a date-time object from an input object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(object input)
        {
            if (input is DateTimeVP)
            {
                return (DateTimeVP)input;
            }
            else
                return Evaluate(input.ToString());
        }
        /// <summary>
        /// Produces a date-time object from a string.
        /// Some valid strings:
        /// simpleTime = "1/1/2000"
        /// httpTime = "Fri, 27 Feb 2009 03:11:21 GMT"
        /// w3orgTime = "2009/02/26 18:37:58"
        /// nyTime = "Thursday, February 26, 2009"
        /// perlTime = "February 26, 2009"
        /// isoTime = "2002-02-10"
        /// windowsTime = "2/21/2009 10:35 PM"
        /// windowsPanelTime = "8:04:00 PM"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override object Evaluate(string input)
        {
            return System.DateTime.Parse(input);
        }
    }

    /// <summary>
    /// A class that converts between System.String and a date-time
    /// </summary>
    public class DateVP : DateTimeVP { }
}
