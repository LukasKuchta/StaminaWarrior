using Backend.Application.Abstractions.Commands;
using Backend.Application.Warriors.CreateWarrior;
using Backend.Infrastructure;
using Backend.Infrastructure.Messaging.InternalCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.Warriors;

[ApiController]
[Route("api/warriors")]
public class WarriorsController : ControllerBase
{
    private readonly ICommandScheduler _commandScheduler;
    private readonly ICommandExecutor _commandExecutor;
    private readonly ApplicationDbContext _context;
    private readonly ISender _sender;
    private readonly IServiceProvider _prov;

    public WarriorsController(
        ICommandScheduler commandScheduler,
        ICommandExecutor commandExecutor,
        ISender sender, IServiceProvider prov, ApplicationDbContext context)
    {
        _commandScheduler = commandScheduler;
        _commandExecutor = commandExecutor;
        _sender = sender;
        _prov = prov;
        _context = context;
    }

    [HttpGet("{id}")]
    public IActionResult GetWarrior(Guid id, CancellationToken cancellationToken)
    {
        //GetWarriorQuery query = new GetWarriorQuery(warriorId);
        //var result = await _sender.Send(query, cancellationToken);

        //return result.IsSuccess ? Ok(result.Value) : NotFound);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetWarriors(GetWarriorsRequest request, CancellationToken cancellationToken)
    {
        //GetWarriorQuery query = new GetWarriorQuery(warriorId);
        //var result = await _sender.Send(query, cancellationToken);

        //return result.IsSuccess ? Ok(result.Value) : NotFound);
        return Ok();
    }

    /*
     * Do not expose command directly. It is part of internal API. It is valid only inside the scope of this application.
     * Dont coupling end point with command.
     * This is leaking information about internal API.
     * Command can contains more information usable inside application.
     */
    [HttpPost]
    public async Task<ActionResult> CreateWarrior(CreateWarriorRequest request, CancellationToken cancellationToken)
    {

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        for (var i = 0; i < 1; i++)
        {

            var x = new CreateWarriorCommand(request.UserId, request.WarriorName);
            _commandScheduler.Schedule<Guid>(x);
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        // REST API convention
        // created 201 contains header location where to get created item and its id            
        //return CreatedAtAction(nameof(GetWarrior), new { Id = result }, result);
        return Ok();
    }
}
