using CookBook.Clean.Application;
using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Core;
using CookBook.Clean.Core.Shared.ValueObjects;
using CookBook.Clean.WebApi.DTOs.Ingredient;
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
    public async Task<ActionResult<Guid>> Create(IngredientCreateDtoOut dtoOut)
    {
        var ingredientCreateDto = new IngredientCreateDto
        {
            Name = dtoOut.Name,
            Description = dtoOut.Description,
            ImageUrl = dtoOut.ImageUrl != null
                ? ImageUrl.CreateObject(dtoOut.ImageUrl).Value
                : null
        };
        
        var result = await _mediator.Send(new CreateIngredientCommand(ingredientCreateDto));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
        
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IngredientGetDetailDtoOut>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetIngredientDetailQuery(id));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
    
    [HttpGet(Name = "GetList")]
    public async Task<ActionResult<IEnumerable<IngredientGetListDtoOut>>> GetList(
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
    public async Task<ActionResult<Guid>> Update(IngredientUpdateDtoOut dtoOut)
    {
        Result<ImageUrl>? urlObjectResult = null;
        if (dtoOut.ImageUrl is not null)
        {
            urlObjectResult = ImageUrl.CreateObject(dtoOut.ImageUrl);
            if (urlObjectResult.IsFailure)
                return BadRequest(urlObjectResult.Error);
        }
        
        var ingredientUpdateDto = new IngredientUpdateDto
        {
            Id = dtoOut.Id,
            Name = dtoOut.Name,
            Description = dtoOut.Description,
            ImageUrl = urlObjectResult?.Value
        };
        
        var result = await _mediator.Send(new UpdateIngredientCommand(ingredientUpdateDto));
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
