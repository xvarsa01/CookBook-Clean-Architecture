using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.Commands.Ingredients;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.CleanArch.Presentation.WebApi.Controllers;

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
    public async Task<ActionResult<Guid>> Create(
        [FromBody] IngredientCreateRequest request)
    {
        Result<IngredientId> result = await _mediator.Send(new CreateIngredientCommand(request));
        if (result.IsSuccess)
        {
            return Ok(result.Value.Value);
        }
        return BadRequest(result.Error);
    }
        
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IngredientDetailResponse>> GetById(Guid id)
    {
        var ingredientId = new IngredientId(id);
        var result = await _mediator.Send(new GetIngredientDetailQuery(ingredientId));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
    
    [HttpGet(Name = "GetList")]
    public async Task<ActionResult<IEnumerable<IngredientListResponse>>> GetList(
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
    public async Task<ActionResult<IngredientId>> Update(IngredientUpdateRequest request)
    {
        var result = await _mediator.Send(new UpdateIngredientCommand(request));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var ingredientId = new IngredientId(id);
        var result = await _mediator.Send(new DeleteIngredientCommand(ingredientId));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }
}
