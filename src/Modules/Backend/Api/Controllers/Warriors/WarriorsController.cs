using Backend.Application.Contracts;
using Backend.Application.Warriors.CreateWarrior;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.Warriors;

[ApiController]
[Route("api/warriors")]
public class WarriorsController : ControllerBase
{
    private readonly IBackendModule _backendModule;

    public WarriorsController(IBackendModule backendModule)
    {
        _backendModule = backendModule;
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

        

        var result = await _backendModule.ExecuteCommandAsync(new CreateWarriorCommand(request.UserId, request.WarriorName));                
        return CreatedAtAction(nameof(GetWarrior), new { Id = result }, result);
  

        // REST API convention
        // created 201 contains header location where to get created item and its id            
        //return Ok();
        //return CreatedAtAction(nameof(GetWarrior), new { Id = result }, result);
    }
}
