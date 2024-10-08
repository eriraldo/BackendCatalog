using System;
using System.Collections;
using System.Collections.Generic;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Data.Common;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Catalog.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ItemsController: ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController( IItemsRepository repository){
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);

            if(item == null){
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            repository.CreateItem(item);
        return CreatedAtAction(nameof(GetItem),new{id = item.Id}, item.AsDto());

        }

        [HttpPut("{id}")]

        public ActionResult UpdateItem(Guid id, UpdateItemDto ItemDto)
        {
            var existingItem = repository.GetItem(id);

            if(existingItem is null){
                return NotFound();
            }

            Item updatedItem = existingItem with {
                Name = ItemDto.Name,
                Price = ItemDto.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);

            if(existingItem is null){
                return NotFound();
            }
            
            repository.DeleteItem(id);
            return NoContent();
        }
    }
}