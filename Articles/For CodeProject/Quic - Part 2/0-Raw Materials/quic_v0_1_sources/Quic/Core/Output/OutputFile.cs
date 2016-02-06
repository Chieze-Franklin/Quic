using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic.Utils;

namespace Quic
{
    /// <summary>
    /// Represents a file to be outputed after a file translation.
    /// </summary>
    public abstract class OutputFile : OutputObject
    {
        /// <summary>
        /// Creates a new instance of Quic.OutputFile
        /// </summary>
        /// <param name="name"></param>
        public OutputFile(string name)
            : base(name) { }
    }

    /// <summary>
    /// Represents a file content as an array of bytes
    /// </summary>
    public class BytesFile : OutputFile
    {
        /// <summary>
        /// Creates a new instance of Quic.BytesOutputFile
        /// </summary>
        /// <param name="name"></param>
        public BytesFile(string name) : base(name) { }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="canOverride"></param>
        public override void Commit(bool canOverride)
        {
            FileSystemServices.CreateFile(FullName, canOverride, "");

            FileSystemServices.WriteBytesToFile(FullName, Bytes);
        }

        /// <summary>
        /// Gets or sets the array of bytes in this file.
        /// </summary>
        public byte[] Bytes { get; set; }
    }

    /// <summary>
    /// Represents a file that contains raw texts, in lines
    /// </summary>
    public class TextFile : OutputFile
    {
        List<string> lines;
        int cursorIndex = 0;
        protected string NewLine = "\n"; //Environment.NewLine;

        public TextFile(string name) : base(name) 
        {
            lines = new List<string>();
            lines.Add(""); //add empty line
        }

        /// <summary>
        /// Moves cursor to the end of the file, then writes text
        /// </summary>
        /// <param name="text"></param>
        public void Append(string text) 
        {
            MoveCursorToEnd();
            Write(text);
        }
        /// <summary>
        /// Moves cursor to the end of the file, and adds new line
        /// </summary>
        public void AppendLine()
        {
            Append(NewLine);
        }
        /// <summary>
        /// Moves cursor to the end of the file, writes text, and adds new line
        /// </summary>
        /// <param name="text"></param>
        public void AppendLine(string text)
        {
            Append(text + NewLine);
        }
        public void Backspace()
        {
            if (CursorIndex == 0)
                return;

            string currentText = Text;
            int currentIndex = CursorIndex - 1;

            currentText = currentText.Remove(currentIndex, 1);

            Text = currentText;
            CursorIndex = currentIndex;
        }
        public override void Commit(bool canOverride)
        {
            FileSystemServices.CreateFile(FullName, canOverride, "");

            FileSystemServices.WriteTextToFile(FullName, Text);
        }
        public void Delete()
        {
            if (CursorIndex == Text.Length)
                return;

            string currentText = Text;
            int currentIndex = CursorIndex;

            currentText = currentText.Remove(currentIndex, 1);

            Text = currentText;
        }
        /// <summary>
        /// Deletes the content of the line but retains the line
        /// </summary>
        /// <param name="line"></param>
        public void DeleteLine(int line)
        {
            lines[line] = "";

            MoveCursorTo(line, 0);
        }
        public void MoveCursorTo(int index)
        {
            CursorIndex = index;
        }
        /// <summary>
        /// Moves cursor to a particular column on a particular line.
        /// Indeces are zero-based, so the first line has a line number of zero.
        /// Likewise, the first column on a line has the column number of zero.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        public void MoveCursorTo(int line, int column) 
        {
            //ensure that column falls within that line
            if (column < 0 || column > lines[line].Length)
                throw new IndexOutOfRangeException("Target column not within range of the line.");

            //flatten the file to get the appropriate cursor index
            //sum up the lengths of all preceding lines (plus the newline strings)
            //then add that to the column number
            int finalCursorIndex = 0;
            for (int index = 0; index < line; index++) 
            {
                finalCursorIndex += lines[index].Length + NewLine.Length;
            }
            finalCursorIndex += column;

            CursorIndex = finalCursorIndex;
        }
        public void MoveCursorToBeginning() 
        {
            CursorIndex = 0;
        }
        public void MoveCursorToEnd()
        {
            CursorIndex = Text.Length;
        }
        /// <summary>
        /// Removes a line completely.
        /// Note that a file must contain at least one line; 
        /// removing the last line of the file will still leave the file with one empty line.
        /// </summary>
        /// <param name="line"></param>
        public void RemoveLine(int line)
        {
            lines.RemoveAt(line);

            if (line == 0)
            {
                if (lines.Count == 0)
                    lines.Add(""); //add empty line
                MoveCursorTo(0, 0);
            }
            else
            {
                MoveCursorTo(line - 1, lines[line - 1].Length);
            }
        }
        /// <summary>
        /// Writes text at current cursor position
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            string currentText = Text;
            int currentIndex = CursorIndex;

            currentText = currentText.Insert(currentIndex, text);
            currentIndex += text.Length;

            Text = currentText;
            CursorIndex = currentIndex;
        }
        /// <summary>
        /// Writes new line at current cursor position
        /// </summary>
        public void WriteLine()
        {
            Write(NewLine);
        }
        /// <summary>
        /// Writes text at current cursor position, and adds new line
        /// </summary>
        /// <param name="text"></param>
        public void WriteLine(string text)
        {
            Write(text + NewLine);
        }

        /// <summary>
        /// Gets or sets the cursor position
        /// </summary>
        public int CursorIndex
        {
            get
            {
                return cursorIndex;
            }
            set
            {
                if (value < 0 || value > Text.Length)
                    throw new IndexOutOfRangeException("Target index not within range of the file.");

                cursorIndex = value;
            } 
        }
        /// <summary>
        /// Gets the lines of this file
        /// </summary>
        public string[] Lines 
        {
            get 
            {
                return lines.ToArray(); 
            }
        }
        /// <summary>
        /// Gets or sets the text of this file
        /// </summary>
        public string Text
        {
            get 
            {
                return string.Join(NewLine, lines);
            }
            set 
            {
                lines = value.Split('\r', '\n').ToList();
                //MoveCursorToEnd();
                MoveCursorToBeginning();
            } 
        }
    }

    public abstract class SectionedTextFile : TextFile
    {
        public SectionedTextFile(string name)
            : base(name)
        { }

        public TextFile CurrentSection { get; internal set; }
    }

    public class HtmlOutputFile : SectionedTextFile 
    {
        TextFile head;

        public HtmlOutputFile(string name)
            : base(name)
        {
            head = new TextFile("head");
        }

        public override void Commit(bool canOverride)
        {
            Text =
                string.Join(NewLine,
                "<!DOCTYPE html>", //defaults to HTML5
                "<html>",
                "<head>", 
                HeadSection.Text,
                "</head>",
                "<body>",
                Text, 
                "</body>", 
                "</html>");

            base.Commit(canOverride);
        }

        public TextFile HeadSection { get { return head; } }
    }

    public class CssOutputFile : SectionedTextFile
    {
        public CssOutputFile(string name) : base(name) { }
    }

    public class JsOutputFile : SectionedTextFile
    {
        public JsOutputFile(string name) : base(name) { }
    }
}
