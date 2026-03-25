using CookBook.Clean.Application;
using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Core;
using CookBook.Clean.Core.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Clean.WebApi.Controllers;

[ApiController]
[Route("ingredient")]
public class IngredientController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<IngredientController> _logger;

    public IngredientController(IMediator mediator, ILogger<IngredientController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost(Name = "CreateIngredient")]
    public async Task<ActionResult<Guid>> Create(IngredientCreateDto dtoOut)
    {
        var result = await _mediator.Send(new CreateIngredientCommand(dtoOut));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
        
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IngredientGetDetailDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetIngredientDetailQuery(id));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
    
    [HttpGet(Name = "GetList")]
    public async Task<ActionResult<IEnumerable<IngredientGetListDto>>> GetList(
        [FromQuery] IngredientFilter filter,
        [FromQuery] PagingOptions paging)
    {
        var result = await _mediator.Send(new GetIngredientListQuery(filter, paging));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpPut(Name = "UpdateIngredient")]
    public async Task<ActionResult<Guid>> Update(IngredientUpdateDto dtoOut)
    {
        var result = await _mediator.Send(new UpdateIngredientCommand(dtoOut));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _mediator.Send(new DeleteIngredientCommand(id));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }
}
