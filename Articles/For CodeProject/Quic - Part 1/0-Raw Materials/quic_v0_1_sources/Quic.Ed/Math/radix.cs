using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

namespace Quic.Ed.Math
{
    /// <summary>
    /// Represents a root expression
    /// </summary>
    public class Radix : UIElement
    {
        public override void Render()
        {
            if (this.Document.OutputFile is TextFile)
            {
                TextFile outputFile = (TextFile)this.Document.OutputFile;

                //get measurements
                float indexWidth = 1;// indexHeight = 10;
                if (!string.IsNullOrEmpty(index))
                {
                    indexWidth += (index.Length * 5);
                }
                int radicandWidth = 1;// radicandHeight = 15;
                if (!string.IsNullOrEmpty(radicand))
                {
                    radicandWidth += (radicand.Length * 10);
                }

                outputFile.WriteLine(string.Format("<svg width=\"{0}\" height=\"24\">", 
                    6 + indexWidth + radicandWidth));

                //index
                if (!string.IsNullOrEmpty(index))
                {
                    outputFile.WriteLine(string.Format("<text x=\"1\" y=\"10\" fill=\"black\" font-size=\"10\">{0}</text>", index));
                    indexWidth += index.Length;
                }

                //radicand
                if (!string.IsNullOrEmpty(radicand))
                {
                    outputFile.WriteLine(string.Format("<text x=\"{0}\" y=\"20\" fill=\"black\" font-size=\"15\">{1}</text>", 
                        indexWidth + 7, radicand));
                }

                //polyline
                outputFile.WriteLine(string.Format("<polyline points=\"0,11 {0},11 {1},20 {2},5 {3},5\"" +
                    " style=\"stroke:black; stroke-width:1\" fill=\"none\" />",
                    indexWidth, indexWidth + 4, indexWidth + 6, indexWidth + 7 + radicandWidth));

                outputFile.Write("</svg>");
            }
        }

        public string index { get; set; }
        public string radicand { get; set; }
    }

    //alias
    public class RadicalSign : Radix { }
}
