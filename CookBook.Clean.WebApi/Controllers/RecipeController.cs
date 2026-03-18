using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.RecipeRoot.AddIngredient;
using CookBook.Clean.UseCases.RecipeRoot.Create;
using CookBook.Clean.UseCases.RecipeRoot.Delete;
using CookBook.Clean.UseCases.RecipeRoot.Get;
using CookBook.Clean.UseCases.RecipeRoot.GetList;
using CookBook.Clean.UseCases.RecipeRoot.IngredientUpdate;
using CookBook.Clean.UseCases.RecipeRoot.RemoveIngredient;
using CookBook.Clean.UseCases.RecipeRoot.Update;
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
        var result = await _mediator.Send(new CreateRecipeUseCase(requestDto.Name, requestDto.Description, requestDto.ImageUrl, requestDto.Duration, requestDto.Type));
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeDetailModel>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRecipeQuery(id));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet(Name = "GetRecipeList")]
    public async Task<ActionResult<IEnumerable<RecipeListModel>>> GetList()
    {
        var result = await _mediator.Send(new GetListRecipeQuery());
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpPut(Name = "UpdateRecipe")]
    public async Task<ActionResult<Guid>> Update(RecipeUpdateRequestDto requestDto)
    {
        var result = await _mediator.Send(new UpdateRecipeUseCase(requestDto.Id, requestDto.Name, requestDto.Description, requestDto.ImageUrl, requestDto.Duration, requestDto.Type));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
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

    [HttpPost("{recipeId:guid}/ingredient", Name = "AddIngredientToRecipe")]
    public async Task<ActionResult<Guid>> AddIngredient(Guid recipeId, RecipeAddIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new AddIngredientToRecipeUseCase(recipeId, requestDto.IngredientId, requestDto.Amount, requestDto.Unit));
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{recipeId:guid}/ingredient", Name = "RemoveIngredientFromRecipe")]
    public async Task<ActionResult> RemoveIngredient(Guid recipeId, RecipeRemoveIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new RemoveIngredientFromRecipeUseCase(recipeId, requestDto.EntryId));
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result.Error);
    }
    
    [HttpPut("{recipeId:guid}/ingredient", Name = "UpdateIngredientInRecipe")]
    public async Task<ActionResult> Update(Guid recipeId, RecipeUpdateIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new UpdateIngredientInRecipeUseCase(recipeId, requestDto.EntryId, requestDto.NewAmount, requestDto.NewUnit));
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }
}
