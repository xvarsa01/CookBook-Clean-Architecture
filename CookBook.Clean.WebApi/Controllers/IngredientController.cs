using CookBook.Clean.Application;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.IngredientRoot.Create;
using CookBook.Clean.Application.IngredientRoot.Delete;
using CookBook.Clean.Application.IngredientRoot.Get;
using CookBook.Clean.Application.IngredientRoot.GetList;
using CookBook.Clean.Application.IngredientRoot.Update;
using CookBook.Clean.Application.Models;
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
        var result = await _mediator.Send(new CreateIngredientUseCase(requestDto.Name, requestDto.Description, requestDto.ImageUrl));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
        
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IngredientDetailModel>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetIngredientQuery(id));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
    
    [HttpGet(Name = "GetList")]
    public async Task<ActionResult<IEnumerable<IngredientListModel>>> GetList(
        [FromQuery] IngredientFilter filter,
        [FromQuery] PagingOptions paging)
    {
        var result = await _mediator.Send(new GetListIngredientQuery(filter, paging));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
    
    [HttpPut(Name = "UpdateIngredient")]
    public async Task<ActionResult<Guid>> Update(IngredientUpdateRequestDto requestDto)
    {
        var result = await _mediator.Send(new UpdateIngredientUseCase(requestDto.Id, requestDto.Name, requestDto.Description, requestDto.ImageUrl));
        if (result.Success)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
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
