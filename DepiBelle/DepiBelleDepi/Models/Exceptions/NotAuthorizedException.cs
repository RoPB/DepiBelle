using System;
namespace DepiBelleDepi.Models
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
        {
        }

        public NotAuthorizedException(string message)
            : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
