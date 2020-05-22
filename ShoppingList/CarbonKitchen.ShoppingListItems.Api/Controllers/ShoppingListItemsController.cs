namespace CarbonKitchen.ShoppingListItems.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/shoppinglistitems")]
    public class ShoppingListItemsController : Controller
    {
        private readonly IShoppingListItemRepository _shoppingListItemRepository;
        private readonly IMapper _mapper;

        public ShoppingListItemsController(IShoppingListItemRepository shoppingListItemRepository
            , IMapper mapper)
        {
            _shoppingListItemRepository = shoppingListItemRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet(Name = "GetShoppingListItems")]
        public ActionResult<IEnumerable<ShoppingListItemDto>> GetCategories([FromQuery] ShoppingListItemParametersDto shoppingListItemParametersDto)
        {
            var shoppingListItemsFromRepo = _shoppingListItemRepository.GetShoppingListItems(shoppingListItemParametersDto);
            
            var previousPageLink = shoppingListItemsFromRepo.HasPrevious
                    ? CreateShoppingListItemsResourceUri(shoppingListItemParametersDto,
                        ResourceUriType.PreviousPage)
                    : null;

            var nextPageLink = shoppingListItemsFromRepo.HasNext
                ? CreateShoppingListItemsResourceUri(shoppingListItemParametersDto,
                    ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = shoppingListItemsFromRepo.TotalCount,
                pageSize = shoppingListItemsFromRepo.PageSize,
                pageNumber = shoppingListItemsFromRepo.PageNumber,
                totalPages = shoppingListItemsFromRepo.TotalPages,
                hasPrevious = shoppingListItemsFromRepo.HasPrevious,
                hasNext = shoppingListItemsFromRepo.HasNext,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var shoppingListItemsDto = _mapper.Map<IEnumerable<ShoppingListItemDto>>(shoppingListItemsFromRepo);
            return Ok(shoppingListItemsDto);
        }


        [HttpGet("{shoppingListItemId}", Name = "GetShoppingListItem")]
        public ActionResult<ShoppingListItemDto> GetShoppingListItem(int shoppingListItemId)
        {
            var shoppingListItemFromRepo = _shoppingListItemRepository.GetShoppingListItem(shoppingListItemId);

            if (shoppingListItemFromRepo == null)
            {
                return NotFound();
            }

            var shoppingListItemDto = _mapper.Map<ShoppingListItemDto>(shoppingListItemFromRepo);

            return Ok(shoppingListItemDto);
        }

        [HttpPost]
        public ActionResult<ShoppingListItemDto> AddShoppingListItem(ShoppingListItemForCreationDto shoppingListItemForCreation)
        {
            var shoppingListItem = _mapper.Map<ShoppingListItem>(shoppingListItemForCreation);
            _shoppingListItemRepository.AddShoppingListItem(shoppingListItem);
            _shoppingListItemRepository.Save();

            var shoppingListItemDto = _mapper.Map<ShoppingListItemDto>(shoppingListItem);
            return CreatedAtRoute("GetShoppingListItem",
                new { shoppingListItemDto.ShoppingListItemId },
                shoppingListItemDto);
        }

        [HttpDelete("{shoppingListItemId}")]
        public ActionResult DeleteShoppingListItem(int shoppingListItemId)
        {
            var shoppingListItemFromRepo = _shoppingListItemRepository.GetShoppingListItem(shoppingListItemId);

            if (shoppingListItemFromRepo == null)
            {
                return NotFound();
            }

            _shoppingListItemRepository.DeleteShoppingListItem(shoppingListItemFromRepo);
            _shoppingListItemRepository.Save();

            return NoContent();
        }

        [HttpPut("{shoppingListItemId}")]
        public IActionResult UpdateShoppingListItem(int shoppingListItemId, ShoppingListItemForUpdateDto shoppingListItem)
        {
            var shoppingListItemFromRepo = _shoppingListItemRepository.GetShoppingListItem(shoppingListItemId);

            if (shoppingListItemFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(shoppingListItem, shoppingListItemFromRepo);
            _shoppingListItemRepository.UpdateShoppingListItem(shoppingListItemFromRepo);

            _shoppingListItemRepository.Save();

            return NoContent();
        }

        [HttpPatch("{shoppingListItemId}")]
        public IActionResult PartiallyUpdateShoppingListItem(int shoppingListItemId, JsonPatchDocument<ShoppingListItemForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingShoppingListItem = _shoppingListItemRepository.GetShoppingListItem(shoppingListItemId);

            if (existingShoppingListItem == null)
            {
                return NotFound();
            }

            var shoppingListItemToPatch = _mapper.Map<ShoppingListItemForUpdateDto>(existingShoppingListItem); // map the shoppingListItem we got from the database to an updatable shoppingListItem model
            patchDoc.ApplyTo(shoppingListItemToPatch, ModelState); // apply patchdoc updates to the updatable shoppingListItem

            if (!TryValidateModel(shoppingListItemToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(shoppingListItemToPatch, existingShoppingListItem); // apply updates from the updatable shoppingListItem to the db entity so we can apply the updates to the database
            _shoppingListItemRepository.UpdateShoppingListItem(existingShoppingListItem); // apply business updates to data if needed

            _shoppingListItemRepository.Save(); // save changes in the database

            return NoContent();
        }

        private string CreateShoppingListItemsResourceUri(
            ShoppingListItemParametersDto shoppingListItemParametersDto,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetShoppingListItems",
                        new
                        {
                            filters = shoppingListItemParametersDto.Filters,
                            orderBy = shoppingListItemParametersDto.SortOrder,
                            pageNumber = shoppingListItemParametersDto.PageNumber - 1,
                            pageSize = shoppingListItemParametersDto.PageSize,
                            searchQuery = shoppingListItemParametersDto.QueryString
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetShoppingListItems",
                        new
                        {
                            filters = shoppingListItemParametersDto.Filters,
                            orderBy = shoppingListItemParametersDto.SortOrder,
                            pageNumber = shoppingListItemParametersDto.PageNumber + 1,
                            pageSize = shoppingListItemParametersDto.PageSize,
                            searchQuery = shoppingListItemParametersDto.QueryString
                        });

                default:
                    return Url.Link("GetShoppingListItems",
                        new
                        {
                            filters = shoppingListItemParametersDto.Filters,
                            orderBy = shoppingListItemParametersDto.SortOrder,
                            pageNumber = shoppingListItemParametersDto.PageNumber,
                            pageSize = shoppingListItemParametersDto.PageSize,
                            searchQuery = shoppingListItemParametersDto.QueryString
                        });
            }
        }
    }
}