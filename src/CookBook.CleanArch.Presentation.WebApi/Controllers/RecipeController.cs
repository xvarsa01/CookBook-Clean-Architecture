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

    public RecipeController(IMediator mediator)
    {
        _mediator = mediator;
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
    public async Task<ActionResult<PagedResult<RecipeListResponse>>> GetList(
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
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
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
    
    [HttpPut(Name = "UpdateRecipe")]
    public async Task<ActionResult<RecipeId>> Update(RecipeUpdateWithIngredientsRequest requestOut)
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

    [HttpPost("{id:guid}/review", Name = "AddReviewToRecipe")]
    public async Task<ActionResult<RecipeReviewId>> AddReview(Guid id, RecipeAddReviewRequest request)
    {
        var recipeId = new RecipeId(id);
        var result = await _mediator.Send(new AddReviewToRecipeCommand(recipeId, request));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}/review/{reviewId:guid}", Name = "RemoveReviewFromRecipe")]
    public async Task<ActionResult> RemoveReview(Guid id, Guid reviewId)
    {
        var recipeId = new RecipeId(id);
        var recipeReviewId = new RecipeReviewId(reviewId);

        var result = await _mediator.Send(new RemoveReviewFromRecipeCommand(recipeId, recipeReviewId));
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }
}
