using CookBook.Clean.UseCases;
using CookBook.Clean.UseCases.Ingredient.Create;
using CookBook.Clean.UseCases.Ingredient.Delete;
using CookBook.Clean.UseCases.Ingredient.Get;
using CookBook.Clean.UseCases.Ingredient.GetList;
using CookBook.Clean.UseCases.Ingredient.Update;
using CookBook.Clean.WebApi.DTOs;
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
    public async Task<ActionResult<Guid>> Create(IngredientCreateRequestDto requestDto)
    {
        var result = await _mediator.Send(new CreateIngredientUseCase(requestDto.Name, requestDto.Descripton, requestDto.ImageUrl));
        return Ok(result);
    }
        
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetIngredientUseCase(id));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
    
    [HttpGet(Name = "GetList")]
    public async Task<ActionResult> GetList()
    {
        var result = await _mediator.Send(new GetListIngredientUseCase());
        return Ok(result);
    }
    
    [HttpPut(Name = "UpdateIngredient")]
    public async Task<ActionResult<Guid>> Update(IngredientUpdateRequestDto requestDto)
    {
        var result = await _mediator.Send(new UpdateIngredientUseCase(requestDto.Id, requestDto.Name, requestDto.Descripton, requestDto.ImageUrl));
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _mediator.Send(new DeleteIngredientUseCase(id));
        if (result.Success)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }
}
