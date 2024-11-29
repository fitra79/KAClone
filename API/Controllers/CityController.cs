using Api.Controllers;
using Application.Cities.Commands.Create;
using Application.Cities.Queries;
using Application.Common.Extensions;
using Application.Common.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CityController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<DocumentRootJson<List<CityVm>>>> GetCities(CancellationToken cancellationToken)
        {
            var query = new GetCityListQuery();
            return await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateCity([FromBody] CreateCityCommand command, CancellationToken cancellationToken)
        {
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }
    }
}
