using System;

namespace Rent.ContextPoint.Exceptions
{
    [Serializable()]
    public class UsernameTooShortException : System.Exception
    {
        public UsernameTooShortException() : base() { }
        public UsernameTooShortException(string message) : base(message) { }
        public UsernameTooShortException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected UsernameTooShortException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}