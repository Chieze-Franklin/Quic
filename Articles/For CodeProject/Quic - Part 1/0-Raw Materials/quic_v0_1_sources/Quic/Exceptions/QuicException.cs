using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic.Messaging;

namespace Quic
{
    public class QuicException : Exception, INotification
    {
        MessageType msgType = MessageType.Error;

        public QuicException(string message, string filePath)
            : base(message)
        {
            FilePath = filePath;
        }
        public QuicException(string message, string filePath, Exception inner)
            : base(message, inner)
        {
            FilePath = filePath;
        }
        public QuicException(string message, string filePath, int line, int column)
            : base(message)
        {
            FilePath = filePath;
            Line = line;
            Column = column;
            HasLineInfo = true;
        }
        public QuicException(string message, string filePath, int line, int column, Exception inner)
            : base(message, inner)
        {
            FilePath = filePath;
            Line = line;
            Column = column;
            HasLineInfo = true;
        }

        public MessageType MessageType
        {
            get { return msgType; }
            set { msgType = value; }
        }
        public string FilePath { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }
        public bool HasLineInfo { get; private set; }
    }
}
