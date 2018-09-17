namespace Contact.Commands
{
    using MediatR;
    using System;

    public class CreateGroup : Command, IRequest<bool>
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }
}
