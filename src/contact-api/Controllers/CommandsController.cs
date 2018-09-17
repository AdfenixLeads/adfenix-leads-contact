using Contact.Commands;
using Contact.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Contact.API.Controllers
{
    [Route("api/v1/[controller]")]
    //[Authorize]
    public class CommandsController : ControllerBase
    {
        private readonly IIdentityService identityService;
        private readonly IMediator mediator;

        public CommandsController(IMediator mediator, 
            IIdentityService identityService)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
         
        //POST api/v1/[controller]/create
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateContact(CreateContactCommand createContactCommand)
        {
            createContactCommand.OwnerId = identityService.GetUserIdentity();
            var result = await mediator.Send(createContactCommand);

            return result ? 
                (IActionResult)Ok() : 
                (IActionResult)BadRequest();
        }

        //POST api/v1/[controller]/update
        [Route("update")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateContact(UpdateContactCommand updateContactCommand)
        {
            updateContactCommand.OwnerId = identityService.GetUserIdentity();
            var result = await mediator.Send(updateContactCommand);

            return result ?
                (IActionResult) Ok() :
                (IActionResult) BadRequest();
        }
    }
}
