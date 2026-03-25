using CookBook.Clean.Application;
using CookBook.Clean.Application.Commands.Recipes;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Application.Queries.Recipes;
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
    public async Task<ActionResult<Guid>> Create(RecipeCreateDto dtoOut)
    {
        var result = await _mediator.Send(new CreateRecipeCommand(dtoOut));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeGetDetailDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRecipeDetailQuery(id));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet(Name = "GetRecipeList")]
    public async Task<ActionResult<IEnumerable<RecipeGetListDto>>> GetList(
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
    
    [HttpGet("ingredient/{ingredientId:guid}", Name = "GetRecipeListByIngredientId")]
    public async Task<ActionResult<IEnumerable<RecipeGetListDto>>> GetListByIngredient(Guid ingredientId)
    {
        var result = await _mediator.Send(new GetRecipeListByContainingIngredientIdQuery(ingredientId));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
    
    [HttpGet("ingredient/{ingredientNameSubstring}", Name = "GetRecipeListByIngredientName")]
    public async Task<ActionResult<IEnumerable<RecipeGetListDto>>> GetListByIngredientName(string ingredientNameSubstring)
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
    public async Task<ActionResult<Guid>> Update(RecipeUpdateDto dtoOut)
    {
        
        var result = await _mediator.Send(new UpdateRecipeCommand(dtoOut));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _mediator.Send(new DeleteRecipeCommand(id));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return NotFound(result.Error);
    }

    [HttpPost("{recipeId:guid}/ingredient", Name = "AddIngredientToRecipe")]
    public async Task<ActionResult<Guid>> AddIngredient(Guid recipeId, RecipeAddIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new AddIngredientToRecipeCommand(recipeId, requestDto.IngredientId, requestDto.Amount, requestDto.Unit));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{recipeId:guid}/ingredient", Name = "RemoveIngredientFromRecipe")]
    public async Task<ActionResult> RemoveIngredient(Guid recipeId, RecipeRemoveIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new RemoveIngredientFromRecipeCommand(recipeId, requestDto.EntryId));
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(result.Error);
    }
    
    [HttpPut("{recipeId:guid}/ingredient", Name = "UpdateIngredientInRecipe")]
    public async Task<ActionResult> Update(Guid recipeId, RecipeUpdateIngredientRequestDto requestDto)
    {
        var result = await _mediator.Send(new UpdateIngredientInRecipeCommand(recipeId, requestDto.EntryId, requestDto.NewAmount, requestDto.NewUnit));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }
}
