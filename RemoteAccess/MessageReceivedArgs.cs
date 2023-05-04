using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteAccess
{
    public class MessageReceivedArgs : EventArgs
    {
        private RemoteAccessMessage _message;

        public MessageReceivedArgs(RemoteAccessMessage message)
        {
            _message = message;
        }

        public RemoteAccessMessage Message
        {
            get
            {
                return _message;
            }
        }
    }
}
