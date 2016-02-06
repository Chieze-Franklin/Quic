using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class ColumnSpanProvider : ValueProvider
{
    static ColumnSpanProvider singleton;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns></returns>
    public static ColumnSpanProvider Singleton()
    {
        if (singleton == null)
            singleton = new ColumnSpanProvider();

        return singleton;
    }

    public override object Evaluate(object input)
    {
        if (input is int)
        {
            return (int)input;
        }

        return Evaluate(input.ToString());
    }

    public override object Evaluate(string input)
    {
        input = input.Trim();
        int cols = 12;

        if (input.EndsWith("%"))
        {
            input = input.Substring(0, input.Length - 1);
            double dblinput = double.Parse(input);
            double dblCols = (dblinput * 12) / 100;
            cols = (int)Math.Round(dblCols, MidpointRounding.AwayFromZero);
        }
        else
        {
            cols = int.Parse(input);
        }

        return cols;
    }
}