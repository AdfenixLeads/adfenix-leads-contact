namespace Agent.Infrastructure.Exceptions
{
    using System;

    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class AgentDomainException : Exception
    {
        public AgentDomainException()
        { }

        public AgentDomainException(string message)
            : base(message)
        { }

        public AgentDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
