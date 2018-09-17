namespace Contact.Commands.Validations
{
    using Commands;
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(command => command.Email).NotEmpty();
            //RuleFor(command => command.Items).Must(ContainItems).WithMessage("No order items found");
        }

        private bool BeValidExpirationDate(DateTime dateTime)
        {
            return dateTime >= DateTime.UtcNow;
        }

        //private bool ContainItems(IEnumerable<ItemDTO> items)
        //{
        //    return items.Any();
        //}
    }
}
