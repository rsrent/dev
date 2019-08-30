using System;
namespace Rent.ContextPoint.Exceptions
{
    public class ProtectedObjectException : System.Exception
    {
        public ProtectedObjectException() : base() { }
        public ProtectedObjectException(string message) : base(message) { }
        public ProtectedObjectException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ProtectedObjectException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
