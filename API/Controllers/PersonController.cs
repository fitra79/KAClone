using Api.Controllers;
using Application.Common.Extensions;
using Application.Common.Vms;
using Application.Persons.Commands.Create;
using Application.Persons.Commands.Delete;
using Application.Persons.Commands.Update;
using Application.Persons.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    public class PersonController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<DocumentRootJson<List<PersonVm>>>> GetDivisions(CancellationToken cancellationToken)
        {
            var query = new GetPersonQueries();
            return await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreatePerson([FromBody] CreatePersonCommand command,
        CancellationToken cancellationToken)
        {
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> UpdatePerson(
        [BindRequired] string id,
        [FromBody] UpdatePersonCommand command,
        CancellationToken cancellationToken)
        {
            command.Id = id;
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> DeletePerson(
        [BindRequired] string id,
        CancellationToken cancellationToken)
        {
            return await Mediator.Send(new DeletePersonCommand { Id = id }, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}