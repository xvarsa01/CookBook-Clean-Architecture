using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.CleanArch.Presentation.WebApi.Controllers;

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
    public async Task<ActionResult<RecipeId>> Create(RecipeCreateRequest requestOut)
    {
        var result = await _mediator.Send(new CreateRecipeCommand(requestOut));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeGetDetailResponse>> GetById(RecipeId id)
    {
        var result = await _mediator.Send(new GetRecipeDetailQuery(id));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet(Name = "GetRecipeList")]
    public async Task<ActionResult<IEnumerable<RecipeGetListResponse>>> GetList(
        [FromQuery] RecipeFilter filter,
        [FromQuery] PagingOptions paging)
    {
        var result = await _mediator.Send(new GetRecipeListQuery(filter, paging));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
    
    [HttpGet("ingredient/{ingredientId}", Name = "GetRecipeListByIngredientId")]
    public async Task<ActionResult<IEnumerable<RecipeGetListResponse>>> GetListByIngredient(
        [FromRoute] IngredientId ingredientId)
    {
        var result = await _mediator.Send(new GetRecipeListByContainingIngredientIdQuery(ingredientId));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
    
    [HttpGet("ingredient", Name = "GetRecipeListByIngredientName")]
    public async Task<ActionResult<IEnumerable<RecipeGetListResponse>>> GetListByIngredientName(
        [FromQuery] string ingredientNameSubstring)
    {
        var result = await _mediator.Send(new GetRecipeListByContainingIngredientNameQuery(ingredientNameSubstring));
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPut(Name = "UpdateRecipe")]
    public async Task<ActionResult<RecipeId>> Update(RecipeUpdateRequest requestOut)
    {
        
        var result = await _mediator.Send(new UpdateRecipeCommand(requestOut));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(
        [FromRoute] RecipeId id)
    {
        var result = await _mediator.Send(new DeleteRecipeCommand(id));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }

    [HttpPost("{recipeId}/ingredient", Name = "AddIngredientToRecipe")]
    public async Task<ActionResult<Guid>> AddIngredient(
        [FromRoute] RecipeId recipeId,
        RecipeAddIngredientRequest request)
    {
        var result = await _mediator.Send(new AddIngredientToRecipeCommand(recipeId, request));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{recipeId}/ingredient/{ingredientEntryId}", Name = "RemoveIngredientFromRecipe")]
    public async Task<ActionResult> RemoveIngredient(
        [FromRoute] RecipeId recipeId,
        IngredientInRecipeId ingredientEntryId)
    {
        var result = await _mediator.Send(new RemoveIngredientFromRecipeCommand(recipeId, ingredientEntryId));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(result.Error);
    }
    
    [HttpPut("{recipeId}/ingredient", Name = "UpdateIngredientInRecipe")]
    public async Task<ActionResult> Update(
        [FromRoute] RecipeId recipeId,
        RecipeUpdateIngredientRequest request)
    {
        var result = await _mediator.Send(new UpdateIngredientInRecipeCommand(recipeId, request));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }
}
