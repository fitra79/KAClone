using Api.Controllers;
using Application.Common.Extensions;
using Application.Common.Vms;
using Application.ToDoItems.Commands.Create;
using Application.ToDoItems.Commands.Delete;
using Application.ToDoItems.Commands.Update;
using Application.ToDoItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    public class ToDoItemController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<DocumentRootJson<List<ToDoItemVm>>>> GetDivisions(CancellationToken cancellationToken)
        {
            var query = new GetToDoItemQueries();
            return await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateToDoItem([FromBody] CreateToDoItemCommand command,
        CancellationToken cancellationToken)
        {
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> UpdateToDoItem(
        [BindRequired] string id,
        [FromBody] UpdateToDoItemCommand command,
        CancellationToken cancellationToken)
        {
            command.Id = id;
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> DeleteToDoItem(
        [BindRequired] string id,
        CancellationToken cancellationToken)
        {
            return await Mediator.Send(new DeleteToDoItemCommand { Id = id }, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}