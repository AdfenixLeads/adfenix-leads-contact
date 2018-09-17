namespace Contact.Domain.Exceptions
{
    using System;

    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class ContactDomainException : Exception
    { 
        public ContactDomainException()
        { }

        public ContactDomainException(string message)
            : base(message)
        { }

        public ContactDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
