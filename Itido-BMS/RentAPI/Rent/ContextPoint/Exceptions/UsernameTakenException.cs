using System;

namespace Rent.ContextPoint.Exceptions
{
    [Serializable()]
    public class UsernameTakenException : System.Exception
    {
        public UsernameTakenException() : base() { }
        public UsernameTakenException(string message) : base(message) { }
        public UsernameTakenException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected UsernameTakenException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}