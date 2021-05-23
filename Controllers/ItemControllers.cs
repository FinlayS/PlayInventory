using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private readonly IRepository<InventoryItem> itemsRepository;

    public ItemsController(IRepository<InventoryItem> itemsRepository)
    {
      this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid UserId)
    {
      if (UserId == Guid.Empty)
      {
        return BadRequest();
      }

      var items = (await itemsRepository.GetAllAsync(item => item.UserId == UserId))
                  .Select(items => items.AsDto());

      return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsyc(GrantItemsDto grantItemsDto)
    {
      var InventoryItem = await itemsRepository.GetAsync(
        item => item.UserId == grantItemsDto.UserId && item.CatalogItemId == grantItemsDto.CatalogItemId);

      if (InventoryItem == null)
      {
        InventoryItem = new InventoryItem
        {
          CatalogItemId = grantItemsDto.CatalogItemId,
          UserId = grantItemsDto.UserId,
          Quantity = grantItemsDto.Quantity,
          AquiredDate = DateTimeOffset.UtcNow
        };

        await itemsRepository.CreatAsync(InventoryItem);
      }

      else
      {
        InventoryItem.Quantity += grantItemsDto.Quantity;
        await itemsRepository.UpdateAsync(InventoryItem);
      }

      return Ok();
    }
  }
}