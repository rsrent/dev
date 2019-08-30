namespace Rent.ContextPoint.Exceptions
{
    public class NothingUpdatedException : System.Exception
    {
        public NothingUpdatedException() : base() { }
        public NothingUpdatedException(string message) : base(message) { }
        public NothingUpdatedException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected NothingUpdatedException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}