using CookBook.Clean.Application;
using CookBook.Clean.Application.Commands.Recipes;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Application.Queries.Recipes;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;
using CookBook.Clean.WebApi.DTOs;
using CookBook.Clean.WebApi.DTOs.Recipe;
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
        Result<ImageUrl>? urlObjectResult = null;
        if (requestDto.ImageUrl is not null)
        {
            urlObjectResult = ImageUrl.CreateObject(requestDto.ImageUrl);
            if (urlObjectResult.IsFailure)
                return BadRequest(urlObjectResult.Error);
        }

        var nameObjectResult = RecipeName.CreateObject(requestDto.Name);
        if (nameObjectResult.IsFailure)
            return BadRequest(nameObjectResult.Error);

        var durationObjectResult = RecipeDuration.CreateObject(requestDto.Duration);
        if (durationObjectResult.IsFailure)
            return BadRequest(durationObjectResult.Error);
        
        var recipeCreateDto = new RecipeCreateDto
        {
            Name = nameObjectResult.Value,
            Description = requestDto.Description,
            ImageUrl = urlObjectResult?.Value,
            Duration = durationObjectResult.Value,
            Type = requestDto.Type
        };
        
        var result = await _mediator.Send(new CreateRecipeCommand(recipeCreateDto));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeGetDetailDtoOut>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRecipeDetailQuery(id));
        if (result.IsSuccess)
        {
            RecipeGetDetailDtoOut outDto = new ()
            {
                Id = result.Value.Id,
                Name = result.Value.Name.Value,
                Description = result.Value.Description,
                ImageUrl = result.Value.ImageUrl?.Value,
                Duration = result.Value.Duration.Value,
                Type = result.Value.Type,
                Ingredients = result.Value.Ingredients
            };
            return Ok(outDto);
        }
        return NotFound(result.Error);
    }

    [HttpGet(Name = "GetRecipeList")]
    public async Task<ActionResult<IEnumerable<RecipeGetListDtoOut>>> GetList(
        [FromQuery] RecipeFilter filter,
        [FromQuery] PagingOptions paging)
    {
        var result = await _mediator.Send(new GetRecipeListQuery(filter, paging));
        if (result.IsSuccess)
        {
            List<RecipeGetListDtoOut> dtoOut = result.Value
                .Select(x => new RecipeGetListDtoOut
                {
                    Id = x.Id,
                    Name = x.Name.Value,
                    ImageUrl = x.ImageUrl?.Value
                })
                .ToList();

            return Ok(dtoOut);
        }
        return BadRequest(result.Error);
    }
    
    [HttpGet("ingredient/{ingredientId:guid}", Name = "GetRecipeListByIngredientId")]
    public async Task<ActionResult<IEnumerable<RecipeListModel>>> GetListByIngredient(Guid ingredientId)
    {
        var result = await _mediator.Send(new GetRecipeListByContainingIngredientIdQuery(ingredientId));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpGet("ingredient/{ingredientNameSubstring}", Name = "GetRecipeListByIngredientName")]
    public async Task<ActionResult<IEnumerable<RecipeListModel>>> GetListByIngredientName(string ingredientNameSubstring)
    {
        var result = await _mediator.Send(new GetRecipeListByContainingIngredientNameQuery(ingredientNameSubstring));
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpPut(Name = "UpdateRecipe")]
    public async Task<ActionResult<Guid>> Update(RecipeUpdateRequestDto requestDto)
    {
        Result<ImageUrl>? urlObjectResult = null;
        if (requestDto.ImageUrl is not null)
        {
            urlObjectResult = ImageUrl.CreateObject(requestDto.ImageUrl);
            if (urlObjectResult.IsFailure)
                return BadRequest(urlObjectResult.Error);
        }
        Result<RecipeName>? nameObjectResult = null;
        if (requestDto.Name is not null)
        {
            nameObjectResult = RecipeName.CreateObject(requestDto.Name);
            if (nameObjectResult.IsFailure)
                return BadRequest(nameObjectResult.Error);
        }
        Result<RecipeDuration>? durationObjectResult = null;
        if (requestDto.Duration is not null)
        {
            durationObjectResult = RecipeDuration.CreateObject(requestDto.Duration.Value);
            if (durationObjectResult.IsFailure)
                return BadRequest(durationObjectResult.Error);
        }
        
        var recipeUpdateDto = new RecipeUpdateDto
        {
            Id = requestDto.Id,
            Name = nameObjectResult?.Value,
            Description = requestDto.Description,
            ImageUrl = urlObjectResult?.Value,
            Duration = durationObjectResult?.Value,
            Type = requestDto.Type
        };
        
        var result = await _mediator.Send(new UpdateRecipeCommand(recipeUpdateDto));
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
