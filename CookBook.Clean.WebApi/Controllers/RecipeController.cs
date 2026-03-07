using CookBook.Clean.UseCases;
using CookBook.Clean.UseCases.Recipe.Get;
using CookBook.Clean.UseCases.Recipe.GetList;
using CookBook.Clean.UseCases.Recipe.Update;
using CookBook.Clean.UseCases.Recipe.Delete;
using CookBook.Clean.UseCases.Recipe.AddIngredient;
using CookBook.Clean.UseCases.Recipe.Create;
using CookBook.Clean.UseCases.Recipe.RemoveIngredient;
using CookBook.Clean.WebApi.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Clean.WebApi.Controllers;

[ApiController]
[Route("recipe")]
public class RecipeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RecipeController> _logger;

    public RecipeController(IMediator mediator, ILogger<RecipeController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost(Name = "CreateRecipe")]
    public async Task<ActionResult<Guid>> Create(RecipeCreateRequestDto requestDto)
    {
        var result = await _mediator.Send(new CreateRecipeUseCase(requestDto.Name, requestDto.Description, requestDto.ImageUrl, requestDto.Type));
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRecipeUseCase(id));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet(Name = "GetRecipeList")]
    public async Task<ActionResult> GetList()
    {
        var result = await _mediator.Send(new GetListRecipeUseCase());
        return Ok(result);
    }

    [HttpPut(Name = "UpdateRecipe")]
    public async Task<ActionResult> Update(RecipeUpdateRequestDto requestDto)
    {
        var result = await _mediator.Send(new UpdateRecipeUseCase(requestDto.Id, requestDto.Name, requestDto.Descripton, requestDto.ImageUrl, requestDto.Type));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _mediator.Send(new DeleteRecipeUseCase(id));
        if (result.Success)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }

    [HttpPost("{id:guid}/ingredient", Name = "AddIngredientToRecipe")]
    public async Task<ActionResult<Guid>> AddIngredient(Guid id, RecipeAddIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new AddIngredientToRecipeUseCase(id, requestDto.IngredientId, requestDto.Amount, requestDto.Unit));
        if (result.Success)
        {
            return Ok(result);
        }
        return NotFound(result.Error);
    }

    [HttpDelete("{id:guid}/ingredient", Name = "RemoveIngredientFromRecipe")]
    public async Task<ActionResult> RemoveIngredient(Guid id, RecipeRemoveIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new RemoveIngredientFromRecipeUseCase(id, requestDto.IngredientId));
        if (result.Success)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }
}
