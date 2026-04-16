using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Recipes;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
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
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeResponse>> GetById(Guid id)
    {
        var recipeId = new RecipeId(id);
        var result = await _mediator.Send(new GetRecipeDetailQuery(recipeId));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet(Name = "GetRecipeList")]
    public async Task<ActionResult<IEnumerable<RecipeListResponse>>> GetList(
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
    
    [HttpGet("ingredient/{id:guid}", Name = "GetRecipeListByIngredientId")]
    public async Task<ActionResult<IEnumerable<RecipeListResponse>>> GetListByIngredient(Guid id)
    {
        var ingredientId = new IngredientId(id);
        var result = await _mediator.Send(new GetRecipeListByContainingIngredientIdQuery(ingredientId));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
    
    [HttpGet("ingredient", Name = "GetRecipeListByIngredientName")]
    public async Task<ActionResult<IEnumerable<RecipeListResponse>>> GetListByIngredientName(
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

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var recipeId = new RecipeId(id);
        var result = await _mediator.Send(new DeleteRecipeCommand(recipeId));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }

    [HttpPost("{id}/ingredient", Name = "AddIngredientToRecipe")]
    public async Task<ActionResult<Guid>> AddIngredient(Guid id, RecipeAddIngredientRequest request)
    {
        var recipeId = new RecipeId(id);
        var result = await _mediator.Send(new AddIngredientToRecipeCommand(recipeId, request));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}/ingredient/{entryId:guid}", Name = "RemoveIngredientFromRecipe")]
    public async Task<ActionResult> RemoveIngredient(Guid id, Guid entryId)
    {
        var recipeId = new RecipeId(id);
        var ingredientInRecipeId = new RecipeIngredientId(entryId);
        
        var result = await _mediator.Send(new RemoveIngredientFromRecipeByEntryIdCommand(recipeId, ingredientInRecipeId));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(result.Error);
    }
    
    [HttpPut("{id}/ingredient", Name = "UpdateIngredientInRecipe")]
    public async Task<ActionResult> Update(Guid id, RecipeUpdateIngredientRequest request)
    {
        var recipeId = new RecipeId(id);
        var result = await _mediator.Send(new UpdateIngredientInRecipeCommand(recipeId, request));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }
}
