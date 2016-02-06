using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic.Messaging
{
    /// <summary>
    /// To be implemented by objects that can be passed as notifications
    /// </summary>
    public interface INotification
    {
        string Message { get; }
        MessageType MessageType { get; }
        string FilePath { get; }
        int Line { get; }
        int Column { get; }
        bool HasLineInfo { get; }
    }
}
