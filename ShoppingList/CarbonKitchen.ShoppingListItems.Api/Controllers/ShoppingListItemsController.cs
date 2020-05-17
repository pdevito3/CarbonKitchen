namespace CarbonKitchen.ShoppingListItems.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Commands;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Queries;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
    using MediatR;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/shoppinglistitems")]
    public class ShoppingListItemsController : Controller
    {
        private readonly IMediator _mediator;

        public ShoppingListItemsController(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(Name = "GetShoppingListItems")]
        public IActionResult GetShoppingListItems([FromQuery] ShoppingListItemParametersDto shoppingListItemsParametersDto)
        {
            var query = new GetAllShoppingListItemsQuery(shoppingListItemsParametersDto, this);
            var result = _mediator.Send(query);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.Result.PaginationMetadata));

            return Ok(result.Result.ShoppingListItemDtoList);
        }

        [HttpGet("{shoppingListItemsId}", Name = "GetShoppingListItem")]
        public IActionResult GetShoppingListItem(int shoppingListItemsId)
        {
            var query = new GetShoppingListItemQuery(shoppingListItemsId);
            var result = _mediator.Send(query);

            return result.Result != null ? (IActionResult) Ok(result.Result) : NotFound();
        }

        [HttpPost]
        public ActionResult<ShoppingListItemDto> AddShoppingListItem(CreateShoppingListItemCommand command)
        {
            var result = _mediator.Send(command);
            return CreatedAtRoute("GetShoppingListItem",
                new { result.Result.ShoppingListItemId },
                result.Result);
        }

        [HttpDelete("{shoppingListItemsId}")]
        public IActionResult DeleteShoppingListItem(int shoppingListItemsId)
        {
            var query = new DeleteShoppingListItemCommand(shoppingListItemsId);
            var result = _mediator.Send(query);

            return result.Result ? (IActionResult)NoContent() : NotFound();
        }

        [HttpPut("{shoppingListItemsId}")]
        public IActionResult UpdateShoppingListItem(int shoppingListItemsId, ShoppingListItemForUpdateDto shoppingListItems)
        {
            var query = new UpdateEntireShoppingListItemCommand(shoppingListItemsId, shoppingListItems);
            var result = _mediator.Send(query);

            return result.Result.ToUpper() == "NOCONTENT" ? (IActionResult)NoContent() : NotFound();
        }

        [HttpPatch("{shoppingListItemsId}")]
        public IActionResult PartiallyUpdateShoppingListItem(int shoppingListItemsId, JsonPatchDocument<ShoppingListItemForUpdateDto> patchDoc)
        {
            var query = new UpdatePartialShoppingListItemCommand(shoppingListItemsId, patchDoc, this);
            var result = _mediator.Send(query);

            return result.Result;
        }
    }
}