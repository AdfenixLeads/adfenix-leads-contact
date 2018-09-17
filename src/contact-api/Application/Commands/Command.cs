namespace Contact.Commands
{
    using System;

    public abstract class Command
    {
        public string AggregateId { get; set; }
    }
}
