using System;
namespace Rent.ContextPoint.Exceptions
{
    [Serializable()]
    public class BadRequestException : System.Exception
    {
        public BadRequestException() : base() { }
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected BadRequestException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
