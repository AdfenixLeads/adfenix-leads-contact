namespace Contact.API.Controllers
{
    using Contact.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Projection;
    using Contact.Projection.Query;
    using LeadsPlus.Core.Query;
    using Contact.Domain.Query;
    using System.Collections.Generic;

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class QueriesController : ControllerBase
    {
        private readonly IQueryExecutor queryExecutor;
        private readonly IIdentityService identityService;

        public QueriesController(IQueryExecutor queryExecutor, IIdentityService identityService)
        {
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [Route("getbyid")]
        [HttpGet]
        public async Task<IActionResult> GetContact(GetContactQuery query)
        {
            var contact = await queryExecutor.Execute<GetContactQuery, Contact>(query);

            return Ok(contact);
        }

        [Route("getall")]
        [HttpGet]
        //[ProducesResponseType(typeof(List<Agent>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await queryExecutor.Execute<GetAllContactQuery, List<Contact>>(new GetAllContactQuery());
            return Ok(contacts);
        }
    }
}
